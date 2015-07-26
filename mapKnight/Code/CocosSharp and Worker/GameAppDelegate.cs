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

		public GameAppDelegate (string version, ControlType ControlType){
			app_version = version;
			RunningControlType = ControlType;
		}

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
<<<<<<< HEAD:mapKnight/Code/CocosSharp and Worker/GameAppDelegate.cs
=======
			// This tells the application to not use antialiasing which can 
			// improve the performance of your game.
>>>>>>> 49908d873f6d98cc8e79416728edeef90665b42b:mapKnight/Code/CocosSharp and Worker/GameAppDelegate.cs

			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("landscape");
			application.ContentSearchPaths.Add ("menu");
			application.ContentSearchPaths.Add ("interface");
			application.ContentSearchPaths.Add ("particle");

			runningWindow = mainWindow;
			runningWindow.DisplayStats = true;
			runningWindow.StatsScale = 3;
			// Get the resolution of the main window...
			var bounds = mainWindow.WindowSizeInPixels;
<<<<<<< HEAD:mapKnight/Code/CocosSharp and Worker/GameAppDelegate.cs
			
			//startScene = new StartScene (mainWindow);
			gameScene = new GameScene (mainWindow, RunningControlType, new AndroidLogRegister());
=======

			CCScene.SetDefaultDesignResolution (bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);

			//startScene = new StartScene (mainWindow);
			gameScene = new GameScene (mainWindow, RunningControlType);
>>>>>>> 49908d873f6d98cc8e79416728edeef90665b42b:mapKnight/Code/CocosSharp and Worker/GameAppDelegate.cs

			// startet das erste Fenster
			mainWindow.RunWithScene (gameScene);
		}

		private void startGame()
		{
			runningWindow.DefaultDirector.RunWithScene (gameScene);
		}
	}
}