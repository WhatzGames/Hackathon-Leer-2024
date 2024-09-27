namespace Hackathon_24;

public class ProbabilityOfDoubleLetters
{
    public Dictionary<string, double> ProbabilityDoubleLetters()
    {
        return new Dictionary<string, double>()
        {
            { "ER", 4.09 },
            { "EN", 4.00 },
            { "CH", 2.42 },
            { "DE", 2.27 },
            { "EI", 1.93 },
            { "ND", 1.87 },
            { "TE", 1.85 },
            { "IN", 1.68 },
            { "IE", 1.63 },
            { "GE", 1.47 },
            { "SS", 0.76 },
            { "NN", 0.43 },
            { "LL", 0.42 },
            { "EE", 0.23 },
            { "MM", 0.23 },
            { "TT", 0.23 },
            { "RR", 0.15 },
            { "DD", 0.13 },
            { "FF", 0.12 },
            { "AA", 0.08 }
        };
    }
}