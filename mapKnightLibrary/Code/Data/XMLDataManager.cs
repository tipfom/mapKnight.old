using System;

namespace mapKnightLibrary
{
	public class XMLDataManager
	{
		static string defaultstringvalue = "default";
		static int defaultintvalue = 0;

		public virtual bool BeginRead(string package)
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

		public virtual string GetString(string name){
			//zum Aufrufen von Werten aus einer string Datenbank 
			return defaultstringvalue;
		}

		public virtual int GetInt(string name){
			//zum Aufrufen von Werten aus einer int Datenbank 
			return defaultintvalue;
		}
	}
}

