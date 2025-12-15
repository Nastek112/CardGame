using System;

namespace CardGame.Core.Game
{
    /// <summary>
    /// Тип цели для применения заклинаний/действий.
    /// </summary>
    public enum TargetType
    {
        Player,
        Creature
    }

    /// <summary>
    /// Цель: либо игрок (PlayerIndex), либо существо на столе игрока (PlayerIndex + CreatureIndex).
    /// </summary>
    public sealed class Target
    {
        public TargetType Type { get; set; }

        /// <summary>Индекс игрока (обычно 0 или 1).</summary>
        public int PlayerIndex { get; set; }

        /// <summary>Индекс существа на столе игрока. Используется только для TargetType.Creature.</summary>
        public int? CreatureIndex { get; set; }

        // Нужен для JSON-десериализации
        public Target() { }

        private Target(TargetType type, int playerIndex, int? creatureIndex)
        {
            Type = type;
            PlayerIndex = playerIndex;
            CreatureIndex = creatureIndex;
            Validate();
        }

        public static Target Player(int playerIndex) =>
            new Target(TargetType.Player, playerIndex, null);

        public static Target Creature(int playerIndex, int creatureIndex) =>
            new Target(TargetType.Creature, playerIndex, creatureIndex);

        /// <summary>
        /// Проверка корректности цели (можно вызывать в Engine перед применением).
        /// </summary>
        public void Validate()
        {
            if (PlayerIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(PlayerIndex), "Индекс игрока не может быть отрицательным.");

            if (Type == TargetType.Player)
            {
                if (CreatureIndex != null)
                    throw new InvalidOperationException("Для цели Player нельзя задавать CreatureIndex.");
            }
            else // Creature
            {
                if (CreatureIndex is null)
                    throw new InvalidOperationException("Для цели Creature нужно задать CreatureIndex.");
                if (CreatureIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(CreatureIndex), "Индекс существа не может быть отрицательным.");
            }
        }

        public override string ToString() =>
            Type == TargetType.Player
                ? $"Player[{PlayerIndex}]"
                : $"Creature[{PlayerIndex}][{CreatureIndex}]";
    }
}
