using System;
using System.Threading;

using Box2D.Dynamics;
using Box2D.Common;

using CocosSharp;

namespace mapKnightLibrary
{
	public class WallJumpManager
	{
		public bool OnJump{ get; private set;}

		public delegate void BodyJumpEventHandler(JumpEventHandler e);
		public event BodyJumpEventHandler JumpEnded;

		public b2Body jumpBody{ private get; set;}

		float maxTime, time;
		CCSize jumpSize;
		int jumpModifier;

		bool AbortX;

		public Direction CurrentJumpingDirection{ get; private set;}

		public WallJumpManager (b2Body parentJumpBody, float JumpTimeNeeded, float JumpHeight, float JumpWidth)
		{

			jumpBody = parentJumpBody;
			maxTime = JumpTimeNeeded;
			jumpSize = new CCSize (JumpWidth, JumpHeight);

			CurrentJumpingDirection = Direction.Left;
		}

		public void EndJump()
		{
			if (OnJump == true) {
				time = 0f;
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

		public void StartJump(Direction jumpDirection, Direction wallPosition)
		{
			if (jumpDirection == wallPosition) {
				if (jumpDirection == Direction.Left)
					jumpModifier = 1;
				else
					jumpModifier = -1;
				CurrentJumpingDirection = jumpDirection;
				OnJump = true;
			} else {
				b2Vec2 Velocity = jumpBody.LinearVelocity;
				Velocity.y = jumpSize.Height;
				Velocity.x = jumpSize.Width;
				jumpBody.LinearVelocity = Velocity;
			}
		} 

		public void Tick(float frameTime)
		{
			if (OnJump) {
				time += frameTime;

				b2Vec2 Velocity = jumpBody.LinearVelocity;
				Velocity.y = jumpSize.Height - jumpSize.Height * time / maxTime;
				if (AbortX == false) {
					if (Velocity.y < jumpSize.Height - jumpSize.Height * maxTime / 2) {
						Velocity.x = jumpModifier * -jumpSize.Width * (time - maxTime / 2) /  maxTime;
					} else {
						Velocity.x = jumpModifier * jumpSize.Width * time / maxTime;
					}
				}
				jumpBody.LinearVelocity = Velocity;

				if (time > maxTime) {
					EndJump ();
				}
			}
		}
	}

	public class JumpEventHandler 
	{
		public JumpEventHandler(){
			
		}
	}

	public enum Axis{
		x = 0,
		y = 1
	}
}

