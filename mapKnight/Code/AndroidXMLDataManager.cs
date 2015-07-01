using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;

using mapKnightLibrary;

namespace mapKnight
{
	public class AndroidXMLDataManager : XMLDataManager
	{
		public string name{ get; private set; }
		public string package{ get; private set; }

		Dictionary<string,int> ChapterIndex;
		Dictionary<string,string>[] EntryDictionary;

		int CurrentChapter;

		public AndroidXMLDataManager (Stream XMLData, string package)
		{
			this.package = package;
			using (System.Xml.Linq.XDocument DataDocument = XDocument.Load (XMLData)) {
				foreach (XElement Set in DataDocument.Element(package).Elements) {
					if (Set.Attribute ("package").ToString () == package) {
						ChapterIndex = new Dictionary<string, int> ();
						foreach (XElement Chapter in Set.Elements) {
							ChapterIndex.Add (Chapter.Name, CurrentChapter);
							EntryDictionary [CurrentChapter] = new Dictionary<string, string> ();
							CurrentChapter++;
							foreach (XElement Entry in Chapter.Elements) {
								EntryDictionary [CurrentChapter].Add (Entry.Attribute ("name").ToString (), Entry.Value);
							}
						}
					}
				}
			}
		}

		public override bool BeginRead (string chapter)
		{
			
		}

		public override int GetInt (string name)
		{
			
		}

		public override string GetString (string name)
		{
			
		}

		public override bool EndRead ()
		{
			
		}
	}
}