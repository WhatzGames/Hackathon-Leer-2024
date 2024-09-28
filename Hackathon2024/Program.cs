// See https://aka.ms/new-console-template for more information

using Hackathon2024.Botfarm;

List<IRunningBot> bots = new List<IRunningBot>();
BotfarmInit init = new BotfarmInit();

bots.AddRange(init.GetRunningBots());

Task.Run(() =>
{
    for (var i = 0; i < bots.Count; i++)
    {
        bots[i].RunBot();
        Console.WriteLine($"Startet bot {i}");
    }
});

while (true)
{
}