using Turnaj.Logic;
using Turnaj.Model;

namespace Turnaj.Tests;

internal static class TournamentSetup
{
    public static void Check(OperationResult result) => Assert.True(result.Ok, result.Message);

    public static TournamentService WithPlayers(int rounds, params string[] nicknames)
    {
        var service = new TournamentService();
        Check(service.CreateTournament("Školní pohár", "šachy", rounds));
        foreach (string nick in nicknames)
        {
            Check(service.AddPlayer(nick, nick));
            Check(service.Enroll(nick));
        }
        return service;
    }
}
