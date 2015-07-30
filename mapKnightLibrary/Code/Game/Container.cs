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
		public List<Chest> chestContainer;

		public Container ()
		{
			platformContainer = new List<Platform> ();	
			jumpPadContainer = new List<JumpPad> ();
			chestContainer = new List<Chest> ();

			CrossLog.Log (this, "Created a new Instance of Container", MessageType.Debug);
		}
	}
}

