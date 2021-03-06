﻿using System;
using System.Collections.Generic;

using CocosSharp;

using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

using mapKnightLibrary.Inventory;

namespace mapKnightLibrary
{
	public class RoboBob : Character
	{
		//statische eigenschaften
		static int WallSlideSpeed = 2;
		static float Scale = 0.47f;
		//statische grafische variablen
		static CCSpriteSheet CharacterSprites = new CCSpriteSheet("character/character.plist");

		static List<CCSpriteFrame> CharacterWalkSprites = CharacterSprites.Frames.FindAll ((frame) => frame.TextureFilename.StartsWith ("walk"));
		static CCAnimation CharacterWalkAnimation = new CCAnimation (CharacterWalkSprites, 0.05f);
		static CCRepeatForever CharacterWalkRepeat = new CCRepeatForever (new CCAnimate (CharacterWalkAnimation));

		static List<CCSpriteFrame> CharacterSlideSprites = CharacterSprites.Frames.FindAll((frame) => frame.TextureFilename.StartsWith("slide"));
		static CCAnimation CharacterSlideAnimation = new CCAnimation (CharacterSlideSprites, 0.05f);
		static CCRepeatForever CharacterSlideRepeat = new CCRepeatForever (new CCAnimate (CharacterSlideAnimation));

		static List<CCSpriteFrame> CharacterJumpSprites = CharacterSprites.Frames.FindAll((frame) => frame.TextureFilename.StartsWith("jump"));
		static CCAnimation CharacterJumpAnimation = new CCAnimation (CharacterJumpSprites, 0.05f);
		static CCAnimate CharacterJumpAnimate = new CCAnimate (CharacterJumpAnimation);

		static List<CCSpriteFrame> CharacterFallSprites = CharacterSprites.Frames.FindAll((frame) => frame.TextureFilename.StartsWith("fall"));
		static CCAnimation CharacterFallAnimation = new CCAnimation (CharacterFallSprites, 0.05f);
		static CCRepeatForever CharacterFallkRepeat = new CCRepeatForever (new CCAnimate (CharacterFallAnimation));

		static CCRect CharacterStandingTextureRect = CharacterSprites.Frames.Find ((frame) => frame.TextureFilename.StartsWith ("walk0")).TextureRectInPixels;
		static bool CharacterStandingTextureRotated = CharacterSprites.Frames.Find ((frame) => frame.TextureFilename.StartsWith ("walk0")).IsRotated;
		//variablen
		CCSize CharacterSize;
		float CharSizeHeight;
		float CharSizeWidth;

		CCSprite internHelmetSprite, internChestplateSprite, internGloveSprite, internShoesSprite;
		CCLayer internCharacterLayer;
		CCPoint CharacterPosition;

		IArmor Helmet, Chestplate, Gloves, Shoes;

		b2Body characterBody;

		int Life, Mana;

		float JumpTimeCount;
		PlayerMovingType MoveType;
		public PlayerMovingType CurrentMovingType { 
			get { return MoveType; }
			private set {
				if (value != MoveType && JumpTimeCount <= 0) {
					MoveType = value;
					if (value == PlayerMovingType.Jumping) {
						JumpTimeCount = CharacterJumpAnimation.Duration;
					}
					if (MovingTypeChanged != null)
						MovingTypeChanged (this, value);
				}
			}
		}

		public CCSprite HelmetSprite {
			get {
				return internHelmetSprite;
			}
		}

		public CCSprite ChestplateSprite {
			get {
				return internChestplateSprite;
			}
		}

		public CCSprite GloveSprite {
			get {
				return internGloveSprite;
			}
		}

		public CCSprite ShoesSprite {
			get {
				return internShoesSprite;
			}
		}

		public CCLayer CharacterLayer {
			get {
				return internCharacterLayer;
			}
		}

		private	event EventHandler<PlayerMovingType> MovingTypeChanged;
		private event EventHandler<Direction> DirectionChanged;

		JumpManager JumpManager;
		bool Jumping, ClimbJump;
		bool DoubleJump;
		bool AvoidPlatformGlitch;

		bool Character.Jump{ get{return Jumping;} set {Jumping = value;}}

		PhysicsHandler physicsHandler;

		public int CurrentLife {
			get { return Life; }
			set {
				Life = value;
				StatChanged (this, new StatisticChangeEventArgHandler (Statistic.Life));
			}
		}

