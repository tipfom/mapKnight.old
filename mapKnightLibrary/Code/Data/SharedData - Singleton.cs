using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace mapKnightLibrary
{
	public class SharedData // Singleton-Klasse
	//https://social.msdn.microsoft.com/Forums/de-DE/a06290ad-72a3-4a0f-9f29-adeff4cec628/gibt-es-globale-objekte-in-c?forum=visualcsharpde
	{
		// Singleton Instances
		private static volatile SharedData instance;
		private static object syncRoot = new Object();

		//Database Instances
		private string UniqueGeneratedString;

		private Dictionary<ShareableInformation,string> KnownData;

		DataBaseManager DataBase;

		private SharedData ()
		{
			UniqueGeneratedString = GenerateGenericString (64);

			KnownData = new Dictionary<ShareableInformation, string> ();

			DataBase = DependencyService.Get<DataBaseManager> ();
			//----------------------------------------------------------------------------------------------------
			if (DataBase.GetOrCreate ("string:font_standart", UniqueGeneratedString) != UniqueGeneratedString)
				KnownData.Add (ShareableInformation.standart_font, DataBase.GetOrCreate ("string:font_standart"));
			else
				DataBase.Delete ("string:font_standart");
			//----------------------------------------------------------------------------------------------------
			if (DataBase.GetOrCreate ("int:font_size", UniqueGeneratedString) != UniqueGeneratedString)
				KnownData.Add (ShareableInformation.standart_fontsize, DataBase.GetOrCreate ("int:font_size"));
			else
				DataBase.Delete ("int:font_size");
			//----------------------------------------------------------------------------------------------------
			if (DataBase.GetOrCreate ("string:app_version", UniqueGeneratedString) != UniqueGeneratedString)
				KnownData.Add (ShareableInformation.application_version, DataBase.GetOrCreate ("string:app_version"));
			else
				DataBase.Delete ("string:app_version");
			//----------------------------------------------------------------------------------------------------
			if (DataBase.GetOrCreate ("string:app_build", UniqueGeneratedString) != UniqueGeneratedString)
				KnownData.Add (ShareableInformation.application_build, DataBase.GetOrCreate ("string:app_build"));
			else
				DataBase.Delete ("string:app_build");
			//----------------------------------------------------------------------------------------------------
			if (DataBase.GetOrCreate ("string:app_name", UniqueGeneratedString) != UniqueGeneratedString)
				KnownData.Add (ShareableInformation.application_name, DataBase.GetOrCreate ("string:app_name"));
			else
				DataBase.Delete ("string:app_name");
			//----------------------------------------------------------------------------------------------------
		}

		public static SharedData Instance{
			get{ 
				if (instance == null) {
					lock (syncRoot) {
						if (instance == null)
							instance = new SharedData ();
					}
				}
				return instance;
			}
		}

		public string Get(ShareableInformation RequestedInformation){
			if (KnownData.ContainsKey (RequestedInformation)) {
				return KnownData [RequestedInformation];
			} else {
				throw new ArgumentException (RequestedInformation.ToString () + " is not registered by SharedData");
			}		
		}

		private string GenerateGenericString(int Length){
			string GeneratedString = "";
			Random Random = new Random ();

			for (int i = 0; i < Length; i++) {
				GeneratedString += Convert.ToChar (Convert.ToInt32 (Random.Next (32, 126))).ToString ();
			}

			return GeneratedString;
		}
	}
}