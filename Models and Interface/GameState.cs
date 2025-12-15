using System.Collections.Generic;

namespace CardGame.Models
{
    public class GameState
    {
        public List<PlayerState> Players { get; set; } = new List<PlayerState>();
        public int CurrentPlayerIndex { get; set; } = 0;
        public int TurnNumber { get; set; } = 1;

        // NOTE: this class is intentionally minimal — расширяйте по договорённости с командой.
    }
}
