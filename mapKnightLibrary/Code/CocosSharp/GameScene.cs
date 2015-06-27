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

		CCEventListenerTouchAllAtOnce touchListener;

		CCSize screenSize;

		Container gameContainer;

		CCTouch LastCanceledTouch;

		ControlType CurrentControlType;

		public GameScene (CCWindow mainWindow, Container mainContainer, ControlType RunningControlType) : base(mainWindow)
		{
			CurrentControlType = RunningControlType;

			gameContainer = mainContainer;
			GameLayer = new MergedLayer (mainWindow, gameContainer);

			this.AddChild (GameLayer);

			screenSize = mainWindow.WindowSizeInPixels;

			//Touchlistener Initialisierung
			touchListener = new CCEventListenerTouchAllAtOnce ();

			switch (RunningControlType) {
			case ControlType.Slide:
				touchListener.OnTouchesMoved = Slide_HandleTouchesMoved; 
				touchListener.OnTouchesBegan = Slide_HandleTouchesBegan;
				touchListener.OnTouchesEnded = Slide_HandleTouchesEnded;
				touchListener.OnTouchesCancelled = Slide_HandleTouchesCanceled;

				break;
			case ControlType.Button:
				JumpButton = new CCSprite ("menu_arrow");
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

				this.AddChild (JumpButton);
				this.AddChild (MoveLeftButton);
				this.AddChild (MoveRightButton);

				touchListener.OnTouchesMoved = Button_HandleTouchesMoved; 
				touchListener.OnTouchesBegan = Button_HandleTouchesBegan;
				touchListener.OnTouchesEnded = Button_HandleTouchesEnded;
				touchListener.OnTouchesCancelled = Button_HandleTouchesCanceled;

				break;
			}

			AddEventListener (touchListener, this);

			//FPS Label
			FPSLabel = new CCLabel ("Score: 0", "arial", 22);
			FPSLabel.Position = new CCPoint (100, 100);
			FPSLabel.Color = new CCColor3B (255, 255, 0);
			AddChild (FPSLabel);

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

			this.AddChild (MapNameLabel);
			this.AddChild (MapCreatorLabel);
			this.AddChild (MapVersionLabel);

			//Interface
			ManaSprite = new CCSprite[2];
			LifeSprite = new CCSprite[2];

			ManaSprite [0] = new CCSprite ("batterie_full"){ IsAntialiased = false };
			ManaSprite [1] = new CCSprite ("batterie_empty"){ IsAntialiased = false };
			LifeSprite [0] = new CCSprite ("heart_full"){ IsAntialiased = false };
			LifeSprite [1] = new CCSprite ("heart_empty"){ IsAntialiased = false };

			ManaSprite [0].Scale = 6f;
			ManaSprite [1].Scale = 6f;
			LifeSprite [0].Scale = 6f;
			LifeSprite [1].Scale = 6f;

			ManaSprite [1].Position = new CCPoint (screenSize.Width - ManaSprite [1].ScaledContentSize.Width, screenSize.Height - ManaSprite [1].ScaledContentSize.Height);
			LifeSprite [1].Position = new CCPoint (screenSize.Width - LifeSprite [1].ScaledContentSize.Width, ManaSprite [1].Position.Y - LifeSprite [1].ScaledContentSize.Height);

			this.AddChild (ManaSprite [1]);
			this.AddChild (ManaSprite [0]);
			this.AddChild (LifeSprite [1]);
			this.AddChild (LifeSprite [0]);

			this.InterfaceUpdate (null, new StatisticChangeEventArgHandler (Statistic.Life));
			this.InterfaceUpdate (null, new StatisticChangeEventArgHandler (Statistic.Mana));

			gameContainer.mainCharacter.StatChanged += InterfaceUpdate;

			gameContainer.mainCharacter.CurrentLife = 23;
			gameContainer.mainCharacter.CurrentMana = 4;

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
			FPSLabel.Text = (1 / frameTime).ToString () + " fps";

			gameContainer.physicsHandler.step (frameTime);
			gameContainer.mainCharacter.Update (frameTime);

			foreach (Platform knownPlatform in gameContainer.platformContainer) {
				knownPlatform.Move ();
			}

			if (CurrentControlType == ControlType.Button)
				gameContainer.mainCharacter.Jump = false;

			//debugmenu update	
		
		}

		#region SlideTouchHandler
		private void Slide_HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (Touch.LocationOnScreen.X < screenSize.Width / 3) {
					//keine Aktionen in diesem bereich bei bewegung
				} else {
					if (Touch.StartLocationOnScreen.Y - Touch.LocationOnScreen.Y > screenSize.Height / 3 && LastCanceledTouch != Touch) {
						gameContainer.mainCharacter.Jump = true;
						LastCanceledTouch = Touch;
					}
				}
			}
		}

		private void Slide_HandleTouchesBegan(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (Touch.LocationOnScreen.X < screenSize.Width / 3) {
					if (Touch.LocationOnScreen.X < screenSize.Width / 6) {
						gameContainer.mainCharacter.MoveDirection = Direction.Left;
					} else {
						gameContainer.mainCharacter.MoveDirection = Direction.Right;
					}
				} else { }
			}
		}

		private void Slide_HandleTouchesEnded(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (Touch.LocationOnScreen.X < screenSize.Width / 3) {
					if (Touch.LocationOnScreen.X < screenSize.Width / 6) {
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					} else {
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					}
				} else {
					gameContainer.mainCharacter.Jump = false;
				}
			}
		}

		private void Slide_HandleTouchesCanceled(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (Touch.LocationOnScreen.X < screenSize.Width / 3) {
					if (Touch.LocationOnScreen.X < screenSize.Width / 6) {
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					} else {
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					}
				} else {
					gameContainer.mainCharacter.Jump = false;
				}
			}
		}
		#endregion

		#region ButtonTouchListener
		private void Button_HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			//zur Zeit noch unnötig
		}

		private void Button_HandleTouchesBegan(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				//bei den Y Koordinaten muss von der screenHeight subtrahiert werden, weil TouchPoint(0,0) = links oben; ButtonPoint(0,0) = linksunten
				if (Math.Abs ((Touch.LocationOnScreen.X - MoveLeftButton.Position.X) + (screenSize.Height - Touch.LocationOnScreen.Y - MoveLeftButton.Position.Y)) <= MoveLeftButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.LocationOnScreen.X - MoveLeftButton.Position.X) + (screenSize.Height - Touch.LocationOnScreen.Y - MoveLeftButton.Position.Y)) <= MoveLeftButton.ScaledContentSize.Height / 2) {
						//wenn der LeftButton geklickt wurde 
						gameContainer.mainCharacter.MoveDirection = Direction.Left;
					}
				} else if (Math.Abs ((Touch.LocationOnScreen.X - MoveRightButton.Position.X) + (screenSize.Height - Touch.LocationOnScreen.Y - MoveRightButton.Position.Y)) <= MoveRightButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.LocationOnScreen.X - MoveRightButton.Position.X) + (screenSize.Height - Touch.LocationOnScreen.Y - MoveRightButton.Position.Y)) <= MoveRightButton.ScaledContentSize.Height / 2) {
						//wenn der RightButton geklickt wurde 
						gameContainer.mainCharacter.MoveDirection = Direction.Right;
					}
				} else if (Math.Abs ((Touch.LocationOnScreen.X - JumpButton.Position.X) + (screenSize.Height - Touch.LocationOnScreen.Y - JumpButton.Position.Y)) <= JumpButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.LocationOnScreen.X - JumpButton.Position.X) + (screenSize.Height - Touch.LocationOnScreen.Y - JumpButton.Position.Y)) <= JumpButton.ScaledContentSize.Height / 2 && Touch != LastCanceledTouch) {
						//wenn der RightButton geklickt wurde 
						gameContainer.mainCharacter.Jump = true;
						LastCanceledTouch = Touch;
					}
				}
			}
		}

		private void Button_HandleTouchesEnded(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (Math.Abs ((Touch.PreviousLocationOnScreen.X - MoveLeftButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveLeftButton.Position.Y)) <= MoveLeftButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.PreviousLocationOnScreen.X - MoveLeftButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveLeftButton.Position.Y)) <= MoveLeftButton.ScaledContentSize.Height / 2) {
						//wenn der LeftButton gecanceled wurde 
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					}
				} else if (Math.Abs ((Touch.PreviousLocationOnScreen.X - MoveRightButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveRightButton.Position.Y)) <= MoveRightButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.PreviousLocationOnScreen.X - MoveRightButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveRightButton.Position.Y)) <= MoveRightButton.ScaledContentSize.Height / 2) {
						//wenn der RightButton gecanceled wurde 
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					}
				}
			}
		}

		private void Button_HandleTouchesCanceled(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				if (Math.Abs ((Touch.PreviousLocationOnScreen.X - MoveLeftButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveLeftButton.Position.Y)) <= MoveLeftButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.PreviousLocationOnScreen.X - MoveLeftButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveLeftButton.Position.Y)) <= MoveLeftButton.ScaledContentSize.Height / 2) {
						//wenn der LeftButton gecanceled wurde 
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					}
				} else if (Math.Abs ((Touch.PreviousLocationOnScreen.X - MoveRightButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveRightButton.Position.Y)) <= MoveRightButton.ScaledContentSize.Width / 2) {
					if (Math.Abs (-(Touch.PreviousLocationOnScreen.X - MoveRightButton.Position.X) + (screenSize.Height - Touch.PreviousLocationOnScreen.Y - MoveRightButton.Position.Y)) <= MoveRightButton.ScaledContentSize.Height / 2) {
						//wenn der RightButton gecanceled wurde 
						gameContainer.mainCharacter.MoveDirection = Direction.None;
					}
				}
			}
		}
		#endregion
	}
}

