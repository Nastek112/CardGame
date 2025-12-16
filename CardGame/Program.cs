using System;
using CardGame.Core.Engine;
using CardGame.Core.Game;
using CardGame.Core.Cards;

namespace CardGame.App
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Добро пожаловать в карточную игру! ===");
            Console.Write("Введите имя Игрока 1: ");
            string p1 = Console.ReadLine() ?? "Player 1";
            Console.Write("Введите имя Игрока 2: ");
            string p2 = Console.ReadLine() ?? "Player 2";

            var engine = new GameEngine();
            var state = engine.NewGame();

            while (true)
            {
                var current = state.Players[state.CurrentPlayerIndex];
                Console.WriteLine("\n--------------------------------------");
                Console.WriteLine($"Ход: {current.Name} | Здоровье: {current.Health} | Мана: {current.Mana}");
                ShowBoard(state);
                ShowHand(current);

                Console.WriteLine("Введите индекс карты для розыгрыша (или -1 чтобы пропустить ход):");
                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Неверный ввод, попробуй снова...");
                    continue;
                }

                if (choice == -1)
                {
                    Console.WriteLine($"{current.Name} пропустил ход.");
                }
                else if (choice >= 0 && choice < current.Hand.Count)
                {
                    Target? target = null;
                    var card = current.Hand[choice];

                    if (card is SpellCard)
                    {
                        Console.WriteLine("Заклинание требует цель");
                        Console.Write("Введите 0 для Игрока, 1 для Существа: ");
                        int t = int.Parse(Console.ReadLine() ?? "0");

                        if (t == 0)
                        {
                            Console.Write("Введите игрока (0 или 1): ");
                            int pi = int.Parse(Console.ReadLine() ?? "0");
                            target = Target.Player(pi);
                        }
                        else
                        {
                            Console.Write("Введите игрока (0 или 1): ");
                            int pi = int.Parse(Console.ReadLine() ?? "0");
                            Console.Write("Введите индекс существа: ");
                            int ci = int.Parse(Console.ReadLine() ?? "0");
                            target = Target.Creature(pi, ci);
                        }
                    }

                    try
                    {
                        engine.PlayCard(state, choice, target);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка: " + ex.Message);
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный индекс!");
                    continue;
                }

                if (IsGameOver(state))
                {
                    Console.WriteLine("\n=== Игра окончена! ===");
                    break;
                }

                engine.EndTurn(state);
            }

            Console.WriteLine("Спасибо за игру!");
        }

        static void ShowBoard(GameState state)
        {
            for (int p = 0; p < state.Players.Count; p++)
            {
                Console.WriteLine($"\nСтол игрока {state.Players[p].Name}:");
                if (state.Players[p].Board.Count == 0)
                    Console.WriteLine("  (пусто)");
                else
                {
                    for (int i = 0; i < state.Players[p].Board.Count; i++)
                    {
                        var cr = state.Players[p].Board[i];
                        Console.WriteLine($"  [{i}] {cr.Name} HP:{cr.Health} ATK:{cr.Attack}");
                    }
                }
            }
        }

        static void ShowHand(PlayerState player)
        {
            Console.WriteLine("\nРука карт:");
            for (int i = 0; i < player.Hand.Count; i++)
            {
                Console.WriteLine($"  [{i}] {player.Hand[i]}");
            }
        }

        static bool IsGameOver(GameState state)
        {
            foreach (var p in state.Players)
            {
                if (p.Health <= 0)
                {
                    Console.WriteLine($"Игрок {p.Name} проиграл!");
                    return true;
                }
            }
            return false;
        }
    }
}
