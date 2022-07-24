using System.Collections.Generic;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Items;
using ChampionsOfForest.Localization;

using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Player.Perk;

namespace ChampionsOfForest.Player
{
	public class PerkDatabase
	{
		public static List<Perk> perks = new List<Perk>();

		public static void FillPerkList()
		{
			perks.Clear();
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.meleeDmgFromStr.valueAdditive += 0.01f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = Translations.PerkDatabase_1,
				originalDescription = Translations.PerkDatabase_2("1%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellDmgFromInt.valueAdditive += 0.01f,

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = Translations.PerkDatabase_3,
				originalDescription = Translations.PerkDatabase_4("1%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.rangedDmgFromAgi.valueAdditive += 0.01f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = Translations.PerkDatabase_5,
				originalDescription = Translations.PerkDatabase_6("1%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.energyRecoveryFromInt.valueAdditive += 0.01f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = Translations.PerkDatabase_7,
				originalDescription = Translations.PerkDatabase_8("1%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxEnergyFromAgi.valueAdditive += 0.5f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 0,
				name = Translations.PerkDatabase_9,
				originalDescription = Translations.PerkDatabase_10(0.5),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxHealthFromVit.valueAdditive += 2f,

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = Translations.PerkDatabase_11,
				originalDescription = Translations.PerkDatabase_12(2),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allRecoveryMult.Add(0.1f),

				category = PerkCategory.Support,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 0,
				name = Translations.PerkDatabase_13,
				originalDescription = Translations.PerkDatabase_14("10%"),
				updateDescription = x =>
				{
					float f = 0.1f * x;
					return Translations.PerkDatabase_15((f).ToString("P"));
				},
				textureVariation = 0, //0 or 1
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.perk_hungerRate.valueMultiplicative *= 0.7f; ModdedPlayer.Stats.perk_thirstRate.valueMultiplicative *= 0.7f; },
				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 4 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_16,
				originalDescription = Translations.PerkDatabase_17("30%"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.7f;
					for (int i = 1; i < x; i++)
						f *= 0.7f;
					return Translations.PerkDatabase_15((1 - f).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.staminaRecoveryperSecond.Add(0.5f),
				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 4 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_18,
				originalDescription = Translations.PerkDatabase_19(0.5),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x => Translations.PerkDatabase_21((0.5f * x).ToString("N1"))
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.meleeFlatDmg.valueAdditive += 5,
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 0, 10 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_22,
				originalDescription = Translations.PerkDatabase_23(5),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15(x * 5);
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.meleeIncreasedDmg.valueAdditive += 0.1f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 0, 9, 11 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_24,
				originalDescription = Translations.PerkDatabase_25("10%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.strength.Add(20),
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 0, 10 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_26,
				originalDescription = Translations.PerkDatabase_27(20),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.rangedFlatDmg.valueAdditive += 8,
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_28,
				originalDescription = Translations.PerkDatabase_29("8"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15(x * 8);
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.projectileSize.valueAdditive += 0.05f,
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_30,
				originalDescription = Translations.PerkDatabase_31("5%"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((x * 0.05f).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.projectileSpeed.valueAdditive += 0.05f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_32,
				originalDescription = Translations.PerkDatabase_33("5%"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((x * 0.05f).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellCostEnergyCost.Multiply(0.85f),

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 1 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_34,
				originalDescription = Translations.PerkDatabase_35("15%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellCost.valueMultiplicative *= 1 - 0.09f,

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 1 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_36,
				originalDescription = Translations.PerkDatabase_37("9%"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.91f;
					for (int i = 1; i < x; i++)
						f *= 0.91f;
					return Translations.PerkDatabase_15((1 - f).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.70f;
					ModdedPlayer.Stats.allDamage.valueMultiplicative *= 0.70f;
					ModdedPlayer.Stats.thornsDmgMult.Multiply(1.69f);
				},

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { 5 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_38,
				originalDescription = Translations.PerkDatabase_39("30%", "69%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.92f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_40,
				originalDescription = Translations.PerkDatabase_41("8%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.92f;
					for (int i = 1; i < x; i++)
						f *= 0.92f;
					return Translations.PerkDatabase_15((1 - f).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.85f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 18 },
				levelReq = 8,
				cost = 2,
				scale = 1,
				posX = 3f,
				posY = 2.25f,
				name = Translations.PerkDatabase_42,
				originalDescription = Translations.PerkDatabase_43("15%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => StatActions.AddAllStats(5),
				category = PerkCategory.Utility,
				unlockPath = new int[] { 3 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_44,
				originalDescription = Translations.PerkDatabase_45(5),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => StatActions.AddAllStats(20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 20 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_44,
				originalDescription = Translations.PerkDatabase_45(20),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => StatActions.AddAttackSpeed(0.05f),

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11 },
				levelReq = 6,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = Translations.MainMenu_Guide_49,
				originalDescription = Translations.PerkDatabase_47("5%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((0.05f * x).ToString("P"));
				},
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileNoConsumeChance.valueAdditive += 0.04f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12, 14 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_48,
				originalDescription = Translations.PerkDatabase_49("4%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((0.04f * x).ToString("P"));
				},
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileNoConsumeChance.valueAdditive += 0.13f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 23 },
				levelReq = 9,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_50,
				originalDescription = Translations.PerkDatabase_49("13%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileNoConsumeChance.valueAdditive += 0.13f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 24 },
				levelReq = 12,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -2.25f,
				name = Translations.PerkDatabase_52,
				originalDescription = Translations.PerkDatabase_49("13%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => StatActions.AddAllStats(10),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 21 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_44,
				originalDescription = Translations.PerkDatabase_45(10),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.jumpPower.valueAdditive += 0.15f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 3 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_54,
				originalDescription = Translations.PerkDatabase_55("15%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((0.15f * x).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.movementSpeed.valueAdditive += 0.1f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 27 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_56,
				originalDescription = Translations.PerkDatabase_57(" 10%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((0.1f * x).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxHealth.valueAdditive += 35,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 5 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_58,
				originalDescription = Translations.PerkDatabase_59(" 35"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((35 * x).ToString("N"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.healthRecoveryPerSecond.valueAdditive += 0.25f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 6 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_60,
				originalDescription = Translations.PerkDatabase_61("0.25"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((0.25f * x).ToString("N2"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.armor.Add(80),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 5 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_63,
				originalDescription = Translations.PerkDatabase_64("80"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15((80 * x).ToString("N"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.9f,
				category = PerkCategory.Defense,
				unlockPath = new int[] { 31 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_65,
				originalDescription = Translations.PerkDatabase_66("10%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.85f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 32 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_67,
				originalDescription = Translations.PerkDatabase_68("15%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.8f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 33 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_69,
				originalDescription = Translations.PerkDatabase_68("20%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.magicDamageTaken.Multiply(0.85f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 29, 31 },
				levelReq = 6,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.MainMenu_Guide_19,
				originalDescription = Translations.PerkDatabase_70("15%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.85f;
					for (int i = 1; i < x; i++)
						f *= 0.85f;
					return Translations.PerkDatabase_15((1 - f).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.getHitChance.Multiply(0.75f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 34 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 2.25f,
				name = Translations.PerkDatabase_71,
				originalDescription = Translations.PerkDatabase_72("25%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allArmorPiercing.valueAdditive += 5,

				category = PerkCategory.Support,
				unlockPath = new int[] { 6 },
				levelReq = 6,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_73,
				originalDescription = Translations.PerkDatabase_74(5),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.meleeArmorPiercing.valueAdditive += 12,

				category = PerkCategory.Support,
				unlockPath = new int[] { 37 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_75,
				originalDescription = Translations.PerkDatabase_76(12),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.rangedArmorPiercing.valueAdditive += 6,

				category = PerkCategory.Support,
				unlockPath = new int[] { 37 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_77,
				originalDescription = Translations.PerkDatabase_78(6),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.healthPerSecRate.valueAdditive += 0.1f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 30 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_79,
				originalDescription = Translations.PerkDatabase_80("10%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.energyRecoveryperSecond.valueAdditive += 0.15f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 30 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_81,
				originalDescription = Translations.PerkDatabase_82(0.15f),
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.expGain.Add(0.1f),

				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 0f,
				name = Translations.PerkDatabase_83,
				originalDescription = Translations.PerkDatabase_84("10%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.healthOnHit.valueAdditive += 1f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_85,
				originalDescription = Translations.PerkDatabase_86("1"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.energyOnHit.valueAdditive += 0.5f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 41 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_87,
				originalDescription = Translations.PerkDatabase_88("0.5"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.instance.AddGeneratedResource(33, 10);
					MoreCraftingReceipes.SetCustomReceipeUnlockState(MoreCraftingReceipes.CustomReceipe.ClothFromBoar, true);
					MoreCraftingReceipes.SetCustomReceipeUnlockState(MoreCraftingReceipes.CustomReceipe.ClothFromDeer, true);
					MoreCraftingReceipes.SetCustomReceipeUnlockState(MoreCraftingReceipes.CustomReceipe.ClothFromRabbit, true);
					MoreCraftingReceipes.SetCustomReceipeUnlockState(MoreCraftingReceipes.CustomReceipe.ClothFromRacoon, true);
					MoreCraftingReceipes.AddReceipes();
				},
				category = PerkCategory.Utility,
				unlockPath = new int[] { 4 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 0,
				name = Translations.PerkDatabase_89,
				originalDescription = Translations.PerkDatabase_90("10"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddGeneratedResource(29, 2),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -3.5f,
				posY = 0,
				name = Translations.PerkDatabase_91,
				originalDescription = Translations.PerkDatabase_92("2"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.instance.AddExtraItemCapacity(29, 15); ModdedPlayer.instance.AddExtraItemCapacity(175, 15); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 46 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_93,
				originalDescription = Translations.PerkDatabase_94("15"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddGeneratedResource(175, 5),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 47 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_95,
				originalDescription = Translations.PerkDatabase_96("5"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddGeneratedResource(49, 3),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 0,
				name = Translations.PerkDatabase_97,
				originalDescription = Translations.PerkDatabase_98("3"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddGeneratedResource(262, 3),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -6.5f,
				posY = 0,
				name = Translations.PerkDatabase_99,
				originalDescription = Translations.PerkDatabase_100("3"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddGeneratedResource(37, 5),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -5.5f,
				posY = 0,
				name = Translations.PerkDatabase_101,
				originalDescription = Translations.PerkDatabase_102("5"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(37, 15),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 51 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -6f,
				posY = -0.75f,
				name = Translations.PerkDatabase_103,
				originalDescription = Translations.PerkDatabase_104("15"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(49, 20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 49 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_105,
				originalDescription = Translations.PerkDatabase_106("20"),
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileNoConsumeChance.valueAdditive += 0.45f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 25 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = -3f,
				name = Translations.PerkDatabase_107,
				originalDescription = Translations.PerkDatabase_108("45%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellFlatDmg.valueAdditive += 10,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 1 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_109,
				originalDescription = Translations.PerkDatabase_110("10"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 76, 35, 123, 207, 127 }, 5),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -7.5f,
				posY = 0,
				name = Translations.PerkDatabase_111,
				originalDescription = Translations.PerkDatabase_112("5"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 109, 89 }, 20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -8.5f,
				posY = 0,
				name = Translations.PerkDatabase_113,
				originalDescription = Translations.PerkDatabase_114("20"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(307, 20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -13.5f,
				posY = 0,
				name = Translations.PerkDatabase_115,
				originalDescription = Translations.PerkDatabase_116("20"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.instance.AddExtraItemCapacity(94, 20); ModdedPlayer.instance.AddExtraItemCapacity(178, 100); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -9.5f,
				posY = 0,
				name = Translations.PerkDatabase_117,
				originalDescription = Translations.PerkDatabase_118(100, 20),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 90, 47, 46, 101 }, 10),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 59 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -10f,
				posY = 0.75f,
				name = Translations.PerkDatabase_119,
				originalDescription = Translations.PerkDatabase_120("10"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.instance.AddExtraItemCapacity(57, 6); ModdedPlayer.instance.AddExtraItemCapacity(53, 4); ModdedPlayer.instance.AddExtraItemCapacity(54, 2); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -10.5f,
				posY = 0,
				name = Translations.PerkDatabase_121,
				originalDescription = Translations.PerkDatabase_122(6,4,2),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 31, 142, 141, 41, 43, 144 }, 5),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -11.5f,
				posY = 0,
				name = Translations.PerkDatabase_123,
				originalDescription = Translations.PerkDatabase_124("5"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 177, 71, 56 }, 5); ModdedPlayer.instance.AddExtraItemCapacity(82, 50); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -12.5f,
				posY = 0,
				name = Translations.PerkDatabase_125,
				originalDescription = Translations.PerkDatabase_126(5,50),
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearDamageMult.valueMultiplicative *= 2f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_127,
				originalDescription = Translations.PerkDatabase_128("100%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bulletDamageMult.valueMultiplicative *= 1.75f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = Translations.PerkDatabase_129,
				originalDescription = Translations.PerkDatabase_130("75%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_crossbowDamageMult.valueMultiplicative *= 2.0f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -3f,
				name = Translations.PerkDatabase_131,
				originalDescription = Translations.PerkDatabase_132("100%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bowDamageMult.valueMultiplicative *= 1.35f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -3.75f,
				name = Translations.PerkDatabase_133,
				originalDescription = Translations.PerkDatabase_134("35%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_healingDomeGivesImmunity.value = true,

				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_135,
				originalDescription = Translations.PerkDatabase_136,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => BlackFlame.GiveDamageBuff = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_137,
				originalDescription = Translations.PerkDatabase_138("33%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_warCryGiveDamage.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_139,
				originalDescription = Translations.PerkDatabase_140,
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeIncreasedDmg.valueAdditive += 0.25f; ModdedPlayer.Stats.attackStaminaCost.valueMultiplicative *= 1.4f; },

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11, 10 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_141,
				originalDescription = Translations.PerkDatabase_142("40%", "25%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 1.25f;
					for (int i = 1; i < x; i++)
						f += 0.25f;
					float f1 = 1.4f;
					for (int i = 1; i < x; i++)
						f1 *= 1.4f;
					return Translations.PerkDatabase_15((f - 1).ToString("P") + ", " + (f1 - 1).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.spellIncreasedDmg.valueAdditive += 0.25f; ModdedPlayer.Stats.spellCost.valueMultiplicative *= 1.4f; },

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 15, 55 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_145,
				originalDescription = Translations.PerkDatabase_146("25%", "40%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				 {
					 float f1 = 0.25f * x;
					 float f2 = Mathf.Pow(1.4f, x);
					 return Translations.PerkDatabase_148(f1,f2-1.0f);
				 }
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_magicArrowDmgDebuff.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -2.25f,
				name = Translations.PerkDatabase_149,
				originalDescription = Translations.PerkDatabase_150("50%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_magicArrowDuration.valueAdditive += 5,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 73 },
				levelReq = 40,
				cost = 2,
				scale = 1,
				posX = 4f,
				posY = -2.25f,
				name = Translations.PerkDatabase_151,
				originalDescription = Translations.PerkDatabase_152(5),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_magicArrowDoubleSlow.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 74 },
				levelReq = 46,
				cost = 2,
				scale = 1,
				posX = 5f,
				posY = -2.25f,
				name = Translations.PerkDatabase_153,
				originalDescription = Translations.PerkDatabase_154("40%", "80%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.heavyAttackDmg.valueMultiplicative *= 1.6f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 10 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_155,
				originalDescription = Translations.PerkDatabase_156("60%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.heavyAttackDmg.valueMultiplicative *= 3f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 76 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 0f,
				name = Translations.PerkDatabase_157,
				originalDescription = Translations.PerkDatabase_158("200%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.critDamage.valueAdditive += 0.20f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9, 10 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_159,
				originalDescription = Translations.PerkDatabase_160("20%"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.critChance.valueAdditive += 0.10f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9, 78 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_161,
				originalDescription = Translations.PerkDatabase_162("10%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxEnergy.valueAdditive += 15,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 3 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_163,
				originalDescription = Translations.PerkDatabase_164("15"),
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_multishotProjectileCount.valueAdditive += 2,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 31,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_165,
				originalDescription = Translations.PerkDatabase_166(2),
				updateDescription = x => Translations.PerkDatabase_167( (10 * Mathf.Pow(2 * x, 1.75f)).ToString("N"), (10 * Mathf.Pow((2 + 2 * x), 1.75f)).ToString("N")),
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_turboRaftOwners.Add(1),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 50 },
				levelReq = 22,
				cost = 1,
				scale = 1,
				posX = -7f,
				posY = 0.75f,
				name = Translations.PerkDatabase_168,
				originalDescription = Translations.PerkDatabase_169,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_RaftSpeedMultipier.Add(0.5f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 82 },
				levelReq = 23,
				cost = 1,
				scale = 1,
				posX = -7.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_170,
				originalDescription = Translations.PerkDatabase_171("50%"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_purgeHeal.value = true,
				category = PerkCategory.Support,
				unlockPath = new int[] { 6 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -0.75f,
				name = Translations.PerkDatabase_172,
				originalDescription = Translations.PerkDatabase_173,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.925f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 24,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 3f,
				name = Translations.PerkDatabase_42,
				originalDescription = Translations.PerkDatabase_43("7.5%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.9f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 85 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 3.75f,
				name = Translations.PerkDatabase_42,
				originalDescription = Translations.PerkDatabase_174("10%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.88f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 86 },
				levelReq = 46,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 4.5f,
				name = Translations.PerkDatabase_42,
				originalDescription = Translations.PerkDatabase_174("12%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_infinityMagic.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 87 },
				levelReq = 60,
				cost = 2,
				scale = 1,
				posX = 1f,
				posY = 5.25f,
				name = Translations.PerkDatabase_175,
				originalDescription = Translations.PerkDatabase_176("5%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellIncreasedDmg.valueAdditive *= 1.7f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 88 },
				levelReq = 61,
				cost = 2,
				scale = 1,
				posX = 0.5f,
				posY = 6f,
				name = Translations.PerkDatabase_177,
				originalDescription = Translations.PerkDatabase_178("70%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_fireDmgIncreaseOnHit.value = true,

				category = PerkCategory.Support,
				unlockPath = new int[] { -1 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0f,
				name = Translations.PerkDatabase_179,
				originalDescription = Translations.PerkDatabase_180(15),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_nearDeathExperienceUnlocked.value = true,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 35 },
				levelReq = 20,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_181,
				originalDescription = Translations.PerkDatabase_182("100%",10,5),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrow_HeadDamage.Add(0.5f),

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 93 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -3.75f,
				name = Translations.PerkDatabase_183,
				originalDescription = Translations.PerkDatabase_184,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.projectile_DamagePerDistance.valueAdditive += 0.01f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 67 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = -1f,
				posY = -3.75f,
				name = Translations.PerkDatabase_185,
				originalDescription = Translations.PerkDatabase_186("3%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrow_SlowDuration.valueAdditive += 6,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 92 },
				levelReq = 41,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = -3.75f,
				name = Translations.PerkDatabase_187,
				originalDescription = Translations.PerkDatabase_188(6),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrow_SlowAmount.valueAdditive -= 0.3f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 94 },
				levelReq = 42,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -3.75f,
				name = Translations.PerkDatabase_189,
				originalDescription = Translations.PerkDatabase_190("10%", "40%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusOnHS.valueAdditive += 0.5f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 217, 13 },
				levelReq = 14,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_191,
				originalDescription = Translations.PerkDatabase_192("50%", "100%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusOnAtkSpeed.valueAdditive += 0.2f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 96 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_193,
				originalDescription = Translations.PerkDatabase_196("30%","50%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusOnAtkSpeed.valueAdditive += 0.25f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 97 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_195,
				originalDescription = Translations.PerkDatabase_196("50%","75%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusSlowDuration.valueAdditive += 10f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 98 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 5.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_197,
				originalDescription = Translations.PerkDatabase_198(10),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => BlackFlame.GiveAfterburn = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 69 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = Translations.PerkDatabase_199,
				originalDescription = Translations.PerkDatabase_200(BlackFlame.afterburn_chance.ToString("P"),
					(BlackFlame.afterburn_debuff_amount-1).ToString("P"),BlackFlame.afterburn_duration),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => BlackFlame.DmgAmp = 2,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 100 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -3f,
				name = Translations.PerkDatabase_201,
				originalDescription = Translations.PerkDatabase_202,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_frenzyAtkSpeed.valueAdditive += 0.06f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 72 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_203,
				originalDescription = Translations.PerkDatabase_204("6%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDuration.valueAdditive += 4,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 72 },
				levelReq = 26,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_205,
				originalDescription = Translations.PerkDatabase_206(4),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_shieldPersistanceLifetime.valueAdditive += 10f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_207,
				originalDescription = Translations.PerkDatabase_208(10),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_blinkDamage.Add(44f),

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 86 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 3.75f,
				name = Translations.PerkDatabase_209,
				originalDescription = Translations.PerkDatabase_210,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.energyRecoveryFromInt.valueAdditive += 0.008f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 21 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_211,
				originalDescription = Translations.PerkDatabase_212("0.8%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_parryIgnites.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_213,
				originalDescription = Translations.PerkDatabase_214,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_parryRadius.Add(1),

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19, 16 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_215,
				originalDescription = Translations.PerkDatabase_216(1),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => StatActions.AddMagicFind(0.13f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { -1 },
				levelReq = 5,
				cost = 1,
				scale = 1f,
				posX = -0.75f,
				posY = -1.1f,
				name = Translations.PerkDatabase_217,
				originalDescription = Translations.PerkDatabase_218("13%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => StatActions.AddMagicFind(0.15f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 109 },
				levelReq = 15,
				cost = 1,
				scale = 1f,
				posX = -1.25f,
				posY = -1.85f,
				name = Translations.PerkDatabase_219,
				originalDescription = Translations.PerkDatabase_220("15%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bia_HealthTaken.valueAdditive += 0.25f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 67 },
				levelReq = 16,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -3.75f,
				name = Translations.PerkDatabase_221,
				originalDescription = Translations.PerkDatabase_222("25%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bia_HealthDmMult.valueAdditive += 5f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 14,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 2.25f,
				name = Translations.PerkDatabase_223,
				originalDescription = Translations.PerkDatabase_224(5),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_healingDomeRegEnergy.value = true,

				category = PerkCategory.Support,
				unlockPath = new int[] { 68 },
				levelReq = 36,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_225,
				originalDescription = Translations.PerkDatabase_226,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxHealthMult.valueAdditive += 0.20f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 29 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_227,
				originalDescription = Translations.PerkDatabase_228("20%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxEnergyMult.valueAdditive += 0.20f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 80 },
				levelReq = 9,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_229,
				originalDescription = Translations.PerkDatabase_230("20%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_craftingReroll.value = true,

				category = PerkCategory.Utility,
				unlockPath = new int[] { -1 },
				levelReq = 10,
				cost = 0,
				scale = 1f,
				posX = -0.75f,
				posY = 1.1f,
				name = Translations.PerkDatabase_231,
				originalDescription = Translations.PerkDatabase_232(2),
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_craftingReforge.value = true,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 116 },
				levelReq = 25,
				cost = 0,
				scale = 1f,
				posX = -0.25f,
				posY = 1.85f,
				name = Translations.PerkDatabase_233,
				originalDescription = Translations.PerkDatabase_234(3),
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.perk_flashlightIntensity.valueAdditive++; ModdedPlayer.Stats.perk_flashlightBatteryDrain.valueAdditive++; },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 62 },
				levelReq = 20,
				cost = 1,
				scale = 1f,
				posX = -12f,
				posY = -0.75f,
				name = Translations.PerkDatabase_235,
				originalDescription = Translations.PerkDatabase_236("100%"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.allDamageTaken.Multiply(1.12f); ModdedPlayer.Stats.allDamage.Add(0.12f); },

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { 5 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_237,
				originalDescription = Translations.PerkDatabase_238("12%"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					float f = 1.12f;
					for (int i = 1; i < x; i++)
						f *= 1.12f;
					return Translations.PerkDatabase_15((0.12f * x).ToString("P"));
				},
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileDamageIncreasedBySize.value = true,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 13 },
				levelReq = 32,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_241,
				originalDescription = Translations.PerkDatabase_242,
				textureVariation = 0, //0 or 1
				stackable = false,
				updateDescription = _ => Translations.PerkDatabase_243("1%", "2%")
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bunnyHop.value = true,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 27 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_244,
				originalDescription = Translations.PerkDatabase_245,
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.weaponRange.valueMultiplicative *= 1.1f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 22 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -2.25f,
				name = Translations.PerkDatabase_246,
				originalDescription = Translations.PerkDatabase_247("10%"),
				textureVariation = 0, //0 or 1
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.chanceToBleed.valueAdditive += 0.07f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 78, 79 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_248,
				originalDescription = Translations.PerkDatabase_249("7%"),
				textureVariation = 0, //0 or 1
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.chanceToWeaken.valueAdditive += 0.07f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 71, 22 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_250,
				originalDescription = Translations.PerkDatabase_251("7%"),
				textureVariation = 0, //0 or 1
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearCritChance.valueAdditive += 0.36f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 64 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_252,
				originalDescription = Translations.PerkDatabase_253("40%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearCritChance.valueAdditive += 0.15f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 125 },
				levelReq = 33,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -2.25f,
				name = Translations.PerkDatabase_254,
				originalDescription = Translations.PerkDatabase_255("15%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearhellChance.valueAdditive += 0.40f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 126 },
				levelReq = 34,
				cost = 2,
				scale = 1,
				posX = -1f,
				posY = -2.25f,
				name = Translations.PerkDatabase_256,
				originalDescription = Translations.PerkDatabase_257("40%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearhellChance.valueAdditive += 0.06f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 127 },
				levelReq = 53,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -2.25f,
				name = Translations.PerkDatabase_258,
				originalDescription = Translations.PerkDatabase_259("6%"),
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>Translations.PerkDatabase_443((x * 0.06f + 0.4f).ToString("P"))//tr
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearDamageMult.valueMultiplicative *= 2f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 126 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = -0.5f,
				posY = -3f,
				name = Translations.PerkDatabase_260,
				originalDescription = Translations.PerkDatabase_261,
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bulletCritChance.valueAdditive += 0.2f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 65 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -2.25f,
				name = Translations.PerkDatabase_262,
				originalDescription = Translations.PerkDatabase_263("20%","30%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearExtraArmorReduction.value = true,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 127, 129 },
				levelReq = 35,
				cost = 2,
				scale = 1,
				posX = -1.5f,
				posY = -3f,
				name = Translations.PerkDatabase_264,
				originalDescription = Translations.PerkDatabase_265("300%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bunnyHopUpgrade.value = true,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 121 },
				levelReq = 55,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_266,
				originalDescription = Translations.PerkDatabase_267,
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1.7f; ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= 0.9f; ModdedPlayer.Stats.rangedIncreasedDmg.Multiply(0.80f); },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 7f,
				posY = -0.75f,
				name = Translations.PerkDatabase_268,
				originalDescription = Translations.PerkDatabase_269("70%", "10%", "20%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 0.75f; ModdedPlayer.Stats.meleeIncreasedDmg.Multiply(2); },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8f,
				posY = -0.75f,
				name = Translations.PerkDatabase_270,
				originalDescription = Translations.PerkDatabase_271("25%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeIncreasedDmg.valueAdditive += 3.5f; ModdedPlayer.Stats.rangedIncreasedDmg.valueMultiplicative *= 0; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 7.5f,
				posY = 0f,
				name = Translations.PerkDatabase_272,
				originalDescription = Translations.PerkDatabase_273(0,"350%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeIncreasedDmg.valueAdditive += 0.25f; ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1.25f;
					ModdedPlayer.Stats.weaponRange.Multiply(1.25f);
				},
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 79 },
				levelReq = 60,
				cost = 2,
				scale = 1,
				posX = 2f,
				posY = 2.25f,
				name = Translations.PerkDatabase_274,
				originalDescription = Translations.PerkDatabase_275("25%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.rangedIncreasedDmg.valueMultiplicative *= 2f; ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= 0.01f; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_272,
				originalDescription = Translations.PerkDatabase_276("-99%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.rangedIncreasedDmg.valueAdditive += 2.0f; ModdedPlayer.Stats.movementSpeed.valueMultiplicative *= 0.75f; ModdedPlayer.Stats.jumpPower.valueAdditive -= 0.65f; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8f,
				posY = -0.75f,
				name = Translations.PerkDatabase_277,
				originalDescription = Translations.PerkDatabase_278("200%", "25%", "35%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1.5f;
					ModdedPlayer.Stats.projectileSize.valueMultiplicative *= 1.5f;
					ModdedPlayer.Stats.projectileSpeed.valueMultiplicative *= 1.5f;
					ModdedPlayer.Stats.critDamage.Add(0.5f);
					ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 0.8f; ModdedPlayer.Stats.spellCost.Multiply(3); },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9f,
				posY = -0.75f,
				name = Translations.PerkDatabase_268,
				originalDescription = Translations.PerkDatabase_279("50%","20%", "200%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};

			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.maxEnergyMult.Multiply(0.65f); ModdedPlayer.Stats.staminaPerSecRate.Multiply(0.65f); ModdedPlayer.Stats.spellIncreasedDmg.Multiply(2); },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9f,
				posY = -0.75f,
				name = Translations.PerkDatabase_280,
				originalDescription = Translations.PerkDatabase_281("35%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 0.6f; ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.5f; },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8.5f,
				posY = 0f,
				name = Translations.PerkDatabase_282,
				originalDescription = Translations.PerkDatabase_283("50%", "40%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeFlatDmg.valueMultiplicative *= 0.5f; ModdedPlayer.Stats.rangedFlatDmg.valueMultiplicative *= 0.5f; ModdedPlayer.Stats.spellIncreasedDmg.Add(4f); },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9.5f,
				posY = 0f,
				name = Translations.PerkDatabase_284,
				originalDescription = Translations.PerkDatabase_285("400%"),
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_parryDmgBonus.valueAdditive += 1.5f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 107, 108 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_286,
				originalDescription = Translations.PerkDatabase_287(20),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDamageDebuffAmount.valueAdditive += 0.35f,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 214 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = Translations.PerkDatabase_288,
				originalDescription = Translations.PerkDatabase_289("35%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDamageDebuffAmount.valueAdditive += 0.5f,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 144 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -3f,
				name = Translations.PerkDatabase_290,
				originalDescription = Translations.PerkDatabase_291("50%"),
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_danceOfFiregod.value = true,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 71 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_292,
				originalDescription = Translations.PerkDatabase_293,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.staminaOnHit.valueAdditive += 3,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_294,
				originalDescription = Translations.PerkDatabase_295("3"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_frenzyMS.value = true,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 102 },
				levelReq = 26,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -0.75f,
				name = Translations.PerkDatabase_296,
				originalDescription = Translations.PerkDatabase_297("5%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_furySwipes.value = true,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 148 },
				levelReq = 43,
				cost = 1,
				scale = 1,
				posX = 6f,
				posY = -0.75f,
				name = Translations.PerkDatabase_298,
				originalDescription = Translations.PerkDatabase_299("10"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDamageBuff.valueAdditive++,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 103 },
				levelReq = 27,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_300,
				originalDescription = Translations.PerkDatabase_301("25%",2),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 1.25f;
					for (int i = 1; i < x; i++)
						f += 0.25f;
					return Translations.PerkDatabase_302((f - 1).ToString("P"));
				},
			};
			if (!GameSetup.IsMultiplayer)
				new Perk()
				{
					onApply = () => ModdedPlayer.Stats.MaxLogs.Add(1),
					category = PerkCategory.Utility,
					unlockPath = new int[] { 61 },
					levelReq = 20,
					cost = 1,
					scale = 1,
					posX = -11f,
					posY = -0.75f,
					name = Translations.PerkDatabase_303,
					originalDescription = Translations.PerkDatabase_304,
					textureVariation = 0,
					stackable = true,
				};
			else
				new Perk()
				{
					onApply = () => ModdedPlayer.Stats.magicFind.Add(0.15f),
					category = PerkCategory.Utility,
					unlockPath = new int[] { 110 },
					levelReq = 25,
					cost = 1,
					scale = 1f,
					posX = -0.75f,
					posY = -2.6f,
					name = Translations.PerkDatabase_305,
					originalDescription = Translations.PerkDatabase_306("15%"),
					textureVariation = 0,
					stackable = false,
				};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileDamageIncreasedBySpeed.value = true,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 14 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_307,
				originalDescription = Translations.PerkDatabase_308,
				textureVariation = 0, //0 or 1
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_magicArrowCrit.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 73 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -3f,
				name = Translations.PerkDatabase_309,
				originalDescription = Translations.PerkDatabase_310,
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_ballLightning_Crit.value = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 88 },
				levelReq = 62,
				cost = 2,
				scale = 1,
				posX = 1.5f,
				posY = 6f,
				name = Translations.PerkDatabase_311,
				originalDescription = Translations.PerkDatabase_312,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_blinkRange.valueAdditive += 10f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 105 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 3.75f,
				name = Translations.PerkDatabase_313,
				originalDescription = Translations.PerkDatabase_314("10"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireDamage.valueAdditive += 0.15f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 90 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = Translations.PerkDatabase_315,
				originalDescription = Translations.PerkDatabase_316("15%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>Translations.PerkDatabase_317( (x * 0.15f).ToString("P"))
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.spell_bia_TripleDmg.value = true; ModdedPlayer.Stats.allRecoveryMult.valueMultiplicative *= 0.5f; },
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 111 },
				levelReq = 55,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -3.75f,
				name = Translations.PerkDatabase_318,
				originalDescription = Translations.PerkDatabase_319,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bia_Stun.value = true,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 111 },
				levelReq = 30,
				cost = 2,
				scale = 1,
				posX = 1.5f,
				posY = -4.5f,
				name = Translations.PerkDatabase_320,
				originalDescription = Translations.PerkDatabase_321("5"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.energyOnHit.valueAdditive += 1f; ModdedPlayer.Stats.healthOnHit.valueAdditive += 1.5f; },
				category = PerkCategory.Support,
				unlockPath = new int[] { 44, 43 },
				levelReq = 47,
				cost = 1,
				scale = 1,
				posX = -3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_322,
				originalDescription = Translations.PerkDatabase_323(1,1.5f),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { SpellDataBase.spellDictionary[10].Cooldown -= 15; },
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 153, 75 },
				levelReq = 47,
				cost = 2,
				scale = 1,
				posX = 4.5f,
				posY = -3f,
				name = Translations.PerkDatabase_324,
				originalDescription = Translations.PerkDatabase_325(15),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.spell_parryRadius.Add(1); ModdedPlayer.Stats.perk_parryAnything.value = true; },
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 78 },
				levelReq = 34,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_326,
				originalDescription = Translations.PerkDatabase_327(1),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.thornsPerStrenght.valueAdditive += 2f,
				category = PerkCategory.Defense,
				unlockPath = new int[] { 31 },
				levelReq = 3,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_328,
				originalDescription = Translations.PerkDatabase_329(2),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.thornsDmgMult.valueMultiplicative *= 2; ModdedPlayer.Stats.armor.valueAdditive += 600; },
				category = PerkCategory.Defense,
				unlockPath = new int[] { 32 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 0f,
				name = Translations.PerkDatabase_330,
				originalDescription = Translations.PerkDatabase_331(600, "100%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.magicDamageTaken.Multiply(0.8f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 35, 32 },
				levelReq = 24,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_332,
				originalDescription = Translations.PerkDatabase_333("20%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_blackholePullImmune.value = true,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 164 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_334,
				originalDescription = Translations.PerkDatabase_335,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_blizzardSlowReduced.value = true,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 164 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -2.25f,
				name = Translations.PerkDatabase_336,
				originalDescription = Translations.PerkDatabase_337,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_trueAim.value = true,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 218 },
				unlockRequirement = new int[] { 67 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = 2.25f,
				name = Translations.PerkDatabase_338,
				originalDescription = Translations.PerkDatabase_339("60", "50%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.thorns.valueAdditive += 30,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 162 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_340,
				originalDescription = Translations.PerkDatabase_341(),
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[16].Cooldown -= 40,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 154 },
				levelReq = 80,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 6.75f,
				name = Translations.PerkDatabase_342,
				originalDescription = Translations.PerkDatabase_343(40),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[16].Cooldown -= 25,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 169 },
				levelReq = 120,
				cost = 2,
				scale = 1,
				posX = 3f,
				posY = 6.75f,
				name = Translations.PerkDatabase_344,
				originalDescription = Translations.PerkDatabase_345(25),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[3].Cooldown -= 6,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 155 },
				levelReq = 66,
				cost = 2,
				scale = 1,
				posX = 5f,
				posY = 3.75f,
				name = Translations.PerkDatabase_346,
				originalDescription = Translations.PerkDatabase_347(6),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[4].Cooldown -= 20,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 85 },
				levelReq = 44,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 3f,
				name = Translations.PerkDatabase_348,
				originalDescription = Translations.PerkDatabase_349(20),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_parryDmgBonus.valueAdditive += 2f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 143 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_350,
				originalDescription = Translations.PerkDatabase_351,
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_goldenResolve.value = true,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 87 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 4.5f,
				name = Translations.PerkDatabase_352,
				originalDescription = Translations.PerkDatabase_353("50%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[4].Cooldown -= 90,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 87 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = 4.5f,
				name = Translations.PerkDatabase_354,
				originalDescription = Translations.PerkDatabase_355(90),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_healingDomeDuration.valueAdditive += 50,
				category = PerkCategory.Support,
				unlockPath = new int[] { 113 },
				levelReq = 44,
				cost = 1,
				scale = 1,
				posX = -5.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_356,
				originalDescription = Translations.PerkDatabase_357,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { SpellDataBase.spellDictionary[2].Cooldown -= 35; SpellDataBase.spellDictionary[13].Cooldown -= 7.5f; },
				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 58,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_358,
				originalDescription = Translations.PerkDatabase_359("50%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_danceOfFiregodAtkCap.value = true,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 146 },
				levelReq = 46,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -0.75f,
				name = Translations.PerkDatabase_360,
				originalDescription = Translations.PerkDatabase_361("200%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_craftingPolishing.value = true,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 116 },
				levelReq = 35,
				cost = 0,
				scale = 1f,
				posX = -1.25f,
				posY = 1.85f,
				name = Translations.PerkDatabase_362,
				originalDescription = Translations.PerkDatabase_363,
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_craftingEmpowering.value = true,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 179, 117 },
				unlockRequirement = new int[] { 179, 117 },
				levelReq = 55,
				cost = 0,
				scale = 1f,
				posX = -0.75f,
				posY = 2.6f,
				name = Translations.PerkDatabase_364,
				originalDescription = Translations.PerkDatabase_365,
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => StatActions.AddMagicFind(0.15f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 110 },
				levelReq = 55,
				cost = 2,
				scale = 1f,
				posX = -1.75f,
				posY = -2.6f,
				name = Translations.PerkDatabase_366,
				originalDescription = Translations.PerkDatabase_367("15%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.85f; ModdedPlayer.Stats.armor.valueAdditive += 500; },
				category = PerkCategory.Defense,
				unlockPath = new int[] { 163, 34 },
				levelReq = 41,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 0.75f,
				name = Translations.PerkDatabase_368,
				originalDescription = Translations.PerkDatabase_369("500", "15%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_isShieldAutocast.value = true,
				category = PerkCategory.Defense,
				unlockPath = new int[] { 104, 34 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 5.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_370,
				originalDescription = Translations.PerkDatabase_371("90%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrowDuration.Add(5),

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 95 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = -5f,
				posY = -3.75f,
				name = Translations.PerkDatabase_372,
				originalDescription = Translations.PerkDatabase_373(5),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_ballLightning_DamageScaling.Add(45),
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 170 },
				levelReq = 130,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 7.5f,
				name = Translations.PerkDatabase_374,
				originalDescription = Translations.PerkDatabase_375("4500%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					MoreCraftingReceipes.SetCustomReceipeUnlockState(MoreCraftingReceipes.CustomReceipe.PlaneAxe, true);
					MoreCraftingReceipes.AddReceipes();
				},
				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_376,
				originalDescription = Translations.PerkDatabase_377(1,1,2),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.headShotDamage.Add(4f),
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 66 },
				levelReq = 38,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -3f,
				name = Translations.PerkDatabase_378,
				originalDescription = Translations.PerkDatabase_379,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.explosionDamage.Add(0.5f),
				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_380,
				originalDescription = Translations.PerkDatabase_381("50%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>Translations.PerkDatabase_382((x * 0.5f).ToString("P"))
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.rangedIncreasedDmg.Multiply(1.1f);
					ModdedPlayer.Stats.headShotDamage.Add(3);
					ModdedPlayer.Stats.critDamage.Add(1f);
				},
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 67 },
				unlockRequirement = new int[] { 67, 66, 65, 64 },
				levelReq = 65,
				cost = 2,
				scale = 1,
				posX = -0.5f,
				posY = -4.5f,
				name = Translations.PerkDatabase_383,
				originalDescription = Translations.PerkDatabase_384("10%", "300%", "100%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_doubleStickHarvesting.value = true,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 56, 57 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -8f,
				posY = -0.75f,
				name = Translations.PerkDatabase_385,
				originalDescription = Translations.PerkDatabase_386,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.block.Add(20f),
				category = PerkCategory.Defense,
				unlockPath = new int[] { 29, 35 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_387,
				originalDescription = Translations.PerkDatabase_388(20),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.thornsPerVit.Add(1.5f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 168 },
				levelReq = 16,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 2.25f,
				name = Translations.PerkDatabase_389,
				originalDescription = Translations.PerkDatabase_390("150%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.thornsArmorPiercing.Add(2.5f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 192 },
				levelReq = 17,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 3f,
				name = Translations.PerkDatabase_391,
				originalDescription = Translations.PerkDatabase_392("250%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireTickRate.Add(0.20f),

				category = PerkCategory.Support,
				unlockPath = new int[] { 156 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_393,
				originalDescription = Translations.PerkDatabase_394("20%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x => Translations.PerkDatabase_382( (x * 0.20f).ToString("P"))
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireTickRate.Add(1.2f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 194 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_395,
				originalDescription = Translations.PerkDatabase_396("120%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireDuration.Add(0.20f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 156 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_397,
				originalDescription = Translations.PerkDatabase_398("20%"),
				textureVariation = 0,
				stackable = true,
				updateDescription = x => Translations.PerkDatabase_382( (x * 0.2f).ToString("P"))
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireDuration.Add(1f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 196 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_399,
				originalDescription = Translations.PerkDatabase_400("100%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.expGain.Add(0.20f),

				category = PerkCategory.Support,
				unlockPath = new int[] { 42 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = -5.5f,
				posY = 0f,
				name = Translations.PerkDatabase_401,
				originalDescription = Translations.PerkDatabase_402("20%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellFlatDmg.Add(2022),

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 89 },
				levelReq = 122,
				cost = 2,
				scale = 1,
				posX = -0.5f,
				posY = 6f,
				name = Translations.PerkDatabase_403,
				originalDescription = Translations.PerkDatabase_404(2022),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_blackhole_radius.Add(15),
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 199 },
				levelReq = 128,
				cost = 2,
				scale = 1,
				posX = -1.5f,
				posY = 6f,
				name = Translations.PerkDatabase_405,
				originalDescription = Translations.PerkDatabase_406(15),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_blackhole_radius.Add(2.5f),
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 200 },
				levelReq = 132,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 6f,
				name = Translations.PerkDatabase_407,
				originalDescription = Translations.PerkDatabase_408("2.5"),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_blackhole_pullforce.Add(25),
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 200 },
				levelReq = 130,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = 5.25f,
				name = Translations.PerkDatabase_407,
				originalDescription = Translations.PerkDatabase_409(25),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.vitality.Add(30),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 114 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -1.5f,
				name = Translations.MainMenu_Guide_8,
				originalDescription = Translations.PerkDatabase_410(30),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.thornsDmgMult.Multiply(3f);
					ModdedPlayer.Stats.thornsArmorPiercing.Add(3f);
					ModdedPlayer.Stats.allDamageTaken.Multiply(0.95f);
					ModdedPlayer.Stats.meleeFlatDmg.Multiply(0.8f);
					ModdedPlayer.Stats.rangedFlatDmg.Multiply(0.8f);
				},
				category = PerkCategory.Defense,
				unlockPath = new int[] { 89 },
				unlockRequirement = new int[] { 193 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 4f,
				posY = 3.75f,
				name = Translations.PerkDatabase_411,
				originalDescription = Translations.PerkDatabase_412("20%"),
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allRecoveryMult.Multiply(1.3f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 159 },
				levelReq = 76,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -2.25f,
				name = Translations.PerkDatabase_413,
				originalDescription = Translations.PerkDatabase_414("30%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_taunt_speedChange.Multiply(0.5f * 0.4f),

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 15 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_415,
				originalDescription = Translations.PerkDatabase_416("60%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_taunt_pullEnemiesIn.value = true,
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 206 },
				levelReq = 60,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 0f,
				name = Translations.PerkDatabase_417,
				originalDescription = Translations.PerkDatabase_418,
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_chargedAtkKnockback.value = true,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 77 },
				levelReq = 37,
				cost = 1,
				scale = 1,
				posX = 5.5f,
				posY = 0f,
				name = Translations.PerkDatabase_419,
				originalDescription = Translations.PerkDatabase_420,
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_parryAttackSpeed.Add(2.0f) ,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 78 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_421,
				originalDescription = Translations.PerkDatabase_422("200%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_craftingRerollingSingleStat.value = true,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 179 },
				levelReq = 45,
				cost = 0,
				scale = 1f,
				posX = -1.75f,
				posY = 2.6f,
				name = Translations.PerkDatabase_423,
				originalDescription = Translations.PerkDatabase_424,
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive += 20f;
					ModdedPlayer.Stats.spell_fireboltDamageScaling.valueAdditive += 0.3f;
				},
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 2.25f,
				name = Translations.PerkDatabase_425,
				originalDescription = Translations.PerkDatabase_426("30%", "20"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive += 120;
					ModdedPlayer.Stats.spell_fireboltDamageScaling.valueAdditive += 0.45f;
				},
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 211 },
				levelReq = 17,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 2.25f,
				name = Translations.PerkDatabase_425,
				originalDescription = Translations.PerkDatabase_426("45%", "120"),
				updateDescription = x => Translations.PerkDatabase_428(ModdedPlayer.Stats.spell_fireboltDamageScaling.valueAdditive.ToString("P"), ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive.ToString("N")),
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.heavyAttackDmg.Add(10),

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 77 },
				levelReq = 70,
				cost = 2,
				scale = 1,
				posX = 5f,
				posY = 0.75f,
				name = Translations.PerkDatabase_429,
				originalDescription = Translations.PerkDatabase_430("1000%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.strength.Add(300),
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_431,
				originalDescription = Translations.PerkDatabase_432(300),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.meleeIncreasedDmg.Multiply(1.25f),
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 214 },
				levelReq = 60,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_433,
				originalDescription = Translations.PerkDatabase_434("25%"),
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.spell_fireboltEnergyCost.Substract(13);
				},
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 212 },
				levelReq = 65,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 2.25f,
				name = Translations.PerkDatabase_435,
				originalDescription = Translations.PerkDatabase_436(13,15),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.rangedIncreasedDmg.Add(0.08f),
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 16,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_437,
				originalDescription = Translations.PerkDatabase_438("8%"),
				textureVariation = 0, //0 or 1
				stackable = false,

			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.critDamage.Add(0.4f),
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 217 },
				levelReq = 36,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = 2.25f,
				name = Translations.PerkDatabase_439,
				originalDescription = Translations.PerkDatabase_440("40%"),
				textureVariation = 0, //0 or 1
				stackable = false,

			};
			new Perk()
			{
				onApply = () => COTFEvents.Instance.OnDodge.AddListener(() => TheForest.Utils.LocalPlayer.Stats.Health += ModdedPlayer.Stats.spell_shieldMax * 0.05f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 36 },
				levelReq = 60,
				cost = 1,
				scale = 1,
				posX = 6f,
				posY = 2.25f,
				name = Translations.PerkDatabase_441,
				originalDescription = Translations.PerkDatabase_442("5%"),
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bia_Crit.value = true,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 159 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -4.5f,
				name = "",
				originalDescription = Translations.PerkDatabase_444,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.projectileSpeed.Multiply(2),
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 159 },
				levelReq = 90,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -7.25f,
				name = Translations.PerkDatabase_445, //tr
				originalDescription = Translations.PerkDatabase_446,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => COTFEvents.Instance.OnDodge.AddListener(() => ModdedPlayer.instance.damageAbsorbAmounts[0] += ModdedPlayer.Stats.TotalMaxHealth* 0.5f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 36 },
				levelReq = 100,
				cost = 1,
				scale = 1,
				posX = 7f,
				posY = 2.25f,
				name = Translations.PerkDatabase_447,//tr
				originalDescription = Translations.PerkDatabase_448("50%"),//tr
				textureVariation = 0,
				stackable = false,
			};

			foreach (var item in perks)
			{
				item.Description = item.originalDescription;
			}
		}
	}
}