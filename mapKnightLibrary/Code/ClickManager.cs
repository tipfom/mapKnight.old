using System;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnightLibrary
{
	public class ClickManager : CCEventListenerTouchAllAtOnce
	{
		Container gameContainer;
		CCSize screenSize;
		CCTouch LastCanceledTouch;

		List<IClickable> ObjectList;

		public ClickManager (CCSize ScreenSize, Container mainContainer) : base()
		{
			ObjectList = new List<IClickable> ();

			gameContainer = mainContainer;
			screenSize = ScreenSize;

			this.OnTouchesBegan += HandleTouchesBegan;
			this.OnTouchesCancelled += HandleTouchesCanceled;
			this.OnTouchesEnded += HandleTouchesEnded;
			this.OnTouchesMoved += HandleTouchesMoved;
		}

		public void AddObject(IClickable Object){
			ObjectList.Add (Object);
		}

		public void RemoveObject(IClickable Object){
			if (ObjectList.Contains (Object))
				ObjectList.Remove (Object);
		}

		//Slide Region ist in einer Textdatei, da es zZ mit dem neuen Konzept nicht funktioniert

		#region ButtonTouchListener
		private void HandleTouchesMoved(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				foreach (IClickable Object in ObjectList) {
					if (Object.MovedXChangeMin == Math.Abs (Touch.StartLocationOnScreen.X - Touch.LocationOnScreen.X) || Object.MovedYChangeMin == Math.Abs (Touch.StartLocationOnScreen.Y - Touch.LocationOnScreen.Y)) {
						if (Math.Abs ((Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Width / 2) {
							if (Math.Abs (-(Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Height / 2) {
								Object.Clicked (Touch, TouchInfo.Moved);
							}
						}
					}
				}
			}
		}

		private void HandleTouchesBegan(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				foreach (IClickable Object in ObjectList) {
					if (Math.Abs ((Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Width / 2) {
						if (Math.Abs (-(Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Height / 2) {
							Object.Clicked (Touch, TouchInfo.Began);
						}
					}
				}
			}
		}

		private void HandleTouchesEnded(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				foreach (IClickable Object in ObjectList) {
					if (Math.Abs ((Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Width / 2) {
						if (Math.Abs (-(Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Height / 2) {
							Object.Clicked (Touch, TouchInfo.Ended);
						}
					}
				}
			}
		}

		private void HandleTouchesCanceled(System.Collections.Generic.List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (CCTouch Touch in touches) {
				foreach (IClickable Object in ObjectList) {
					if (Math.Abs ((Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Width / 2) {
						if (Math.Abs (-(Touch.LocationOnScreen.X - Object.Center.X) + (screenSize.Height - Touch.LocationOnScreen.Y - Object.Center.Y)) <= Object.Size.Height / 2) {
							Object.Clicked (Touch, TouchInfo.Canceled);
						}
					}
				}
			}
		}
		#endregion
	}
}

