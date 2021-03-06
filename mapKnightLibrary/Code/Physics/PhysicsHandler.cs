﻿using System;
using System.Collections.Generic;

using CocosSharp;

using Box2D;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Rope;

namespace mapKnightLibrary
{
	public class PhysicsHandler
	{
		public b2World gameWorld;

		b2Body Ground;

		public DebugDraw debugDrawer;

		public static float pixelPerMeter = 50f;

		public CollusionSensor collusionSensor;

		public PhysicsHandler ()
		{
			
		}

		public void Initialize(float mapSizeWidth, Character Character, CCTileMapLayer physicsLayer, CCTileMap physicsMap, Container gameContainer)
		{
			mapSizeWidth /= pixelPerMeter;
			gameWorld = new b2World (new b2Vec2 (0, -15f));
			gameWorld.AllowSleep = false;

			//definiert den MainCharacter
			Character.createPhysicsBody (gameWorld);
			defineGround (mapSizeWidth);

			createBox2DWorldByLayer (physicsLayer, physicsMap);

			debugDrawer = new DebugDraw ();
			gameWorld.SetDebugDraw (debugDrawer);
			debugDrawer.AppendFlags (b2DrawFlags.e_shapeBit);

			collusionSensor = new CollusionSensor (gameContainer);
			gameWorld.SetContactListener (collusionSensor);
		}

		private void defineGround (float mapSizeWidth)
		{
			//definiert den Boden

			b2BodyDef groundDef = new b2BodyDef ();
			groundDef.position = new b2Vec2 (0, -3f);

			Ground = gameWorld.CreateBody(groundDef);

			b2PolygonShape groundShape = new b2PolygonShape ();
			groundShape.SetAsBox (mapSizeWidth, 3f);

			Ground.CreateFixture (groundShape,0.0f);	
		}

		public void step(float frameTime)
		{
			gameWorld.Step (frameTime, 6, 2);
		}

		#region how to create a physicsmap

		private void createBox2DWorldByLayer(CCTileMapLayer physicsLayer, CCTileMap physicsMap)
		{
			CrossLog.Log (this, "Start creating the 2DWorld", MessageType.Debug);
			bool[,] Tile = extractHitboxTiles (physicsLayer, physicsMap);
			bool[,] Checked = new bool[(int)physicsLayer.LayerSize.Size.Width, (int)physicsLayer.LayerSize.Size.Height];

			//geht das Abbild durch
			int CurrentWidth;
			CurrentWidth = 0;

			int LastX;
			LastX = 0;

			for (int y = 0; y < physicsLayer.LayerSize.Size.Height; y++) {
				for (int x = 0; x < physicsLayer.LayerSize.Size.Width; x++) {
					if (Tile [x, y] == false) {
						if (CurrentWidth > 0) {
							createBoxAt (LastX * (int)physicsLayer.TileTexelSize.Width * (int)physicsMap.ScaleX, ((int)physicsLayer.LayerSize.Size.Height - y - 1) * (int)physicsLayer.TileTexelSize.Height * (int)physicsMap.ScaleY, CurrentWidth * physicsLayer.TileTexelSize.Width * physicsMap.ScaleX, physicsLayer.TileTexelSize.Height * (int)physicsMap.ScaleY);
						}
						LastX = x + 1;
						CurrentWidth = 0;
					} else {
						CurrentWidth += 1;
					}
				}
				if (CurrentWidth > 0) {
					createBoxAt (LastX * (int)physicsLayer.TileTexelSize.Width, ((int)physicsLayer.LayerSize.Size.Height - y - 1) * (int)physicsLayer.TileTexelSize.Height, CurrentWidth * physicsLayer.TileTexelSize.Width, physicsLayer.TileTexelSize.Height);
				}
				LastX = 0;
				CurrentWidth = 0;
			}

			string[,] CustomTile = extractCustomTiles (physicsLayer, physicsMap);

			for (int y = 0; y < physicsLayer.LayerSize.Size.Height; y++) {
				for (int x = 0; x < physicsLayer.LayerSize.Size.Width; x++) {
					if (CustomTile [x, y] != null) {
						createCustomBodyAt (x * (int)physicsLayer.TileTexelSize.Width * (int)physicsMap.ScaleX, ((int)physicsLayer.LayerSize.Size.Height - y - 1) * (int)physicsLayer.TileTexelSize.Height * (int)physicsMap.ScaleY, physicsLayer.TileTexelSize.Width * physicsMap.ScaleX, physicsLayer.TileTexelSize.Height * physicsMap.ScaleY, CustomTile [x, y]);
					}
				}
			}
			CrossLog.Log (this, "Ended creating the 2DWorld", MessageType.Debug);
		}

