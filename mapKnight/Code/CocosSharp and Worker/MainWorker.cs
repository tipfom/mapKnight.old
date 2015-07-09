using System;
using System.IO;
using System.Collections.Generic;

using mapKnightLibrary;

using Microsoft.Xna.Framework;

using CocosSharp;
using Box2D;

using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android;
using Android.Widget;
using Android.Views;
using Android.Content;


namespace mapKnight
{
	[Activity(
		Label = "mapKnight",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/heart",
		Theme = "@style/Theme.Main",
		LaunchMode = LaunchMode.SingleInstance,
		ScreenOrientation = ScreenOrientation.Nosensor,
		//MainLauncher = true,
		ConfigurationChanges =  ConfigChanges.Keyboard | 
		ConfigChanges.KeyboardHidden)]

	public class MainWorker : ActionBarActivity
	{
		Intent GameIntent;

		ControlType CurrentControlType;

		public MainWorker(){
			CurrentControlType = ControlType.Button;
		}

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//HideNavBar ();

			SetContentView (Resource.Layout.ContainerWindow);

			FrameLayout MainContainer = FindViewById<FrameLayout> (Resource.Id.MainContainer);

			MainWindow mainWindow = new MainWindow ();
			GameIntent = new Intent (this, typeof(Game));
			mainWindow.startGameEvent += (object sender, EventArgs e) => {
				GameIntent.PutExtra ("string:CurrentControlType", CurrentControlType.ToString ());
				this.StartActivity (GameIntent);
				this.Finish ();
			};

			var tansaction = SupportFragmentManager.BeginTransaction ();
			tansaction.Add (Resource.Id.MainContainer, mainWindow, "MainWindow");
			tansaction.Commit ();


			SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.ToolBar);
			ListView toolBarList = FindViewById<ListView>(Resource.Id.leftdrawer);

			List<string> inhalt = new List<string> ();
			inhalt.Add ("Options");
			inhalt.Add ("Coded by tipfom");
			inhalt.Add ("Designed by Exo");
			inhalt.Add ("Version = " + GetString (Resource.String.app_version) + " Build " + GetString (Resource.String.app_build));
			ArrayAdapter<string> adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, inhalt);
			toolBarList.Adapter = adapter;
			toolBarList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
			{
				switch(e.Position){
				case 0:
					//Optionsitem
					OptionPopUp OptionPopUp = new OptionPopUp(CurrentControlType);
					Android.App.FragmentTransaction popuptransaction = this.FragmentManager.BeginTransaction();
					OptionPopUp.Show(popuptransaction, "OptionPopUp");
					OptionPopUp.ControlTypeToggled += (object _sender, ControlType _e) => {CurrentControlType = _e;};
					break;
				}
			};
			SetSupportActionBar (toolBar);
		}

		private void HideNavBar()
		{
			//versteckt die Navigationsleiste
			View decorView = Window.DecorView;
			int newUiOptions = (int)decorView.WindowSystemUiVisibility;

			newUiOptions |= (int)SystemUiFlags.Fullscreen;
			newUiOptions |= (int)SystemUiFlags.HideNavigation;
			newUiOptions |= (int)SystemUiFlags.ImmersiveSticky;

			decorView.SystemUiVisibility = (StatusBarVisibility)newUiOptions;
		}
	}
}
