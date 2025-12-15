namespace CardGame.Core.Cards
{
    /// <summary>
    /// Тип заклинания.
    /// </summary>
    public enum SpellType
    {
        /// <summary>Наносит урон цели.</summary>
        Damage,

        /// <summary>Лечит цель.</summary>
        Heal,

        /// <summary>Увеличивает атаку существа (баф).</summary>
        BuffAttack
    }
}
    /// <summary>
    /// Карта-заклинание: тип (урон/лечение/баф) и сила эффекта.
    /// </summary>
    public sealed class SpellCard : Card
    {
        /// <summary>Тип заклинания.</summary>
        public SpellType Type { get; set; }

        /// <summary>Сила эффекта (например, урон/лечение/баф).</summary>
        public int Value { get; set; }

        // Нужен для JSON-десериализации
        public SpellCard() { }

        public SpellCard(string name, int manaCost, SpellType type, int value)
            : base(name, manaCost)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Значение эффекта должно быть > 0.");

            Type = type;
            Value = value;
        }

        public override string ToString() =>
            $"{Name} (cost: {ManaCost}) {Type} {Value}";
    }
    }
