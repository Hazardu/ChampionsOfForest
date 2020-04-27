# Champions Of The Forest
Turns a survival horror game "The Forest" into a RPG.
This was an attempt at creating a modification with models, sounds and HD textures.
Download here: https://modapi.survivetheforest.net/mod/101/champions-of-the-forest


### List of handy things:
[ResourceLoader.cs](https://github.com/Hazardu/ChampionsOfForest/blob/master/Res/ResourceLoader.cs) contains runtime checking for existing files, downloading missing and loading of
-  .obj files for 3D models
-  HD images, that modapi libraries does not support.
-  audio 
-  assetbundles
Handled at launch of the game and kept in memory for later use

Custom cheat commands - ingame developer console additions - a class inheriting from DebugConsole class needs to contain methods that start with _ then it will be added to list of commands.

[Networking](https://github.com/Hazardu/ChampionsOfForest/tree/master/Network) manages sending data between clients, with an option to select specyfic users to whom send a command.
- Sends byte arrays as strings via text chat.


RPG elements
- [inventory](https://github.com/Hazardu/ChampionsOfForest/blob/master/Player/Inventory.cs)
- [items and their stats](https://github.com/Hazardu/ChampionsOfForest/tree/master/Items) 
- Perks can be created with a single constructor
- Spells can be created with a single constructor, but creating appealing visual effects resulted in limited amount of them.
- Enemy stat changes, adding armor, overriding vanilla properties.
- Custom difficulty options - shared between players in coop. Features insta death, dropping items on death, and extended friendly fire
- Leveling 
- Custom weapons - actually clones of the plane axe, copied it with all of it's components, as setting those up would be lenghty and bug-prone


### A TOOL that helped me make this - made by me
-  [this tool that parses Perks.cs and displays perk positions even before compiling](https://drive.google.com/open?id=1WgCD28isriRzmylybSVSF2GteerNLtJQ)



###### *This is my first repo go easy on me*
