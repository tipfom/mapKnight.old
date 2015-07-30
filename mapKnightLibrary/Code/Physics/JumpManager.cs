using System;
using System.Threading;

using Box2D.Dynamics;
using Box2D.Common;

using CocosSharp;

namespace mapKnightLibrary
{
	public class JumpManager
	{
		public bool OnJump{ get; private set;}


		public b2Body jumpBody{ private get; set;}

		float time, tick;
		int jumpModifier;

		bool AbortX;

		ClimbJumpConfig ClimbJumpConfig;
		WallJumpConfig WallJumpConfig;

		JumpType operatingJumpType;

		public Direction CurrentJumpingDirection{ get; private set;}

		public JumpManager (b2Body parentJumpBody, ClimbJumpConfig bodyClimbJumpConfig, WallJumpConfig bodyWallJumpConfig)
		{

			jumpBody = parentJumpBody;
		
			ClimbJumpConfig = bodyClimbJumpConfig;
			WallJumpConfig = bodyWallJumpConfig;

			CurrentJumpingDirection = Direction.Left;
		}

		public void EndJump()
		{
			if (OnJump == true) {
				#if LOGJUMP
				CrossLog.Log (this, "Ended Jump(" + operatingJumpType.ToString () + ") in direction " + CurrentJumpingDirection.ToString (), MessageType.Debug);
				#endif
				time = 0f;
				tick = 0f;
				OnJump = false;
				AbortX = false;
				//AbortY = false;
				b2Vec2 Velocity = jumpBody.LinearVelocity;
				Velocity.y = 0f;
				Velocity.x = 0f;
				jumpBody.LinearVelocity = Velocity;
			}
		}

		public void AbortAccelerationOn(Axis axis)
		{
			if (OnJump) {
				switch (axis) {
				case Axis.x:
					AbortX = true;
					break;
				case Axis.y:
					//AbortY = true;
					break;
				default:
					//nothing
					break;
				}
			}
		}

		public void StartJump(Direction jumpDirection, JumpType jumpType)
		{
			switch (jumpType) {
			case JumpType.ClimbJump:
				if (jumpDirection == Direction.Left)
					jumpModifier = 1;
				else
					jumpModifier = -1;
				CurrentJumpingDirection = jumpDirection;
				OnJump = true;
				break;
			case JumpType.WallJump:
				switch (jumpDirection) {
				case Direction.Right:
					jumpBody.LinearVelocity = new b2Vec2 (-WallJumpConfig.jumpImpuls.x, WallJumpConfig.jumpImpuls.y);

					break;
				case Direction.Left:
					jumpBody.LinearVelocity = new b2Vec2 (WallJumpConfig.jumpImpuls.x, WallJumpConfig.jumpImpuls.y);
					break;
				}
				OnJump = true;
				CurrentJumpingDirection = jumpDirection;
				break;
			}
			operatingJumpType = jumpType;

			#if LOGJUMP
			CrossLog.Log (this, "Began Jump(" + jumpType.ToString () + ") in direction " + jumpDirection.ToString (), MessageType.Debug);
			#endif
		}

		public void Tick(float frameTime)
		{
			if (OnJump) {
				b2Vec2 Velocity = jumpBody.LinearVelocity;
				switch (operatingJumpType) {
				case JumpType.ClimbJump:
					time += frameTime;

					Velocity.y = ClimbJumpConfig.jumpSize.Height - ClimbJumpConfig.jumpSize.Height * time / ClimbJumpConfig.timeNeeded;
					if (AbortX == false) {
						if (Velocity.y < ClimbJumpConfig.jumpSize.Height - ClimbJumpConfig.jumpSize.Height * ClimbJumpConfig.timeNeeded / 2) {
							Velocity.x = jumpModifier * -ClimbJumpConfig.jumpSize.Width * (time - ClimbJumpConfig.timeNeeded / 2) / ClimbJumpConfig.timeNeeded;
						} else {
							Velocity.x = jumpModifier * ClimbJumpConfig.jumpSize.Width * time / ClimbJumpConfig.timeNeeded;
						}
					}
					jumpBody.LinearVelocity = Velocity;

					if (time > ClimbJumpConfig.timeNeeded) {
						EndJump ();
					}
					break;
				case JumpType.WallJump:
					tick++;

					switch (CurrentJumpingDirection) {
					case Direction.Left:
						if (Velocity.x >= WallJumpConfig.jumpImpuls.x / 3)
							Velocity.x -= WallJumpConfig.jumpOnXDecrease;
						break;
					case Direction.Right:

						if (Velocity.x <= -WallJumpConfig.jumpImpuls.x / 3)
							Velocity.x += WallJumpConfig.jumpOnXDecrease;
						break;
					}

					jumpBody.LinearVelocity = Velocity;

					if (tick == WallJumpConfig.jumpTickCount) {
						EndJump ();
					}
					break;
				default:
					OnJump = false;
					break;
				}
			}
		}
	}
}