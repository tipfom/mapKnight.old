using System;
using System.Collections.Generic;

using CocosSharp;

using Microsoft.Xna.Framework.Input;

namespace mapKnightLibrary
{
	public class StartScreen : CCScene
	{
		//Button Event
		public delegate void HandleSingleplayerStart(string args);
		public event HandleSingleplayerStart StartSingleplayer;

		CCLayer OnlyLayer;

		CCSprite SingleplayerButtonSprite;
		CCSprite MultiplayerButtonSprite;
		CCSprite LoginButtonSprite;
		CCLabel SingleplayerLabel;
		CCLabel MultiplayerLabel;
		CCLabel LoginButtonLabel;

		CCLabel VersionLabel;

		CCEventListenerTouchAllAtOnce ClickListener;
		CCSize screenSize;

		public StartScreen (CCWindow mainWindow) : base(mainWindow)
		{
			screenSize = mainWindow.WindowSizeInPixels;
			ClickListener = new CCEventListenerTouchAllAtOnce ();
			ClickListener.OnTouchesEnded += HandleTouchEnded;
			this.AddEventListener (ClickListener);

			OnlyLayer = new CCLayer ();

			SingleplayerButtonSprite = new CCSprite ();
			LoginButtonSprite = new CCSprite ();
			SingleplayerLabel = new CCLabel ("Singleplayer", SharedData.Instance.Get (ShareableInformation.standart_font), Convert.ToInt32 (SharedData.Instance.Get (ShareableInformation.standart_fontsize)));
			MultiplayerLabel = new CCLabel("Multiplayer", SharedData.Instance.Get (ShareableInformation.standart_font), Convert.ToInt32 (SharedData.Instance.Get (ShareableInformation.standart_fontsize)));
			LoginButtonLabel = new CCLabel ("Login", SharedData.Instance.Get (ShareableInformation.standart_font), Convert.ToInt32 (SharedData.Instance.Get (ShareableInformation.standart_fontsize)));

			VersionLabel = new CCLabel (SharedData.Instance.Get (ShareableInformation.application_name) + " " + SharedData.Instance.Get (ShareableInformation.application_version) + " Build " + SharedData.Instance.Get (ShareableInformation.application_build),
				SharedData.Instance.Get (ShareableInformation.standart_font), 20,CCLabelFormat.SystemFont);

			VersionLabel.Position = new CCPoint (VersionLabel.ContentSize.Width / 2 + 10, mainWindow.WindowSizeInPixels.Height- VersionLabel.ContentSize.Height);

			SingleplayerLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width / 4 * 3, mainWindow.WindowSizeInPixels.Height - SingleplayerLabel.ContentSize.Height / 2 - mainWindow.WindowSizeInPixels.Height/5);
			MultiplayerLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width/4*3, SingleplayerLabel.PositionY-MultiplayerLabel.ContentSize.Height - mainWindow.WindowSizeInPixels.Height/5);
			LoginButtonLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width/8*5, MultiplayerLabel.PositionY-LoginButtonLabel.ContentSize.Height - mainWindow.WindowSizeInPixels.Height/5);

			OnlyLayer.AddChild (SingleplayerButtonSprite);
			OnlyLayer.AddChild (SingleplayerLabel);
			OnlyLayer.AddChild (LoginButtonSprite);
			OnlyLayer.AddChild (LoginButtonLabel);
			OnlyLayer.AddChild (MultiplayerLabel);
			OnlyLayer.AddChild (VersionLabel);

			this.AddChild (OnlyLayer);

			Schedule (Loop);
		}

		private void Loop (float frametime) {
			if(GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).Buttons.Back == ButtonState.Pressed){
				AppExitNotifier.AppBackKeyPressed (this);
			}
		}

		protected override void Dispose (bool disposing)
		{
			this.RemoveAllListeners ();
			ClickListener.Dispose ();
			base.Dispose (disposing);
		}

		private void HandleTouchEnded (List<CCTouch> touches, CCEvent touchevent) {
			foreach (CCTouch touch in touches) {
				if (Math.Abs ((touch.LocationOnScreen.X - SingleplayerLabel.Position.X) + (screenSize.Height - touch.LocationOnScreen.Y - SingleplayerLabel.Position.Y)) <= SingleplayerLabel.ContentSize.Width / 2) {
					if (Math.Abs (-(touch.LocationOnScreen.X - SingleplayerLabel.Position.X) + (screenSize.Height - touch.LocationOnScreen.Y - SingleplayerLabel.Position.Y)) <= SingleplayerLabel.ContentSize.Height / 2) {
						if (StartSingleplayer != null)
							StartSingleplayer ("");
						else
							throw new MissingMemberException ("its not defined what the app should do, when the 'Singleplayer' button gets clicked");
					}
				}
			}
		}
	}
}