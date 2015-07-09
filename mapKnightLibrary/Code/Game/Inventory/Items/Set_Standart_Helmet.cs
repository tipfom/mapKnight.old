using System;
using System.Collections.Generic;

using mapKnightLibrary.Inventory;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Items{
		public class Set_Standart_Helmet : IArmor
		{
			static CCSpriteSheet SetSheet = new CCSpriteSheet("character/set_standart.plist");
			List<CCSpriteFrame> HelmetWalkSprites;
			List<CCSpriteFrame> HelmetJumpSprites;
			List<CCSpriteFrame> HelmetSlideSprites;
			List<CCSpriteFrame> HelmetFallSprites;

			Dictionary<Inventory.Attribute,short> HelmetAttributes;
			Dictionary<PlayerMovingType, CCAnimate> HelmetAnimations;
			Dictionary<PlayerMovingType, CCPoint> RealHelmetAnimationPositions;
			Dictionary<PlayerMovingType, CCPoint> HelmetAnimationPositions;

			public Set_Standart_Helmet ()
			{
				HelmetAttributes = new Dictionary<mapKnightLibrary.Inventory.Attribute, short> ();
				HelmetAttributes.Add (Inventory.Attribute.PhysicalArmor, 7);
				HelmetAttributes.Add (Inventory.Attribute.MagicArmor, 2);
				HelmetAttributes.Add (Inventory.Attribute.LifeRegeneration, 3);
				HelmetAttributes.Add (Inventory.Attribute.ManaRegeneration, 1);
				HelmetAttributes.Add (Inventory.Attribute.Mana, 8);
				HelmetAttributes.Add (Inventory.Attribute.Health, 16);
				HelmetAttributes.Add (Inventory.Attribute.Jump, 0);
				HelmetAttributes.Add (Inventory.Attribute.Speed, 0);
				HelmetAttributes.Add (Inventory.Attribute.Strenght, 14);
				HelmetAttributes.Add (Inventory.Attribute.Intelligence, 8);

				//load Frames
				HelmetWalkSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_walk"));
				HelmetJumpSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_jump"));
				HelmetSlideSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_slide"));
				HelmetFallSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_fall"));

				//init Animations
				HelmetAnimations = new Dictionary<PlayerMovingType, CCAnimate> ();
				HelmetAnimations.Add (PlayerMovingType.Running, new CCAnimate (new CCAnimation (HelmetWalkSprites, 0.05f)));
				HelmetAnimations.Add (PlayerMovingType.Jumping, new CCAnimate (new CCAnimation (HelmetJumpSprites, 0.05f)));
				HelmetAnimations.Add (PlayerMovingType.Sliding, new CCAnimate (new CCAnimation (HelmetSlideSprites, 0.05f)));
				HelmetAnimations.Add (PlayerMovingType.Falling, new CCAnimate (new CCAnimation (HelmetFallSprites, 0.05f)));

				//init anim Positions
				RealHelmetAnimationPositions = new Dictionary<PlayerMovingType, CCPoint> ();
				RealHelmetAnimationPositions.Add (PlayerMovingType.Running, new CCPoint (9, 10));
				RealHelmetAnimationPositions.Add (PlayerMovingType.Jumping, new CCPoint (28, 30));
				RealHelmetAnimationPositions.Add (PlayerMovingType.Sliding, new CCPoint (0, 10));
				RealHelmetAnimationPositions.Add (PlayerMovingType.Falling, new CCPoint (41, 10));
				HelmetAnimationPositions = RealHelmetAnimationPositions;
			}

			#region IEquipable implementation

			public EquipSlot EquipSlot {
				get {
					return EquipSlot.Helmet;
				}
			}

			public Dictionary<Inventory.Attribute, short> AttributeChange {
				get {
					return HelmetAttributes;
				}
			}

			#endregion

			#region IArmor implementation

			public float Health { get; set; }

			public Dictionary<PlayerMovingType, CCAnimate> ArmorAnimations {
				get {
					return HelmetAnimations;
				}
			}

			public Dictionary<PlayerMovingType, CCPoint> ArmorAnimationPosition {
				get {
					return HelmetAnimationPositions;
				}
			}

			public CCSpriteFrame StandingFrame {
				get {
					return HelmetWalkSprites [0];
				}
			}

			public void PreScale (float Scale) {
				ArmorAnimationPosition [PlayerMovingType.Running] = new CCPoint (ArmorAnimationPosition [PlayerMovingType.Running].X * Scale, ArmorAnimationPosition [PlayerMovingType.Running].Y * Scale);
				ArmorAnimationPosition [PlayerMovingType.Jumping] = new CCPoint (ArmorAnimationPosition [PlayerMovingType.Jumping].X * Scale, ArmorAnimationPosition [PlayerMovingType.Jumping].Y * Scale);
				ArmorAnimationPosition [PlayerMovingType.Sliding] = new CCPoint (ArmorAnimationPosition [PlayerMovingType.Sliding].X * Scale, ArmorAnimationPosition [PlayerMovingType.Sliding].Y * Scale);
				ArmorAnimationPosition [PlayerMovingType.Falling] = new CCPoint (ArmorAnimationPosition [PlayerMovingType.Falling].X * Scale, ArmorAnimationPosition [PlayerMovingType.Falling].Y * Scale);
			}

			#endregion

			#region IItem implementation

			public string name {
				get {
					return "Standart Armor Helmet";
				}
			}

			public string ID {
				get {
					return "SAH";
				}
			}

			public CCTexture2D PreviewImage {
				get {
					return HelmetWalkSprites [0].Texture;
				}
			}

			public short Cost {
				get {
					return -1;
				}
			}

			public float StackCount {
				get {
					return 1;
				}
			}

			#endregion
		}
	}
}

