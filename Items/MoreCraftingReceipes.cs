using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChampionsOfForest;

using TheForest.Items;
using TheForest.Items.Craft;
using TheForest.Utils;

using UnityEngine;

using static MoreCraftingReceipes.VanillaItemIDs;

//based off code from another mod RMoreCrafting https://modapi.survivetheforest.net/mod/118/more-crafting by Rurido
public static class MoreCraftingReceipes
{
	public class COTFCustomReceipe
	{
		public Receipe receipe;
		public bool unlocked;

		public COTFCustomReceipe(Receipe receipe, bool unlocked = false)
		{
			this.receipe = receipe;
			this.unlocked = unlocked;
		}
	}
	public enum CustomReceipe
	{
		ModernArrows,
		FlintlockAmmo,
		CrossbowAmmo,
		ClothFromDeer,
		ClothFromRabbit,
		ClothFromRacoon,
		ClothFromBoar,
		PlaneAxe,
	}


	private static List<Receipe> baseReceipes;
	private static List<COTFCustomReceipe> customReceipeList = new List<COTFCustomReceipe>();
	public static void SetCustomReceipeUnlockState(CustomReceipe customReceipe, in bool b)
	{
		customReceipeList[(int)customReceipe].unlocked = b;
	}
	public static bool BlockUpdating = false;
	private const int IdStartIndex = 600;
	public static void LockAll()
	{
		foreach (var item in customReceipeList)
		{
			item.unlocked = false;
		}
	}

	/// <summary>
	/// Populate the list that contains custom receipes
	/// </summary>
	public static void Initialize()
	{
		try
		{
			BlockUpdating = false;
			baseReceipes = null;
			customReceipeList.Clear();
			_NextReceipeId = IdStartIndex;
			try
			{
				Receipe poisonArrowsCopy = ReceipeDatabase._instance._receipes.Where(x => x._productItemID == 83 && x._ingredients.Any(y => y._itemID == 112)).FirstOrDefault().CopyReceipe();
				poisonArrowsCopy._weaponStatUpgrades[0]._type = WeaponStatUpgrade.Types.ModernAmmo;
				poisonArrowsCopy._ingredients[1] = CreateReceipeIngredient(COINS, 15);
				poisonArrowsCopy._name = "Modern Arrows";
				customReceipeList.Add(new COTFCustomReceipe(poisonArrowsCopy));
			}
			catch (Exception e)
			{

				ModAPI.Log.Write("Exception copying receipe " + e);
			}
			CreateReceipe(FLINTLOCKAMMO, 3,
				CreateReceipeIngredient(COINS, 15),
				CreateReceipeIngredient(SMALLROCK, 3),
				CreateReceipeIngredient(ROCK, 1))._name = "Flintlock ammo";

			CreateReceipe(CROSSBOWAMMO, 1,
				CreateReceipeIngredient(ROCK, 1),
				CreateReceipeIngredient(STICK, 1))._name = "Crossbow bolts";

			CreateReceipe(CLOTH, 10,
				CreateReceipeIngredient(DEERSKIN, 1))._name = "Cloth";

			CreateReceipe(CLOTH, 8,
				CreateReceipeIngredient(RABBITSKIN, 1))._name = "Cloth";

			CreateReceipe(CLOTH, 8,
				CreateReceipeIngredient(RACOONSKIN, 1))._name = "Cloth";

			CreateReceipe(CLOTH, 10,
				CreateReceipeIngredient(BOARSKIN, 1))._name = "Cloth";

			CreateReceipe(AXEPLANE, 1,
				CreateReceipeIngredient(AXECRAFTED, 1),
				CreateReceipeIngredient(ROPE, 1),
				CreateReceipeIngredient(STICK, 2),
				CreateReceipeIngredient(CLOTH, 25),
				CreateReceipeIngredient(RABBITSKIN, 1)
				)._name = "Plane Axe";

		}
		catch (Exception ex)
		{

			ModAPI.Log.Write("Custom crafting recipes exception: " + ex);
		}

	}

