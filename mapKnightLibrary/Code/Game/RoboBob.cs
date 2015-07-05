using System;
using System.Collections.Generic;

using CocosSharp;

using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

using mapKnightLibrary.Inventory;

namespace mapKnightLibrary
{
	public class RoboBob : CCLayer, Character
	{
		//statische eigenschaften
		static int MoveSpeed = 9;
		static int JumpSpeed = 10;
		static int WallSlideSpeed = 2;
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
		CCSprite CharacterSprite;
		CCPoint CharacterPosition;

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

		public RoboBob ()
		{
			//static CharacterAttributes
			staticAttributes.Add (Inventory.Attribute.Health, 35);
			staticAttributes.Add (Inventory.Attribute.Intelligence, 10);
			staticAttributes.Add (Inventory.Attribute.Jump, 30);
			staticAttributes.Add (Inventory.Attribute.LifeRegeneration, 1);
			staticAttributes.Add (Inventory.Attribute.MagicArmor, 6);
			staticAttributes.Add (Inventory.Attribute.Mana, 15);
			staticAttributes.Add (Inventory.Attribute.ManaRegeneration, 1);
			staticAttributes.Add (Inventory.Attribute.PhysicalArmor, 30);
			staticAttributes.Add (Inventory.Attribute.Speed, 6);
			staticAttributes.Add (Inventory.Attribute.Strenght, 17);
			UpdateAttributes ();

			CharacterSprite = new CCSprite (CharacterWalkSprites [0]);
			CharacterSprite.Scale = 0.5f;
			CharacterSize = CharacterSprite.ScaledContentSize;

			DoubleJump = true;

			DirectionChanged += UpdateSprite;
			MovingTypeChanged += UpdateSprite;

			CurrentMovingType = PlayerMovingType.Running;
			MoveDirection = Direction.None;

			JumpManager = new JumpManager (this.characterBody, 
				new ClimbJumpConfig () { jumpSize = new CCSize (6f, 17f), timeNeeded = 0.3f }, 
				new WallJumpConfig () { jumpImpuls = new b2Vec2 (6f, 15f), jumpTickCount = 20f, jumpOnXDecrease = 0.1f });
		}
	
		protected override void AddedToScene ()
		{
			base.AddedToScene ();
			this.AddChild (CharacterSprite);
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
		}

		void Character.Update(float frameTime)
		{
			if (physicsHandler != null) {
				if (CurrentMovingType == PlayerMovingType.Jumping)
					JumpTimeCount -= frameTime;
				//Immer wenn die Jump Animation läuft wird der TickCoun

				CharacterPosition = new CCPoint (characterBody.Position.x * PhysicsHandler.pixelPerMeter, characterBody.Position.y * PhysicsHandler.pixelPerMeter);
				CharacterSprite.Position = CharacterPosition;

				b2Vec2 Velocity = characterBody.LinearVelocity;

				//handling custom ground properties
				switch (physicsHandler.collusionSensor.playerGround) {
				case WorldFixtureData.platform:
					
					switch (MoveDirection) {
					case Direction.Left:
						Velocity.x = physicsHandler.collusionSensor.playerGroundVelocity.x - MoveSpeed;
						break;
					case Direction.Right:
						Velocity.x = physicsHandler.collusionSensor.playerGroundVelocity.x + MoveSpeed;
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
							Velocity.y += JumpSpeed;
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
						Velocity.y = -WallSlideSpeed;
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
							Velocity.x = -MoveSpeed;
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
							Velocity.x = MoveSpeed;
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
							DoubleJump = false;
							Velocity.y = JumpSpeed;
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
						Velocity.x = -MoveSpeed;
						break;
					case Direction.Right:
						Velocity.x = MoveSpeed;
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
						Velocity.x = -MoveSpeed;
						break;
					case Direction.Right:
						Velocity.x = MoveSpeed;
						break;
					case Direction.None:
						Velocity.x = 0;
						break;
					default:
						Velocity.x = 0;
						break;
					}

					if (Jumping == true && physicsHandler.collusionSensor.playerCanJump == true) {
						Velocity.y = JumpSpeed;
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
				b2Vec2 LeftSensorPosition = new b2Vec2 (-0.45f * CharacterSize.Width/ PhysicsHandler.pixelPerMeter, -0.14f * CharacterSize.Height/ PhysicsHandler.pixelPerMeter);
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
				b2Vec2 RightSensorPosition = new b2Vec2 (0.45f * CharacterSize.Width/ PhysicsHandler.pixelPerMeter, -0.14f * CharacterSize.Height/ PhysicsHandler.pixelPerMeter);
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
			}
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
				if (CharacterSprite.ScaleX > 0)
					CharacterSprite.ScaleX *= -1;
				else
					CharacterSprite.ScaleX *= 1;
				UpdateSprite (this, CurrentMovingType);
				break;
			case Direction.Right:
				if (CharacterSprite.ScaleX > 0)
					CharacterSprite.ScaleX *= 1;
				else
					CharacterSprite.ScaleX *= -1;
				UpdateSprite (this, CurrentMovingType);
				break;
			case Direction.None:
				switch (CurrentMovingType) {
				case PlayerMovingType.Running:
					CharacterSprite.StopAllActions ();
					if (CharacterSprite.IsTextureRectRotated != CharacterStandingTextureRotated)
						CharacterSprite.IsTextureRectRotated = CharacterStandingTextureRotated;
					CharacterSprite.TextureRectInPixels = CharacterStandingTextureRect;
					break;
				default:
					UpdateSprite (this, CurrentMovingType);
					break;}
				break;
			}
		}

		private void UpdateSprite(object sender, PlayerMovingType e)
		{
			CharacterSprite.StopAllActions ();
			switch (e) {
			case PlayerMovingType.Falling:
				CharacterSprite.RepeatForever (CharacterFallkRepeat);
				break;
			case PlayerMovingType.Running:
				if (MoveDirection != Direction.None) {
					CharacterSprite.RepeatForever (CharacterWalkRepeat);
				} else {
					CharacterSprite.StopAllActions ();
					if (!CharacterSprite.IsTextureRectRotated)
						CharacterSprite.IsTextureRectRotated = true;
					CharacterSprite.TextureRectInPixels = CharacterStandingTextureRect;
				}
				break;
			case PlayerMovingType.Jumping:
				CharacterSprite.RunAction (CharacterJumpAnimate);
				break;
			case PlayerMovingType.Sliding:
				CharacterSprite.RepeatForever (CharacterSlideRepeat);
				break;
			}
		}
	}
}