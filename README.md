# mapKnight

Project mapKnight created by two german students is a sidescroller with rpg-elements.
It is going to use Map API's to detect the users current position and change the map theme by his current terrain.


Changelog (german) : 

!!! 13.06.2015 !!!

- Korrektur der Kamerapositionierung
- Laden der Hitboxstrukturen aus dem "MainLayer"
- Verbessern des Hitboxerstellungsallgorythmus
- Implementation von Platformen, welche sich bewegen k鰊nen
  * Erstellen der Textur

!!! 14.06.2015 !!!
- Vollständige Implementation der Platformen
- Implementation einer Untergrunderkennung
- Hinzufügen eines Hintergrunds zur Scene
- Verbessern der Map(Entfernen von unnötigen Pflanzen, Hinzufügen von Kästchen etc.)
- Implementation von Lebens- bzw. Manaanzeige
- Implementation von Displayerweiternden Funktionen(Immersive Mode)

!!! 19.06.2015 !!!
- Vervollständigung der Interfaceimplementation (beheben von bugs)
- Renderimprovements beim Interface (Events) //nicht sichtbar
- Verbesserung der Erstellung des characterbody's //nicht sichtbar
- Implementation einer Mechanik, welche es erlaubt langsamer an W鋘den hinab zu gleiten
- Implementation von JumpPads
- Erstellen der ersten publishbaren APK

!!! 20.06.2015 !!!
- Verbessern des Immersive Modes
- Einführen einer Doppelsprungmechanik
- Verbessern der Bedienoberfläche
- Einführen einer Doppelsprungmechanuk
- Einführen einer Walljumpmechanik
- Einführen eines Hauptmenü

!!! 21.06.2015 !!!
- Einführen einer neuen Methode den Character zu steuern
- Einführen eines Optionsmenü
  * Verstellbare Kontrollmöglichkeiten
- Implementation eines DatenSpeicherallgorythmus (unvollständig)
- tempöräres Beheben eines Bugs bei Abspringen von einer Platform
- Implementation von Funktionen, welche es erlauben Platformen und JumpPads aus der Map zu laden

!!! 22.06.2015 !!!
- Verbesserungen am Walljump
- Verbesserungen am Platformloading
- Vielzahl kleiner anderer Verbesserungen

!!! 23.06.2015 !!!
- Verbesserungen an der Map

!!! 24.06.2015 !!!
- Einführen eines Partikelsystems

!!! 25.06.2015 !!!
- Verändern der Textur des Jumppads
- Grafische Arbeiten am Character

!!! 27.06.2015 !!!
- Grafische Verbesserungen am Jumppad
- Änderung an der Gameplaymechanik (Walljump => Wallclimb)
- Einführen eines Walljumps
- Änderungen an der Sprungmechanik
- Änderung einiger Texturen
- Änderungen an der Map
- Einführen eines "Ladebildschirms"
- Änderungen am Interface

!!! 28.06.2015 !!!
- Umsteigen von Cocossharp V#1.4.0.0 auf V#1.5.0.1
  - Bessere Render Performance
  - Hinzufügen der Möglichkeit größere Tiles in der Map zu laden
  - http://forums.xamarin.com/discussion/44195/cocossharp-v1-5-0-0-release#latest
  - http://forums.xamarin.com/discussion/44192/cocossharp-v1-5-0-0-the-new-renderer-pipeline
- Änderungen am Interface
- Hinzufügen eines neuen Characters mit spezifischen Bewegungsanimationen
- Änderungen an der Map
- Ausgleichen einiger Gameplay-Werte
- Hinzufügen einer (in der jetzigen Version) nutzlosen Kiste
- Hinzufügen einer Funktion, die es der App erlaubt Tiles zur Laufzeit zu ändern
- Verbesserungen am Walljump

!!! 30.06.2015 !!!
- Implementation of a System to Save Data in 
  - a SQLDatabase
  - a DataFile
  - Code

!!! 01.07.2015 !!!
- Changed the Character Animations

!!! 02.07.2015 !!!
- Changed the Camera Follow
- Improved the characters animations
- Changed the Interface

!!! 03.07.2015 !!!
- Changed the way touches are recognized
- Added a clickable Chest to the Game

!!! 05.07.2015 !!!
Running into the HotAndHotter Alpha Version! :D
What will it bring to the game? 
The Answer is an Inventory with many items such like potions, armors (which also effect the players look), weapons and a crafting-system and probably some network stuff(bluetooth for example).
- Implemented an Inventory Container
- Implemented a Icon to the interface which acts like a drop down menu to use your selected potions
- Implementes some interfaces and enumerations
- Implemented characterattributes
- Fixed a bug which causes the character not to change the animation while faling

!!! 09.07.2015 !!!
- Added custom Armors with different look and attributes to the game
- Changed the gamefont (http://www.schriftarten-fonts.de/fonts/9761/04b30.html)

!!! 26.07.2015 !!!
- added custom build properties
- poorly added logging
- fixed some performance issues
- fixed some bugs causing the app to lag
- added custom vertices to the box2d<->map parser
- updated some packages
