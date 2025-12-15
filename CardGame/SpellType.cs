using System;

namespace CardGame.Core.Cards
{
    /// Типы заклинаний.
    public enum SpellType
    {
        Damage,
        Heal,
        BuffAttack
    }

    /// Карта-заклинание.
    public sealed class SpellCard : Card
    {
        public SpellType Type { get; set; }
        public int Value { get; set; }

        // Нужен для JSON-десериализации
        public SpellCard() { }

        public SpellCard(string name, int manaCost, SpellType type, int value)
            : base(name, manaCost)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            Type = type;
            Value = value;
        }

        public override string ToString() =>
            $"{Name} (cost: {ManaCost}) {Type} {Value}";
    }
}
