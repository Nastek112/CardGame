using System.Collections.Generic;

namespace CardGame.Core.Game
{
    /// Состояние всей игры:
    /// список игроков, чей ход и номер хода.
    public sealed class GameState
    {
        public List<PlayerState> Players { get; set; } = new();
        public int CurrentPlayerIndex { get; set; }
        public int TurnNumber { get; set; }

        public GameState() { }
    }
}
