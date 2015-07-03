using System;
using System.Collections.Generic;

using CocosSharp;


namespace mapKnightLibrary
{
	public class GameScene : CCScene
	{
		MergedLayer GameLayer;
		CCSprite[] ManaSprite, LifeSprite;
		CCSprite JumpButton, MoveRightButton, MoveLeftButton;
		CCLabel FPSLabel;
		CCLabel MapNameLabel, MapVersionLabel, MapCreatorLabel;
		CCLayer InterfaceLayer;
		CCSize screenSize;

		Container gameContainer;
		ControlType CurrentControlType;

		ClickManager clickManager;

		public GameScene (CCWindow mainWindow, Container mainContainer, ControlType RunningControlType) : base(mainWindow)
		{
			CurrentControlType = RunningControlType;

			gameContainer = mainContainer;
			GameLayer = new MergedLayer (mainWindow, gameContainer);

			InterfaceLayer = new CCLayer ();

			this.AddChild (GameLayer);
		
			screenSize = mainWindow.WindowSizeInPixels;

			//Touchlistener Initialisierung
			clickManager = new ClickManager (screenSize, gameContainer);
			this.AddEventListener (clickManager, this);

			switch (RunningControlType) {
			case ControlType.Slide:
				break;
			case ControlType.Button:
				JumpButton = new CCSprite ("menu_arrow_jump");
				MoveLeftButton = new CCSprite ("menu_arrow");
				MoveRightButton = new CCSprite ("menu_arrow");

				JumpButton.Scale = screenSize.Width / 4 / JumpButton.TextureRectInPixels.Size.Width;
				MoveLeftButton.Scale = screenSize.Width / 6 / MoveLeftButton.TextureRectInPixels.Size.Width;
				MoveRightButton.Scale = screenSize.Width / 6 / MoveRightButton.TextureRectInPixels.Size.Width;

				JumpButton.Position = new CCPoint (screenSize.Width - JumpButton.ScaledContentSize.Width / 2, JumpButton.ScaledContentSize.Height / 2);
				MoveLeftButton.Position = new CCPoint (MoveLeftButton.ScaledContentSize.Width / 2, MoveLeftButton.ScaledContentSize.Height / 2);
				MoveRightButton.Position = new CCPoint (MoveLeftButton.PositionX + 20f + MoveRightButton.ScaledContentSize.Width, MoveLeftButton.ScaledContentSize.Height / 2);

				CCRotateTo LeftRotate = new CCRotateTo (0, 270f);
				MoveLeftButton.RunAction (LeftRotate);

				CCRotateTo RightRotate = new CCRotateTo (0, 90f);
				MoveRightButton.RunAction (RightRotate);

				InterfaceLayer.AddChild (JumpButton);
				InterfaceLayer.AddChild (MoveLeftButton);
				InterfaceLayer.AddChild (MoveRightButton);

				Clickable JumpButtonClickable = new Clickable (JumpButton.ScaledContentSize, JumpButton.Position, new CCSize (1000, 1000)){ };
				Clickable MoveLeftButtonClickable = new Clickable (MoveLeftButton.ScaledContentSize, MoveLeftButton.Position, new CCSize (1000, 1000)){ };
				Clickable MoveRightButtonClickable = new Clickable (MoveRightButton.ScaledContentSize, MoveRightButton.Position, new CCSize (1000, 1000)){ };

				JumpButtonClickable.ClickedEvent += JumpB_HandleButtonClicked;
				MoveLeftButtonClickable.ClickedEvent += LeftB_HandleButtonClicked;
				MoveRightButtonClickable.ClickedEvent += RightB_HandleButtonClicked;

				clickManager.AddObject (JumpButtonClickable);
				clickManager.AddObject (MoveLeftButtonClickable);
				clickManager.AddObject (MoveRightButtonClickable);
				break;
			}

			//Map Label
			MapNameLabel = new CCLabel ("Map Name : " + GameLayer.mapName, "arial", 22) {
				Color = new CCColor3B (255, 255, 255)
			};
			MapNameLabel.Position = new CCPoint (MapNameLabel.ContentSize.Width / 2, screenSize.Height - 10 - MapNameLabel.ContentSize.Height);
			MapCreatorLabel = new CCLabel ("Map Creator : " + GameLayer.mapCreator, "arial", 22) {
				Color = new CCColor3B (255, 255, 255)
			};
			MapCreatorLabel.Position = new CCPoint (MapCreatorLabel.ContentSize.Width / 2, MapNameLabel.Position.Y - 10 - MapCreatorLabel.ContentSize.Height);
			MapVersionLabel = new CCLabel ("Map Version : " + GameLayer.mapVersion, "arial", 22) {
				Color = new CCColor3B (255, 255, 255)
			};
			MapVersionLabel.Position = new CCPoint (MapVersionLabel.ContentSize.Width / 2, MapCreatorLabel.Position.Y - 10 - MapVersionLabel.ContentSize.Height);

			InterfaceLayer.AddChild (MapNameLabel);
			InterfaceLayer.AddChild (MapCreatorLabel);
			InterfaceLayer.AddChild (MapVersionLabel);

			//Interface
			ManaSprite = new CCSprite[2];
			LifeSprite = new CCSprite[2];

			ManaSprite [0] = new CCSprite ("batterie_full"){ IsAntialiased = false };
			ManaSprite [1] = new CCSprite ("batterie_empty"){ IsAntialiased = false };
			LifeSprite [0] = new CCSprite ("heart_full"){ IsAntialiased = false };
			LifeSprite [1] = new CCSprite ("heart_empty"){ IsAntialiased = false };

			ManaSprite [0].Scale = 6f;
			ManaSprite [1].Scale = 6f;
			LifeSprite [0].Scale = 2f;
			LifeSprite [1].Scale = 2f;

			LifeSprite [1].Position = new CCPoint (screenSize.Width - LifeSprite [1].ScaledContentSize.Width, screenSize.Height - LifeSprite [1].ScaledContentSize.Height);
			ManaSprite [1].Position = new CCPoint (screenSize.Width - ManaSprite [1].ScaledContentSize.Width, LifeSprite [1].Position.Y - ManaSprite [1].ScaledContentSize.Height - 10f);

			InterfaceLayer.AddChild (ManaSprite [1]);
			InterfaceLayer.AddChild (ManaSprite [0]);
			InterfaceLayer.AddChild (LifeSprite [1]);
			InterfaceLayer.AddChild (LifeSprite [0]);

			//FPS Label
			FPSLabel = new CCLabel ("Score: 0", "arial", 22);
			FPSLabel.Position = new CCPoint (FPSLabel.ContentSize.Width / 2, MapVersionLabel.Position.Y - 10 - FPSLabel.ContentSize.Height);
			FPSLabel.Color = new CCColor3B (255, 255, 255);
			InterfaceLayer.AddChild (FPSLabel);

			//Interface Update und Punktbindung
			this.InterfaceUpdate (null, new StatisticChangeEventArgHandler (Statistic.Life));
			this.InterfaceUpdate (null, new StatisticChangeEventArgHandler (Statistic.Mana));

			gameContainer.mainCharacter.StatChanged += InterfaceUpdate;

			gameContainer.mainCharacter.CurrentLife = 23;
			gameContainer.mainCharacter.CurrentMana = 4;

			this.AddChild (InterfaceLayer);

			foreach (Chest knownChest in gameContainer.chestContainer) {
				clickManager.AddObject (knownChest);
			}

			Schedule (GameLoop);
		}


