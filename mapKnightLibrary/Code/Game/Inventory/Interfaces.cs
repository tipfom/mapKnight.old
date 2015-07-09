using System;
using System.Collections.Generic;

using CocosSharp;

using Box2D.Dynamics;
using Box2D.Common;

namespace mapKnightLibrary
{
	namespace Inventory
	{
		public interface IItem
		{
			string name{ get; }
			string ID { get; } //Muss einzigartig sein
			CCTexture2D PreviewImage { get; }
			short Cost { get; }
			float StackCount { get; }
		}

		public interface ICraftable
		{
			bool CanBeCombined(IItem[] ItemsToCraft);
			IItem CraftResult{ get; }
		}

		public interface IEquipable : IItem
		{
			EquipSlot EquipSlot{ get; }
			Dictionary<Attribute, short> AttributeChange{ get; } // statisch
		}

		public interface IConsumable : IItem
		{
			Dictionary<Attribute, short> AttributeChange{ get; } // Wenn es konsumiert wird
			float EffectTime{ get; } //0 = Ganze Runde
			bool AttributeChangeOverTime{ get; } //z.B. Manapotions
		}

		public interface IPotion : IConsumable
		{
			CCAnimation CharacterLookChange{ get; }
		}

		public interface IMoneyBag : IItem
		{
			CCAnimation ConsumeAnimation{ get; }
			float Value{ get; }
		}

		public interface IArmor : IEquipable
		{
			float Health{ get; set; }
			Dictionary<PlayerMovingType, CCAnimate> ArmorAnimations{ get; }
			Dictionary<PlayerMovingType, CCPoint> ArmorAnimationPosition{ get; }
			CCSpriteFrame StandingFrame{ get; }
			void PreScale (float Scale);
		}

		public interface IWeapon : IEquipable
		{
			float Damage{ get; }
			float Health{ get; }
			b2BodyDef Hitbox (Direction HitboxAllignment, float Scale, b2Vec2 Position);
			Dictionary<PlayerMovingType, CCAnimation> CharacterLookChange{ get; }
		}
	}
}