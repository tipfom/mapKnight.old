using System;

using CocosSharp;

using Box2D.Common;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Dynamics;

namespace mapKnightLibrary
{
	public class JumpPad : CCSprite 
	{
		private static float SpriteScale = 5f;

		public b2Body JumpPadBody;

		b2Vec2 jumpImpuls;
		public int totalJumps{ get; private set;}

		public static CCSize JumpPadSize {
			get { 
				CCSprite TempSprite = new CCSprite ("jumppad");
				TempSprite.Scale = SpriteScale;
				return new CCSize (TempSprite.ScaledContentSize.Width, TempSprite.ScaledContentSize.Height / 2);
			}
		}

		public JumpPad (b2Vec2 JumpImpuls, CCPoint Position, b2World gameWorld)
		{
			this.Texture = new CCTexture2D ("jumppad");
			this.Scale = SpriteScale;
			this.Position = Position;
			this.IsAntialiased = false;

			jumpImpuls = JumpImpuls;
			totalJumps = 0;

			//box2d
			b2BodyDef jumpPadDef = new b2BodyDef ();
			jumpPadDef.type = b2BodyType.b2_kinematicBody;
			jumpPadDef.position = new b2Vec2 ((Position.X + this.ScaledContentSize.Width/2)/PhysicsHandler.pixelPerMeter, (Position.Y + this.ScaledContentSize.Height/4) / PhysicsHandler.pixelPerMeter);
			JumpPadBody = gameWorld.CreateBody (jumpPadDef);

			b2PolygonShape jumpPadShape = new b2PolygonShape ();
			jumpPadShape.SetAsBox ((float)this.ScaledContentSize.Width / PhysicsHandler.pixelPerMeter / 2, (float)this.ScaledContentSize.Height / PhysicsHandler.pixelPerMeter / 4);// /4 weil die hitbox nur die hälfte der textur ist

			b2FixtureDef jumpPadFixture = new b2FixtureDef ();
			jumpPadFixture.shape = jumpPadShape;
			jumpPadFixture.density = 0.0f; //Dichte
			jumpPadFixture.restitution = 0f; //Rückprall
			jumpPadFixture.friction = 0f;
			jumpPadFixture.userData = WorldFixtureData.jumppad;
			JumpPadBody.CreateFixture (jumpPadFixture);
			//
		}

		public void ApplyImpulsTo(b2Body target)
		{
			if (target != null)
				target.LinearVelocity = jumpImpuls;
		}
	}
}