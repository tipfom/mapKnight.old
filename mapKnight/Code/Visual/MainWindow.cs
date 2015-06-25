using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnight
{
    public class MainWindow : Android.Support.V4.App.Fragment,View.IOnTouchListener
	{
        private float LastTouchY;
        private FrameLayout PopUpContainer;

		public event EventHandler startGameEvent;

		public MainWindow ()
		{
            
        }
            
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View View = inflater.Inflate(Resource.Layout.MainWindow,container,false);

             PopUpContainer = View.FindViewById<FrameLayout>(Resource.Id.PopUpContainer);

            var transaction = Activity.SupportFragmentManager.BeginTransaction();
            transaction.Add(PopUpContainer.Id,new InventorySlideIn(),"InventorySlideIn");
            transaction.Commit();

			ImageButton PopUpButton = View.FindViewById<ImageButton>(Resource.Id.button_stats);

            PopUpButton.Click+= (object sender, EventArgs e) => {
                //erstellen des Statistiken Popups
				StatsPopUp optionPopUp = new StatsPopUp();
                Android.App.FragmentTransaction popuptransaction = Activity.FragmentManager.BeginTransaction();
				optionPopUp.Show(popuptransaction, "StatsPopUp");
            };

            PopUpContainer.SetOnTouchListener(this);


			Button PlayButton = View.FindViewById<Button>(Resource.Id.button_play);
			PlayButton.Click += (object sender, EventArgs e) => {
				if(startGameEvent != null)
					startGameEvent(this, new EventArgs());
				//bindet den "Spielen" Button an die Spielen Anwendung
				//(Variablen)
			};

            return View;    
		}

        public bool OnTouch(View view, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    LastTouchY = e.GetY();
                    return true;
                case MotionEventActions.Move:
                    float CurrentYTouched = e.GetY();
                    float Change = LastTouchY - CurrentYTouched;

                    float translationY = view.TranslationY;

                    translationY -= Change;

                    if (translationY < 0)
                    { //Um zu gewährleisten, dass man das Fragment immer hoch ziehen kann
                        translationY = 0;
                    }
                    else if (translationY > view.Height*360/400)
                    {
                        translationY = view.Height*360/400;
                    }

                    view.TranslationY = translationY;

                    return true;
                case MotionEventActions.Outside:
                    if (view.TranslationY <= 340)
                    {
                        view.Animate().SetDuration(500).SetInterpolator(new Android.Views.Animations.OvershootInterpolator(5)).TranslationY(360);
                    }
                    return true;
                default:
                    return true;
            }
        }
    }
}

