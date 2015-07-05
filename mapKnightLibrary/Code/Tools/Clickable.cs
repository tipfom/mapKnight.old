using System;

using CocosSharp;

namespace mapKnightLibrary
{
	public class Clickable : IClickable
	{
		CCPoint center;
		CCSize size;
		float ChangeX, ChangeY;

		public Clickable (CCSize ClickableSize, CCPoint ClickableCenter, CCSize ClickableMovedSize)
		{
			center = ClickableCenter;
			size = ClickableSize;
			ChangeX = ClickableMovedSize.Width;
			ChangeY = ClickableMovedSize.Height;
		}

		public event EventHandler<TouchInfo> ClickedEvent;

		public void Clicked (CCTouch sender, TouchInfo info)
		{
			ClickedEvent (sender, info);
		}

		public CocosSharp.CCSize Size {get { return size; } }

		public CocosSharp.CCPoint Center { get{ return center; }}

		public float MovedXChangeMin { get { return ChangeX; } }

		public float MovedYChangeMin { get { return ChangeY; } }

		public static Clickable BySprite(CCSprite Sprite, CCSize ClickableMovedSize)
		{
			return new Clickable (Sprite.ScaledContentSize, new CCPoint (Sprite.Position.X, Sprite.Position.Y), ClickableMovedSize);
		}
	}
}

