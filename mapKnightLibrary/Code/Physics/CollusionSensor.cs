using System;

using System.Linq;
using System.Collections.Generic;

using Box2D;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Rope;

namespace mapKnightLibrary
{
	public class CollusionSensor : b2ContactListener
	{
		public WorldFixtureData playerGround;
		public Direction WallContact;

		public b2Vec2 playerGroundVelocity { 
			get { 
				if (playerGroundBody != null)
					return playerGroundBody.LinearVelocity;
				else
					return new b2Vec2 (0, 0);
			}
		}

		b2Body playerGroundBody;
		public JumpPad playerGroundJumpPad;

		Container gameContainer;

		public CollusionSensor (Container GameContainer)
		{
			gameContainer = GameContainer;
		}

		public override void BeginContact (Box2D.Dynamics.Contacts.b2Contact contact)
		{
			base.BeginContact (contact);

			if (contact.FixtureA.UserData != null && contact.FixtureB.UserData != null) {
				switch ((WorldFixtureData)contact.FixtureA.UserData) {
				case WorldFixtureData.playergroundsensor:
					switch ((WorldFixtureData)contact.FixtureB.UserData) {
					case WorldFixtureData.ground:
						playerGround = WorldFixtureData.ground;
						break;
					case WorldFixtureData.platform:
						playerGround = WorldFixtureData.platform;
						playerGroundBody = contact.FixtureB.Body;
						break;
					case WorldFixtureData.jumppad:
						playerGround = WorldFixtureData.jumppad;
						playerGroundJumpPad = gameContainer.jumpPadContainer.First (search => search.JumpPadBody.Position == contact.FixtureB.Body.Position);
						break;
					default:
						playerGround = WorldFixtureData.air;
						break;
					}
					break;
				case WorldFixtureData.playerleftsensor:
					if ((WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.ground) {
						WallContact = Direction.Left;
					} else {
						WallContact = Direction.None;
					}
					break;
				case WorldFixtureData.playerrightsensor:
					if ((WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.ground) {
						WallContact = Direction.Right;
					} else {
						WallContact = Direction.None;
					}
					break;
				default:
					switch ((WorldFixtureData)contact.FixtureB.UserData) {
					case WorldFixtureData.playergroundsensor:
						switch ((WorldFixtureData)contact.FixtureA.UserData) {
						case WorldFixtureData.ground:
							playerGround = WorldFixtureData.ground;
							break;
						case WorldFixtureData.platform:
							playerGround = WorldFixtureData.platform;
							playerGroundBody = contact.FixtureA.Body;
							break;
						case WorldFixtureData.jumppad:
							playerGround = WorldFixtureData.jumppad;
							playerGroundJumpPad = gameContainer.jumpPadContainer.First (search => search.JumpPadBody.Position == contact.FixtureB.Body.Position);
							break;
						default:
							playerGround = WorldFixtureData.air;
							break;
						}
						break;
					case WorldFixtureData.playerleftsensor:
						if ((WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.ground) {
							WallContact = Direction.Left;
						} else {
							WallContact = Direction.None;
						}
						break;
					case WorldFixtureData.playerrightsensor:
						if ((WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.ground) {
							WallContact = Direction.Right;
						} else {
							WallContact = Direction.None;
						}
						break;
					}
					break;
				}
			}
		}

		public bool playerCanJump{get 
			{ 
				if (playerGround == WorldFixtureData.ground || playerGround == WorldFixtureData.platform || playerGround == WorldFixtureData.jumppad)
					return true;
				else
					return false;
			}
		}

		public override void EndContact (Box2D.Dynamics.Contacts.b2Contact contact)
		{
			if (contact.FixtureA.UserData != null && contact.FixtureB.UserData != null) {
				switch ((WorldFixtureData)contact.FixtureA.UserData) {
				case WorldFixtureData.playergroundsensor:
					if ((WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.platform) {
						playerGroundBody = null;
						playerGround = WorldFixtureData.air;
					} else if ((WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.jumppad) {
						playerGroundJumpPad = null;
						playerGround = WorldFixtureData.air;
					}
					else {
						playerGround = WorldFixtureData.air;
					}
					break;
				case WorldFixtureData.playerleftsensor:
					WallContact = Direction.None;
					break;
				case WorldFixtureData.playerrightsensor:
					WallContact = Direction.None;
					break;
				default:
					switch ((WorldFixtureData)contact.FixtureB.UserData) {
					case WorldFixtureData.playergroundsensor:
						if ((WorldFixtureData)contact.FixtureA.UserData == WorldFixtureData.platform) {
							playerGroundBody = null;
							playerGround = WorldFixtureData.air;
						} else if ((WorldFixtureData)contact.FixtureA.UserData == WorldFixtureData.jumppad) {
							playerGroundJumpPad = null;
							playerGround = WorldFixtureData.air;
						} else {
							playerGround = WorldFixtureData.air;
						}
						break;
					case WorldFixtureData.playerleftsensor:
						WallContact = Direction.None;
						break;
					case WorldFixtureData.playerrightsensor:
						WallContact = Direction.None;
						break;
					}
					break;
				}
				if ((WorldFixtureData)contact.FixtureA.UserData == WorldFixtureData.playergroundsensor && (WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.platform ||
					(WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.playergroundsensor && (WorldFixtureData)contact.FixtureA.UserData == WorldFixtureData.platform) {
						} else if ((WorldFixtureData)contact.FixtureA.UserData == WorldFixtureData.playergroundsensor || (WorldFixtureData)contact.FixtureB.UserData == WorldFixtureData.playergroundsensor) {
					}
			}

		}

		public override void PreSolve (Box2D.Dynamics.Contacts.b2Contact contact, b2Manifold oldManifold)
		{
			
		}

		public override void PostSolve (Box2D.Dynamics.Contacts.b2Contact contact, ref b2ContactImpulse impulse)
		{
			
		}
	}
}

