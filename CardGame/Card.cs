using System;

namespace CardGame.Core.Cards
{
    /// Базовый класс карты — содержит общие свойства, 
    /// которые есть у всех карт (название, стоимость маны).
    public abstract class Card
    {
        /// Название карты</summary>
        public string Name { get; init; } = string.Empty;

        /// Стоимость розыгрыша (мана)</summary>
        public int ManaCost { get; init; }

        protected Card() { }

        protected Card(string name, int manaCost)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название карты не может быть пустым.", nameof(name));
            if (manaCost < 0)
                throw new ArgumentOutOfRangeException(nameof(manaCost), "Стоимость не может быть отрицательной.");

            Name = name;
            ManaCost = manaCost;
        }

        public override string ToString() => $"{Name} (cost: {ManaCost})";
    }
}
