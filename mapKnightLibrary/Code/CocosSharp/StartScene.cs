using System;

using CocosSharp;

namespace mapKnightLibrary
{
	public class StartScene : CCScene
	{
		CCLabel NameLabel, CreatorLabel, VersionLabel, InfoLabel;
		CCSprite PlayButton;
		CCLayer mainLayer;
		CCEventListenerTouchAllAtOnce touchListener;

		public delegate void startGameVoid();
		public event startGameVoid startGame;

		public StartScene (CCWindow mainWindow) : base(mainWindow)
		{
			mainLayer = new CCLayer ();
			AddChild (mainLayer);

			PlayButton = new CCSprite ("logo");

			NameLabel = new CCLabel ("Map Knight - Alpha", "arial", 22);
			CreatorLabel = new CCLabel ("Created by tipfom and Exo", "arial", 22);
			VersionLabel = new CCLabel ("Version unspecified", "arial", 22);
			InfoLabel = new CCLabel ("Click to play", "arial", 22);

			PlayButton.ScaleX = mainWindow.WindowSizeInPixels.Width / PlayButton.ContentSize.Width;
			PlayButton.ScaleY = mainWindow.WindowSizeInPixels.Height / PlayButton.ContentSize.Height;
			PlayButton.Position = new CCPoint (PlayButton.ScaledContentSize.Width / 2, PlayButton.ScaledContentSize.Height / 2);


			NameLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width / 4 * 3, mainWindow.WindowSizeInPixels.Height / 2 - NameLabel.ContentSize.Height);
			CreatorLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width / 4 * 3, NameLabel.PositionY - CreatorLabel.ContentSize.Height - 30);
			VersionLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width / 4 * 3, CreatorLabel.PositionY - VersionLabel.ContentSize.Height - 30);
			InfoLabel.Position = new CCPoint (mainWindow.WindowSizeInPixels.Width / 4 * 3, VersionLabel.PositionY - InfoLabel.ContentSize.Height - 30);
		
			mainLayer.AddChild (PlayButton);
			mainLayer.AddChild (NameLabel);
			mainLayer.AddChild (CreatorLabel);
			mainLayer.AddChild (VersionLabel);
			mainLayer.AddChild (InfoLabel);


			touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = HandleTouchesEnded;
			AddEventListener (touchListener, this);
		}

		private void HandleTouchesEnded(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (startGame != null) {
					startGame ();
				}
			}
		}

		public string Version { set{ VersionLabel.Text = "Version : " + value; }}
	}
}