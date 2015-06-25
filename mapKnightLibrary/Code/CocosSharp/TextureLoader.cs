using System;

using CocosSharp;

namespace mapKnightLibrary
{
	public class TextureLoader
	{
		public static TextureLoader mainTextureLoader;

		public TextureLoader ()
		{
		}

		public CCTexture2D GetTexture(string ID) 
		{

			return new CCTexture2D ();
		}
	}
}

