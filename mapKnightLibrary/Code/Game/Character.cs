using System;

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

		//Variable Eigenschaften
		int CurrentLife{ get; set;}
		int CurrentMana{ get; set;}
		Direction MoveDirection{ get; set;}
		CCPoint Position{ get; set;}
		bool Jump{ get; set;}

		//statische Eigenschaften
		int MaxLife{ get;}
		int MaxMana{ get;}
		CCSprite Sprite{ get;}
		CCSize Size{ get; }

		//Events
		event EventHandler<StatisticChangeEventArgHandler> StatChanged;
	}

	public enum Direction{
		Left,
		Right,
		None
	}

	public enum Statistic
	{
		Mana,
		Life
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

