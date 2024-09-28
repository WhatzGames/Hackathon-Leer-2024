using Hackathon2024.Bots;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Botfarm;

public class BotfarmInit
{
    //note more then 10
    private int MaxBots = 1;
    
    private const string BotName = "OutOfMemory";
    private const string dir =
        "C:\\Users\\Kurt\\RiderProjects\\Hackathon 2024\\Hackathon-Leer-2024\\BotDbs\\";
    private List<string> dirs = new List<string>();
    private List<string> names = new List<string>();
    private List<IBot> bots = new List<IBot>();
    private List<IRunningBot> runningBots = new List<IRunningBot>();
    public List<IRunningBot> GetRunningBots()
    {
        for (int i = 1; i <= MaxBots; i++)
        {
            dirs.Add(dir + "db"+ i + ".sqlite");
        }

        for (int i = 1; i <= MaxBots; i++)
        {
            names.Add(BotName + i);
        }

        for (int i = 1; i <= MaxBots; i++)
        {
            bots.Add(new Bot5(new PossibleWordList(dirs[i-1])));
        }
        

        List<string> secrets = new List<string>()
        {
            "660c776e-b192-4f39-8a26-cdc7e1fd5c66",
            "5b047679-8656-4e61-83e9-ad85286d5e8a",
            "6fe2b392-c162-4b84-98c3-d28641048195",
            "b0e550a7-dbe5-46ef-8086-58ba63b9062c",
            "99536462-03ce-46f6-b98a-1c3124884d2b"
        };

        for (int i = 1; i <= MaxBots; i++)
        {
            var bot = new RunningBot1(
                names[i-1],
                secrets[i-1],
                bots[i-1]);
            runningBots.Add(bot);
        }


        return runningBots;
    }
}