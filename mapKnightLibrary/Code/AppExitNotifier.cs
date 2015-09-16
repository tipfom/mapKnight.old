using System;

namespace mapKnightLibrary
{
	public class AppExitNotifier
	{
		public static event EventHandler HandleBackKeyPressed;
		public static void AppBackKeyPressed (object sender) {
			if (HandleBackKeyPressed != null) {
				CrossLog.Log ("PortableLibrary", "ApplicationExitNotifier", "Handling Application-BackKey-Pressing", MessageType.Debug);
				HandleBackKeyPressed (sender, EventArgs.Empty);
			}
		}
	}

}

