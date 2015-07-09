using System;
using Microsoft.Xna.Framework;

using CocosSharp;

using Android.App;
using Android.Content.PM;
using Android.OS;

using mapKnightLibrary;
using mapKnightLibrary.Inventory;
using Android.Views;

namespace mapKnight
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		string app_version;
		ControlType RunningControlType;

		GameScene gameScene ;
		CCWindow runningWindow;

		mapKnightLibrary.Container gameContainer;

		public event EventHandler ApplicationFinishedLaunching;

		public GameAppDelegate (string version, ControlType ControlType){
			app_version = version;
			RunningControlType = ControlType;
		}

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
			// This tells the application to not use antialiasing which can 
			// improve the performance of your game.

			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("landscape");
			application.ContentSearchPaths.Add ("menu");
			application.ContentSearchPaths.Add ("interface");
			application.ContentSearchPaths.Add ("particle");

			runningWindow = mainWindow;
			runningWindow.DisplayStats = true;
			runningWindow.StatsScale = 3;
			// Get the resolution of the main window...
			var bounds = mainWindow.WindowSizeInPixels;

			CCScene.SetDefaultDesignResolution (bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);

			//startScene = new StartScene (mainWindow);
			gameScene = new GameScene (mainWindow, RunningControlType);

			// startet das erste Fenster
			mainWindow.RunWithScene (gameScene);
			//startScene.Version = app_version;
			//startScene.startGame += startGame;
			//if (ApplicationFinishedLaunching != null)
			//	ApplicationFinishedLaunching (this, EventArgs.Empty);
		}

		private void startGame()
		{
			runningWindow.DefaultDirector.RunWithScene (gameScene);
		}
	}
}