using Turnaj.Logic;
using Turnaj.Model;
using static Turnaj.Tests.TournamentSetup;

namespace Turnaj.Tests;

public class TournamentTests
{
    [Theory]
    [InlineData(null, "šachy", 3)]
    [InlineData("", "šachy", 3)]
    [InlineData("  ", "šachy", 3)]
    [InlineData("Pohár", null, 3)]
    [InlineData("Pohár", "  ", 3)]
    [InlineData("Pohár", "šachy", 0)]
    [InlineData("Pohár", "šachy", -1)]
    public void InvalidTournamentLeavesStateEmpty(string? name, string? discipline, int rounds)
    {
        var service = new TournamentService();
        var result = service.CreateTournament(name, discipline, rounds);
        Assert.False(result.Ok);
        Assert.False(string.IsNullOrWhiteSpace(result.Message));
        Assert.Null(service.Current);
    }

    [Fact]
    public void SecondOpenTournamentCannotReplaceFirst()
    {
        var service = new TournamentService();
        Check(service.CreateTournament(" Pohár ", " šachy ", 101));
        var first = Assert.IsType<Tournament>(service.Current);
        Assert.Equal(("Pohár", "šachy", 101), (first.Name, first.Discipline, first.RoundCount));
        Assert.False(service.CreateTournament("Jiný", "piškvorky", 2).Ok);
        Assert.Same(first, service.Current);
    }

    [Fact]
    public void RegistrationUsesRegistryAndCanonicalNickname()
    {
        var service = WithPlayers(1);
        Assert.False(service.Enroll("Eva").Ok);
        Check(service.AddPlayer("Eva Nová", "Eva"));
        Assert.False(service.Unenroll("Eva").Ok);
        Check(service.Enroll(" eva "));
        Assert.False(service.Enroll("EVA").Ok);
        Assert.Equal("Eva", Assert.Single(service.Current!.Enrolled));
        Check(service.Unenroll("eVa"));
        Assert.False(service.Unenroll("Eva").Ok);
        Assert.Empty(service.Current.Enrolled);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void MissingNicknameKeepsRoster(string? nick)
    {
        var service = WithPlayers(1, "Eva");
        Assert.False(service.Enroll(nick).Ok);
        Assert.False(service.Unenroll(nick).Ok);
        Assert.Equal("Eva", Assert.Single(service.Current!.Enrolled));
    }

    [Fact]
    public void FirstRoundLocksBothRegistrationOperations()
    {
        var service = WithPlayers(2, "Eva", "Jan");
        Check(service.AddPlayer("Petr", "Petr"));
        Check(service.CreateRound());
        Assert.False(service.Enroll("Petr").Ok);
        Assert.False(service.Unenroll("Eva").Ok);
        Assert.Equal(new[] { "Eva", "Jan" }, service.Current!.Enrolled);
        Assert.Empty(Assert.Single(service.Current.Rounds).Matches);
    }

    [Fact]
    public void RoundRequiresOpenTournament()
    {
        var service = new TournamentService();
        Assert.False(service.CreateRound().Ok);
        service = WithPlayers(2, "A", "B");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Check(service.SetResult(1, 1, MatchResult.Draw));
        Check(service.CloseRound(1));
        Check(service.FinishTournament());
        Assert.False(service.CreateRound().Ok);
        Assert.Single(service.Current!.Rounds);
    }

    [Fact]
    public void NextRoundNeedsClosedPredecessorAndAvailableSlot()
    {
        var service = WithPlayers(2, "A", "B");
        Check(service.CreateRound());
        Assert.False(service.CreateRound().Ok);
        Assert.Equal(1, Assert.Single(service.Current!.Rounds).Number);
        for (int number = 1; number <= 2; number++)
        {
            if (number == 2) Check(service.CreateRound());
            Check(service.AddPair("A", "B"));
            Check(service.SetResult(number, 1, MatchResult.Draw));
            Check(service.CloseRound(number));
        }
        Assert.False(service.CreateRound().Ok);
        Assert.Equal(new[] { 1, 2 }, service.Current.Rounds.Select(round => round.Number));
    }
}
