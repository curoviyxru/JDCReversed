using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace JDCReversed;

public class DiscoveryManager
{
    private readonly object _udpLock = new();
    private Timer? _broadcastTimer;
    private Timer? _completedTimer;
    private UdpClient? _udp;

    public List<string> FoundHosts { get; } = new();

    public bool IsCompleted => _udp == null;

    public bool SingleShot { get; set; }

    public void Discover()
    {
        lock (_udpLock)
        {
            if (_udp != null)
                return;

            _udp = new UdpClient(7000);
            _udp.EnableBroadcast = true;
            _broadcastTimer = new Timer(500);
            _broadcastTimer.Elapsed += OnTimerElapsed;
            _broadcastTimer.Enabled = true;
            _completedTimer = new Timer(5000);
            _completedTimer.Elapsed += OnCompletedTimerElapsed;
            _completedTimer.Enabled = true;
            FoundHosts.Clear();
            Listen();
            Broadcast();
        }
    }

    private void Listen()
    {
        lock (_udpLock)
        {
            if (_udp == null)
                return;

            _udp.BeginReceive(EndReceive, null);
        }
    }

    private void EndReceive(IAsyncResult ar)
    {
        lock (_udpLock)
        {
            if (_udp == null)
                return;

            var endpoint = new IPEndPoint(IPAddress.Any, 0);
            var array = _udp.EndReceive(ar, ref endpoint);
            if (array.Length > 0)
            {
                var data = Encoding.UTF8.GetString(array);
                //var packet = JsonConvert.DeserializeObject<DiscoveryPacket>(data);

                if (endpoint != null && !FoundHosts.Contains(endpoint.Address.ToString()))
                {
                    FoundHosts.Add(endpoint.Address.ToString());
                    Console.WriteLine($"Got console: {endpoint.Address}, {data}");

                    if (SingleShot)
                    {
                        Cancel();
                        return;
                    }
                }
            }

            Listen();
        }
    }

    private void Broadcast()
    {
        lock (_udpLock)
        {
            if (_udp == null)
                return;

            var bytes = Encoding.ASCII.GetBytes("phonescoring.jd.ubisoft.com");
            try
            {
                _udp.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, 6000));
            }
            catch
            {
                
            }
        }
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Broadcast();
    }

    private void OnCompletedTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        Cancel();
    }

    public void Cancel()
    {
        lock (_udpLock)
        {
            if (_udp == null)
                return;

            if (_broadcastTimer != null)
            {
                _broadcastTimer.Enabled = false;
                _broadcastTimer.Elapsed -= OnTimerElapsed;
                _broadcastTimer = null;
            }

            if (_completedTimer != null)
            {
                _completedTimer.Enabled = false;
                _completedTimer.Elapsed -= OnCompletedTimerElapsed;
                _completedTimer = null;
            }

            _udp.Close();
            _udp = null;
        }
    }

    public class DiscoveryPacket
    {
        [JsonProperty("platform")] public string? Platform { get; set; }

        [JsonProperty("titleId")] public string? TitleId { get; set; }

        [JsonProperty("protocol")] public string? Protocol { get; set; }

        [JsonProperty("consoleName")] public string? ConsoleName { get; set; }
    }
}