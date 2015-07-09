using System;
using System.Collections.Generic;

using mapKnightLibrary.Inventory;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Items{
		public class Set_Standart_Chestplate : IArmor
		{
			static CCSpriteSheet SetSheet = new CCSpriteSheet("character/set_standart.plist");
			List<CCSpriteFrame> ChestplateWalkSprites;
			List<CCSpriteFrame> ChestplateJumpSprites;
			List<CCSpriteFrame> ChestplateSlideSprites;
			List<CCSpriteFrame> ChestplateFallSprites;

			Dictionary<Inventory.Attribute,short> ChestplateAttributes;
			Dictionary<PlayerMovingType, CCAnimate> ChestplateAnimations;
			Dictionary<PlayerMovingType, CCPoint> RealChestplateAnimationPositions;
			Dictionary<PlayerMovingType, CCPoint> ChestplateAnimationPositions;

			public Set_Standart_Chestplate ()
			{
				ChestplateAttributes = new Dictionary<mapKnightLibrary.Inventory.Attribute, short> ();
				ChestplateAttributes.Add (Inventory.Attribute.PhysicalArmor, 14);
				ChestplateAttributes.Add (Inventory.Attribute.MagicArmor, 3);
				ChestplateAttributes.Add (Inventory.Attribute.LifeRegeneration, 2);
				ChestplateAttributes.Add (Inventory.Attribute.ManaRegeneration, 1);
				ChestplateAttributes.Add (Inventory.Attribute.Mana, 5);
				ChestplateAttributes.Add (Inventory.Attribute.Health, 20);
				ChestplateAttributes.Add (Inventory.Attribute.Jump, 0);
				ChestplateAttributes.Add (Inventory.Attribute.Speed, 0);
				ChestplateAttributes.Add (Inventory.Attribute.Strenght, 17);
				ChestplateAttributes.Add (Inventory.Attribute.Intelligence, 2);

				//load Frames
				ChestplateWalkSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_walk"));
				ChestplateJumpSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_jump"));
				ChestplateSlideSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_slide"));
				ChestplateFallSprites = SetSheet.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("[" + this.ID + "]" + "_fall"));

				//init Animations
				ChestplateAnimations = new Dictionary<PlayerMovingType, CCAnimate> ();
				ChestplateAnimations.Add (PlayerMovingType.Running, new CCAnimate (new CCAnimation (ChestplateWalkSprites, 0.05f)));
				ChestplateAnimations.Add (PlayerMovingType.Jumping, new CCAnimate (new CCAnimation (ChestplateJumpSprites, 0.05f)));
				ChestplateAnimations.Add (PlayerMovingType.Sliding, new CCAnimate (new CCAnimation (ChestplateSlideSprites, 0.05f)));
				ChestplateAnimations.Add (PlayerMovingType.Falling, new CCAnimate (new CCAnimation (ChestplateFallSprites, 0.05f)));

				//init anim Positions
				RealChestplateAnimationPositions = new Dictionary<PlayerMovingType, CCPoint> ();
				RealChestplateAnimationPositions.Add (PlayerMovingType.Running, new CCPoint (22, 96));
				RealChestplateAnimationPositions.Add (PlayerMovingType.Jumping, new CCPoint (44, 109));
				RealChestplateAnimationPositions.Add (PlayerMovingType.Sliding, new CCPoint (43, 83));
				RealChestplateAnimationPositions.Add (PlayerMovingType.Falling, new CCPoint (58, 114));
				ChestplateAnimationPositions = RealChestplateAnimationPositions;
			}

			#region IEquipable implementation

			public EquipSlot EquipSlot {
				get {
					return EquipSlot.Chestplate;
				}
			}

			public Dictionary<Inventory.Attribute, short> AttributeChange {
				get {
					return ChestplateAttributes;
				}
			}

			#endregion

			#region IArmor implementation

			public float Health { get; set; }

			public Dictionary<PlayerMovingType, CCAnimate> ArmorAnimations {
				get {
					return ChestplateAnimations;
				}
			}

			public Dictionary<PlayerMovingType, CCPoint> ArmorAnimationPosition {
				get {
					return ChestplateAnimationPositions;
				}
			}

			public CCSpriteFrame StandingFrame {
				get {
					return ChestplateWalkSprites [0];
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
					return "Standart Armor Chestplate";
				}
			}

			public string ID {
				get {
					return "SAC";
				}
			}

			public CCTexture2D PreviewImage {
				get {
					return ChestplateWalkSprites [0].Texture;
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

