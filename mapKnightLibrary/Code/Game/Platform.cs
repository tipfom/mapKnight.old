using System;
using System.Collections.Generic;

using CocosSharp;

using Box2D.Common;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Dynamics;

namespace mapKnightLibrary
{
	public class Platform : CCSprite 
	{
		private static float SpriteScale = 3f;

		CCSize wayToMove;
		float progressionX, progressionY;
		int speed;

		int CurrentWaypoint;
		List<CCPoint> Waypoints;

		b2Body platformBody;

		public static CCSize PlatformSize { 
			get { 
				CCSprite TempSprite = new CCSprite ("platform");
				TempSprite.Scale = SpriteScale;
				return new CCSize (TempSprite.ScaledContentSize.Width, TempSprite.ScaledContentSize.Height);
			} 
		}

		public Platform (List<CCPoint> platformWaypoints, int platformSpeed, Container gameContainer)
		{
			this.Texture = new CCTexture2D("platform");
			this.Scale = SpriteScale;
			this.IsAntialiased = false;

			this.Position = platformWaypoints [0];
			Waypoints = platformWaypoints;
			speed = platformSpeed;
			//umso geringer der speed umso schneller die platform

			CurrentWaypoint = 0;
			wayToMove = new CCSize (Waypoints [CurrentWaypoint + 1].X - Waypoints [CurrentWaypoint].X, Waypoints [CurrentWaypoint + 1].Y - Waypoints [CurrentWaypoint].Y);
		
			//box2d
			b2BodyDef platformDef = new b2BodyDef ();
			platformDef.type = b2BodyType.b2_kinematicBody;
			platformDef.position = new b2Vec2 (Waypoints[CurrentWaypoint].X / PhysicsHandler.pixelPerMeter, Waypoints[CurrentWaypoint].Y / PhysicsHandler.pixelPerMeter);
			platformBody = gameContainer.physicsHandler.gameWorld.CreateBody (platformDef);

			b2PolygonShape platformShape = new b2PolygonShape ();
			platformShape.SetAsBox ((float)this.ScaledContentSize.Width / PhysicsHandler.pixelPerMeter / 2, (float)this.ScaledContentSize.Height / PhysicsHandler.pixelPerMeter / 2);

			b2FixtureDef platformFixture = new b2FixtureDef ();
			platformFixture.shape = platformShape;
			platformFixture.density = 0.0f; //Dichte
			platformFixture.restitution = 0f; //Rückprall
			platformFixture.userData = WorldFixtureData.platform;
			platformBody.CreateFixture (platformFixture);
			//

			this.Position = new CCPoint (platformBody.Position.x * PhysicsHandler.pixelPerMeter, platformBody.Position.y * PhysicsHandler.pixelPerMeter);

			progressionX = wayToMove.Width/ (float)speed;
			progressionY =  wayToMove.Height/(float)speed ;
			if (float.IsInfinity (progressionX))
				progressionX = 0;
			if (float.IsInfinity (progressionY))
				progressionY = 0;
			b2Vec2 Velocity = platformBody.LinearVelocity;
			Velocity.y = progressionY;
			Velocity.x = progressionX;
			platformBody.LinearVelocity = Velocity;
		}

		public void Move(){
			if (wayToMove.Width < 0 && this.Position.X < Waypoints [CurrentWaypoint + 1].X || wayToMove.Width > 0 && this.Position.X > Waypoints [CurrentWaypoint + 1].X || wayToMove.Height < 0 && this.Position.Y < Waypoints [CurrentWaypoint + 1].Y || wayToMove.Height > 0 && this.Position.Y > Waypoints [CurrentWaypoint + 1].Y) {
				CurrentWaypoint++;
				if (CurrentWaypoint >= Waypoints.Count - 1) {
					CurrentWaypoint = 0;
					Waypoints.Reverse ();
				}
				wayToMove = new CCSize (Waypoints [CurrentWaypoint + 1].X - Waypoints [CurrentWaypoint].X, Waypoints [CurrentWaypoint + 1].Y - Waypoints [CurrentWaypoint].Y);

				progressionX = wayToMove.Width / (float)speed;
				progressionY = wayToMove.Height / (float)speed;
				if (float.IsInfinity (progressionX))
					progressionX = 0;
				if (float.IsInfinity (progressionY))
					progressionY = 0;
				b2Vec2 Velocity = platformBody.LinearVelocity;
				Velocity.y = progressionY;
				Velocity.x = progressionX;
				platformBody.LinearVelocity = Velocity;
				this.Position = new CCPoint (platformBody.Position.x * PhysicsHandler.pixelPerMeter - this.ScaledContentSize.Width / 2, platformBody.Position.y * PhysicsHandler.pixelPerMeter - this.ScaledContentSize.Height / 2);
			} else {
				this.Position = new CCPoint (platformBody.Position.x * PhysicsHandler.pixelPerMeter - this.ScaledContentSize.Width / 2, platformBody.Position.y * PhysicsHandler.pixelPerMeter - this.ScaledContentSize.Height / 2);
			}
		}
	}
}