using System;
using System.Collections.Generic;

using mapKnightLibrary;

namespace mapKnight
{
	public class AndroidLogRegister : BasicLogRegister
	{
		Dictionary<object,AndroidLog> LogRegister;

		public AndroidLogRegister ()
		{
			LogRegister = new Dictionary<object, AndroidLog> ();
		}
	
		public override void Register(object target, string tag) {
			LogRegister.Add (target, new AndroidLog (tag));
		}
	
		public override BasicLog this[object sender] {
			get {
				if (LogRegister.ContainsKey (sender)) { 
					return LogRegister [sender];
				} else {
					throw new AccessViolationException (sender.ToString() + " tried to access an unregistered Log");
				}
			}
		}
	}

	public class AndroidLog : BasicLog
	{
		public string Tag{ get; private set; }

		public AndroidLog(string Tag){
			this.Tag = Tag;
		}

		public override void Log(string text){
			Android.Util.Log.Debug (this.Tag, text);
		}
	}
}