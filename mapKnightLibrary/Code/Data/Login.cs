using System;

namespace mapKnightLibrary
{
	public interface ILogin{
		bool RequestLogin (object context);
		bool Connected { get; }
		void Disconnect ();
		void Write (string data, bool overwrite);
		void Write (byte data, bool overwrite);
		string Read ();
	}
}