using System;

namespace CardGame.Core.Cards
{ 
    /// Тип заклинания.
    public enum SpellType
    {
        /// Наносит урон цели.</summary>
        Damage,

        /// Лечит цель.</summary>
        Heal,

        /// Увеличивает атаку существа.</summary>
        BuffAttack
    }


    /// Карта-заклинание: тип (урон/лечение/баф) и сила эффекта.
    public sealed class SpellCard : Card
    {
        public SpellType Type { get; set; }
        public int Value { get; set; }

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
