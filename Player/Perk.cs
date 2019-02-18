using ChampionsOfForest.Effects;
using ChampionsOfForest.Items;
using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public class Perk
    {
        public static List<Perk> AllPerks = new List<Perk>();
        public static void FillPerkList()
        {
            AllPerks.Clear();
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamagePerStrenght += 0.005f,
                DisableMethods = () => ModdedPlayer.instance.DamagePerStrenght -= 0.005f,
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stronger Hits",
                Description = "Gene allows muscules to quickly change their structure to a more efficient one.\nEvery point of STRENGHT increases MEELE DAMAGE by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellDamageperInt += 0.005f,
                DisableMethods = () => ModdedPlayer.instance.SpellDamageperInt -= 0.005f,
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stronger Spells",
                Description = "Gene changes the composition of axon sheath that greatly increases brain's power.\nEvery point of INTELLIGENCE increases SPELL DAMAGE by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.RangedDamageperAgi += 0.005f,
                DisableMethods = () => ModdedPlayer.instance.RangedDamageperAgi -= 0.005f,
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stronger Projectiles",
                Description = "Neural connections between muscules and the brain are now a lot more sensitive. Your movements become a lot more precise.\nEvery point of AGILITY increases RANGED DAMAGE by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyRegenPerInt += 0.005f,
                DisableMethods = () => ModdedPlayer.instance.EnergyRegenPerInt -= 0.005f,
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stamina Recovery",
                Description = "Heart's muscules become even more resistant to exhaustion.\nEvery point of INTELLIGENCE increases stamina recover by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyPerAgility += 0.5f,
                DisableMethods = () => ModdedPlayer.instance.EnergyPerAgility -= 0.5f,
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 0,
                Name = "More Stamina",
                Description = "Hemoglobin is replaced with an alternative substance capable of carrying more oxygen.\nEvery point of AGILITY increases max stamina by 0.5",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealthPerVitality += 1f,
                DisableMethods = () => ModdedPlayer.instance.HealthPerVitality -= 1f,
                Category = PerkCategory.Defense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "More Health",
                Description = "Skin and bones become more resisitant to injuries.\nEvery point of VITALITY increases max health by 1",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealingMultipier *= 1.10f,
                DisableMethods = () => ModdedPlayer.instance.HealingMultipier /= 1.10f,
                Category = PerkCategory.Support,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 0,
                Name = "More Healing",
                Description = "Blood becomes denser, is less vunerable to bleeding and wounds are healed faster.\nIncreases all healing by 10%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HungerRate *= 0.96f,
                DisableMethods = () => ModdedPlayer.instance.HungerRate /= 0.96f,
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 4 },
                LevelRequirement = 5,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = 0.75f,
                Name = "Metabolism",
                Description = "Additional microorganisms are now present in the digestive system that allow to feed of previousely undigested food.\nDecreases hunger rate by 4%.",
                TextureVariation = 1, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ThirstRate *= 0.96f,
                DisableMethods = () => ModdedPlayer.instance.ThirstRate /= 0.96f,
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 4 },
                LevelRequirement = 5,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = -0.75f,
                Name = "Water Conservation",
                Description = "Sweating is decreased, kidneys keep more water.\nDecreases thirst rate by 4%.",
                TextureVariation = 1, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeDamageBonus += 5,
                DisableMethods = () => ModdedPlayer.instance.MeleeDamageBonus -= 5,
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 0, 10 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Damage",
                Description = "Grip strenght increases by 1 kg.\nIncreases melee damage by 5",
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.02f,
                DisableMethods = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add -= 0.02f,
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 0, 9, 11 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Damage",
                Description = "Biceps slightly increases in size.\nIncreases melee damage by 2%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.strenght += 10,
                DisableMethods = () => ModdedPlayer.instance.strenght -= 10,
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 0, 10 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Strenght",
                Description = "All flexors gain in size.\nIncreases strenght by 10",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.RangedDamageBonus += 5,
                DisableMethods = () => ModdedPlayer.instance.MeleeDamageBonus -= 5,
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 2 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Damage",
                Description = "Shoulder muscules grow.\nIncreases projectile damage by 5",
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ProjectileSizeRatio += 0.05f,
                DisableMethods = () => ModdedPlayer.instance.ProjectileSizeRatio -= 0.05f,
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 2 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Size",
                Description = "Increased overall physical strenght allows for precise shoots from hand held weapons with bigger ammunition.\nIncreases projectile size by 5%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ProjectileSpeedRatio += 0.05f,
                DisableMethods = () => ModdedPlayer.instance.ProjectileSpeedRatio -= 0.05f,
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 2 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Speed",
                Description = "Increased overall physical strenght allows for stronger drawing of ranged weaponry.\nIncreases projectile speed by 5%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, 0.05f),
                DisableMethods = () => ItemDataBase.RemovePercentage(ref ModdedPlayer.instance.SpellCostToStamina, 0.05f),
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 1 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Transmutation",
                Description = "The costs of casting spells become easier to quicly recover from.\n5% of the spell cost is now taxed from stamina instead of energy.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellCostRatio *= 1 - 0.025f,
                DisableMethods = () => ModdedPlayer.instance.SpellCostRatio /= 1 - 0.025f,
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 15 },
                LevelRequirement = 7,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Resource Cost Reduction",
                Description = "In order for the organism to preserve energy, spell costs are reduced by 2,5%",
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            //0.7 perks
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.DamageReductionPerks *= 0.80f; ModdedPlayer.instance.DamageOutputMultPerks *= 0.80f; },
                DisableMethods = () => { ModdedPlayer.instance.DamageReductionPerks /= 0.80f; ModdedPlayer.instance.DamageOutputMultPerks /= 0.80f; },
                Category = PerkCategory.Defense,
                Icon = null,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Undestructable",
                Description = "Decreases all damage taken and decreases all damage dealt by 20%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellCostRatio *= 0.98f,
                DisableMethods = () => ModdedPlayer.instance.SpellCostRatio /= 0.98f,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 16 },
                LevelRequirement = 7,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 1.5f,
                Name = "Cool Down Reduction",
                Description = " Reduces spell cooldown by 2%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellCostRatio *= 0.925f,
                DisableMethods = () => ModdedPlayer.instance.SpellCostRatio /= 0.925f,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 18 },
                LevelRequirement = 8,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 2.25f,
                Name = "Greater Cool Down Reduction",
                Description = " Reduces spell cooldown by 7,5%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAllStats(5),
                DisableMethods = () => StatActions.RemoveAllStats(5),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 3 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "All attributes",
                Description = "+5 to every strenght, agility, vitality and intelligence",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAllStats(10),
                DisableMethods = () => StatActions.RemoveAllStats(10),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 20 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "All attributes",
                Description = "+10 to every strenght, agility, vitality and intelligence",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAttackSpeed(0.02f),
                DisableMethods = () => StatActions.AddAttackSpeed(0.02f),
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 11 },
                LevelRequirement = 6,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = -1.5f,
                Name = "Attack speed",
                Description = "+2% to attack speed ",
                TextureVariation = 0,
                Endless = true,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.02f,
                DisableMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.02f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12, 14 },
                LevelRequirement = 7,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Reusability I",
                Description = "+2% chance on hit to get 1 arrow.",
                TextureVariation = 0,
                Endless = true,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.03f,
                DisableMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.03f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 23 },
                LevelRequirement = 9,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = -1.5f,
                Name = "Reusability II",
                Description = "+3% chance on hit to get 1 arrow.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.1f,
                DisableMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.1f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 24 },
                LevelRequirement = 12,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -0.75f,
                Name = "Reusability III",
                Description = "+10% chance on hit to get 1 arrow.",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAllStats(10),
                DisableMethods = () => StatActions.RemoveAllStats(10),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 21 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 0.75f,
                Name = "All attributes",
                Description = "+5 to every strenght, agility, vitality and intelligence",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.JumpPower += 0.05f,
                DisableMethods = () => ModdedPlayer.instance.JumpPower -= 0.05f,
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 3 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Jump",
                Description = "Increases jump height by 5%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MoveSpeed += 0.015f,
                DisableMethods = () => ModdedPlayer.instance.MoveSpeed -= 0.015f,
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 27 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 1.5f,
                Name = "Light foot",
                Description = "Increases movement speed by 1.5%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealthBonus += 10,
                DisableMethods = () => ModdedPlayer.instance.HealthBonus -= 10,
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Bonus Health",
                Description = "Increases health by 10. This is further multipied by maximum health percent.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.LifeRegen += 0.5f,
                DisableMethods = () => ModdedPlayer.instance.LifeRegen -= 0.5f,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 6 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 0f,
                Name = "Health Regen",
                Description = "Increases health per second regeneration by 0.5. This is further multipied by health regen percent and all healing percent.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.Armor += 10,
                DisableMethods = () => ModdedPlayer.instance.Armor -= 10,
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Bonus Armor",
                Description = "Increases armor by 10.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,
                DisableMethods = () => ModdedPlayer.instance.DamageReductionPerks /= 0.9f,
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 31 },
                LevelRequirement = 20,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "Durability",
                Description = "Decreases all damage taken by 10%.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,
                DisableMethods = () => ModdedPlayer.instance.DamageReductionPerks /= 0.9f,
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 32 },
                LevelRequirement = 35,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 0.75f,
                Name = "Durability II",
                Description = "Further decreases all damage taken by 10%.",
                TextureVariation = 0,
                Endless = false,
            };


            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,
                DisableMethods = () => ModdedPlayer.instance.DamageReductionPerks /= 0.9f,
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 33 },
                LevelRequirement = 40,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = 1.5f,
                Name = "Durability III",
                Description = "Further decreases all damage taken by 10%.",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.MagicResistance, 0.02f),
                DisableMethods = () => ItemDataBase.RemovePercentage(ref ModdedPlayer.instance.MagicResistance, 0.02f),
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 29, 31 },
                LevelRequirement = 6,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Magic Resistance",
                Description = "Decreases magic damage taken by 2%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.DodgeChance, 0.15f),
                DisableMethods = () => ItemDataBase.RemovePercentage(ref ModdedPlayer.instance.DodgeChance, 0.15f),
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 34 },
                LevelRequirement = 50,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = 0.75f,
                Name = "Dodge",
                Description = "Increases dodge chance by 15%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ARreduction_all += 3,
                DisableMethods = () => ModdedPlayer.instance.ARreduction_all -= 3,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 6 },
                LevelRequirement = 6,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = 0.75f,
                Name = "Armor Penetration",
                Description = "Increases armor penetration from all sources by 3",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ARreduction_melee += 3,
                DisableMethods = () => ModdedPlayer.instance.ARreduction_melee -= 3,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 37 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 1.5f,
                Name = "Armor Piercing Edge",
                Description = "Increases armor penetration from melee weapons by 3",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ARreduction_ranged += 3,
                DisableMethods = () => ModdedPlayer.instance.ARreduction_ranged -= 3,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 37 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 1.5f,
                Name = "Anti armor projectiles",
                Description = "Increases armor penetration from ranged weapons by 3",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealthRegenPercent += 0.1f,
                DisableMethods = () => ModdedPlayer.instance.HealthRegenPercent -= 0.1f,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 30 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3.5f,
                PosOffsetY = 0f,
                Name = "More Health Regen",
                Description = "Passive health regeneration is increased by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyPerSecond += 0.15f,
                DisableMethods = () => ModdedPlayer.instance.EnergyPerSecond -= 0.15f,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 30 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3f,
                PosOffsetY = -0.75f,
                Name = "Energy generation",
                Description = "Passive energy regeneration is increased by 0.15/s",
                TextureVariation = 0,
                Endless = true,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ExpFactor *= 1.1f,
                DisableMethods = () => ModdedPlayer.instance.ExpFactor /= 1.1f,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 40 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 0f,
                Name = "Insight",
                Description = "All experience gained increased by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.LifeOnHit += 1f,
                DisableMethods = () => ModdedPlayer.instance.LifeOnHit -= 1f,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 40 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = -0.75f,
                Name = "Life On Hit",
                Description = "Life on hit increased by 1",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyOnHit += 0.25f,
                DisableMethods = () => ModdedPlayer.instance.EnergyOnHit -= 0.25f,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 41 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = -1.5f,
                Name = "Energy On Hit",
                Description = "Energy on hit increased by 0.25",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(33, 2),
                DisableMethods = () => ModdedPlayer.instance.AddGeneratedResource(33, -2),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 4 },
                LevelRequirement = 12,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 0,
                Name = "Alternative cloth sources",
                Description = "Increases daily generation of cloth by 2",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(29, 1),
                DisableMethods = () => ModdedPlayer.instance.AddGeneratedResource(29, -1),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3.5f,
                PosOffsetY = 0,
                Name = "Demolition",
                Description = "Increases daily generation of bombs by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(29, 10); ModdedPlayer.instance.AddExtraItemCapacity(175, 10); },
                DisableMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(29, -10); ModdedPlayer.instance.AddExtraItemCapacity(175, -10); },
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 46 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = 0.75f,
                Name = "Pockets for explosives",
                Description = "Increases max amount of carried bombs and dynamite by 10",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(175, 1),
                DisableMethods = () => ModdedPlayer.instance.AddGeneratedResource(175, -1),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 47 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 1.5f,
                Name = "Demolition Expert",
                Description = "Increases daily generation of dynamite by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(49, 1),
                DisableMethods = () => ModdedPlayer.instance.AddGeneratedResource(49, -1),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 0,
                Name = "Meds",
                Description = "Increases daily generation of meds by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(262, 1),
                DisableMethods = () => ModdedPlayer.instance.AddGeneratedResource(262, -1),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -6.5f,
                PosOffsetY = 0,
                Name = "Fuel",
                Description = "Increases daily generation of fuel cans by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(37, 1),
                DisableMethods = () => ModdedPlayer.instance.AddGeneratedResource(37, -1),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -5.5f,
                PosOffsetY = 0,
                Name = "Booze",
                Description = "Increases daily generation of booze by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(37, 15),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(37, -15),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 51 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -6f,
                PosOffsetY = -0.75f,
                Name = "More Booze",
                Description = "Increases max amount of carried booze by 15",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(49, 15),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(49, -15),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 49 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = -0.75f,
                Name = "More Meds",
                Description = "Increases max amount of carried meds by 15",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityAmount++,
                DisableMethods = () => ModdedPlayer.instance.ReusabilityAmount--,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 25 },
                LevelRequirement = 45,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -0.75f,
                Name = "More Projectiles On Hit",
                Description = "Increases max amount of projectiles recovered using reusability perks by 1",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellDamageBonus += 5,
                DisableMethods = () => ModdedPlayer.instance.SpellDamageBonus -= 5,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 1 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Spell Power",
                Description = "Increases spell damage by 5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 76, 35, 123, 207, 127 }, 5),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 76, 35, 123, 207, 127 }, -5),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -7.5f,
                PosOffsetY = 0,
                Name = "More Meat",
                Description = "Increases carry amount of all meats by 5",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 109, 89 }, 20),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 109, 89 }, -20),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -8.5f,
                PosOffsetY = 0,
                Name = "More Snacks",
                Description = "Increases carry amount of candy bars and sodas by 20",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(307, 20),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(307, -20),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -13.5f,
                PosOffsetY = 0,
                Name = "More Bolts",
                Description = "Increases carry amount of crossbow bolts by 20",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(94, 20); ModdedPlayer.instance.AddExtraItemCapacity(178, 100); },
                DisableMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(94, -20); ModdedPlayer.instance.AddExtraItemCapacity(178, -100); },
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -9.5f,
                PosOffsetY = 0,
                Name = "Corpse collecting",
                Description = "Increases carry amount of bones by 100 and skulls by 20",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 90, 47, 46, 101 }, 10),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 90, 47, 46, 101 }, -10),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 59 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -10f,
                PosOffsetY = 0.75f,
                Name = "More Limbs",
                Description = "Increases carry amount of arms, legs, heads and headbombs by 10",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(57, 3); ModdedPlayer.instance.AddExtraItemCapacity(53, 2); ModdedPlayer.instance.AddExtraItemCapacity(54, 1); },
                DisableMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(57, -3); ModdedPlayer.instance.AddExtraItemCapacity(53, -2); ModdedPlayer.instance.AddExtraItemCapacity(54, -1); },
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -10.5f,
                PosOffsetY = 0,
                Name = "More Building Resources",
                Description = "Increases carry amount of sticks by 3, rocks by 2 and ropes by 1",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 31, 142, 141, 41, 43, 144 }, 5),
                DisableMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 31, 142, 141, 41, 43, 144 }, -5),
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -11.5f,
                PosOffsetY = 0,
                Name = "More Misceleanous Items",
                Description = "Increases carry amount of pots, turtle shells, watches, circuit boards, air carnisters and flares by 5",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 177, 71, 56 }, 5); ModdedPlayer.instance.AddExtraItemCapacity(82, 50); },
                DisableMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 177, 71, 56 }, -5); ModdedPlayer.instance.AddExtraItemCapacity(82, -50); },
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -12.5f,
                PosOffsetY = 0,
                Name = "More Ammo",
                Description = "Increases carry amount of weak and upgraded spears and molotovs by 5, small rocks by 50",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearDamageMult *= 1.4f,
                DisableMethods = () => ModdedPlayer.instance.SpearDamageMult /= 1.4f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Spear Specialization",
                Description = "Thrown spears deal 40% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BulletDamageMult *= 1.4f,
                DisableMethods = () => ModdedPlayer.instance.BulletDamageMult /= 1.4f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = -2.25f,
                Name = "Pistol Specialization",
                Description = "Bullets deal 40% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CrossbowDamageMult *= 1.4f,
                DisableMethods = () => ModdedPlayer.instance.CrossbowDamageMult /= 1.4f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = -3f,
                Name = "Crossbow Specialization",
                Description = "Bolts deal 40% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BowDamageMult *= 1.4f,
                DisableMethods = () => ModdedPlayer.instance.BowDamageMult /= 1.4f,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0f,
                PosOffsetY = -3.75f,
                Name = "Bow Specialization",
                Description = "Arrows deal 40% more damage",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => SpellActions.HealingDomeGivesImmunity = true,
                DisableMethods = () => SpellActions.HealingDomeGivesImmunity = false,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 40 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = 0.75f,
                Name = "Sanctuary",
                Description = "Healing dome provides immunity to root like effects",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => BlackFlame.GiveDamageBuff = true,
                DisableMethods = () => BlackFlame.GiveDamageBuff = false,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Enchant weapon",
                Description = "While black flame is on, melee damage is increased by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.WarCryGiveDamage = true,
                DisableMethods = () => SpellActions.WarCryGiveDamage = false,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = -1.5f,
                Name = "Empowered War Cry",
                Description = "Warcry additionally increases all damage by 10%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 1.04f; ModdedPlayer.instance.StaminaAttackCost *= 1.05f; },
                DisableMethods = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Mult /= 1.04f; ModdedPlayer.instance.StaminaAttackCost /= 1.05f; },
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 11, 10 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Power Swing",
                Description = "Attacks use 5% more stamina and deal 4% more damage",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.SpellDamageAmplifier_Mult *= 1.04f; ModdedPlayer.instance.SpellCostRatio *= 1.05f; },
                DisableMethods = () => { ModdedPlayer.instance.SpellDamageAmplifier_Mult /= 1.04f; ModdedPlayer.instance.SpellCostRatio /= 1.05f; },
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 15, 55 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Overcharge",
                Description = "Spell damage is increased by 5%, spell costs are increased by 5%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowDmgDebuff = true,
                DisableMethods = () => SpellActions.MagicArrowDmgDebuff = false,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -2.25f,
                Name = "Exposure",
                Description = "Magic arrow causes hit enemies to take 15% more damage for the duration of the slow.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowDuration += 3,
                DisableMethods = () => SpellActions.MagicArrowDuration -= 3,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 73 },
                LevelRequirement = 40,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -2.25f,
                Name = "Disabler",
                Description = "Magic arrow's negative effects last additional 3 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowDoubleSlow = true,
                DisableMethods = () => SpellActions.MagicArrowDoubleSlow = false,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 74 },
                LevelRequirement = 46,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -2.25f,
                Name = "Magic Binding",
                Description = "Magic arrow's slow amount is doubled. It's upgraded from 35% to 70%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HeavyAttackMult *= 1.5f,
                DisableMethods = () => ModdedPlayer.instance.HeavyAttackMult /= 1.5f,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 10 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "Charged Attack",
                Description = "Charged melee attacks deal additional 50%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HeavyAttackMult *= 2f,
                DisableMethods = () => ModdedPlayer.instance.HeavyAttackMult /= 2f,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 76 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = 0f,
                Name = "Super Charged Attack",
                Description = "Charged melee attacks deal 300% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CritDamage += 5,
                DisableMethods = () => ModdedPlayer.instance.CritDamage -= 5,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 9,10 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 0.75f,
                Name = "Lucky Hits",
                Description = "Increases Critical hit damage by 5%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CritChance += 0.15f,
                DisableMethods = () => ModdedPlayer.instance.CritChance -= 0.15f,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 9,78 },
                LevelRequirement = 35,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 1.5f,
                Name = "Overhelming Odds",
                Description = "Increases Critical chance bu 15%.",
                TextureVariation = 0,
                Endless = false,
            };
             new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyBonus += 5,
                DisableMethods = () => ModdedPlayer.instance.EnergyBonus -= 5,
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 3 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Endurance",
                Description = "Increases maximum energy by 5",
                TextureVariation = 0,
                Endless = true,
            };

            
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MultishotCount += 1,
                DisableMethods = () => ModdedPlayer.instance.MultishotCount -= 1,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 31,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = -1.5f,
                Name = "Multishot Empower",
                Description = "Increases the projectile count of multishot, also increases the spells cost.",
                TextureVariation = 0,
                Endless = true,
            };

























        }

        public int ID;
        public int[] InheritIDs;
        public int PointsToBuy = 1;
        public int LevelRequirement;

        public bool IsBought = false;
        public bool Applied = false;

        public delegate void OnApply();
        public delegate void OnDisable();
        public OnApply ApplyMethods;
        public OnDisable DisableMethods;

        public string Name;
        public string Description;

        public Texture2D Icon;

        public bool Endless = false;
        public int ApplyAmount;

        public int TextureVariation = 0;
        public float Size = 1;
        public float PosOffsetX;
        public float PosOffsetY;
        public enum PerkCategory { MeleeOffense, RangedOffense, MagicOffense, Defense, Support, Utility }
        public PerkCategory Category;

        public Perk(string name, string description, int[] inheritIDs, float x, float y, PerkCategory category, float size, int levelRequirement, OnApply applyMethods, OnDisable disableMethods)
        {
            Name = name;
            Description = description;
            InheritIDs = inheritIDs;
            Category = category;
            Size = size;
            Endless = false;
            PosOffsetX = x;
            PosOffsetY = y;
            LevelRequirement = levelRequirement;
            ApplyMethods = applyMethods;
            DisableMethods = disableMethods;
            ID = AllPerks.Count;
            Applied = false;
            AllPerks.Add(this);


        }
        public Perk()
        {
            Applied = false;

            ID = AllPerks.Count;
            AllPerks.Add(this);

        }
        public Perk(string name, string description, int inheritIDs, float x, float y, PerkCategory category, float size, int levelRequirement, OnApply applyMethods, OnDisable disableMethods)
        {
            Name = name;
            Description = description;
            InheritIDs = new int[] { inheritIDs };
            PosOffsetX = x;
            PosOffsetY = y;
            Category = category;
            Size = size;
            Endless = false;
            LevelRequirement = levelRequirement;
            ApplyMethods = applyMethods;
            DisableMethods = disableMethods;
            ID = AllPerks.Count;
            Applied = false;
            AllPerks.Add(this);

        }
    }


}
