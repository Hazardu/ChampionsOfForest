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
                    if (!ModdedPlayer.instance.StunImmune)
                    {
                        if (activeBuffs.ContainsKey(source))
                        {
                            activeBuffs[source].duration = duration;
                            if (activeBuffs[source].AccumulateEffect)
                            {
                                activeBuffs[source].amount += amount;

                            }
                            else if (activeBuffs[source].amount < amount)
                            {
                                activeBuffs[source].amount = amount;
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
                new Buff(3, "Poisoned", true, true, 2);
                new Buff(4, "CC Immune", false, false, 0, (f) => ModdedPlayer.instance.DebuffImmune = false, f => ModdedPlayer.instance.DebuffImmune = true);

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
//30 & 31 - absolute zero
//32 - poison from enemy hit
//33 - poisons slow
//40 - immunity to cc