	private static int _NextReceipeId = IdStartIndex;

	public static int NextReceipeId
	{
		get
		{
			return _NextReceipeId++;
		}
	}



	public static void AddReceipes()
	{
		if (BlockUpdating)
			return;
		if (baseReceipes == null)
			baseReceipes = ReceipeDatabase._instance._receipes.ToList();
		foreach (var item in customReceipeList)
		{
			LocalPlayer.ReceipeBook.RemoveReceipe(item.receipe._id);

		}
		var customReceipes = customReceipeList.Where(x => x.unlocked).Select(x => x.receipe);
		List<int> addedIDs = new List<int>();
		if (customReceipes.Count() > 0)
		{
			try
			{
				var receipes2 = new List<Receipe>(baseReceipes);
				receipes2.AddRange(customReceipes);
				ReceipeDatabase._instance._receipes = receipes2.ToArray();
			}
			catch
			{
				CotfUtils.Log("Crafting: Merging Failed");
			}
			foreach (var item in customReceipes)
			{
				LocalPlayer.ReceipeBook.AddReceipe(item._id);

			}
		}
		else
		{
			ReceipeDatabase._instance._receipes = baseReceipes.ToArray();
		}
	}

	public static ReceipeIngredient CreateReceipeIngredient(VanillaItemIDs itemId, int amount)
	{
		return new ReceipeIngredient
		{
			_amount = amount,
			_itemID = (int)itemId
		};
	}
	public static Receipe CopyReceipe(this Receipe r)
	{
		return new Receipe()
		{
			_id = NextReceipeId,
			_batchUpgrade = r._batchUpgrade,
			_forceUnique = r._forceUnique,
			_hidden = r._hidden,
			_ingredients = r._ingredients,
			_name = r._name,
			_productItemAmount = r._productItemAmount,
			_productItemID = r._productItemID,
			_productItemType = r._productItemType,
			_type = r._type,
			_weaponStatUpgrades = r._weaponStatUpgrades
		};
	}
	public static Receipe CreateReceipe(VanillaItemIDs productItemId, int productItemAmout, params ReceipeIngredient[] receipeIngredients)
	{
		var r = new Receipe
		{
			_id = NextReceipeId,
			_ingredients = receipeIngredients,
			_productItemAmount = new RandomRange
			{
				_max = productItemAmout,
				_min = productItemAmout
			},
			_productItemID = (int)productItemId,
			_productItemType = TheForest.Items.ItemDatabase.ItemById((int)productItemId)._type,
			_type = Receipe.Types.Craft,
			_weaponStatUpgrades = new WeaponStatUpgrade[0]
		};
		customReceipeList.Add(new COTFCustomReceipe(r));

		return r;
	}
	//		--------------------------ITEM IDs--------------------------