		public int CurrentMana {
			get { return Mana; }
			set { 
				Mana = value;
				StatChanged (this, new StatisticChangeEventArgHandler (Statistic.Mana));
			}
		}

		Dictionary<Inventory.Attribute,short> staticAttributes = new Dictionary<mapKnightLibrary.Inventory.Attribute, short> ();
		Dictionary<Inventory.Attribute,short> internAttributes = new Dictionary<mapKnightLibrary.Inventory.Attribute, short>();
		public Dictionary<Inventory.Attribute, short> Attributes {
			get {
				return internAttributes;
			}
		}

		public event EventHandler<StatisticChangeEventArgHandler> StatChanged;

		public RoboBob (GameInventory gameInventory)
		{

			//Übertragen der Rüstung
			Helmet = (IArmor)gameInventory.EquipedItems [EquipSlot.Helmet];
			Chestplate = (IArmor)gameInventory.EquipedItems [EquipSlot.Chestplate];
			Gloves = (IArmor)gameInventory.EquipedItems [EquipSlot.Gloves];
			Shoes = (IArmor)gameInventory.EquipedItems [EquipSlot.Shoes];

			#region staticCharacterAttributes
			//static CharacterAttributes
			staticAttributes.Add (Inventory.Attribute.Health, 35);
			staticAttributes.Add (Inventory.Attribute.Intelligence, 10);
			staticAttributes.Add (Inventory.Attribute.Jump, 600);
			staticAttributes.Add (Inventory.Attribute.LifeRegeneration, 1);
			staticAttributes.Add (Inventory.Attribute.MagicArmor, 6);
			staticAttributes.Add (Inventory.Attribute.Mana, 15);
			staticAttributes.Add (Inventory.Attribute.ManaRegeneration, 1);
			staticAttributes.Add (Inventory.Attribute.PhysicalArmor, 30);
			staticAttributes.Add (Inventory.Attribute.Speed, 400);
			staticAttributes.Add (Inventory.Attribute.Strenght, 17);
			UpdateAttributes (new Dictionary<Inventory.Attribute, short>[] {
				Helmet.AttributeChange,
				Chestplate.AttributeChange,
				Gloves.AttributeChange,
				Shoes.AttributeChange
			});
			#endregion


			internHelmetSprite = new CCSprite (){ ScaleY = Scale, ScaleX = Scale };
			internGloveSprite = new CCSprite (){ ScaleY = Scale, ScaleX = Scale };
			internChestplateSprite = new CCSprite (){ ScaleY = Scale, ScaleX = Scale };
			internShoesSprite = new CCSprite (){ ScaleY = Scale, ScaleX = Scale };
			internCharacterLayer = new CCLayer () { };
			this.CharacterSize = new CCSize (Gloves.StandingFrame.ContentSize.Width * Scale, Gloves.ArmorAnimationPosition [PlayerMovingType.Running].Y * Scale + Gloves.StandingFrame.ContentSize.Height * Scale);
			CharSizeHeight = CharacterSize.Height / 2;
			CharSizeWidth = CharacterSize.Width / 2;

			CharacterLayer.AddChild( ChestplateSprite);
			CharacterLayer.AddChild (GloveSprite);
			CharacterLayer.AddChild (ShoesSprite);
			CharacterLayer.AddChild (HelmetSprite);

			Helmet.PreScale (Scale);
			Chestplate.PreScale (Scale);
			Gloves.PreScale (Scale);
			Shoes.PreScale (Scale);


			DoubleJump = true;

			DirectionChanged += UpdateSprite;
			MovingTypeChanged += UpdateSprite;

			CurrentMovingType = PlayerMovingType.Running;
			MoveDirection = Direction.None;

			JumpManager = new JumpManager (this.characterBody, 
				new ClimbJumpConfig () { jumpSize = new CCSize (6f, 17f), timeNeeded = 0.3f }, 
				new WallJumpConfig () { jumpImpuls = new b2Vec2 (6f, 15f), jumpTickCount = 20f, jumpOnXDecrease = 0.1f });
		}
	
		CCPoint Character.Position {
			get { return CharacterPosition; } 
			set { CharacterPosition = value; }
		}

		CCSize Character.Size {
			get {
				return CharacterSize;
			}
		}

