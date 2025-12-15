using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CardGame.Interfaces;
using CardGame.Models;

namespace CardGame.Storage
{
    public class JsonGameStorage : IGameStorage
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options;

        public JsonGameStorage(string path = "save.json")
        {
            _path = path;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
        }

        public GameState? Load()
        {
            if (!File.Exists(_path)) return null;
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<GameState>(json, _options);
        }

        public void Save(GameState state)
        {
            var json = JsonSerializer.Serialize(state, _options);
            File.WriteAllText(_path, json);
        }
    }
}
