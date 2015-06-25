using System;

using System.Collections.Generic;

namespace mapKnightLibrary
{
	public class Container
	{
		public Character mainCharacter;
		public PhysicsHandler physicsHandler;
		public List<Platform> platformContainer;
		public List<JumpPad> jumpPadContainer;

		public Container ()
		{
			platformContainer = new List<Platform> ();	
			jumpPadContainer = new List<JumpPad> ();
		}
	}
}

