using System.IO;
using Xunit;
using CardGame.Storage;
using CardGame.Models;

public class StorageTests
{
    [Fact]
    public void JsonGameStorage_SaveAndLoad_Roundtrip()
    {
        var path = "test_save.json";
        if (File.Exists(path)) File.Delete(path);

        var storage = new JsonGameStorage(path);
        var gs = new GameState();
        gs.TurnNumber = 7;
        gs.Players.Add(new PlayerState { Name = "A" });
        gs.Players.Add(new PlayerState { Name = "B" });

        storage.Save(gs);

        var loaded = storage.Load();
        Assert.NotNull(loaded);
        Assert.Equal(7, loaded!.TurnNumber);
        Assert.Equal(2, loaded.Players.Count);
        Assert.Equal("A", loaded.Players[0].Name);

        File.Delete(path);
    }
}
