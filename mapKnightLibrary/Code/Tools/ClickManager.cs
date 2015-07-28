using System;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnightLibrary
{
	public class ClickManager : CCEventListenerTouchAllAtOnce
	{
		Container gameContainer;
		CCSize screenSize;

		List<IClickable> RemoveList;
		List<IClickable> AddList;
		List<IClickable> ObjectList;

		public ClickManager (CCSize ScreenSize, Container mainContainer) : base()
		{
			ObjectList = new List<IClickable> ();
			RemoveList = new List<IClickable> ();
			AddList = new List<IClickable> ();

			gameContainer = mainContainer;
			screenSize = ScreenSize;

			this.OnTouchesBegan += HandleTouchesBegan;
			this.OnTouchesCancelled += HandleTouchesCanceled;
			this.OnTouchesEnded += HandleTouchesEnded;
			this.OnTouchesMoved += HandleTouchesMoved;
		}

		public void AddObject(IClickable Object){
			AddList.Add (Object);
		}

		public void RemoveObject(IClickable Object){
			if (ObjectList.Contains (Object))
				RemoveList.Add (Object);
		}

		public void AddManyObjects(IClickable[] Objects){
			AddList.AddRange (Objects);
		}

		public void RemoveManyObjects(IClickable[] Objects){
			foreach (IClickable Object in Objects) {
				if (ObjectList.Contains (Object))
					RemoveList.Add (Object);		
			}
		}

		bool CurrentlyFlushing;
		void Flush(){
			if (!CurrentlyFlushing) {
				CurrentlyFlushing = true;
				if (AddList.Count > 0) {
					for (int i = 0; i < AddList.Count; i++) {
						ObjectList.Add (AddList [i]);
					}
					AddList.Clear ();
				}
				if (RemoveList.Count > 0) {
					for (int i = 0; i < RemoveList.Count; i++) {
						ObjectList.Remove (RemoveList [i]);
					}
					RemoveList.Clear ();
				}
				CurrentlyFlushing = false;
			}
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
			Flush ();
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
			Flush ();
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
			Flush ();
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
			Flush ();
		}
		#endregion
	}
}

