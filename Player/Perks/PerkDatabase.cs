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
				name = Translations.PerkDatabase_1/*og:Stronger Hits*/,//tr
				originalDescription = Translations.PerkDatabase_2 + "1%"/*og:Gene allows muscles to quickly change their structure to a more efficient one.\nEvery point of STRENGTH increases MEELE DAMAGE by 1%.*/,//tr
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
				name = Translations.PerkDatabase_3/*og:Stronger Spells*/,//tr
				originalDescription = Translations.PerkDatabase_4 + "1%"/*og:Gene changes the composition of axon sheath that greatly increases brain's power.\nEvery point of INTELLIGENCE increases SPELL DAMAGE by 1%.*/,//tr
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
				name = Translations.PerkDatabase_5/*og:Stronger Projectiles*/,//tr
				originalDescription = Translations.PerkDatabase_6 + "1%"/*og:Neural connections between muscles and the brain are now a lot more sensitive. Your movements become a lot more precise.\nEvery point of AGILITY increases RANGED DAMAGE by 1%.*/,//tr
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
				name = Translations.PerkDatabase_7/*og:Inner Energy*/,//tr
				originalDescription = Translations.PerkDatabase_8 + "1%"/*og:Heart's muscles become even more resistant to exhaustion.\nEvery point of intelligence increases stamina and energy recovery rate by */,//tr
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
				name = Translations.PerkDatabase_9/*og:More Stamina*/,//tr
				originalDescription = Translations.PerkDatabase_10/*og:Hemoglobin is replaced with an alternative substance capable of carrying more oxygen.\nEvery point of AGILITY increases max stamina by 0.5*/,//tr
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
				name = Translations.PerkDatabase_11/*og:More Health*/,//tr
				originalDescription = Translations.PerkDatabase_12 + "2"/*og:Skin and bones become more resistant to injuries.\nEvery point of VITALITY increases max health by */,//tr
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
				name = Translations.PerkDatabase_13/*og:More Healing*/,//tr
				originalDescription = Translations.PerkDatabase_14 + " 10%"/*og:Blood becomes denser and binds more oxygen, is less vulnerable to bleeding and wounds are healed faster. Stamina and energy recover faster.\nIncreases all healing and recovery by 10%*/,//tr
				updateDescription = x =>
				{
					float f = 0.1f * x;
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (f).ToString("P"); //tr
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
				name = Translations.PerkDatabase_16/*og:Metabolism*/,//tr
				originalDescription = Translations.PerkDatabase_17 + "30%"/*og:Additional microorganisms are now present in the digestive system. Sweating is decreased\nDecreases hunger and thirst rate by 30%.*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.7f;
					for (int i = 1; i < x; i++)
						f *= 0.7f;
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (1 - f).ToString("P"); //tr
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
				name = Translations.PerkDatabase_18/*og:Breath under control*/,//tr
				originalDescription = string.Format(Translations.PerkDatabase_19, 0.5)/*og:Recovers 0.5 more stamina per second. Stamina is used for sprinting and swinging weapons.*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x => Translations.PerkDatabase_21/*og:Total: */ + (0.5f * x).ToString(Translations.PerkDatabase_20/*og:N1*/) //tr
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
				name = Translations.PerkDatabase_22/*og:Grip Strength*/,//tr
				originalDescription = Translations.PerkDatabase_23 + "5"/*og:Grip strength increases.\nIncreases melee damage by 5*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + x * 5; //tr
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
				name = Translations.PerkDatabase_24/*og:Arm Strength*/,//tr
				originalDescription = Translations.PerkDatabase_25 + "10%"/*og:Biceps slightly increases in size.\nIncreases melee damage by 10%*/,//tr
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
				name = Translations.PerkDatabase_26/*og:Body Strength*/,//tr
				originalDescription = Translations.PerkDatabase_27 + "20"/*og:All flexors gain in size.\nIncreases strength by 20*/,//tr
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
				name = Translations.PerkDatabase_28/*og:Projectile Damage*/,//tr
				originalDescription = Translations.PerkDatabase_29 + "8"/*og:Shoulder muscles grow.\nIncreases projectile damage by */,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + x * 8; //tr
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
				name = Translations.PerkDatabase_30/*og:Large caliber*/,//tr
				originalDescription = Translations.PerkDatabase_31/*og:Bigger is better.\nIncreases projectile size by 5%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (x * 0.05f).ToString("P"); //tr
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
				name = Translations.PerkDatabase_32/*og:Speed*/,//tr
				originalDescription = Translations.PerkDatabase_33/*og:Increased overall physical strength allows for stronger drawing of ranged weaponry.\nIncreases projectile speed by 5%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (x * 0.05f).ToString("P"); //tr
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
				name = Translations.PerkDatabase_34/*og:Transmutation*/,//tr
				originalDescription = Translations.PerkDatabase_35/*Spells become easier to  recover from.\n15% of the spell cost is now taxed from stamina instead of energy.*/,//tr
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
				name = Translations.PerkDatabase_36/*og:Resource Cost Reduction*/,//tr
				originalDescription = Translations.PerkDatabase_37 + "9%"/*og:In order to preserve energy, spell costs are reduced by 4%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.91f;
					for (int i = 1; i < x; i++)
						f *= 0.91f;
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (1 - f).ToString("P"); //tr
				},
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.70f; ModdedPlayer.Stats.allDamage.valueMultiplicative *= 0.70f;				ModdedPlayer.Stats.thornsDmgMult.Multiply(1.69f);
				},

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { 5 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = Translations.PerkDatabase_38/*og:Indestructable*/,//tr
				originalDescription = Translations.PerkDatabase_39/*og:Decreases all damage taken and decreases damage dealt by 30%. Thorns damage is increased by 69%*/,//tr
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
				name = Translations.PerkDatabase_40/*og:Cooldown Reduction*/,//tr
				originalDescription = Translations.PerkDatabase_41 + "8%"/*og: Reduces spell cooldown by 8%*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.92f;
					for (int i = 1; i < x; i++)
						f *= 0.92f;
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (1 - f).ToString("P"); //tr
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
				name = Translations.PerkDatabase_42/*og:Greater Cooldown Reduction*/,//tr
				originalDescription = Translations.PerkDatabase_43 + "15%"/*og: Reduces spell cooldown by 15%*/,//tr
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
				name = Translations.PerkDatabase_44/*og:All attributes*/,//tr
				originalDescription = "+5 "+Translations.PerkDatabase_45/*og:+5 to every strength, agility, vitality and intelligence*/,//tr
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
				name = Translations.PerkDatabase_44/*og:All attributes*/,//tr
				originalDescription = "+20 "+Translations.PerkDatabase_46/*og:+20 to every strength, agility, vitality and intelligence*/,//tr
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
				name = Translations.MainMenu_Guide_49/*og:Attack speed*/,//tr
				originalDescription = "+5% "+Translations.PerkDatabase_47/*og:+4% to attack speed */,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (0.04f * x).ToString("P"); //tr
				},
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileNoConsumeChance.valueAdditive += 0.03f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12, 14 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_48/*og:Reusability I*/,//tr
				originalDescription = Translations.PerkDatabase_49/*og:+3% chance to not consume ammo while firing.*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (0.03f * x).ToString("P"); //tr
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
				name = Translations.PerkDatabase_50/*og:Reusability II*/,//tr
				originalDescription = Translations.PerkDatabase_51/*og:+13% chance to not consume ammo while firing.*/,//tr
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
				name = Translations.PerkDatabase_52/*og:Reusability III*/,//tr
				originalDescription = Translations.PerkDatabase_51/*og:+13% chance to not consume ammo while firing.*/,//tr
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
				name = Translations.PerkDatabase_44/*og:All attributes*/,//tr
				originalDescription = Translations.PerkDatabase_53/*og:+10 to strength, agility, vitality and intelligence*/,//tr
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
				name = Translations.PerkDatabase_54/*og:Jump*/,//tr
				originalDescription = Translations.PerkDatabase_55 + "15%"/*og:Increases jump height by 15%*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (0.15f * x).ToString("P"); //tr
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
				name = Translations.PerkDatabase_56/*og:Light foot*/,//tr
				originalDescription = Translations.PerkDatabase_57 + " 10%"/*og:Increases movement speed by 10%*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (0.1f * x).ToString("P"); //tr
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
				name = Translations.PerkDatabase_58/*og:Bonus Health*/,//tr
				originalDescription = Translations.PerkDatabase_59 +" 35"/*og:Increases health by 35.*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (35 * x).ToString("N"); //tr
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
				name = Translations.PerkDatabase_60/*og:Health Regen*/,//tr
				originalDescription = Translations.PerkDatabase_61/*og:Increases health per second regeneration by 0.25 HP/second. This is further multiplied by health regeneration percent and all healing percent.*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (0.25f * x).ToString(Translations.PerkDatabase_62/*og:N2*/); //tr
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
				name = Translations.PerkDatabase_63/*og:Bonus Armor*/,//tr
				originalDescription = Translations.PerkDatabase_64 + " 80"/*og:Increases armor by 80.*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (80 * x).ToString("N"); //tr
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
				name = Translations.PerkDatabase_65/*og:Durability*/,//tr
				originalDescription = Translations.PerkDatabase_66 + " 10%"/*og:Decreases all damage taken by 10%.*/,//tr
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
				name = Translations.PerkDatabase_67/*og:Durability II*/,//tr
				originalDescription = Translations.PerkDatabase_68 + " 15%"/*og:Further decreases all damage taken by 15%.*/,//tr
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
				name = Translations.PerkDatabase_69/*og:Durability III*/,//tr
				originalDescription = Translations.PerkDatabase_68 + " 20%"/*og:Further decreases all damage taken by 20%.*/,//tr
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
				name = Translations.MainMenu_Guide_19/*og:Magic Resistance*/,//tr
				originalDescription = Translations.PerkDatabase_70+" 15%"/*og:Decreases magic damage taken by 15%*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 0.85f;
					for (int i = 1; i < x; i++)
						f *= 0.85f;
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + (1 - f).ToString("P"); //tr
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
				name = Translations.PerkDatabase_71/*og:Dodge*/,//tr
				originalDescription = string.Format(Translations.PerkDatabase_72, "25%")/*og:Increases dodge chance by 25%*/,//tr
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
				name = Translations.PerkDatabase_73/*og:Armor Penetration*/,//tr
				originalDescription = Translations.PerkDatabase_74 + " 5"/*og:Increases armor penetration from all sources by 5*/,//tr
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
				name = Translations.PerkDatabase_75/*og:Armor Piercing Edge*/,//tr
				originalDescription = Translations.PerkDatabase_76/*og:Increases armor penetration from melee weapons by 12*/,//tr
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
				name = Translations.PerkDatabase_77/*og:Anti armor projectiles*/,//tr
				originalDescription = Translations.PerkDatabase_78 + " 6"/*og:Increases armor penetration from ranged weapons by 6*/,//tr
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
				name = Translations.PerkDatabase_79/*og:More Health Regen*/,//tr
				originalDescription = Translations.PerkDatabase_80/*og:Passive health regeneration is increased by 10%*/,//tr
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
				name = Translations.PerkDatabase_81/*og:Energy generation*/,//tr
				originalDescription = Translations.PerkDatabase_82/*og:Passive energy regeneration is increased by 0.15/s*/,//tr
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
				name = Translations.PerkDatabase_83/*og:Insight*/,//tr
				originalDescription = Translations.PerkDatabase_84/*og:All experience gained increased by 10%*/,//tr
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
				name = Translations.PerkDatabase_85/*og:Combat Health Regen*/,//tr
				originalDescription = Translations.PerkDatabase_86/*og:Life on hit increased by 1*/,//tr
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
				name = Translations.PerkDatabase_87/*og:Combat Energy Regen*/,//tr
				originalDescription = Translations.PerkDatabase_88/*og:Energy on hit increased by 0.5*/,//tr
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
				name = Translations.PerkDatabase_89/*og:Alternative cloth sources*/,//tr
				originalDescription = Translations.PerkDatabase_90/*og:Increases daily generation of cloth by 10. Allows turning animal fur skin into cloth by crafting. Place multiple of the same fur on the mat to craft.*/,//tr
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
				name = Translations.PerkDatabase_91/*og:Demolition*/,//tr
				originalDescription = Translations.PerkDatabase_92/*og:Increases daily generation of bombs by 2. If it exceeds your max amount of bombs carried, excess will be lost.*/,//tr
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.instance.AddExtraItemCapacity(29, 10); ModdedPlayer.instance.AddExtraItemCapacity(175, 15); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 46 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_93/*og:Pockets for explosives*/,//tr
				originalDescription = Translations.PerkDatabase_94/*og:Increases max amount of carried bombs and dynamite by 30*/,//tr
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
				name = Translations.PerkDatabase_95/*og:Demolition Expert*/,//tr
				originalDescription = Translations.PerkDatabase_96/*og:Increases daily generation of dynamite by 5. If it exceeds your max amount of bombs carried, excess will be lost.*/,//tr
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
				name = Translations.PerkDatabase_97/*og:Meds*/,//tr
				originalDescription = Translations.PerkDatabase_98/*og:Increases daily generation of meds by 3. If it exceeds your max amount of bombs carried, excess will be lost.*/,//tr
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
				name = Translations.PerkDatabase_99/*og:Fuel*/,//tr
				originalDescription = Translations.PerkDatabase_100/*og:Increases daily generation of fuel cans by 3. If it exceeds your max amount of bombs carried, excess will be lost.*/,//tr
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
				name = Translations.PerkDatabase_101/*og:Booze*/,//tr
				originalDescription = Translations.PerkDatabase_102/*og:Increases daily generation of booze by 5. If it exceeds your max amount of bombs carried, excess will be lost.*/,//tr
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
				name = Translations.PerkDatabase_103/*og:More Booze*/,//tr
				originalDescription = Translations.PerkDatabase_104/*og:Increases max amount of carried booze by 25*/,//tr
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
				name = Translations.PerkDatabase_105/*og:More Meds*/,//tr
				originalDescription = Translations.PerkDatabase_106/*og:Increases max amount of carried meds by 20*/,//tr
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_projectileNoConsumeChance.valueAdditive += 0.4f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 25 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = -3f,
				name = Translations.PerkDatabase_107/*og:Endless Quiver*/,//tr
				originalDescription = Translations.PerkDatabase_108/*og:Gives 40% chance to not consume ammo when firing a projectile*/,//tr
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
				name = Translations.PerkDatabase_109/*og:Spell Power*/,//tr
				originalDescription = Translations.PerkDatabase_110/*og:Increases spell damage by 5*/,//tr
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
				name = Translations.PerkDatabase_111/*og:More Meat*/,//tr
				originalDescription = Translations.PerkDatabase_112/*og:Increases carry amount of all meats by 5*/,//tr
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
				name = Translations.PerkDatabase_113/*og:More Snacks*/,//tr
				originalDescription = Translations.PerkDatabase_114/*og:Increases carry amount of candy bars and sodas by 20*/,//tr
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
				name = Translations.PerkDatabase_115/*og:More Bolts*/,//tr
				originalDescription = Translations.PerkDatabase_116/*og:Increases carry amount of crossbow bolts by 20*/,//tr
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
				name = Translations.PerkDatabase_117/*og:Corpse collecting*/,//tr
				originalDescription = Translations.PerkDatabase_118/*og:Increases carry amount of bones by 100 and skulls by 20*/,//tr
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
				name = Translations.PerkDatabase_119/*og:More Limbs*/,//tr
				originalDescription = Translations.PerkDatabase_120/*og:Increases carry amount of arms, legs, heads and headbombs by 10*/,//tr
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.instance.AddExtraItemCapacity(57, 6); ModdedPlayer.instance.AddExtraItemCapacity(53, 2); ModdedPlayer.instance.AddExtraItemCapacity(54, 1); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -10.5f,
				posY = 0,
				name = Translations.PerkDatabase_121/*og:More Building Resources*/,//tr
				originalDescription = Translations.PerkDatabase_122/*og:Increases carry amount of sticks by 6, rocks by 2 and ropes by 1*/,//tr
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
				name = Translations.PerkDatabase_123/*og:More Miscellaneous Items*/,//tr
				originalDescription = Translations.PerkDatabase_124/*og:Increases carry amount of pots, turtle shells, watches, circuit boards, air canisters and flares by 5*/,//tr
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
				name = Translations.PerkDatabase_125/*og:More Ammo*/,//tr
				originalDescription = Translations.PerkDatabase_126/*og:Increases carry amount of weak and upgraded spears and molotovs by 5, small rocks by 50. Allows you to craft flint lock ammo (15 coins + 1 rock), crossbow bolts (3 rocks + 3 sticks), and modern arrows (arrows + coins)*/,//tr
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
				name = Translations.PerkDatabase_127/*og:Spear Specialization*/,//tr
				originalDescription = Translations.PerkDatabase_128/*og:Thrown spears deal 100% more damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bulletDamageMult.valueMultiplicative *= 1.6f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = Translations.PerkDatabase_129/*og:Pistol Specialization*/,//tr
				originalDescription = Translations.PerkDatabase_130/*og:Bullets deal 60% more damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_crossbowDamageMult.valueMultiplicative *= 1.8f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -3f,
				name = Translations.PerkDatabase_131/*og:Crossbow Specialization*/,//tr
				originalDescription = Translations.PerkDatabase_132/*og:Bolts deal 80% more damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_bowDamageMult.valueMultiplicative *= 1.4f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -3.75f,
				name = Translations.PerkDatabase_133/*og:Bow Specialization*/,//tr
				originalDescription = Translations.PerkDatabase_134/*og:Arrows deal 40% more damage*/,//tr
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
				name = Translations.PerkDatabase_135/*og:Sanctuary*/,//tr
				originalDescription = Translations.PerkDatabase_136/*og:Healing dome provides immunity to stun and root effects*/,//tr
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
				name = Translations.PerkDatabase_137/*og:Enchant weapon*/,//tr
				originalDescription = Translations.PerkDatabase_138/*og:While black flame is on, all damage is increased by 40%*/,//tr
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
				name = Translations.PerkDatabase_139/*og:Empowered War Cry*/,//tr
				originalDescription = Translations.PerkDatabase_140/*og:Warcry additionally increases all damage dealt*/,//tr
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
				name = Translations.PerkDatabase_141/*og:Power Swing*/,//tr
				originalDescription = Translations.PerkDatabase_142/*og:Attacks use 40% more stamina and deal 25% more damage*/,//tr
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
					return Translations.PerkDatabase_15/*og:\nTotal from this perk: */ + Translations.PerkDatabase_144/*og:\nDamage - */ + (f - 1).ToString("P") + Translations.PerkDatabase_143/*og:\nStamina Cost - */ + (f1 - 1).ToString("P"); //tr
				},
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.spellIncreasedDmg.valueAdditive += 0.15f; ModdedPlayer.Stats.spellCost.valueMultiplicative *= 1.20f; },

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 15, 55 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_145/*og:Overcharge*/,//tr
				originalDescription = Translations.PerkDatabase_146/*og:Spell damage is increased by 15%, spell costs are increased by 20%*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				 {
					 float f1 = 0.15f * x;
					 float f2 = Mathf.Pow(1.2f, x);
					 return string.Format(Translations.PerkDatabase_148/*og:Total increase to spell damage: {0}\nspell cost increase: {1}*/, f1.ToString(Translations.PerkDatabase_147/*og:P1*/), (f2 - 1f).ToString(Translations.PerkDatabase_147/*og:P1*/)); //tr
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
				name = Translations.PerkDatabase_149/*og:Exposure*/,//tr
				originalDescription = Translations.PerkDatabase_150/*og:Magic arrow causes hit enemies to take 40% more damage for the duration of the slow.*/,//tr
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
				name = Translations.PerkDatabase_151/*og:Disabler*/,//tr
				originalDescription = Translations.PerkDatabase_152/*og:Magic arrow's negative effects last additional 5 seconds*/,//tr
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
				name = Translations.PerkDatabase_153/*og:Magic Binding*/,//tr
				originalDescription = Translations.PerkDatabase_154/*og:Magic arrow's slow amount is doubled. It's upgraded from 45% to 90%*/,//tr
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.heavyAttackDmg.valueMultiplicative *= 1.5f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 10 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_155/*og:Charged Attack*/,//tr
				originalDescription = Translations.PerkDatabase_156/*og:Charged melee attacks deal additional 50%*/,//tr
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
				name = Translations.PerkDatabase_157/*og:Super Charged Attack*/,//tr
				originalDescription = Translations.PerkDatabase_158/*og:Charged melee attacks deal 300% more damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.critDamage.valueAdditive += 0.10f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9, 10 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_159/*og:Lucky Hits*/,//tr
				originalDescription = Translations.PerkDatabase_160/*og:Increases Critical hit damage by 10%*/,//tr
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
				name = Translations.PerkDatabase_161/*og:Overwhelming Odds*/,//tr
				originalDescription = Translations.PerkDatabase_162/*og:Increases Critical chance by 10%.*/,//tr
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
				name = Translations.PerkDatabase_163/*og:Endurance*/,//tr
				originalDescription = Translations.PerkDatabase_164/*og:Increases maximum energy by 15*/,//tr
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
				name = Translations.PerkDatabase_165/*og:Multishot Empower*/,//tr
				originalDescription = Translations.PerkDatabase_166/*og:Increases the projectile count of multishot by 2, also increases the spells cost. Multishot does not apply to spears.*/,//tr
				updateDescription = x =>string.Format(Translations.PerkDatabase_167/*og:\nMultishot cost now: {0}\nCost after upgrading: {1}*/, (10 * Mathf.Pow(2 * x, 1.75f)).ToString("N"), (10 * Mathf.Pow((2 + 2 * x), 1.75f)).ToString("N")),//tr
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
				name = Translations.PerkDatabase_168/*og:Transporter*/,//tr
				originalDescription = Translations.PerkDatabase_169/*og:Allows you to use raft on land, turning it into a wooden hovercraft.*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_RaftSpeedMultipier.Add(1),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 82 },
				levelReq = 23,
				cost = 1,
				scale = 1,
				posX = -7.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_170/*og:Turbo*/,//tr
				originalDescription = Translations.PerkDatabase_171/*og:Hovercraft but faster!\nIncreases the speed of rafts by 100%.*/,//tr
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
				name = Translations.PerkDatabase_172/*og:Closing Wounds*/,//tr
				originalDescription = Translations.PerkDatabase_173/*og:Purge now heals all players for percent of their missing health and restores energy for percent of missing energy.*/,//tr
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
				name = Translations.PerkDatabase_42/*og:Greater Cool Down Reduction*/,//tr
				originalDescription = Translations.PerkDatabase_43/*og: Reduces spell cooldown by 7,5%*/,//tr
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
				name = Translations.PerkDatabase_42/*og:Greater Cool Down Reduction*/,//tr
				originalDescription = Translations.PerkDatabase_174/*og: Reduces spell cooldown by 10%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.9f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 86 },
				levelReq = 46,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 4.5f,
				name = Translations.PerkDatabase_42/*og:Greater Cool Down Reduction*/,//tr
				originalDescription = Translations.PerkDatabase_174/*og: Reduces spell cooldown by 10%*/,//tr
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
				name = Translations.PerkDatabase_175/*og:Infinity*/,//tr
				originalDescription = Translations.PerkDatabase_176/*og:Every time you cast a spell, all cooldowns are reduced by 5%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellIncreasedDmg.valueAdditive *= 1.5f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 88 },
				levelReq = 61,
				cost = 2,
				scale = 1,
				posX = 0.5f,
				posY = 6f,
				name = Translations.PerkDatabase_177/*og:Armageddon*/,//tr
				originalDescription = Translations.PerkDatabase_178/*og:Spell damage increased by 50%*/,//tr
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
				name = Translations.PerkDatabase_179/*og:Inner Fire*/,//tr
				originalDescription = Translations.PerkDatabase_180/*og:Upon hitting an enemy, leave a debuff for 4 seconds, increase fire damage against that enemy equal to your spell amplification*/,//tr
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
				name = Translations.PerkDatabase_181/*og:Near Death Experience*/,//tr
				originalDescription = Translations.PerkDatabase_182/*og:Upon receiving fatal damage, instead of dieing restore your health to 100% and gain 5 seconds of immunity to debuffs. This may occur once every 10 minutes*/,//tr
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
				name = Translations.PerkDatabase_183/*og:Seeking Arrow - Head Hunting*/,//tr
				originalDescription = Translations.PerkDatabase_184/*og:Seeking arrow headshot penalty is removed*/,//tr
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
				name = Translations.PerkDatabase_185/*og:Sniper*/,//tr
				originalDescription = Translations.PerkDatabase_186/*og:Projectiles deal additional 1% damage for every meter they travelled. */,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrow_SlowDuration.valueAdditive += 3,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 92 },
				levelReq = 41,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = -3.75f,
				name = Translations.PerkDatabase_187/*og:Seeking Arrow - Crippling precision*/,//tr
				originalDescription = Translations.PerkDatabase_188/*og:Seeking arrow slow duration is increased by 3 additional seconds*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrow_SlowAmount.valueAdditive -= 0.2f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 94 },
				levelReq = 42,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -3.75f,
				name = Translations.PerkDatabase_189/*og:Seeking Arrow - Movement Impairing Arrows*/,//tr
				originalDescription = Translations.PerkDatabase_190/*og:Seeking arrow slow amount is increased from 10% to 30%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusOnHS.valueAdditive += 0.3f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 217, 13 },
				levelReq = 14,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_191/*og:Focus - Perfection*/,//tr
				originalDescription = Translations.PerkDatabase_192/*og:Focus damage bonus on headshot is increased from 50% to 80%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusOnAtkSpeed.valueAdditive += 0.15f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 96 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_193/*og:Focus - Quickscope*/,//tr
				originalDescription = Translations.PerkDatabase_194/*og:Focus extra attack on bodyshot is increased from 30% to 45%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusOnAtkSpeed.valueAdditive += 0.15f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 97 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_195/*og:Focus - Quickerscope*/,//tr
				originalDescription = Translations.PerkDatabase_196/*og:Focus extra attack speed on bodyshot is increased from 45% to 60%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_focusSlowDuration.valueAdditive += 20f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 98 },
				levelReq = 35,
				cost = 2,
				scale = 1,
				posX = 5.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_197/*og:Focus - Cripple*/,//tr
				originalDescription = Translations.PerkDatabase_198/*og:Focus Slow lasts additional 20 seconds*/,//tr
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
				name = Translations.PerkDatabase_199/*og:Afterburn*/,//tr
				originalDescription = Translations.PerkDatabase_200/*og:Black flames have a 10% chance to apply a weakening effect on enemies, making them take 15% more damage for 25 seconds*/,//tr
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
				name = Translations.PerkDatabase_201/*og:Netherflame*/,//tr
				originalDescription = Translations.PerkDatabase_202/*og:Black flames have double damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_frenzyAtkSpeed.valueAdditive += 0.05f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 72 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_203/*og:Frenzy - Haste*/,//tr
				originalDescription = Translations.PerkDatabase_204/*og:Every stack of frenzy increases attack speed by 5%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDuration.valueAdditive += 1,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 72 },
				levelReq = 26,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_205/*og:Concussive Bash*/,//tr
				originalDescription = Translations.PerkDatabase_206/*og:Bash active and passive effect duration is increased by 1 seconds.\nIf bash applies bleed, bleeding deals overall more damage*/,//tr
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_shieldPersistanceLifetime.valueAdditive += 60f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_207/*og:Shield - Endurance*/,//tr
				originalDescription = Translations.PerkDatabase_208/*og:Shield doesn't decay for 1 minute longer.*/,//tr
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_blinkDamage.Add(14f),

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 86 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 3.75f,
				name = Translations.PerkDatabase_209/*og:Blink - Passthrough*/,//tr
				originalDescription = Translations.PerkDatabase_210/*og:Blink now deals damage to enemies that you teleport through*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.energyRecoveryFromInt.valueAdditive += 0.005f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 21 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_211/*og:Inner Energy II*/,//tr
				originalDescription = Translations.PerkDatabase_212/*og:Every point of intelligence further increases stamina and energy recovery by 0.5%.*/,//tr
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
				name = Translations.PerkDatabase_213/*og:Flame Guard*/,//tr
				originalDescription = Translations.PerkDatabase_214/*og:Parry ignites enemies*/,//tr
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
				name = Translations.PerkDatabase_215/*og:Parry range*/,//tr
				originalDescription = Translations.PerkDatabase_216/*og:Increases the radius of parry by 1m*/,//tr
				textureVariation = 0,
				stackable = true,
			};
			new Perk()
			{
				onApply = () => StatActions.AddMagicFind(0.1f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { -1 },
				levelReq = 5,
				cost = 1,
				scale = 1f,
				posX = -0.75f,
				posY = -1.1f,
				name = Translations.PerkDatabase_217/*og:Luck Enchantment*/,//tr
				originalDescription = Translations.PerkDatabase_218/*og:Increases magic find by 10%. Magic find increases the quantity of items dropped.*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => StatActions.AddMagicFind(0.1f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 109 },
				levelReq = 15,
				cost = 1,
				scale = 1f,
				posX = -1.25f,
				posY = -1.85f,
				name = Translations.PerkDatabase_219/*og:Luck Enchantment II*/,//tr
				originalDescription = Translations.PerkDatabase_220/*og:Increases magic find by additional 10%. Magic find increases the quantity of items dropped.*/,//tr
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
				name = Translations.PerkDatabase_221/*og:Near death arrow*/,//tr
				originalDescription = Translations.PerkDatabase_222/*og:Blood infused arrow takes 25% more health to convert it to damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bia_HealthDmMult.valueAdditive += 1f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 14,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 2.25f,
				name = Translations.PerkDatabase_223/*og:Arcane Blood*/,//tr
				originalDescription = Translations.PerkDatabase_224/*og:Blood infused arrow damage per health is increased by 1 dmg/hp.*/,//tr
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
				name = Translations.PerkDatabase_225/*og:Energy Field*/,//tr
				originalDescription = Translations.PerkDatabase_226/*og:Healing dome regenerates energy*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxHealthMult.valueAdditive += 0.1f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 29 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_227/*og:Enhanced vitality*/,//tr
				originalDescription = Translations.PerkDatabase_228/*og:Increases max health by 10%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.maxEnergyMult.valueAdditive += 0.1f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 80 },
				levelReq = 9,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_229/*og:Enhanced energy*/,//tr
				originalDescription = Translations.PerkDatabase_230/*og:Increases max energy by 10%*/,//tr
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
				name = Translations.PerkDatabase_231/*og:Rerolling*/,//tr
				originalDescription = Translations.PerkDatabase_232/*og:Opens Crafting Menu in inventory. Allows you to reroll item's properties by placing 2 items of the same rarity as ingredients.*/,//tr
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
				name = Translations.PerkDatabase_233/*og:Reforging*/,//tr
				originalDescription = Translations.PerkDatabase_234/*og:Adds a tab to crafting menu. Allows you to reforge an item into any other item of the same tier by placing 3 items of the same or higher rarity as ingredients.*/,//tr
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
				name = Translations.PerkDatabase_235/*og:Light The Way*/,//tr
				originalDescription = Translations.PerkDatabase_236/*og:Flashlight is 100% brighter and lasts 100% longer*/,//tr
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
				name = Translations.PerkDatabase_237/*og:Glass Cannon*/,//tr
				originalDescription = Translations.PerkDatabase_238/*og:Increases all damage taken and increases all damage dealt by 12%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x =>
				{
					float f = 1.12f;
					for (int i = 1; i < x; i++)
						f *= 1.12f;
					return Translations.PerkDatabase_240/*og:\nTotal from this perk: Damage taken: */ + (f - 1).ToString("P") + Translations.PerkDatabase_239/*og:\nDamage dealt: */ + (0.12f * x).ToString("P");//tr
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
				name = Translations.PerkDatabase_241/*og:Size Matters*/,//tr
				originalDescription = Translations.PerkDatabase_242/*og:Projectile size increases projectile's damage.*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
				updateDescription = _ => Translations.PerkDatabase_243/*og:Every 1% of increased projectile size increases ranged damage by 2%*///tr
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
				name = Translations.PerkDatabase_244/*og:Momentum transfer*/,//tr
				originalDescription = Translations.PerkDatabase_245/*og:Upon landing on the ground gain short burst of movement speed*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.weaponRange.valueMultiplicative *= 1.05f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 22 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -2.25f,
				name = Translations.PerkDatabase_246/*og:Long arm*/,//tr
				originalDescription = Translations.PerkDatabase_247/*og:Increases melee weapon range by 5%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.chanceToBleed.valueAdditive += 0.025f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 78, 79 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_248/*og:Bleed*/,//tr
				originalDescription = Translations.PerkDatabase_249/*og:Hitting an enemy has 2.5% chance to make them bleed*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.chanceToWeaken.valueAdditive += 0.03f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 71, 22 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_250/*og:Weaken*/,//tr
				originalDescription = Translations.PerkDatabase_251/*og:Hitting an enemy has 3% chance to make them take more damage*/,//tr
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
				name = Translations.PerkDatabase_252/*og:Gold Medalist*/,//tr
				originalDescription = Translations.PerkDatabase_253/*og:Greatly increases spear throw skill. Spear has increased critical hit chance to 40%. Critical shots trigger randomly upon hitting any body part and deal headshot damage.*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearCritChance.valueAdditive += 0.1f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 125 },
				levelReq = 33,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -2.25f,
				name = Translations.PerkDatabase_254/*og:Spear gamble*/,//tr
				originalDescription = Translations.PerkDatabase_255/*og:Spear has increased critical hit chance to 50%*/,//tr
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
				name = Translations.PerkDatabase_256/*og:Double spears*/,//tr
				originalDescription = Translations.PerkDatabase_257/*og:When a spear hits a target, it has a 40% chance summon another spear and launch it at the enemy*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.perk_thrownSpearhellChance.valueAdditive += 0.03f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 127 },
				levelReq = 53,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -2.25f,
				name = Translations.PerkDatabase_258/*og:Spears!*/,//tr
				originalDescription = Translations.PerkDatabase_259/*og:Increases the chance of double spears by 3%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = true,
				updateDescription = x => string.Format("Total chance to cast another spear: {0}\nGetting above 100% will yield no results", (x * 0.03f + 0.4f).ToString("P"))
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
				name = Translations.PerkDatabase_260/*og:Spear Mastery*/,//tr
				originalDescription = Translations.PerkDatabase_261/*og:Doubles thrown spear damage*/,//tr
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
				name = Translations.PerkDatabase_262/*og:Deadeye*/,//tr
				originalDescription = Translations.PerkDatabase_263/*og:Increases headshot chance of pistol's bullets by 20%, to a total of 30%. Bullet headshots trigger randomly on hit*/,//tr
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
				name = Translations.PerkDatabase_264/*og:Piercing*/,//tr
				originalDescription = Translations.PerkDatabase_265/*og:Spear armor reduction from ranged is increased to 150%, additionally, thrown spears also reduce armor equal to melee armor reduction*/,//tr
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
				name = Translations.PerkDatabase_266/*og:Bunny hopping*/,//tr
				originalDescription = Translations.PerkDatabase_267/*og:Increases the speed and duration of the burst*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1.65f; ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= 0.9f; ModdedPlayer.Stats.rangedIncreasedDmg.Divide(2); },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 7f,
				posY = -0.75f,
				name = Translations.PerkDatabase_268/*og:Curse of Quickening*/,//tr
				originalDescription = Translations.PerkDatabase_269/*og:Increases attack speed by 65%, but decreases melee damage by 10% and ranged damage by 50%*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 0.5f; ModdedPlayer.Stats.meleeIncreasedDmg.Multiply(2); },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8f,
				posY = -0.75f,
				name = Translations.PerkDatabase_270/*og:Curse of Strengthening*/,//tr
				originalDescription = Translations.PerkDatabase_271/*og:Decreases attack speed by 50%, but melee damage is doubled*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeIncreasedDmg.valueAdditive += 2f; ModdedPlayer.Stats.rangedIncreasedDmg.valueMultiplicative *= 0; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 7.5f,
				posY = 0f,
				name = Translations.PerkDatabase_272/*og:Curse of Binding*/,//tr
				originalDescription = Translations.PerkDatabase_273/*og:Makes you unable to damage enemies with ranged weapons, causing all of them to deal 0 damage, but at the same time, you deal 200% increased melee damage*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeIncreasedDmg.valueAdditive += 0.2f; ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1.2f; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 79 },
				levelReq = 60,
				cost = 2,
				scale = 1,
				posX = 2f,
				posY = 2.25f,
				name = Translations.PerkDatabase_274/*og:Melee Mastery*/,//tr
				originalDescription = Translations.PerkDatabase_275/*og:Increases melee weapon damage and attack speed by 20%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.rangedIncreasedDmg.valueMultiplicative *= 2f; ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= 0; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_272/*og:Curse of Binding*/,//tr
				originalDescription = Translations.PerkDatabase_276/*og:Makes you unable to damage enemies with melee weapons, causing all of them to deal 0 damage, but at the same time, you deal double ranged damage*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.rangedIncreasedDmg.valueAdditive += 1.2f; ModdedPlayer.Stats.movementSpeed.valueMultiplicative *= 0.8f; ModdedPlayer.Stats.jumpPower.valueAdditive -= 0.7f; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8f,
				posY = -0.75f,
				name = Translations.PerkDatabase_277/*og:Curse of Crippling*/,//tr
				originalDescription = Translations.PerkDatabase_278/*og:You become more deadly but less precise.\nYour ranged damage is increased by 120%, but you loose 20% movement speed and 30% jump power.*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1.65f; ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= 0.4f; ModdedPlayer.Stats.rangedIncreasedDmg.Multiply(0.90f); },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9f,
				posY = -0.75f,
				name = Translations.PerkDatabase_268/*og:Curse of Quickening*/,//tr
				originalDescription = Translations.PerkDatabase_279/*og:Increases attack speed by 65%, but decreases melee damage by 60% and ranged damage by 10%*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};

			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.maxEnergyMult.Multiply(0.4f); ModdedPlayer.Stats.staminaPerSecRate.Multiply(0.5f); ModdedPlayer.Stats.spellIncreasedDmg.Multiply(2); },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9f,
				posY = -0.75f,
				name = Translations.PerkDatabase_280/*og:Curse of Exhaustion*/,//tr
				originalDescription = Translations.PerkDatabase_281/*og:Doubles spell damage, but your maximum energy is reduced by 60% and stamina regenerates slower*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 0.6f; ModdedPlayer.Stats.cooldown.valueMultiplicative *= 0.65f; },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8.5f,
				posY = 0f,
				name = Translations.PerkDatabase_282/*og:Curse of Speed*/,//tr
				originalDescription = Translations.PerkDatabase_283/*og:Cooldown reduction increased by 35%, but attack speed decreased by 40%*/,//tr
				textureVariation = 1, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= 0.5f; ModdedPlayer.Stats.rangedIncreasedDmg.valueMultiplicative *= 0.5f; ModdedPlayer.Stats.spellIncreasedDmg.Add(1.5f); },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9.5f,
				posY = 0f,
				name = Translations.PerkDatabase_284/*og:Curse of Power*/,//tr
				originalDescription = Translations.PerkDatabase_285/*og:Magic damage is increased by 150%, but ranged and melee damage are halved*/,//tr
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
				name = Translations.PerkDatabase_286/*og:Counter Strike*/,//tr
				originalDescription = Translations.PerkDatabase_287/*og:When parrying, gain attack dmg for the next attack. Bonus melee damage is equal to damage of parry. This effect can stack, lasts 20 seconds, and is consumed upon performing a melee attack.*/,//tr
				textureVariation = 0,
				stackable = false,
			};

			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDamageDebuffAmount.valueAdditive += 0.3f,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 214 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = Translations.PerkDatabase_288/*og:Skull Basher*/,//tr
				originalDescription = Translations.PerkDatabase_289/*og:Bashed enemies  take additional 30% more damage. Debuff lasts as long as bash slow.*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bashDamageDebuffAmount.valueAdditive += 0.4f,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 144 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -3f,
				name = Translations.PerkDatabase_290/*og:Skull Basher II*/,//tr
				originalDescription = Translations.PerkDatabase_291/*og:Bashed enemies take 40% additional damage. With previous perk, the total value is 100% increased damage versus bashed enemies.*/,//tr
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
				name = Translations.PerkDatabase_292/*og:Dance of the Firegod*/,//tr
				originalDescription = Translations.PerkDatabase_293/*og:When black flame is on, your melee damage is increased, based on how fast you're going. However the spell cost is increased by 10 times. Activating black flame disables berserk stamina refilling, and when black flame is put out, berserk spell returns to normal*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.staminaOnHit.valueAdditive += 5,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_294/*og:Combat Regen*/,//tr
				originalDescription = Translations.PerkDatabase_295/*og:Gain 5 points of stamina on hit*/,//tr
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
				name = Translations.PerkDatabase_296/*og:Mania*/,//tr
				originalDescription = Translations.PerkDatabase_297/*og:Frenzy increases movement speed by 5% per stack*/,//tr
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
				name = Translations.PerkDatabase_298/*og:Fury Swipes*/,//tr
				originalDescription = Translations.PerkDatabase_299/*og:When during frenzy you hit the same enemy over and over, gain more and more damage. Melee stacks 6 times faster.*/,//tr
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
				name = Translations.PerkDatabase_300/*og:Lucky Bashes*/,//tr
				originalDescription = Translations.PerkDatabase_301/*og:When you bash an enemy, gain 15% critical hit damage for 2 seconds.*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x =>
				{
					float f = 1.15f;
					for (int i = 1; i < x; i++)
						f += 0.15f;
					return Translations.PerkDatabase_302/*og:\nTotal from this perk:\nCrit damage after bashing - */ + (f - 1).ToString("P"); //tr
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
					name = Translations.PerkDatabase_303/*og:More Carried Logs*/,//tr
					originalDescription = Translations.PerkDatabase_304/*og:Increases the base amount of logs that a player can carry on their shoulder. The additional carried logs are invisible. */,//tr
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
					name = Translations.PerkDatabase_305/*og:Looting*/,//tr
					originalDescription = Translations.PerkDatabase_306/*og:Increases magic find by 15%*/,//tr
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
				name = Translations.PerkDatabase_307/*og:Speed Matters*/,//tr
				originalDescription = Translations.PerkDatabase_308/*og:Projectile speed increases projectile's crit damage.*/,//tr
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
				name = Translations.PerkDatabase_309/*og:Magic Arrow Devastation*/,//tr
				originalDescription = Translations.PerkDatabase_310/*og:Magic arrow can critically hit.*/,//tr
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
				name = Translations.PerkDatabase_311/*og:Nuke Conjuration*/,//tr
				originalDescription = Translations.PerkDatabase_312/*og:Ball Lightning can critically hit.*/,//tr
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
				name = Translations.PerkDatabase_313/*og:Blink - Wormhole*/,//tr
				originalDescription = Translations.PerkDatabase_314/*og:Blink has 66.6% increased distance*/,//tr
				textureVariation = 0,
				stackable = false,
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
				name = Translations.PerkDatabase_315/*og:Fiery Embrace*/,//tr
				originalDescription = Translations.PerkDatabase_316/*og:Fire damage is increased by 15%*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x => string.Format(Translations.PerkDatabase_317/*og:Total from this perk: {0}*/, (x * 0.15f).ToString("P")) //tr
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
				name = Translations.PerkDatabase_318/*og:Cursed Arrow*/,//tr
				originalDescription = Translations.PerkDatabase_319/*og:Blood infused arrow deals triple damage, but all recovery is halved, and you loose energy for a short time after casting the spell*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_bia_Weaken.value = true,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 111 },
				levelReq = 30,
				cost = 2,
				scale = 1,
				posX = 1.5f,
				posY = -4.5f,
				name = Translations.PerkDatabase_320/*og:Deep Wounds*/,//tr
				originalDescription = Translations.PerkDatabase_321/*og:Enemies hit by blood infused arrow take 100% increased damage from all sources for 15 seconds*/,//tr
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
				name = Translations.PerkDatabase_322/*og:Rejuvenation*/,//tr
				originalDescription = Translations.PerkDatabase_323/*og:Gain +1 energy on hit, and +1.5 life per hit.*/,//tr
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
				name = Translations.PerkDatabase_324/*og:Endless stream*/,//tr
				originalDescription = Translations.PerkDatabase_325/*og:Reduce the cooldown of Magic Arrow by 15 seconds*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.spell_parryRadius.Add(1); ModdedPlayer.Stats.perk_parryAnything.value = true; },
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 78 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_326/*og:Parry Mastery*/,//tr
				originalDescription = Translations.PerkDatabase_327/*og:Increases the radius of Parry by 1m, allows you to parry any type of enemy.*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.thornsPerStrenght.valueAdditive += 1.2f,
				category = PerkCategory.Defense,
				unlockPath = new int[] { 31 },
				levelReq = 3,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_328/*og:Thorny Skin*/,//tr
				originalDescription = Translations.PerkDatabase_329/*og:Every point of strength increases thorns by 1.2\nThorns scale with melee damage multiplier stats*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.thornsDmgMult.valueMultiplicative *= 2; ModdedPlayer.Stats.armor.valueAdditive += 400; },
				category = PerkCategory.Defense,
				unlockPath = new int[] { 32 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 0f,
				name = Translations.PerkDatabase_330/*og:Iron Maiden*/,//tr
				originalDescription = Translations.PerkDatabase_331/*og:Increases armor by 400, and increases thorns damage by 100%*/,//tr
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
				name = Translations.PerkDatabase_332/*og:Anti-Magic Training*/,//tr
				originalDescription = Translations.PerkDatabase_333/*og:Decreases magic damage taken by 20%*/,//tr
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
				name = Translations.PerkDatabase_334/*og:Dense Matter*/,//tr
				originalDescription = Translations.PerkDatabase_335/*og:Black holes cannot suck you in*/,//tr
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
				name = Translations.PerkDatabase_336/*og:Warmth*/,//tr
				originalDescription = Translations.PerkDatabase_337/*og:Blizzard slow effect greatly reduced*/,//tr
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
				name = Translations.PerkDatabase_338/*og:True Aim*/,//tr
				originalDescription = Translations.PerkDatabase_339/*og:Arrow headshots which hit enemies over 60 feet away and are not affected by seeking arrow hit enemies twice, and deal 50% increased damage*/,//tr
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
				name = Translations.PerkDatabase_340/*og:Spikes*/,//tr
				originalDescription = Translations.PerkDatabase_341/*og:Adds 30 thorns*/,//tr
				textureVariation = 0,
				stackable = true,
			};

			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[16].Cooldown -= 60,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 154 },
				levelReq = 80,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 6.75f,
				name = Translations.PerkDatabase_342/*og:Storm Season*/,//tr
				originalDescription = Translations.PerkDatabase_343/*og:Ball Lightning has its cooldown reduced by 60 seconds*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[16].Cooldown -= 20,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 169 },
				levelReq = 120,
				cost = 2,
				scale = 1,
				posX = 3f,
				posY = 6.75f,
				name = Translations.PerkDatabase_344/*og:Endless Storm*/,//tr
				originalDescription = Translations.PerkDatabase_345/*og:Ball Lightning has its cooldown reduced by 20 seconds*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => SpellDataBase.spellDictionary[3].Cooldown -= 7,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 155 },
				levelReq = 66,
				cost = 2,
				scale = 1,
				posX = 5f,
				posY = 3.75f,
				name = Translations.PerkDatabase_346/*og:Quick Silver*/,//tr
				originalDescription = Translations.PerkDatabase_347/*og:Blink has it's cooldown reduced by 7 seconds*/,//tr
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
				name = Translations.PerkDatabase_348/*og:Wrath of the Sun*/,//tr
				originalDescription = Translations.PerkDatabase_349/*og:Sun Flare cooldown is reduced by 20*/,//tr
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
				name = Translations.PerkDatabase_350/*og:Full Counter*/,//tr
				originalDescription = Translations.PerkDatabase_351/*og:Increase the damage bonus from parrying*/,//tr
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
				name = Translations.PerkDatabase_352/*og:Golden Resolve*/,//tr
				originalDescription = Translations.PerkDatabase_353/*og:Gold reduces damage taken by 50%*/,//tr
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
				name = Translations.PerkDatabase_354/*og:Sudden Rampage*/,//tr
				originalDescription = Translations.PerkDatabase_355/*og:Cooldown of Berserk is decreased by 90*/,//tr
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
				name = Translations.PerkDatabase_356/*og:Safe Heaven*/,//tr
				originalDescription = Translations.PerkDatabase_357/*og:Healing dome lasts a minute.*/,//tr
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
				name = Translations.PerkDatabase_358/*og:Time of Need*/,//tr
				originalDescription = Translations.PerkDatabase_359/*og:The cooldown of healing dome and purge is reduced by 50%*/,//tr
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
				name = Translations.PerkDatabase_360/*og:Breathing Tehniques*/,//tr
				originalDescription = Translations.PerkDatabase_361/*og:When black flame is on, your attack speed is fixed at 100%*/,//tr
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
				name = Translations.PerkDatabase_362/*og:Polishing*/,//tr
				originalDescription = Translations.PerkDatabase_363/*og:Adds a tab to crafting menu. Allows you to change the value of a single stat into either higher or lower. Allows emptying of sockets. Requires one item of the same rarity or greater*/,//tr
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
				name = Translations.PerkDatabase_364/*og:Empowering*/,//tr
				originalDescription = Translations.PerkDatabase_365/*og:Adds a tab to crafting menu. Allows you to change the level of an item to player's current level, without rerolling values. Requires nine items of the same or higher rarity*/,//tr
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
				name = Translations.PerkDatabase_366/*og:Luck Enchantment III*/,//tr
				originalDescription = Translations.PerkDatabase_367/*og:Increases magic find by additional 15%. Magic find increases the quantity of items dropped.*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.allDamageTaken.valueMultiplicative *= 0.95f; ModdedPlayer.Stats.armor.valueAdditive += 500; },
				category = PerkCategory.Defense,
				unlockPath = new int[] { 163, 34 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 0.75f,
				name = Translations.PerkDatabase_368/*og:Heavy metal*/,//tr
				originalDescription = Translations.PerkDatabase_369/*og:Increases armor by 500, reduces damage taken by 5%*/,//tr
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
				name = Translations.PerkDatabase_370/*og:Autocast Shield*/,//tr
				originalDescription = Translations.PerkDatabase_371/*og:When your energy and stamina is above 90% of max, and you have Sustain Shield spell equipped, the spell is automatically cast*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_seekingArrowDuration.Add(20),

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 95 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = -5f,
				posY = -3.75f,
				name = Translations.PerkDatabase_372/*og:Seeking Arrow - Improved Memory*/,//tr
				originalDescription = Translations.PerkDatabase_373/*og:Seeking arrow target stays for 20 seconds longer before disappearing*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_ballLightning_DamageScaling.Add(15),
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 170 },
				levelReq = 130,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 7.5f,
				name = Translations.PerkDatabase_374/*og:Storm of the century*/,//tr
				originalDescription = Translations.PerkDatabase_375/*og:Ball Lightning damage scaling is increased by 1500%*/,//tr
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
				name = Translations.PerkDatabase_376/*og:Craftable Plane Axe*/,//tr
				originalDescription = Translations.PerkDatabase_377/*og:Allows you to craft a plane axe.\nThe receipe is: 1 crafted axe, 1 rope, 2 sticks*/,//tr
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
				name = Translations.PerkDatabase_378/*og:Hawk's eye*/,//tr
				originalDescription = Translations.PerkDatabase_379/*og:Headshot damage is greatly increased*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.explosionDamage.Add(0.35f),
				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_380/*og:Explosion Damage Up*/,//tr
				originalDescription = Translations.PerkDatabase_381/*og:Increases explosion and dropkick attack damage by 35%. This affects explosions of other players too.*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x => string.Format(Translations.PerkDatabase_382/*og:\nTotal: {0}*/, (x * 0.35f).ToString("P")) //tr
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.rangedIncreasedDmg.Multiply(1.3f);
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
				name = Translations.PerkDatabase_383/*og:Ranged Mastery*/,//tr
				originalDescription = Translations.PerkDatabase_384/*og:Ranged damage increased by 30%, headshot damage increased by 300%, critical hit damage increased by 100%*/,//tr
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
				name = Translations.PerkDatabase_385/*og:Harvester*/,//tr
				originalDescription = Translations.PerkDatabase_386/*og:Bushes drop twice as many sticks*/,//tr
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
				name = Translations.PerkDatabase_387/*og:Block*/,//tr
				originalDescription = Translations.PerkDatabase_388,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.thornsPerVit.Add(0.5f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 168 },
				levelReq = 16,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 2.25f,
				name = Translations.PerkDatabase_389/*og:Thorny Skin II*/,//tr
				originalDescription = Translations.PerkDatabase_390/*og:Every 2 points of vitality adds 1 thorns*/,//tr
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
				name = Translations.PerkDatabase_391/*og:Corrosive Skin*/,//tr
				originalDescription = Translations.PerkDatabase_392/*og:Getting hit reduces enemy armor for 250% of your all armor penetration stat*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireTickRate.Add(0.1f),

				category = PerkCategory.Support,
				unlockPath = new int[] { 156 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = Translations.PerkDatabase_393/*og:Pyromancy*/,//tr
				originalDescription = Translations.PerkDatabase_394/*og:Fire damage ticks 10% faster on enemies. This property stacks with other players*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x => string.Format(Translations.PerkDatabase_382/*og:\nTotal: {0}*/, (x * 0.10f).ToString("P")) //tr
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireTickRate.Add(1f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 194 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = Translations.PerkDatabase_395/*og:Heat Surge*/,//tr
				originalDescription = Translations.PerkDatabase_396/*og:Fire damage ticks 100% faster on enemies. This property stacks with other players*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireDuration.Add(0.07f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 156 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = Translations.PerkDatabase_397/*og:Flame lifetime*/,//tr
				originalDescription = Translations.PerkDatabase_398/*og:Enemies are ignited for 7% longer. This property stacks with other players*/,//tr
				textureVariation = 0,
				stackable = true,
				updateDescription = x => string.Format(Translations.PerkDatabase_382/*og:\nTotal: {0}*/, (x * 0.07f).ToString("P")) //tr
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.fireDuration.Add(0.4f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 196 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = Translations.PerkDatabase_399/*og:Living flame*/,//tr
				originalDescription = Translations.PerkDatabase_400/*og:Enemies are ignited for 40% longer. This property stacks with other players*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.expGain.Add(0.15f),

				category = PerkCategory.Support,
				unlockPath = new int[] { 42 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = -5.5f,
				posY = 0f,
				name = Translations.PerkDatabase_401/*og:Insight II*/,//tr
				originalDescription = Translations.PerkDatabase_402/*og:All experience gained increased by 15%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spellFlatDmg.Add(1300),

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 89 },
				levelReq = 122,
				cost = 2,
				scale = 1,
				posX = -0.5f,
				posY = 6f,
				name = Translations.PerkDatabase_403/*og:The Real Armageddon*/,//tr
				originalDescription = Translations.PerkDatabase_404/*og:The previous Armageddon was the 2012 kind, very disappointing.\nBase spell damage is increased by 1300*/,//tr
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
				name = Translations.PerkDatabase_405/*og:The Big Succ*/,//tr
				originalDescription = Translations.PerkDatabase_406/*og:Increase the size of the blackhole by 15 meters. Which is basically double the size*/,//tr
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
				name = Translations.PerkDatabase_407/*og:The Mega Endless Succ*/,//tr
				originalDescription = Translations.PerkDatabase_408/*og:Increase the size of the black hole by 2.5 meters*/,//tr
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
				name = Translations.PerkDatabase_407/*og:The Mega Endless Succ*/,//tr
				originalDescription = Translations.PerkDatabase_409/*og:Increase the pull force of the black hole by 25*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.vitality.Add(20),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 114 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -1.5f,
				name = Translations.MainMenu_Guide_8/*og:Vitality*/,//tr
				originalDescription = Translations.PerkDatabase_410/*og:Increase vitality by 20*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.thornsDmgMult.Multiply(2f);
					ModdedPlayer.Stats.thornsArmorPiercing.Add(2.5f);
					ModdedPlayer.Stats.allDamageTaken.Multiply(0.95f);
					ModdedPlayer.Stats.meleeIncreasedDmg.Multiply(0.9f);
					ModdedPlayer.Stats.rangedIncreasedDmg.Multiply(0.9f);
					ModdedPlayer.Stats.spellIncreasedDmg.Multiply(0.9f);
				},
				category = PerkCategory.Defense,
				unlockPath = new int[] { 89 },
				unlockRequirement = new int[] { 193 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 4f,
				posY = 3.75f,
				name = Translations.PerkDatabase_411/*og:Curse of Thorns*/,//tr
				originalDescription = Translations.PerkDatabase_412/*og:Thorns damage is doubled. Thorns armor piercing is doubled. Damage taken is slightly reduced. Melee, ranged and spell damage are decreased by 10%*/,//tr
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.allRecoveryMult.Multiply(1.5f),
				category = PerkCategory.Support,
				unlockPath = new int[] { 159 },
				levelReq = 76,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -2.25f,
				name = Translations.PerkDatabase_413/*og:Restoration*/,//tr
				originalDescription = Translations.PerkDatabase_414/*og:All healing and stamina/energy recovery are increased by 50%*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.spell_taunt_speedChange.Multiply(0.5f * 0.7f),

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 15 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = Translations.PerkDatabase_415/*og:Falter*/,//tr
				originalDescription = Translations.PerkDatabase_416/*og:Taunt no longer makes enemies attack faster. Instead, it slows them by 30%*/,//tr
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
				name = Translations.PerkDatabase_417/*og:Can't Take Me Down Alone*/,//tr
				originalDescription = Translations.PerkDatabase_418/*og:Taunt pulls enemies in to the center*/,//tr
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
				name = Translations.PerkDatabase_419/*og:Charge Pushback*/,//tr
				originalDescription = Translations.PerkDatabase_420/*og:Charged melee attacks push enemies back*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => { ModdedPlayer.Stats.spell_parryRadius.Add(1); ModdedPlayer.Stats.perk_parryAnything.value = true; },
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 78 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_421/*og:Instant Riposte*/,//tr
				originalDescription = Translations.PerkDatabase_422/*og:Parrying increases attack speed by 80% for 5 seconds*/,//tr
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
				name = Translations.PerkDatabase_423/*og:Rerolling individual stats*/,//tr
				originalDescription = Translations.PerkDatabase_424/*og:Adds a tab to crafting menu. Allows you to change the value of a single stat another stats that can occur on an item. Allows emptying of sockets. Requires one item of the same rarity or greater*/,//tr
				textureVariation = 1,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive += 55f;
					ModdedPlayer.Stats.spell_fireboltDamageScaling.valueAdditive += 0.1f;
				},
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 2.25f,
				name = Translations.PerkDatabase_425/*og:Firebolt upgrade*/,//tr
				originalDescription = "Firebolt damage increases, but so does it's cost\n" +
				Translations.PerkDatabase_426/*og:Damage scaling increases to 75% from 20%. Cost increases to 25 from 15*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive += ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive;
					ModdedPlayer.Stats.spell_fireboltDamageScaling.valueAdditive += 0.25f;
				},
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 211 },
				levelReq = 17,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 2.25f,
				name = Translations.PerkDatabase_425/*og:Firebolt upgrade*/,//tr
				originalDescription = "Firebolt damage increases, but so does it's cost\n" +
				Translations.PerkDatabase_427/*og:Damage scaling increases to 25%. Cost doubles\n*/,//tr
				updateDescription = x => string.Format(Translations.PerkDatabase_428/*og:Damage scaling: {0}\nCost: {1}*/, ModdedPlayer.Stats.spell_fireboltDamageScaling.valueAdditive.ToString("P"), ModdedPlayer.Stats.spell_fireboltEnergyCost.valueAdditive.ToString("N")),//tr
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
				name = Translations.PerkDatabase_429/*og:Heaviest Blow*/,//tr
				originalDescription = Translations.PerkDatabase_430/*og:Charged melee attacks deal 10 000% increased damage*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.strength.Add(100),
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = Translations.PerkDatabase_431/*og:Body Strength II*/,//tr
				originalDescription = Translations.PerkDatabase_432/*og:Increases strength by 100*/,//tr
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
				name = Translations.PerkDatabase_433/*og:Steroid*/,//tr
				originalDescription = Translations.PerkDatabase_434/*og:Increases melee damage by 25%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,
			};
			new Perk()
			{
				onApply = () =>
				{
					ModdedPlayer.Stats.spell_fireboltEnergyCost.Substract(10);
				},
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 212 },
				levelReq = 65,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 2.25f,
				name = Translations.PerkDatabase_435/*og:Firebolt cost reduction*/,//tr
				originalDescription = Translations.PerkDatabase_436/*og:Firebolt cost is reduced from 15 to 5\n*/,//tr
				textureVariation = 0,
				stackable = false,
			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.rangedIncreasedDmg.Add(0.06f),
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 16,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = Translations.PerkDatabase_437/*og:Target practise*/,//tr
				originalDescription = Translations.PerkDatabase_438/*og:Increases ranged damage by 6%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,

			};
			new Perk()
			{
				onApply = () => ModdedPlayer.Stats.critChance.Add(0.08f),
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 217 },
				levelReq = 36,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = 2.25f,
				name = Translations.PerkDatabase_439/*og:Bullseye*/,//tr
				originalDescription = Translations.PerkDatabase_440/*og:Increases crit chance by 8%*/,//tr
				textureVariation = 0, //0 or 1
				stackable = false,

			};
			new Perk()
			{
				onApply = () => COTFEvents.Instance.OnDodge.AddListener(() => TheForest.Utils.LocalPlayer.Stats.Health += ModdedPlayer.Stats.TotalMaxHealth * 0.05f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 36 },
				levelReq = 60,
				cost = 1,
				scale = 1,
				posX = 6f,
				posY = 2.25f,
				name = Translations.PerkDatabase_441/*og:Improved Dodges*/,//tr
				originalDescription = Translations.PerkDatabase_442/*og:Heal for 5% of max health when you dodge*/,//tr
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