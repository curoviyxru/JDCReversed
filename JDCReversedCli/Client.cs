using JDCReversed;
using JDCReversed.Packets;

namespace JDCReversedCli;

//TODO mutexes
public class Client
{
    const int AccelBatchCount = 25;
    const int AccelBatchSize = 2;
    const int AccelTotalPackets = AccelBatchCount * AccelBatchSize;
    const int AccelAcqDelta = 1000 / AccelTotalPackets;
    const int AccelAcqLatency = AccelAcqDelta * AccelBatchSize;

    public enum NavigationAction
    {
        SwipeLeft, SwipeRight, SwipeUp, SwipeDown, SelectionConfirm, ActionLeft, ActionRight
    }

    private int _currentRow;
    private int _currentItem;
    private int _currentAction;

    private int _rowCount;
    private int _itemCount;
    private int _actionCount;

    private bool _sendScoringData;
    private int _scoringDataSent;

    private JdPhoneCarouselRow[]? _rows;

    public bool Started { private set; get; } = false;

    private WebSocketConnection? _connection;
    private KeyIntercept _intercept = new();
    private OpenVRHandler _vrHandler = new();

    public Client()
    {

    }

    public void HandleResponse(JdObject? response)
    {
        switch (response)
        {
            case JdPhoneUiSetupData data:
                {
                    _rows = data.SetupData?.MainCarousel?.Rows;
                    if (_rows != null) _rowCount = _rows.Length;

                    if (data.InputSetup?.CarouselPosSetup != null)
                    {
                        _currentRow = data.InputSetup.CarouselPosSetup.RowIndex;
                        var rowItems = _rows?[_currentRow].Items;
                        if (rowItems != null) _itemCount = rowItems.Length;
                        _currentItem = data.InputSetup.CarouselPosSetup.ItemIndex;
                        var actionItems = rowItems?[_currentItem].Actions;
                        if (actionItems != null) _actionCount = actionItems.Length;
                        _currentAction = data.InputSetup.CarouselPosSetup.ActionIndex;
                    }

                    break;
                }
            case JdInputSetupConsoleCommandData data:
                {
                    if (data.CarouselPosSetup != null)
                    {
                        _currentRow = data.CarouselPosSetup.RowIndex;
                        var rowItems = _rows?[_currentRow].Items;
                        if (rowItems != null) _itemCount = rowItems.Length;
                        _currentItem = data.CarouselPosSetup.ItemIndex;
                        var actionItems = rowItems?[_currentItem].Actions;
                        if (actionItems != null) _actionCount = actionItems.Length;
                        _currentAction = data.CarouselPosSetup.ActionIndex;
                    }

                    break;
                }
            case JdEnableAccelValuesSendingConsoleCommandData:
                {
                    _scoringDataSent = 0;
                    _vrHandler.ResetLastVelocity();

                    _sendScoringData = true;
                    break;
                }
            case JdDisableAccelValuesSendingConsoleCommandData:
                {
                    _sendScoringData = false;
                    break;
                }
            default:
                {
                    Console.WriteLine("Got response: " + response?.GetType());
                    break;
                }
        }
    }

    public async Task Navigate(NavigationAction action)
    {
        if (_connection == null)
        {
            return;
        }

        JdObject? request = null;

        switch (action)
        {
            case NavigationAction.ActionLeft:
            case NavigationAction.ActionRight:
                {
                    if (_actionCount == 0)
                        break;

                    int delta = action == NavigationAction.ActionRight ? 1 : -1;

                    _currentAction = (_actionCount + _currentAction + delta) % _actionCount;
                    request = new JdChangeActionPhoneCommandData
                    {
                        RowIndex = _currentRow,
                        ItemIndex = _currentItem,
                        ActionIndex = _currentAction
                    };
                    break;
                }
            case NavigationAction.SwipeUp:
            case NavigationAction.SwipeDown:
                {
                    if (_rowCount == 0)
                        break;

                    int delta = action == NavigationAction.SwipeDown ? 1 : -1;

                    _currentRow = (_rowCount + _currentRow + delta) % _rowCount;
                    _currentItem = 0;
                    _currentAction = 0;
                    request = new JdChangeRowPhoneCommandData
                    {
                        RowIndex = _currentRow
                    };
                    break;
                }
            case NavigationAction.SwipeLeft:
            case NavigationAction.SwipeRight:
                {
                    if (_itemCount == 0)
                        break;

                    int delta = action == NavigationAction.SwipeRight ? 1 : -1;

                    _currentItem = (_itemCount + _currentItem + delta) % _itemCount;
                    _currentAction = 0;
                    request = new JdChangeItemPhoneCommandData
                    {
                        RowIndex = _currentRow,
                        ItemIndex = _currentItem
                    };
                    break;
                }
            case NavigationAction.SelectionConfirm:
                {
                    request = new JdInputPhoneCommandData()
                    {
                        Input = JdInputPhoneCommandData.ActionInput.ACCEPT
                    };
                    break;
                }
        }

        if (request != null)
        {
            await _connection.SendAsync(request);
        }
    }

