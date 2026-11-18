using Turnaj.Model;
using static Turnaj.Tests.TournamentSetup;

namespace Turnaj.Tests;

public class RoundTests
{
    [Fact]
    public void PairsRequireDifferentEnrolledPlayersEachUsedOnce()
    {
        var service = WithPlayers(1, "A", "B", "C");
        Check(service.AddPlayer("David", "D"));
        Check(service.CreateRound());
        Assert.False(service.AddPair("A", "a").Ok);
        Assert.False(service.AddPair("A", "D").Ok);
        Check(service.AddPair("A", "B"));
        Assert.False(service.AddPair("C", "a").Ok);
        Assert.False(service.AddPair("b", "C").Ok);
        var match = Assert.Single(service.GetRound(1)!.Matches);
        Assert.Equal(("A", "B"), (match.First, match.Second));
    }

    [Fact]
    public void ByeIsExclusiveInBothDirections()
    {
        var service = WithPlayers(1, "A", "B", "C", "D", "E");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Assert.False(service.SetBye("a").Ok);
        Check(service.SetBye("C"));
        Assert.False(service.SetBye("D").Ok);
        Assert.False(service.AddPair("D", "c").Ok);
        Assert.Equal("C", service.GetRound(1)!.ByeNick);
        Assert.Single(service.GetRound(1)!.Matches);
    }

    [Theory]
    [InlineData(MatchResult.None)]
    [InlineData((MatchResult)99)]
    public void InvalidCorrectionPreservesOriginalResult(MatchResult invalid)
    {
        var service = WithPlayers(1, "A", "B");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Check(service.SetResult(1, 1, MatchResult.FirstWins));
        Assert.False(service.SetResult(1, 1, invalid).Ok);
        Assert.Equal(MatchResult.FirstWins, service.GetRound(1)!.Matches[0].Result);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(2, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 2)]
    public void UnknownMatchOrRoundCannotChangeResult(int round, int match)
    {
        var service = WithPlayers(1, "A", "B");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Assert.False(service.SetResult(round, match, MatchResult.Draw).Ok);
        Assert.Equal(MatchResult.None, service.GetRound(1)!.Matches[0].Result);
    }

    [Fact]
    public void ClosingCountsMissingResultsAndFreezesOnlyCompletedRound()
    {
        var service = WithPlayers(2, "A", "B", "C", "D", "E");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Check(service.AddPair("C", "D"));
        Check(service.SetBye("E"));
        var first = service.GetRound(1)!;

        var missingTwo = service.CloseRound(1);
        Assert.False(missingTwo.Ok);
        Assert.Contains("2", missingTwo.Message);
        Assert.False(first.IsClosed);
        Check(service.SetResult(1, 2, MatchResult.Draw));
        var missingOne = service.CloseRound(1);
        Assert.False(missingOne.Ok);
        Assert.Contains("1", missingOne.Message);
        Assert.False(first.IsClosed);

        Check(service.SetResult(1, 1, MatchResult.FirstWins));
        Check(service.CloseRound(1));
        Assert.True(first.IsClosed);
        Check(service.CreateRound());
        Assert.False(service.SetResult(1, 1, MatchResult.SecondWins).Ok);
        Assert.Equal(MatchResult.FirstWins, first.Matches[0].Result);
    }

    [Fact]
    public void UnknownResultInModelDoesNotCountAsCompleted()
    {
        var service = WithPlayers(1, "A", "B");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        var round = service.GetRound(1)!;
        round.Matches[0].Result = (MatchResult)99;
        var result = service.CloseRound(1);
        Assert.False(result.Ok);
        Assert.Contains("1", result.Message);
        Assert.False(round.IsClosed);
        Assert.All(service.GetStandings(), row => Assert.Equal(0, row.Played));
    }
}
