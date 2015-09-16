using System;

using mapKnightLibrary;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Plus;
using Android.Gms.Games;
using Android.Support.V4.App;
using Android.Util;
using Android.Text.Format;
using Java.Interop;
using Android.Gms.Games.Snapshot;

//[assembly: Xamarin.Forms.Dependency (typeof (mapKnight.ALogin))]

namespace mapKnight
{
	public class ALogin : Java.Lang.Object, ILogin, IGoogleApiClientConnectionCallbacks, IGoogleApiClientOnConnectionFailedListener
	{
		private static string Tag = "GooglePlayLogIn";

		//https://developers.google.com/games/services/android/init
		//https://developers.google.com/games/services/android/savedgames
		private const int REQUESTCODE_SAVED_GAMES = 9009;
		private const int REQUESTCODE_SIGN_IN = 9001;
		private const int REQUESTCODE_RESOLVE_ERROR = 9000;

		// ALogin == AndroidLogin
		private IGoogleApiClient GoogleClient;
		private Activity GoogleLoginActivity;
		private string CurrentSave;

		public ALogin ()
		{
		}

		#region ILogin implementation

		public bool RequestLogin (object context)
		{
			if (context is Activity) {
				GoogleLoginActivity = (Activity)context;
				try {
					ALog._Debug ("Android", "AndroidLogin", "Login requested");
					//start des Aufbauprozesses
					ALog._Debug ("Android", "AndroidLogin", "Starting to create a GoogleApiClient");
					GoogleClient = new GoogleApiClientBuilder (GoogleLoginActivity)
						.AddApi (GamesClass.API).AddScope (GamesClass.ScopeGames)
						.AddApi (PlusClass.API).AddScope (PlusClass.ScopePlusLogin)
						.AddApi (DriveClass.API).AddScope (DriveClass.ScopeAppfolder)
						.AddConnectionCallbacks(this).AddOnConnectionFailedListener(this)
						.Build ();
					GoogleClient.RegisterConnectionCallbacks(this);
					GoogleClient.RegisterConnectionFailedListener(this);

					GoogleClient.Connect ();

					ALog._Debug ("Android", "AndroidLogin", "Connecting");
					return true;
				} catch (Exception e) {
					ALog._Error ("Android", "AndroidLogin", e);
					ALog._Warn ("Android", "AndroidLogin", "Catched error while login");
				}
				return false;
			} else {
				throw new ArgumentException ("given context is not an activity");
			}
		}

		public void Disconnect ()
		{
			GoogleClient.Disconnect ();
		}

		public void Write (string data, bool overwrite)
		{
		}

		public void Write (byte data, bool overwrite)
		{
		}

		public string Read ()
		{
			return "";
		}

		public bool Connected { get; private set; }

		#endregion

		public void CheckResult(int requestCode, Result resultCode, Intent data){
			if (data.HasExtra (Snapshots.ExtraSnapshotMetadata)) {
				// Load a snapshot.
				SnapshotMetadata snapshotMetadata = ObjectTypeHelper.Cast<SnapshotMetadata> (data.GetParcelableExtra (Snapshots.ExtraSnapshotMetadata));
				CurrentSave = snapshotMetadata.ToString ();

				// Load the game data from the Snapshot
				// ...
			} else if (data.HasExtra (Snapshots.ExtraSnapshotNew)) {
				// Create a new snapshot named with a unique string
				String unique = new Random ().Next (int.MaxValue).ToString ();
				CurrentSave = "snapshotTemp-" + unique;

				// Create the new snapshot
				// ...
			}
		}

		#region Listener implementations

		public void OnConnected (Bundle connectionHint)
		{
			Connected = true;
			ALog._Debug ("Android", "AndroidLogin", "Login connected");
			Intent SnapshotIntent = GamesClass.Snapshots.GetSelectSnapshotIntent (GoogleClient, "mapKnight Google+ Login", true, true, 5);
			GoogleLoginActivity.StartActivityForResult (SnapshotIntent, REQUESTCODE_SAVED_GAMES);

		}

		//Fehlerbehebung bei der Verbindung
		private bool AllreadyResolvingConnectionFailure = false;
		private bool AutoStartSignInFlow = true;

		public void OnConnectionSuspended (int cause)
		{
			//reconnect
			ALog._Debug ("Android", Tag, "Login suspended");
			GoogleClient.Connect ();
		}

		public void OnConnectionFailed (Android.Gms.Common.ConnectionResult result)
		{
			ALog._Debug ("Android", Tag, "Connection failed");
			ALog._Warn ("Android", Tag, "Reason : " + result.ToString ());

			if (AllreadyResolvingConnectionFailure) {
				// Already resolving
				ALog._Debug ("Android", Tag, "Allready resolving a connection failure");
				return;
			}

			if (AutoStartSignInFlow) {
				AutoStartSignInFlow = false;
				AllreadyResolvingConnectionFailure = true;

				if (result.HasResolution) {
					result.StartResolutionForResult (GoogleLoginActivity, REQUESTCODE_RESOLVE_ERROR);
				} else {
					AllreadyResolvingConnectionFailure = false;
				}
			}
		}

		#endregion
	}
}

