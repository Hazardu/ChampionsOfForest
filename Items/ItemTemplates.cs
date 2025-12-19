using System.Linq;
using System.Collections.Generic;
using ChampionsOfForest.Items;
using ChampionsOfForest.Items.Sets;
using ChampionsOfForest.Localization;
using ChampionsOfForest.Player;

using static ChampionsOfForest.Items.ItemDatabase.Stat;
using static ChampionsOfForest.Items.ItemDatabase;

namespace ChampionsOfForest.Items.ItemTemplates
{

	public abstract class ItemTemplateBuilder : ItemDefinition
	{

		private static List<ItemStat> defaultStats = null;
		private static List<ItemStat> magicStats = null;
		private static List<ItemStat> meleeStats = null;
		private static List<ItemStat> rangedStats = null;
		private static List<ItemStat> defenseStats = null;
		private static List<ItemStat> recoveryStats = null;

		protected const int PADDING = 10000;
		protected void Register()
		{

			id = ItemDatabase.ItemTemplateStorage[type].Count() + PADDING * (int)type;
			ItemDatabase.ItemTemplateStorage[type].Add(this);

		}

		public ItemTemplateBuilder()
		{
			if (defaultStats == null || defaultStats.Count() == 0)
			{
				defaultStats = ToItemStatList(defaultStatIds);
			}
			if (magicStats == null || magicStats.Count() == 0)
			{
				magicStats = ToItemStatList(magicStatIds);
			}
			if (meleeStats == null || meleeStats.Count() == 0)
			{
				meleeStats = ToItemStatList(meleeStatIds);
			}
			if (rangedStats == null || rangedStats.Count() == 0)
			{
				rangedStats = ToItemStatList(rangedStatIds);
			}
			if (defenseStats == null || defenseStats.Count() == 0)
			{
				defenseStats = ToItemStatList(defenseStatIds);
			}
			if (recoveryStats == null || recoveryStats.Count() == 0)
			{
				recoveryStats = ToItemStatList(recoveryStatIds);
			}
		}

		protected List<ItemStat> ToItemStatList(Stat[] stats)
		{
			var list = new List<ItemStat>();
			foreach (var stat in defaultStatIds)
			{
				list.Add(new ItemStat(ItemDatabase.Stats[(int)stat], 1, -1, 1));
			}
			return list;
		}

		// pool of stats to appear on items by default
		private readonly Stat[] defaultStatIds = new Stat[]
		{
			STRENGTH,
			AGILITY,
			VITALITY,
			INTELLIGENCE,
			MAX_LIFE,
			MAX_ENERGY,
			LIFE_REGEN_BASE,
			STAMINA_REGEN_BASE,
			STAMINA_AND_ENERGY_REGEN_MULT,
			LIFE_REGEN_MULT,
			DAMAGE_REDUCTION,
			CRIT_CHANCE,
			CRIT_DAMAGE,
			LIFE_ON_HIT,
			DODGE_CHANCE,
			ARMOR,
			ELITE_DAMAGE_REDUCTION,
			ATTACKSPEED,
			EXP_GAIN_MULT,
			MASSACRE_DURATION,
			SPELL_DAMAGE_MULTIPLIER,
			MELEE_DAMAGE_INCREASE,
			RANGED_DAMAGE_INCREASE,
			BASE_SPELL_DAMAGE,
			BASE_MELEE_DAMAGE,
			BASE_RANGED_DAMAGE,
			ALL_RECOVERY,
			MOVEMENT_SPEED,
			ATTACK_COST_REDUCTION,
			SPELL_COST_REDUCTION,
			SPELL_COST_TO_STAMINA,
			ENERGY_REGEN_BASE,
			MAX_LIFE_MULT,
			MAX_ENERGY_MULT,
			COOLDOWN_REDUCTION,
			ENERGY_ON_HIT,
			LOOT_QUANTITY,  // think about keeping this later
			LOOT_QUALITY,
			ALL_ATTRIBUTES,
			JUMP_POWER,
			THORNS
		};

