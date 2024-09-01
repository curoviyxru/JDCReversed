using System.Runtime.InteropServices;
using JDCReversed;
using JDCReversed.Packets;
using Valve.VR;

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

    private bool _sendScoringData;
    private int _scoringDataSent;
    private AccelDataItem _previousVelocity;
    
    private JdPhoneCarouselRow[]? _rows;
    
    public Client(string host) : base(host)
    {
    }

    public override void HandleResponse(JdObject? response)
    {
        Console.WriteLine("Got response: " + response?.GetType());

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
                _previousVelocity = new AccelDataItem();
                _sendScoringData = true;
                break;
            }
            case JdDisableAccelValuesSendingConsoleCommandData:
            {
                _sendScoringData = false;
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

    public async Task Update()
    {
        if (client != null && _sendScoringData)
        {
            int batchCount = 10;
            int batchSize = 5;
            int packetsTotal = batchCount * batchSize;
            int delta = 1000 / packetsTotal;
            double limit = 4;
            double multiplier = 9.80665; //9.80665
            for (int j = 0; j < batchCount; ++j)
            {
                JdPhoneScoringData scoringData = new()
                {
                    Timestamp = _scoringDataSent,
                    AccelData = new AccelDataItem[batchSize]
                };

                for (int i = 0; i < scoringData.AccelData.Length; ++i)
                {
                    uint cid = foundControllerRight == invalidController ? foundController : foundControllerRight;
                    VRControllerState_t controllerState = new();
                    TrackedDevicePose_t trackedDevicePose = new();
                    OpenVR.System.GetControllerStateWithPose(ETrackingUniverseOrigin.TrackingUniverseStanding, cid, ref controllerState, (uint)Marshal.SizeOf(typeof(VRControllerState_t)), ref trackedDevicePose);
                    scoringData.AccelData[i] = new AccelDataItem
                    {
                        //Player's left - right
                        X = Math.Clamp((trackedDevicePose.vVelocity.v0 - _previousVelocity.X) * -1000 / delta / multiplier, -limit, limit),
                        //Player's up - down
                        Y = Math.Clamp((trackedDevicePose.vVelocity.v1 - _previousVelocity.Y) *  1000 / delta / multiplier, -limit, limit),
                        //Player's forward - backward
                        Z = Math.Clamp((trackedDevicePose.vVelocity.v2 - _previousVelocity.Z) *  1000 / delta / multiplier, -limit, limit),
                    };
                    _scoringDataSent++;
                    _previousVelocity.X = trackedDevicePose.vVelocity.v0;
                    _previousVelocity.Y = trackedDevicePose.vVelocity.v1;
                    _previousVelocity.Z = trackedDevicePose.vVelocity.v2;
                    double x = trackedDevicePose.mDeviceToAbsoluteTracking.m3;
                    double y = trackedDevicePose.mDeviceToAbsoluteTracking.m7;
                    double z = trackedDevicePose.mDeviceToAbsoluteTracking.m11;
                    //x = scoringData.AccelData[i].X;
                    //y = scoringData.AccelData[i].Y;
                    //z = scoringData.AccelData[i].Z;
                    //x = trackedDevicePose.vVelocity.v0;
                    //y = trackedDevicePose.vVelocity.v1;
                    //z = trackedDevicePose.vVelocity.v2;
                    Console.WriteLine("Scoring data: " + i + " " + x + " " + y + " " + z + " " + trackedDevicePose.bPoseIsValid);
                    Thread.Sleep(delta);
                }

                await client.SendAsync(scoringData);
            }
        }
    }

    private static Client? client = null;

    const uint invalidController = OpenVR.k_unMaxTrackedDeviceCount + 1;
    static uint foundController = invalidController;
    static uint foundControllerRight = invalidController;

    private static void StartOpenVR() {
        EVRInitError error = EVRInitError.None;
        OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);
        
        for (uint i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; ++i) {
            ETrackedDeviceClass cls = OpenVR.System.GetTrackedDeviceClass(i);
            if (cls != ETrackedDeviceClass.Controller) continue;
            ETrackedControllerRole role = OpenVR.System.GetControllerRoleForTrackedDeviceIndex(i);
            if (foundController == invalidController) foundController = i;
            if (foundControllerRight == invalidController && role == ETrackedControllerRole.RightHand) foundControllerRight = i; 
        }

        //OpenVR.Shutdown();
    }

    public async static void Start()
    {
        KeyIntercept.OnKeyPressed += KeyPressed;
        StartOpenVR();
        
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
                await client.Update();
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