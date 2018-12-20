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
            //Tier one basic upgrades that allow stats to take effect
            new Perk("Stronger Hits", "Every point of STRENGHT increases MEELE DAMAGE by 0.5%.", -1,150,0 , PerkCategory.MeleeOffense, 1, 1, () => ModdedPlayer.instance.DamagePerStrenght += 0.005f, () => ModdedPlayer.instance.DamagePerStrenght -= 0.005f);
            new Perk("Stronger Spells", "Every point of INTELLIGENCE increases SPELL DAMAGE by 0.5%.", -1, 150, 0, PerkCategory.MagicOffense, 1, 1, () => ModdedPlayer.instance.SpellDamageperInt += 0.005f, () => ModdedPlayer.instance.SpellDamageperInt -= 0.005f);
            new Perk("Stronger Projectiles", "Every point of AGILITY increases RANGED DAMAGE by 0.5%.", -1, 150, 0, PerkCategory.RangedOffense, 1, 1, () => ModdedPlayer.instance.SpellDamageperInt += 0.005f, () => ModdedPlayer.instance.SpellDamageperInt -= 0.005f);
            new Perk("Stamina Recovery", "Every point of INTELLIGENCE increases stamina recover by 0.5%.", -1, 150, 0, PerkCategory.Utility, 1, 1, () => ModdedPlayer.instance.EnergyRegenPerInt += 0.005f, () => ModdedPlayer.instance.EnergyRegenPerInt -= 0.005f);
            new Perk("More Stamina", "Every point of AGILITY increases max stamina by 0.5", -1, 150, 30, PerkCategory.Utility, 1, 1, () => ModdedPlayer.instance.EnergyPerAgility += 0.5f, () => ModdedPlayer.instance.EnergyPerAgility -= 0.5f);
            new Perk("More Health", "Every point of VITALITY increases max health by 3", -1, 150, 0, PerkCategory.Defense, 1, 1, () => ModdedPlayer.instance.HealthPerVitality += 3, () => ModdedPlayer.instance.HealthPerVitality -= 3f);
            new Perk("More Healing", "Increases healing by 5%", -1, 150, 30, PerkCategory.Defense, 1, 1, () => ModdedPlayer.instance.HealingMultipier *= 1.05f, () => ModdedPlayer.instance.HealingMultipier /= 1.05f);
        }

        public int ID;
        public int[] InheritIDs;
        public int PointsToBuy =1;
        public bool IsBought= false;
        public bool Applied=false;
        public delegate void OnApply();
        public delegate void OnDisable();
        public OnApply ApplyMethods;
        public OnDisable DisableMethods;
        public string Name;
        public string Description;
        public int LevelRequirement;
        public int Tier =1;
        public float Radius;
        public float Angle;
        public enum PerkCategory { MeleeOffense, RangedOffense, MagicOffense, Defense, Support, Utility }
        public PerkCategory Category;

        public Perk(string name, string description, int[] inheritIDs,float radius,float angle, PerkCategory category, int tier, int levelRequirement, OnApply applyMethods, OnDisable disableMethods)
        {
            Name = name;
            Description = description;
            InheritIDs = inheritIDs;
            Category = category;
            Tier = tier;
            Radius = radius;
            Angle = angle;
            LevelRequirement = levelRequirement;
            ApplyMethods = applyMethods;
            DisableMethods = disableMethods;
            ID = AllPerks.Count;
            Applied = false;
            AllPerks.Add(this);
            ModAPI.Log.Write("[" + ID + "]" + name);


        }
        public Perk()
        {
            Applied = false;

            ID = AllPerks.Count;
            ModAPI.Log.Write("[" + ID + "]" + Name);
        }
        public Perk(string name, string description, int inheritIDs, float radius, float angle, PerkCategory category, int tier, int levelRequirement, OnApply applyMethods, OnDisable disableMethods)
        {
            Name = name;
            Description = description;
            InheritIDs = new int[] { inheritIDs };
            Radius = radius;
            Angle = angle;
            Category = category;
            Tier = tier;
            LevelRequirement = levelRequirement;
            ApplyMethods = applyMethods;
            DisableMethods = disableMethods;
            ID = AllPerks.Count;
            Applied = false;
            AllPerks.Add(this);
            ModAPI.Log.Write("[" + ID + "]" + name);

        }
    }


}
