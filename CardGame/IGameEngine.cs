using CardGame.Core.Game;

namespace CardGame.Core.Engine
{
    public interface IGameEngine
    {
        GameState NewGame();
        void PlayCard(GameState state, int handIndex, Target? target);
        void EndTurn(GameState state);
    }
}
