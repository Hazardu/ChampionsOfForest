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


[Networking](https://github.com/Hazardu/ChampionsOfForest/tree/master/Network) manages sending data between clients, with an option to select specyfic users to whom send a command.
- Sends byte arrays as strings via text chat.


RPG elements
- [inventory](https://github.com/Hazardu/ChampionsOfForest/blob/master/Player/Inventory.cs)
- [items and their stats](https://github.com/Hazardu/ChampionsOfForest/tree/master/Items) 
- Perks can be created with a single constructor
- Spells can be created with a single constructor, but creating appealing visual effects resulted in limited amount of them.
- Enemy stat changes, adding armor, overriding vanilla properties.





###### *This is my first repo go easy on me*
