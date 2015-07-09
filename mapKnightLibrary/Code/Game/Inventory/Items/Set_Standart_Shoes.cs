using System;
using System.Collections.Generic;

using mapKnightLibrary.Inventory;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Items{
		public class Set_Standart_Shoes : IArmor 
		{
			static CCSpriteSheet SetSheet = new CCSpriteSheet("character/set_standart.plist");
			List<CCSpriteFrame> ShoesWalkSprites;
			List<CCSpriteFrame> ShoesJumpSprites;
			List<CCSpriteFrame> ShoesSlideSprites;
			List<CCSpriteFrame> ShoesFallSprites;

			Dictionary<Inventory.Attribute,short> ShoesAttributes;
			Dictionary<PlayerMovingType, CCAnimate> ShoesAnimations;
			Dictionary<PlayerMovingType, CCPoint> RealShoesAnimationPositions;
			Dictionary<PlayerMovingType, CCPoint> ShoesAnimationPositions;

			public Set_Standart_Shoes ()
			{
				ShoesAttributes = new Dictionary<mapKnightLibrary.Inventory.Attribute, short> ();
				ShoesAttributes.Add (Inventory.Attribute.PhysicalArmor, 4);
				ShoesAttributes.Add (Inventory.Attribute.MagicArmor, 1);
				ShoesAttributes.Add (Inventory.Attribute.LifeRegeneration, 1);
				ShoesAttributes.Add (Inventory.Attribute.ManaRegeneration, 1);
				ShoesAttributes.Add (Inventory.Attribute.Mana, 2);
				ShoesAttributes.Add (Inventory.Attribute.Health, 6);
				ShoesAttributes.Add (Inventory.Attribute.Jump, 1);
				ShoesAttributes.Add (Inventory.Attribute.Speed, 2);
				ShoesAttributes.Add (Inventory.Attribute.Strenght, 1);
				ShoesAttributes.Add (Inventory.Attribute.Intelligence, 0);

				//load Frames
				ShoesWalkSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_walk"));
				ShoesJumpSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_jump"));
				ShoesSlideSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_slide"));
				ShoesFallSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_fall"));

				//init Animations
				ShoesAnimations = new Dictionary<PlayerMovingType, CCAnimate> ();
				ShoesAnimations.Add (PlayerMovingType.Running, new CCAnimate (new CCAnimation (ShoesWalkSprites, 0.05f)));
				ShoesAnimations.Add (PlayerMovingType.Jumping, new CCAnimate (new CCAnimation (ShoesJumpSprites, 0.2f)));
				ShoesAnimations.Add (PlayerMovingType.Sliding, new CCAnimate (new CCAnimation (ShoesSlideSprites, 0.05f)));
				ShoesAnimations.Add (PlayerMovingType.Falling, new CCAnimate (new CCAnimation (ShoesFallSprites, 0.05f)));

				//init anim Positions
				RealShoesAnimationPositions = new Dictionary<PlayerMovingType, CCPoint> ();
				RealShoesAnimationPositions.Add (PlayerMovingType.Running, new CCPoint (26, 133));
				RealShoesAnimationPositions.Add (PlayerMovingType.Jumping, new CCPoint (36, 150));
				RealShoesAnimationPositions.Add (PlayerMovingType.Sliding, new CCPoint (98, 83));
				RealShoesAnimationPositions.Add (PlayerMovingType.Falling, new CCPoint (61, 152));
				ShoesAnimationPositions = RealShoesAnimationPositions;
			}

			#region IEquipable implementation

			public EquipSlot EquipSlot {
				get {
					return EquipSlot.Shoes;
				}
			}

			public Dictionary<Inventory.Attribute, short> AttributeChange {
				get {
					return ShoesAttributes;
				}
			}

			#endregion

			#region IArmor implementation

			public float Health { get; set; }

			public Dictionary<PlayerMovingType, CCAnimate> ArmorAnimations {
				get {
					return ShoesAnimations;
				}
			}

			public Dictionary<PlayerMovingType, CCPoint> ArmorAnimationPosition {
				get {
					return ShoesAnimationPositions;
				}
			}

			public CCSpriteFrame StandingFrame {
				get {
					return ShoesWalkSprites [0];
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
					return "Standart Armor Shoes";
				}
			}

			public string ID {
				get {
					return "SAS";
				}
			}

			public CCTexture2D PreviewImage {
				get {
					return ShoesWalkSprites [0].Texture;
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

