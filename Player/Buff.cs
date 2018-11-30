using UnityEngine;
namespace ChampionsOfForest
{
    public class Buff
    {
        public int ID;
        public float amount;
        public float duration;
        public string name;
        public bool isNegative;
        public delegate void onEnd(float f);
        public delegate void onStart(float f);
        public onEnd end;
        public onStart start;

        /// <summary>
        /// recreates a buff from one with the given from the database. sets the values and calls onStart
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <param name="duration"></param>
        public Buff(int id, float amount, float duration)
        {
            ID = id;
            Buff b = BuffDataBase.buffs[id];
            end = b.end;
            start = b.start;
            name = b.name;
            isNegative = b.isNegative;
            this.amount = amount;
            this.duration = duration;
            start(amount);
            BuffDataBase.activeBuffs.Add(id, this);
        }

        /// <summary>
        /// creates a buff and adds to the database
        /// </summary>
        public Buff(int sourceid, string name, bool IsNegative, onEnd ONEND, onStart ONSTART)
        {
            isNegative = IsNegative;
            ID = sourceid;
            this.name = name;
            end = ONEND;
            start = ONSTART;
            if (!BuffDataBase.buffs.ContainsKey(ID))
            {
                BuffDataBase.buffs.Add(ID, this);
            }
            else
            {
                ModAPI.Log.Write("Failed adding a buff to the database. Buff with given id exists in the database: name = " + name);
            }
        }

        /// <summary>
        /// determines if the buff is already over. Call this in Modded player update
        /// </summary>
        public void Update()
        {
            if (duration > 0)
            {
                duration -= Time.deltaTime;
            }
            else
            {
                BuffDataBase.activeBuffs.Remove(ID);
                end(amount);
            }

        }
    }
}
