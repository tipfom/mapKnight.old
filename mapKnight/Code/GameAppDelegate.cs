using System;
using Microsoft.Xna.Framework;

using CocosSharp;

using Android.App;
using Android.Content.PM;
using Android.OS;

using mapKnightLibrary;
using Android.Views;

namespace mapKnight
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		string app_version;
		ControlType RunningControlType;

		GameScene gameScene ;
		OptionScene optionScene;
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
			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("landscape");
			application.ContentSearchPaths.Add ("menu");
			application.ContentSearchPaths.Add ("interface");
			application.ContentSearchPaths.Add ("particle");

			gameContainer = new mapKnightLibrary.Container ();
			gameContainer.mainCharacter= new RoboBob();
			gameContainer.physicsHandler = new PhysicsHandler ();	

			runningWindow = mainWindow;
			// This tells the application to not use antialiasing which can 
			// improve the performance of your game.

			// Get the resolution of the main window...
			var bounds = mainWindow.WindowSizeInPixels;

			////definieren der Windowgröße auf 1280x576 p (16:9)
			CCScene.SetDefaultDesignResolution (bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);

			//startScene = new StartScene (mainWindow);
			gameScene = new GameScene (mainWindow, gameContainer, RunningControlType);
			optionScene = new OptionScene (mainWindow);

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