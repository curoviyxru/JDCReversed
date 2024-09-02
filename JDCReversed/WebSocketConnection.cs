using System.Net.WebSockets;
using JDCReversed.Packets;
using Websocket.Client;

namespace JDCReversed;

//TODO: JD2015 connection socket?
//TODO: some vanilla JD2017 packets?
//TODO: assets/images downloading?
public class WebSocketConnection
{
    public double AccelAcquisitionFreqHz { get; set; } = 50;
    public double AccelMaxRange { get; set; } = 4;
    public double AccelAcquisitionLatency { get; set; } = 50;
    public Action<JdObject?>? HandleResponse { get; set; }

    private readonly WebsocketClient _ws;

    public WebSocketConnection(string host)
    {
        var factory = new Func<ClientWebSocket>(() =>
        {
            var ws = new ClientWebSocket();
            //TODO: fix keepalive
            ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            ws.Options.AddSubProtocol("v1.phonescoring.jd.ubisoft.com");
            return ws;
        });
        _ws = new WebsocketClient(new Uri($"ws://{host}:8080/smartphone"), factory);
        _ws.MessageReceived.Subscribe(async msg => await HandleOnMessage(msg));
        _ws.DisconnectionHappened.Subscribe(HandleOnClose);
        _ws.IsReconnectionEnabled = false;
    }

    public bool IsAlive => _ws.IsStarted;
    public bool PrintPackets { get; set; } = false;

    private void HandleOnClose(DisconnectionInfo result)
    {
        Console.WriteLine($"Closed: {result.CloseStatus}, {result.CloseStatusDescription}");
    }

    private async Task HandleOnMessage(ResponseMessage msg)
    {
        switch (msg.MessageType)
        {
            case WebSocketMessageType.Binary:
                Console.WriteLine("Got a binary. Skipping.");
                return;
            case WebSocketMessageType.Close:
                await DisconnectAsync();
                return;
        }

        if (PrintPackets)
            Console.WriteLine(msg.Text);

        var raw = JdObject.Deserialize(msg.Text);
        switch (raw)
        {
            case JdPhoneDataCmdHandshakeContinue data:
            {
                if (data.ProtocolVersion > 3)
                {
                    await DisconnectAsync();
                    break;
                }

                await SendAsync(new JdPhoneDataCmdSync
                {
                    PhoneId = data.PhoneId
                });
                break;
            }
            case JdPhoneDataCmdSyncEnd data:
            {
                await SendAsync(new JdPhoneDataCmdSyncEnd
                {
                    PhoneId = data.PhoneId
                });
                Console.WriteLine($"Phone connected: {data.PhoneId}");
                break;
            }
            case JdPhoneDataCmdSyncStart _: break;
            case JdProfilePhoneUiData _: break;
            case JdEnablePhotoConsoleCommandData _: break;
            case JdShortcutSetupConsoleCommandData _: break;
            case JdInputSetupConsoleCommandData _: break;
            case JdPlaySoundConsoleCommandData _: break;
            case JdTriggerTransitionConsoleCommandData _: break;
            case JdClosePopupConsoleCommandData _: break;
            case JdPhoneUiSetupData _: break;
            case JdEnableLobbyStartbuttonConsoleCommandData _: break;
            case JdEnableAccelValuesSendingConsoleCommandData _: break;
            case JdDisableAccelValuesSendingConsoleCommandData _: break;
            case JdNewPhotoConsoleCommandData _: break;
            case JdOpenPhoneKeyboardConsoleCommandData _: break;
            case JdEnableCarouselConsoleCommandData _: break;
            case Jd2015NotPhoneScoring _:
            {
                await DisconnectAsync();
                break;
            }
            default:
            {
                Console.WriteLine($"Got unknown data: {msg.Text}");
                break;
            }
        }

        HandleResponse?.Invoke(raw);
    }

    public async Task ConnectAsync()
    {
        await _ws.Start();
        Console.WriteLine("Connected");
        await SendAsync(new JdPhoneDataCmdHandshakeHello
        {
            //Android: "0.1" in code
            ClientVersion = "0.1",
            //Android: 0 in code
            AccelAcquisitionFreqHz = AccelAcquisitionFreqHz,
            //Not Android (iOS): 4f
            //Android: android.hardware.Sensor.getMaximumRange() / 9.80665f
            AccelMaxRange = AccelMaxRange,
            //Android: 40 in code
            AccelAcquisitionLatency = AccelAcquisitionLatency,
            //TODO: how to authorize?
            JmcsToken = string.Empty
        });
    }

    public async Task DisconnectAsync()
    {
        Console.WriteLine("Disconnecting");
        await _ws.Stop(WebSocketCloseStatus.EndpointUnavailable, "CLOSE_GOING_AWAY");
    }

    public async Task SendAsync(object obj, bool wrapRoot = true)
    {
        obj = wrapRoot
            ? new Dictionary<string, object>
            {
                { "root", obj }
            }
            : obj;

        var serialized = JdObject.Serialize(obj);

        if (PrintPackets)
        {
            Console.WriteLine(serialized);
        }

        await _ws.SendInstant(serialized);
    }
}