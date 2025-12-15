using System;

namespace CardGame.Models
{
    public enum CardKind { Creature, Spell, Helper }

    public enum SpellType { Damage, Heal, Buff }

    public class Card
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public CardKind Kind { get; set; }

        // Creature
        public int? Attack { get; set; }
        public int? Health { get; set; }

        // Spell
        public SpellType? SpellKind { get; set; }
        public int? SpellValue { get; set; }

        public string? Description { get; set; }
    }
}
