using System;

namespace CardGame.Interfaces
{
    using CardGame.Models;

    public interface IGameStorage
    {
        GameState? Load();

        void Save(GameState state);
    }
}

