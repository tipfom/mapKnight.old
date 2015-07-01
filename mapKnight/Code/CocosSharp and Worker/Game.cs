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
		Theme = "@style/GameTheme",
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

			//SQLTEST
			AndroidSQLDataManager test = new AndroidSQLDataManager (Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), StaticData.databasepath));

			test.BeginRead ();
			test.GetOrCreate ("testkey", "testvalue");
			test.GetOrCreate ("teststringkey", "testvalue");
			test.GetOrCreate ("inttestkey", "testvalue");
			test.GetOrCreate ("inttestkey", 123);
			test.GetOrCreate ("inttest2key", 333);
			test.GetOrCreate ("mlg", 1337);
			int x = test.GetOrCreate ("mlg", 7);
			test.Set ("testkey", "haha");
			string y = test.GetOrCreate ("testkey", "default");
			string z = test.GetOrCreate ("inttestkey", "default");
			test.EndRead ();
			//END SQLTEST

			//AndroidXMLDataManager xmltest = new AndroidXMLDataManager (Assets.Open ("Config/values.xml"), "de-de");

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

