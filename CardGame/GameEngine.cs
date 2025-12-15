using System;
using CardGame.Core.Cards;
using CardGame.Core.Game;

namespace CardGame.Core.Engine
{
    public sealed class GameEngine : IGameEngine
    {
        public GameState NewGame()
        {
            var state = new GameState();
            state.Players.Add(new PlayerState("Player 1"));
            state.Players.Add(new PlayerState("Player 2"));

            // Стартовые карты 
            state.Players[0].Hand.Add(new CreatureCard("Wolf", 1, 2, 2));
            state.Players[0].Hand.Add(new SpellCard("Fireball", 2, SpellType.Damage, 3));
            state.Players[0].Hand.Add(new SpellCard("Heal", 1, SpellType.Heal, 2));

            state.Players[1].Hand.Add(new CreatureCard("Bear", 2, 1, 4));
            state.Players[1].Hand.Add(new SpellCard("Rage", 1, SpellType.BuffAttack, 2));
            state.Players[1].Hand.Add(new SpellCard("Fireball", 2, SpellType.Damage, 3));

            // Первый ход
            state.CurrentPlayerIndex = 0;
            state.TurnNumber = 1;
            state.Players[0].StartTurn();

            return state;
        }

        public void PlayCard(GameState state, int handIndex, Target? target)
        {
            if (state is null) throw new ArgumentNullException(nameof(state));

            var player = state.Players[state.CurrentPlayerIndex];

            if (handIndex < 0 || handIndex >= player.Hand.Count)
                throw new ArgumentOutOfRangeException(nameof(handIndex), "Некорректный индекс карты в руке.");

            var card = player.Hand[handIndex];

            // Проверка маны
            if (player.Mana < card.ManaCost)
                throw new InvalidOperationException("Недостаточно маны для розыгрыша этой карты.");

            // Потратить ману сразу
            player.SpendMana(card.ManaCost);

            // Существо: положить на стол
            if (card is CreatureCard creature)
            {
                player.Hand.RemoveAt(handIndex);
                player.Board.Add(creature);
                return;
            }

            // Заклинание: нужна цель
            if (card is SpellCard spell)
            {
                if (target is null)
                    throw new InvalidOperationException("Для заклинания нужно указать цель.");

                target.Validate();

                ApplySpell(state, spell, target);

                player.Hand.RemoveAt(handIndex);
                return;
            }

            throw new InvalidOperationException("Неизвестный тип карты.");
        }

        public void EndTurn(GameState state)
        {
            if (state is null) throw new ArgumentNullException(nameof(state));

            state.CurrentPlayerIndex = (state.CurrentPlayerIndex + 1) % state.Players.Count;
            state.TurnNumber++;

            state.Players[state.CurrentPlayerIndex].StartTurn();
        }

        private static void ApplySpell(GameState state, SpellCard spell, Target target)
        {
            // Верхние границы индексов — это уже проверка “по состоянию игры”
            if (target.PlayerIndex >= state.Players.Count)
                throw new ArgumentOutOfRangeException(nameof(target.PlayerIndex), "Такого игрока не существует.");

            var targetPlayer = state.Players[target.PlayerIndex];

            switch (spell.Type)
            {
                case SpellType.Damage:
                    if (target.Type == TargetType.Player)
                    {
                        targetPlayer.TakeDamage(spell.Value);
                        return;
                    }

                    // TargetType.Creature
                    if (target.CreatureIndex is null)
                        throw new InvalidOperationException("Для цели-существа нужен CreatureIndex.");

                    if (target.CreatureIndex.Value >= targetPlayer.Board.Count)
                        throw new ArgumentOutOfRangeException(nameof(target.CreatureIndex), "Такого существа на столе нет.");

                    targetPlayer.Board[target.CreatureIndex.Value].TakeDamage(spell.Value);
                    return;

                case SpellType.Heal:
                    if (target.Type == TargetType.Player)
                    {
                        targetPlayer.Heal(spell.Value);
                        return;
                    }

                    if (target.CreatureIndex is null)
                        throw new InvalidOperationException("Для цели-существа нужен CreatureIndex.");

                    if (target.CreatureIndex.Value >= targetPlayer.Board.Count)
                        throw new ArgumentOutOfRangeException(nameof(target.CreatureIndex), "Такого существа на столе нет.");

                    targetPlayer.Board[target.CreatureIndex.Value].Heal(spell.Value);
                    return;

                case SpellType.BuffAttack:
                    if (target.Type != TargetType.Creature)
                        throw new InvalidOperationException("BuffAttack можно применять только к существу.");

                    if (target.CreatureIndex is null)
                        throw new InvalidOperationException("Для цели-существа нужен CreatureIndex.");

                    if (target.CreatureIndex.Value >= targetPlayer.Board.Count)
                        throw new ArgumentOutOfRangeException(nameof(target.CreatureIndex), "Такого существа на столе нет.");

                    targetPlayer.Board[target.CreatureIndex.Value].ChangeAttack(spell.Value);
                    return;

                default:
                    throw new InvalidOperationException("Неизвестный тип заклинания.");
            }
        }
    }
}
