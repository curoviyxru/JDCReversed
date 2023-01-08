using JDCReversed;

Console.WriteLine("Hosts scanning started.");
var discovery = new DiscoveryManager
{
    SingleShot = true
};
discovery.Discover();
while (!discovery.IsCompleted)
{
}

if (discovery.FoundHosts.Count == 0)
{
    Console.WriteLine("I haven't found any hosts. Exiting.");
    return;
}

var host = discovery.FoundHosts.First();
Console.WriteLine($"Connecting using first found host: {host}.");

var wsConnection = new WebSocketConnection(host)
{
    PrintPackets = false
};
await wsConnection.ConnectAsync();
while (wsConnection.IsAlive) Console.ReadLine();