		bool Character.bindToPhysicsHandler(PhysicsHandler parentPhysicsHandler)
		{
			physicsHandler = parentPhysicsHandler; 
			//erstellt den Body und bindet ihn an den Jumpmanager
			JumpManager.jumpBody = this.createPhysicsBody (physicsHandler.gameWorld);
			return true;
		}

		public void UpdateAttributes (Dictionary<Inventory.Attribute, short>[] Attributes = null)
		{
			this.internAttributes = staticAttributes;
			if (Attributes != null) {
				foreach (Dictionary<Inventory.Attribute,short> AttributeDictionary in Attributes) {
					foreach (var Attribute in AttributeDictionary.Keys) {
						this.internAttributes [Attribute] += AttributeDictionary [Attribute];
					}
				}
			}

			CrossLog.Log (this, "Updated Attributes", MessageType.Debug);
			foreach (KeyValuePair<Inventory.Attribute,short> AttributePair in internAttributes) {
				CrossLog.Log (this, "\t" + AttributePair.Key.ToString () + " = " + AttributePair.Value.ToString (), MessageType.Info);
			}
		}

		void Character.Update(float frameTime)
		{
			if (physicsHandler != null) {
				if (CurrentMovingType == PlayerMovingType.Jumping)
					JumpTimeCount -= frameTime;
				//Immer wenn die Jump Animation läuft wird der TickCoun

				CharacterPosition = new CCPoint (characterBody.Position.x * PhysicsHandler.pixelPerMeter, characterBody.Position.y * PhysicsHandler.pixelPerMeter);

				internCharacterLayer.Position = new CCPoint (CharacterPosition.X, CharacterPosition.Y - HelmetSprite.ScaledContentSize.Height / 4);
				if (ChestplateSprite.ScaleX > 0) {
					HelmetSprite.Position = new CCPoint (-CharSizeWidth + Helmet.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Helmet.ArmorAnimationPosition [CurrentMovingType].Y - HelmetSprite.ScaledContentSize.Height / 2);
					ChestplateSprite.Position = new CCPoint (-CharSizeWidth + Chestplate.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Chestplate.ArmorAnimationPosition [CurrentMovingType].Y - ChestplateSprite.ScaledContentSize.Height / 2);
					GloveSprite.Position = new CCPoint (-CharSizeWidth + Gloves.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Gloves.ArmorAnimationPosition [CurrentMovingType].Y - GloveSprite.ScaledContentSize.Height / 2);
					ShoesSprite.Position = new CCPoint (-CharSizeWidth + Shoes.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Shoes.ArmorAnimationPosition [CurrentMovingType].Y - ShoesSprite.ScaledContentSize.Height / 2);
				} else {
					HelmetSprite.Position = new CCPoint (CharSizeWidth - Helmet.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Helmet.ArmorAnimationPosition [CurrentMovingType].Y - HelmetSprite.ScaledContentSize.Height / 2);
					ChestplateSprite.Position = new CCPoint (CharSizeWidth - Chestplate.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Chestplate.ArmorAnimationPosition [CurrentMovingType].Y - ChestplateSprite.ScaledContentSize.Height / 2);
					GloveSprite.Position = new CCPoint (CharSizeWidth - Gloves.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Gloves.ArmorAnimationPosition [CurrentMovingType].Y - GloveSprite.ScaledContentSize.Height / 2);
					ShoesSprite.Position = new CCPoint (CharSizeWidth - Shoes.ArmorAnimationPosition [CurrentMovingType].X,
						CharSizeHeight - Shoes.ArmorAnimationPosition [CurrentMovingType].Y - ShoesSprite.ScaledContentSize.Height / 2);
				}

				b2Vec2 Velocity = characterBody.LinearVelocity;

				//handling custom ground properties
				switch (physicsHandler.collusionSensor.playerGround) {
				case WorldFixtureData.platform:
					
					switch (MoveDirection) {
					case Direction.Left:
						Velocity.x = physicsHandler.collusionSensor.playerGroundVelocity.x - (float)this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						break;
					case Direction.Right:
						Velocity.x = physicsHandler.collusionSensor.playerGroundVelocity.x + (float)this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;

						break;
					case Direction.None:
						Velocity.x = physicsHandler.collusionSensor.playerGroundVelocity.x;
						break;
					default:
						Velocity.x = physicsHandler.collusionSensor.playerGroundVelocity.x;
						break;
					}

					if (AvoidPlatformGlitch == false) {
						Velocity.y = physicsHandler.collusionSensor.playerGroundVelocity.y;
						if (Jumping == true && physicsHandler.collusionSensor.playerCanJump == true) {
							Velocity.y += this.Attributes [Inventory.Attribute.Jump] / PhysicsHandler.pixelPerMeter;

							AvoidPlatformGlitch = true;
							CurrentMovingType = PlayerMovingType.Jumping;
						}
					} else
						AvoidPlatformGlitch = false;

					characterBody.LinearVelocity = Velocity;

					DoubleJump = true;
					ClimbJump = true;

					JumpManager.EndJump ();

					CurrentMovingType = PlayerMovingType.Running;

					break;
				case WorldFixtureData.air:
					
					if (physicsHandler.collusionSensor.WallContact == MoveDirection && MoveDirection != Direction.None && JumpManager.OnJump == false) {
						Velocity.y = -WallSlideSpeed / PhysicsHandler.pixelPerMeter;
						CurrentMovingType = PlayerMovingType.Sliding;
					} else if (JumpManager.OnJump == false) {
						CurrentMovingType = PlayerMovingType.Falling;
					}

					switch (MoveDirection) {
					case Direction.Left:
						if (physicsHandler.collusionSensor.WallContact == MoveDirection && MoveDirection != Direction.None && ClimbJump == true && JumpManager.OnJump == false) {
							JumpManager.StartJump (this.MoveDirection, JumpType.ClimbJump);
							CurrentMovingType = PlayerMovingType.Jumping;
							ClimbJump = false;
						} else if (JumpManager.OnJump == false) {
							if (MoveDirection != JumpManager.CurrentJumpingDirection)
								JumpManager.AbortAccelerationOn (Axis.x);
							Velocity.x = -(float)this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						}
						break;
					case Direction.Right:
						if (physicsHandler.collusionSensor.WallContact == MoveDirection && MoveDirection != Direction.None && ClimbJump == true && JumpManager.OnJump == false) {
							JumpManager.StartJump (this.MoveDirection, JumpType.ClimbJump);
							CurrentMovingType = PlayerMovingType.Jumping;
							ClimbJump = false;
						} else if (JumpManager.OnJump == false) {
							if (MoveDirection != JumpManager.CurrentJumpingDirection)
								JumpManager.AbortAccelerationOn (Axis.x);
							Velocity.x = (float)this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						}
						break;
					case Direction.None:
						if (JumpManager.OnJump == false)
							Velocity.x = 0;
						break;
					default:
						if (JumpManager.OnJump == false)
							Velocity.x = 0;
						break;
					}

					if (JumpManager.OnJump == false) {
						if (physicsHandler.collusionSensor.WallContact == Direction.None && Jumping == true && DoubleJump == true) {
							//Doppelsprung
							DoubleJump = false;
							Velocity.y = (float)this.Attributes [Inventory.Attribute.Jump] * 0.8f / PhysicsHandler.pixelPerMeter;
							CurrentMovingType = PlayerMovingType.Jumping;
						} else if (physicsHandler.collusionSensor.WallContact != Direction.None && physicsHandler.collusionSensor.WallContact == MoveDirection && Jumping == true) {
							JumpManager.StartJump (this.MoveDirection, JumpType.WallJump);
							CurrentMovingType = PlayerMovingType.Jumping;
							break;
						}
					}

					characterBody.LinearVelocity = Velocity;

					break;
				case WorldFixtureData.jumppad:
					
					switch (MoveDirection) {
					case Direction.Left:
						Velocity.x = -(float)this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						break;
					case Direction.Right:
						Velocity.x = (float)this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						break;
					case Direction.None:
						Velocity.x = 0;
						break;
					default:
						Velocity.x = 0;
						break;
					}

					characterBody.LinearVelocity = Velocity;

					if (Jumping == true && physicsHandler.collusionSensor.playerCanJump == true) {
						physicsHandler.collusionSensor.playerGroundJumpPad.ApplyImpulsTo (this.characterBody);
						CurrentMovingType = PlayerMovingType.Jumping;
					}

					DoubleJump = true;
					ClimbJump = true;

					JumpManager.EndJump ();

					CurrentMovingType = PlayerMovingType.Running;

					break;
				default:
					switch (MoveDirection) {
					case Direction.Left:
						Velocity.x = -this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						break;
					case Direction.Right:
						Velocity.x = this.Attributes [Inventory.Attribute.Speed] / PhysicsHandler.pixelPerMeter;
						break;
					case Direction.None:
						Velocity.x = 0;
						break;
					default:
						Velocity.x = 0;
						break;
					}

					if (Jumping == true && physicsHandler.collusionSensor.playerCanJump == true) {
						Velocity.y = (float)this.Attributes [Inventory.Attribute.Jump] / PhysicsHandler.pixelPerMeter;
						CurrentMovingType = PlayerMovingType.Jumping;
					}

					characterBody.LinearVelocity = Velocity;

					DoubleJump = true;
					ClimbJump = true;
				
					JumpManager.EndJump ();

					CurrentMovingType = PlayerMovingType.Running;
					break;
				}
				JumpManager.Tick (frameTime);
			}
		}

