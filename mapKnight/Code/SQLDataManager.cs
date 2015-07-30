using System;
using System.IO;
using System.Data;
using System.Collections.Generic;

using Mono.Data.Sqlite;

namespace mapKnightLibrary
{
	public class AndroidSQLDataManager : DataBaseManager
	{
		static string Tag = "AndroidSQLDataManager";
		//http://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_4_using_adonet/

		SqliteConnection DataBase;

		string DataPath;

		public AndroidSQLDataManager (string DataFilePath)
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
			} else {
				DataBase = new SqliteConnection ("Data Source=" + DataFilePath);
				CrossLog.Log ("Android", Tag, "Set DataBase to " + DataBase.ToString(), MessageType.Debug);
			}
			DataPath = DataFilePath;
		}

		public override int GetOrCreate (string name, int defaultvalue = 0)
		{
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
						return ReadData [name];
					else
						Command.CommandText = "INSERT INTO [intdata] ([name], [value]) VALUES ('" + name + "', '" + defaultvalue + "');";
						Command.ExecuteNonQuery ();
						return defaultvalue;
				} else {
					//sonst wird ein neuer Datensatz angelegt
					Command.CommandText = "INSERT INTO [intdata] ([name], [value]) VALUES ('" + name + "', '" + defaultvalue + "');";
					Command.ExecuteNonQuery ();
					return defaultvalue;
				}
			}
		}

		public override string GetOrCreate (string name, string defaultvalue = "default")
		{
			using (SqliteCommand Command = DataBase.CreateCommand ()) {
				Command.CommandText = "SELECT * FROM [stringdata]";
				SqliteDataReader CommandExecuteReader = Command.ExecuteReader ();

				//liest die Daten der Datenbank in ein Dictionary
				Dictionary<string, string> ReadData = new Dictionary<string, string>();
				while (CommandExecuteReader.Read ()) {
					ReadData.Add (CommandExecuteReader ["name"].ToString (), CommandExecuteReader ["value"].ToString ());
				}
				CommandExecuteReader.Close ();

				if (ReadData.Count > 0) {
					//wenn Daten ausgelesen wurden
					if (ReadData.ContainsKey (name))
						return ReadData [name];
					else
						Command.CommandText = "INSERT INTO [stringdata] ([name], [value]) VALUES ('" + name + "','" + defaultvalue + "')";
						Command.ExecuteNonQuery ();
						return defaultvalue;
				} else {
					//sonst wird ein neuer Datensatz angelegt
					Command.CommandText = "INSERT INTO [stringdata] ([name], [value]) VALUES ('" + name + "','" + defaultvalue + "')";
					Command.ExecuteNonQuery ();
					return defaultvalue;
				}
			}
		}

		public override void Set (string name, string value)
		{
			using (SqliteCommand Command = DataBase.CreateCommand ()) {
				int number;
				if (int.TryParse (name, out number)) {
					//wenn die Zahl eine Nummer ist
					Command.CommandText = "UPDATE [intdata] SET [value]='" + value + "' WHERE [name]='" + name + "';";
				}else
				{
					Command.CommandText = "UPDATE [stringdata] SET [value]='" + value + "' WHERE [name]='" + name + "';";
				}
				Command.ExecuteNonQuery ();
			}
		}

		public override bool BeginRead ()
		{
			CrossLog.Log ("Android", Tag, "Begin Reading from " + DataBase.DataSource, MessageType.Debug);
			DataBase.Open ();
			return true;
		}

		public override bool EndRead ()
		{
			CrossLog.Log ("Android", Tag, "Ended Reading from " + DataBase.DataSource, MessageType.Debug);
			DataBase.Close ();
			return true;
		}
	}
}