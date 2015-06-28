using System;
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

		//particles
		GroundParticle PlayerMovingParticle;

		public MergedLayer (CCWindow mainWindow, Container mainContainer)
		{
			screenSize = mainWindow.WindowSizeInPixels;
			gameContainer = mainContainer;

			mapVersion = "unspecified";
			mapCreator = "unspecified";
			mapName = "unspecified";

		}


		protected override void AddedToScene ()
		{

			base.AddedToScene ();

			Background = new CCSprite ("background");
			Background.Scale = screenSize.Width / Background.ContentSize.Width;

			Map = new CCTileMap (new CCTileMapInfo ("tilemaps/bitte.tmx")); 
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

			Map.Scale = 3f; //Scale auch beim Spawnpoint ändern
			Map.Antialiased = false;
			//Map.LayerNamed ("mainlayer")
			pointLayer = Map.ObjectGroupNamed ("points");
			Dictionary<string,string> spawnPoint = pointLayer.ObjectNamed ("SpawnPoint");

			gameContainer.mainCharacter.Position = new CCPoint ((float)Convert.ToInt32 (spawnPoint ["x"]) * Map.ScaleX, (float)Convert.ToInt32 (spawnPoint ["y"]) * Map.ScaleY);
			gameContainer.physicsHandler.Initialize (Map.MapDimensions.Size.Width * Map.TileTexelSize.Width * Map.ScaleX, gameContainer.mainCharacter, Map.LayerNamed ("mainlayer"), Map, gameContainer);
			gameContainer.mainCharacter.bindToPhysicsHandler (gameContainer.physicsHandler);

			//Platform hinzufügen
			gameContainer.platformContainer.AddRange (TMXLayerDataLoader.LoadPlatformFromLayer (Map, Map.ObjectGroupNamed ("aditionallayer"), gameContainer));
			gameContainer.jumpPadContainer.AddRange (TMXLayerDataLoader.LoadJumpPadFromLayer (Map, Map.ObjectGroupNamed ("aditionallayer"), gameContainer));

			this.AddChild (Background, -1);

			this.AddChild (Map, 1);
			this.AddChild (gameContainer.mainCharacter.Sprite, 2);
			this.AddChild (gameContainer.physicsHandler.debugDrawer.DrawNode, 0);

			foreach (Platform knownPlatform in gameContainer.platformContainer) {
				this.AddChild (knownPlatform);
			}
			foreach (JumpPad knownJumpPad in gameContainer.jumpPadContainer) {
				this.AddChild (knownJumpPad);
			}

			//particle init

			PlayerMovingParticle = new GroundParticle ();
			this.AddChild (PlayerMovingParticle);
			PlayerMovingParticle.Position = gameContainer.mainCharacter.Position;

			Schedule (Update);
		}

		public void CenterCamera(){

			int x = (int)Math.Max (gameContainer.mainCharacter.Position.X, screenSize.Width / 2);
			int y = (int)Math.Max (gameContainer.mainCharacter.Position.Y, screenSize.Height / 2);

			x = (int)Math.Min (x, Map.MapDimensions.Size.Width * Map.TileTexelSize.Width * Map.ScaleX - screenSize.Width / 2);
			y = (int)Math.Min (y, Map.MapDimensions.Size.Height * Map.TileTexelSize.Height * Map.ScaleY - screenSize.Height / 2);
			//bestimmt, ob die x bzw y koordinate außerhalbe der map wäre

			//Zeit um folgendes herauszufinden : 2 Tage
			this.Position = new CCPoint (screenSize.Width / 2 - x, screenSize.Height / 2 - y);
			Map.TileLayersContainer.Position = new CCPoint (screenSize.Width / 2 - x / Map.ScaleX, screenSize.Height / 2 - y / Map.ScaleY);
			Map.Position = new CCPoint (x - screenSize.Width / 2 * Map.ScaleX, y - screenSize.Height / 2 * Map.ScaleY);

			Background.Position = new CCPoint (x,y);
		}

		public override void Update (float dt)
		{
			if (gameContainer.mainCharacter.MoveDirection != PlayerMovingParticle.ParticleAppearDirection)
				this.PlayerMovingParticle.ParticleAppearDirection = gameContainer.mainCharacter.MoveDirection;
			this.PlayerMovingParticle.Position = new CCPoint (gameContainer.mainCharacter.Position.X, gameContainer.mainCharacter.Position.Y - gameContainer.mainCharacter.Size.Height / 2);
		}
			
		public CCPoint MapPosition{get{ return Map.Position;}}

		public void UpdateTile(CCTileMapCoordinates tile, CCTileGidAndFlags updatedTileGID, MapLayer layer){
			Map.LayerNamed ("mainlayer").SetTileGID (updatedTileGID, tile);
		}
	}
}