using System;

namespace mapKnightLibrary
{
	public enum ControlType
	{
		Slide = 0,
		Button = 1
	}

	public enum Axis{
		x = 0,
		y = 1
	}

	public enum JumpType
	{
		WallJump = 0,
		ClimbJump = 1
	}

	public enum MapLayer
	{
		MainLayer,
		Decorations,
		Chests
	}

	public enum PlayerMovingType
	{
		Sliding = 3,
		Falling = 1,
		Running = 0,
		Jumping = 4
	}

	public enum Direction{
		Left,
		Right,
		None
	}

	public enum Statistic
	{
		Mana,
		Life
	}
}