		public b2Body createPhysicsBody (b2World bodyWorld)
		{
			//definiert den physikalischen körper des Männchens
			if (characterBody == null) {
				b2BodyDef characterDef = new b2BodyDef ();
				characterDef.type = b2BodyType.b2_dynamicBody;
				characterDef.position = new b2Vec2 (CharacterPosition.X / PhysicsHandler.pixelPerMeter, CharacterPosition.Y / PhysicsHandler.pixelPerMeter);
				characterBody = bodyWorld.CreateBody (characterDef);

				b2PolygonShape characterShape = new b2PolygonShape ();
				characterShape.SetAsBox ((float)CharacterSize.Width / PhysicsHandler.pixelPerMeter / 2, (float)CharacterSize.Height / PhysicsHandler.pixelPerMeter / 2);

				b2FixtureDef characterFixture = new b2FixtureDef ();
				characterFixture.shape = characterShape;
				characterFixture.density = 0.0f; //Dichte
				characterFixture.friction = 0f;
				characterFixture.restitution = 0f; //Rückprall
				characterBody.CreateFixture (characterFixture);

				b2PolygonShape characterGroundSensorShape = new b2PolygonShape ();
				b2Vec2 GroundSensorPosition = new b2Vec2 (0f, -0.51f * CharacterSize.Height / PhysicsHandler.pixelPerMeter);
				characterGroundSensorShape.SetAsBox (((float)CharacterSize.Width - 1f) / PhysicsHandler.pixelPerMeter / 2, 5f / PhysicsHandler.pixelPerMeter, GroundSensorPosition, 0f);

				//untergrundsensor
				b2FixtureDef groundSensor = new b2FixtureDef ();
				groundSensor.isSensor = true;
				groundSensor.userData = WorldFixtureData.playergroundsensor;
				groundSensor.shape = characterGroundSensorShape;
				groundSensor.density = 0.0f; //Dichte
				groundSensor.friction = 0f;
				groundSensor.restitution = 0f; //Rückprall
				characterBody.CreateFixture (groundSensor);

				b2PolygonShape characterLeftSensorShape = new b2PolygonShape ();
				b2Vec2 LeftSensorPosition = new b2Vec2 (-0.45f * CharacterSize.Width / PhysicsHandler.pixelPerMeter, -0.14f * CharacterSize.Height / PhysicsHandler.pixelPerMeter);
				characterLeftSensorShape.SetAsBox ((float)5f / PhysicsHandler.pixelPerMeter, CharacterSize.Height / PhysicsHandler.pixelPerMeter / 4, LeftSensorPosition, 0f);

				//leftsensor
				b2FixtureDef leftSensor = new b2FixtureDef ();
				leftSensor.isSensor = true;
				leftSensor.userData = WorldFixtureData.playerleftsensor;
				leftSensor.shape = characterLeftSensorShape;
				leftSensor.density = 0.0f; //Dichte
				leftSensor.friction = 0f;
				leftSensor.restitution = 0f; //Rückprall
				characterBody.CreateFixture (leftSensor);

				b2PolygonShape characterRightSensorShape = new b2PolygonShape ();
				b2Vec2 RightSensorPosition = new b2Vec2 (0.45f * CharacterSize.Width / PhysicsHandler.pixelPerMeter, -0.14f * CharacterSize.Height / PhysicsHandler.pixelPerMeter);
				characterRightSensorShape.SetAsBox ((float)5f / PhysicsHandler.pixelPerMeter, CharacterSize.Height / PhysicsHandler.pixelPerMeter / 4, RightSensorPosition, 0f);

				//rightsensor
				b2FixtureDef rightSensor = new b2FixtureDef ();
				rightSensor.isSensor = true;
				rightSensor.userData = WorldFixtureData.playerrightsensor;
				rightSensor.shape = characterRightSensorShape;
				rightSensor.density = 0.0f; //Dichte
				rightSensor.friction = 0f;
				rightSensor.restitution = 0f; //Rückprall
				characterBody.CreateFixture (rightSensor);

				CrossLog.Log (this, "Created Characterbody", MessageType.Debug);
			}
			CrossLog.Log (this, "Returned Characterbody", MessageType.Debug);
			return characterBody;
		}