		void InterfaceUpdate(object sender, StatisticChangeEventArgHandler e){
			switch (e.Statistic) {
			case Statistic.Life:
				LifeSprite [0].TextureRectInPixels = new CCRect (0,
					LifeSprite [0].Texture.ContentSizeInPixels.Height - LifeSprite [0].Texture.ContentSizeInPixels.Height * gameContainer.mainCharacter.CurrentLife / gameContainer.mainCharacter.MaxLife,
					LifeSprite [0].Texture.ContentSizeInPixels.Width,
					LifeSprite [0].Texture.ContentSizeInPixels.Height * gameContainer.mainCharacter.CurrentLife / gameContainer.mainCharacter.MaxLife);

				LifeSprite [0].ContentSize = LifeSprite [0].TextureRectInPixels.Size;
				LifeSprite [0].Position = new CCPoint (screenSize.Width - LifeSprite [0].ScaledContentSize.Width, LifeSprite [1].Position.Y - LifeSprite [1].ScaledContentSize.Height / 2 + LifeSprite [0].ScaledContentSize.Height / 2);

				break;
			case Statistic.Mana:
				ManaSprite [0].TextureRectInPixels = new CCRect (0,
					ManaSprite [0].Texture.ContentSizeInPixels.Height - ManaSprite [0].Texture.ContentSizeInPixels.Height * gameContainer.mainCharacter.CurrentLife / gameContainer.mainCharacter.MaxLife,
					ManaSprite [0].Texture.ContentSizeInPixels.Width,
					ManaSprite [0].Texture.ContentSizeInPixels.Height * gameContainer.mainCharacter.CurrentLife / gameContainer.mainCharacter.MaxLife);

				ManaSprite [0].ContentSize = ManaSprite [0].TextureRectInPixels.Size;
				ManaSprite [0].Position = new CCPoint (screenSize.Width - ManaSprite [0].ScaledContentSize.Width, ManaSprite [1].Position.Y - ManaSprite [1].ScaledContentSize.Height / 2 + ManaSprite [0].ScaledContentSize.Height / 2);

				break;
			}
		}

