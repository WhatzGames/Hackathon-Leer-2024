// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Hackathon2024.Bots;
using Hackathon2024.DTO;
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

    await socketIo.EmitAsync("authenticate", (success) => Console.WriteLine($"Authentication successful: {success}"),
        args[1]);

    Dictionary<Guid, IBot> bots = []; 

    socketIo.On("data", async (response) =>
    {
        using var game = response.GetValue<JsonDocument>();
        JsonElement root = game.RootElement;
        string? type = root.GetProperty("type")
                           .GetString();
        if (!root.GetProperty("id")
                 .TryGetGuid(out Guid gameId))
        {
            Console.WriteLine("No game id found");
        }
        switch (type)
        {
            case "INIT":
                Console.WriteLine("Init");
                Init? init = game.Deserialize<Init>();
                if (init is null)
                {
                    Console.WriteLine("parsing init failed!");
                    break;
                }
                bots[gameId] = BotFactory.CreateBot(args[0]);
                break;
            case "ROUND":
                Console.WriteLine("Round");
                Round? round = game.Deserialize<Round>();
                if (round is null)
                {
                    Console.WriteLine("parsing round failed!");
                    break;
                }
                char character = bots[gameId].CalculateNextStep(round);
                await response.CallbackAsync(character);
                Console.WriteLine($"{gameId} selected {character}");
                break;
            case "RESULT":
                Console.WriteLine("Result");
                Result? result = game.Deserialize<Result>();
                if (result is null)
                {
                    Console.WriteLine("parsing result failed!");
                    break;
                }
                bots[gameId].Complete(result);
                bots.Remove(gameId);
                Console.WriteLine($"{gameId}: {(result.players?.Find(x => x.id == result.self)
                                                      ?.score)} FOR WORD {result.word}");
                break;
        }
    });
    using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    while (await periodicTimer.WaitForNextTickAsync())
    {
        Console.WriteLine("Still Alive!");
    }