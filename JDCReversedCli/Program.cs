using JDCReversed;
using JDCReversedCli;

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

    var client = new Client(host)
    {
        PrintPackets = false
    };
    await client.ConnectAsync();
    while (client.IsAlive)
    {
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.Escape:
            {
                await client.DisconnectAsync();
                return;
            }
            case ConsoleKey.LeftArrow:
            {
                await client.Navigate(Client.NavigationAction.SwipeLeft);
                break;
            }
            case ConsoleKey.RightArrow:
            {
                await client.Navigate(Client.NavigationAction.SwipeRight);
                break;
            }
            case ConsoleKey.UpArrow:
            {
                await client.Navigate(Client.NavigationAction.SwipeUp);
                break;
            }
            case ConsoleKey.DownArrow:
            {
                await client.Navigate(Client.NavigationAction.SwipeDown);
                break;
            }
            case ConsoleKey.Q:
            {
                await client.Navigate(Client.NavigationAction.ActionLeft);
                break;
            }
            case ConsoleKey.E:
            {
                await client.Navigate(Client.NavigationAction.ActionRight);
                break;
            }
        }
    }
}