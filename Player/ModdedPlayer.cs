using Bolt;
using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TheForest.Utils;
using UnityEngine;
using static ChampionsOfForest.Player.BuffDB;
using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
    public class ModdedPlayer : MonoBehaviour
    {
        public readonly float baseHealth = 50;
        public readonly float baseEnergy = 50;
        public static float basejumpPower;
        public static ModdedPlayer instance = null;


        public float MaxHealth
        {
            get
            {
                float x = baseHealth + (vitality) * HealthPerVitality;
                x += HealthBonus;
                x *= 1 + MaxHealthPercent;
                return x;
            }
        }
        public float MaxEnergy
        {
            get
            {
                float x = baseEnergy + (agility) * EnergyPerAgility;
                x += EnergyBonus;
                x *= 1 + MaxEnergyPercent;
                return x;
            }
        }
        public bool Critted => CritChance >= UnityEngine.Random.value;
        public float SpellAMP
        {
            get
            {
                float f = SpellDamageperInt * intelligence;
                return (1 + f) * SpellDamageAmplifier * DamageOutputMultTotal;
            }
        }
        public float MeleeAMP => DamageOutputMultTotal * ((strenght * DamagePerStrenght) + 1) * MeleeDamageAmplifier;
        public float RangedAMP
        {
            get
            {
                float f = (agility) * RangedDamageperAgi;
                return (1 + f) * RangedDamageAmplifier * DamageOutputMultTotal;

            }
        }
        public float CritDamageBuff
        {
            get
            {
                if (Critted)
                {
                    return (CritDamage / 100) + 1;
                }
                return 1;
            }
        }
        public float StaminaAndEnergyRegenAmp => 1 + (intelligence) * EnergyRegenPerInt;
        public float ArmorDmgRed => ModReferences.DamageReduction(Armor - (int)ArmorReduction);
        public float DamageReductionTotal => (DamageReduction) * (DamageReductionPerks);
        public float DamageOutputMultTotal => DamageOutputMultPerks * DamageOutputMult;
        public int MeleeArmorReduction => ARreduction_all + ARreduction_melee;
        public int RangedArmorReduction => ARreduction_all + ARreduction_ranged;


        private int _level = 1;
        public int Level { get => _level; set => _level = value; }
        public float HealingMultipier = 1;
        public int strenght = 1;    //increases damage
        public int intelligence = 1; //increases spell damage
        public int agility = 1;     //increases energy
        public int vitality = 1;     //increases health  
        public float StaminaRecover => (4 + StaminaRegen) * (1 + StaminaRegenPercent);
        public float DamagePerStrenght = 0.00f;
        public float SpellDamageperInt = 0.00f;
        public float RangedDamageperAgi = 0.00f;
        public float EnergyRegenPerInt = 0.00f;
        public float EnergyPerAgility = 0f;
        public float HealthPerVitality = 0f;
        public float FireAmp = 0;
        public bool SpellAmpFireDmg;
        public float HealthRegenPercent = 0;
        public float StaminaRegenPercent = 0;
        public int HealthBonus = 0;
        public int EnergyBonus = 0;
        public float MaxHealthPercent = 0;
        public float MaxEnergyPercent = 0;
        public float CoolDownMultipier = 1;
        public float SpellDamageAmplifier => SpellDamageAmplifier_Mult * SpellDamageAmplifier_Add;
        public float MeleeDamageAmplifier => MeleeDamageAmplifier_Mult * MeleeDamageAmplifier_Add;
        public float RangedDamageAmplifier => RangedDamageAmplifier_Mult * RangedDamageAmplifier_Add;
        public float HeadShotDamage = 6;
        public float SpellDamageAmplifier_Mult = 1;
        public float MeleeDamageAmplifier_Mult = 1;
        public float RangedDamageAmplifier_Mult = 1;

        public float SpellDamageAmplifier_Add = 1;
        public float MeleeDamageAmplifier_Add = 1;
        public float RangedDamageAmplifier_Add = 1;


        public float SpellDamageBonus = 0;
        public float MeleeDamageBonus = 0;
        public float RangedDamageBonus = 0;

        public float MeleeRange = 1;
        public float DamageReduction = 1;
        public float DamageReductionPerks = 1;
        public float DamageOutputMult = 1;
        public float DamageOutputMultPerks = 1;
        public float CritChance = 0.05f;
        public float CritDamage = 50;
        public float LifeOnHit = 0;
        public float LifeRegen = 0;
        public float StaminaRegen = 0;
        public float DodgeChance = 0;
        public float SlowAmount = 0;
        public bool Silenced = false;
        public bool Rooted = false;
        public bool Stunned = false;
        public int Armor = 0;
        public float ArmorReduction = 0;
        public float MagicResistance = 0;


        public float AttackSpeed => AttackSpeedAdd * AttackSpeedMult;
        public float AttackSpeedMult = 1;
        public float AttackSpeedAdd = 1;

        public float RangedAttackSpeed = 1;
        public float BowRearmSpeed = 0.0f;
        public bool GreatBowIgnites = false;



        public int StunImmune = 0;
        public int RootImmune = 0;
        public int DebuffImmune = 0;
        public int DebuffResistant = 0;
        public float MoveSpeed = 1f;
        public float JumpPower = 1f;
        public float SpellCostToStamina = 0;
        public float SpellCostRatio = 1;
        public float StaminaAttackCost = 0;
        public float BlockFactor = 0.5f;
        public float ExpFactor = 1;
        public long ExpCurrent = 0;
        public long ExpGoal = 1;
        public int PermanentBonusPerkPoints;
        public int MutationPoints = 0;
        public long NewlyGainedExp;
        public int MassacreKills;
        public string MassacreText = "";
        public float MassacreMultipier = 1;
        public float TimeUntillMassacreReset;
        public float MaxMassacreTime = 20;
        public float TimeBonusPerKill = 5;
        public float EnergyOnHit = 0;
        public float EnergyPerSecond = 0;
        public int ARreduction_all = 0;
        public int ARreduction_melee = 0;
        public int ARreduction_ranged = 0;
        public bool TurboRaft = false;
        public float RaftSpeedMultipier = 1;
        public float ChanceToSlowOnHit = 0;
        public float ChanceToBleedOnHit = 0;
        public float ChanceToWeakenOnHit = 0;
        public float DamageAbsorbAmount
        {
            get
            {
                float f = 0;
                for (int i = 0; i < damageAbsorbAmounts.Length; i++)
                {
                    f += damageAbsorbAmounts[i];
                }
                return f;
            }
        }
        public float[] damageAbsorbAmounts = new float[2];//every unique source of shielding gets their own slot here, if its not unique it uses [0]
        //[1] is channeled shield spell;


        public float StealthDamage = 1; //to do

        public static readonly float HungerPerLevelRateMult = 0.04f;
        public static readonly float ThirstPerLevelRateMult = 0.04f;
        public float ThirstRate = 1;
        public float HungerRate = 1;
        public float FireDamageTakenMult = 1;
        public float AreaDamageProcChance = 0.15f;
        public float AreaDamage = 0;
        public float AreaDamageRadius = 4;
        public float ProjectileSpeedRatio = 1f;
        public float ProjectileSizeRatio = 1;
        public float HeavyAttackMult = 1;
        public Dictionary<int, int> GeneratedResources = new Dictionary<int, int>();

        public float MagicFindMultipier = 1;

        //perks 
        public float ReusabilityChance = 0;
        public int ReusabilityAmount = 1;
        //public TheForest.Items.Item lastShotProjectile;
        public float SpearDamageMult = 1;
        public float SpearCritChance = 0.04f;
        public float SpearhellChance = 0;
        public float BulletCritChance = 0.1f;
        public float BulletDamageMult = 1;
        public float CrossbowDamageMult = 1;
        public float BowDamageMult = 1;
        public int MultishotCount = 1;

        //Item abilities variables
        //Smokeys quiver
        public bool IsSmokeysQuiver = false;
        public bool SpearArmorRedBonus = false;
        public bool IsCrossfire = false;
        public bool IsHazardCrown = false;
        public int HazardCrownBonus = 0;
        public float LastCrossfireTime = 0;

        //Any hammer
        public bool IsHammerStun = false;
        public float HammerStunDuration = 0.4f;
        public float HammerStunAmount = 0.25f;
        public float HammerSmashDamageAmp = 1f;

        //Hexed pants of mr Moritz
        public bool HexedPantsOfMrM_Enabled = false;
        public float HexedPantsOfMrM_StandTime = 0;

        //Death Pact shoulders
        public bool DeathPact_Enabled = false;
        public float DeathPact_Amount = 1;


        public int LastDayOfGeneration = 0;
        public float RootDuration = 0;
        public float StunDuration = 0;

        public bool NearDeathExperience = false;
        public bool NearDeathExperienceUnlocked = false;

        public bool KingQruiesSpecial = false;
        public bool SoraSpecial = false;
        public float flashlightIntensity = 1;
        public float flashlightBatteryDrain = 1;

        public bool CraftingReroll = false;
        public bool CraftingReforge = false;


        public bool BunnyHop = false;
        public bool BunnyHopUpgrade = false;


        public bool ProjectileDamageIncreasedBySize = false;
        public bool isGreed = false;
        public bool isWindArmor = false;
        private float _greedCooldown;

        public Dictionary<int, ExtraItemCapacity> ExtraCarryingCapactity = new Dictionary<int, ExtraItemCapacity>();
        public struct ExtraItemCapacity
        {
            public int ID;
            public int Amount;
            public bool applied;
            public ExtraItemCapacity(int id, int amount)
            {
                ID = id;
                Amount = amount;
                applied = false;
            }
            public void NewApply()
            {
                applied = true;
                LocalPlayer.Inventory.AddMaxAmountBonus(ID, Amount);
            }

            public void ExistingApply(int NewAmount)
            {
                if (applied)
                {
                    Remove();
                }
                Amount += NewAmount;
                if (Amount > 0)
                {
                    NewApply();
                }
                else
                {
                    instance.ExtraCarryingCapactity.Remove(ID);
                }

            }
            public void Remove()
            {

                switch (ID)
                {
                    case 53:    //rock and rock bag
                        if (LocalPlayer.Inventory.AmountOf(214) > 0)
                        {
                            LocalPlayer.Inventory.SetMaxAmountBonus(ID, 5);
                        }
                        break;
                    case 82:    //small rock and small rock bag
                        if (LocalPlayer.Inventory.AmountOf(282) > 0)
                        {
                            LocalPlayer.Inventory.SetMaxAmountBonus(ID, 15);
                        }
                        break;
                    case 57:    //stick and stick bag
                        if (LocalPlayer.Inventory.AmountOf(215) > 0)
                        {
                            LocalPlayer.Inventory.SetMaxAmountBonus(ID, 10);
                        }
                        break;
                    case 56:    //spears and spear bag
                        if (LocalPlayer.Inventory.AmountOf(290) > 0)
                        {
                            LocalPlayer.Inventory.SetMaxAmountBonus(ID, 4);
                        }
                        break;
                    default:

                        LocalPlayer.Inventory.SetMaxAmountBonus(ID, 0);
                        break;


                }
                applied = false;
            }
        }
        public void AddExtraItemCapacity(int ID, int Amount)
        {
            if (ExtraCarryingCapactity.ContainsKey(ID))
            {
                ExtraCarryingCapactity[ID].ExistingApply(Amount);
            }
            else if (Amount > 0)
            {
                ExtraItemCapacity EIC = new ExtraItemCapacity(ID, Amount);
                EIC.NewApply();
                ExtraCarryingCapactity.Add(ID, EIC);
            }
        }
        public void AddExtraItemCapacity(int[] ID, int Amount)
        {
            for (int i = 0; i < ID.Length; i++)
            {
                AddExtraItemCapacity(ID[i], Amount);
            }
        }

        private void Start()
        {
            instance = this;
            MoveSpeed = 1f;
            MutationPoints = 1;
            ExpGoal = GetGoalExp();

            if (!GameSetup.IsNewGame)
            {
                Serializer.Load();
            }
            InitializeHandHeld();
            Invoke("SendJoinMessage", 10);
            StartCoroutine(InitializeCamera());
        }

        IEnumerator InitializeCamera()
        {
            while (Camera.main == null) 
            {
                yield return null;
            }
            Camera.main.gameObject.AddComponent<RealisticBlackHoleEffect>();

        }

        public void SendLevelMessage()
        {
            if (ChatBoxMod.instance != null)
            {
                if (BoltNetwork.isRunning)
                {
                    NetworkManager.SendText("II" + LocalPlayer.Entity.GetState<IPlayerState>().name + " has reached level " + ModdedPlayer.instance.Level + "!", NetworkManager.Target.Everyone);
   
                }
            }
            MainMenu.Instance.LevelUpAction();
        }
        public void SendJoinMessage()
        {
            if (ChatBoxMod.instance != null)
            {
                if (BoltNetwork.isRunning)
                {
                    string s ="II"+ LocalPlayer.Entity.GetState<IPlayerState>().name + " JOINED THE SERVER!\nWELCOME!\n INSTALLED MODS: \n";
                    Regex regex = new Regex(@"\w+");
                    foreach (var item in ModAPI.Mods.LoadedMods)
                    {
                        s += regex.Match(item.Value.UniqueId).Value + " [" + item.Value.Version + "]\n";
                    }
                    NetworkManager.SendText(s, NetworkManager.Target.Everyone);
                }
            }
        }

        public void SendLeaveMessage(string Player)
        {
            if (ChatBoxMod.instance != null)
            {
                if (BoltNetwork.isRunning)
                {
                    string s = "II"+Player + " LEFT! FAREWELL!";
                    NetworkManager.SendText(s, NetworkManager.Target.Everyone);
                }
            }
        }

        public static int AttributeBonus(int x)
        {
            if (x == 1) return 1;
            int bonus = Mathf.RoundToInt(Mathf.Sqrt(x));
            return AttributeBonus(x - 1) + bonus;
        }

        public void AssignLevelAttributes()
        {
            int x = AttributeBonus(Level);

            strenght = x;
            intelligence = x;
            vitality = x;
            agility = x;
        }

        private static string ListComponents(Transform t,string prefix ="")
        {
            string result = prefix+ t.name+":\n";
            var components = t.gameObject.GetComponents<Component>();
            foreach (var comp in components)
            {
                result += prefix + "\t- " + comp.GetType().ToString() + "\n";
            }
            foreach (Transform child in t)
            {
                result += ListComponents(child, prefix +"\t");
            }
            return result;
        }

        private void Update()
        {

            if (ModAPI.Input.GetButtonDown("EquipWeapon"))
            {
                if (Inventory.Instance.ItemList[-12] != null)
                {
                    if (Inventory.Instance.ItemList[-12].Equipped)
                    {
                        if (Inventory.Instance.ItemList[-12].weaponModel != BaseItem.WeaponModelType.Greatbow)
                        {
                            PlayerInventoryMod.ToEquipWeaponType = Inventory.Instance.ItemList[-12].weaponModel;
                            LocalPlayer.Inventory.StashEquipedWeapon(false);
                            LocalPlayer.Inventory.Equip(80, false);
                            PlayerInventoryMod.ToEquipWeaponType = BaseItem.WeaponModelType.None;


                        }
                        else
                        {
                            PlayerInventoryMod.ToEquipWeaponType = Inventory.Instance.ItemList[-12].weaponModel;
                            LocalPlayer.Inventory.StashEquipedWeapon(false);
                            if (LocalPlayer.Inventory.Equip(79, false))
                            {
                                CustomBowBase.baseBow.SetActive(false);
                                GreatBow.instance.SetActive(true);

                            }
                            else
                            {
                                CotfUtils.Log("NO CRAFTED BOW!");
                            }
                            PlayerInventoryMod.ToEquipWeaponType = BaseItem.WeaponModelType.None;

                        }
                    }
                }
            }
            try
            {
                float dmgPerSecond = 0;
                int poisonCount = 0;
                ArmorReduction = 0;
                int[] keys = new List<int>(BuffDB.activeBuffs.Keys).ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    Buff buff = BuffDB.activeBuffs[keys[i]];
                    if (DebuffImmune>0 && buff.isNegative && buff.DispellAmount <= 2)
                    {
                        BuffDB.activeBuffs[keys[i]].ForceEndBuff(keys[i]);
                        continue;
                    }
                    else if (DebuffResistant>0 && buff.isNegative && buff.DispellAmount <= 1)
                    {
                        BuffDB.activeBuffs[keys[i]].ForceEndBuff(keys[i]);
                        continue;
                    }
                    else
                    {
                        BuffDB.activeBuffs[keys[i]].UpdateBuff(keys[i]);
                        if (buff._ID == 3)
                        {
                            poisonCount++;
                            dmgPerSecond += buff.amount;
                        }
                        else if (buff._ID == 21)
                        {
                            ArmorReduction -= buff.amount;
                        }
                    }
                }
                if (dmgPerSecond != 0)
                {
                    dmgPerSecond *= 1 - MagicResistance;
                    dmgPerSecond *= DamageReductionTotal;
                    LocalPlayer.Stats.Health -= dmgPerSecond * Time.deltaTime;
                    LocalPlayer.Stats.HealthTarget -= dmgPerSecond * Time.deltaTime * 2;

                    if (poisonCount > 1)
                    {
                        BuffDB.AddBuff(1, 33, 0.7f, 1);
                    }


                }
                if (LocalPlayer.Stats.Health <= 0 && !LocalPlayer.Stats.Dead)
                {
                    LocalPlayer.Stats.Hit(10, true, PlayerStats.DamageType.Drowning);
                }

            }
            catch (Exception e)
            {
                //ModAPI.Log.Write("Poisoning player error" + e.ToString());
            }



            if (LocalPlayer.Stats != null)
            {
                float ats = ModdedPlayer.instance.AttackSpeed;
                if (GreatBow.isEnabled) ats /= 2f;


                if (LocalPlayer.Stats.Stamina > 4)
                {

                    LocalPlayer.Animator.speed = ats;
                }
                else
                {
                    LocalPlayer.Animator.speed = Mathf.Min(0.5f, ats / 2);

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

                if (Clock.Day > LastDayOfGeneration)
                {
                    for (int i = 0; i < Clock.Day - LastDayOfGeneration; i++)
                    {
                        foreach (KeyValuePair<int, int> pair in GeneratedResources)
                        {
                            LocalPlayer.Inventory.AddItem(pair.Key, pair.Value);
                        }
                    }
                    LastDayOfGeneration = Clock.Day;
                }

            }
            //if (UnityEngine.Input.GetKeyDown(KeyCode.F5))
            //{

            //    for (int i = 0; i < ModReferences.rightHandTransform.childCount; i++)
            //    {
            //        Transform trans = ModReferences.rightHandTransform.GetChild(i);


            //        try
            //        {
            //            Debug.LogWarning(trans.name);
                 
            //        if (trans.gameObject.activeSelf)
            //        {
            //            var components = trans.gameObject.GetComponents<Component>();
            //            string s = trans.name+"\n\n\n";
            //            foreach (var item in components)
            //            {
            //                s += "\n" + item.ToString();
            //            }
            //            s += "\n\n\n" + ModReferences.ListAllChildren(trans, "");
            //            ModAPI.Log.Write(s);
            //           Debug.Log(s);
                        
            //        }
            //        }
            //        catch (Exception e)
            //        {

            //            Debug.LogError(e.ToString());
            //        }
            //    }
            //}



            if (TimeUntillMassacreReset > 0)
            {
                TimeUntillMassacreReset -= Time.unscaledDeltaTime;
                if (TimeUntillMassacreReset <= 0)
                {
                    AddFinalExperience(Convert.ToInt64((double)NewlyGainedExp * MassacreMultipier));
                    NewlyGainedExp = 0;
                    TimeUntillMassacreReset = 0;
                    MassacreKills = 0;
                    CountMassacre();
                }


            }

            if (Effects.Multishot.IsOn)
            {
                if (!SpellCaster.RemoveStamina(3 * Time.deltaTime) || LocalPlayer.Stats.Stamina < 7)
                {
                    Effects.Multishot.IsOn = false;
                    Effects.Multishot.localPlayerInstance.SetActive(false);
                }
            }
            Berserker.Effect();

            if (Rooted)
            {
                if (StunImmune>0 || RootImmune>0)
                {
                    Rooted = false;
                    if (!Stunned)
                    {
                        LocalPlayer.FpCharacter.MovementLocked = false;
                        LocalPlayer.FpCharacter.CanJump = true;
                        LocalPlayer.Rigidbody.isKinematic = false;
                        LocalPlayer.Rigidbody.useGravity = true;
                        LocalPlayer.Rigidbody.WakeUp();
                    }
                }

                RootDuration -= Time.deltaTime;
                if (RootDuration < 0)
                {

                    Rooted = false;
                    if (!Stunned)
                    {
                        LocalPlayer.Rigidbody.isKinematic = false;
                        LocalPlayer.Rigidbody.useGravity = true;
                        LocalPlayer.Rigidbody.WakeUp();
                        LocalPlayer.FpCharacter.MovementLocked = false;
                        LocalPlayer.FpCharacter.CanJump = true;
                    }
                }
            }
            if (Stunned)
            {
                if (StunImmune>0)
                {
                    Stunned = false;
                    LocalPlayer.FpCharacter.Locked = false;
                    if (!Rooted)
                    {
                        LocalPlayer.FpCharacter.MovementLocked = false;
                        LocalPlayer.FpCharacter.CanJump = true;
                        LocalPlayer.Rigidbody.isKinematic = false;
                        LocalPlayer.Rigidbody.useGravity = true;
                        LocalPlayer.Rigidbody.WakeUp();
                    }
                }
                StunDuration -= Time.deltaTime;
                if (StunDuration < 0)
                {
                 
                    Stunned = false;
                    LocalPlayer.FpCharacter.Locked = false;
                    if (!Rooted)
                    {
                        LocalPlayer.FpCharacter.MovementLocked = false;
                        LocalPlayer.FpCharacter.CanJump = true;
   LocalPlayer.Rigidbody.isKinematic = false;
                    LocalPlayer.Rigidbody.useGravity = true;
                    LocalPlayer.Rigidbody.WakeUp();
                    }
                }
            }
            if (HexedPantsOfMrM_Enabled)
            {
                if (LocalPlayer.FpCharacter.velocity.sqrMagnitude < 0.1)//if standing still
                {
                    HexedPantsOfMrM_StandTime = Mathf.Clamp(HexedPantsOfMrM_StandTime - Time.deltaTime, -1.1f, 1.1f);
                    if (HexedPantsOfMrM_StandTime <= 1)
                    {
                        if (LocalPlayer.Stats.Health > 5)
                        {
                            LocalPlayer.Stats.Health -= Time.deltaTime * MaxHealth * 0.015f;
                        }
                    }
                }
                else //if moving
                {
                    HexedPantsOfMrM_StandTime = Mathf.Clamp(HexedPantsOfMrM_StandTime + Time.deltaTime, -1.1f, 1.1f);
                    if (HexedPantsOfMrM_StandTime >= 1)
                    {
                        AddBuff(9, 41, 1.2f, 1f);
                        AddBuff(11, 42, 1.2f, 1f);
                    }
                }
            }
            if (DeathPact_Enabled)
            {
                DamageOutputMult /= DeathPact_Amount;

                DeathPact_Amount =1 +Mathf.RoundToInt((1 - (LocalPlayer.Stats.Health / MaxHealth)) * 100) * 0.03f;
                AddBuff(12, 43, DeathPact_Amount, 1f);

                DamageOutputMult *= DeathPact_Amount;

            }
            if (isGreed)
            {
                _greedCooldown -= Time.deltaTime;
                if(_greedCooldown< 0)
                {
                    AutoPickupItems.DoPickup();
                    _greedCooldown = 1f;
                }
            }
        }
        public float DealDamageToShield(float f)
        {
            for (int i = 0; i < damageAbsorbAmounts.Length; i++)
            {
                if (damageAbsorbAmounts[i] > 0)
                {
                    if (f - damageAbsorbAmounts[i] <= 0)
                    {
                        damageAbsorbAmounts[i] -= f;
                        return 0;
                    }
                    else
                    {
                        f -= damageAbsorbAmounts[i];
                        damageAbsorbAmounts[i] = 0;
                    }
                }
            }
            return f;
        }

        public void Root(float duration)
        {
            if (StunImmune>0 || RootImmune>0)
            {
                return;
            }
            LocalPlayer.HitReactions.enableFootShake(1, 0.2f);

            Rooted = true;
            if (RootDuration < duration)
            {
                RootDuration = duration;
            }
        }
        public void Stun(float duration)
        {
            if (StunImmune>0)
            {
                return;
            }
            LocalPlayer.HitReactions.enableFootShake(1, 0.6f);

            Stunned = true;
            if (StunDuration < duration)
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
                TimeUntillMassacreReset = Mathf.Clamp(TimeUntillMassacreReset + TimeBonusPerKill, 20, MaxMassacreTime);
            }
            NewlyGainedExp += Amount;

        }
        public void AddFinalExperience(long Amount)
        {
         
            ExpCurrent += Convert.ToInt64(Amount * ExpFactor);
            int i = 0;
            while (ExpCurrent >= ExpGoal || (Level > 100 && ExpCurrent < 0))
            {
                ModdedPlayer.instance.ExpCurrent -= ModdedPlayer.instance.ExpGoal;
                ModdedPlayer.instance.LevelUp();
                MainMenu.Instance.LevelsToGain++;
                i++;
            }

            if (i > 0)
            {
                if (GameSetup.IsMultiplayer)
                {
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(19);
                            w.Write(ModReferences.ThisPlayerID);
                            w.Write(ModdedPlayer.instance.Level);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                        answerStream.Close();
                    }
                }
            }
        }
        public void OnGetHit()
        {
            if(SpellActions.ChanceToParryOnHit&& Random.value < 0.15f)
            {
                SpellActions.DoParry(LocalPlayer.Transform.forward + LocalPlayer.Transform.position);
            }
        }
        public void OnHit()
        {
                LocalPlayer.Stats.HealthTarget += LifeOnHit * HealingMultipier;
                LocalPlayer.Stats.Health += LifeOnHit * HealingMultipier;
                LocalPlayer.Stats.Energy += EnergyOnHit * StaminaAndEnergyRegenAmp;
                SpellActions.OnFrenzyAttack();
         
        }
        public void OnHitEffectsClient(BoltEntity ent,float damage)
        {
            if (ent != null)
            {
                if (ChanceToBleedOnHit > 0 && Random.value < ChanceToBleedOnHit)
                {
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(32);
                            w.Write(ent.networkId.PackedValue);
                            w.Write(Mathf.CeilToInt(damage / 20));
                            w.Write(10);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
                        answerStream.Close();
                    }
                }
                if (ChanceToSlowOnHit > 0 && Random.value < ChanceToSlowOnHit)
                {
                    int id = 62 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(22);
                            w.Write(ent.networkId.PackedValue);
                            w.Write(0.5f);
                            w.Write(8f);
                            w.Write(id);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
                        answerStream.Close();
                    }
                }
                if (ChanceToWeakenOnHit > 0 && Random.value < ChanceToWeakenOnHit)
                {
                    int id = 62 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
                    using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                    {
                        using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                        {
                            w.Write(34);
                            w.Write(ent.networkId.PackedValue );
                            w.Write(id);
                            w.Write(1.2f);
                            w.Write(8f);
                        w.Close();
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
                        answerStream.Close();
                    }
                }
            }
        }
        public void OnHitEffectsHost(EnemyProgression p,float damage)
        {
            if (p != null)
            {
                if (ChanceToBleedOnHit > 0 && Random.value < ChanceToBleedOnHit)
                {
                    p.DoDoT(Mathf.CeilToInt(damage / 20), 10);
                }
                if (ChanceToSlowOnHit > 0 && Random.value < ChanceToSlowOnHit)
                {
                    int id = 62 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
                    p.Slow(id, 0.5f, 8);
                }
                if (ChanceToWeakenOnHit > 0 && Random.value < ChanceToWeakenOnHit)
                {
                    int id = 62 + ModReferences.Players.IndexOf(LocalPlayer.GameObject);
                    p.DmgTakenDebuff(id, 1.2f, 8);
                }
            }
        }
        

        public void OnHit_Ranged()
        {
            SpellCaster.InfinityLoopEffect();
         
        }
        public void OnHit_Melee()
        {
            SpellCaster.InfinityLoopEffect();

        }

        public bool DoAreaDamage(Transform rootTR, float damage)
        {
            try
            {
                if (Random.value < AreaDamageProcChance && AreaDamage > 0)
                {
                    DoGuaranteedAreaDamage(rootTR, damage);
                    return true;
                }
            }
            catch (Exception exc)
            {

                ModAPI.Log.Write("Area dmg exception " + exc.ToString());
            }
            return false;
        }
        public void DoGuaranteedAreaDamage(Transform rootTR, float damage)
        {
            RaycastHit[] hits = Physics.SphereCastAll(rootTR.position, AreaDamageRadius, Vector3.one, AreaDamageRadius);
            int d = Mathf.FloorToInt(damage * AreaDamage);
            if (d > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.root != rootTR.root)
                    {
                        if (hits[i].transform.tag == "enemyCollide")
                        {
                            if (GameSetup.IsMpClient)
                            {
                                BoltEntity entity = hits[i].transform.GetComponent<BoltEntity>();
                                if (entity == null)
                                {
                                    entity = hits[i].transform.GetComponentInParent<BoltEntity>();
                                }
                                if (entity != null)
                                {
                                    PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
                                    playerHitEnemy.Hit = d;
                                    playerHitEnemy.Target = entity;
                                    playerHitEnemy.Send();
                                }
                            }
                            else
                            {
                                hits[i].transform.root.SendMessage("Hit", d, SendMessageOptions.DontRequireReceiver);
                            }
                        }
                        else if (hits[i].transform.tag == "BreakableWood" || hits[i].transform.tag == "animalCollide")
                        {
                            hits[i].transform.root.SendMessage("Hit", d, SendMessageOptions.DontRequireReceiver);

                        }
                    }
                }
            }
        }

        public void LevelUp()
        {
            MutationPoints++;
            Level++;
            ExpGoal = GetGoalExp();
            SendLevelMessage();

            int ap = Mathf.RoundToInt(Mathf.Sqrt(Level));
            agility += ap;
            strenght += ap;
            vitality += ap;
            intelligence += ap;

        }
        public long GetGoalExp()
        {
            return GetGoalExp(Level);
        }
        public long GetGoalExp(int lvl)
        {
            int x = lvl;
            if (x >= 146)
                return 9000000000000000000;
           // var y = 125*System.Math.Pow(1.35f,x-10) + 20 + 5* System.Math.Pow(x, 3.1);
            var y = 120*System.Math.Pow(1.345f,x-10) + 20 + 5* x*x*x;
            y = y / 3.3;
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
                MassacreText = "TRIPLE KILL   +10% exp";
            }
            else if (KCInRange(4, 5))
            {
                MassacreMultipier = 1.25f;
                MassacreText = "QUADRA KILL   +25% exp";
            }
            else if (KCInRange(5, 6))
            {
                MassacreMultipier = 1.5f;
                MassacreText = "PENTA KILL   +50% exp";
            }
            else if (KCInRange(6, 10))
            {
                MassacreMultipier = 1.75f;
                MassacreText = "MASSACRE   +75% exp";
            }
            else if (KCInRange(10, 12))
            {
                MassacreMultipier = 2.3f;
                MassacreText = "BIG MASSACRE   +130% exp";
            }
            else if (KCInRange(12, 16))
            {
                MassacreMultipier = 3f;
                MassacreText = "BIGGER MASSACRE   +200% exp";
            }
            else if (KCInRange(16, 20))
            {
                MassacreMultipier = 5f;
                MassacreText = "HUGE MASSACRE   +400% exp";
            }
            else if (KCInRange(20, 25))
            {
                MassacreMultipier = 8.5F;
                MassacreText = "BLOODY MASSACRE   +750% exp";
            }
            else if (KCInRange(25, 30))
            {
                MassacreMultipier = 16F;
                MassacreText = "WICKED SICK   +1,500% exp";
            }
            else if (KCInRange(30, 40))
            {
                MassacreMultipier = 22.5f;
                MassacreText = "UNSTOPPABLE   +2,150% exp";
            }
            else if (KCInRange(40, 50))
            {
                MassacreMultipier = 35;
                MassacreText = "GODLIKE MASSACRE   +3,400% exp";
            }
            else if (KCInRange(50, 65))
            {
                MassacreMultipier = 50;
                MassacreText = "BEYOND GODLIKE   +4,900% exp";
            }
            else if (KCInRange(65, 75))
            {
                MassacreMultipier = 100;
                MassacreText = "SLAUGHTER   +9,900% exp";
            }
            else if (KCInRange(75, 100))
            {
                MassacreMultipier = 250;
                MassacreText = "BLOODBATH   +24,900% exp";
            }
            else if (MassacreKills >= 100)
            {
                MassacreMultipier = 1000;
                MassacreText = "R A M P A G E    +100,000% exp";
            }
        }

        private bool KCInRange(int min, int max)
        {
            if (MassacreKills >= min && MassacreKills < max)
            {
                return true;
            }
            return false;
        }

        private void FinishMassacre()
        {

            TimeUntillMassacreReset = 0;
            long Amount = Convert.ToInt64((double)NewlyGainedExp * MassacreMultipier);
            NewlyGainedExp = 0;
            MassacreMultipier = 1;
            AddFinalExperience(Amount);
        }

        public void InitializeHandHeld()
        {
            StartCoroutine(InitHandCoroutine());
        }
        IEnumerator InitHandCoroutine()
        {
            while (ModReferences.rightHandTransform == null)
            {
                yield return null;
                if (LocalPlayer.Inventory != null)
                {
                    LocalPlayer.Inventory.Equip(80, false);
                }
            }
            yield return null;

            //Multishot
            Effects.Multishot.localPlayerInstance = Effects.Multishot.Create(LocalPlayer.Transform, ModReferences.rightHandTransform);
            Effects.Multishot.localPlayerInstance.SetActive(false);

            //yield return null;
            //do
            //{
            //    if (LocalPlayer.Inventory.Owns(79))
            //        LocalPlayer.Inventory.Equip(79, false);

            //}
            //while (PlayerInventoryMod.originalBowModel == null);

        }

        public void AddGeneratedResource(int id, int amount)
        {
            if (amount > 0)
            {
                if (GeneratedResources.ContainsKey(id))
                {
                    GeneratedResources[id] += amount;

                }
                else
                {
                    GeneratedResources.Add(id, amount);
                }
            }
            else
            {
                if (GeneratedResources.ContainsKey(id))
                {
                    GeneratedResources[id] += amount;
                    if (GeneratedResources[id] <= 0)
                    {
                        GeneratedResources.Remove(id);
                    }
                }
            }
        }
        public static void UnAssignAllStats()
        {
            foreach (var item in instance.ExtraCarryingCapactity)
            {
                item.Value.Remove();
            }
            instance.ExtraCarryingCapactity.Clear();


            foreach (KeyValuePair<int, Item> item in Inventory.Instance.ItemList)
            {

                  if (item.Value == null) continue;
                if (item.Value.Equipped)
                {
                    item.Value.onUnequip?.Invoke();
                    item.Value.Equipped = false;
                    foreach (var stat in item.Value.Stats)
                    {
                try
                {
                        stat.OnUnequip?.Invoke(stat.Amount);
                }
                catch (Exception e)
                {
                            Debug.Log("err: " + e.Message);
                } 
                    }
                }
            }
            for (int i = 0; i < Perk.AllPerks.Count; i++)
            {
                if (Perk.AllPerks[i].IsBought)
                {
                    Perk.AllPerks[i].IsBought = false;
                    Perk.AllPerks[i].DisableMethods?.Invoke();
                }
            }
            
        }
        public static void ResetAllStats()
        {
           
            foreach (var item in instance.ExtraCarryingCapactity)
            {
            item.Value.Remove();
            }
            
            activeBuffs.Clear();
            SpellCaster.InfinityEnabled = false;
            SpellCaster.InfinityLoopEnabled = false;
            SpellActions.BlinkRange = 15;
            SpellActions.BlinkDamage = 0;
            SpellActions.HealingDomeGivesImmunity = false;
            SpellActions.HealingDomeRegEnergy = false;
            SpellActions.FlareDamage = 6;
            SpellActions.FlareSlow = 0.75f;
            SpellActions.FlareBoost = 1.25f;
            SpellActions.FlareHeal = 2;
            SpellActions.FlareRadius = 3.5f;
            SpellActions.FlareDuration = 8;
            SpellActions.BLACKHOLE_damage = 40;
            SpellActions.BLACKHOLE_duration = 7;
            SpellActions.BLACKHOLE_radius = 15;
            SpellActions.BLACKHOLE_pullforce = 25;
            SpellActions.ShieldPerSecond = 1;
            SpellActions.MaxShield = 10;
            SpellActions.ShieldCastTime = 0;
            SpellActions.ShieldPersistanceLifetime = 3;
            SpellActions.PurgeHeal = false;
            SpellActions.WarCryRadius = 50;
            SpellActions.CataclysmArcane = false;
            SpellActions.WarCryGiveDamage = false;
            SpellActions.WarCryGiveArmor = false;
            SpellActions.PortalDuration = 30;
            SpellActions.MagicArrowDmgDebuff = false;
            SpellActions.MagicArrowDoubleSlow = false;
            SpellActions.MagicArrowDuration = 10;
            SpellActions. SnapFreezeDist = 15;
            SpellActions. SnapFloatAmount = 0.2f;
            SpellActions. SnapFreezeDuration = 7f;
        SpellActions.BL_Damage = 120;
            SpellActions.BashExtraDamage = 1.06f;
            SpellActions.BashDamageBuff = 1f;
            SpellActions.BashSlowAmount = 0.7f;
            SpellActions.BashLifesteal = 0.0f;
            SpellActions.BashBleedChance = 0;
            SpellActions.BashBleedDmg = 0.3f;
            SpellActions.BashDuration = 3;
            SpellActions.FrenzyMaxStacks = 5;
            SpellActions.FrenzyStacks = 0;
            SpellActions.FrenzyAtkSpeed = 0.02f;
            SpellActions.FrenzyDmg = 0.075f;
            SpellActions.FocusBonusDmg =0;
            SpellActions.FocusOnHS = 1;
            SpellActions.FocusOnBS = 0.2f;
            SpellActions.FocusOnAtkSpeed = 1.3f;
            SpellActions.FocusSlowAmount = 0.5f;
            SpellActions.FocusSlowDuration = 4;
            SpellActions.SeekingArrow_ChangeTargetOnHit = false;
            SpellActions.SeekingArrow_TimeStamp = 0;
            SpellActions.SeekingArrow_HeadDamage = 2;
            SpellActions.SeekingArrow_SlowDuration = 4;
            SpellActions.FocusOnAtkSpeedDuration = 4;
            SpellActions.SeekingArrow_SlowAmount = 0.4f;
            SpellActions.SeekingArrowDuration = 30f;
            SpellActions.SeekingArrow_DamagePerDistance = 0.01f;
            SpellActions.ChanceToParryOnHit = false;
            SpellActions.ParryIgnites = false;
            SpellActions.ParryRadius = 3.5f;
            SpellActions.ParryDamage = 40;
            SpellActions.ParryBuffDuration = 10;
            SpellActions.Focus = false;
            SpellActions.ParryHeal = 5;
            SpellActions.ParryEnergy = 10;
            SpellActions. CataclysmDamage = 24;
SpellActions. CataclysmDuration = 12;
SpellActions. CataclysmRadius = 5;
            SpellActions.BIA_bonusDamage=0;
        SpellActions.BIA_SpellDmMult = 1.2f;
        SpellActions.BIA_HealthDmMult = 3f;
            SpellActions.BIA_HealthTakenMult = 0.65f;
        BlackFlame.DmgAmp = 1;
            BlackFlame.GiveAfterburn =false;
            BlackFlame.GiveDamageBuff =false;
            WeaponInfoMod.AlwaysIgnite = false;
            AutoPickupItems.radius = 7.5f;
            Berserker.active = false;
            instance.HealingMultipier = 1;
            instance.strenght = 1;
            instance.intelligence = 1;
            instance.agility = 1;
            instance.vitality = 1;
            instance.AssignLevelAttributes();
            instance.DamagePerStrenght = 0.00f;
            instance.SpellDamageperInt = 0.00f;
            instance.RangedDamageperAgi = 0.00f;
            instance.ArmorReduction = 0;
            instance.EnergyRegenPerInt = 0.00f;
            instance.EnergyPerAgility = 0f;
            instance.HealthPerVitality = 0f;
            instance.HealthRegenPercent = 0;
            instance.StaminaRegenPercent = 0;
            instance.HealthBonus = 0;
            instance.EnergyBonus = 0;
            instance.MaxHealthPercent = 0;
            instance.MaxEnergyPercent = 0;
            instance.GreatBowIgnites = false;
            instance.CoolDownMultipier = 1;
            instance.SpellDamageAmplifier_Mult = 1;
            instance.MeleeDamageAmplifier_Mult = 1;
            instance.RangedDamageAmplifier_Mult = 1;
            instance.SpellDamageAmplifier_Add = 1;
            instance.MeleeDamageAmplifier_Add = 1;
            instance.RangedDamageAmplifier_Add = 1;
            instance.SpellDamageBonus = 0;
            instance.MeleeDamageBonus = 0;
            instance.RangedDamageBonus = 0;
            instance.MeleeRange = 1;
            instance.DamageReduction = 1;
            instance.DamageReductionPerks = 1;
            instance.DamageOutputMult = 1;
            instance.DamageOutputMultPerks = 1;
            instance.CritChance = 0.05f;
            instance.CritDamage = 50;
            instance.LifeOnHit = 0;
            instance.LifeRegen = 0;
            instance.StaminaRegen = 0;
            instance.DodgeChance = 0;
            instance.SlowAmount = 0;
            instance.Silenced = false;
            instance.Rooted = false;
            instance.Stunned = false;
            instance.Armor = 0;
            instance.MagicResistance = 0;
            instance.CraftingReroll = false;
            instance.CraftingReforge = false;
            instance.BunnyHopUpgrade = false;
            instance.AttackSpeedMult = 1;
            instance.AttackSpeedAdd = 1;
            instance.StunImmune = 0;
            instance.RootImmune = 0;
            instance.DebuffImmune = 0;
            instance.DebuffResistant = 0;
            instance.MoveSpeed = 1f;
            instance.JumpPower = 1f;
            instance.SpellCostToStamina = 0;
            instance.SpellCostRatio = 1;
            instance.StaminaAttackCost = 0;
            instance.BlockFactor = 0.5f;
            instance.ExpFactor = 1;
            instance.MaxMassacreTime = 20;
            instance.TimeBonusPerKill = 5;
            instance.EnergyOnHit = 0;
            instance.EnergyPerSecond = 0;
            instance.ARreduction_all = 0;
            instance.ARreduction_melee = 0;
            instance.ARreduction_ranged = 0;
            instance.damageAbsorbAmounts = new float[2];
            instance.StealthDamage = 1;
            instance.ThirstRate = 1;
            instance.HungerRate = 1;
            instance.FireDamageTakenMult = 1;
            instance.AreaDamageProcChance = 0.15f;
            instance.AreaDamage = 0;
            instance.AreaDamageRadius = 4;
            instance.ProjectileSpeedRatio = 1f;
            instance.ProjectileSizeRatio = 1;
            instance.GeneratedResources.Clear();
            instance.MagicFindMultipier = 1;
            Items.StatActions.AddMagicFind(0);
            instance.ReusabilityChance = 0;
            instance.ReusabilityAmount = 1;
            instance.IsSmokeysQuiver = false;
            instance.IsHammerStun = false;
            instance.KingQruiesSpecial = false;
            instance.HammerStunDuration = 0.4f;
            instance.HammerStunAmount = 0.25f;
            instance.HammerSmashDamageAmp = 1f;
            instance.flashlightIntensity = 1;
            instance.flashlightBatteryDrain = 1;
            instance. RangedAttackSpeed = 1;
            instance.HexedPantsOfMrM_Enabled = false;
            instance.HexedPantsOfMrM_StandTime = 0;
            instance.DeathPact_Enabled = false;
            instance.DeathPact_Amount = 1;
            instance.SoraSpecial = false;
            instance.ExtraCarryingCapactity.Clear();
            instance.SpearDamageMult = 1;
            instance.isGreed = false;
            instance.BunnyHop = false;
            instance.BulletDamageMult = 1;
            instance.CrossbowDamageMult = 1;
            instance.BowDamageMult = 1;
            instance.HeavyAttackMult = 1;
            instance.IsCrossfire = false;
            instance.SpearArmorRedBonus = false;
            instance.MultishotCount = 1;
            instance.SpearCritChance = 0.04f;
            instance.SpearhellChance = 0.00f;
            instance. BulletCritChance = 0.1f;
        instance.TurboRaft = false;
            instance.RaftSpeedMultipier = 1;
            instance.FireAmp=0;
            instance.HeadShotDamage = 6;
            instance.SpellAmpFireDmg = false;
            instance.ProjectileDamageIncreasedBySize = false;
            instance.ChanceToSlowOnHit = 0;
            instance.ChanceToBleedOnHit = 0;
            instance.ChanceToWeakenOnHit = 0;
            instance.NearDeathExperience = false;
            instance.NearDeathExperienceUnlocked = false;
            instance.HammerStunDuration = 0.4f;
            instance.HammerStunAmount = 0.25f;
            instance.HammerSmashDamageAmp = 1f;
            instance.IsHazardCrown = false;
            ReapplyAllItems();
            ReapplyAllPerks();
            ReapplyAllSpell();
            foreach (var extraItem in instance.ExtraCarryingCapactity)
            {
                extraItem.Value.NewApply();
            }
        }

        public static void ReapplyAllSpell()
        {
            for (int i = 0; i < SpellCaster.SpellCount; i++)
            {
                SpellCaster.instance.infos[i].spell?.passive?.Invoke(true);   
            }
        }

        public static void ReapplyAllItems()
        {
            //items
            foreach (int key in Inventory.Instance.ItemList.Keys)
            {
                if (Inventory.Instance.ItemList[key] != null)
                {
                    Inventory.Instance.ItemList[key].Equipped = false;
                }
            }
                    }
        public static void ReapplyAllPerks()
        {
            //perks
            for (int i = 0; i < Perk.AllPerks.Count; i++)
            {
                if (Perk.AllPerks[i].IsBought)
                {
                    if (Perk.AllPerks[i].Endless)
                    {
                        for (int j = 0; j < Perk.AllPerks[i].ApplyAmount; j++)
                        {
                    Perk.AllPerks[i].ApplyMethods();

                        }
                    }
                    else 
                    Perk.AllPerks[i].ApplyMethods();
                }
            }
        }

        public static void Respec()
        {
            MainMenu.Instance.StartCoroutine(  MainMenu.Instance.FadeMenuSwitch(MainMenu.OpenedMenuMode.Hud));
            LocalPlayer.Stats.Health = 1;
            LocalPlayer.Stats.HealthTarget = 1;
            LocalPlayer.Stats.Energy = 1;
            UnAssignAllStats();
            Effects.Sound_Effects.GlobalSFX.Play(2);


            instance.MutationPoints = instance.Level + instance.PermanentBonusPerkPoints;
            foreach (int i in SpellDataBase.spellDictionary.Keys)
            {
                SpellDataBase.spellDictionary[i].Bought = false;
            }
            for (int i = 0; i < SpellCaster.SpellCount; i++)
            {
                SpellCaster.instance.SetSpell(i);
            }


            for (int i = 0; i < Perk.AllPerks.Count; i++)
            {

                Perk.AllPerks[i].IsBought = false;
                Perk.AllPerks[i].Applied = false;
                Perk.AllPerks[i].ApplyAmount = 0;


            }
            ResetAllStats();
        }

        public static int RangedRepetitions()
        {
            int repeats = 1;
            if (Effects.Multishot.IsOn)
            {
                bool b = ModdedPlayer.instance.SoraSpecial ? SpellCaster.RemoveStamina(0.5f * ModdedPlayer.instance.MultishotCount * ModdedPlayer.instance.MultishotCount * ModdedPlayer.instance.MultishotCount) : SpellCaster.RemoveStamina(5 * ModdedPlayer.instance.MultishotCount * ModdedPlayer.instance.MultishotCount * ModdedPlayer.instance.MultishotCount);
                if (b)
                {
                    repeats += ModdedPlayer.instance.MultishotCount;
                    if (ModdedPlayer.instance.SoraSpecial) repeats += 4;

                }
                else
                {
                    Effects.Multishot.IsOn = false;
                    Effects.Multishot.localPlayerInstance.SetActive(false);
                }
            }
            return repeats;
        }
    }
}