		// adds the defaultStatIds n times to the stat pool for the item
		public ItemTemplateBuilder DefaultStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(defaultStats, probability));
			return this;
		}

		public ItemTemplateBuilder StatSlot(Stat[] statPool, float probability = 1)
		{
			statSlots.Add(new StatSlot(ToItemStatList(statPool), probability));
			return this;
		}

		public ItemTemplateBuilder DropsFrom(EnemyProgression.Enemy enemyTypes)
		{
			lootTable = enemyTypes;
			return this;
		}

		public ItemTemplateBuilder Name(string s)
		{
			name = s;
			return this;
		}

		public ItemTemplateBuilder Description(string s)
		{
			description = s;
			return this;
		}

		public ItemTemplateBuilder Lore(string s)
		{
			lore = s;
			return this;
		}

		public ItemTemplateBuilder UniqueStat(string statDescription, OnItemUsed _onEquip, OnItemUsed _onUnequip)
		{
			onEquipCallback = _onEquip;
			onUnequipCallback = _onUnequip;
			uniqueStat = statDescription;
			return this;
		}

		public ItemTemplateBuilder LevelRequirement(int minimumLevel)
		{
			minLevel = minimumLevel;
			maxLevel = minimumLevel + 1;
			return this;
		}

		public ItemTemplateBuilder Rarity(int r)
		{
			base.rarity = (Rarity)r;
			return this;
		}
		public ItemTemplateBuilder Rarity(Rarity r)
		{
			base.rarity = r;
			return this;
		}

		public ItemTemplateBuilder Icon(int iconid)
		{
			icon = Res.ResourceLoader.GetTexture(iconid); // Texture2D <- int
			return this;
		}

		private readonly Stat[] magicStatIds = new Stat[]
		{
			INTELLIGENCE,
			SPELL_DAMAGE_MULTIPLIER,
			BASE_SPELL_DAMAGE,
			SPELL_COST_REDUCTION,
			ENERGY_REGEN_BASE,
			MAX_ENERGY_MULT,
			COOLDOWN_REDUCTION,
			SPELL_DMG_FROM_INT
		};

		public ItemTemplateBuilder MagicStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(magicStats, probability));
			return this;
		}

		private readonly Stat[] meleeStatIds = new Stat[]
		{
			STRENGTH,
			MELEE_DAMAGE_INCREASE,
			BASE_MELEE_DAMAGE,
			MELEE_DMG_FROM_STR,
			MELEE_WEAPON_RANGE,
			MELEE_ARMOR_PIERCING,
		};

		public ItemTemplateBuilder MeleeStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(meleeStats, probability));
			return this;
		}

		private readonly Stat[] rangedStatIds = new Stat[]
		{
			AGILITY,
			RANGED_DAMAGE_INCREASE,
			BASE_RANGED_DAMAGE,
			RANGED_DMG_FROM_AGI,
			PROJECTILE_SPEED,
			PROJECTILE_SIZE,
			RANGED_ARMOR_PIERCING,
			HEADSHOT_DAMAGE_MULT,
			PROJECTILE_PIERCE_CHANCE,
			THROWN_SPEAR_DAMAGE,
		};

		public ItemTemplateBuilder RangedStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(rangedStats, probability));
			return this;
		}

		private readonly Stat[] defenseStatIds = new Stat[]
		{
			VITALITY,
			MAX_LIFE,
			DAMAGE_REDUCTION,
			DODGE_CHANCE,
			ARMOR,
			ELITE_DAMAGE_REDUCTION,
			MAX_HEALTH_FROM_VITALITY,
			MAX_LIFE_MULT,
			BLOCK,
			THORNS,
		};

		public ItemTemplateBuilder DefenseStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(defenseStats, probability));
			return this;
		}

		private readonly Stat[] recoveryStatIds = new Stat[]
		{
			LIFE_REGEN_BASE,
			LIFE_REGEN_MULT,
			LIFE_ON_HIT,
			ALL_RECOVERY,
			ENERGY_REGEN_BASE,
			ENERGY_ON_HIT,
			STAMINA_REGEN_BASE,
			STAMINA_AND_ENERGY_REGEN_MULT,
		};

		public ItemTemplateBuilder RecoveryStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(recoveryStats, probability));
			return this;
		}


		public ItemTemplateBuilder SetDropFromEverything()
		{
			//Lootable from everything
			lootTable = (EnemyProgression.Enemy)0b11111111111111111111111;
			return this;
		}

		//Sets the item to drop from only a specyfic group of enemies
		public ItemTemplateBuilder SetDropArmsy()
		{
			lootTable = EnemyProgression.Enemy.RegularArmsy | EnemyProgression.Enemy.PaleArmsy;
			return this;
		}

		public ItemTemplateBuilder SetDropVags()
		{
			lootTable = EnemyProgression.Enemy.PaleVags | EnemyProgression.Enemy.RegularVags;
			return this;
		}

		public ItemTemplateBuilder SetDropCow()
		{
			lootTable |= EnemyProgression.Enemy.Cowman;
			return this;
		}

		public ItemTemplateBuilder SetDropBaby()
		{
			lootTable |= EnemyProgression.Enemy.Baby;
			return this;
		}

		public ItemTemplateBuilder SetDropMegan()
		{
			lootTable |= EnemyProgression.Enemy.Megan;
			return this;
		}

		public ItemTemplateBuilder SetDropCreepy()
		{
			lootTable |= EnemyProgression.Enemy.RegularArmsy | EnemyProgression.Enemy.PaleArmsy | EnemyProgression.Enemy.RegularVags | EnemyProgression.Enemy.PaleVags | EnemyProgression.Enemy.Cowman | EnemyProgression.Enemy.Baby | EnemyProgression.Enemy.Girl | EnemyProgression.Enemy.Worm | EnemyProgression.Enemy.Megan;
			return this;
		}

		public ItemTemplateBuilder SetDropCannibals()
		{
			lootTable |=
				EnemyProgression.Enemy.NormalMale | EnemyProgression.Enemy.NormalLeaderMale | EnemyProgression.Enemy.NormalFemale | EnemyProgression.Enemy.NormalSkinnyMale | EnemyProgression.Enemy.NormalSkinnyFemale | EnemyProgression.Enemy.PaleMale | EnemyProgression.Enemy.PaleSkinnyMale | EnemyProgression.Enemy.PaleSkinnedMale | EnemyProgression.Enemy.PaleSkinnedSkinnyMale | EnemyProgression.Enemy.PaintedMale | EnemyProgression.Enemy.PaintedLeaderMale | EnemyProgression.Enemy.PaintedFemale | EnemyProgression.Enemy.Fireman;
			return this;
		}
		public ItemTemplateBuilder Weight(int w)
		{
			lootWeight = w;
			return this;
		}

		//TODO add a method to set the loot table to drop only from caves
	}
	public class Greatsword : ItemTemplateBuilder
	{
		public Greatsword()
		{
			type = ItemType.Weapon;
			subtype = ItemSubtype.GreatSword;
			Icon(88);

			Register();
        }
	}

	public class Longsword : ItemTemplateBuilder
	{
		public Longsword()
		{
			type = ItemType.Weapon;
			subtype = ItemSubtype.LongSword;
			Icon(89);

			Register();
		}
	}

	public class Hammer : ItemTemplateBuilder
	{
		public Hammer()
		{
			type = ItemType.Weapon;
			subtype = ItemSubtype.Hammer;
			Icon(109);

			Register();
		}
	}

	public class Polearm : ItemTemplateBuilder
	{
		public Polearm()
		{
			type = ItemType.Weapon;
			subtype = ItemSubtype.Polearm;
            Icon(181);

			Register();
		}
	}

	public class Axe : ItemTemplateBuilder
	{
		public Axe()
		{
			type = ItemType.Weapon;
			subtype = ItemSubtype.Axe;
            Icon(138);

			Register();
		}
	}

	public class Greatbow : ItemTemplateBuilder
	{
		public Greatbow()
		{
			type = ItemType.Weapon;
			subtype = ItemSubtype.Greatbow;
            Icon(170);

			Register();
		}
	}


	public class Material : ItemTemplateBuilder
	{
		public Material()
		{
			type = ItemType.Material;
			LevelRequirement(20);
			// Materials dont have a default icon

			Register();
		}
	}

	public class Shield : ItemTemplateBuilder
	{

		private static readonly Stat[] shieldStatIds =
		{
			STRENGTH,
			VITALITY,
			MAX_LIFE,
			MAX_ENERGY,
			LIFE_REGEN_BASE,
			STAMINA_REGEN_BASE,
			STAMINA_AND_ENERGY_REGEN_MULT,
			LIFE_REGEN_MULT,
			DAMAGE_REDUCTION,
			CRIT_CHANCE,
			CRIT_DAMAGE,
			LIFE_ON_HIT,
			DODGE_CHANCE,
			ARMOR,
			ELITE_DAMAGE_REDUCTION,
			ATTACKSPEED,
			MASSACRE_DURATION,
			MELEE_DAMAGE_INCREASE,
			BASE_MELEE_DAMAGE,
			ALL_RECOVERY,
			MELEE_WEAPON_RANGE,
			ATTACK_COST_REDUCTION,
			SPELL_COST_REDUCTION,
			SPELL_COST_TO_STAMINA,
			ENERGY_REGEN_BASE,
			MAX_LIFE_MULT,
			MAX_ENERGY_MULT,
			COOLDOWN_REDUCTION,
			ENERGY_ON_HIT,
			BLOCK,
			MELEE_ARMOR_PIERCING,
			THORNS,
			THROWN_SPEAR_DAMAGE,
			};
		private static List<ItemStat> shieldStats = null;

		public Shield ShieldStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(shieldStats, probability));
			return this;
		}

		public Shield()
		{
			type = ItemType.Shield;
			if (shieldStats == null)
				shieldStats = ToItemStatList(shieldStatIds);
			LevelRequirement(10);
            Icon(99);
			maximumSocketSlots = 1;

			Register();
		}
	}

	public class Quiver : ItemTemplateBuilder
	{
		public Quiver()
		{
			type = ItemType.Quiver;
			LevelRequirement(10);
            Icon(98);

			Register();
		}
	}

	public class Other : ItemTemplateBuilder
	{
		public Other()
		{
			type = ItemType.Other;
            Icon(105);

			Register();
		}
	}

	public class Helmet : ItemTemplateBuilder
	{
		public Helmet()
		{
			type = ItemType.Helmet;
			LevelRequirement(6);
            Icon(91);

			Register();
		}
	}

	public class Boot : ItemTemplateBuilder
	{
		public Boot()
		{
			type = ItemType.Boot;
			LevelRequirement(5);
            Icon(85);

			Register();
		}
	}

	public class Pants : ItemTemplateBuilder
	{
		public Pants()
		{
			type = ItemType.Pants;
            Icon(87);

			Register();
		}
	}

	public class ChestArmor : ItemTemplateBuilder
	{
		public ChestArmor()
		{
			type = ItemType.ChestArmor;
			LevelRequirement(1);
            Icon(96);

			Register();
		}
	}

	public class ShoulderArmor : ItemTemplateBuilder
	{
		public ShoulderArmor()
		{
			type = ItemType.ShoulderArmor;
			LevelRequirement(8);
            Icon(95);

			Register();
		}
	}

	public class Glove : ItemTemplateBuilder
	{
		public Glove()
		{
			type = ItemType.Glove;
			LevelRequirement(7);
            Icon(86);

			Register();
		}
	}

	public class Bracer : ItemTemplateBuilder
	{
		public Bracer()
		{
			type = ItemType.Bracer;
			LevelRequirement(9);
            Icon(93);

			Register();
		}
	}

	public class Amulet : ItemTemplateBuilder
	{


		private static readonly Stat[] amuletStatIds =
		{
			STRENGTH,
			AGILITY,
			VITALITY,
			INTELLIGENCE,
			MAX_LIFE,
			MAX_ENERGY,
			LIFE_REGEN_BASE,
			STAMINA_REGEN_BASE,
			STAMINA_AND_ENERGY_REGEN_MULT,
			LIFE_REGEN_MULT,
			DAMAGE_REDUCTION,
			CRIT_CHANCE,
			CRIT_DAMAGE,
			LIFE_ON_HIT,
			DODGE_CHANCE,
			ARMOR,
			ELITE_DAMAGE_REDUCTION,
			ATTACKSPEED,
			EXP_GAIN_MULT,
			MASSACRE_DURATION,
			SPELL_DAMAGE_MULTIPLIER,
			MELEE_DAMAGE_INCREASE,
			RANGED_DAMAGE_INCREASE,
			BASE_SPELL_DAMAGE,
			BASE_MELEE_DAMAGE,
			BASE_RANGED_DAMAGE,
			MAX_ENERGY_FROM_AGILITY,
			MAX_HEALTH_FROM_VITALITY,
			SPELL_DMG_FROM_INT,
			MELEE_DMG_FROM_STR,
			ALL_RECOVERY,
			MOVEMENT_SPEED,
			MELEE_WEAPON_RANGE,
			ATTACK_COST_REDUCTION,
			SPELL_COST_REDUCTION,
			SPELL_COST_TO_STAMINA,
			ENERGY_REGEN_BASE,
			MAX_LIFE_MULT,
			MAX_ENERGY_MULT,
			COOLDOWN_REDUCTION,
			RANGED_DMG_FROM_AGI,
			ENERGY_ON_HIT,
			BLOCK,
			PROJECTILE_SPEED,
			PROJECTILE_SIZE,
			MELEE_ARMOR_PIERCING,
			RANGED_ARMOR_PIERCING,
			ARMOR_PIERCING,
			ALL_ATTRIBUTES,
			HEADSHOT_DAMAGE_MULT,
			FIRE_DAMAGE,
			CHANCE_ON_HIT_TO_SLOW,
			CHANCE_ON_HIT_TO_BLEED,
			CHANCE_ON_HIT_TO_WEAKEN,
			THORNS,
			PROJECTILE_PIERCE_CHANCE,
			EXPLOSION_DAMAGE,
			THROWN_SPEAR_DAMAGE,


			};
		private static List<ItemStat> amuletStats = null;


		public Amulet()
		{
			type = ItemType.Amulet;
			if (amuletStats == null)
				amuletStats = ToItemStatList(amuletStatIds);
			LevelRequirement(20);
			Icon(101);

			Register();
		}

		public Amulet AmuletStatSlot(int n = 1, float probability = 1)
		{
			for (int i = 0; i < n; i++)
				statSlots.Add(new StatSlot(amuletStats, probability));
			return this;
		}

		public Amulet Scarf()
		{
			Icon(100); // this is the id of scarf icon
			return this;
		}
	}

	public class Ring : ItemTemplateBuilder
	{
		private static readonly Stat[] ringStatIds =
		{
				STRENGTH,
				AGILITY,
				VITALITY,
				INTELLIGENCE,
				MAX_LIFE,
				MAX_ENERGY,
				LIFE_REGEN_BASE,
				STAMINA_REGEN_BASE,
				STAMINA_AND_ENERGY_REGEN_MULT,
				LIFE_REGEN_MULT,
				CRIT_CHANCE,
				CRIT_DAMAGE,
				LIFE_ON_HIT,
				ATTACKSPEED,
				EXP_GAIN_MULT,
				ALL_RECOVERY,
				ATTACK_COST_REDUCTION,
				SPELL_COST_REDUCTION,
				SPELL_COST_TO_STAMINA,
				ENERGY_REGEN_BASE,
				MAX_LIFE_MULT,
				MAX_ENERGY_MULT,
				COOLDOWN_REDUCTION,
				ENERGY_ON_HIT,
				PROJECTILE_SPEED,
				PROJECTILE_SIZE,
				LOOT_QUANTITY,
				ALL_ATTRIBUTES
			};
		private static List<ItemStat> ringStats = null;

		public Ring()
		{
			type = ItemType.Ring;
			if (ringStats == null)
				ringStats = ToItemStatList(ringStatIds);
			Icon(90);
			LevelRequirement(15);

			Register();
		}

		public Ring RingStatSlot(int n = 1, float probability = 1)
		{
			for(int i = 0; i<n; i++)
				statSlots.Add(new StatSlot(ringStats, probability));
			return this;
		}
	}

	public class SpellScroll : ItemTemplateBuilder
	{
		public SpellScroll()
		{
			type = ItemType.SpellScroll;
			LevelRequirement(10);
			Icon(110);

			Register();
		}
	}

	public class Heart : ItemTemplateBuilder
	{
		public Heart(string effectDescription, OnItemUsed consumeEvent)
		{
			type = ItemType.Other;
			LevelRequirement(1);

			Icon(105);
			Register();

			onEquipCallback= consumeEvent;
			uniqueStat = effectDescription;
		}
	}

    /*
	 * OLD
		new BaseItem(new int[][]
		{
		})
		{
			name = Translations.ItemDatabase_ItemDefinitions_201, //tr
			description = Translations.ItemDatabase_ItemDefinitions_202, //tr
			lore = Translations.ItemDatabase_ItemDefinitions_203, //tr
			uniqueStat = Translations.ItemDatabase_ItemDefinitions_204, //tr
			Rarity = 4,
			minLevel = 1,
			maxLevel = 2,
			CanConsume = true,
			StackSize = 100,
			type = BaseItem.ItemType.Other,
			icon = Res.ResourceLoader.GetTexture(105),
			onEquip = ModdedPlayer.Respec
		};


	* NEW
		new Consumable()
			.Name("heart of purity")
			.Description("respecs spent points")
			.Something()
			.DefaultStatSlot(3)
			.StatSlot({ BASESPELLDAMAGE, BASEMELEEDAMAGE, BASERANGEDDAMAGE })
			.StatSlot({ THORNS })
	 
	 # Example 2:
		new Ring()
            .RingStatSlot(3)
            .DefaultStatSlot(1)
            .StatSlot(new Stat[] {THORNS} )
            .Name("Hazards test loop")
            .Description("Description goes here")
            .Rarity(4); 
	 
	 */

}
