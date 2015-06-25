using System;
using System.IO;

using mapKnightLibrary;

using Microsoft.Xna.Framework;

using CocosSharp;
using Box2D;

using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android;
using Android.Widget;
using Android.Views;


namespace mapKnight
{
	[Activity(
		Label = "mapKnight",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/heart",
		Theme = "@android:style/Theme.NoTitleBar",
		LaunchMode = LaunchMode.SingleInstance,
		NoHistory = true,
		ScreenOrientation = ScreenOrientation.SensorLandscape,
		ConfigurationChanges =  ConfigChanges.Keyboard | 
		ConfigChanges.KeyboardHidden)]
	
	public class Game : AndroidGameActivity
	{
		CCApplication gameApplication;

		public Game(){
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			gameApplication = new CCApplication ();
			//Initialisiert die Game Anwedung
			GameAppDelegate GameAppDelegate = new GameAppDelegate (GetString (Resource.String.app_version) + " Build = " + GetString (Resource.String.app_build),
				                                  (ControlType)Enum.Parse (typeof(ControlType), Intent.GetStringExtra ("string:CurrentControlType")));

			gameApplication.ApplicationDelegate = GameAppDelegate;

			gameApplication.StartGame ();
			HideNavBar ();
			SetContentView (gameApplication.AndroidContentView);
		}

		private void HideNavBar()
		{
			//versteckt die Navigationsleiste
			View decorView = Window.DecorView;
			int newUiOptions = (int)decorView.WindowSystemUiVisibility;

			newUiOptions |= (int)SystemUiFlags.Fullscreen;
			newUiOptions |= (int)SystemUiFlags.HideNavigation;
			newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

			decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
		}
	}
}

