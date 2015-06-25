using System;
using System.Collections.Generic;

using CocosSharp;

using Box2D.Common;

namespace mapKnightLibrary
{
	public class TMXLayerDataLoader
	{
		public static List<Platform> LoadPlatformFromLayer(CCTileMap TileMap, CCTileMapObjectGroup PlatformHolder, Container gameContainer){
			List<Platform> LoadedPlatforms = new List<Platform> ();

			foreach (Dictionary<string,string> LayerObject in PlatformHolder.Objects) {
				if (LayerObject.ContainsKey ("type") == true) {
					if (LayerObject ["type"] == "platform") {
						int LoadedSpeed = 200;
						if (LayerObject.ContainsKey ("speed"))
							LoadedSpeed = Convert.ToInt32 (LayerObject ["speed"]);
						List<CCPoint> LoadedWaipoints = new List<CCPoint> ();
						LoadedWaipoints.Add (new CCPoint ((float)Convert.ToInt32 (LayerObject ["x"]) * TileMap.ScaleX, (float)Convert.ToInt32 (LayerObject ["y"]) * TileMap.ScaleY));
						if (LayerObject.ContainsKey ("waypoints") == true) {
							foreach (string WayPointPair in LayerObject ["waypoints"].Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries)) {
								CCPoint TempLoadedPoint = new CCPoint ();
								string[] Waypoint = WayPointPair.Split (new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
								if (Waypoint.Length > 1) {
									TempLoadedPoint.X = LoadedWaipoints [LoadedWaipoints.Count - 1].X + (float)Convert.ToInt32 (Waypoint [0]) * TileMap.ScaleX * TileMap.TileTexelSize.Width;
									TempLoadedPoint.Y = LoadedWaipoints [LoadedWaipoints.Count - 1].Y + (float)Convert.ToInt32 (Waypoint [1]) * TileMap.ScaleY * TileMap.TileTexelSize.Height;
								} else {
									throw new ArgumentException ("Incorrect Waypoints");
								}
								LoadedWaipoints.Add (TempLoadedPoint);
							}
						}
						LoadedPlatforms.Add (new Platform (LoadedWaipoints, LoadedSpeed, gameContainer));
					}
				}
			}

			return LoadedPlatforms;
		}

		public static List<JumpPad> LoadJumpPadFromLayer(CCTileMap TileMap, CCTileMapObjectGroup PlatformHolder, Container gameContainer){
			List<JumpPad> LoadedJumpPads = new List<JumpPad> ();

			foreach (Dictionary<string,string> LayerObject in PlatformHolder.Objects) {
				if (LayerObject.ContainsKey ("type") == true) {
					if (LayerObject ["type"] == "jumppad") {
						b2Vec2 LoadedBoostVector = new b2Vec2 ();
						if (LayerObject.ContainsKey ("boostvec")) {
							string[] BoostVecData = LayerObject ["boostvec"].Split (new char[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);
							if (BoostVecData.Length > 1) {
								LoadedBoostVector = new b2Vec2 ((float)Convert.ToInt32 (BoostVecData [0]), (float)Convert.ToInt32 (BoostVecData [1]));
							}
						}
						CCPoint LoadedPosition;
						LoadedPosition = new CCPoint ((float)Convert.ToInt16 (LayerObject ["x"]) * TileMap.ScaleX - JumpPad.JumpPadSize.Width / 2, (float)Convert.ToInt32 (LayerObject ["y"]) * TileMap.ScaleY - JumpPad.JumpPadSize.Height);
						LoadedJumpPads.Add (new JumpPad (LoadedBoostVector, LoadedPosition, gameContainer.physicsHandler.gameWorld));
					}
				}
			}

			return LoadedJumpPads;
		}
	}
}

