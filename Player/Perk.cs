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
                ApplyMethods = () => ModdedPlayer.instance.DamagePerStrength += 0.005f,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stronger Hits",
                _description = "Gene allows muscules to quickly change their structure to a more efficient one.\nEvery point of STRENGHT increases MEELE DAMAGE by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellDamageperInt += 0.005f,
                
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stronger Spells",
                _description = "Gene changes the composition of axon sheath that greatly increases brain's power.\nEvery point of INTELLIGENCE increases SPELL DAMAGE by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.RangedDamageperAgi += 0.005f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stronger Projectiles",
                _description = "Neural connections between muscules and the brain are now a lot more sensitive. Your movements become a lot more precise.\nEvery point of AGILITY increases RANGED DAMAGE by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyRegenPerInt += 0.005f,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "Stamina Recovery",
                _description = "Heart's muscules become even more resistant to exhaustion.\nEvery point of INTELLIGENCE increases stamina recover by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyPerAgility += 0.5f,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 0,
                Name = "More Stamina",
                _description = "Hemoglobin is replaced with an alternative substance capable of carrying more oxygen.\nEvery point of AGILITY increases max stamina by 0.5",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealthPerVitality += 1.5f,
                
                Category = PerkCategory.Defense,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 0,
                Name = "More Health",
                _description = "Skin and bones become more resisitant to injuries.\nEvery point of VITALITY increases max health by 1.5",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealingMultipier *= 1.05f,
                
                Category = PerkCategory.Support,
                Icon = null,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 0,
                Name = "More Healing",
                _description = "Blood becomes denser, is less vunerable to bleeding and wounds are healed faster.\nIncreases all healing by 5%",
                onPucharseDescriptionUpdate = x =>
                {

                    float f = 1.05f;
                    for (int i = 1; i < x; i++)
                        f *= 1.05f;
                    return "\nTotal from this perk: " + (f - 1).ToString("P");
            },
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HungerRate *= 0.9f,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 4 },
                LevelRequirement = 5,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = 0.75f,
                Name = "Metabolism",
                _description = "Additional microorganisms are now present in the digestive system that allow to feed of previousely undigested food.\nDecreases hunger rate by 10%.",
                TextureVariation = 1, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    float f = 0.9f;
                    for (int i = 1; i < x; i++)
                        f *= 0.9f;
                    return "\nTotal from this perk: " + (1-f).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ThirstRate *= 0.9f,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 4 },
                LevelRequirement = 5,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = -0.75f,
                Name = "Water Conservation",
                _description = "Sweating is decreased, kidneys keep more water.\nDecreases thirst rate by 10%.",
                TextureVariation = 1, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    float f = 0.9f;
                    for (int i = 1; i < x; i++)
                        f *= 0.9f;
                    return "\nTotal from this perk: " + (1 - f).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeDamageBonus += 5,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 0, 10 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Damage",
                _description = "Grip strength increases by 1 kg.\nIncreases melee damage by 5",
                TextureVariation = 0, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    
                    return "\nTotal from this perk: " + x*5;
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.1f,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 0, 9, 11 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Damage",
                _description = "Biceps slightly increases in size.\nIncreases melee damage by 10%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.strength += 10,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 0, 10 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Strength",
                _description = "All flexors gain in size.\nIncreases strength by 10",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.RangedDamageBonus += 5,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 2 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Damage",
                _description = "Shoulder muscules grow.\nIncreases projectile damage by 5",
                TextureVariation = 0, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + x*5;
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ProjectileSizeRatio += 0.05f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 2 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Size",
                _description = "Increased overall physical strength allows for precise shoots from hand held weapons with bigger ammunition.\nIncreases projectile size by 5%",
                TextureVariation = 0, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (x*0.05f).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ProjectileSpeedRatio += 0.05f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 2 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Speed",
                _description = "Increased overall physical strength allows for stronger drawing of ranged weaponry.\nIncreases projectile speed by 5%",
                TextureVariation = 0, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (x * 0.05f).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.SpellCostToStamina, 0.1f),
                
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 1 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Transmutation",
                _description = "The costs of casting spells become easier to quicly recover from.\n10% of the spell cost is now taxed from stamina instead of energy.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellCostRatio *= 1 - 0.04f,
                
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 15 },
                LevelRequirement = 7,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Resource Cost Reduction",
                _description = "In order to preserve energy, spell costs are reduced by 4%",
                TextureVariation = 0, //0 or 1
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    float f = 0.96f;
                    for (int i = 1; i < x; i++)
                        f *= 0.96f;
                    return "\nTotal from this perk: " + (1 - f).ToString("P");
                },
            };
            //0.7 perks
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.DamageReductionPerks *= 0.70f; ModdedPlayer.instance.DamageOutputMultPerks *= 0.70f; },
                
                Category = PerkCategory.Defense,
                Icon = null,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Undestructable",
                _description = "Decreases all damage taken and decreases all damage dealt by 30%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CoolDownMultipier *= 0.96f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 16 },
                LevelRequirement = 7,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 1.5f,
                Name = "Cool Down Reduction",
                _description = " Reduces spell cooldown by 4%",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    float f = 0.96f;
                    for (int i = 1; i < x; i++)
                        f *= 0.96f;
                    return "\nTotal from this perk: " + (1 - f).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CoolDownMultipier *= 0.925f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 18 },
                LevelRequirement = 8,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 2.25f,
                Name = "Greater Cool Down Reduction",
                _description = " Reduces spell cooldown by 7,5%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAllStats(5),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 3 },
                LevelRequirement = 1,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "All attributes",
                _description = "+5 to every strength, agility, vitality and intelligence",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAllStats(15),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 20 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "All attributes",
                _description = "+15 to every strength, agility, vitality and intelligence",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAttackSpeed(0.04f),
                
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 11 },
                LevelRequirement = 6,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = -1.5f,
                Name = "Attack speed",
                _description = "+4% to attack speed ",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (0.04f*x).ToString("P");
                },
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.03f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12, 14 },
                LevelRequirement = 7,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Reusability I",
                _description = "+3% chance to not consume ammo while firing.",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (0.03f * x).ToString("P");
                },
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.13f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 23 },
                LevelRequirement = 9,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = -1.5f,
                Name = "Reusability II",
                _description = "+13% chance to not consume ammo while firing.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance += 0.13f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 24 },
                LevelRequirement = 12,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -0.75f,
                Name = "Reusability III",
                _description = "+13% chance to not consume ammo while firing.",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => StatActions.AddAllStats(10),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 21 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 0.75f,
                Name = "All attributes",
                _description = "+10 to every strength, agility, vitality and intelligence",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.JumpPower += 0.06f,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 3 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 0.75f,
                Name = "Jump",
                _description = "Increases jump height by 6%",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (0.06f * x).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MoveSpeed += 0.035f,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 27 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 1.5f,
                Name = "Light foot",
                _description = "Increases movement speed by 3.5%",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (0.035f * x).ToString("P");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealthBonus += 25,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Bonus Health",
                _description = "Increases health by 25. This is further multipied by maximum health percent.",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (25 * x).ToString("N");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.LifeRegen += 0.25f,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 6 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 0f,
                Name = "Health Regen",
                _description = "Increases health per second regeneration by 0.25. This is further multipied by health regen percent and all healing percent.",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (0.25f * x).ToString("N2");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.Armor += 40,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 0f,
                Name = "Bonus Armor",
                _description = "Increases armor by 40.",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    return "\nTotal from this perk: " + (40 * x).ToString("N");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 31 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "Durability",
                _description = "Decreases all damage taken by 10%.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 32 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 0.75f,
                Name = "Durability II",
                _description = "Further decreases all damage taken by 10%.",
                TextureVariation = 0,
                Endless = false,
            };


            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DamageReductionPerks *= 0.9f,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 33 },
                LevelRequirement = 40,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = 1.5f,
                Name = "Durability III",
                _description = "Further decreases all damage taken by 10%.",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.MagicResistance, 0.05f),
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 29, 31 },
                LevelRequirement = 6,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Magic Resistance",
                _description = "Decreases magic damage taken by 5%",
                TextureVariation = 0,
                Endless = true,
                onPucharseDescriptionUpdate = x =>
                {
                    float f = 0.95f;
                    for (int i = 1; i < x; i++)
                        f *= 0.95f;
                    return "\nTotal from this perk: " + (1-f).ToString("N");
                },
            };
            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.DodgeChance, 0.25f),
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 34 },
                LevelRequirement = 50,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = 2.25f,
                Name = "Dodge",
                _description = "Increases dodge chance by 25%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ARreduction_all += 3,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 6 },
                LevelRequirement = 6,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = 0.75f,
                Name = "Armor Penetration",
                _description = "Increases armor penetration from all sources by 3",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ARreduction_melee += 5,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 37 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 1.5f,
                Name = "Armor Piercing Edge",
                _description = "Increases armor penetration from melee weapons by 5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ARreduction_ranged += 5,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 37 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 1.5f,
                Name = "Anti armor projectiles",
                _description = "Increases armor penetration from ranged weapons by 5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HealthRegenPercent += 0.1f,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 30 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3.5f,
                PosOffsetY = 0f,
                Name = "More Health Regen",
                _description = "Passive health regeneration is increased by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyPerSecond += 0.15f,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 30 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3f,
                PosOffsetY = -0.75f,
                Name = "Energy generation",
                _description = "Passive energy regeneration is increased by 0.15/s",
                TextureVariation = 0,
                Endless = true,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ExpFactor *= 1.1f,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 40 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 0f,
                Name = "Insight",
                _description = "All experience gained increased by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.LifeOnHit += 1f,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 40 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = -0.75f,
                Name = "Life On Hit",
                _description = "Life on hit increased by 1",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyOnHit += 0.5f,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 41 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = -1.5f,
                Name = "Energy On Hit",
                _description = "Energy on hit increased by 0.5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(33, 6),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 4 },
                LevelRequirement = 12,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 0,
                Name = "Alternative cloth sources",
                _description = "Increases daily generation of cloth by 6",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(29, 2),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3.5f,
                PosOffsetY = 0,
                Name = "Demolition",
                _description = "Increases daily generation of bombs by 2. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(29, 10); ModdedPlayer.instance.AddExtraItemCapacity(175, 15); },
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 46 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = 0.75f,
                Name = "Pockets for explosives",
                _description = "Increases max amount of carried bombs and dynamite by 30",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(175, 2),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 47 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 1.5f,
                Name = "Demolition Expert",
                _description = "Increases daily generation of dynamite by 2. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(49, 1),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 0,
                Name = "Meds",
                _description = "Increases daily generation of meds by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(262, 1),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -6.5f,
                PosOffsetY = 0,
                Name = "Fuel",
                _description = "Increases daily generation of fuel cans by 1. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddGeneratedResource(37, 2),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -5.5f,
                PosOffsetY = 0,
                Name = "Booze",
                _description = "Increases daily generation of booze by 2. If it exceeds your max amount of bombs carried, excess will be lost.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(37, 15),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 51 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -6f,
                PosOffsetY = -0.75f,
                Name = "More Booze",
                _description = "Increases max amount of carried booze by 15",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(49, 20),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 49 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = -0.75f,
                Name = "More Meds",
                _description = "Increases max amount of carried meds by 20",
                TextureVariation = 0,
                Endless = true,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ReusabilityChance+= 0.5f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 25 },
                LevelRequirement = 45,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -0.75f,
                Name = "Infinity",
                _description = "Gives 50% chance to not consume ammo when firing a projectile",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellDamageBonus += 5,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 1 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Spell Power",
                _description = "Increases spell damage by 5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 76, 35, 123, 207, 127 }, 5),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -7.5f,
                PosOffsetY = 0,
                Name = "More Meat",
                _description = "Increases carry amount of all meats by 5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 109, 89 }, 20),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -8.5f,
                PosOffsetY = 0,
                Name = "More Snacks",
                _description = "Increases carry amount of candy bars and sodas by 20",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(307, 20),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -13.5f,
                PosOffsetY = 0,
                Name = "More Bolts",
                _description = "Increases carry amount of crossbow bolts by 20",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(94, 20); ModdedPlayer.instance.AddExtraItemCapacity(178, 100); },
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -9.5f,
                PosOffsetY = 0,
                Name = "Corpse collecting",
                _description = "Increases carry amount of bones by 100 and skulls by 20",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 90, 47, 46, 101 }, 10),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 59 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -10f,
                PosOffsetY = 0.75f,
                Name = "More Limbs",
                _description = "Increases carry amount of arms, legs, heads and headbombs by 10",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(57, 6); ModdedPlayer.instance.AddExtraItemCapacity(53, 2); ModdedPlayer.instance.AddExtraItemCapacity(54, 1); },
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -10.5f,
                PosOffsetY = 0,
                Name = "More Building Resources",
                _description = "Increases carry amount of sticks by 6, rocks by 2 and ropes by 1",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 31, 142, 141, 41, 43, 144 }, 5),
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -11.5f,
                PosOffsetY = 0,
                Name = "More Misceleanous Items",
                _description = "Increases carry amount of pots, turtle shells, watches, circuit boards, air carnisters and flares by 5",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AddExtraItemCapacity(new int[] { 177, 71, 56 }, 5); ModdedPlayer.instance.AddExtraItemCapacity(82, 50); },
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 45 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -12.5f,
                PosOffsetY = 0,
                Name = "More Ammo",
                _description = "Increases carry amount of weak and upgraded spears and molotovs by 5, small rocks by 50",
                TextureVariation = 0,
                Endless = true,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearDamageMult *= 2f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Spear Specialization",
                _description = "Thrown spears deal 100% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BulletDamageMult *= 1.6f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = -2.25f,
                Name = "Pistol Specialization",
                _description = "Bullets deal 60% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CrossbowDamageMult *= 1.8f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = -3f,
                Name = "Crossbow Specialization",
                _description = "Bolts deal 80% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BowDamageMult *= 1.4f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0f,
                PosOffsetY = -3.75f,
                Name = "Bow Specialization",
                _description = "Arrows deal 40% more damage",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => SpellActions.HealingDomeGivesImmunity = true,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 40 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4f,
                PosOffsetY = 0.75f,
                Name = "Sanctuary",
                _description = "Healing dome provides immunity to stuns anr root effects",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => BlackFlame.GiveDamageBuff = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Enchant weapon",
                _description = "While black flame is on, melee damage is increased by 60%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.WarCryGiveDamage = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = -1.5f,
                Name = "Empowered War Cry",
                _description = "Warcry additionally increases all damage dealt",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.25f; ModdedPlayer.instance.StaminaAttackCost *= 1.3f; },
                
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 11, 10 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Power Swing",
                _description = "Attacks use 30% more stamina and deal 25% more damage",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.SpellDamageAmplifier_Add += 0.1f; ModdedPlayer.instance.SpellCostRatio *= 1.25f; },
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 15, 55 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -0.75f,
                Name = "Overcharge",
                _description = "Spell damage is increased by 10%, spell costs are increased by 25%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowDmgDebuff = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -2.25f,
                Name = "Exposure",
                _description = "Magic arrow causes hit enemies to take 15% more damage for the duration of the slow.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowDuration += 5,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 73 },
                LevelRequirement = 40,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -2.25f,
                Name = "Disabler",
                _description = "Magic arrow's negative effects last additional 5 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowDoubleSlow = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 74 },
                LevelRequirement = 46,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -2.25f,
                Name = "Magic Binding",
                _description = "Magic arrow's slow amount is doubled. It's upgraded from 35% to 70%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HeavyAttackMult *= 1.5f,
                
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 10 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "Charged Attack",
                _description = "Charged melee attacks deal additional 50%",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.HeavyAttackMult *= 2f,
                
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 76 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = 0f,
                Name = "Super Charged Attack",
                _description = "Charged melee attacks deal 300% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CritDamage += 5,
                
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 9,10 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 0.75f,
                Name = "Lucky Hits",
                _description = "Increases Critical hit damage by 5%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CritChance += 0.12f,
                
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 9,78 },
                LevelRequirement = 35,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 1.5f,
                Name = "Overhelming Odds",
                _description = "Increases Critical chance by 12%.",
                TextureVariation = 0,
                Endless = false,
            };
             new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyBonus += 10,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 3 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -0.75f,
                Name = "Endurance",
                _description = "Increases maximum energy by 10",
                TextureVariation = 0,
                Endless = true,
            };


            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MultishotCount += 2,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 12 },
                LevelRequirement = 31,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = -1.5f,
                Name = "Multishot Empower",
                _description = "Increases the projectile count of multishot by 2, also increases the spells cost. Multishot does not apply to spears.",
                onPucharseDescriptionUpdate = x => string.Format("\nMultishot cost now: {0}\nCost after upgrading: {1}", (10 * Mathf.Pow(2*x, 1.75f)).ToString("N"), (10 * Mathf.Pow((2 + 2*x), 1.75f)).ToString("N")),
            
                TextureVariation = 0,
                Endless = true,
            };

            
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.TurboRaft = true,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 50 },
                LevelRequirement = 22,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -7f,
                PosOffsetY = 0.75f,
                Name = "Transporter",
                _description = "Allows you to use raft on land, turning it into a wooden hovercraft. WORKS FOR HOST/SINGLEPLAYER ONLY!",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.RaftSpeedMultipier++,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 82 },
                LevelRequirement = 23,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -7.5f,
                PosOffsetY = 1.5f,
                Name = "Turbo",
                _description = "Hovercraft but faster!\nIncreases the speed of rafts by 100%. WORKS FOR HOST/SINGLEPLAYER ONLY!",
                TextureVariation = 0,
                Endless = true,
            };
              new Perk()
            {
                ApplyMethods = () =>SpellActions.PurgeHeal =true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 15 },
                LevelRequirement = 10,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "Transpurgation",
                _description = "Purge now heals all players for percent of their missing health and restores energy for percent of missing energy.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CoolDownMultipier *= 0.925f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 19 },
                LevelRequirement = 24,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 3f,
                Name = "Greater Cool Down Reduction",
                _description = " Reduces spell cooldown by 7,5%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CoolDownMultipier *= 0.9f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 85 },
                LevelRequirement = 35,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 3.75f,
                Name = "Greater Cool Down Reduction",
                _description = " Reduces spell cooldown by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CoolDownMultipier *= 0.9f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 86 },
                LevelRequirement = 46,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 4.5f,
                Name = "Greater Cool Down Reduction",
                _description = " Reduces spell cooldown by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () =>SpellCaster.InfinityEnabled =true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 87 },
                LevelRequirement = 60,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = 5.25f,
                Name = "Infinity",
                _description = "Every time you cast a spell, all cooldowns are reduced by 5%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellDamageAmplifier_Add += 0.5f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 88 },
                LevelRequirement = 61,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = 6f,
                Name = "Armageddon",
                _description = "Spell damage increased by 50%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpellAmpFireDmg =true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { -1 },
                LevelRequirement = 2,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = 0f,
                Name = "Inner Fire",
                _description = "Upon hitting an enemy, leave a debuff for 4 seconds, increase fire damage against that enemy equal to your spell amplification",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.NearDeathExperienceUnlocked =true,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 35 },
                LevelRequirement = 20,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = -1.5f,
                Name = "Near Death Experience",
                _description = "Upon recieving fatal damage, instead of dieing restore your health to 100% and gain 5 seconds of immunity to debuffs. This may occur once every 10 minutes",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.SeekingArrow_HeadDamage =3,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55 },
                LevelRequirement = 13,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = -1.5f,
                Name = "Seeking Arrow - Head Hunting",
                _description = "Seeking arrow additional damage on headshot is increased to x3",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.SeekingArrow_DamagePerDistance +=0.01f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 92 },
                LevelRequirement = 19,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0f,
                PosOffsetY = -2.25f,
                Name = "Seeking Arrow - Distant Killer",
                _description = "Seeking arrow additional damage per distance increased from 1% per 1m to 2% per 1m",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.SeekingArrow_SlowDuration += 4,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 93 },
                LevelRequirement = 26,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = -3f,
                Name = "Seeking Arrow - Crippling precision",
                _description = "Seeking arrow slow duration is increased by 4 additional seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.SeekingArrow_SlowAmount -= 0.2f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 94 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = -3.75f,
                Name = "Seeking Arrow - Stun Arrows",
                _description = "Seeking arrow slow amount is increased - from 60% to 80%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.FocusOnHS +=0.5f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 55},
                LevelRequirement = 14,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -0.5f,
                PosOffsetY = -1.5f,
                Name = "Focus - Perfection",
                _description = "Focus damage bonus on headshot is increased from 100% to 150%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.FocusOnAtkSpeed += 0.15f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 96},
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -1f,
                PosOffsetY = -2.25f,
                Name = "Focus - Quick Adjustments",
                _description = "Focus extra attack on bodyshot is increased from 30% to 45%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.FocusOnAtkSpeed += 0.15f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 97},
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -0.5f,
                PosOffsetY = -3f,
                Name = "Focus - Quicker Adjustments",
                _description = "Focus extra attack speed on bodyshot is increased from 45% to 60%",
                TextureVariation = 0,
                Endless = false,
            };
              new Perk()
            {
                ApplyMethods = () => SpellActions.FocusSlowDuration += 20f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 97},
                LevelRequirement = 35,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = -3f,
                Name = "Focus - Knock Out",
                _description = "Focus Slow is prolongued by additional 20 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => BlackFlame.GiveAfterburn = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 69 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = -2.25f,
                Name = "Afterburn",
                _description = "Black flames have a 10% chance to apply a weakening effect on enemies, making them take 15% more damage for 25 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => BlackFlame.DmgAmp = 2,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 100 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -3f,
                Name = "Netherflame",
                _description = "Black flames have double damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.FrenzyAtkSpeed += 0.02f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 72 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -0.75f,
                Name = "Frenzy - Haste",
                _description = "Every stack of frenzy increases attack speed by 2%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.BashDuration += 1,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 72 },
                LevelRequirement = 26,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = -1.5f,
                Name = "Greater Bash",
                _description = "Bash duration is increased by 1 seconds.\nIf bash applies bleed, bleeding deals overall more damage",
                TextureVariation = 0,
                Endless = true,
            };
               new Perk()
            {
                ApplyMethods = () => SpellActions.ShieldPersistanceLifetime += 60f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 16 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 1.5f,
                Name = "Shield - Endurance",
                _description = "Shield doesnt decay for 1 minute longer.",
                TextureVariation = 0,
                Endless = true,
            };       new Perk()
            {
                ApplyMethods = () => SpellActions.BlinkDamage += 14f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 86 },
                LevelRequirement = 40,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 3.75f,
                Name = "Blink - Passthrough",
                _description = "Blink now deals damage to enemies that you teleport through",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.EnergyRegenPerInt += 0.005f,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 21 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -0.75f,
                Name = "Stamina Recovery II",
                _description = "Every point of INTELLIGENCE further increases stamina recover by 0.5%.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.ParryIgnites = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 16 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 0.75f,
                Name = "Flame Guard",
                _description = "Parry ignites enemies",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.ParryRadius++,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 19,107 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 1.5f,
                Name = "Parry range",
                _description = "Increases the radius of parry by 1m",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => StatActions.AddMagicFind(0.1f),
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 42 },
                LevelRequirement = 14,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -5.5f,
                PosOffsetY = 0f,
                Name = "Luck Enchantment",
                _description = "Increases magic find by 10%. Magic find increases the quantity of items dropped.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => StatActions.AddMagicFind(0.15f),
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 109 },
                LevelRequirement = 55,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = -6.5f,
                PosOffsetY = 0f,
                Name = "Item Rain",
                _description = "Increases magic find by additional 15%. Magic find increases the quantity of items dropped.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.BIA_HealthTakenMult +=0.25f,
                
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 67 },
                LevelRequirement = 16,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = -3.75f,
                Name = "Near death arrow",
                _description = "Blood infused arrow takes 25% more health to convert it to damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.BIA_HealthDmMult +=2f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 19 },
                LevelRequirement = 14,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 2.25f,
                Name = "Arcane Blood",
                _description = "Blood infused arrow damage per health is increased by 2 dmg/hp.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.HealingDomeRegEnergy = true,
                
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 68 },
                LevelRequirement = 36,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -4.5f,
                PosOffsetY = 1.5f,
                Name = "Energy Field",
                _description = "Healing dome regenerates energy",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MaxHealthPercent += 0.1f,
                
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 29},
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Enchanced vitality",
                _description = "Increases max health by 10%",
                TextureVariation = 0,
                Endless = false,
            };
             new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MaxEnergyPercent += 0.1f,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 80},
                LevelRequirement = 9,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Enchanced energy",
                _description = "Increases max energy by 10%",
                TextureVariation = 0,
                Endless = false,
            };
             new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CraftingReroll= true,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { -1},
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1.5f,
                PosOffsetX = 0f,
                PosOffsetY = 3f,
                Name = "Crafting - Rerolling",
                _description = "Opens Crafting Menu in inventory. Allows you to reroll item's properites by placing 2 items of the same rarity as ingredients.",
                TextureVariation = 1,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.CraftingReforge= true,
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 116},
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1.5f,
                PosOffsetX = 1.5f,
                PosOffsetY = 3f,
                Name = "Crafting - Reforging",
                _description = "Adds a tab to crafting menu. Allows you to reforge an item into any other item of the same tier by placing 3 items of the same or higher rarity as ingredients.",
                TextureVariation = 1,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.flashlightIntensity++;ModdedPlayer.instance.flashlightBatteryDrain++;  },
                
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 62 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1f,
                PosOffsetX = -12f,
                PosOffsetY = -0.75f,
                Name = "Light The Way",
                _description = "Flashlight is 100% brighter and lasts 100% longer",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.DamageReductionPerks *= 1.1f; ModdedPlayer.instance.DamageOutputMultPerks *= 1.1f; },
                
                Category = PerkCategory.Defense,
                Icon = null,
                InheritIDs = new int[] { 5 },
                LevelRequirement = 8,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 1.5f,
                Name = "Glass Cannon",
                _description = "Increases all damage taken and increases all damage dealt by 10%",
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ProjectileDamageIncreasedBySize= true,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 13 },
                LevelRequirement = 32,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 0.75f,
                Name = "Size Matters",
                _description = "Projectile size increases projectile's damage.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BunnyHop = true,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 27 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 0.75f,
                Name = "Momentum transfer",
                _description = "Upon landing gain shot movement speed buff",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeRange += 0.04f,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 22 },
                LevelRequirement = 4,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = -2.25f,
                Name = "Long arm",
                _description = "Increases melee weapon range by 4%",
                TextureVariation = 0, //0 or 1
                Endless = true,
            }; 
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ChanceToBleedOnHit += 0.02f,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 78,79 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 1.5f,
                Name = "Bleed",
                _description = "Hitting an enemy has 2% chance to make them bleed",
                TextureVariation = 0, //0 or 1
                Endless = true,
            }; 
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ChanceToWeakenOnHit += 0.025f,
                
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 71,22 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = -1.5f,
                Name = "Weaken",
                _description = "Hitting an enemy has 2.5% chance to make them take more damage",
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearCritChance += 0.36f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 64 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = -1.5f,
                Name = "Javelin",
                _description = "Spear has increased headshot chance to 40%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearCritChance += 0.1f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 125 },
                LevelRequirement = 33,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0f,
                PosOffsetY = -2.25f,
                Name = "Spear gamble",
                _description = "Spear has increased headshot chance to 50%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearhellChance += 0.45f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 126 },
                LevelRequirement = 34,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = -1f,
                PosOffsetY = -2.25f,
                Name = "Double spears",
                _description = "When a spear hits a target, it has a 30% chance summon another spear and launch it at the enemy",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearhellChance += 0.02f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 127 },
                LevelRequirement = 53,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2f,
                PosOffsetY = -2.25f,
                Name = "Spearinfinity",
                _description = "Increases the chance of doublespears by 2%",
                TextureVariation = 0, //0 or 1
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearDamageMult *=1.75f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 126 },
                LevelRequirement = 50,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -0.5f,
                PosOffsetY = -3f,
                Name = "Spear Mastery",
                _description = "Increases spear damage by 75%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BulletCritChance +=0.2f,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 65 },
                LevelRequirement = 45,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -2.25f,
                Name = "Deadeye",
                _description = "Increases headshot chance of pistol's bullets by 20%, to a total of 30%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.SpearArmorRedBonus=true,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 127,129 },
                LevelRequirement = 35,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = -1.5f,
                PosOffsetY = -3f,
                Name = "Piercing",
                _description = "Spear armor reduction from ranged is increased to 150%, additionally, thrown spears also reduce armor equal to melee armor reduction",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BunnyHopUpgrade = true,
                
                Category = PerkCategory.Utility,
                Icon = null,
                InheritIDs = new int[] { 121 },
                LevelRequirement = 55,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 1.5f,
                Name = "Bunny hopping",
                _description = "Increases the speed and duration of the buff",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AttackSpeedMult *= 1.65f; ModdedPlayer.instance.MeleeDamageAmplifier_Mult*= 0.5f; ModdedPlayer.instance.RangedDamageAmplifier_Mult /= 10; },
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 7f,
                PosOffsetY = -0.75f,
                Name = "Curse of Quickening",
                _description = "Increases attack speed by 65%, but decreases melee damage by 50% and ranged damage by 90%",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AttackSpeedMult *=0.5f; ModdedPlayer.instance.MeleeDamageAmplifier_Add +=1.5f;},
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 8f,
                PosOffsetY = -0.75f,
                Name = "Curse of Strengthening",
                _description = "Decreases attack speed by 50%, but greatly increases melee damage by 150%",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Add+=1.1f; ModdedPlayer.instance.RangedDamageAmplifier_Mult *= 0; },
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 7.5f,
                PosOffsetY = 0f,
                Name = "Curse of Binding",
                _description = "Makes you unable to damage enemies with ranged weapons, causing all of them to deal 110% less damage, but at the same time, you deal 100% increased melee damage",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Add +=0.2f; ModdedPlayer.instance.AttackSpeedMult *= 1.2f; },
                Category = PerkCategory.MeleeOffense,
                Icon = null,
                InheritIDs = new int[] { 79 },
                LevelRequirement = 60,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 2.25f,
                Name = "Melee Mastery",
                _description = "Increases melee weapon damage and attack speed by 20%",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.RangedDamageAmplifier_Add +=1f; ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0; },
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 8.5f,
                PosOffsetY = -1.5f,
                Name = "Curse of Binding",
                _description = "Makes you unable to damage enemies with melee weapons, causing all of them to deal 100% less damage, but at the same time, you deal 100% increased ranged damage",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.RangedDamageAmplifier_Add += 1.2f; ModdedPlayer.instance.MoveSpeedMult *= 0.8f; ModdedPlayer.instance.JumpPower *= 0.7f; },
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 8f,
                PosOffsetY = -0.75f,
                Name = "Curse of Crippling",
                _description = "You become more deadly but less precise.\nYour ranged damage is increased by 120%, but you loose 20% movement speed and 30% jump power.",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AttackSpeedMult *= 1.65f; ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0.1f; ModdedPlayer.instance.RangedDamageAmplifier_Mult /= 4; },
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 9f,
                PosOffsetY = -0.75f,
                Name = "Curse of Quickening",
                _description = "Increases attack speed by 65%, but decreases melee damage by 90% and ranged damage by 25%",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.MaxEnergyPercent *= 0.5f; ModdedPlayer.instance.StaminaRegenPercent -=0.5f; ModdedPlayer.instance.SpellDamageAmplifier_Add += 1f; },
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 9f,
                PosOffsetY = -0.75f,
                Name = "Curse of Exhaustion",
                _description = "Increases attack spell damage by 100%, but your energy is reduced by 50% and stamina regenerates slower",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.AttackSpeedMult *= 0.6f; ModdedPlayer.instance.CoolDownMultipier *= 0.65f; },
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 8.5f,
                PosOffsetY = 0f,
                Name = "Curse of Speed",
                _description = "Cooldown reduction increased by 35%, but attack speed decreased by 40%",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= 0.4f; ModdedPlayer.instance.RangedDamageAmplifier_Mult *= 0.4f; ModdedPlayer.instance.SpellDamageAmplifier_Add += 1f; },
                Category = PerkCategory.MagicOffense,
                Icon = null,
                InheritIDs = new int[] { 89 },
                LevelRequirement = 77,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 9.5f,
                PosOffsetY = 0f,
                Name = "Curse of Power",
                _description = "Magic damage is increased by 100%, but ranged and melee are weaker by 60% ",
                TextureVariation = 1, //0 or 1
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.ParryDmgBonus += 1.5f,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 107 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 0.75f,
                Name = "Counter Strike",
                _description = "When parrying, gain attack dmg for the next attack. Bonus melee damage is equial to damage of parry. This effect can stack, lasts 20 seconds, and is consumed upon performing a melee attack.",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add+= 0.5f,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 11 },
                LevelRequirement = 45,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -1.5f,
                Name = "Skull Basher",
                _description = "When bash is equipped, melee weapons deal 50% more damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.MeleeDamageAmplifier_Add += 0.6f,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 144 },
                LevelRequirement = 50,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1f,
                PosOffsetY = -2.25f,
                Name = "Skull Basher II",
                _description = "When bash is equipped, melee weapons deal 110% more damage",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DanceOfFiregod=true,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 71 },
                LevelRequirement = 45,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -0.75f,
                Name = "Dance of the Firegod",
                _description = "When black flame is on, your melee damage is increased, based on how fast youre going.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.StaminaOnHit += 3,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 9 },
                LevelRequirement = 5,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 1.5f,
                Name = "Combat Regen",
                _description = "Gain 3 points of stamina on hit",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.FrenzyMS =true,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 102 },
                LevelRequirement = 26,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -0.75f,
                Name = "Mania",
                _description = "Frenzy increases movement speed by 5% per stack",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.FurySwipes = true,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 148 },
                LevelRequirement = 43,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 6f,
                PosOffsetY = -0.75f,
                Name = "Fury Swipes",
                _description = "When during frenzy you hit the same enemy over and over, gain more and more damage. Melee stacks 6 times faster.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.BashDamageBuff++,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 103 },
                LevelRequirement = 27,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = -1.5f,
                Name = "Lucky Bashes",
                _description = "When you bash an enemy, gain 100% critical hit damage for 2 seconds.",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.MaxLogs++,
                Category = PerkCategory.Utility,
                InheritIDs = new int[] { 61 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -11f,
                PosOffsetY = -0.75f,
                Name = "More Carried Logs",
                _description = "Increases the base amount of logs that a player can carry on their shoulder. The additional carried logs are invisible",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.ProjectileDamageIncreasedBySpeed = true,
                
                Category = PerkCategory.RangedOffense,
                Icon = null,
                InheritIDs = new int[] { 14 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 0f,
                Name = "Speed Matters",
                _description = "Projectile speed increases projectile's crit damage.",
                TextureVariation = 0, //0 or 1
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => SpellActions.MagicArrowCrit = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 73 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = -3f,
                Name = "Magic Arrow Devastation",
                _description = "Magic arrow can critically hit.",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => SpellActions.BL_Crit = true,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 88 },
                LevelRequirement = 62,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = 6f,
                Name = "Nuke Conjuration",
                _description = "Ball Lightning can critically hit.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.BlinkRange += 10f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 105 },
                LevelRequirement = 40,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 3.75f,
                Name = "Blink - Wormhole",
                _description = "Blink has 66.6% increased distance",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.FireAmp +=0.1f,
                
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 90 },
                LevelRequirement = 10,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -2.5f,
                PosOffsetY = 0f,
                Name = "Fiery Embrace",
                _description = "Fire damage is increased by 10%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { SpellActions.BIA_TripleDmg = true; ModdedPlayer.instance.HealingMultipier *= 0.5f; },
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 111 },
                LevelRequirement = 55,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = -3.75f,
                Name = "Cursed Arrow",
                _description = "Blood infused arrow deals triple damage, but healing recieved is halved, and you loose energy for a short time after casting the spell",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.BIA_Weaken = true,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 111 },
                LevelRequirement = 30,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 1.5f,
                PosOffsetY = -4.5f,
                Name = "Deep Wounds",
                _description = "Enemies hit by blood infused arrow take 100% increased damage from all sources for 15 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.EnergyOnHit += 1f; ModdedPlayer.instance.LifeOnHit += 1.5f; },
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 44,43 },
                LevelRequirement = 47,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -3.5f,
                PosOffsetY = -1.5f,
                Name = "Rejuvenation",
                _description = "Gain +1 energy on hit, and +1.5 life per hit.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { SpellDataBase.spellDictionary[10].Cooldown -= 15; },
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 153,75 },
                LevelRequirement = 47,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = -3f,
                Name = "Endless stream",
                _description = "Reduce the cooldown of Magic Arrow by 15 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { SpellActions.ParryRadius++; ModdedPlayer.instance.ParryAnything = true; },
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 78 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = 0.75f,
                Name = "Parry Mastery",
                _description = "Increases the radius of Parry by 1m, allows you to parry any type of enemy.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.thornsPerStrenght+=1.2f,
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 31},
                LevelRequirement = 3,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 0.75f,
                Name = "Thorny Skin",
                _description = "Every point of strength increases thorns by 1.2\nThorns scale with melee damage multipier stats",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { ModdedPlayer.instance.thornsMult *= 2; ModdedPlayer.instance.Armor += 400; },
                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 32 },
                LevelRequirement = 25,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = 0f,
                Name = "Iron Maiden",
                _description = "Increases armor by 400, and increases thorns damage by 100%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.MagicResistance, 0.2f),

                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 35,32 },
                LevelRequirement = 24,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4f,
                PosOffsetY = -0.75f,
                Name = "Anti-Magic Training",
                _description = "Decreases magic damage taken by 20%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BlackholePullImmune = true,

                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 164 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = -1.5f,
                Name = "Dense Matter",
                _description = "Black holes cannot suck you in",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.BlizzardSlowReduced = true,

                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 164 },
                LevelRequirement = 30,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -2.25f,
                Name = "Warmth",
                _description = "Blizzard slow effect greatly reduced",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.TrueAim =true,
                Category = PerkCategory.RangedOffense,
                InheritIDs = new int[] { 54 },
                RequiredIds= new int[] { 67 },
                LevelRequirement = 70,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5.5f,
                PosOffsetY = 0f,
                Name = "True Aim",
                _description = "Arrow headshots which hit enemies over 60 m away and are not affected by seeking arrow hit enemies twice, and deal 4x damage",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.thorns += 50,

                Category = PerkCategory.Defense,
                InheritIDs = new int[] { 162 },
                LevelRequirement = 15,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 1.5f,
                Name = "Spikes",
                _description = "Adds 50 thorns",
                TextureVariation = 0,
                Endless = false,
            };

            new Perk()
            {
                ApplyMethods = () => SpellDataBase.spellDictionary[16].Cooldown-=60,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 154 },
                LevelRequirement = 80,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2f,
                PosOffsetY = 6.75f,
                Name = "Storm Season",
                _description = "Ball Lightning has its cooldown reduced by 60 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellDataBase.spellDictionary[16].Cooldown -= 20,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 169 },
                LevelRequirement = 120,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3f,
                PosOffsetY = 6.75f,
                Name = "Endless Storm",
                _description = "Ball Lightning has its cooldown reduced by 20 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellDataBase.spellDictionary[3].Cooldown -= 7,

                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 155 },
                LevelRequirement = 66,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = 3.75f,
                Name = "Blink - Ascendancy",
                _description = "Blink has it's cooldown reduced by 7 seconds",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellDataBase.spellDictionary[4].Cooldown -= 20,

                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 85 },
                LevelRequirement = 44,
                PointsToBuy = 2,
                Size = 1,
                PosOffsetX = 3.5f,
                PosOffsetY = 3f,
                Name = "Wrath of the Sun",
                _description = "Sun Flare cooldown is reduced by 20",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.ParryDmgBonus += 2f,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 143 },
                LevelRequirement = 20,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 4.5f,
                PosOffsetY = 1.5f,
                Name = "Full Counter",
                _description = "Increase the damage bonus from parrying",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.GoldenResolve=true,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 87 },
                LevelRequirement = 50,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 2.5f,
                PosOffsetY = 4.5f,
                Name = "Golden Resolve",
                _description = "Gold reduces damage taken by 50%",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => SpellDataBase.spellDictionary[4].Cooldown -= 90,
                Category = PerkCategory.MagicOffense,
                InheritIDs = new int[] { 87 },
                LevelRequirement = 50,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 0.5f,
                PosOffsetY = 4.5f,
                Name = "Sudden Rampage",
                _description = "Cooldown of Berserk is decreased by 90",
                TextureVariation = 0,
                Endless = true,
            };
            new Perk()
            {
                ApplyMethods = () => SpellActions.HealingDomeDuration += 50,
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 113 },
                LevelRequirement = 44,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -5.5f,
                PosOffsetY = 1.5f,
                Name = "Safe Heaven",
                _description = "Healing dome lasts a minute.",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => { SpellDataBase.spellDictionary[2].Cooldown -= 35; SpellDataBase.spellDictionary[13].Cooldown -= 7.5f; },
                Category = PerkCategory.Support,
                InheritIDs = new int[] { 176 },
                LevelRequirement = 58,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = -6.5f,
                PosOffsetY = 1.5f,
                Name = "Time of Need",
                _description = "The cooldown of healing dome and purge is reduced by 50%",
                TextureVariation = 0,
                Endless = false,
            };
            new Perk()
            {
                ApplyMethods = () => ModdedPlayer.instance.DanceOfFiregodAtkCap = true,
                Category = PerkCategory.MeleeOffense,
                InheritIDs = new int[] { 146 },
                LevelRequirement = 46,
                PointsToBuy = 1,
                Size = 1,
                PosOffsetX = 5f,
                PosOffsetY = -0.75f,
                Name = "Breathing Tehniques",
                _description = "When black flame is on, your attack speed is fixed at 100%",
                TextureVariation = 0,
                Endless = false,
            };
        }

        public int ID;
        public int[] InheritIDs;
        public int[] RequiredIds;
        public int PointsToBuy = 1;
        public int LevelRequirement;

        public bool IsBought = false;
        public bool Applied = false;

        public delegate void OnApply();
        public delegate string OnPucharseDescriptionUpdate(int level);
        public OnApply ApplyMethods;
        public OnPucharseDescriptionUpdate onPucharseDescriptionUpdate;


        public string Name;
        public string _description;
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

        public Perk(string name, string description, int[] inheritIDs, float x, float y, PerkCategory category, float size, int levelRequirement, OnApply applyMethods)
        {
            Name = name;
            _description = description;
            InheritIDs = inheritIDs;
            Category = category;
            Size = size;
            Endless = false;
            PosOffsetX = x;
            PosOffsetY = y;
            LevelRequirement = levelRequirement;
            ApplyMethods = applyMethods;
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
        public Perk(string name, string description, int inheritIDs, float x, float y, PerkCategory category, float size, int levelRequirement, OnApply applyMethods)
        {
            Name = name;
            _description = description;
            InheritIDs = new int[] { inheritIDs };
            PosOffsetX = x;
            PosOffsetY = y;
            Category = category;
            Size = size;
            Endless = false;
            LevelRequirement = levelRequirement;
            ApplyMethods = applyMethods;
            ID = AllPerks.Count;
            Applied = false;
            AllPerks.Add(this);

        }
        public void OnBuy()
        {
            try
            {

            if (Endless)
            {
                if (onPucharseDescriptionUpdate != null)
                    Description = _description + ' ' + onPucharseDescriptionUpdate(ApplyAmount);
            }
            else
            {
                if (onPucharseDescriptionUpdate != null)
                    Description = _description +' '+ onPucharseDescriptionUpdate(1);

            }
            }
            catch (System.Exception)
            {
                
            }
        }


    }


}
