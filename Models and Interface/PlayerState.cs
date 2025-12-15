using System;
using System.Collections.Generic;

namespace CardGame.Models
{
    public class PlayerState
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;

        public List<Card> Deck { get; set; } = new List<Card>();
        public List<Card> Hand { get; set; } = new List<Card>();
        public List<Card> Battlefield { get; set; } = new List<Card>();

        public int Health { get; set; } = 30;
    }
}
