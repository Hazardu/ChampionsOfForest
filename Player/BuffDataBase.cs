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
                            if (activeBuffs[source].amount < amount)
                            {
                                activeBuffs[source].amount = amount;
                            }
                            return true;
                        }
                        else
                        {
                            Buff b = new Buff(id, amount, duration);
                            activeBuffs.Add(source, b);
                            b.OnStart(b.amount);
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
                new Buff(1, "Move speed reduced", true, SpellActions.BUFF_DivideMS, SpellActions.BUFF_MultMS);
                new Buff(2, "Attack speed reduced", true, SpellActions.BUFF_DivideAS, SpellActions.BUFF_MultAS);
            }
            catch (System.Exception ex)
            {
                ModAPI.Console.Write(ex.ToString());

            }
        }

        public class Buff
        {
            public int _ID;
            public float amount;
            public float duration;
            public string BuffName;
            public bool isNegative;
            public delegate void onBuffEnd(float f);
            public delegate void onBuffStart(float f);
            public onBuffEnd OnEnd;
            public onBuffStart OnStart;



            public Buff(int id, float amount, float duration)
            {
                _ID = id;
                Buff b = BuffDB.BuffsByID[id];
                OnEnd = b.OnEnd;
                OnStart = b.OnStart;
                BuffName = b.BuffName;
                isNegative = b.isNegative;
                this.amount = amount;
                this.duration = duration;
            }


            public Buff(int BuffID, string name, bool IsNegative, onBuffEnd END = null, onBuffStart START = null)
            {
                _ID = BuffID;

                isNegative = IsNegative;
                BuffName = name;
                OnEnd = END;
                OnStart = START;
                amount = 1;
                duration = 1;
                BuffDB.BuffsByID.Add(BuffID, this);

            }

            /// <summary>
            /// determines if the buff is already over. Call this in Modded player update
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
                    OnEnd(amount);
                }

            }
        }
    }
}
