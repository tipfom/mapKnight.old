using System;
using System.IO;

//using SQLite;

namespace mapKnightLibrary
{
	public abstract class DataManager 
	{
		string DataBasePath;

		//SQLiteConnection DataBaseConnection;

		public DataManager (string DataBaseFilePath)
		{
			DataBasePath = DataBaseFilePath;

			//DataBaseConnection = new SQLiteConnection (DataBasePath);
		}

		public void GetOrCreate(string name, string defaultvalue){
			
		}
	}
}

