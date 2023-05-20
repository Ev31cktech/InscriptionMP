﻿using System.Collections.Generic;

namespace Inscription_Server
{
	abstract class Card
	{
		public uint Health { get; private set; }
		public uint Power { get; private set; }
		public string Name { get; private set; }

		private List<Sigil> Sigils;
		public Card(string name, uint power, uint health,params Sigil[] sigils)
		{
			Sigils = new List<Sigil>(sigils);
			Health = health;
			Power = power;
			Name = name;
		}
		public virtual void OnAttack()
		{

		}
		public virtual void OnDamage()
		{

		}
		public virtual void OnSummon()
		{

		}
		public virtual void OnDeath()
		{

		}
		public virtual void OnBeginTurn()
		{

		}
		public virtual void OnSacrifice()
		{

		}
		public virtual void OnDraw()
		{

		}
		public virtual void OnAction()
		{

		}
	}
}
