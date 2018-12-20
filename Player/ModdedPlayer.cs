using ChampionsOfForest.Player;
using System;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest
{
    public class ModdedPlayer : MonoBehaviour
    {
        public static ModdedPlayer instance = null;
        public float MaxHealth
        {
            get
            {
                float x = 20 + vitality * HealthPerVitality;
                x += HealthBonus;
                x *= 1 + MaxHealthPercent;
                return x;
            }
        }
        public float MaxEnergy
        {
            get
            {
                float x = 10 + agility * EnergyPerAgility;
                x += EnergyBonus;
                x *= 1 + MaxEnergyPercent;
                return x;
            }
        }
        public bool Critted => CritChance >= 100 * UnityEngine.Random.value;
        public float SpellAMP
        {
            get
            {
                float f = SpellDamageperInt * intelligence;
                return (1 + f) * SpellDamageAmplifier * DamageOutputMult;
            }
        }
        public float MeleeAMP
        {
            get
            {
                return DamageOutputMult * (strenght * DamagePerStrenght + 1) * MeleeDamageAmplifier;
            }
        }
        public float RangedAMP
        {
            get
            {
                float f = agility * RangedDamageperAgi;
                return (1 + f) * RangedDamageAmplifier * DamageOutputMult;

            }
        }
        public float CritDamageBuff
        {
            get
            {
                if (Critted)
                {
                    return CritDamage / 100 + 1;
                }
                return 1;
            }
        }

        public float ArmorDmgRed => Mathf.Min(1, Mathf.Sqrt((Armor) / 10) / 100);
        public int Level = 1;

        public float HealingMultipier = 1;
        public int strenght = 1;    //increases damage
        public int intelligence = 1; //increases spell damage
        public int agility = 1;     //increases energy
        public int vitality = 1;     //increases health
        public float StaminaRecover => (4 + EnergyRegen) * (1 + EnergyRegenPercent);
        public float DamagePerStrenght = 0.01f;
        public float SpellDamageperInt = 0.01f;
        public float RangedDamageperAgi = 0.01f;
        public float EnergyRegenPerInt = 0.075f;
        public float EnergyPerAgility = 0.25f;
        public float HealthPerVitality = 2f;
        public float HealthRegenPercent = 0;
        public float EnergyRegenPercent = 0;
        public int HealthBonus = 0;
        public int EnergyBonus = 0;
        public float MaxHealthPercent = 0;
        public float MaxEnergyPercent = 0;
        public float CoolDownMultipier = 1;
        public float SpellDamageAmplifier = 1;
        public float MeleeDamageAmplifier = 1;
        public float RangedDamageAmplifier = 1;
        public float SpellDamageBonus = 0;
        public float MeleeDamageBonus = 0;
        public float RangedDamageBonus = 0;
        public float MeleeRange = 1;


        public float DamageReduction = 0;
        public float DamageOutputMult = 1;
        public float CritChance = 0;
        public float CritDamage = 50;
        public float LifeOnHit = 0;
        public float LifeRegen = 0;
        public float EnergyRegen = 0;
        public float DodgeChance = 0;
        public float SlowAmount = 0;
        public bool Silenced = false;
        public bool Stunned = false;
        public int Armor = 0;
        public float MagicResistance = 0;
        public float AttackSpeed = 1;
        public bool StunImmune = false;
        public bool DebuffImmune = false;
        public float MoveSpeed = 1f;
        public float SpellCostToStamina = 0;
        public float StaminaAttackCostReduction = 0;
        public float BlockFactor = 1;
        public float ExpFactor = 1;
        public long ExpCurrent = 15;
        public long ExpGoal = 20;



        public int PermanentBonusPerkPoints;
        public int PerkPoints = 10;
        public long NewlyGainedExp;

        public int MassacreKills;
        public string MassacreText = "";
        private float MassacreMultipier = 1;
        public float TimeUntillMassacreReset;
        public float MaxMassacreTime = 20;
        public float TimeBonusPerKill;


        private float StunDuration = 0;
        private void Start()
        {
            ModAPI.Log.Write("SETUP: Created Player");
            instance = this;
            MoveSpeed = 1f;
         
        }


        private void Update()
        {

            try
            {
                if (ModAPI.Input.GetButtonDown("EquipWeapon"))
                {
                    if (Inventory.Instance.ItemList[-12] != null)
                    {
                        if (Inventory.Instance.ItemList[-12].Equipped)
                        {
                            PlayerInventoryMod.CustomEquipModel = Inventory.Instance.ItemList[-12].weaponModel;
                            LocalPlayer.Inventory.StashEquipedWeapon(false);
                            LocalPlayer.Inventory.Equip(80, false);

                            PlayerInventoryMod.CustomEquipModel = BaseItem.WeaponModelType.None;
                        }
                    }
                }
                float dmgPerSecond = 0;
                int poisonCount = 0;
                foreach (KeyValuePair<int, BuffDB.Buff> item in BuffDB.activeBuffs)
                {
                    if (DebuffImmune && item.Value.isNegative && item.Value.DispellAmount <= 2)
                    {
                        BuffDB.activeBuffs[item.Key].ForceEndBuff(item.Key);
                    }
                    else if (StunImmune && item.Value.isNegative && item.Value.DispellAmount <= 1)
                    {
                        BuffDB.activeBuffs[item.Key].ForceEndBuff(item.Key);

                    }
                    else
                    {
                        BuffDB.activeBuffs[item.Key].UpdateBuff(item.Key);
                        if (item.Value._ID == 3)
                        {
                            poisonCount++;
                            dmgPerSecond += item.Value.amount;
                        }
                    }
                }
                LocalPlayer.Stats.Health -= dmgPerSecond * Time.deltaTime;
                LocalPlayer.Stats.HealthTarget -= dmgPerSecond * Time.deltaTime;

                if (poisonCount > 1)
                {
                    BuffDB.AddBuff(1, 33, 0.7f, 1);
                }

                if (LocalPlayer.Stats.Health <= 0)
                {
                    LocalPlayer.Stats.Hit(1, true);
                }



            }
            catch (Exception e)
            {
                ModAPI.Log.Write(e.ToString());
            }
            if (Time.time % 10 == 5)
            {
                LocalPlayer.Stats.Skills.BreathingSkillLevelBonus = 0.05f;
                LocalPlayer.Stats.Skills.BreathingSkillLevelDuration = 1500000;
                LocalPlayer.Stats.Skills.RunSkillLevelBonus = 0.05f;
                LocalPlayer.Stats.Skills.RunSkillLevelDuration = 6000000;
                LocalPlayer.Stats.Skills.TotalRunDuration = 0;
            }
            if (TimeUntillMassacreReset > 0)
            {
                TimeUntillMassacreReset -= Time.deltaTime;
                if (TimeUntillMassacreReset <= 0)
                {
                    AddFinalExperience((long)Mathf.Round(NewlyGainedExp * MassacreMultipier));
                    NewlyGainedExp = 0;
                    TimeUntillMassacreReset = 0;
                    MassacreKills = 0;
                    CountMassacre();
                }


            }
            if (LocalPlayer.Stats.Health < MaxHealth)
            {
                if (LocalPlayer.Stats.Health < LocalPlayer.Stats.HealthTarget)
                {
                    LocalPlayer.Stats.Health += LifeRegen * (HealthRegenPercent + 1) * HealingMultipier;
                }
                else
                {
                    LocalPlayer.Stats.Health += LifeRegen * (HealthRegenPercent + 1) * HealingMultipier / 10;
                }
            }
            LocalPlayer.Stats.PhysicalStrength.CurrentStrength = 10;

            if (Stunned)
            {
                if (StunImmune) Stunned = false;
                StunDuration -= Time.deltaTime;
                if(StunDuration < 0)
                {
                    Stunned = false;
                }
            }
        }
        public void Stun(float duration)
        {
            if (StunImmune) return;
            Stunned = true;
            if(StunDuration < duration)
            {
                StunDuration = duration;
            }
        }
        public void AddKillExperience(long Amount)
        {
            MassacreKills++;
            CountMassacre();
            if (TimeUntillMassacreReset == 0)
            {
                TimeUntillMassacreReset = MaxMassacreTime;
            }
            else
            {
                TimeUntillMassacreReset = Mathf.Clamp(TimeUntillMassacreReset + 10, 20, MaxMassacreTime);
            }
            NewlyGainedExp += Amount;
        }
        public void AddFinalExperience(long Amount)
        {
            ExpCurrent += Amount;
            while (ExpCurrent >= ExpGoal)
            {
                ExpCurrent -= ExpGoal;
                LevelUp();
            }
        }
        public void LevelUp()
        {
            Level++;

            ExpGoal = GetGoalExp();
        }
        private long GetGoalExp()
        {
            int x = Level;
            float a = 1.7f;
            float b = 4;
            float c = 55;
            float d = 0.7f;
            double y = Mathf.Pow(x, a) * b + Mathf.Sin(d * x) * c + c * x;
            return Convert.ToInt64(y);
        }
        private long GetGoalExp(int lvl)
        {
            int x = lvl;
            float a = 1.7f;
            float b = 4;
            float c = 55;
            float d = 0.7f;
            double y = Mathf.Pow(x, a) * b + Mathf.Sin(d * x) * c + c * x;
            return Convert.ToInt64(y);
        }
        public void CountMassacre()
        {
            if (KCInRange(0, 3))
            {
                MassacreMultipier = 1;
                MassacreText = "";
            }
            else if (KCInRange(3, 4))
            {
                MassacreMultipier = 1.1f;
                MassacreText = "TRIPLE KILL";
            }
            else if (KCInRange(4, 5))
            {
                MassacreMultipier = 1.3f;
                MassacreText = "QUADRA KILL";
            }
            else if (KCInRange(5, 6))
            {
                MassacreMultipier = 1.5f;
                MassacreText = "PENTA KILL";
            }
            else if (KCInRange(6, 10))
            {
                MassacreMultipier = 1.75f;
                MassacreText = "MASSACRE";
            }
            else if (KCInRange(10, 12))
            {
                MassacreMultipier = 2.3f;
                MassacreText = "BIG MASSACRE";
            }
            else if (KCInRange(12, 16))
            {
                MassacreMultipier = 3f;
                MassacreText = "BIGGER MASSACRE";
            }
            else if (KCInRange(16, 20))
            {
                MassacreMultipier = 5f;
                MassacreText = "HUGE MASSACRE";
            }
            else if (KCInRange(20, 25))
            {
                MassacreMultipier = 8.5F;
                MassacreText = "BLOODY MASSACRE";
            }
            else if (KCInRange(25, 30))
            {
                MassacreMultipier = 15F;
                MassacreText = "WICED SICK";
            }
            else if (KCInRange(30, 40))
            {
                MassacreMultipier = 22.5f;
                MassacreText = "UNSTOPPABLE MASSACRE";
            }
            else if (KCInRange(40, 50))
            {
                MassacreMultipier = 35;
                MassacreText = "GODLIKE MASSACRE";
            }
            else if (KCInRange(50, 65))
            {
                MassacreMultipier = 50;
                MassacreText = "BEYOND GODLIKE";
            }
            else if (KCInRange(65, 75))
            {
                MassacreMultipier = 250;
                MassacreText = "SLAUGHTER";
            }
            else if (KCInRange(75, 100))
            {
                MassacreMultipier = 1000;
                MassacreText = "BLOODBATH";
            }
            else if (MassacreKills >= 100)
            {
                MassacreMultipier = 10000;
                MassacreText = "R A M P A G E";
            }
        }

        private bool KCInRange(int min, int max)
        {
            if (MassacreKills > min && MassacreKills <= max)
            {
                return true;
            }
            return false;
        }

        private void FinishMassacre()
        {

            TimeUntillMassacreReset = 0;
            long Amount = NewlyGainedExp * (long)MassacreMultipier;
            NewlyGainedExp = 0;
            MassacreMultipier = 1;
            AddFinalExperience(Amount);
        }
    }
}
