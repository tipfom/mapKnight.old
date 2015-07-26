# mapKnight

Project mapKnight created by two german students is a sidescroller with rpg-elements.
It is going to use Map API's to detect the users current position and change the map theme by his current terrain.


Changelog (german) : 

!!! 13.06.2015 !!!

- improved the camerafollow script
- improved the hitbox-creation-algorythm

- added a function to load the mapcollusions from the map it self

- created a platform texture

!!! 14.06.2015 !!!
- coded the platform

- implemented a function to recognize the current ground type
- implemented the android immersive mode
- added a background to the game
- added a life and mana display

- improved the map

!!! 19.06.2015 !!!
- completed the interface

- improved the rendering
- improved the creation of the characters collusion body

- added a slide-on-a-wall function to the character
- implemented the jumppad

!!! 20.06.2015 !!!
- implemented a doublejump mechanic
- added the walljump

- improved the UI
  - added a main menu
- improved the android immersive mode

!!! 21.06.2015 !!!
- added a new way to control the character ( by buttons )
- added an optionwindow
  - customizable control types
- implemented a function allowing the mapcreator to define platform- and jumppadposition in the map itself

- removed a bug causing the doublejump to disable while standing on a platform

- improved the SQL-data algorythm

!!! 22.06.2015 !!!
- improved the walljump
- improved the platform-from-map-loading-algorythm

- many little changes

!!! 23.06.2015 !!!
- improved the map

!!! 24.06.2015 !!!
- slightly added some particles

!!! 25.06.2015 !!!
- changed the texture of the jumppad

!!! 27.06.2015 !!!
- done grafical work on the jumppad

- added a real walljump
- added a loadingscreen

- changed gameplay mechanics (walljump => wallclimb)
- changed the jumpmechanik
- changed some textures
- changed the map
- changed the UI

!!! 28.06.2015 !!!
- added a new charactermodel with custom movement animations
- added a chest
- added a function to change tiles while runtime

- changed the "CocosSharp" Version tp 1.5.0.0
  - http://forums.xamarin.com/discussion/44195/cocossharp-v1-5-0-0-release#latest
  - http://forums.xamarin.com/discussion/44192/cocossharp-v1-5-0-0-the-new-renderer-pipeline
- changed the UI
- changed parts of the map

- balanced some gameplay things
- improved the walljump

!!! 30.06.2015 !!!
- implementation of a System to save data in 
  - a SQLDatabase
  - a DataFile
  - code (useless since 09.07.2015)

!!! 01.07.2015 !!!
- changed the Character Animations

!!! 02.07.2015 !!!
- changed the Camera Follow
- changed the Interface

- improved the characters animations

!!! 03.07.2015 !!!
- added a clickable Chest to the Game

- changed the way touches are recognized

!!! 05.07.2015 !!!
*Running into the HotAndHotter Alpha Version! :D
What will it bring to the game? 
The Answer is an Inventory with many items such like potions, armors (which also effect the players look), weapons and a crafting-system and probably some network stuff(bluetooth for example).
- implemented an Inventory Container
- implemented a Icon to the interface which acts like a drop down menu to use your selected potions
- implemented some interfaces and enumerations
- implemented characterattributes

- fixed a bug which causes the character not to change the animation while faling

!!! 09.07.2015 !!!
- added custom armors with different look and attributes to the game

- changed the gamefont ( now == http://www.schriftarten-fonts.de/fonts/9761/04b30.html )

!!! 26.07.2015 !!!
- added custom build properties
- added custom vertices to the box2d<->map parser
- poorly added logging

- updated some packages

- fixed some performance issues
- fixed some bugs causing the app to lag