		void GameLoop(float frameTime)
		{
			GameLayer.CenterCamera ();
			FPSLabel.Text = Math.Round ((1 / frameTime), 1).ToString () + " fps";
			FPSLabel.PositionX = FPSLabel.ContentSize.Width / 2;

			gameContainer.physicsHandler.step (frameTime);
			gameContainer.mainCharacter.Update (frameTime);

			foreach (Platform knownPlatform in gameContainer.platformContainer) {
				knownPlatform.Move ();
			}
			foreach (Chest knownChest in gameContainer.chestContainer) {
				knownChest.UpdateClickPosition (GameLayer.cameraMover.CameraCenter, screenSize);
			}

			if (CurrentControlType == ControlType.Button)
				gameContainer.mainCharacter.Jump = false;


			#if DEBUG
			//debugmenu update	
			gameContainer.physicsHandler.debugDrawer.ClearBuffer ();
			gameContainer.physicsHandler.gameWorld.DrawDebugData ();
			#endif
		}

		#region HandleButtonClick

		CCTouch LastCanceledTouch;

		void JumpB_HandleButtonClicked(object sender, TouchInfo e){
			switch (e) {
			case TouchInfo.Began:
				//wenn der RightButton geklickt wurde 
				if (sender != LastCanceledTouch) {
					gameContainer.mainCharacter.Jump = true;
					LastCanceledTouch = (CCTouch)sender;
				}
				break;
			}
		}

		void LeftB_HandleButtonClicked(object sender, TouchInfo e){
			switch (e) {
			case TouchInfo.Began:
				gameContainer.mainCharacter.MoveDirection = Direction.Left;
				break;
			case TouchInfo.Ended:
				gameContainer.mainCharacter.MoveDirection = Direction.None;
				break;
			case TouchInfo.Canceled:
				gameContainer.mainCharacter.MoveDirection = Direction.None;
				break;
			}
		}

		void RightB_HandleButtonClicked(object sender, TouchInfo e){
			switch (e) {
			case TouchInfo.Began:
				gameContainer.mainCharacter.MoveDirection = Direction.Right;
				break;
			case TouchInfo.Ended:
				gameContainer.mainCharacter.MoveDirection = Direction.None;
				break;
			case TouchInfo.Canceled:
				gameContainer.mainCharacter.MoveDirection = Direction.None;
				break;
			}
		}

		#endregion
	}
}

