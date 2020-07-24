using System.Collections.Generic;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Items;

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
				apply = () => ModdedPlayer.instance.DamagePerStrength += 0.01f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = "Stronger Hits",
				originalDescription = "Gene allows muscules to quickly change their structure to a more efficient one.\nEvery point of STRENGHT increases MEELE DAMAGE by 1%.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpellDamageperInt += 0.01f,

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = "Stronger Spells",
				originalDescription = "Gene changes the composition of axon sheath that greatly increases brain's power.\nEvery point of INTELLIGENCE increases SPELL DAMAGE by 1%.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.RangedDamageperAgi += 0.01f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = "Stronger Projectiles",
				originalDescription = "Neural connections between muscules and the brain are now a lot more sensitive. Your movements become a lot more precise.\nEvery point of AGILITY increases RANGED DAMAGE by 1%.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.EnergyRegenPerInt += 0.01f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = "Stamina Recovery",
				originalDescription = "Heart's muscules become even more resistant to exhaustion.\nEvery point of INTELLIGENCE increases stamina recovery rate by 1%.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.EnergyPerAgility += 0.5f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 0,
				name = "More Stamina",
				originalDescription = "Hemoglobin is replaced with an alternative substance capable of carrying more oxygen.\nEvery point of AGILITY increases max stamina by 0.5",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.HealthPerVitality += 1.75f,

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 0,
				name = "More Health",
				originalDescription = "Skin and bones become more resisitant to injuries.\nEvery point of VITALITY increases max health by 1.75",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.HealingMultipier *= 1.05f,

				category = PerkCategory.Support,
				texture = null,
				unlockPath = new int[] { -1 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 0,
				name = "More Healing",
				originalDescription = "Blood becomes denser, is less vunerable to bleeding and wounds are healed faster.\nIncreases all healing by 5%",
				updateDescription = x =>
				{
					float f = 1.05f;
					for (int i = 1; i < x; i++)
						f *= 1.05f;
					return "\nTotal from this perk: " + (f - 1).ToString("P");
				},
				textureVariation = 0, //0 or 1
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.HungerRate *= 0.9f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 4 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = 0.75f,
				name = "Metabolism",
				originalDescription = "Additional microorganisms are now present in the digestive system that allow to feed of previousely undigested food.\nDecreases hunger rate by 10%.",
				textureVariation = 1, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					float f = 0.9f;
					for (int i = 1; i < x; i++)
						f *= 0.9f;
					return "\nTotal from this perk: " + (1 - f).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ThirstRate *= 0.9f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 4 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -0.75f,
				name = "Water Conservation",
				originalDescription = "Sweating is decreased, kidneys keep more water.\nDecreases thirst rate by 10%.",
				textureVariation = 1, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					float f = 0.9f;
					for (int i = 1; i < x; i++)
						f *= 0.9f;
					return "\nTotal from this perk: " + (1 - f).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MeleeDamageBonus += 5,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 0, 10 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = "Damage",
				originalDescription = "Grip strength increases by 1 kg.\nIncreases melee damage by 5",
				textureVariation = 0, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + x * 5;
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.1f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 0, 9, 11 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = "Damage",
				originalDescription = "Biceps slightly increases in size.\nIncreases melee damage by 10%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.strength += 10,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 0, 10 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = "Strength",
				originalDescription = "All flexors gain in size.\nIncreases strength by 10",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.RangedDamageBonus += 5,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = "Damage",
				originalDescription = "Shoulder muscules grow.\nIncreases projectile damage by 5",
				textureVariation = 0, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + x * 5;
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ProjectileSizeRatio += 0.05f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = "Size",
				originalDescription = "Increased overall physical strength allows for precise shoots from hand held weapons with bigger ammunition.\nIncreases projectile size by 5%",
				textureVariation = 0, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (x * 0.05f).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ProjectileSpeedRatio += 0.05f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 2 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = "Speed",
				originalDescription = "Increased overall physical strength allows for stronger drawing of ranged weaponry.\nIncreases projectile speed by 5%",
				textureVariation = 0, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (x * 0.05f).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, 0.1f),

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 1 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = "Transmutation",
				originalDescription = "The costs of casting spells become easier to quicly recover from.\n10% of the spell cost is now taxed from stamina instead of energy.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpellCostRatio *= 1 - 0.04f,

				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 15 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = "Resource Cost Reduction",
				originalDescription = "In order to preserve energy, spell costs are reduced by 4%",
				textureVariation = 0, //0 or 1
				uncapped = true,
				updateDescription = x =>
				{
					float f = 0.96f;
					for (int i = 1; i < x; i++)
						f *= 0.96f;
					return "\nTotal from this perk: " + (1 - f).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.DamageReductionPerks *= 0.70f; ModdedPlayer.instance.DamageOutputMultPerks *= 0.70f; },

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { 5 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = "Undestructable",
				originalDescription = "Decreases all damage taken and decreases all damage dealt by 30%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CoolDownMultipier *= 0.9f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 1.5f,
				name = "Cool Down Reduction",
				originalDescription = " Reduces spell cooldown by 10%",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					float f = 0.96f;
					for (int i = 1; i < x; i++)
						f *= 0.96f;
					return "\nTotal from this perk: " + (1 - f).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CoolDownMultipier *= 0.925f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 18 },
				levelReq = 8,
				cost = 2,
				scale = 1,
				posX = 3f,
				posY = 2.25f,
				name = "Greater Cool Down Reduction",
				originalDescription = " Reduces spell cooldown by 7,5%",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => StatActions.AddAllStats(5),
				category = PerkCategory.Utility,
				unlockPath = new int[] { 3 },
				levelReq = 1,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = "All attributes",
				originalDescription = "+5 to every strength, agility, vitality and intelligence",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => StatActions.AddAllStats(15),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 20 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = "All attributes",
				originalDescription = "+15 to every strength, agility, vitality and intelligence",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => StatActions.AddAttackSpeed(0.04f),

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11 },
				levelReq = 6,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = "Attack speed",
				originalDescription = "+4% to attack speed ",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (0.04f * x).ToString("P");
				},
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.ReusabilityChance += 0.03f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12, 14 },
				levelReq = 7,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = "Reusability I",
				originalDescription = "+3% chance to not consume ammo while firing.",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (0.03f * x).ToString("P");
				},
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.ReusabilityChance += 0.13f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 23 },
				levelReq = 9,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = "Reusability II",
				originalDescription = "+13% chance to not consume ammo while firing.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ReusabilityChance += 0.13f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 24 },
				levelReq = 12,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = "Reusability III",
				originalDescription = "+13% chance to not consume ammo while firing.",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => StatActions.AddAllStats(10),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 21 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = "All attributes",
				originalDescription = "+10 to strength, agility, vitality and intelligence",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.JumpPower += 0.06f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 3 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 0.75f,
				name = "Jump",
				originalDescription = "Increases jump height by 6%",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (0.06f * x).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MoveSpeed += 0.035f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 27 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = "Light foot",
				originalDescription = "Increases movement speed by 3.5%",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (0.035f * x).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.HealthBonus += 25,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 5 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = "Bonus Health",
				originalDescription = "Increases health by 25. This is further multipied by maximum health percent.",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (25 * x).ToString("N");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.LifeRegen += 0.25f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 6 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 0f,
				name = "Health Regen",
				originalDescription = "Increases health per second regeneration by 0.25 HP/second. This is further multipied by health regen percent and all healing percent.",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (0.25f * x).ToString("N2");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.Armor += 40,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 5 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 0f,
				name = "Bonus Armor",
				originalDescription = "Increases armor by 40.",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					return "\nTotal from this perk: " + (40 * x).ToString("N");
				},
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 31 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = "Durability",
				originalDescription = "Decreases all damage taken by 10%.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 32 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = "Durability II",
				originalDescription = "Further decreases all damage taken by 10%.",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 33 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = "Durability III",
				originalDescription = "Further decreases all damage taken by 10%.",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.MagicResistance, 0.05f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 29, 31 },
				levelReq = 6,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = "Magic Resistance",
				originalDescription = "Decreases magic damage taken by 5%",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					float f = 0.95f;
					for (int i = 1; i < x; i++)
						f *= 0.95f;
					return "\nTotal from this perk: " + (1 - f).ToString("N");
				},
			};
			new Perk()
			{
				apply = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.DodgeChance, 0.25f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 34 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 2.25f,
				name = "Dodge",
				originalDescription = "Increases dodge chance by 25%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ARreduction_all += 3,

				category = PerkCategory.Support,
				unlockPath = new int[] { 6 },
				levelReq = 6,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = 0.75f,
				name = "Armor Penetration",
				originalDescription = "Increases armor penetration from all sources by 3",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ARreduction_melee += 10,

				category = PerkCategory.Support,
				unlockPath = new int[] { 37 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 1.5f,
				name = "Armor Piercing Edge",
				originalDescription = "Increases armor penetration from melee weapons by 10",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ARreduction_ranged += 5,

				category = PerkCategory.Support,
				unlockPath = new int[] { 37 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 1.5f,
				name = "Anti armor projectiles",
				originalDescription = "Increases armor penetration from ranged weapons by 5",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.HealthRegenPercent += 0.1f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 30 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -3.5f,
				posY = 0f,
				name = "More Health Regen",
				originalDescription = "Passive health regeneration is increased by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.EnergyPerSecond += 0.15f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 30 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -3f,
				posY = -0.75f,
				name = "Energy generation",
				originalDescription = "Passive energy regeneration is increased by 0.15/s",
				textureVariation = 0,
				uncapped = true,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.ExpFactor *= 1.1f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 0f,
				name = "Insight",
				originalDescription = "All experience gained increased by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.LifeOnHit += 1f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -0.75f,
				name = "Life On Hit",
				originalDescription = "Life on hit increased by 1",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.EnergyOnHit += 0.5f,

				category = PerkCategory.Support,
				unlockPath = new int[] { 41 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = -1.5f,
				name = "Energy On Hit",
				originalDescription = "Energy on hit increased by 0.5",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddGeneratedResource(33, 6),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 4 },
				levelReq = 12,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 0,
				name = "Alternative cloth sources",
				originalDescription = "Increases daily generation of cloth by 6",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddGeneratedResource(29, 2),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -3.5f,
				posY = 0,
				name = "Demolition",
				originalDescription = "Increases daily generation of bombs by 2. If it exceeds your max amount of bombs carried, excess will be lost.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AddExtraItemCapacity(29, 10); ModdedPlayer.instance.AddExtraItemCapacity(175, 15); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 46 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = 0.75f,
				name = "Pockets for explosives",
				originalDescription = "Increases max amount of carried bombs and dynamite by 30",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddGeneratedResource(175, 2),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 47 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 1.5f,
				name = "Demolition Expert",
				originalDescription = "Increases daily generation of dynamite by 2. If it exceeds your max amount of bombs carried, excess will be lost.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddGeneratedResource(49, 1),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 0,
				name = "Meds",
				originalDescription = "Increases daily generation of meds by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddGeneratedResource(262, 1),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -6.5f,
				posY = 0,
				name = "Fuel",
				originalDescription = "Increases daily generation of fuel cans by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddGeneratedResource(37, 2),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -5.5f,
				posY = 0,
				name = "Booze",
				originalDescription = "Increases daily generation of booze by 2. If it exceeds your max amount of bombs carried, excess will be lost.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(37, 15),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 51 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -6f,
				posY = -0.75f,
				name = "More Booze",
				originalDescription = "Increases max amount of carried booze by 25",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(49, 20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 49 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = -0.75f,
				name = "More Meds",
				originalDescription = "Increases max amount of carried meds by 20",
				textureVariation = 0,
				uncapped = true,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.ReusabilityChance += 0.5f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 25 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -0.75f,
				name = "Infinity",
				originalDescription = "Gives 50% chance to not consume ammo when firing a projectile",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpellDamageBonus += 5,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 1 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = "Spell Power",
				originalDescription = "Increases spell damage by 5",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 76, 35, 123, 207, 127 }, 5),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -7.5f,
				posY = 0,
				name = "More Meat",
				originalDescription = "Increases carry amount of all meats by 5",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 109, 89 }, 20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -8.5f,
				posY = 0,
				name = "More Snacks",
				originalDescription = "Increases carry amount of candy bars and sodas by 20",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(307, 20),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -13.5f,
				posY = 0,
				name = "More Bolts",
				originalDescription = "Increases carry amount of crossbow bolts by 20",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AddExtraItemCapacity(94, 20); ModdedPlayer.instance.AddExtraItemCapacity(178, 100); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -9.5f,
				posY = 0,
				name = "Corpse collecting",
				originalDescription = "Increases carry amount of bones by 100 and skulls by 20",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 90, 47, 46, 101 }, 10),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 59 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -10f,
				posY = 0.75f,
				name = "More Limbs",
				originalDescription = "Increases carry amount of arms, legs, heads and headbombs by 10",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AddExtraItemCapacity(57, 6); ModdedPlayer.instance.AddExtraItemCapacity(53, 2); ModdedPlayer.instance.AddExtraItemCapacity(54, 1); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -10.5f,
				posY = 0,
				name = "More Building Resources",
				originalDescription = "Increases carry amount of sticks by 6, rocks by 2 and ropes by 1",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 31, 142, 141, 41, 43, 144 }, 5),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -11.5f,
				posY = 0,
				name = "More Misceleanous Items",
				originalDescription = "Increases carry amount of pots, turtle shells, watches, circuit boards, air carnisters and flares by 5",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 177, 71, 56 }, 5); ModdedPlayer.instance.AddExtraItemCapacity(82, 50); },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 45 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -12.5f,
				posY = 0,
				name = "More Ammo",
				originalDescription = "Increases carry amount of weak and upgraded spears and molotovs by 5, small rocks by 50",
				textureVariation = 0,
				uncapped = true,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearDamageMult *= 2f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = "Spear Specialization",
				originalDescription = "Thrown spears deal 100% more damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BulletDamageMult *= 1.6f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = "Pistol Specialization",
				originalDescription = "Bullets deal 60% more damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CrossbowDamageMult *= 1.8f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -3f,
				name = "Crossbow Specialization",
				originalDescription = "Bolts deal 80% more damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BowDamageMult *= 1.4f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -3.75f,
				name = "Bow Specialization",
				originalDescription = "Arrows deal 40% more damage",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => SpellActions.HealingDomeGivesImmunity = true,

				category = PerkCategory.Support,
				unlockPath = new int[] { 40 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = -4f,
				posY = 0.75f,
				name = "Sanctuary",
				originalDescription = "Healing dome provides immunity to stuns anr root effects",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => BlackFlame.GiveDamageBuff = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = "Enchant weapon",
				originalDescription = "While black flame is on, melee damage is increased by 60%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.WarCryGiveDamage = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = "Empowered War Cry",
				originalDescription = "Warcry additionally increases all damage dealt",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.25f; ModdedPlayer.instance.StaminaAttackCost *= 1.4f; },

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11, 10 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = "Power Swing",
				originalDescription = "Attacks use 40% more stamina and deal 25% more damage",
				textureVariation = 0,
				uncapped = true,
				updateDescription = x =>
				{
					float f = 1.25f;
					for (int i = 1; i < x; i++)
						f += 0.25f;
					float f1 = 1.3f;
					for (int i = 1; i < x; i++)
						f1 *= 1.4f;
					return "\nTotal from this perk:\nDamage - " + (f - 1).ToString("P") + "\nStamina Cost - " + (f1 - 1).ToString("P");
				},
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.SpellDamageAmplifier_Add += 0.1f; ModdedPlayer.instance.SpellCostRatio *= 1.25f; },

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 15, 55 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -0.75f,
				name = "Overcharge",
				originalDescription = "Spell damage is increased by 10%, spell costs are increased by 25%",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => SpellActions.MagicArrowDmgDebuff = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -2.25f,
				name = "Exposure",
				originalDescription = "Magic arrow causes hit enemies to take 40% more damage for the duration of the slow.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.MagicArrowDuration += 5,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 73 },
				levelReq = 40,
				cost = 2,
				scale = 1,
				posX = 4f,
				posY = -2.25f,
				name = "Disabler",
				originalDescription = "Magic arrow's negative effects last additional 5 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.MagicArrowDoubleSlow = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 74 },
				levelReq = 46,
				cost = 2,
				scale = 1,
				posX = 5f,
				posY = -2.25f,
				name = "Magic Binding",
				originalDescription = "Magic arrow's slow amount is doubled. It's upgraded from 45% to 90%",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.HeavyAttackMult *= 1.5f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 10 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = "Charged Attack",
				originalDescription = "Charged melee attacks deal additional 50%",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.HeavyAttackMult *= 4f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 76 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 0f,
				name = "Super Charged Attack",
				originalDescription = "Charged melee attacks deal 300% more damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CritDamage += 10,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9, 10 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = "Lucky Hits",
				originalDescription = "Increases Critical hit damage by 10%",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CritChance += 0.12f,

				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9, 78 },
				levelReq = 35,
				cost = 2,
				scale = 1,
				posX = 2.5f,
				posY = 1.5f,
				name = "Overhelming Odds",
				originalDescription = "Increases Critical chance by 12%.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.EnergyBonus += 10,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 3 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -0.75f,
				name = "Endurance",
				originalDescription = "Increases maximum energy by 10",
				textureVariation = 0,
				uncapped = true,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.MultishotCount += 2,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 12 },
				levelReq = 31,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = -1.5f,
				name = "Multishot Empower",
				originalDescription = "Increases the projectile count of multishot by 2, also increases the spells cost. Multishot does not apply to spears.",
				updateDescription = x => string.Format("\nMultishot cost now: {0}\nCost after upgrading: {1}", (10 * Mathf.Pow(2 * x, 1.75f)).ToString("N"), (10 * Mathf.Pow((2 + 2 * x), 1.75f)).ToString("N")),
				textureVariation = 0,
				uncapped = true,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.TurboRaft = true,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 50 },
				levelReq = 22,
				cost = 1,
				scale = 1,
				posX = -7f,
				posY = 0.75f,
				name = "Transporter",
				originalDescription = "Allows you to use raft on land, turning it into a wooden hovercraft. WORKS FOR HOST/SINGLEPLAYER ONLY!",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.RaftSpeedMultipier++,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 82 },
				levelReq = 23,
				cost = 1,
				scale = 1,
				posX = -7.5f,
				posY = 1.5f,
				name = "Turbo",
				originalDescription = "Hovercraft but faster!\nIncreases the speed of rafts by 100%. WORKS FOR HOST/SINGLEPLAYER ONLY!",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => SpellActions.PurgeHeal = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 15 },
				levelReq = 10,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = "Transpurgation",
				originalDescription = "Purge now heals all players for percent of their missing health and restores energy for percent of missing energy.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CoolDownMultipier *= 0.925f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 24,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 3f,
				name = "Greater Cool Down Reduction",
				originalDescription = " Reduces spell cooldown by 7,5%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CoolDownMultipier *= 0.9f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 85 },
				levelReq = 35,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 3.75f,
				name = "Greater Cool Down Reduction",
				originalDescription = " Reduces spell cooldown by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CoolDownMultipier *= 0.9f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 86 },
				levelReq = 46,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 4.5f,
				name = "Greater Cool Down Reduction",
				originalDescription = " Reduces spell cooldown by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellCaster.InfinityEnabled = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 87 },
				levelReq = 60,
				cost = 2,
				scale = 1,
				posX = 1f,
				posY = 5.25f,
				name = "Infinity",
				originalDescription = "Every time you cast a spell, all cooldowns are reduced by 5%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpellDamageAmplifier_Add += 0.5f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 88 },
				levelReq = 61,
				cost = 2,
				scale = 1,
				posX = 0.5f,
				posY = 6f,
				name = "Armageddon",
				originalDescription = "Spell damage increased by 50%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpellAmpFireDmg = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { -1 },
				levelReq = 2,
				cost = 1,
				scale = 1,
				posX = -1.5f,
				posY = 0f,
				name = "Inner Fire",
				originalDescription = "Upon hitting an enemy, leave a debuff for 4 seconds, increase fire damage against that enemy equal to your spell amplification",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.NearDeathExperienceUnlocked = true,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 35 },
				levelReq = 20,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = "Near Death Experience",
				originalDescription = "Upon recieving fatal damage, instead of dieing restore your health to 100% and gain 5 seconds of immunity to debuffs. This may occur once every 10 minutes",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.SeekingArrow_HeadDamage = 3,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 13,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -1.5f,
				name = "Seeking Arrow - Head Hunting",
				originalDescription = "Seeking arrow additional damage on headshot is increased to x3",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.SeekingArrow_DamagePerDistance += 0.01f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 92 },
				levelReq = 19,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -2.25f,
				name = "Seeking Arrow - Distant Killer",
				originalDescription = "Seeking arrow additional damage per distance increased from 1% per 1m to 2% per 1m",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.SeekingArrow_SlowDuration += 4,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 93 },
				levelReq = 26,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -3f,
				name = "Seeking Arrow - Crippling precision",
				originalDescription = "Seeking arrow slow duration is increased by 4 additional seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.SeekingArrow_SlowAmount -= 0.2f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 94 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -3.75f,
				name = "Seeking Arrow - Stun Arrows",
				originalDescription = "Seeking arrow slow amount is increased - from 60% to 80%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.FocusOnHS += 0.5f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 55 },
				levelReq = 14,
				cost = 1,
				scale = 1,
				posX = -0.5f,
				posY = -1.5f,
				name = "Focus - Perfection",
				originalDescription = "Focus damage bonus on headshot is increased from 100% to 150%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.FocusOnAtkSpeed += 0.15f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 96 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = -1f,
				posY = -2.25f,
				name = "Focus - Quick Adjustments",
				originalDescription = "Focus extra attack on bodyshot is increased from 30% to 45%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.FocusOnAtkSpeed += 0.15f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 97 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = -0.5f,
				posY = -3f,
				name = "Focus - Quicker Adjustments",
				originalDescription = "Focus extra attack speed on bodyshot is increased from 45% to 60%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.FocusSlowDuration += 20f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 97 },
				levelReq = 35,
				cost = 2,
				scale = 1,
				posX = -1.5f,
				posY = -3f,
				name = "Focus - Knock Out",
				originalDescription = "Focus Slow is prolongued by additional 20 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => BlackFlame.GiveAfterburn = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 69 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = "Afterburn",
				originalDescription = "Black flames have a 10% chance to apply a weakening effect on enemies, making them take 15% more damage for 25 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => BlackFlame.DmgAmp = 2,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 100 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -3f,
				name = "Netherflame",
				originalDescription = "Black flames have double damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.FrenzyAtkSpeed += 0.02f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 72 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = "Frenzy - Haste",
				originalDescription = "Every stack of frenzy increases attack speed by 2%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.BashDuration += 1,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 72 },
				levelReq = 26,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = "Greater Bash",
				originalDescription = "Bash duration is increased by 1 seconds.\nIf bash applies bleed, bleeding deals overall more damage",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => SpellActions.ShieldPersistanceLifetime += 60f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = "Shield - Endurance",
				originalDescription = "Shield doesnt decay for 1 minute longer.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => SpellActions.BlinkDamage += 14f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 86 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 3.75f,
				name = "Blink - Passthrough",
				originalDescription = "Blink now deals damage to enemies that you teleport through",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.EnergyRegenPerInt += 0.005f,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 21 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = "Stamina Recovery II",
				originalDescription = "Every point of INTELLIGENCE further increases stamina recover by 0.5%.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.ParryIgnites = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 16 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = "Flame Guard",
				originalDescription = "Parry ignites enemies",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.ParryRadius++,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19, 107 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = "Parry range",
				originalDescription = "Increases the radius of parry by 1m",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => StatActions.AddMagicFind(0.1f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { -1 },
				levelReq = 5,
				cost = 1,
				scale = 1f,
				posX = -0.75f,
				posY = -1.1f,
				name = "Luck Enchantment",
				originalDescription = "Increases magic find by 10%. Magic find increases the quantity of items dropped.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => StatActions.AddMagicFind(0.15f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 109 },
				levelReq = 15,
				cost = 2,
				scale = 1f,
				posX = -1.25f,
				posY = -1.85f,
				name = "Luck Enchantment II",
				originalDescription = "Increases magic find by additional 15%. Magic find increases the quantity of items dropped.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.BIA_HealthTakenMult += 0.25f,

				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 67 },
				levelReq = 16,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -3.75f,
				name = "Near death arrow",
				originalDescription = "Blood infused arrow takes 25% more health to convert it to damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.BIA_HealthDmMult += 2f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 19 },
				levelReq = 14,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 2.25f,
				name = "Arcane Blood",
				originalDescription = "Blood infused arrow damage per health is increased by 2 dmg/hp.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => SpellActions.HealingDomeRegEnergy = true,

				category = PerkCategory.Support,
				unlockPath = new int[] { 68 },
				levelReq = 36,
				cost = 1,
				scale = 1,
				posX = -4.5f,
				posY = 1.5f,
				name = "Energy Field",
				originalDescription = "Healing dome regenerates energy",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MaxHealthPercent += 0.1f,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 29 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = "Enchanced vitality",
				originalDescription = "Increases max health by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MaxEnergyPercent += 0.1f,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 80 },
				levelReq = 9,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = "Enchanced energy",
				originalDescription = "Increases max energy by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CraftingReroll = true,

				category = PerkCategory.Utility,
				unlockPath = new int[] { -1 },
				levelReq = 10,
				cost = 1,
				scale = 1f,
				posX = -0.75f,
				posY = 1.1f,
				name = "Rerolling",
				originalDescription = "Opens Crafting Menu in inventory. Allows you to reroll item's properites by placing 2 items of the same rarity as ingredients.",
				textureVariation = 1,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CraftingReforge = true,

				category = PerkCategory.Utility,
				unlockPath = new int[] { 116 },
				levelReq = 25,
				cost = 1,
				scale = 1f,
				posX = -0.25f,
				posY = 1.85f,
				name = "Reforging",
				originalDescription = "Adds a tab to crafting menu. Allows you to reforge an item into any other item of the same tier by placing 3 items of the same or higher rarity as ingredients.",
				textureVariation = 1,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.flashlightIntensity++; ModdedPlayer.instance.flashlightBatteryDrain++; },

				category = PerkCategory.Utility,
				unlockPath = new int[] { 62 },
				levelReq = 20,
				cost = 1,
				scale = 1f,
				posX = -12f,
				posY = -0.75f,
				name = "Light The Way",
				originalDescription = "Flashlight is 100% brighter and lasts 100% longer",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.DamageReductionPerks *= 1.1f; ModdedPlayer.instance.DamageOutputMultPerks *= 1.1f; },

				category = PerkCategory.Defense,
				texture = null,
				unlockPath = new int[] { 5 },
				levelReq = 8,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = "Glass Cannon",
				originalDescription = "Increases all damage taken and increases all damage dealt by 10%",
				textureVariation = 0, //0 or 1
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ProjectileDamageIncreasedBySize = true,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 13 },
				levelReq = 32,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = "Size Matters",
				originalDescription = "Projectile size increases projectile's damage.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BunnyHop = true,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 27 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = "Momentum transfer",
				originalDescription = "Upon landing gain shot movement speed buff",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MeleeRange += 0.04f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 22 },
				levelReq = 4,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = -2.25f,
				name = "Long arm",
				originalDescription = "Increases melee weapon range by 4%",
				textureVariation = 0, //0 or 1
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ChanceToBleedOnHit += 0.02f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 78, 79 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = "Bleed",
				originalDescription = "Hitting an enemy has 2% chance to make them bleed",
				textureVariation = 0, //0 or 1
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ChanceToWeakenOnHit += 0.025f,

				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 71, 22 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -1.5f,
				name = "Weaken",
				originalDescription = "Hitting an enemy has 2.5% chance to make them take more damage",
				textureVariation = 0, //0 or 1
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearCritChance += 0.36f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 64 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = -1.5f,
				name = "Javelin",
				originalDescription = "Spear has increased headshot chance to 40%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearCritChance += 0.1f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 125 },
				levelReq = 33,
				cost = 1,
				scale = 1,
				posX = 0f,
				posY = -2.25f,
				name = "Spear gamble",
				originalDescription = "Spear has increased headshot chance to 50%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearhellChance += 0.45f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 126 },
				levelReq = 34,
				cost = 2,
				scale = 1,
				posX = -1f,
				posY = -2.25f,
				name = "Double spears",
				originalDescription = "When a spear hits a target, it has a 30% chance summon another spear and launch it at the enemy",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearhellChance += 0.02f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 127 },
				levelReq = 53,
				cost = 1,
				scale = 1,
				posX = -2f,
				posY = -2.25f,
				name = "Spearinfinity",
				originalDescription = "Increases the chance of doublespears by 2%",
				textureVariation = 0, //0 or 1
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearDamageMult *= 1.75f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 126 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = -0.5f,
				posY = -3f,
				name = "Spear Mastery",
				originalDescription = "Increases spear damage by 75%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BulletCritChance += 0.2f,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 65 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -2.25f,
				name = "Deadeye",
				originalDescription = "Increases headshot chance of pistol's bullets by 20%, to a total of 30%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.SpearArmorRedBonus = true,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 127, 129 },
				levelReq = 35,
				cost = 2,
				scale = 1,
				posX = -1.5f,
				posY = -3f,
				name = "Piercing",
				originalDescription = "Spear armor reduction from ranged is increased to 150%, additionally, thrown spears also reduce armor equal to melee armor reduction",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BunnyHopUpgrade = true,

				category = PerkCategory.Utility,
				texture = null,
				unlockPath = new int[] { 121 },
				levelReq = 55,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = "Bunny hopping",
				originalDescription = "Increases the speed and duration of the buff",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AttackSpeedMult *= 1.65f; ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0.9f; ModdedPlayer.instance.RangedDamageAmplifier_Mult /= 2; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 7f,
				posY = -0.75f,
				name = "Curse of Quickening",
				originalDescription = "Increases attack speed by 65%, but decreases melee damage by 10% and ranged damage by 50%",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AttackSpeedMult *= 0.5f; ModdedPlayer.instance.MeleeDamageAmplifier_Add += 1.5f; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8f,
				posY = -0.75f,
				name = "Curse of Strengthening",
				originalDescription = "Decreases attack speed by 50%, but greatly increases melee damage by 150%",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Add += 2f; ModdedPlayer.instance.RangedDamageAmplifier_Mult *= 0; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 7.5f,
				posY = 0f,
				name = "Curse of Binding",
				originalDescription = "Makes you unable to damage enemies with ranged weapons, causing all of them to deal 0 damage, but at the same time, you deal 200% increased melee damage",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.2f; ModdedPlayer.instance.AttackSpeedMult *= 1.2f; },
				category = PerkCategory.MeleeOffense,
				texture = null,
				unlockPath = new int[] { 79 },
				levelReq = 60,
				cost = 2,
				scale = 1,
				posX = 2f,
				posY = 2.25f,
				name = "Melee Mastery",
				originalDescription = "Increases melee weapon damage and attack speed by 20%",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.RangedDamageAmplifier_Add += 1.5f; ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8.5f,
				posY = -1.5f,
				name = "Curse of Binding",
				originalDescription = "Makes you unable to damage enemies with melee weapons, causing all of them to deal 0 damage, but at the same time, you deal 150% increased ranged damage",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.RangedDamageAmplifier_Add += 1.2f; ModdedPlayer.instance.MoveSpeedMult *= 0.8f; ModdedPlayer.instance.JumpPower *= 0.7f; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8f,
				posY = -0.75f,
				name = "Curse of Crippling",
				originalDescription = "You become more deadly but less precise.\nYour ranged damage is increased by 120%, but you loose 20% movement speed and 30% jump power.",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AttackSpeedMult *= 1.65f; ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0.1f; ModdedPlayer.instance.RangedDamageAmplifier_Mult /= 4; },
				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9f,
				posY = -0.75f,
				name = "Curse of Quickening",
				originalDescription = "Increases attack speed by 65%, but decreases melee damage by 90% and ranged damage by 25%",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};

			new Perk()
			{
				apply = () => { ModdedPlayer.instance.MaxEnergyPercent *= 0.5f; ModdedPlayer.instance.StaminaRegenPercent -= 0.5f; ModdedPlayer.instance.SpellDamageAmplifier_Add += 1f; },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9f,
				posY = -0.75f,
				name = "Curse of Exhaustion",
				originalDescription = "Increases attack spell damage by 100%, but your energy is reduced by 50% and stamina regenerates slower",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.AttackSpeedMult *= 0.6f; ModdedPlayer.instance.CoolDownMultipier *= 0.65f; },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 8.5f,
				posY = 0f,
				name = "Curse of Speed",
				originalDescription = "Cooldown reduction increased by 35%, but attack speed decreased by 40%",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0.4f; ModdedPlayer.instance.RangedDamageAmplifier_Mult *= 0.4f; ModdedPlayer.instance.SpellDamageAmplifier_Add += 1f; },
				category = PerkCategory.MagicOffense,
				texture = null,
				unlockPath = new int[] { 89 },
				levelReq = 77,
				cost = 2,
				scale = 1,
				posX = 9.5f,
				posY = 0f,
				name = "Curse of Power",
				originalDescription = "Magic damage is increased by 100%, but ranged and melee are weaker by 60% ",
				textureVariation = 1, //0 or 1
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.ParryDmgBonus += 1.5f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 107 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = "Counter Strike",
				originalDescription = "When parrying, gain attack dmg for the next attack. Bonus melee damage is equial to damage of parry. This effect can stack, lasts 20 seconds, and is consumed upon performing a melee attack.",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.5f,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 11 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = -1.5f,
				name = "Skull Basher",
				originalDescription = "When bash is equipped, melee weapons deal 50% more damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.6f,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 144 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 1f,
				posY = -2.25f,
				name = "Skull Basher II",
				originalDescription = "When bash is equipped, melee weapons deal 110% more damage",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => ModdedPlayer.instance.DanceOfFiregod = true,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 71 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = "Dance of the Firegod",
				originalDescription = "When black flame is on, your melee damage is increased, based on how fast youre going.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.StaminaOnHit += 3,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 9 },
				levelReq = 5,
				cost = 1,
				scale = 1,
				posX = 1.5f,
				posY = 1.5f,
				name = "Combat Regen",
				originalDescription = "Gain 3 points of stamina on hit",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => SpellActions.FrenzyMS = true,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 102 },
				levelReq = 26,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -0.75f,
				name = "Mania",
				originalDescription = "Frenzy increases movement speed by 5% per stack",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.FurySwipes = true,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 148 },
				levelReq = 43,
				cost = 1,
				scale = 1,
				posX = 6f,
				posY = -0.75f,
				name = "Fury Swipes",
				originalDescription = "When during frenzy you hit the same enemy over and over, gain more and more damage. Melee stacks 6 times faster.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.BashDamageBuff++,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 103 },
				levelReq = 27,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = -1.5f,
				name = "Lucky Bashes",
				originalDescription = "When you bash an enemy, gain 15% critical hit damage for 2 seconds.",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.MaxLogs++,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 61 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = -11f,
				posY = -0.75f,
				name = "More Carried Logs",
				originalDescription = "Increases the base amount of logs that a player can carry on their shoulder. The additional carried logs are invisible. Buggy in multiplayer",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.ProjectileDamageIncreasedBySpeed = true,

				category = PerkCategory.RangedOffense,
				texture = null,
				unlockPath = new int[] { 14 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 0f,
				name = "Speed Matters",
				originalDescription = "Projectile speed increases projectile's crit damage.",
				textureVariation = 0, //0 or 1
				uncapped = false,
			};

			new Perk()
			{
				apply = () => SpellActions.MagicArrowCrit = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 73 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = -3f,
				name = "Magic Arrow Devastation",
				originalDescription = "Magic arrow can critically hit.",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => SpellActions.BL_Crit = true,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 88 },
				levelReq = 62,
				cost = 2,
				scale = 1,
				posX = 1.5f,
				posY = 6f,
				name = "Nuke Conjuration",
				originalDescription = "Ball Lightning can critically hit.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.BlinkRange += 10f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 105 },
				levelReq = 40,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 3.75f,
				name = "Blink - Wormhole",
				originalDescription = "Blink has 66.6% increased distance",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.FireAmp += 0.1f,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 90 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = -2.5f,
				posY = 0f,
				name = "Fiery Embrace",
				originalDescription = "Fire damage is increased by 10%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { SpellActions.BIA_TripleDmg = true; ModdedPlayer.instance.HealingMultipier *= 0.5f; },
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 111 },
				levelReq = 55,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = -3.75f,
				name = "Cursed Arrow",
				originalDescription = "Blood infused arrow deals triple damage, but healing recieved is halved, and you loose energy for a short time after casting the spell",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.BIA_Weaken = true,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 111 },
				levelReq = 30,
				cost = 2,
				scale = 1,
				posX = 1.5f,
				posY = -4.5f,
				name = "Deep Wounds",
				originalDescription = "Enemies hit by blood infused arrow take 100% increased damage from all sources for 15 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.EnergyOnHit += 1f; ModdedPlayer.instance.LifeOnHit += 1.5f; },
				category = PerkCategory.Support,
				unlockPath = new int[] { 44, 43 },
				levelReq = 47,
				cost = 1,
				scale = 1,
				posX = -3.5f,
				posY = -1.5f,
				name = "Rejuvenation",
				originalDescription = "Gain +1 energy on hit, and +1.5 life per hit.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { SpellDataBase.spellDictionary[10].Cooldown -= 15; },
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 153, 75 },
				levelReq = 47,
				cost = 2,
				scale = 1,
				posX = 4.5f,
				posY = -3f,
				name = "Endless stream",
				originalDescription = "Reduce the cooldown of Magic Arrow by 15 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { SpellActions.ParryRadius++; ModdedPlayer.instance.ParryAnything = true; },
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 78 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = 0.75f,
				name = "Parry Mastery",
				originalDescription = "Increases the radius of Parry by 1m, allows you to parry any type of enemy.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.thornsPerStrenght += 1.2f,
				category = PerkCategory.Defense,
				unlockPath = new int[] { 31 },
				levelReq = 3,
				cost = 1,
				scale = 1,
				posX = 3f,
				posY = 0.75f,
				name = "Thorny Skin",
				originalDescription = "Every point of strength increases thorns by 1.2\nThorns scale with melee damage multipier stats",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.thornsMult *= 2; ModdedPlayer.instance.Armor += 400; },
				category = PerkCategory.Defense,
				unlockPath = new int[] { 32 },
				levelReq = 25,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 0f,
				name = "Iron Maiden",
				originalDescription = "Increases armor by 400, and increases thorns damage by 100%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.MagicResistance, 0.2f),

				category = PerkCategory.Defense,
				unlockPath = new int[] { 35, 32 },
				levelReq = 24,
				cost = 1,
				scale = 1,
				posX = 4f,
				posY = -0.75f,
				name = "Anti-Magic Training",
				originalDescription = "Decreases magic damage taken by 20%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BlackholePullImmune = true,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 164 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = -1.5f,
				name = "Dense Matter",
				originalDescription = "Black holes cannot suck you in",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.BlizzardSlowReduced = true,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 164 },
				levelReq = 30,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -2.25f,
				name = "Warmth",
				originalDescription = "Blizzard slow effect greatly reduced",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.TrueAim = true,
				category = PerkCategory.RangedOffense,
				unlockPath = new int[] { 54 },
				unlockRequirement = new int[] { 67 },
				levelReq = 70,
				cost = 1,
				scale = 1,
				posX = 5.5f,
				posY = 0f,
				name = "True Aim",
				originalDescription = "Arrow headshots which hit enemies over 60 m away and are not affected by seeking arrow hit enemies twice, and deal 4x damage",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.thorns += 50,

				category = PerkCategory.Defense,
				unlockPath = new int[] { 162 },
				levelReq = 15,
				cost = 1,
				scale = 1,
				posX = 3.5f,
				posY = 1.5f,
				name = "Spikes",
				originalDescription = "Adds 50 thorns",
				textureVariation = 0,
				uncapped = false,
			};

			new Perk()
			{
				apply = () => SpellDataBase.spellDictionary[16].Cooldown -= 60,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 154 },
				levelReq = 80,
				cost = 1,
				scale = 1,
				posX = 2f,
				posY = 6.75f,
				name = "Storm Season",
				originalDescription = "Ball Lightning has its cooldown reduced by 60 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellDataBase.spellDictionary[16].Cooldown -= 20,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 169 },
				levelReq = 120,
				cost = 2,
				scale = 1,
				posX = 3f,
				posY = 6.75f,
				name = "Endless Storm",
				originalDescription = "Ball Lightning has its cooldown reduced by 20 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellDataBase.spellDictionary[3].Cooldown -= 7,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 155 },
				levelReq = 66,
				cost = 2,
				scale = 1,
				posX = 5f,
				posY = 3.75f,
				name = "Quick Silver",
				originalDescription = "Blink has it's cooldown reduced by 7 seconds",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellDataBase.spellDictionary[4].Cooldown -= 20,

				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 85 },
				levelReq = 44,
				cost = 2,
				scale = 1,
				posX = 3.5f,
				posY = 3f,
				name = "Wrath of the Sun",
				originalDescription = "Sun Flare cooldown is reduced by 20",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.ParryDmgBonus += 2f,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 143 },
				levelReq = 20,
				cost = 1,
				scale = 1,
				posX = 4.5f,
				posY = 1.5f,
				name = "Full Counter",
				originalDescription = "Increase the damage bonus from parrying",
				textureVariation = 0,
				uncapped = true,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.GoldenResolve = true,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 87 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 2.5f,
				posY = 4.5f,
				name = "Golden Resolve",
				originalDescription = "Gold reduces damage taken by 50%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellDataBase.spellDictionary[4].Cooldown -= 90,
				category = PerkCategory.MagicOffense,
				unlockPath = new int[] { 87 },
				levelReq = 50,
				cost = 1,
				scale = 1,
				posX = 0.5f,
				posY = 4.5f,
				name = "Sudden Rampage",
				originalDescription = "Cooldown of Berserk is decreased by 90",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => SpellActions.HealingDomeDuration += 50,
				category = PerkCategory.Support,
				unlockPath = new int[] { 113 },
				levelReq = 44,
				cost = 1,
				scale = 1,
				posX = -5.5f,
				posY = 1.5f,
				name = "Safe Heaven",
				originalDescription = "Healing dome lasts a minute.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { SpellDataBase.spellDictionary[2].Cooldown -= 35; SpellDataBase.spellDictionary[13].Cooldown -= 7.5f; },
				category = PerkCategory.Support,
				unlockPath = new int[] { 176 },
				levelReq = 58,
				cost = 1,
				scale = 1,
				posX = -6.5f,
				posY = 1.5f,
				name = "Time of Need",
				originalDescription = "The cooldown of healing dome and purge is reduced by 50%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.DanceOfFiregodAtkCap = true,
				category = PerkCategory.MeleeOffense,
				unlockPath = new int[] { 146 },
				levelReq = 46,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = -0.75f,
				name = "Breathing Tehniques",
				originalDescription = "When black flame is on, your attack speed is fixed at 100%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CraftingPolishing = true,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 116 },
				levelReq = 35,
				cost = 1,
				scale = 1f,
				posX = -1.25f,
				posY = 1.85f,
				name = "Polishing",
				originalDescription = "Adds a tab to crafting menu. Allows you to change the value of a single stat into either higher or not change it at all. Requires one item of the same rarity or greater",
				textureVariation = 1,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.CraftingEmpowering = true,
				category = PerkCategory.Utility,
				unlockPath = new int[] { 179, 117 },
				unlockRequirement = new int[] { 179, 117 },
				levelReq = 55,
				cost = 1,
				scale = 1.5f,
				posX = -0.75f,
				posY = 2.85f,
				name = "Empowering",
				originalDescription = "Adds a tab to crafting menu. Allows you to change the level of an item to player's current level, without rerolling values. Requires nine items of the same or higher rarity",
				textureVariation = 1,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => StatActions.AddMagicFind(0.15f),

				category = PerkCategory.Utility,
				unlockPath = new int[] { 109 },
				levelReq = 55,
				cost = 2,
				scale = 1f,
				posX = -1.75f,
				posY = -2.6f,
				name = "Luck Enchantment III",
				originalDescription = "Increases magic find by additional 15%. Magic find increases the quantity of items dropped.",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => { ModdedPlayer.instance.DamageReductionPerks *= 0.95f; ModdedPlayer.instance.Armor += 500; },
				category = PerkCategory.Defense,
				unlockPath = new int[] { 163, 34 },
				levelReq = 45,
				cost = 1,
				scale = 1,
				posX = 5f,
				posY = 0.75f,
				name = "Heavy metal",
				originalDescription = "Increases armor by 500, reduces damage taken by 5%",
				textureVariation = 0,
				uncapped = false,
			};
			new Perk()
			{
				apply = () => ModdedPlayer.instance.isShieldAutocast = true,
				category = PerkCategory.Defense,
				unlockPath = new int[] { 104, 34 },
				levelReq = 10,
				cost = 1,
				scale = 1,
				posX = 5.5f,
				posY = 1.5f,
				name = "Autocast Shield",
				originalDescription = "When your energy and stamina is above 90% of max, and you have Sustain Shield spell equipped, the spell is automatically cast",
				textureVariation = 0,
				uncapped = false,
			};
		






			foreach (var item in perks)
			{
				item.Description = item.originalDescription;
			}
		}
	}
}