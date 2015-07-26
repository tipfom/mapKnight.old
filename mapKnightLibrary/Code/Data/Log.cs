using System;

namespace mapKnightLibrary
{
	public abstract class BasicLog
	{
		public virtual void Log(string text){
			
		}
	}

	public abstract class BasicLogRegister
	{
		public virtual void Register(object target, string tag){
			
		}

		public abstract BasicLog this[object sender]{ get; }
	}
}

