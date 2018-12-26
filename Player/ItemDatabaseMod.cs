//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TheForest.Items;
//namespace ChampionsOfForest.Player
//{
//    public class ItemDatabaseMod //: ItemDatabase
//    {
//        public static Dictionary<int, int> BaseCarryAmounts = new Dictionary<int, int>(){
//{ 29, 5 },      //BombTimed
//{ 31, 5 },      //CBoard
//{ 33, 0 },      //Cloth
//{ 34, 10000 },      //Leaf
//{ 280, 0 },      //TapeSticky
//{ 35, 3 },      //Lizard
//{ 36, 0 },      //Battery
//{ 262, 5 },      //Fuel
//{ 37, 7 },      //Booze
//{ 38, 10000 },      //Cash
//{ 41, 4 },      //Watch
//{ 42, 0 },      //Feather
//{ 43, 10 },      //Flare
//{ 44, 1 },      //FlareGun
//{ 230, 1 },      //FlintLock
//{ 232, 1 },      //FlintLock part 1
//{ 233, 1 },      //FlintLock part 2
//{ 234, 1 },      //FlintLock part 3
//{ 235, 1 },      //FlintLock part 4
//{ 236, 1 },      //FlintLock part 5
//{ 237, 1 },      //FlintLock part 6
//{ 238, 1 },      //FlintLock part 7
//{ 241, 1 },      //FlintLock part 8
//{ 46, 1 },      //Head
//{ 47, 2 },      //Leg
//{ 48, 1 },      //Lighter
//{ 49, 5 },      //Meds
//{ 51, 2 },      //PlasticTorch
//{ 283, 1 },      //PlasticTorch_Bow
//{ 287, 1 },      //PlasticTorch_ModernBow
//{ 288, 1 },      //PlasticTorch_Chainsaw
//{ 289, 1 },      //PlasticTorch_Flintlock
//{ 53, 5 },      //Rock
//{ 54, 4 },      //Rope
//{ 56, 1 },      //Spear
//{ 57, 10 },      //Stick
//{ 60, 0 },      //Tooth
//{ 61, 1 },      //Walkman
//{ 63, 1 },      //Pedometer
//{ 67, 10 },      //Marigold
//{ 68, 5 },      //MedicineCrafted
//{ 212, 5 },      //MedicineCraftedPlus
//{ 69, 1 },      //Cassette 1
//{ 71, 9 },      //Molotov
//{ 74, 1 },      //SurvivalBook
//{ 75, 1 },      //FireStick
//{ 291, 1 },      //Hairspray
//{ 76, 3 },      //Rabbit Dead
//{ 77, -1 },      //Rabbit Alive
//{ 78, -1 },      //Log
//{ 79, 1 },      //Bow
//{ 306, 1 },      //BowCross
//{ 281, 1 },      //Slingshot
//{ 279, 1 },      //RecurveBow
//{ 80, 1 },      //Axe Plane
//{ 261, 1 },      //Chainsaw
//{ 81, -1 },      //Tennis Ball
//{ 294, 1 },      //ArtifactBall
//{ 82, 10 },      //Small Rock
//{ 83, 30 },      //Arrows
//{ 86, 1 },      //AxeRusty
//{ 87, 1 },      //AxeCrafted
//{ 257, 1 },      //RepairTool
//{ 88, 1 },      //AxeModern
//{ 89, 15 },      //ChocolateBar
//{ 90, 2 },      //Arm
//{ 91, 0 },      //Coins
//{ 92, 10 },      //LizardSkin
//{ 94, 4 },      //Skull
//{ 95, 1 },      //ClubCrafted
//{ 96, 1 },      //Club
//{ 97, 10 },      //ConeFlower
//{ 98, 10 },      //Chicory
//{ 99, 10 },      //Aloe
//{ 100, 5 },      //EnergyMix
//{ 213, 5 },      //EnergyMixPlus
//{ 101, 1 },      //HeadBomb
//{ 103, 0 },      //Seed_Aloe
//{ 104, 0 },      //TreeSap
//{ 105, 1 },      //StickUpgraded
//{ 106, 1 },      //RockUpgraded
//{ 107, 0 },      //FlareGunAmmo
//{ 231, 0 },      //flintlockAmmo
//{ 307, 10 },      //CrossbowAmmo
//{ 108, 0 },      //Glass
//{ 109, 10 },      //Soda
//{ 110, 1 },      //PlaneFood
//{ 112, 30 },      //twinberry
//{ 113, 30 },      //SnowBerry
//{ 114, 30 },      //BlueBerry
//{ 115, 10 },      //MushroomAmanita
//{ 277, 10 },      //MushroomJack
//{ 116, 10 },      //MushroomChanterelle
//{ 278, 10 },      //MushroomDeerMush
//{ 276, 10 },      //MushroomLibertyCap
//{ 275, 10 },      //MushroomPuffmush
//{ 117, 1 },      //Cassette 2
//{ 118, 1 },      //Cassette 4
//{ 119, 1 },      //Cassette 3
//{ 120, 1 },      //Cassette 5
//{ 269, 1 },      //CamcorderTape1
//{ 272, 1 },      //CamcorderTape2
//{ 271, 1 },      //CamcorderTape4
//{ 274, 1 },      //CamcorderTape5
//{ 273, 1 },      //CamcorderTape6
//{ 270, 1 },      //CamcorderTape3
//{ 122, 1 },      //WalkyTalky
//{ 123, 4 },      //GenericMeat
//{ 126, 10 },      //DeerSkin
//{ 299, 1 },      //Warmsuit
//{ 292, 10 },      //BoarSkin
//{ 293, 10 },      //RacoonSkin
//{ 127, 3 },      //Cod
//{ 129, 10 },      //RabbitSkin
//{ 130, 1 },      //Pouch
//{ 131, 3 },      //BluePaint
//{ 132, 3 },      //OrangePaint
//{ 133, 1 },      //Toy_Head
//{ 134, 2 },      //Toy_Arm
//{ 135, 2 },      //Toy_Leg
//{ 136, 1 },      //Toy_Torso
//{ 137, 10 },      //StealthArmor
//{ 138, 1 },      //Axe Climbing
//{ 139, 6 },      //Map_Cave2
//{ 140, 1 },      //Cross
//{ 240, 1 },      //PaintBrush
//{ 141, 1 },      //TurtleShell
//{ 142, 1 },      //Pot
//{ 143, 1 },      //Rebreather
//{ 144, 4 },      //AirCanister
//{ 145, 1 },      //Waterskin
//{ 147, 0 },      //MagazineCaver
//{ 148, 0 },      //Magazine
//{ 149, 0 },      //MagazineLimestone
//{ 150, 0 },      //Bible
//{ 152, 0 },      //PolaroidYacht
//{ 258, 0 },      //PolaroidKeyCard1
//{ 259, 0 },      //PolaroidKeyCard2
//{ 260, 0 },      //PolaroidKeyCard3
//{ 308, 0 },      //PageChurch
//{ 309, 0 },      //PageGlider
//{ 310, 0 },      //PageRollercoaster
//{ 311, 0 },      //PageTower
//{ 220, 0 },      //PhotoCache1
//{ 226, 0 },      //PhotoCache2
//{ 225, 0 },      //PhotoCache3
//{ 224, 0 },      //PhotoCache8
//{ 227, 0 },      //PhotoCache9
//{ 302, 1 },      //PhotoTimmy
//{ 223, 0 },      //PhotoCache6
//{ 222, 0 },      //PhotoCache5
//{ 229, 0 },      //shippingManifest
//{ 243, 0 },      //RestrainingOrder
//{ 244, 0 },      //TerminationLetter
//{ 245, 0 },      //MorgueReport
//{ 246, 0 },      //BiblePage1
//{ 251, 0 },      //MeganDrawingFlower
//{ 253, 0 },      //MeganDrawingUnicorn
//{ 252, 0 },      //MeganDrawingDaddy
//{ 254, 0 },      //MeganDrawingDino
//{ 248, 0 },      //MeganCrayons
//{ 267, 1 },      //Camcorder
//{ 249, 0 },      //ArtifactPhoto
//{ 255, 0 },      //EmailPlane
//{ 256, 0 },      //EmailPhoto
//{ 247, 0 },      //BiblePage2
//{ 303, 0 },      //BiblePage3
//{ 221, 0 },      //PhotoCache4
//{ 219, 0 },      //PolaroidMegan
//{ 153, 0 },      //PolaroidVagz
//{ 154, 0 },      //SketchSinkhole
//{ 155, 0 },      //SketchVagz
//{ 156, 1 },      //MapPiece_1
//{ 157, 1 },      //MapPiece_2
//{ 159, 1 },      //MapPiece_4
//{ 165, 1 },      //MapPiece_3
//{ 169, 1 },      //MapFull
//{ 173, 1 },      //Compass
//{ 175, 5 },      //dynamite
//{ 176, 1 },      //MilkCarton
//{ 177, 1 },      //SpearUpgraded
//{ 178, 15 },      //bone
//{ 179, 1 },      //ToyFull
//{ 180, 1 },      //Katana
//{ 265, 1 },      //Machete
//{ 181, 10 },      //Oyster
//{ 182, 0 },      //PolaroidTeddy
//{ 183, 0 },      //NewspaperStripper
//{ 184, 1 },      //TennisRaquet
//{ 185, -1 },      //AnimalHead_rabbit
//{ 186, -1 },      //AnimalHead_Boar
//{ 187, -1 },      //AnimalHead_Deer
//{ 188, -1 },      //AnimalHead_Crocodile
//{ 189, -1 },      //AnimalHead_Racoon
//{ 190, -1 },      //AnimalHead_Lizard
//{ 191, -1 },      //AnimalHead_Seagul
//{ 192, -1 },      //AnimalHead_Squirrel
//{ 193, -1 },      //AnimalHead_Tortoise
//{ 295, -1 },      //CreepyHead_Armsy
//{ 296, -1 },      //CreepyHead_Baby
//{ 297, -1 },      //CreepyHead_Fat
//{ 298, -1 },      //CreepyHead_Vag
//{ 194, -1 },      //AnimalHead_Goose
//{ 195, -1 },      //AnimalHead_Shark
//{ 196, 1 },      //CaveMap
//{ 197, 1 },      //PassengerManifest
//{ 198, 1 },      //MetalTinTray
//{ 199, 1 },      //SnowShoes
//{ 200, 1 },      //Quiver 
//{ 214, 1 },      //RockBag
//{ 215, 1 },      //StickBag
//{ 282, 1 },      //SmallRockBag
//{ 290, 1 },      //SpearBag
//{ 201, 1 },      //RabbitFurBoots
//{ 202, 1 },      //fortune
//{ 203, 1 },      //BookDarkHaired
//{ 204, 10 },      //BoneArmor
//{ 301, 10 },      //CreepySkin
//{ 205, 0 },      //Seed_Coneflower
//{ 206, 30 },      //Seed_BlueBerry
//{ 207, 3 },      //SmallGenericMeat
//{ 208, 10 },      //TimmyDrawing
//{ 209, 10 },      //Magazine_reality
//{ 263, 10 },      //ChainsawAd
//{ 218, 10 },      //Magazine_science
//{ 210, 1 },      //Keycard
//{ 305, 1 },      //artifactKey
//{ 242, 1 },      //KeycardElevator
//{ 217, 10 },      //SketchArtifact
//{ 250, 10 },      //SketchArtifact4
//{ 211, 30 },      //BlackBerry
//};


//        //public override void OnEnable()
//        //{
//        //    base.OnEnable();
//        //    string s = "{\n";
//        //    foreach (var item in Items)
//        //    {
//        //        s += "{ "+item._id + ", " + item._maxAmount + " },      //"+item._name+"\n";
//        //    }
//        //    ModAPI.Log.Write(s);
//        //}
//    }
//}
