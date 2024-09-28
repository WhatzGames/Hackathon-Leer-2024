using Hackathon2024.Bots;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Botfarm;

public class BotfarmInit
{
    private List<IRunningBot> _list = new List<IRunningBot>();
    public List<IRunningBot> GetRunningBots()
    {
        RunningBot1 bot1 = new RunningBot1("OutOfMemory1",
            "660c776e-b192-4f39-8a26-cdc7e1fd5c66",
            new Bot5(new PossibleWordList()));

        _list.Add(bot1);
        
        RunningBot1 bot2 = new RunningBot1("OutOfMemory2",
            "5b047679-8656-4e61-83e9-ad85286d5e8a",
            new Bot5(new PossibleWordList()));

        //_list.Add(bot2);
        
        RunningBot1 bot3 = new RunningBot1("OutOfMemory3",
            "6fe2b392-c162-4b84-98c3-d28641048195",
            new Bot5(new PossibleWordList()));

        //_list.Add(bot3);
        
        RunningBot1 bot4 = new RunningBot1("OutOfMemory4",
            "b0e550a7-dbe5-46ef-8086-58ba63b9062c",
            new Bot5(new PossibleWordList()));

        //_list.Add(bot4);
        
        RunningBot1 bot5 = new RunningBot1("OutOfMemory5",
            "99536462-03ce-46f6-b98a-1c3124884d2b",
            new Bot5(new PossibleWordList()));

        //_list.Add(bot5);

        return _list;
    }
}