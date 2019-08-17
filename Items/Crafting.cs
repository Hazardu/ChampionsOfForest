//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TheForest.Items;
//using TheForest.Items.Craft;
//using TheForest.Utils;
//using UnityEngine;

//namespace ChampionsOfForest.Items
//{

//    //public class ItemsDebug : ItemDatabase
//    //{
//    //    public override void OnEnable()
//    //    {
//    //        base.OnEnable();
//    //        string msg = "";
//    //        foreach (var i in _itemsCache)
//    //        {
//    //            msg += i.Value._name + "\t\t\t" + i.Value._id + "\n";
//    //        }
//    //        ModAPI.Log.Write(msg);
//    //    }
//    //}


//    //from another mod RMoreCrafting https://modapi.survivetheforest.net/mod/118/more-crafting by Rurido
//    public class Crafting : MonoBehaviour
//    {
//        private static readonly int IdStartIndex = 600;

//        void Start()
//        {
//            try
//            {
//                var arrows = CreateReceipe(83, 1, new ReceipeIngredient[]{
//                    CreateReceipeIngredient(83,1),
//                    CreateReceipeIngredient(91,2),
//                    });
//                arrows._weaponStatUpgrades = new WeaponStatUpgrade[]
//                {
//                    new WeaponStatUpgrade(){_type = WeaponStatUpgrade.Types.ModernAmmo, _amount = 1, _itemId = 0}
//                };
//                arrows._type = Receipe.Types.Upgrade;
//                arrows._productItemType = TheForest.Items.Item.Types.Ammo;


//                Receipe r = ReceipeDatabase._instance._receipes.Where(x => x._productItemID == 83 && x._ingredients.Any(y => y._itemID == 112)).FirstOrDefault();
//                //ModAPI.Log.Write( "_batchUpgrade " + r._batchUpgrade+
//                //    "\n_forceUnique " + r._forceUnique+
//                //    "\n_hidden " + r._hidden+
//                //    "\n_id " + r._id+
//                //    "\n_name " + r._name+
//                //    "\n_productItemType " + r._productItemType.ToString()+
//                //    "\n_type " + r._type.ToString()+
//                //    "\n_weaponStatUpgrades.Count " + r._weaponStatUpgrades.Length);
//                //foreach (var item in r._weaponStatUpgrades)
//                //{
//                //    ModAPI.Log.Write(" _amount " + item._amount + "\n_type" + item._type + "\n_itemId" + item._itemId);
//                //}

//                r._weaponStatUpgrades[0]._type = WeaponStatUpgrade.Types.ModernAmmo;
//                r._ingredients[1] = CreateReceipeIngredient(91,2);
              
//            }
//            catch (Exception e)
//            {
//                CotfUtils.Log("adding items error\n" + e.Message);
//            }
//        }

//        private static int _NextReceipeId = IdStartIndex;

//        public static int NextReceipeId
//        {
//            get
//            {
//                return _NextReceipeId++;
//            }
//        }


//        public static bool AddReceipes(List<Receipe> newReceipes)
//        {          
//            int num = ReceipeDatabase._instance._receipes.Length;
//            Receipe[] receipes = ReceipeDatabase._instance._receipes;
//            List<Receipe> list = new List<Receipe>();
//            using (List<Receipe>.Enumerator enumerator = newReceipes.GetEnumerator())
//            {
//                while (enumerator.MoveNext())
//                {
//                    Receipe newReceipe = enumerator.Current;
//                    if (!receipes.Any((Receipe r) => r.IngredientHash == newReceipe.IngredientHash && r._productItemID == newReceipe._productItemID))
//                    {
//                        list.Add(newReceipe);
//                    }
//                }
//            }
//            if (list.Count > 0)
//            {
//                //RMoreCrafting._instance.Log(string.Concat(new object[]
//                //{
//                //    "Merging ",
//                //    list.Count,
//                //    " Recipe",
//                //    (list.Count == 1) ? "" : "s",
//                //    "..."
//                //}));
//                try
//                {
//                    Receipe[] receipes2 = receipes.Concat(list).ToArray<Receipe>();
//                    ReceipeDatabase._instance._receipes = receipes2;
//                    //RMoreCrafting._instance.Log("Merging Complete");
//                }
//                catch
//                {
//                    //RMoreCrafting._instance.Log("Merging Failed");
//                }
//            }
//            int num2 = ReceipeDatabase._instance._receipes.Length;
//            //RMoreCrafting._instance.Log(string.Concat(new object[]
//            //{
//            //    "Receipe Count: ",
//            //    num,
//            //    " -> ",
//            //    num2
//            //}));
//            return num2 > num;
//        }

