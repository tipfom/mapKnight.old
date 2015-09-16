using System;

using mapKnightLibrary;

using Android;

[assembly: Xamarin.Forms.Dependency (typeof (mapKnight.ALog))]

namespace mapKnight
{
	public class ALog : Java.Lang.Object, ILog
	{
		//ALog == AndroidLog

		#region ILog implementation

		public void Debug (string project, string tag, string message)
		{
			Android.Util.Log.Debug (project, "@ [" + tag + "] - " + message);
		}

		public void Error (string project, string tag, Exception ex)
		{
			Android.Util.Log.Error (project, "@ [" + tag + "] - " + ex.Message);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorSource = " + ex.Source);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorStack = " + ex.StackTrace);
		}

		public void Info (string project, string tag, string message)
		{
			Android.Util.Log.Info (project, "@ [" + tag + "] - " + message);
		}

		public void Warn (string project, string tag, string message)
		{
			Android.Util.Log.Warn (project, "@ [" + tag + "] - " + message);
		}

		public void WTF (string project, string tag, string message, Exception ex)
		{
			Android.Util.Log.Wtf (project, "@ [" + tag + "] - " + message);
			Android.Util.Log.Error (project, "@ [" + tag + "] - ErrorMessage " + ex.Message);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorSource = " + ex.Source);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorStack = " + ex.StackTrace);
		}

		#endregion

		#region shared members

		public static void _Debug (string project, string tag, string message)
		{
			Android.Util.Log.Debug (project, "@ [" + tag + "] - " + message);
		}

		public static void _Error (string project, string tag, Exception ex)
		{
			Android.Util.Log.Error (project, "@ [" + tag + "] - " + ex.Message);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorSource = " + ex.Source);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorStack = " + ex.StackTrace);
		}

		public static void _Info (string project, string tag, string message)
		{
			Android.Util.Log.Info (project, "@ [" + tag + "] - " + message);
		}

		public static void _Warn (string project, string tag, string message)
		{
			Android.Util.Log.Warn (project, "@ [" + tag + "] - " + message);
		}

		public static void _WTF (string project, string tag, string message, Exception ex)
		{
			Android.Util.Log.Wtf (project, "@ [" + tag + "] - " + message);
			Android.Util.Log.Error (project, "@ [" + tag + "] - ErrorMessage " + ex.Message);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorSource = " + ex.Source);
			Android.Util.Log.Info (project, "@ [" + tag + "] - ErrorStack = " + ex.StackTrace);
		}

		#endregion
	}
}

