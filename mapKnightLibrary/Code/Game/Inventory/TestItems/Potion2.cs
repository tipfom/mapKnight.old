﻿using System;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Inventory
	{
		public class Potion2 : IPotion
		{
			public Potion2 ()
			{
			}

			#region IPotion implementation

			public CocosSharp.CCAnimation CharacterLookChange {
				get {
					return new CCAnimation ();
				}
			}

			#endregion

			#region IConsumable implementation

			public Dictionary<Attribute, short> AttributeChange {
				get {
					Dictionary<Attribute,short> ReturnedDictionary = new Dictionary<Attribute, short> ();
					ReturnedDictionary.Add (Attribute.ManaRegeneration, 3);
					return ReturnedDictionary;
				}
			}

			public float EffectTime {
				get {
					return 0;
				}
			}

			public bool AttributeChangeOverTime {
				get {
					return false;
				}
			}

			#endregion

			#region IItem implementation

			public Dictionary<Attribute, short> StaticEffect {
				get {
					throw new NotImplementedException ();
				}
			}

			public string name {
				get {
					return "Potion2_test"; 
				}
			}

			public string ID {
				get {
					return "1";
				}
			}

			public CCTexture2D PreviewImage {
				get {
					return new CCTexture2D ("batterie_empty.png");
				}
			}

			public short Cost {
				get {
					return 0;
				}
			}

			public float StackCount {
				get {
					return 1;
				}
			}

			#endregion
		}
	}
}