		Direction internMoveDirection;
		public Direction MoveDirection {
			get{ return internMoveDirection; }
			set {
				if (internMoveDirection != value) {
					internMoveDirection = value;
					if (DirectionChanged != null)
						DirectionChanged (this, value);
				}
			}
		}

		private void UpdateSprite(object sender, Direction e)
		{
			switch (e) {
			case Direction.Left:
				if (ChestplateSprite.ScaleX > 0) {
					ChestplateSprite.ScaleX *= -1;
					HelmetSprite.ScaleX *= -1;
					GloveSprite.ScaleX *= -1;
					ShoesSprite.ScaleX *= -1;
				} else {
					ChestplateSprite.ScaleX *= 1;
					HelmetSprite.ScaleX *= 1;
					GloveSprite.ScaleX *= 1;
					ShoesSprite.ScaleX *= 1;
				}
				break;
			case Direction.Right:
				if (ChestplateSprite.ScaleX > 0) {
					ChestplateSprite.ScaleX *= 1;
					HelmetSprite.ScaleX *= 1;
					GloveSprite.ScaleX *= 1;
					ShoesSprite.ScaleX *= 1;
				} else {
					ChestplateSprite.ScaleX *= -1;
					HelmetSprite.ScaleX *= -1;
					GloveSprite.ScaleX *= -1;
					ShoesSprite.ScaleX *= -1;
				}
				break;
			}
			UpdateSprite (this, CurrentMovingType);
		}

