using JDCReversed;
using JDCReversed.Packets;

namespace JDCReversedCli;

public class Client : WebSocketConnection
{
    public enum NavigationAction
    {
        SwipeLeft, SwipeRight, SwipeUp, SwipeDown, ActionLeft, ActionRight
    }
    
    private int _currentRow;
    private int _currentItem;
    private int _currentAction;

    private int _rowCount;
    private int _itemCount;
    private int _actionCount;
    
    private JdPhoneCarouselRow[]? _rows;
    
    public Client(string host) : base(host)
    {
    }

    public override void HandleResponse(JdObject? response)
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
        }
    }

    public async Task Navigate(NavigationAction action)
    {
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
        }
        
        if (request != null)
            await SendAsync(request);
    }

    private static Client? client = null;

    public async static void Start()
    {
        KeyIntercept.OnKeyPressed += KeyPressed;
        
        while (true)
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
                return;
            }

            var host = discovery.FoundHosts.First();
            Console.WriteLine($"Connecting using first found host: {host}.");

            client = new Client(host)
            {
                PrintPackets = false
            };

            await client.ConnectAsync();

            while (client.IsAlive)
            {
                
            }
        }
    }

    public static async void KeyPressed(ConsoleKey key) {
        if (client == null) {
            return;
        }

        switch (key)
        {
            case ConsoleKey.U:
                {
                    await client.DisconnectAsync();
                    return;
                }
            case ConsoleKey.J:
                {
                    await client.Navigate(Client.NavigationAction.SwipeLeft);
                    break;
                }
            case ConsoleKey.L:
                {
                    await client.Navigate(Client.NavigationAction.SwipeRight);
                    break;
                }
            case ConsoleKey.I:
                {
                    await client.Navigate(Client.NavigationAction.SwipeUp);
                    break;
                }
            case ConsoleKey.K:
                {
                    await client.Navigate(Client.NavigationAction.SwipeDown);
                    break;
                }
            case ConsoleKey.O:
                {
                    await client.Navigate(Client.NavigationAction.ActionLeft);
                    break;
                }
            case ConsoleKey.P:
                {
                    await client.Navigate(Client.NavigationAction.ActionRight);
                    break;
                }
        }
    }
}