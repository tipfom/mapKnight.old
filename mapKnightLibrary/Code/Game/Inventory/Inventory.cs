using System;
using System.Collections.Generic;

using CocosSharp;

namespace mapKnightLibrary
{
	namespace Inventory {
		public class GameInventory : CCLayer
		{
			CCSprite Sack;
			Clickable SackClickable;
			CCSprite DropDownMenu;
			CCSprite[] EquipedPotionSprites;
			Clickable[] EquipedPotionClickables;
			//CCSprite[] EquipedItemSprites;
			//Clickable[] EquipedItemClickables;

			List<IItem> CollectedItems;
			public Dictionary<EquipSlot,IItem> EquipedItems;

			bool MenuOpened;
			ClickManager ClickManager;
			CCSize ScreenSize;

			public GameInventory (ClickManager clickManager, List<IPotion> SelectedPotions, List<IEquipable> SelectedEquipment, CCSize screenSize) : base()
			{
				MenuOpened = false;

				CollectedItems = new List<IItem> ();
				EquipedItems = new Dictionary<EquipSlot, IItem> ();
				ClickManager = clickManager;
				ScreenSize = screenSize;

				//Initialisierung des linken oberen ClickDings
				Sack = new CCSprite ("interface_sack.png") { IsAntialiased = false };
				Sack.Scale = ScreenSize.Height / 8 / Sack.TextureRectInPixels.Size.Height;
				Sack.Position = new CCPoint (Sack.ScaledContentSize.Width / 2, ScreenSize.Height - Sack.ScaledContentSize.Height / 2);
				SackClickable = Clickable.BySprite (Sack, new CCSize (100, 5000));
				SackClickable.ClickedEvent += HandleMenuClicks;
				ClickManager.AddObject (SackClickable);


				//Initialisierung der ItemBilder
				EquipedPotionSprites = new CCSprite[3];
				EquipedPotionClickables = new Clickable[3];
				if (SelectedPotions.Count > 0) {
					EquipedItems.Add (EquipSlot.PotionOne, SelectedPotions [0]);
					EquipedPotionSprites [0] = new CCSprite () { };
					EquipedPotionSprites [0].Texture = EquipedItems [EquipSlot.PotionOne].PreviewImage;
					EquipedPotionSprites [0].IsAntialiased = false;
					EquipedPotionSprites [0].Scale = ScreenSize.Height / 10 / EquipedPotionSprites [0].TextureRectInPixels.Size.Height;
					EquipedPotionSprites [0].Position = new CCPoint (Sack.Position.X - EquipedPotionSprites [0].ScaledContentSize.Width / 2, Sack.Position.Y - Sack.ScaledContentSize.Height - EquipedPotionSprites [0].ScaledContentSize.Height / 2 - 10f);
					EquipedPotionClickables [0] = Clickable.BySprite (EquipedPotionSprites [0], new CCSize (5000, 5000));
					}
				if(SelectedPotions.Count > 1)
				{
					EquipedItems.Add (EquipSlot.PotionTwo, SelectedPotions [1]);
					EquipedPotionSprites [1] = new CCSprite () { };
					EquipedPotionSprites [1].Texture = EquipedItems [EquipSlot.PotionTwo].PreviewImage;
					EquipedPotionSprites [1].IsAntialiased = false;
					EquipedPotionSprites [1].Scale = ScreenSize.Height / 10 / EquipedPotionSprites [1].TextureRectInPixels.Size.Height;
					EquipedPotionSprites [1].Position = new CCPoint (Sack.Position.X - EquipedPotionSprites [1].ScaledContentSize.Width / 2, EquipedPotionSprites [0].Position.Y - EquipedPotionSprites [0].ScaledContentSize.Height / 2 - EquipedPotionSprites [1].ScaledContentSize.Height / 2 - 10f);
					EquipedPotionClickables [1] = Clickable.BySprite (EquipedPotionSprites [1], new CCSize (5000, 5000));
				}
				if (SelectedPotions.Count > 2) {
					EquipedItems.Add (EquipSlot.PotionThree, SelectedPotions [2]);
					EquipedPotionSprites [2] = new CCSprite () { };
					EquipedPotionSprites [2].Texture = EquipedItems [EquipSlot.PotionThree].PreviewImage;
					EquipedPotionSprites [2].IsAntialiased = false;
					EquipedPotionSprites [2].Scale = ScreenSize.Height / 10 / EquipedPotionSprites [2].TextureRectInPixels.Size.Height;
					EquipedPotionSprites [2].Position = new CCPoint (Sack.Position.X - EquipedPotionSprites [2].ScaledContentSize.Width / 2, EquipedPotionSprites [1].Position.Y - EquipedPotionSprites [1].ScaledContentSize.Height / 2 - EquipedPotionSprites [2].ScaledContentSize.Height / 2 - 10f);
					EquipedPotionClickables [2] = Clickable.BySprite (EquipedPotionSprites [2], new CCSize (5000, 5000));
				}

				if (SelectedEquipment.Count > 4) {
					foreach (IEquipable ArmorPart in SelectedEquipment.FindAll((Equipment) => Equipment is IArmor)) {
						EquipedItems.Add (ArmorPart.EquipSlot, ArmorPart);
					}
					EquipedItems.Add (EquipSlot.Weapon, SelectedEquipment.Find ((Equipment) => Equipment is IWeapon));
				} else {
					//throw new ArgumentException ("Not enough equipment selected");
				}
			}

			protected override void AddedToScene ()
			{
				base.AddedToScene ();

				this.AddChild (Sack);

				DropDownMenu = new CCSprite ("interface_dropdownmenu.png") { IsAntialiased = false };
				DropDownMenu.Scale = Sack.ScaleX;
				DropDownMenu.Position = new CCPoint (Sack.Position.X, Sack.Position.Y - DropDownMenu.ScaledContentSize.Height / 2);
			}

			void HandleMenuClicks(object sender, TouchInfo e){
				switch (e) {
				case TouchInfo.Ended:
					if (MenuOpened) {
						this.RemoveChild (EquipedPotionSprites [0]);
						this.RemoveChild (EquipedPotionSprites [1]);
						this.RemoveChild (EquipedPotionSprites [2]);
						this.RemoveChild (DropDownMenu);
						ClickManager.RemoveManyObjects (EquipedPotionClickables);
						MenuOpened = !MenuOpened;
					} else {
						this.AddChild (DropDownMenu, -1);
						this.AddChild (EquipedPotionSprites [0], -1);
						this.AddChild (EquipedPotionSprites [1], -1);
						this.AddChild (EquipedPotionSprites [2], -1);
						ClickManager.AddManyObjects (EquipedPotionClickables);
						MenuOpened = !MenuOpened;
					}
					break;
				}
			}
		}
	}
}