//        public static ReceipeIngredient CreateReceipeIngredient(int itemId, int amount)
//        {
//            return new ReceipeIngredient
//            {
//                _amount = amount,
//                _itemID = itemId
//            };
//        }
//        public static Receipe CreateReceipe(int productItemId, int productItemAmout, ReceipeIngredient[] receipeIngredients)
//        {
//            return new Receipe
//            {
//                _id = NextReceipeId,
//                _ingredients = receipeIngredients,
//                _productItemAmount = new RandomRange
//                {
//                    _max = productItemAmout,
//                    _min = productItemAmout
//                },
//                _productItemID = productItemId,
//                _productItemType = ItemDatabase.ItemById(productItemId)._type,
//                _type = Receipe.Types.Craft,
//                _weaponStatUpgrades = new WeaponStatUpgrade[0]
//            };
//        }
//    }
//}
/*
 [2019-08-01 16:05] BombTimed			29
CBoard			31
Cloth			33
Leaf			34
TapeSticky			280
Lizard			35
Battery			36
Fuel			262
Booze			37
Cash			38
Watch			41
Feather			42
Flare			43
FlareGun			44
FlintLock			230
FlintLock part 1			232
FlintLock part 2			233
FlintLock part 3			234
FlintLock part 4			235
FlintLock part 5			236
FlintLock part 6			237
FlintLock part 7			238
FlintLock part 8			241
Head			46
Leg			47
Lighter			48
Meds			49
PlasticTorch			51
PlasticTorch_Bow			283
PlasticTorch_ModernBow			287
PlasticTorch_Chainsaw			288
PlasticTorch_Flintlock			289
Rock			53
Rope			54
Spear			56
Stick			57
Tooth			60
Walkman			61
Pedometer			63
Marigold			67
MedicineCrafted			68
MedicineCraftedPlus			212
Cassette 1			69
Molotov			71
SurvivalBook			74
FireStick			75
Hairspray			291
Rabbit Dead			76
Rabbit Alive			77
Log			78
Bow			79
BowCross			306
Slingshot			281
RecurveBow			279
Axe Plane			80
Chainsaw			261
Tennis Ball			81
ArtifactBall			294
Small Rock			82
Arrows			83
AxeRusty			86
AxeCrafted			87
RepairTool			257
AxeModern			88
ChocolateBar			89
Arm			90
Coins			91
LizardSkin			92
Skull			94
ClubCrafted			95
Club			96
ConeFlower			97
Chicory			98
Aloe			99
EnergyMix			100
EnergyMixPlus			213
HeadBomb			101
Seed_Aloe			103
TreeSap			104
StickUpgraded			105
RockUpgraded			106
FlareGunAmmo			107
flintlockAmmo			231
CrossbowAmmo			307
Glass			108
Soda			109
PlaneFood			110
twinberry			112
SnowBerry			113
BlueBerry			114
MushroomAmanita			115
MushroomJack			277
MushroomChanterelle			116
MushroomDeerMush			278
MushroomLibertyCap			276
MushroomPuffmush			275
Cassette 2			117
Cassette 4			118
Cassette 3			119
Cassette 5			120
CamcorderTape1			269
CamcorderTape2			272
CamcorderTape4			271
CamcorderTape5			274
CamcorderTape6			273
CamcorderTape3			270
WalkyTalky			122
GenericMeat			123
DeerSkin			126
Warmsuit			299
BoarSkin			292
RacoonSkin			293
Cod			127
RabbitSkin			129
Pouch			130
BluePaint			131
OrangePaint			132
Toy_Head			133
Toy_Arm			134
Toy_Leg			135
Toy_Torso			136
StealthArmor			137
Axe Climbing			138
Map_Cave2			139
Cross			140
PaintBrush			240
TurtleShell			141
Pot			142
Rebreather			143
AirCanister			144
Waterskin			145
MagazineCaver			147
Magazine			148
MagazineLimestone			149
Bible			150
PolaroidYacht			152
PolaroidKeyCard1			258
PolaroidKeyCard2			259
PolaroidKeyCard3			260
PageChurch			308
PageGlider			309
PageRollercoaster			310
PageTower			311
PhotoCache1			220
PhotoCache2			226
PhotoCache3			225
PhotoCache8			224
PhotoCache9			227
PhotoTimmy			302
PhotoCache6			223
PhotoCache5			222
shippingManifest			229
RestrainingOrder			243
TerminationLetter			244
MorgueReport			245
BiblePage1			246
MeganDrawingFlower			251
MeganDrawingUnicorn			253
MeganDrawingDaddy			252
MeganDrawingDino			254
MeganCrayons			248
Camcorder			267
ArtifactPhoto			249
EmailPlane			255
EmailPhoto			256
BiblePage2			247
BiblePage3			303
PhotoCache4			221
PolaroidMegan			219
PolaroidVagz			153
SketchSinkhole			154
SketchVagz			155
MapPiece_1			156
MapPiece_2			157
MapPiece_4			159
MapPiece_3			165
MapFull			169
Compass			173
dynamite			175
MilkCarton			176
SpearUpgraded			177
bone			178
ToyFull			179
Katana			180
Machete			265
Oyster			181
PolaroidTeddy			182
NewspaperStripper			183
TennisRaquet			184
AnimalHead_rabbit			185
AnimalHead_Boar			186
AnimalHead_Deer			187
AnimalHead_Crocodile			188
AnimalHead_Racoon			189
AnimalHead_Lizard			190
AnimalHead_Seagul			191
AnimalHead_Squirrel			192
AnimalHead_Tortoise			193
CreepyHead_Armsy			295
CreepyHead_Baby			296
CreepyHead_Fat			297
CreepyHead_Vag			298
AnimalHead_Goose			194
AnimalHead_Shark			195
CaveMap			196
PassengerManifest			197
MetalTinTray			198
SnowShoes			199
Quiver 			200
RockBag			214
StickBag			215
SmallRockBag			282
SpearBag			290
RabbitFurBoots			201
fortune			202
BookDarkHaired			203
BoneArmor			204
CreepySkin			301
Seed_Coneflower			205
Seed_BlueBerry			206
SmallGenericMeat			207
TimmyDrawing			208
Magazine_reality			209
ChainsawAd			263
Magazine_science			218
Keycard			210
artifactKey			305
KeycardElevator			242
SketchArtifact			217
SketchArtifact4			250
BlackBerry			211

[2019-08-01 16:06] BombTimed			29
CBoard			31
Cloth			33
Leaf			34
TapeSticky			280
Lizard			35
Battery			36
Fuel			262
Booze			37
Cash			38
Watch			41
Feather			42
Flare			43
FlareGun			44
FlintLock			230
FlintLock part 1			232
FlintLock part 2			233
FlintLock part 3			234
FlintLock part 4			235
FlintLock part 5			236
FlintLock part 6			237
FlintLock part 7			238
FlintLock part 8			241
Head			46
Leg			47
Lighter			48
Meds			49
PlasticTorch			51
PlasticTorch_Bow			283
PlasticTorch_ModernBow			287
PlasticTorch_Chainsaw			288
PlasticTorch_Flintlock			289
Rock			53
Rope			54
Spear			56
Stick			57
Tooth			60
Walkman			61
Pedometer			63
Marigold			67
MedicineCrafted			68
MedicineCraftedPlus			212
Cassette 1			69
Molotov			71
SurvivalBook			74
FireStick			75
Hairspray			291
Rabbit Dead			76
Rabbit Alive			77
Log			78
Bow			79
BowCross			306
Slingshot			281
RecurveBow			279
Axe Plane			80
Chainsaw			261
Tennis Ball			81
ArtifactBall			294
Small Rock			82
Arrows			83
AxeRusty			86
AxeCrafted			87
RepairTool			257
AxeModern			88
ChocolateBar			89
Arm			90
Coins			91
LizardSkin			92
Skull			94
ClubCrafted			95
Club			96
ConeFlower			97
Chicory			98
Aloe			99
EnergyMix			100
EnergyMixPlus			213
HeadBomb			101
Seed_Aloe			103
TreeSap			104
StickUpgraded			105
RockUpgraded			106
FlareGunAmmo			107
flintlockAmmo			231
CrossbowAmmo			307
Glass			108
Soda			109
PlaneFood			110
twinberry			112
SnowBerry			113
BlueBerry			114
MushroomAmanita			115
MushroomJack			277
MushroomChanterelle			116
MushroomDeerMush			278
MushroomLibertyCap			276
MushroomPuffmush			275
Cassette 2			117
Cassette 4			118
Cassette 3			119
Cassette 5			120
CamcorderTape1			269
CamcorderTape2			272
CamcorderTape4			271
CamcorderTape5			274
CamcorderTape6			273
CamcorderTape3			270
WalkyTalky			122
GenericMeat			123
DeerSkin			126
Warmsuit			299
BoarSkin			292
RacoonSkin			293
Cod			127
RabbitSkin			129
Pouch			130
BluePaint			131
OrangePaint			132
Toy_Head			133
Toy_Arm			134
Toy_Leg			135
Toy_Torso			136
StealthArmor			137
Axe Climbing			138
Map_Cave2			139
Cross			140
PaintBrush			240
TurtleShell			141
Pot			142
Rebreather			143
AirCanister			144
Waterskin			145
MagazineCaver			147
Magazine			148
MagazineLimestone			149
Bible			150
PolaroidYacht			152
PolaroidKeyCard1			258
PolaroidKeyCard2			259
PolaroidKeyCard3			260
PageChurch			308
PageGlider			309
PageRollercoaster			310
PageTower			311
PhotoCache1			220
PhotoCache2			226
PhotoCache3			225
PhotoCache8			224
PhotoCache9			227
PhotoTimmy			302
PhotoCache6			223
PhotoCache5			222
shippingManifest			229
RestrainingOrder			243
TerminationLetter			244
MorgueReport			245
BiblePage1			246
MeganDrawingFlower			251
MeganDrawingUnicorn			253
MeganDrawingDaddy			252
MeganDrawingDino			254
MeganCrayons			248
Camcorder			267
ArtifactPhoto			249
EmailPlane			255
EmailPhoto			256
BiblePage2			247
BiblePage3			303
PhotoCache4			221
PolaroidMegan			219
PolaroidVagz			153
SketchSinkhole			154
SketchVagz			155
MapPiece_1			156
MapPiece_2			157
MapPiece_4			159
MapPiece_3			165
MapFull			169
Compass			173
dynamite			175
MilkCarton			176
SpearUpgraded			177
bone			178
ToyFull			179
Katana			180
Machete			265
Oyster			181
PolaroidTeddy			182
NewspaperStripper			183
TennisRaquet			184
AnimalHead_rabbit			185
AnimalHead_Boar			186
AnimalHead_Deer			187
AnimalHead_Crocodile			188
AnimalHead_Racoon			189
AnimalHead_Lizard			190
AnimalHead_Seagul			191
AnimalHead_Squirrel			192
AnimalHead_Tortoise			193
CreepyHead_Armsy			295
CreepyHead_Baby			296
CreepyHead_Fat			297
CreepyHead_Vag			298
AnimalHead_Goose			194
AnimalHead_Shark			195
CaveMap			196
PassengerManifest			197
MetalTinTray			198
SnowShoes			199
Quiver 			200
RockBag			214
StickBag			215
SmallRockBag			282
SpearBag			290
RabbitFurBoots			201
fortune			202
BookDarkHaired			203
BoneArmor			204
CreepySkin			301
Seed_Coneflower			205
Seed_BlueBerry			206
SmallGenericMeat			207
TimmyDrawing			208
Magazine_reality			209
ChainsawAd			263
Magazine_science			218
Keycard			210
artifactKey			305
KeycardElevator			242
SketchArtifact			217
SketchArtifact4			250
BlackBerry			211

*/