    public async Task Update()
    {
        if (_connection == null)
        {
            return;
        }

        if (!_sendScoringData)
        {
            return;
        }

        for (int j = 0; j < AccelBatchCount; ++j)
        {
            JdPhoneScoringData scoringData = new()
            {
                Timestamp = _scoringDataSent,
                AccelData = new AccelDataItem[AccelBatchSize]
            };

            for (int i = 0; i < scoringData.AccelData.Length; ++i)
            {
                _vrHandler.GetAccelValues(ref scoringData.AccelData[i], AccelAcqDelta / 1000.0f);
                _scoringDataSent++;

                Console.WriteLine("Accel data: " + scoringData.AccelData[i].X + " " + scoringData.AccelData[i].Y + " " + scoringData.AccelData[i].Z);
                Thread.Sleep(AccelAcqDelta);
            }

            await _connection.SendAsync(scoringData);
        }
    }

    public void Start()
    {
        if (Started)
        {
            return;
        }

        Started = true;
        
        _intercept.KeyPressed = KeyPressed;
        //_intercept.Start();
        _vrHandler.Start();

        new Thread(UpdateLoop).Start();
    }

    public async Task Stop(bool reconnect = false)
    {
        if (!Started || _connection == null)
        {
            return;
        }

        Started = reconnect;

        await _connection.DisconnectAsync();
    }

    public async void UpdateLoop()
    {
        while (Started)
        {
            Console.WriteLine("Hosts scanning started.");

            var discovery = new DiscoveryManager
            {
                SingleShot = true
            };
            discovery.Discover();

            //TODO: refactor it
            while (!discovery.IsCompleted)
            {
            }

            if (discovery.FoundHosts.Count == 0)
            {
                Console.WriteLine("I haven't found any hosts.");
                Thread.Sleep(3000);
                continue;
            }

            var host = discovery.FoundHosts.First();
            Console.WriteLine($"Connecting using first found host: {host}.");

            _connection = new WebSocketConnection(host)
            {
                PrintPackets = false,
                HandleResponse = HandleResponse,
                AccelAcquisitionFreqHz = AccelTotalPackets,
                AccelAcquisitionLatency = AccelAcqLatency,
                AccelMaxRange = OpenVRHandler.LIMIT
            };

            await _connection.ConnectAsync();

            while (_connection.IsAlive)
            {
                await Update();
            }
        }

        _vrHandler.Stop();
        _intercept.Stop();
    }

    public async void KeyPressed(ConsoleKey key)
    {
        //TODO: full gameplay navigation
        //TODO: OpenVR navigation

        if (_connection == null)
        {
            return;
        }

        switch (key)
        {
            case ConsoleKey.Y:
                {
                    await Stop(true);
                    return;
                }
            case ConsoleKey.U:
                {
                    await Navigate(Client.NavigationAction.SelectionConfirm);
                    return;
                }
            case ConsoleKey.J:
                {
                    await Navigate(Client.NavigationAction.SwipeLeft);
                    break;
                }
            case ConsoleKey.L:
                {
                    await Navigate(Client.NavigationAction.SwipeRight);
                    break;
                }
            case ConsoleKey.I:
                {
                    await Navigate(Client.NavigationAction.SwipeUp);
                    break;
                }
            case ConsoleKey.K:
                {
                    await Navigate(Client.NavigationAction.SwipeDown);
                    break;
                }
            case ConsoleKey.O:
                {
                    await Navigate(Client.NavigationAction.ActionLeft);
                    break;
                }
            case ConsoleKey.P:
                {
                    await Navigate(Client.NavigationAction.ActionRight);
                    break;
                }
        }
    }
}