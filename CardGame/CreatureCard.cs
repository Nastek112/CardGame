using System;

namespace CardGame.Core.Cards
{
    public sealed class CreatureCard : Card
    {
        public int Attack { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public CreatureCard() { }

        public CreatureCard(string name, int manaCost, int attack, int maxHealth)
            : base(name, manaCost)
        {
            if (attack < 0) throw new ArgumentOutOfRangeException(nameof(attack));
            if (maxHealth <= 0) throw new ArgumentOutOfRangeException(nameof(maxHealth));

            Attack = attack;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public bool IsDead => Health <= 0;

        public void TakeDamage(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Health = Math.Max(0, Health - amount);
        }

        public void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
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
