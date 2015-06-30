using System;
using System.IO;
using System.Collections.Generic;

namespace mapKnightLibrary
{
	public class CodeDataManager : DataManager
	{
		public override int GetOrCreate (string name, int defaultvalue)
		{
			switch (name) {
			default:
				return defaultvalue;
			}
		}

		public override string GetOrCreate (string name, string defaultvalue)
		{
			switch (name) {
			case "database":
				return Path.Combine("main_database.db3");
			default:
				return defaultvalue;
			}}
	}
}

