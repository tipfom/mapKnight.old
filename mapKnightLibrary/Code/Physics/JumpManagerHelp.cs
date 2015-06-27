using System;

using Box2D.Common;

using CocosSharp;

namespace mapKnightLibrary
{
	public struct ClimbJumpConfig
	{
		public float timeNeeded;
		public CCSize jumpSize;
	}

	public struct WallJumpConfig
	{
		public b2Vec2 jumpImpuls;
		public float jumpTickCount;
		public float jumpOnXDecrease;
	}
}

