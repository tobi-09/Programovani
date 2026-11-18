using Turnaj.Model;
using static Turnaj.Tests.TournamentSetup;

namespace Turnaj.Tests;

public class StandingsTests
{
    [Theory]
    [InlineData(MatchResult.FirstWins, 2, 0)]
    [InlineData(MatchResult.SecondWins, 0, 2)]
    [InlineData(MatchResult.Draw, 1, 1)]
    public void ScoresMatchSpecification(MatchResult result, int firstPoints, int secondPoints)
    {
        var service = WithPlayers(1, "A", "B", "C");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Check(service.SetBye("C"));
        var before = service.GetStandings();
        Assert.All(before, row => Assert.Equal(0, row.Played));
        Assert.All(before.Where(row => row.Nickname != "C"), row => Assert.Equal(0, row.Points));

        Check(service.SetResult(1, 1, result));
        var rows = service.GetStandings().ToDictionary(row => row.Nickname);
        Assert.Equal((firstPoints, 1), (rows["A"].Points, rows["A"].Played));
        Assert.Equal((secondPoints, 1), (rows["B"].Points, rows["B"].Played));
        Assert.Equal((2, 0), (rows["C"].Points, rows["C"].Played));
    }

    [Fact]
    public void CorrectionRecomputesTotalsAndRanksAcrossRounds()
    {
        var service = WithPlayers(2, "A", "B", "C");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Check(service.SetBye("C"));
        Check(service.SetResult(1, 1, MatchResult.Draw));
        Check(service.CloseRound(1));
        Check(service.CreateRound());
        Check(service.AddPair("C", "A"));
        Check(service.SetBye("B"));
        Check(service.SetResult(2, 1, MatchResult.FirstWins));
        Assert.Equal(new[] {
            new StandingRow(1, "C", 4, 1), new StandingRow(2, "B", 3, 1),
            new StandingRow(3, "A", 1, 2)
        }, service.GetStandings());

        Check(service.SetResult(2, 1, MatchResult.SecondWins));
        Assert.Equal(new[] {
            new StandingRow(1, "A", 3, 2), new StandingRow(1, "B", 3, 1),
            new StandingRow(3, "C", 2, 1)
        }, service.GetStandings());
    }
}
