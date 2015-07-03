using System;

using CocosSharp;

using Box2D.Common;

namespace mapKnightLibrary
{
	public class CameraMover
	{
		CameraBox cameraBox;

		public CCPoint CameraCenter;

		public CameraMover (CCPoint targetPosition, CCSize cameraBoxSize, CCSize MapSize, CCSize screenSize)
		{
			cameraBox = new CameraBox (cameraBoxSize, new b2Vec2 (targetPosition.X, targetPosition.Y), MapSize, screenSize);
		}

		public void Update(CCPoint targetPosition, CCSize targetSize)
		{
			cameraBox.Update (new b2Vec2 (targetPosition.X, targetPosition.Y), targetSize);
			CameraCenter = new CCPoint (cameraBox.CameraCenter.x, cameraBox.CameraCenter.y);
		}

		struct CameraBox
		{
			float right, left;
			float top, bottom;
			CCSize WorldSize;
			CCSize RenderSize;

			static float YOffset = 100f;
			public b2Vec2 CameraCenter;

			b2Vec2 Velocity;

			public CameraBox(CCSize size, b2Vec2 center, CCSize worldsize, CCSize rendersize)
			{
				right = center.x + size.Width / 2;
				left = center.x - size.Width / 2;
				top = center.y + size.Height / 2;
				bottom = center.y - size.Height / 2;

				Velocity = new b2Vec2 (0, 0);
				CameraCenter = center;
				WorldSize = worldsize;
				RenderSize = rendersize;
			}

			public void Update(b2Vec2 updatecenter, CCSize updatebodysize)
			{
				b2Vec2 unupdatedcenter;

				int x = (int)Math.Max (updatecenter.x, RenderSize.Width / 2);
				int y = (int)Math.Max (updatecenter.y, RenderSize.Height / 2);

				x = (int)Math.Min (x, WorldSize.Width - RenderSize.Width / 2);
				y = (int)Math.Min (y, WorldSize.Height - RenderSize.Height / 2);
				//bestimmt, ob die x bzw y koordinate außerhalbe der map wäre

				unupdatedcenter = new b2Vec2 (x, y);

				float shiftX = 0f;
				if (unupdatedcenter.x - updatebodysize.Width / 2 < left) {
					shiftX = unupdatedcenter.x - updatebodysize.Width / 2 - left;
				} else if (unupdatedcenter.x + updatebodysize.Width / 2 > right) {
					shiftX = unupdatedcenter.x + updatebodysize.Width / 2 - right;
				}
				left += shiftX;
				right += shiftX;

				float shiftY = 0f;
				if (unupdatedcenter.y - updatebodysize.Height / 2 < bottom) {
					shiftY = unupdatedcenter.y - updatebodysize.Height / 2 - bottom;
				} else if (unupdatedcenter.y + updatebodysize.Height / 2 > top) {
					shiftY = unupdatedcenter.y + updatebodysize.Height / 2 - top;
				}
				top += shiftY;
				bottom += shiftY;

				CameraCenter = new b2Vec2 ((left + right) / 2, (top + bottom) / 2 + YOffset);
				Velocity = new b2Vec2 (shiftX, shiftY);
			} 
	}
	}
}