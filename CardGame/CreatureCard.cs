using System;

namespace CardGame.Core.Cards
{
    /// <summary>
    /// Карта-сущность: имеет атаку и здоровье.
    /// </summary>
    public sealed class CreatureCard : Card
    {
        /// <summary>Текущая атака существа.</summary>
        public int Attack { get; set; }

        /// <summary>Текущее здоровье (меняется в бою).</summary>
        public int Health { get; set; }

        /// <summary>Максимальное здоровье (нужно, чтобы лечение не “улетало” в бесконечность).</summary>
        public int MaxHealth { get; set; }

        // Нужен для десериализации JSON
        public CreatureCard() { }

        public CreatureCard(string name, int manaCost, int attack, int maxHealth)
            : base(name, manaCost)
        {
            if (attack < 0)
                throw new ArgumentOutOfRangeException(nameof(attack), "Атака не может быть отрицательной.");
            if (maxHealth <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHealth), "Максимальное здоровье должно быть > 0.");

            Attack = attack;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public bool IsDead => Health <= 0;

        public void TakeDamage(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Урон не может быть отрицательным.");

            Health = Math.Max(0, Health - amount);
        }

        public void Heal(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Лечение не может быть отрицательным.");

            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void ChangeAttack(int delta)
        {
            Attack = Math.Max(0, Attack + delta);
        }

        public override string ToString() =>
            $"{Name} (cost: {ManaCost}) ATK:{Attack} HP:{Health}/{MaxHealth}";
    }
}