	public enum VanillaItemIDs
	{
		BOMBTIMED = 29,
		CBOARD = 31,
		CLOTH = 33,
		LEAF = 34,
		TAPESTICKY = 280,
		LIZARD = 35,
		BATTERY = 36,
		FUEL = 262,
		BOOZE = 37,
		CASH = 38,
		WATCH = 41,
		FEATHER = 42,
		FLARE = 43,
		FLAREGUN = 44,
		FLINTLOCK = 230,
		FLINTLOCKPART1 = 232,
		FLINTLOCKPART2 = 233,
		FLINTLOCKPART3 = 234,
		FLINTLOCKPART4 = 235,
		FLINTLOCKPART5 = 236,
		FLINTLOCKPART6 = 237,
		FLINTLOCKPART7 = 238,
		FLINTLOCKPART8 = 241,
		HEAD = 46,
		LEG = 47,
		LIGHTER = 48,
		MEDS = 49,
		PLASTICTORCH = 51,
		PLASTICTORCH_BOW = 283,
		PLASTICTORCH_MODERNBOW = 287,
		PLASTICTORCH_CHAINSAW = 288,
		PLASTICTORCH_FLINTLOCK = 289,
		ROCK = 53,
		ROPE = 54,
		SPEAR = 56,
		STICK = 57,
		TOOTH = 60,
		WALKMAN = 61,
		PEDOMETER = 63,
		MARIGOLD = 67,
		MEDICINECRAFTED = 68,
		MEDICINECRAFTEDPLUS = 212,
		CASSETTE1 = 69,
		MOLOTOV = 71,
		SURVIVALBOOK = 74,
		FIRESTICK = 75,
		HAIRSPRAY = 291,
		RABBITDEAD = 76,
		RABBITALIVE = 77,
		LOG = 78,
		BOW = 79,
		BOWCROSS = 306,
		SLINGSHOT = 281,
		RECURVEBOW = 279,
		AXEPLANE = 80,
		CHAINSAW = 261,
		TENNISBALL = 81,
		ARTIFACTBALL = 294,
		SMALLROCK = 82,
		ARROWS = 83,
		AXERUSTY = 86,
		AXECRAFTED = 87,
		REPAIRTOOL = 257,
		AXEMODERN = 88,
		CHOCOLATEBAR = 89,
		ARM = 90,
		COINS = 91,
		LIZARDSKIN = 92,
		SKULL = 94,
		CLUBCRAFTED = 95,
		CLUB = 96,
		CONEFLOWER = 97,
		CHICORY = 98,
		ALOE = 99,
		ENERGYMIX = 100,
		ENERGYMIXPLUS = 213,
		HEADBOMB = 101,
		SEED_ALOE = 103,
		TREESAP = 104,
		STICKUPGRADED = 105,
		ROCKUPGRADED = 106,
		FLAREGUNAMMO = 107,
		FLINTLOCKAMMO = 231,
		CROSSBOWAMMO = 307,
		GLASS = 108,
		SODA = 109,
		PLANEFOOD = 110,
		TWINBERRY = 112,
		SNOWBERRY = 113,
		BLUEBERRY = 114,
		MUSHROOMAMANITA = 115,
		MUSHROOMJACK = 277,
		MUSHROOMCHANTERELLE = 116,
		MUSHROOMDEERMUSH = 278,
		MUSHROOMLIBERTYCAP = 276,
		MUSHROOMPUFFMUSH = 275,
		CASSETTE2 = 117,
		CASSETTE4 = 118,
		CASSETTE3 = 119,
		CASSETTE5 = 120,
		CAMCORDERTAPE1 = 269,
		CAMCORDERTAPE2 = 272,
		CAMCORDERTAPE4 = 271,
		CAMCORDERTAPE5 = 274,
		CAMCORDERTAPE6 = 273,
		CAMCORDERTAPE3 = 270,
		WALKYTALKY = 122,
		GENERICMEAT = 123,
		DEERSKIN = 126,
		WARMSUIT = 299,
		BOARSKIN = 292,
		RACOONSKIN = 293,
		COD = 127,
		RABBITSKIN = 129,
		POUCH = 130,
		BLUEPAINT = 131,
		ORANGEPAINT = 132,
		TOY_HEAD = 133,
		TOY_ARM = 134,
		TOY_LEG = 135,
		TOY_TORSO = 136,
		STEALTHARMOR = 137,
		AXECLIMBING = 138,
		MAP_CAVE2 = 139,
		CROSS = 140,
		PAINTBRUSH = 240,
		TURTLESHELL = 141,
		POT = 142,
		REBREATHER = 143,
		AIRCANISTER = 144,
		WATERSKIN = 145,
		MAGAZINECAVER = 147,
		MAGAZINE = 148,
		MAGAZINELIMESTONE = 149,
		BIBLE = 150,
		POLAROIDYACHT = 152,
		POLAROIDKEYCARD1 = 258,
		POLAROIDKEYCARD2 = 259,
		POLAROIDKEYCARD3 = 260,
		PAGECHURCH = 308,
		PAGEGLIDER = 309,
		PAGEROLLERCOASTER = 310,
		PAGETOWER = 311,
		PHOTOCACHE1 = 220,
		PHOTOCACHE2 = 226,
		PHOTOCACHE3 = 225,
		PHOTOCACHE8 = 224,
		PHOTOCACHE9 = 227,
		PHOTOTIMMY = 302,
		PHOTOCACHE6 = 223,
		PHOTOCACHE5 = 222,
		SHIPPINGMANIFEST = 229,
		RESTRAININGORDER = 243,
		TERMINATIONLETTER = 244,
		MORGUEREPORT = 245,
		BIBLEPAGE1 = 246,
		MEGANDRAWINGFLOWER = 251,
		MEGANDRAWINGUNICORN = 253,
		MEGANDRAWINGDADDY = 252,
		MEGANDRAWINGDINO = 254,
		MEGANCRAYONS = 248,
		CAMCORDER = 267,
		ARTIFACTPHOTO = 249,
		EMAILPLANE = 255,
		EMAILPHOTO = 256,
		BIBLEPAGE2 = 247,
		BIBLEPAGE3 = 303,
		PHOTOCACHE4 = 221,
		POLAROIDMEGAN = 219,
		POLAROIDVAGZ = 153,
		SKETCHSINKHOLE = 154,
		SKETCHVAGZ = 155,
		MAPPIECE_1 = 156,
		MAPPIECE_2 = 157,
		MAPPIECE_4 = 159,
		MAPPIECE_3 = 165,
		MAPFULL = 169,
		COMPASS = 173,
		DYNAMITE = 175,
		MILKCARTON = 176,
		SPEARUPGRADED = 177,
		BONE = 178,
		TOYFULL = 179,
		KATANA = 180,
		MACHETE = 265,
		OYSTER = 181,
		POLAROIDTEDDY = 182,
		NEWSPAPERSTRIPPER = 183,
		TENNISRAQUET = 184,
		ANIMALHEAD_RABBIT = 185,
		ANIMALHEAD_BOAR = 186,
		ANIMALHEAD_DEER = 187,
		ANIMALHEAD_CROCODILE = 188,
		ANIMALHEAD_RACOON = 189,
		ANIMALHEAD_LIZARD = 190,
		ANIMALHEAD_SEAGUL = 191,
		ANIMALHEAD_SQUIRREL = 192,
		ANIMALHEAD_TORTOISE = 193,
		CREEPYHEAD_ARMSY = 295,
		CREEPYHEAD_BABY = 296,
		CREEPYHEAD_FAT = 297,
		CREEPYHEAD_VAG = 298,
		ANIMALHEAD_GOOSE = 194,
		ANIMALHEAD_SHARK = 195,
		CAVEMAP = 196,
		PASSENGERMANIFEST = 197,
		METALTINTRAY = 198,
		SNOWSHOES = 199,
		QUIVER = 200,
		ROCKBAG = 214,
		STICKBAG = 215,
		SMALLROCKBAG = 282,
		SPEARBAG = 290,
		RABBITFURBOOTS = 201,
		FORTUNE = 202,
		BOOKDARKHAIRED = 203,
		BONEARMOR = 204,
		CREEPYSKIN = 301,
		SEED_CONEFLOWER = 205,
		SEED_BLUEBERRY = 206,
		SMALLGENERICMEAT = 207,
		TIMMYDRAWING = 208,
		MAGAZINE_REALITY = 209,
		CHAINSAWAD = 263,
		MAGAZINE_SCIENCE = 218,
		KEYCARD = 210,
		ARTIFACTKEY = 305,
		KEYCARDELEVATOR = 242,
		SKETCHARTIFACT = 217,
		SKETCHARTIFACT4 = 250,
		BLACKBERRY = 211
	}
	//		------------------------------------------------------------1

}