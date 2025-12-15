using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardGame.Core.Engine;
using CardGame.Core.Game;
using CardGame.Core.Cards;
using System;

namespace CardGame.Core.Tests
{
	[TestClass]
	public class EngineTests
	{
		private GameEngine engine;

		[TestInitialize]
		public void Setup()
		{
			engine = new GameEngine();
		}

		[TestMethod]
		public void DamageSpell_ReducesCreatureHealth()
		{
			var state = engine.NewGame();
			state.Players[1].Board.Add(new CreatureCard("Test", 1, 2, 4));

			var spell = new SpellCard("Fireball", 0, SpellType.Damage, 3);
			state.Players[0].Hand.Add(spell);

			engine.PlayCard(state, state.Players[0].Hand.Count - 1, Target.Creature(1, 0));

			Assert.AreEqual(1, state.Players[1].Board[0].Health);
		}

		[TestMethod]
		public void DamageSpell_RemovesCreature_IfHealthZeroOrLess()
		{
			var state = engine.NewGame();
			state.Players[1].Board.Add(new CreatureCard("Test", 1, 2, 3));

			var spell = new SpellCard("Fireball", 0, SpellType.Damage, 5);
			state.Players[0].Hand.Add(spell);

			engine.PlayCard(state, state.Players[0].Hand.Count - 1, Target.Creature(1, 0));

			Assert.AreEqual(0, state.Players[1].Board.Count);
		}

		[TestMethod]
		public void HealSpell_RestoresCreatureHealth()
		{
			var state = engine.NewGame();
			var testCreature = new CreatureCard("Test", 1, 2, 4);
			testCreature.TakeDamage(2);
			state.Players[1].Board.Add(testCreature);

			var spell = new SpellCard("Heal", 0, SpellType.Heal, 2);
			state.Players[0].Hand.Add(spell);

			engine.PlayCard(state, state.Players[0].Hand.Count - 1, Target.Creature(1, 0));

			Assert.AreEqual(4, state.Players[1].Board[0].Health);
		}

		[TestMethod]
		public void BuffSpell_IncreasesCreatureAttack()
		{
			var state = engine.NewGame();
			state.Players[1].Board.Add(new CreatureCard("Test", 1, 2, 4));

			var spell = new SpellCard("Rage", 0, SpellType.BuffAttack, 3);
			state.Players[0].Hand.Add(spell);

			engine.PlayCard(state, state.Players[0].Hand.Count - 1, Target.Creature(1, 0));

			Assert.AreEqual(5, state.Players[1].Board[0].Attack);
		}

		[TestMethod]
		public void DamageSpell_ReducesPlayerHealth()
		{
			var state = engine.NewGame();
			var original = state.Players[1].Health;

			var spell = new SpellCard("Fireball", 0, SpellType.Damage, 3);
			state.Players[0].Hand.Add(spell);

			engine.PlayCard(state, state.Players[0].Hand.Count - 1, Target.Player(1));

			Assert.AreEqual(original - 3, state.Players[1].Health);
		}

		[TestMethod]
		public void EndTurn_ChangesCurrentPlayer()
		{
			var state = engine.NewGame();
			var current = state.CurrentPlayerIndex;

			engine.EndTurn(state);

			Assert.AreNotEqual(current, state.CurrentPlayerIndex);
		}

		[TestMethod]
		public void CannotPlayCard_IfNotEnoughMana()
		{
			var state = engine.NewGame();
			state.Players[0].Mana = 0;

			var expensive = new CreatureCard("Expensive", 5, 5, 5);
			state.Players[0].Hand.Add(expensive);

			Assert.ThrowsException<InvalidOperationException>(() =>
				engine.PlayCard(state, state.Players[0].Hand.Count - 1, null));
		}
	}
}
