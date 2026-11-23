using Turnaj.Logic;
using Turnaj.Model;
using Turnaj.Storage;
using static Turnaj.Tests.TournamentSetup;

namespace Turnaj.Tests;

public class PlayerHistoryTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FinishedTournamentSurvivesPlayerRemovalAndReload(bool startAnother)
    {
        var service = WithPlayers(1, "A", "B", "C");
        Check(service.CreateRound());
        Check(service.AddPair("A", "B"));
        Check(service.SetBye("C"));
        Check(service.SetResult(1, 1, MatchResult.Draw));
        Check(service.CloseRound(1));
        Check(service.FinishTournament());
        Assert.False(service.RemovePlayer("C").Ok);
        if (startAnother) Check(service.CreateTournament("Druhý", "šachy", 1));
        Assert.False(service.RemovePlayer("A").Ok);

        var directory = Directory.CreateTempSubdirectory();
        try
        {
            var path = Path.Combine(directory.FullName, "turnaj.dat");
            Check(TournamentFileStore.Save(path, service.Players, service.Current));
            var loaded = TournamentFileStore.Load(path);
            Assert.Equal(LoadStatus.Ok, loaded.Status);
            var restored = new TournamentService();
            restored.Replace(loaded.Players!, loaded.Tournament);
            Assert.False(restored.RemovePlayer("a").Ok);
            Assert.Equal(startAnother, restored.RemovePlayer("c").Ok);
        }
        finally
        {
            directory.Delete(true);
        }
    }
}
