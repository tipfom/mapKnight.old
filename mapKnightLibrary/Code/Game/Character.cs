using System;
using System.Collections.Generic;

using CocosSharp;
using Box2D.Dynamics;

namespace mapKnightLibrary
{
	public interface Character
	{
		//Alles Andere
		bool bindToPhysicsHandler(PhysicsHandler parentPhysicsHandler);
		b2Body createPhysicsBody(b2World bodyWorld);
		void Update(float frameTime);
		void UpdateAttributes (Dictionary<Inventory.Attribute,short>[] Attributes = null);

		//Variable Eigenschaften
		int CurrentLife{ get; set;}
		int CurrentMana{ get; set;}
		Direction MoveDirection{ get; set;}
		CCPoint Position{ get; set;}
		bool Jump{ get; set;}
		Dictionary <Inventory.Attribute, short> Attributes{ get; }

		CCLayer CharacterLayer{ get; }

		//statische Eigenschaften
		CCSize Size{ get; }

		//Events
		event EventHandler<StatisticChangeEventArgHandler> StatChanged;
	}


	public class StatisticChangeEventArgHandler : EventArgs
	{
		public Statistic Statistic{ get; private set;}

		public StatisticChangeEventArgHandler(Statistic ChangedStatistic)
		{
			Statistic = ChangedStatistic;
		}
	}
}

