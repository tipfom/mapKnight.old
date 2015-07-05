using System;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Inventory{
		public class NonePotion : IPotion 
		{
			public NonePotion ()
			{
				
			}

			#region IPotion implementation

			public CCAnimation CharacterLookChange {
				get {
					throw new NotImplementedException ();
				}
			}

			#endregion

			#region IConsumable implementation

			public System.Collections.Generic.Dictionary<Attribute, short> AttributeChange {
				get {
					throw new NotImplementedException ();
				}
			}

			public float EffectTime {
				get {
					throw new NotImplementedException ();
				}
			}

			public bool AttributeChangeOverTime {
				get {
					throw new NotImplementedException ();
				}
			}

			#endregion

			#region IItem implementation

			public string name {
				get {
					throw new NotImplementedException ();
				}
			}

			public short ID {
				get {
					throw new NotImplementedException ();
				}
			}

			public CocosSharp.CCTexture2D PreviewImage {
				get {
					throw new NotImplementedException ();
				}
			}

			public short Cost {
				get {
					throw new NotImplementedException ();
				}
			}

			public float StackCount {
				get {
					throw new NotImplementedException ();
				}
			}

			public System.Collections.Generic.Dictionary<Attribute, short> StaticEffect {
				get {
					throw new NotImplementedException ();
				}
			}

			#endregion
		}
	}
}