		private bool[,] extractHitboxTiles(CCTileMapLayer physicsLayer, CCTileMap physicsMap){
			bool[,] Tile = new bool[(int)physicsLayer.LayerSize.Size.Width, (int)physicsLayer.LayerSize.Size.Height];

			//erstellt ein bool Abbild der Map
			Dictionary<string,string> tileProperties;
			for (int x = 1; x < physicsLayer.LayerSize.Size.Width - 1; x++) {
				for (int y = 1; y < physicsLayer.LayerSize.Size.Height - 1; y++) {
					//wenn das tile auf einer Seite offen ist, dann ist es ein Teil der MapHitBox
					//und das tile eine quadr. hitbox hat
					tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x, y)).Gid);
					if (tileProperties != null && tileProperties.ContainsKey("shapetype") && tileProperties["shapetype"] == "box") {
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x - 1, y)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = true;
						}
						tileProperties = null;
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x + 1, y)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = true;
						}
						tileProperties = null;
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x, y - 1)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = true;
						}
						tileProperties = null;
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x, y + 1)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = true;
						}
						tileProperties = null;
					}
				}
			}
			return Tile;
		}

		private string[,] extractCustomTiles(CCTileMapLayer physicsLayer, CCTileMap physicsMap){
			string[,] Tile = new string[(int)physicsLayer.LayerSize.Size.Width, (int)physicsLayer.LayerSize.Size.Height];

			//erstellt ein bool Abbild der Map
			Dictionary<string,string> tileProperties;
			for (int x = 1; x < physicsLayer.LayerSize.Size.Width - 1; x++) {
				for (int y = 1; y < physicsLayer.LayerSize.Size.Height - 1; y++) {
					//wenn das tile auf einer Seite offen ist, dann ist es ein Teil der MapHitBox
					//und das tile eine spezielle hitbox hat
					tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x, y)).Gid);

					if (tileProperties != null && tileProperties.ContainsKey("shapetype") && tileProperties["shapetype"] == "custom") {
						string shapevertexstring;
						if (!tileProperties.ContainsKey ("shapevertex"))
							throw new FormatException ("error in map at tile x=" + x.ToString () + " y=" + y.ToString () + " (tile's shapetype has been set to custom, but no shapevertex property was added)");
						else
							shapevertexstring = tileProperties ["shapevertex"];

						//-----------------------------------------------------------------------------
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x - 1, y)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = shapevertexstring;
						}
			
						//-----------------------------------------------------------------------------
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x + 1, y)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = shapevertexstring;
						}

						//-----------------------------------------------------------------------------
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x, y - 1)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = shapevertexstring;
						}

						//-----------------------------------------------------------------------------
						tileProperties = physicsMap.TilePropertiesForGID (physicsLayer.TileGIDAndFlags (new CCTileMapCoordinates (x, y + 1)).Gid);
						if (tileProperties == null) {
							Tile [x, y] = shapevertexstring;
						}
					}
				}
			}
			return Tile;			
		}

		private void createBoxAt(int x, int y, float pixelWidth, float pixelHeight)
		{
			b2BodyDef boxDef = new b2BodyDef ();
			boxDef.type = b2BodyType.b2_staticBody;
			boxDef.position = new b2Vec2 (x / pixelPerMeter + pixelWidth / pixelPerMeter / 2, y / pixelPerMeter + pixelHeight / pixelPerMeter / 2);
			boxDef.userData = WorldFixtureData.ground;

			b2Body boxBody = gameWorld.CreateBody (boxDef);

			b2PolygonShape boxShape = new b2PolygonShape ();
			boxShape.SetAsBox (pixelWidth / pixelPerMeter / 2, pixelHeight / pixelPerMeter / 2);// :2 wegen des Aufstellen bugs

			b2FixtureDef boxFixture = new b2FixtureDef ();
			boxFixture.shape = boxShape;
			boxFixture.density = 1.0f;
			boxFixture.friction = 0.0f;
			boxFixture.restitution = 0.0f;
			boxFixture.userData = WorldFixtureData.ground;

			boxBody.CreateFixture (boxFixture);

			#if LOGMAPCREATION
			CrossLog.Log (this, "\t\tCreated StandartBoxTile at x=" + x.ToString () +",y=" + y.ToString (), MessageType.Debug);
			#endif
		}

		private void createCustomBodyAt(int x,int y, float pixelWidth, float pixelHeight, string shape){

			b2BodyDef BlockDef = new b2BodyDef ();
			BlockDef.type = b2BodyType.b2_staticBody;
			BlockDef.position = new b2Vec2 (x / pixelPerMeter + pixelWidth / pixelPerMeter / 2, y / pixelPerMeter + pixelHeight / pixelPerMeter / 2);
			BlockDef.userData = WorldFixtureData.ground;

			b2Body BlockBody = gameWorld.CreateBody (BlockDef);

			b2PolygonShape BlockShape = new b2PolygonShape (); 
			string[] UnformattedBlockVertices = shape.Split (';');
			b2Vec2[] BlockVertices = new b2Vec2[UnformattedBlockVertices.Length];
			for (int i = 0; i < UnformattedBlockVertices.Length; i++) {
				BlockVertices [i] = new b2Vec2 ((float)Convert.ToInt32 (UnformattedBlockVertices [i].Split (',') [0]) * pixelWidth / 2 / PhysicsHandler.pixelPerMeter,
					(float)Convert.ToInt32 (UnformattedBlockVertices [i].Split (',') [1]) * pixelHeight / 2 / PhysicsHandler.pixelPerMeter);
			}
			BlockShape.Set (BlockVertices, BlockVertices.Length);

			b2FixtureDef BlockFixture = new b2FixtureDef ();
			BlockFixture.shape = BlockShape;
			BlockFixture.density = 1.0f;
			BlockFixture.friction = 0.0f;
			BlockFixture.restitution = 0.0f;
			BlockFixture.userData = WorldFixtureData.ground;

			BlockBody.CreateFixture (BlockFixture);

			#if LOGMAPCREATION
			CrossLog.Log (this, "\t\tCreated CustomTile at x=" + x.ToString () +",y=" + y.ToString (), MessageType.Debug);
			CrossLog.Log (this, "\t\t\"TileVertices="+shape, MessageType.Debug);
			#endif
		}

		#endregion
	}
}