using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest
{
    public class ClinetEnemyProgression
    {
        public static float LifeTime = 10;
        public BoltEntity Entity;
        public ulong Packed;
        public string EnemyName;
        public int Level;
        public int Health;
        public int MaxHealth;
        public long ExpBounty;
        public int Armor;
        public int ArmorReduction;
        public float SteadFest;
        public int[] Affixes;
        public float creationTime;
        
        

        public ClinetEnemyProgression(Transform tr)
        {
            creationTime = Time.time;
            EnemyProgression p = tr.GetComponent<EnemyProgression>();
            if (p == null)
            {
                p = tr.GetComponentInChildren<EnemyProgression>();
            }
            if (p != null)
            {
                EnemyName = p.EnemyName;
                Level = p.Level;
                Health = (int)p.Health;
                MaxHealth = (int)p.MaxHealth;
                ExpBounty = p.Bounty;
                Armor = p.Armor;
                ArmorReduction = p.ArmorReduction;
                SteadFest = p.SteadFest;
                Affixes = new int[p.abilities.Count];
                for (int i = 0; i < p.abilities.Count; i++)
                {
                    Affixes[i] = (int)p.abilities[i];
                }
            }
        }
        public ClinetEnemyProgression()
        {

        }
        public ClinetEnemyProgression(BoltEntity e)
        {

            creationTime = Time.time;
            Entity = e;
            Packed = e.networkId.PackedValue;
            if (GameSetup.IsMpClient)
            {
                Network.NetworkManager.SendLine("EE" + Packed.ToString() + ";", Network.NetworkManager.Target.OnlyServer);
            }
            else
            {
                EnemyProgression p = EnemyManager.hostDictionary[Packed];
                EnemyName = p.EnemyName;
                Level = p.Level;
                Health = (int)p.Health;
                MaxHealth = (int)p.MaxHealth;
                ExpBounty = p.Bounty;
                Armor = p.Armor;
                ArmorReduction = p.ArmorReduction;
                SteadFest = p.SteadFest;
                Affixes = new int[p.abilities.Count];
                for (int i = 0; i < p.abilities.Count; i++)
                {
                    Affixes[i] = (int)p.abilities[i];
                }
            }
            if (!EnemyManager.clinetProgressions.ContainsKey(e))
            {
                EnemyManager.clinetProgressions.Add(e, this);
            }

        }

        public ClinetEnemyProgression(BoltEntity entity, string enemyName, int level, float health, float maxHealth, int expBounty, int armor, int armorReduction, float steadFest, int[] affixes) : this(entity)
        {
            Entity = entity;
            EnemyName = enemyName;
            Packed = entity.networkId.PackedValue;
            Level = level;
            Health = (int)health;
            MaxHealth = (int)maxHealth;
            ExpBounty = expBounty;
            Armor = armor;
            ArmorReduction = armorReduction;
            SteadFest = steadFest;
            Affixes = affixes;
            creationTime = Time.time;
            if (!EnemyManager.clinetProgressions.ContainsKey(entity))
            {
                EnemyManager.clinetProgressions.Add(entity, this);
            }
        }

    }
}
