using System;

using CocosSharp;

namespace mapKnightLibrary
{
	public class Chest : CCSprite, IClickable 
	{
		//CCSprite MainChestSprite;

		CCPoint ClickBoxPosition;

		//ChestProperties
		public float ChestValue{ get; private set; }

		public Chest (CCTileMapCoordinates ChestPosition, CCTileMapLayer ChestLayer, float MapScale, CCSize MapSize)
		{
			CCSprite OriginalChest = ChestLayer.ExtractTile (ChestPosition, false);
			this.Texture = OriginalChest.Texture;
			this.TextureRectInPixels = OriginalChest.TextureRectInPixels;
			this.ContentSize = OriginalChest.ContentSize;

			this.ScaleX = MapScale;
			this.ScaleY = MapScale;
			//MainChestSprite.Position = new CCPoint (0, 0);
			//this.AddChild (MainChestSprite);
			this.Position = new CCPoint (ChestPosition.Column * ChestLayer.TileTexelSize.Width * MapScale, (MapSize.Height - ChestPosition.Row - 1) * ChestLayer.TileTexelSize.Height * MapScale);
			ChestLayer.RemoveTile (ChestPosition);
			ChestValue = 200f;
		}

		public delegate void ChestOpened(Chest OpenedChest);
		public event ChestOpened OnChestOpened;

		public void Clicked (CCTouch sender, TouchInfo info)
		{
			switch (info) {
			case TouchInfo.Ended:
				if (OnChestOpened != null) {
					OnChestOpened (this);
					CrossLog.Log (this, "User Clicked Chest @x=" + this.Position.X + ",y=" + this.Position.Y, MessageType.Debug);
				}
				break;
			}
		}

		public void UpdateClickPosition(CCPoint CameraPosition, CCSize RenderSize){
			ClickBoxPosition = new CCPoint (this.Position.X - CameraPosition.X + RenderSize.Width / 2 + this.Size.Width / 2, this.Position.Y - CameraPosition.Y + RenderSize.Height / 2 + this.Size.Height / 2);
		}

		public CCSize Size { get { return this.ScaledContentSize; } }

		public CCPoint Center { get { return ClickBoxPosition; } }

		public float MovedXChangeMin { get { return 1000f; } }

		public float MovedYChangeMin { get { return 40f; } }
	}
}

