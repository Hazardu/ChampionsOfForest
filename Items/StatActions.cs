using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Items
{
    internal class StatActions
    {
        public static void AddVitality(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.vitality += Mathf.RoundToInt(f);
        }
        public static void RemoveVitality(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.vitality -= Mathf.RoundToInt(f);
        }
        public static void AddStrenght(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.strenght += Mathf.RoundToInt(f);
        }
        public static void RemoveStrenght(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.strenght -= Mathf.RoundToInt(f);
        }
        public static void AddAgility(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.agility += Mathf.RoundToInt(f);
        }
        public static void RemoveAgility(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.agility -= Mathf.RoundToInt(f);
        }
        public static void AddIntelligence(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.intelligence += Mathf.RoundToInt(f);
        }
        public static void RemoveIntelligence(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.intelligence -= Mathf.RoundToInt(f);
        }
        public static void AddHealth(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealthBonus += Mathf.RoundToInt(f);
        }
        public static void RemoveHealth(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealthBonus -= Mathf.RoundToInt(f);
        }
        public static void AddEnergy(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.EnergyBonus += Mathf.RoundToInt(f);
        }
        public static void RemoveEnergy(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.EnergyBonus -= Mathf.RoundToInt(f);
        }
        public static void AddHPRegen(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.LifeRegen += f;
        }
        public static void RemoveHPRegen(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.LifeRegen -= f;
        }
        public static void AddStaminaRegen(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.StaminaRegen += f;
        }
        public static void RemoveStaminaRegen(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.StaminaRegen -= f;
        }
        public static void AddEnergyRegen(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.EnergyPerSecond += f;
        }
        public static void RemoveEnergyRegen(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.EnergyPerSecond -= f;
        }
        public static void AddStaminaRegenPercent(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.StaminaRegenPercent += f;
        }
        public static void RemoveStaminaRegenPercent(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.StaminaRegenPercent -= f;
        }
        public static void AddHealthRegenPercent(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealthRegenPercent += f;
        }
        public static void RemoveHealthRegenPercent(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealthRegenPercent -= f;
        }
        public static void AddDamageReduction(float f)
        {
            ModdedPlayer.instance.DamageReduction*=1- f;
        }
        public static void RemoveDamageReduction(float f)
        {
            ModdedPlayer.instance.DamageReduction /=1- f;
        }
        public static void AddCritChance(float f)
        {
            ItemDataBase.AddPercentage(ref ChampionsOfForest.ModdedPlayer.instance.CritChance, f);
        }
        public static void RemoveCritChance(float f)
        {
            ItemDataBase.RemovePercentage(ref ChampionsOfForest.ModdedPlayer.instance.CritChance, f);
        }
        public static void AddCritDamage(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.CritDamage += f;
        }
        public static void RemoveCritDamage(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.CritDamage -= f;
        }
        public static void AddLifeOnHit(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.LifeOnHit += f;
        }
        public static void RemoveLifeOnHit(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.LifeOnHit -= f;
        }
        public static void AddDodgeChance(float f)
        {
            ItemDataBase.AddPercentage(ref ChampionsOfForest.ModdedPlayer.instance.DodgeChance, f);
        }
        public static void RemoveDodgeChance(float f)
        {
            ItemDataBase.RemovePercentage(ref ChampionsOfForest.ModdedPlayer.instance.DodgeChance, f);
        }
        public static void AddArmor(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.Armor += Mathf.RoundToInt(f);
        }
        public static void RemoveArmor(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.Armor -= Mathf.RoundToInt(f);
        }
        public static void AddMagicResistance(float f)
        {
            ItemDataBase.AddPercentage(ref ChampionsOfForest.ModdedPlayer.instance.MagicResistance, f);
        }
        public static void RemoveMagicResistance(float f)
        {
            ItemDataBase.RemovePercentage(ref ChampionsOfForest.ModdedPlayer.instance.MagicResistance, f);
        }
        public static void AddAttackSpeed(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.AttackSpeedAdd += f;
        }
        public static void RemoveAttackSpeed(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.AttackSpeedAdd -= f;
        }
        public static void AddExpFactor(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.ExpFactor *= f;
        }
        public static void RemoveExpFactor(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.ExpFactor /= f;
        }
        public static void AddMaxMassacreTime(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MaxMassacreTime += f;
        }
        public static void RemoveMaxMassacreTime(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MaxMassacreTime -= f;
        }
        public static void AddSpellDamageAmplifier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.SpellDamageAmplifier_Add += f;
        }
        public static void RemoveSpellDamageAmplifier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.SpellDamageAmplifier_Add -= f;
        }
        public static void AddMeleeDamageAmplifier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MeleeDamageAmplifier_Add += f;
        }
        public static void RemoveMeleeDamageAmplifier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MeleeDamageAmplifier_Add -= f;
        }
        public static void AddRangedDamageAmplifier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.RangedDamageAmplifier_Add+= f;
        }
        public static void RemoveRangedDamageAmplifier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.RangedDamageAmplifier_Add -= f;
        }
        public static void AddSpellDamageBonus(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.SpellDamageBonus += f;
        }
        public static void RemoveSpellDamageBonus(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.SpellDamageBonus -= f;
        }
        public static void AddMeleeDamageBonus(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MeleeDamageBonus += f;
        }
        public static void RemoveMeleeDamageBonus(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MeleeDamageBonus -= f;
        }
        public static void AddRangedDamageBonus(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.RangedDamageBonus += f;
        }
        public static void RemoveRangedDamageBonus(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.RangedDamageBonus -= f;
        }
        public static void AddEnergyPerAgility(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.EnergyPerAgility += f;
        }
        public static void RemoveEnergyPerAgility(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.EnergyPerAgility -= f;
        }
        public static void AddHealthPerVitality(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealthPerVitality += f;
        }
        public static void RemoveHealthPerVitality(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealthPerVitality -= f;
        }
        public static void AddSpellDamageperInt(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.SpellDamageperInt += f;
        }
        public static void RemoveSpellDamageperInt(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.SpellDamageperInt -= f;
        }
        public static void AddDamagePerStrenght(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.DamagePerStrenght += f;
        }
        public static void RemoveDamagePerStrenght(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.DamagePerStrenght -= f;
        }
        public static void AddHealingMultipier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealingMultipier *= 1 + f;
        }
        public static void RemoveHealingMultipier(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.HealingMultipier /= 1 + f;
        }
        public static void AddMoveSpeed(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MoveSpeed *= 1 + f;
        }
        public static void RemoveMoveSpeed(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MoveSpeed /= 1 + f;
        }
            public static void AddJump(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.JumpPower *= 1 + f;
        }
        public static void RemoveJump(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.JumpPower /= 1 + f;
        }

        //   public static void Add(float f)
        //{
        //    ChampionsOfForest.ModdedPlayer.instance. += f;
        //}
        //public static void Remove(float f)
        //{
        //    ChampionsOfForest.ModdedPlayer.instance. -= f;
        //}


        public static void PERMANENT_perkPointIncrease(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.PermanentBonusPerkPoints += Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.MutationPoints += Mathf.RoundToInt(f);
        }
        public static void PERMANENT_expIncrease(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.AddFinalExperience((long)f);
        }


        public static void AddMagicFind(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MagicFindMultipier += f;
            if(GameSetup.IsMultiplayer)
            Network.NetworkManager.SendLine("AD", Network.NetworkManager.Target.Everyone);

        }
        public static void RemoveMagicFind(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.MagicFindMultipier -= f;
            if (GameSetup.IsMultiplayer)
                Network.NetworkManager.SendLine("AD", Network.NetworkManager.Target.Everyone);
        }

        public static void AddAllStats(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.strenght += Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.vitality += Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.agility += Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.intelligence += Mathf.RoundToInt(f);
        }
        public static void RemoveAllStats(float f)
        {
            ChampionsOfForest.ModdedPlayer.instance.strenght -= Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.vitality -= Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.agility -= Mathf.RoundToInt(f);
            ChampionsOfForest.ModdedPlayer.instance.intelligence -= Mathf.RoundToInt(f);
        }




    }
}
