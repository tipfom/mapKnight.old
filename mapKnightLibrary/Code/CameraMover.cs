using System;

using CocosSharp;

using Box2D.Common;

namespace mapKnightLibrary
{
	public class CameraMover
	{
		CameraBox cameraBox;

		public CCPoint CameraCenter;

		public CameraMover (CCPoint targetPosition, CCSize cameraBoxSize)
		{
			cameraBox = new CameraBox (cameraBoxSize, new b2Vec2 (targetPosition.X, targetPosition.Y));
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

			static float YOffset = 100f;
			public b2Vec2 CameraCenter;

			b2Vec2 Velocity;

			public CameraBox(CCSize size, b2Vec2 center)
			{
				right = center.x + size.Width / 2;
				left = center.x - size.Width / 2;
				top = center.y + size.Height / 2;
				bottom = center.y - size.Height / 2;

				Velocity = new b2Vec2 (0, 0);
				CameraCenter = center;
			}

			public void Update(b2Vec2 updatecenter, CCSize updatebodysize)
			{
				float shiftX = 0f;
				if (updatecenter.x - updatebodysize.Width / 2 < left) {
					shiftX = updatecenter.x - updatebodysize.Width / 2 - left;
				} else if (updatecenter.x + updatebodysize.Width / 2 > right) {
					shiftX = updatecenter.x + updatebodysize.Width / 2 - right;
				}
				left += shiftX;
				right += shiftX;

				float shiftY = 0f;
				if (updatecenter.y - updatebodysize.Height / 2 < bottom) {
					shiftY = updatecenter.y - updatebodysize.Height / 2 - bottom;
				} else if (updatecenter.y + updatebodysize.Height / 2 > top) {
					shiftY = updatecenter.y + updatebodysize.Height / 2 - top;
				}
				top += shiftY;
				bottom += shiftY;

				CameraCenter = new b2Vec2 ((left + right) / 2, (top + bottom) / 2 + YOffset);
				Velocity = new b2Vec2 (shiftX, shiftY);
			} 
	}
	}
}