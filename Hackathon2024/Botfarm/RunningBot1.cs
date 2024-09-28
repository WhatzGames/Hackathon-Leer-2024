namespace Hackathon2024.Botfarm;

using System.Text.Json;
using Hackathon2024.Bots;
using Hackathon2024.DTO;
using SocketIOClient;
using SocketIOClient.Transport;

public class RunningBot1 : IRunningBot
{
    private readonly string _botName;
    private readonly string _secret;
    private readonly IBot _bot;

    public RunningBot1(string botName, string secret, IBot bot)
    {
        _botName = botName;
        _secret = secret;
        _bot = bot;
    }

    public async Task RunBot()
    {
        using var socketIo = new SocketIOClient.SocketIO("https://games.uhno.de",
        new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

        socketIo.OnConnected += (sender, e) => Console.WriteLine("Connected");

        socketIo.OnDisconnected += (sender, e) => Console.WriteLine("Disconnected");

        await socketIo.ConnectAsync();

        Console.WriteLine("Trying to Authenticate");

        await socketIo.EmitAsync("authenticate",
            (success) => Console.WriteLine($"{_botName} Authentication successful: {success}"),
            _secret);

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

                    bots[gameId] = _bot;
                    break;
                case "ROUND":
                    //Console.WriteLine("Round");
                    Round? round = game.Deserialize<Round>();
                    if (round is null)
                    {
                        Console.WriteLine("parsing round failed!");
                        break;
                    }

                    char character = bots[gameId]
                       .CalculateNextStep(round);
                    await response.CallbackAsync(character);
                    //Console.WriteLine($"{_botName} selected {character}");
                    break;
                case "RESULT":
                    //Console.WriteLine("Result");
                    Result? result = game.Deserialize<Result>();
                    if (result is null)
                    {
                        Console.WriteLine("parsing result failed!");
                        break;
                    }

                    bots[gameId]
                       .Complete(result);
                    bots.Remove(gameId);
                    Console.WriteLine($"{_botName}: {(result.players?.Find(x => x.id == result.self)
                                                           ?.score)} FOR WORD {result.word}");
                    break;
            }
        });
        using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        while (await periodicTimer.WaitForNextTickAsync())
        {
            Console.WriteLine($"{_botName} still alive");
        }
    }
}