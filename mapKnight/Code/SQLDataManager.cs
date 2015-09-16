using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

using Mono.Data.Sqlite;

using mapKnightLibrary;

[assembly: Xamarin.Forms.Dependency (typeof (mapKnight.AndroidSQLDataManager))]

namespace mapKnight
{
	public class AndroidSQLDataManager : DataBaseManager
	{
		static string Tag = "AndroidSQLDataManager";
		//http://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_4_using_adonet/

		static string DataFilePath = System.IO.Path.Combine (System.Environment.GetFolderPath (Environment.SpecialFolder.Personal).ToString (), "_game_database_.db3");

		SqliteConnection DataBase;

		string DataPath;

		public bool DatabaseCreationRequired;

		public AndroidSQLDataManager ()
		{
			if (!File.Exists (DataFilePath)) {
				CrossLog.Log ("Android", Tag, "Created new Databasefile (" + DataFilePath + ")", MessageType.Debug);
				//Wenn die Datenbank nicht existiert wird sie erstellt
				SqliteConnection.CreateFile (DataFilePath);
				DataBase = new SqliteConnection ("Data Source=" + DataFilePath);

				int rowcount;

				DataBase.Open ();
				using (SqliteCommand Command = DataBase.CreateCommand ()) {
					//Erstellen der Datenbank für int-Werte
					Command.CommandText = "CREATE TABLE intdata(name CHAR(30), value INT);";
					rowcount = Command.ExecuteNonQuery ();
					//Erstellen der Datenbank für string-Werte
					Command.CommandText = "CREATE TABLE stringdata(name CHAR(30), value CHAR(50));";
					rowcount = Command.ExecuteNonQuery ();
				}
				DataBase.Close ();

				DatabaseCreationRequired = true;
			} else {
				DataBase = new SqliteConnection ("Data Source=" + DataFilePath);
				CrossLog.Log ("Android", Tag, "Set DataBase to " + DataBase.ToString(), MessageType.Debug);
			}
			DataPath = DataFilePath;
		}

		public override int GetOrCreate (string name, int defaultvalue = 0)
		{
			if (name.StartsWith ("int:")) {
				DataBase.Open ();
				using (SqliteCommand Command = DataBase.CreateCommand ()) {
					Command.CommandText = "SELECT * FROM [intdata]";
					SqliteDataReader CommandExecuteReader = Command.ExecuteReader ();

					//liest die Daten der Datenbank in ein Dictionary
					Dictionary<string, int> ReadData = new Dictionary<string, int> ();
					while (CommandExecuteReader.Read ()) {
						ReadData.Add (CommandExecuteReader ["name"].ToString (), Convert.ToInt32 (CommandExecuteReader ["value"].ToString ()));
					}
					CommandExecuteReader.Close ();

					if (ReadData.Count > 0) {
						//wenn Daten ausgelesen wurden
						if (ReadData.ContainsKey (name)) 
						{
							DataBase.Close ();
							return ReadData [name];
						}
						else
							Command.CommandText = "INSERT INTO [intdata] ([name], [value]) VALUES ('" + name + "', '" + defaultvalue + "');";
						Command.ExecuteNonQuery ();
						DataBase.Close ();
						return defaultvalue;
					} else {
						//sonst wird ein neuer Datensatz angelegt
						Command.CommandText = "INSERT INTO [intdata] ([name], [value]) VALUES ('" + name + "', '" + defaultvalue + "');";
						Command.ExecuteNonQuery ();
						DataBase.Close ();
						return defaultvalue;
					}
				}
			} else {
				throw new ArgumentException ("wrong format of dataset name (originalname=" + name + ") put a 'int:' before the name");
			}
		}

		public override string GetOrCreate (string name, string defaultvalue = "default")
		{
			if (name.StartsWith ("string:")) {
				DataBase.Open ();
				using (SqliteCommand Command = DataBase.CreateCommand ()) {
					Command.CommandText = "SELECT * FROM [stringdata]";
					SqliteDataReader CommandExecuteReader = Command.ExecuteReader ();

					//liest die Daten der Datenbank in ein Dictionary
					Dictionary<string, string> ReadData = new Dictionary<string, string> ();
					while (CommandExecuteReader.Read ()) {
						ReadData.Add (CommandExecuteReader ["name"].ToString (), CommandExecuteReader ["value"].ToString ());
					}
					CommandExecuteReader.Close ();

					if (ReadData.Count > 0) {
						//wenn Daten ausgelesen wurden
						if (ReadData.ContainsKey (name)) 
						{
							DataBase.Close ();
							return ReadData [name];
						}
						else
							Command.CommandText = "INSERT INTO [stringdata] ([name], [value]) VALUES ('" + name + "','" + defaultvalue + "')";
						Command.ExecuteNonQuery ();
						DataBase.Close ();
						return defaultvalue;
					} else {
						//sonst wird ein neuer Datensatz angelegt
						Command.CommandText = "INSERT INTO [stringdata] ([name], [value]) VALUES ('" + name + "','" + defaultvalue + "')";
						Command.ExecuteNonQuery ();
						DataBase.Close ();
						return defaultvalue;
					}
				}
			} else if (name.StartsWith ("int:")) {
				return GetOrCreate (name, 0).ToString ();
			} else {
				throw new ArgumentException ("wrong format of dataset name (originalname=" + name + ") put a 'string:' before the name");
			}
		}

		public override void Set (string name, string value)
		{
			DataBase.Open ();
			using (SqliteCommand Command = DataBase.CreateCommand ()) {
				if (name.StartsWith ("int")) {
					//wenn die Zahl eine Nummer ist
					Command.CommandText = "UPDATE [intdata] SET [value]='" + value + "' WHERE [name]='" + name + "';";
				} else {
					Command.CommandText = "UPDATE [stringdata] SET [value]='" + value + "' WHERE [name]='" + name + "';";
				}
				Command.ExecuteNonQuery ();
			}
			DataBase.Close ();
		}

		public override void Delete (string name)
		{
			DataBase.Open ();
			using (SqliteCommand Command = DataBase.CreateCommand ()) {
				if (name.StartsWith ("int")) {
					Command.CommandText = "DELETE FROM [intdata] WHERE [name]='" + name + "';";
				} else {
					Command.CommandText = "DELETE FROM [stringdata] WHERE [name]='" + name + "';";
				}

				Command.ExecuteNonQuery ();
			}
			DataBase.Close ();
		}

		public override bool BeginRead ()
		{
			CrossLog.Log ("Android", Tag, "Begin Reading from " + DataBase.DataSource, MessageType.Debug);
			return true;
		}

		public override bool EndRead ()
		{
			CrossLog.Log ("Android", Tag, "Ended Reading from " + DataBase.DataSource, MessageType.Debug);
			return true;
		}
	}
}