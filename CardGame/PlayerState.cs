using System;
using System.Collections.Generic;
using CardGame.Core.Cards;

namespace CardGame.Core.Game
{
    public sealed class PlayerState
    {
        public string Name { get; set; } = string.Empty;
        public int MaxHealth { get; set; } = 20;
        public int Health { get; set; } = 20;

        public int MaxMana { get; set; }
        public int Mana { get; set; }

        public List<Card> Hand { get; set; } = new();
        public List<CreatureCard> Board { get; set; } = new();

        public PlayerState() { }

        public PlayerState(string name, int maxHealth = 20)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

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

        public void StartTurn()
        {
            MaxMana = Math.Min(10, MaxMana + 1);
            Mana = MaxMana;
        }

        public void SpendMana(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (Mana < amount) throw new InvalidOperationException("Недостаточно маны.");
            Mana -= amount;
        }
    }
}
