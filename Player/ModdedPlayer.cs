using Bolt;
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
        public readonly float baseHealth = 30;
        public readonly float baseEnergy = 30;
        public static float basejumpPower;
        public static ModdedPlayer instance = null;


        public float MaxHealth
        {
            get
            {
                float x = baseHealth + vitality * HealthPerVitality;
                x += HealthBonus;
                x *= 1 + MaxHealthPercent;
                return x;
            }
        }
        public float MaxEnergy
        {
            get
            {
                float x = baseEnergy + agility * EnergyPerAgility;
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
                float f = agility * RangedDamageperAgi;
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
        public float StaminaAndEnergyRegenAmp => 1 + intelligence * EnergyRegenPerInt;
        public float ArmorDmgRed => ModReferences.DamageReduction(Armor);
        public float DamageReductionTotal => (DamageReduction) * (DamageReductionPerks);
        public float DamageOutputMultTotal => DamageOutputMultPerks * DamageOutputMult;
   public int MeleeArmorReduction => ARreduction_all + ARreduction_melee;
        public int RangedArmorReduction => ARreduction_all + ARreduction_ranged;



        public int Level = 1;
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
        public float CritChance = 0;
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
        public float MagicResistance = 0;


        public float AttackSpeed => AttackSpeedAdd * AttackSpeedMult;
        public float AttackSpeedMult = 1;
        public float AttackSpeedAdd = 1;



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
        public TheForest.Items.Item lastShotProjectile;
        public float SpearDamageMult = 1;
        public float BulletDamageMult = 1;
        public float CrossbowDamageMult = 1;
        public float BowDamageMult = 1;
        public int MultishotCount = 1;

        //Item abilities variables
        //Smokeys quiver
        public bool IsSacredArrow = false;
        public bool IsCrossfire = false;
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
                        if (LocalPlayer.Inventory.AmountOf(290) > 1)
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
            else
            {
                ExtraItemCapacity EIC = new ExtraItemCapacity(ID, Amount);
                EIC.NewApply();
                ExtraCarryingCapactity.Add(ID, EIC);
            }
            if (ExtraCarryingCapactity[ID].Amount <= 0)
            {
                ExtraCarryingCapactity.Remove(ID);
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

        }
        public void SendLevelMessage()
        {
            if (ChatBoxMod.instance != null)
            {
                if (BoltNetwork.isRunning)
                {
                    NetworkManager.SendLine("II" + LocalPlayer.Entity.GetState<IPlayerState>().name + " HAS REACHED LEVEL " + ModdedPlayer.instance.Level + "!", NetworkManager.Target.Everyone);
   
                }
            }
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
                    NetworkManager.SendLine(s, NetworkManager.Target.Everyone);
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
                    NetworkManager.SendLine(s, NetworkManager.Target.Everyone);
                }
            }
        }

        private void Update()
        {

            if (ModAPI.Input.GetButtonDown("EquipWeapon"))
            {
                if (Inventory.Instance.ItemList[-12] != null)
                {
                    if (Inventory.Instance.ItemList[-12].Equipped)
                    {
                        PlayerInventoryMod.ToEquipWeaponType = Inventory.Instance.ItemList[-12].weaponModel;
                        LocalPlayer.Inventory.StashEquipedWeapon(false);
                        LocalPlayer.Inventory.Equip(80, false);

                        if (BoltNetwork.isRunning)
                        {
                            Network.NetworkManager.SendLine("CE" + ModReferences.ThisPlayerPacked + ";" + (int)PlayerInventoryMod.ToEquipWeaponType, NetworkManager.Target.Others);
                        }


                        PlayerInventoryMod.ToEquipWeaponType = BaseItem.WeaponModelType.None;


                    }
                }
            }

            try
            {

                float dmgPerSecond = 0;
                int poisonCount = 0;
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
                    LocalPlayer.Stats.Hit(1000, true, PlayerStats.DamageType.Drowning);
                }

            }
            catch (Exception e)
            {
                ModAPI.Log.Write("Poisoning player error" + e.ToString());
            }



            if (LocalPlayer.Stats != null)
            {
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

                DeathPact_Amount = 1 + Mathf.RoundToInt((1 - (LocalPlayer.Stats.Health / MaxHealth)) * 100) * 0.03f;
                AddBuff(12, 43, DeathPact_Amount - 1, 1f);

                DamageOutputMult *= DeathPact_Amount;

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
            while (ModdedPlayer.instance.ExpCurrent >= ModdedPlayer.instance.ExpGoal)
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
                    NetworkManager.SendLine("AL" + ModReferences.ThisPlayerPacked + ";" + ModdedPlayer.instance.Level + ";", NetworkManager.Target.Everyone);
                }
            }
        }
        public void DoOnHit()
        {
            try
            {
                LocalPlayer.Stats.HealthTarget += LifeOnHit * HealingMultipier;
                LocalPlayer.Stats.Health += LifeOnHit * HealingMultipier;
                LocalPlayer.Stats.Energy += EnergyOnHit * StaminaAndEnergyRegenAmp;
            }
            catch (Exception exc)
            {

                ModAPI.Log.Write("Area dmg exception " + exc.ToString());
            }
        }
        public void DoRangedOnHit()
        {
            if (ReusabilityChance > 0 && Random.value <= ReusabilityChance)
            {
                if (lastShotProjectile != null)
                {
                    LocalPlayer.Inventory.AddItem(lastShotProjectile._ammoItemId, ReusabilityAmount);
                }
            }
        }
        public void DoMeleeOnHit()
        {

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
        }
        public long GetGoalExp()
        {
            return GetGoalExp(Level);
        }
        public long GetGoalExp(int lvl)
        {
            int x = lvl;
            float a = 3.3f;
            float b = 3f;
            float c = 60;
            double y = System.Math.Pow(x, a) * b + c * x;
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
                MassacreText = "WICKED SICK";
            }
            else if (KCInRange(30, 40))
            {
                MassacreMultipier = 22.5f;
                MassacreText = "UNSTOPPABLE";
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
            foreach (KeyValuePair<int, Item> item in Inventory.Instance.ItemList)
            {
                if (item.Value.Equipped)
                {
                    item.Value.onUnequip();
                    item.Value.Equipped = false;
                }
            }
            for (int i = 0; i < Perk.AllPerks.Count; i++)
            {
                if (Perk.AllPerks[i].IsBought)
                {
                    Perk.AllPerks[i].IsBought = false;
                    Perk.AllPerks[i].DisableMethods();
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
            SpellActions.BlinkRange = 15;
            SpellActions.BlinkDamage = 0;
            SpellActions.HealingDomeGivesImmunity = false;
            SpellActions.FlareDamage = 6;
            SpellActions.FlareSlow = 0.75f;
            SpellActions.FlareBoost = 1.25f;
            SpellActions.FlareHeal = 2;
            SpellActions.FlareRadius = 3.5f;
            SpellActions.FlareDuration = 8;
            SpellActions.BLACKHOLE_damage = 40;
            SpellActions.BLACKHOLE_duration = 7;
            SpellActions.BLACKHOLE_radius = 12;
            SpellActions.BLACKHOLE_pullforce = 10;
            SpellActions.ShieldPerSecond = 1;
            SpellActions.MaxShield = 10;
            SpellActions.ShieldCastTime = 0;
            SpellActions.ShieldPersistanceLifetime = 3;
                    SpellActions.WarCryRadius = 50;
        SpellActions.WarCryGiveDamage = false;
        SpellActions.WarCryGiveArmor = false;
        SpellActions.PortalDuration = 30;
        SpellActions.MagicArrowDmgDebuff = false;
        SpellActions.MagicArrowDoubleSlow = false;
        SpellActions.MagicArrowDuration = 5f;
        WeaponInfoMod.AlwaysIgnite = false;
            AutoPickupItems.radius = 5;
            instance.HealingMultipier = 1;
            instance.strenght = 1;
            instance.intelligence = 1;
            instance.agility = 1;
            instance.vitality = 1;
            instance.DamagePerStrenght = 0.00f;
            instance.SpellDamageperInt = 0.00f;
            instance.RangedDamageperAgi = 0.00f;
            instance.EnergyRegenPerInt = 0.00f;
            instance.EnergyPerAgility = 0f;
            instance.HealthPerVitality = 0f;
            instance.HealthRegenPercent = 0;
            instance.StaminaRegenPercent = 0;
            instance.HealthBonus = 0;
            instance.EnergyBonus = 0;
            instance.MaxHealthPercent = 0;
            instance.MaxEnergyPercent = 0;
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
            instance.CritChance = 0;
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
            instance.ReusabilityChance = 0;
            instance.ReusabilityAmount = 1;
            instance.IsSacredArrow = false;
            instance.IsHammerStun = false;
            instance.HammerStunDuration = 0.4f;
            instance.HammerStunAmount = 0.25f;
            instance.HammerSmashDamageAmp = 1f;
            instance.HexedPantsOfMrM_Enabled = false;
            instance.HexedPantsOfMrM_StandTime = 0;
            instance.DeathPact_Enabled = false;
            instance.DeathPact_Amount = 1;
            instance.ExtraCarryingCapactity.Clear();
            instance.SpearDamageMult = 1;
            instance.BulletDamageMult = 1;
            instance.CrossbowDamageMult = 1;
            instance.BowDamageMult = 1;
            instance.HeavyAttackMult = 1;
            instance.IsCrossfire = false;
            instance.MultishotCount = 1;
            ReapplyAllItems();
            ReapplyAllPerks();
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
                    Perk.AllPerks[i].ApplyMethods();
                }
            }
        }

        public static void Respec()
        {
            UnAssignAllStats();


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

            }
            ResetAllStats();
        }
    }
}
