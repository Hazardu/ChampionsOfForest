using ChampionsOfForest.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest.Player
{
    public static class BuffDB
    {
        public static Dictionary<int, Buff> activeBuffs = new Dictionary<int, Buff>();
        public static Dictionary<int, Buff> BuffsByID = new Dictionary<int, Buff>();

        public static bool AddBuff(int id, int source, float amount, float duration)
        {
            try
            {
                if (BuffsByID.ContainsKey(id))
                {
                    if (BuffsByID[id].isNegative && BuffsByID[id].DispellAmount <= 1 && (ModdedPlayer.instance.DebuffImmune > 0 || ModdedPlayer.instance.DebuffResistant > 0)) return false;
                    if (BuffsByID[id].isNegative && BuffsByID[id].DispellAmount <= 2 && (ModdedPlayer.instance.DebuffImmune > 0)) return false;
                    if (activeBuffs.ContainsKey(source))
                    {
                        activeBuffs[source].duration = duration;
                        if (activeBuffs[source].OnAddOverrideAmount)
                        {
                            activeBuffs[source].amount = amount;
                        }
                        else
                        {
                            if (activeBuffs[source].AccumulateEffect)
                            {
                                activeBuffs[source].amount += amount;

                            }
                            else if (activeBuffs[source].amount < amount)
                            {

                                activeBuffs[source].amount = amount;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        Buff b = new Buff(id, amount, duration);
                        activeBuffs.Add(source, b);
                        if (b.OnStart != null)
                        {
                            b.OnStart(b.amount);
                        }

                        return true;

                    }
                }
                else
                {
                    ModAPI.Log.Write("Couldnt add a buff, no buff with id " + id + " in the database");
                    return false;
                }
            }
            catch (System.Exception e)
            {

                ModAPI.Log.Write("Error when adding buff... " + e.ToString());

            }
            return false;
        }

        public static void FillBuffList()
        {
            try
            {
                activeBuffs.Clear();
                BuffsByID.Clear();
                new Buff(1, "Move speed reduced", true, false, 1, SpellActions.BUFF_DivideMS, SpellActions.BUFF_MultMS);
                new Buff(2, "Attack speed reduced", true, false, 1, SpellActions.BUFF_DivideAS, SpellActions.BUFF_MultAS);
                new Buff(3, "Poisoned", true, true, 2)
                { DisplayAsPercent = false };
                new Buff(4, "Root Immune", false, false, 0, (f) => ModdedPlayer.instance.RootImmune--, f => ModdedPlayer.instance.RootImmune++)
                { DisplayAmount = false };

                new Buff(5, "Move speed increased", false, false, 1, SpellActions.BUFF_DivideMS, SpellActions.BUFF_MultMS);

                new Buff(6, "Stun Immune", false, false, 0, (f) => ModdedPlayer.instance.StunImmune--, f => ModdedPlayer.instance.StunImmune++)
                { DisplayAmount = false };
                new Buff(7, "Debuff Immune", false, false, 0, (f) => ModdedPlayer.instance.DebuffImmune--, f => ModdedPlayer.instance.DebuffImmune++)
                { DisplayAmount = false };
                new Buff(8, "Debuff Resistant", false, false, 0, (f) => ModdedPlayer.instance.DebuffResistant--, f => ModdedPlayer.instance.DebuffResistant++)
                { DisplayAmount = false };


                new Buff(9, "Increased Damage", false, false, 0, f => ModdedPlayer.instance.DamageOutputMult /= f, f => ModdedPlayer.instance.DamageOutputMult *= f);

                new Buff(10, "Decreased Damage", true, false, 2, f => ModdedPlayer.instance.DamageOutputMult /= f, f => ModdedPlayer.instance.DamageOutputMult *= f);

                new Buff(11, "Energy Regen Amp", false, false, 0, f => ItemDataBase.RemovePercentage(ref ModdedPlayer.instance.StaminaRegenPercent, f), f => ItemDataBase.AddPercentage(ref ModdedPlayer.instance.StaminaRegenPercent, f));

                new Buff(12, "Death Pact Damage", false, false) { OnAddOverrideAmount = true };
                new Buff(13, "Increased Melee Damage", false, false, 0, f => ModdedPlayer.instance.MeleeDamageAmplifier_Mult /= f, f => ModdedPlayer.instance.MeleeDamageAmplifier_Mult *= f);
                new Buff(14, "Attack speed increased", false, false, 1, SpellActions.BUFF_DivideAS, SpellActions.BUFF_MultAS);

                new Buff(15, "Armor", false, false, 1, f => ModdedPlayer.instance.Armor -= Mathf.RoundToInt(f), f => ModdedPlayer.instance.Armor += Mathf.RoundToInt(f)) { DisplayAsPercent = false }; ;

                new Buff(16, "Gold", false, false, 1, f => Gold.Disable(), f => Gold.Enable()) { DisplayAmount = false };

                new Buff(17, "Berserk", false, false, 1, f =>Berserker.OnDisable(),f=> Berserker.OnEnable()) { DisplayAmount = false }; 

                new Buff(18, "Energy Leak", true, false, 1, f =>ModdedPlayer.instance.EnergyPerSecond+=f,f=> ModdedPlayer.instance.EnergyPerSecond -= f); 

            }
            catch (System.Exception ex)
            {
                ModAPI.Log.Write(ex.ToString());

            }
        }

        public class Buff
        {
            public int _ID;
            public float amount;
            public float duration;
            public string BuffName;
            public bool isNegative;
            public bool AccumulateEffect;
            public delegate void onBuffEnd(float f);
            public delegate void onBuffStart(float f);
            public onBuffEnd OnEnd;
            public onBuffStart OnStart;
            public int DispellAmount;
            public bool DisplayAsPercent = true;
            public bool DisplayAmount = true;
            public bool OnAddOverrideAmount = false;
            public Buff(int id, float amount, float duration)
            {
                _ID = id;
                Buff b = BuffDB.BuffsByID[id];
                OnEnd = b.OnEnd;
                OnStart = b.OnStart;
                BuffName = b.BuffName;
                isNegative = b.isNegative;
                DispellAmount = b.DispellAmount;
                AccumulateEffect = b.AccumulateEffect;
                OnAddOverrideAmount = b.OnAddOverrideAmount;
                DisplayAsPercent = b.DisplayAsPercent;
                this.amount = amount;
                this.duration = duration;
            }


            public Buff(int BuffID, string name, bool IsNegative, bool accumulate, int dispellamount = 0, onBuffEnd END = null, onBuffStart START = null)
            {
                _ID = BuffID;
                AccumulateEffect = accumulate;
                isNegative = IsNegative;
                BuffName = name;
                OnEnd = END;
                OnStart = START;
                DispellAmount = dispellamount;
                amount = 1;
                duration = 1;
                BuffDB.BuffsByID.Add(BuffID, this);

            }

            public void ForceEndBuff(int source)
            {
                BuffDB.activeBuffs.Remove(source);
                if (OnEnd != null)
                {
                    OnEnd(amount);
                }
            }


            /// <summary>
            /// determines if the buff is already over. Call this in ModdedPlayer update
            /// </summary>
            public void UpdateBuff(int source)
            {

                if (isNegative && DispellAmount <= 1 && (ModdedPlayer.instance.DebuffImmune > 0 || ModdedPlayer.instance.DebuffResistant > 0)) duration = 0;
                if (isNegative && DispellAmount <= 2 && (ModdedPlayer.instance.DebuffImmune > 0)) duration=0;


                if (duration > 0)
                {
                    duration -= Time.deltaTime;
                }
                else
                {
                    BuffDB.activeBuffs.Remove(source);
                    if (OnEnd != null)
                    {
                        OnEnd(amount);
                    }
                }

            }
        }
    }
}



//SOURCES of debuffs
//5 - flare slow
//6 - flare speed
//30 & 31 - absolute zero
//32 - poison from enemy hit
//33 - poisons slow
//40 - immunity to cc
//41 - Hexing Pants dmg amp
//42 - Hexing Pants regen amp
//43 - Death Pact dmg amp
//44 - Black flame dmg amp
//45 - warcry ms
//46 - warcry as
//47 - warcry dmg
//48 - warcry armor
//49 - gold
//50 - berserker
//51 - berserker energy leak

