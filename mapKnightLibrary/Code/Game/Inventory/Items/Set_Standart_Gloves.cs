using System;
using System.Collections.Generic;

using mapKnightLibrary.Inventory;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Items{
		public class Set_Standart_Glove : IArmor
		{
			static CCSpriteSheet SetSheet = new CCSpriteSheet("character/set_standart.plist");
			List<CCSpriteFrame> GloveWalkSprites;
			List<CCSpriteFrame> GloveJumpSprites;
			List<CCSpriteFrame> GloveSlideSprites;
			List<CCSpriteFrame> GloveFallSprites;

			Dictionary<Inventory.Attribute,short> GloveAttributes;
			Dictionary<PlayerMovingType, CCAnimate> GloveAnimations;
			Dictionary<PlayerMovingType, CCPoint> RealGloveAnimationPositions;
			Dictionary<PlayerMovingType, CCPoint> GloveAnimationPositions;

			public Set_Standart_Glove ()
			{
				GloveAttributes = new Dictionary<mapKnightLibrary.Inventory.Attribute, short> ();
				GloveAttributes.Add (Inventory.Attribute.PhysicalArmor, 4);
				GloveAttributes.Add (Inventory.Attribute.MagicArmor, 1);
				GloveAttributes.Add (Inventory.Attribute.LifeRegeneration, 0);
				GloveAttributes.Add (Inventory.Attribute.ManaRegeneration, 2);
				GloveAttributes.Add (Inventory.Attribute.Mana, 6);
				GloveAttributes.Add (Inventory.Attribute.Health, 4);
				GloveAttributes.Add (Inventory.Attribute.Jump, 0);
				GloveAttributes.Add (Inventory.Attribute.Speed, 1);
				GloveAttributes.Add (Inventory.Attribute.Strenght, 3);
				GloveAttributes.Add (Inventory.Attribute.Intelligence, 2);

				//load Frames
				GloveWalkSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_walk"));
				GloveJumpSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_jump"));
				GloveSlideSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_slide"));
				GloveFallSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_fall"));

				//init Animations
				GloveAnimations = new Dictionary<PlayerMovingType, CCAnimate> ();
				GloveAnimations.Add (PlayerMovingType.Running, new CCAnimate (new CCAnimation (GloveWalkSprites, 0.05f)));
				GloveAnimations.Add (PlayerMovingType.Jumping, new CCAnimate (new CCAnimation (GloveJumpSprites, 0.05f)));
				GloveAnimations.Add (PlayerMovingType.Sliding, new CCAnimate (new CCAnimation (GloveSlideSprites, 0.05f)));
				GloveAnimations.Add (PlayerMovingType.Falling, new CCAnimate (new CCAnimation (GloveFallSprites, 0.05f)));

				//init anim Positions
				RealGloveAnimationPositions = new Dictionary<PlayerMovingType, CCPoint> ();
				RealGloveAnimationPositions.Add (PlayerMovingType.Running, new CCPoint (0, 109));
				RealGloveAnimationPositions.Add (PlayerMovingType.Jumping, new CCPoint (0, 128));
				RealGloveAnimationPositions.Add (PlayerMovingType.Sliding, new CCPoint (45, 81));
				RealGloveAnimationPositions.Add (PlayerMovingType.Falling, new CCPoint (0, 113));
				GloveAnimationPositions = RealGloveAnimationPositions;
			}

			#region IEquipable implementation

			public EquipSlot EquipSlot {
				get {
					return EquipSlot.Gloves;
				}
			}

			public Dictionary<Inventory.Attribute, short> AttributeChange {
				get {
					return GloveAttributes;
				}
			}

			#endregion

			#region IArmor implementation

			public float Health { get; set; }

			public Dictionary<PlayerMovingType, CCAnimate> ArmorAnimations {
				get {
					return GloveAnimations;
				}
			}

			public Dictionary<PlayerMovingType, CCPoint> ArmorAnimationPosition {
				get {
					return GloveAnimationPositions;
				}
			}

			public CCSpriteFrame StandingFrame {
				get {
					return GloveWalkSprites [0];
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
					return "Standart Armor Gloves";
				}
			}

			public string ID {
				get {
					return "SAG";
				}
			}

			public CCTexture2D PreviewImage {
				get {
					return GloveWalkSprites [0].Texture;
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

