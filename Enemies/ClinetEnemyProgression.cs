//Stores info about enemy stats, shared between players in coop

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class ClinetEnemyProgression
	{
		public static float LifeTime = 5;
		public BoltEntity Entity;
		public ulong Packed;
		public string EnemyName;
		public int Level;
		public float Health;
		public float MaxHealth;
		public long ExpBounty;
		public int Armor;
		public int ArmorReduction;
		public float Steadfast;
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
				EnemyName = p.enemyName;
				Level = p.Level;
				Health = p.extraHealth + p.HealthScript.Health;
				MaxHealth = p.maxHealth;
				ExpBounty = p.bounty;
				Armor = p.Armor;
				ArmorReduction = p.ArmorReduction;
				Steadfast = p.Steadfast;
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
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(6);
						w.Write(Packed);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
			}
			else
			{
				Debug.Log("Enemy in dictionary + " + Packed + " contains: " + EnemyManager.hostDictionary.ContainsKey(Packed));
				EnemyProgression p = EnemyManager.hostDictionary[Packed];
				EnemyName = p.enemyName;
				Level = p.Level;
				Health = p.extraHealth + p.HealthScript.Health;
				MaxHealth = p.maxHealth;
				ExpBounty = p.bounty;
				Armor = p.Armor;
				ArmorReduction = p.ArmorReduction;
				Steadfast = p.Steadfast;
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

		public ClinetEnemyProgression(BoltEntity entity, string enemyName, int level, float health, float maxHealth, long expBounty, int armor, int armorReduction, float Steadfast, int[] affixes) : this(entity)
		{
			Entity = entity;
			EnemyName = enemyName;
			Packed = entity.networkId.PackedValue;
			Level = level;
			Health = health;
			MaxHealth = maxHealth;
			ExpBounty = expBounty;
			Armor = armor;
			ArmorReduction = armorReduction;
			this.Steadfast = Steadfast;
			Affixes = affixes;
			creationTime = Time.time;
			if (!EnemyManager.clinetProgressions.ContainsKey(entity))
			{
				EnemyManager.clinetProgressions.Add(entity, this);
			}
		}
	}
}