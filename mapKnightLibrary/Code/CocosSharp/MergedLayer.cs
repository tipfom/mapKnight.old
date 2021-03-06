﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CocosSharp;

using Box2D;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Rope;

namespace mapKnightLibrary
{
	public class MergedLayer : CCLayer
	{
		CCSprite Background;

		CCTileMap Map;
		CCTileMapObjectGroup pointLayer;

		public string mapVersion, mapCreator,mapName;

		CCSize screenSize;

		Container gameContainer;

		public CameraMover cameraMover;

		public MergedLayer (CCWindow mainWindow, Container mainContainer)
		{
			screenSize = CCScene.DefaultDesignResolutionSize;
			gameContainer = mainContainer;

			mapVersion = "unspecified";
			mapCreator = "unspecified";
			mapName = "unspecified";

		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Background = new CCSprite ("background");
			Background.ScaleX = screenSize.Width / Background.ContentSize.Width;
			Background.ScaleY = screenSize.Height / Background.ContentSize.Height;

			Map = new CCTileMap (new CCTileMapInfo ("tilemaps/bitte.tmx")); 
			CrossLog.Log (this, "Loaded Map", MessageType.Debug);
			//bei einer Invocation Exception einfach ne neue Map machen
			//vielleicht auch mal schauen, ob die XML_Datei korrekt ist(Attribute, fehlende < etc.)
			//wenn die Maplayer nicht vollständig geladen werden, muss man die Compression umstellen

			//mapproperties
			if (Map.MapPropertyNamed ("Creator") != null)
				mapCreator = Map.MapPropertyNamed ("Creator");
			if (Map.MapPropertyNamed ("Version") != null)
				mapVersion = Map.MapPropertyNamed ("Version");
			if (Map.MapPropertyNamed ("Name") != null)
				mapName = Map.MapPropertyNamed ("Name");

			CrossLog.Log (this, "\tName = " + mapName, MessageType.Info);
			CrossLog.Log (this, "\tVersion = " + mapVersion, MessageType.Info);
			CrossLog.Log (this, "\tCreator = " + mapCreator, MessageType.Info);

			Map.Scale = 3f; //Scale auch beim Spawnpoint ändern
			Map.Antialiased = false;
			//Map.LayerNamed ("mainlayer")
			pointLayer = Map.ObjectGroupNamed ("points");
			Dictionary<string,string> spawnPoint = pointLayer.ObjectNamed ("SpawnPoint");

			gameContainer.mainCharacter.Position = new CCPoint ((float)Convert.ToInt32 (spawnPoint ["x"]) * Map.ScaleX, (float)Convert.ToInt32 (spawnPoint ["y"]) * Map.ScaleY);
			CrossLog.Log (this, "\tSpawnpoint = " + gameContainer.mainCharacter.Position.ToString(), MessageType.Info);

			gameContainer.physicsHandler.Initialize (Map.MapDimensions.Size.Width * Map.TileTexelSize.Width * Map.ScaleX, gameContainer.mainCharacter, Map.LayerNamed ("mainlayer"), Map, gameContainer);

			//Platform hinzufügen
			gameContainer.platformContainer.AddRange (TMXLayerDataLoader.LoadPlatformFromLayer (Map, Map.ObjectGroupNamed ("aditionallayer"), gameContainer));
			gameContainer.jumpPadContainer.AddRange (TMXLayerDataLoader.LoadJumpPadFromLayer (Map, Map.ObjectGroupNamed ("aditionallayer"), gameContainer));

			this.AddChild (Background, -1);

			this.AddChild (Map, 1);
			this.AddChild (gameContainer.mainCharacter.CharacterLayer, 2);

			this.AddChild (gameContainer.physicsHandler.debugDrawer.DrawNode, 0);

			foreach (Platform knownPlatform in gameContainer.platformContainer) {
				this.AddChild (knownPlatform);
			}
			foreach (JumpPad knownJumpPad in gameContainer.jumpPadContainer) {
				this.AddChild (knownJumpPad);
			}

			//particle init

			//camera move init
			cameraMover = new CameraMover (gameContainer.mainCharacter.Position, new CCSize (150, 200), new CCSize (Map.TileTexelSize.Width * Map.MapDimensions.Size.Width * Map.ScaleX, Map.TileTexelSize.Height * Map.MapDimensions.Size.Height * Map.ScaleY), screenSize);

			//gameContainer.chestContainer.Add (new Chest (new CCTileMapCoordinates (132, 74), Map.LayerNamed ("chestlayer"), Map.ScaleX, Map.MapDimensions.Size));
			foreach (Chest knownChest in gameContainer.chestContainer) {
				this.AddChild (knownChest, 1);
			}

			Schedule (Update);
		}


		public override void Update (float dt)
		{
			cameraMover.Update (gameContainer.mainCharacter.Position, gameContainer.mainCharacter.Size);

			//Zeit um folgendes herauszufinden : 2 Tage
			this.Position = new CCPoint (screenSize.Width / 2 - cameraMover.CameraCenter.X, screenSize.Height / 2 - cameraMover.CameraCenter.Y);
			Map.TileLayersContainer.Position = new CCPoint (screenSize.Width / 2 - cameraMover.CameraCenter.X / Map.ScaleX, screenSize.Height / 2 - cameraMover.CameraCenter.Y / Map.ScaleY);
			Map.Position = new CCPoint (cameraMover.CameraCenter.X - screenSize.Width / 2 * Map.ScaleX, cameraMover.CameraCenter.Y - screenSize.Height / 2 * Map.ScaleY);

			Background.Position = new CCPoint (cameraMover.CameraCenter.X, cameraMover.CameraCenter.Y);
		}
			
		public CCPoint MapPosition{get{ return Map.Position;}}
	}
}