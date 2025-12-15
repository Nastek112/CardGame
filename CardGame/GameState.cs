using System.Collections.Generic;

namespace CardGame.Core.Game
{
    public sealed class GameState
    {
        public List<PlayerState> Players { get; set; } = new();
        public int CurrentPlayerIndex { get; set; } = 0;
        public int TurnNumber { get; set; } = 1;

        // Для JSON-десериализации
        public GameState() { }
    }
}