		private void UpdateSprite(object sender, PlayerMovingType e)
		{
			HelmetSprite.StopAllActions ();
			ChestplateSprite.StopAllActions ();
			GloveSprite.StopAllActions ();
			ShoesSprite.StopAllActions ();
			CharSizeWidth = Gloves.ArmorAnimations [CurrentMovingType].Animation.Frames [0].SpriteFrame.ContentSize.Width / 2 * Scale;
			if (CurrentMovingType == PlayerMovingType.Running && MoveDirection == Direction.None) {
				HelmetSprite.IsTextureRectRotated = Helmet.StandingFrame.IsRotated;
				ChestplateSprite.IsTextureRectRotated = Chestplate.StandingFrame.IsRotated;
				GloveSprite.IsTextureRectRotated = Gloves.StandingFrame.IsRotated;
				ShoesSprite.IsTextureRectRotated = Shoes.StandingFrame.IsRotated;

				HelmetSprite.TextureRectInPixels = Helmet.StandingFrame.TextureRectInPixels;
				ChestplateSprite.TextureRectInPixels = Chestplate.StandingFrame.TextureRectInPixels;
				GloveSprite.TextureRectInPixels = Gloves.StandingFrame.TextureRectInPixels;
				ShoesSprite.TextureRectInPixels = Shoes.StandingFrame.TextureRectInPixels;
			} else {
				HelmetSprite.RepeatForever (new CCRepeatForever (Helmet.ArmorAnimations [CurrentMovingType]));
				ChestplateSprite.RepeatForever (new CCRepeatForever (Chestplate.ArmorAnimations [CurrentMovingType]));
				GloveSprite.RepeatForever (new CCRepeatForever (Gloves.ArmorAnimations [CurrentMovingType]));
				ShoesSprite.RepeatForever (new CCRepeatForever (Shoes.ArmorAnimations [CurrentMovingType]));
			}
		}
	}
}