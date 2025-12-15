using System;
using CardGame.Core.Cards;
using CardGame.Core.Game;

namespace CardGame.Core.Engine
{
    /// Реализация игрового движка:
    /// создает игру, обрабатывает ходы и применяет эффекты карт.
    public sealed class GameEngine : IGameEngine
    {
        public GameState NewGame()
        {
            var state = new GameState();

            // Создаём двух игроков
            state.Players.Add(new PlayerState("Player 1"));
            state.Players.Add(new PlayerState("Player 2"));

            // Раздача стартовых карт
            DealStartingHands(state.Players[0]);
            DealStartingHands(state.Players[1]);

            state.CurrentPlayerIndex = 0;
            state.TurnNumber = 1;

            // Первый ход получает мана
            state.Players[0].StartTurn();

            return state;
        }

        private static void DealStartingHands(PlayerState player)
        {
            player.Hand.Add(new CreatureCard("Wolf", 1, 2, 2));
            player.Hand.Add(new SpellCard("Fireball", 2, SpellType.Damage, 3));
            player.Hand.Add(new SpellCard("Heal", 1, SpellType.Heal, 2));
        }

        public void PlayCard(GameState state, int handIndex, Target? target)
        {
            var player = state.Players[state.CurrentPlayerIndex];

            // Проверка корректности индекса карты
            if (handIndex < 0 || handIndex >= player.Hand.Count)
                throw new ArgumentOutOfRangeException(nameof(handIndex));

            var card = player.Hand[handIndex];

            // Проверка маны
            if (player.Mana < card.ManaCost)
                throw new InvalidOperationException("Недостаточно маны.");

            player.SpendMana(card.ManaCost);

            if (card is CreatureCard creature)
            {
                player.Board.Add(creature);
                player.Hand.RemoveAt(handIndex);
                return;
            }

            if (card is SpellCard spell)
            {
                if (target == null)
                    throw new InvalidOperationException("Для заклинания нужна цель.");

                ApplySpell(state, spell, target);
                player.Hand.RemoveAt(handIndex);
                return;
            }
        }

        public void EndTurn(GameState state)
        {
            // Переход к следующему игроку
            state.CurrentPlayerIndex = (state.CurrentPlayerIndex + 1) % state.Players.Count;
            state.TurnNumber++;
            state.Players[state.CurrentPlayerIndex].StartTurn();
        }

        private static void ApplySpell(GameState state, SpellCard spell, Target target)
        {
            if (target.PlayerIndex >= state.Players.Count)
                throw new ArgumentOutOfRangeException(nameof(target.PlayerIndex));

            var targetPlayer = state.Players[target.PlayerIndex];

            if (spell.Type == SpellType.Damage)
            {
                if (target.Type == TargetType.Player)
                {
                    targetPlayer.TakeDamage(spell.Value);
                }
                else if (target.CreatureIndex.HasValue)
                {
                    int idx = target.CreatureIndex.Value;
                    var creature = targetPlayer.Board[idx];

                    // Наносим урон
                    creature.TakeDamage(spell.Value);

                    // Если мёртв — удаляем
                    if (creature.IsDead)
                    {
                        targetPlayer.Board.RemoveAt(idx);
                    }
                }
            }
            else if (spell.Type == SpellType.Heal)
            {
                if (target.Type == TargetType.Player)
                    targetPlayer.Heal(spell.Value);
                else if (target.CreatureIndex.HasValue)
                    targetPlayer.Board[target.CreatureIndex.Value].Heal(spell.Value);
            }
            else if (spell.Type == SpellType.BuffAttack)
            {
                if (target.CreatureIndex.HasValue)
                    targetPlayer.Board[target.CreatureIndex.Value].ChangeAttack(spell.Value);
            }
        }
    }
}
