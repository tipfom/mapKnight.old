using System;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnightLibrary
{
	public class TextureManager
	{
		Dictionary<string,CCTexture2D> TextureCache;

		private static TextureManager GameTextureManagerInstanze;

		public TextureManager ()
		{
			TextureCache = new Dictionary<string, CCTexture2D>();
			TextureCache.Add ("1", new CCTexture2D ("tile"));
		}

		public static TextureManager GameTextureManager{
			get { return GameTextureManagerInstanze ?? (GameTextureManagerInstanze = new TextureManager ());}
		}

		public CCTexture2D GetTexture(string ID)
		{
			return TextureCache[ID];
		}
	}
}

