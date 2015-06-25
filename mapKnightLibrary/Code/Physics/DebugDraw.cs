using System;

using CocosSharp;

using Box2D.Collision;
using Box2D.Common;

namespace mapKnightLibrary
{
	public class DebugDraw : Box2D.Common.b2Draw
	{
		public CCDrawNode DrawNode;

		CCColor4B CollusionColor;

		public DebugDraw ()
		{
			DrawNode = new CCDrawNode ();
			CollusionColor = new CCColor4B (255, 0, 0, 255);
		}

		public void Render()
		{
			DrawNode.Render ();
			DrawNode.Clear ();
		}

		public override void DrawPolygon (Box2D.Common.b2Vec2[] vertices, int vertexCount, Box2D.Common.b2Color color)
		{
			CCPoint[] verticesToPoint = new CCPoint[vertexCount];
			CCColor4B Color = new CCColor4B (color.r, color.g, color.b, 255);
			for (int i = 0; i < vertexCount; i++) {
				verticesToPoint [i] = new CCPoint (vertices [i].x * 50f, vertices [i].y * 50f);
			}

			DrawNode.DrawPolygon (verticesToPoint, vertexCount, new CCColor4B (0, 0, 0, 0), 3f, Color);
		}

		public override void DrawSolidPolygon (Box2D.Common.b2Vec2[] vertices, int vertexCount, Box2D.Common.b2Color color)
		{
			CCPoint[] verticesToPoint = new CCPoint[vertexCount];
			CCColor4B Color = new CCColor4B (color.r, color.g, color.b, 255);
			for (int i = 0; i < vertexCount; i++) {
				verticesToPoint [i] = new CCPoint (vertices [i].x * 50f, vertices [i].y * 50f);
			}

			DrawNode.DrawPolygon (verticesToPoint, vertexCount, Color, 3f, Color);
		}

		public override void DrawCircle (Box2D.Common.b2Vec2 center, float radius, Box2D.Common.b2Color color)
		{
			CCPoint centerToPoint = new CCPoint (center.x * 50f, center.y * 50f);
			//CCColor4B Color = new CCColor4B (color.r, color.g, color.b, 255);

			DrawNode.DrawCircle (centerToPoint, radius, CollusionColor);
		}

		public override void DrawSolidCircle (Box2D.Common.b2Vec2 center, float radius, Box2D.Common.b2Vec2 axis, Box2D.Common.b2Color color)
		{
			CCPoint centerToPoint = new CCPoint (center.x * 50f, center.y * 50f);
			//CCColor4B Color = new CCColor4B (color.r, color.g, color.b, 255);

			DrawNode.DrawCircle (centerToPoint, radius, CollusionColor);
		}

		public override void DrawSegment (Box2D.Common.b2Vec2 p1, Box2D.Common.b2Vec2 p2, Box2D.Common.b2Color color)
		{
			throw new NotImplementedException ();
		}

		public override void DrawTransform (Box2D.Common.b2Transform xf)
		{
			throw new NotImplementedException ();
		}
	}
}

