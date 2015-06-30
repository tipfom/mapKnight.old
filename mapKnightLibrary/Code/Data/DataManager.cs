using System;
using System.IO;

//using SQLite;

namespace mapKnightLibrary
{
	public abstract class DataManager 
	{
		public virtual bool BeginRead(string dataset)
		{
			//Beginn der Query
			//Leistungsoptimierung
			return false;
		}

		public virtual bool EndRead()
		{
			//Leistungsoptimierung
			return true;
		}

		public virtual string GetOrCreate(string name, string defaultvalue){
			//zum Aufrufen von Werten aus einer string Datenbank 
			return defaultvalue;
		}

		public virtual int GetOrCreate(string name, int defaultvalue){
			//zum Aufrufen von Werten aus einer int Datenbank 
			return defaultvalue;
		}

		public virtual void Set(string name, string value){
			
		}

	}
}