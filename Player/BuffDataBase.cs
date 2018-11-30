using System.Collections.Generic;
namespace ChampionsOfForest
{
    public class BuffDataBase
    {
        public static Dictionary<int, Buff> activeBuffs;
        public static Dictionary<int, Buff> buffs;

        public static bool AddBuff(int id, int source, float amount, float duration)
        {
            if (buffs.ContainsKey(id))
            {
                if (!ModdedPlayer.instance.StunImmune || !buffs[id].isNegative)
                {
                    if (activeBuffs.ContainsKey(source))
                    {
                        activeBuffs[id].duration = duration;
                        if (activeBuffs[id].amount < amount)
                        {
                            activeBuffs[id].amount = amount;
                        }
                    }
                    else
                    {
                        activeBuffs.Add(id, new Buff(source, amount, duration));

                    }
                }
            }
            return false;
        }
        public static void Init()
        {
            activeBuffs = new Dictionary<int, Buff>();
            buffs = new Dictionary<int, Buff>();

            //adding buffs to the database
            new Buff(1, "Movement speed", true, new Buff.onEnd(ModdedPlayer.BUFF_DivideMS), new Buff.onStart(ModdedPlayer.BUFF_MultMS));
            new Buff(2, "Attack speed", true, new Buff.onEnd(ModdedPlayer.BUFF_DivideAS), new Buff.onStart(ModdedPlayer.BUFF_MultAS));

        }
    }
}
