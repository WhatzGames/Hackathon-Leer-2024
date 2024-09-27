// See https://aka.ms/new-console-template for more information

using SocketIOClient;
using SocketIOClient.Transport;

using var socketIo = new SocketIOClient.SocketIO("https://games.uhno.de", new SocketIOOptions()
{
    Transport = TransportProtocol.WebSocket
});

socketIo.OnConnected += (sender, e) => Console.WriteLine("Connected");

socketIo.OnDisconnected += (sender, e) => Console.WriteLine("Disconnected");

await socketIo.ConnectAsync();

Console.WriteLine("Trying to Authenticate");

await socketIo.EmitAsync("authenticate", (success) => Console.WriteLine($"Authentication successful: {success}"), args[0]);

    socketIo.On("data", async (response) =>
    {
        var game = response.GetValue<dynamic>();
        switch (game.type)
        {
            case "INIT":
                Console.WriteLine("Init");
                break;
            case "RESULT":
                Console.WriteLine("Result");
                break;
            case "ROUND":
                Console.WriteLine("Round");
                break;
        }
    });
    using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    while (await periodicTimer.WaitForNextTickAsync())
    {
        Console.WriteLine("Still Alive!");
    }