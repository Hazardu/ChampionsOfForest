//Stores info about enemy stats, shared between players in coop

using System.IO;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Enemies
{
	public class ClientEnemyProgression
	{
		public struct DynamicClientEnemyProgression
		{
			public float Health, Damage;
			public int Armor;
			public int ArmorReduction;

			public DynamicClientEnemyProgression(float health, int armor, int armorReduction, float damage)
			{
				Health = health;
				Armor = armor;
				ArmorReduction = armorReduction;
				Damage = damage;
			}
		}
		DynamicClientEnemyProgression dynCEP;
		public const float LifeTime = 50;
		public const float DynamicLifeTime = 0.75f;
		public BoltEntity Entity;
		public ulong Packed;
		public string EnemyName;
		public int Level;
		public float MaxHealth;
		public long ExpBounty;
		public float Steadfast;
		public int[] Affixes;
		public float creationTime;
		public float dynamicCreationTime;
		public float Health => dynCEP.Health;
		public float Armor => dynCEP.Armor;
		public float ArmorReduction => dynCEP.ArmorReduction;
		public float Damage => dynCEP.Damage;


		/// <summary>
		/// host/singleplayer constructor
		/// </summary>
		/// <param name="tr"></param>
		public ClientEnemyProgression(Transform tr)
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
				dynCEP = new DynamicClientEnemyProgression((float)p.HP, p.armor, p.armorReduction, p.DamageTotal);
				Level = p.level;
				MaxHealth = (float)p.maxHealth;
				ExpBounty = p.bounty;
				Steadfast = p.Steadfast;
				Affixes = new int[p.abilities.Count];
				for (int i = 0; i < p.abilities.Count; i++)
				{
					Affixes[i] = (int)p.abilities[i];
				}
			}
		}
		public void RequestDynamicUpdate()
		{
			using (MemoryStream answerStream = new MemoryStream())
			{
				using (BinaryWriter w = new BinaryWriter(answerStream))
				{
					w.Write(44);
					w.Write(Entity.networkId.PackedValue);
					w.Close();
				}
				Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
				answerStream.Close();
			}
			dynamicCreationTime = Time.time;
			Debug.Log("CP Request sent");
		}
		public bool DynamicOutdated => dynamicCreationTime + DynamicLifeTime < Time.time;
		public void UpdateDynamic(float hp, int ar, int arred, float damage)
		{
			Debug.Log("Updating dcp");
			dynCEP = new DynamicClientEnemyProgression(hp, ar, arred, damage);
			dynamicCreationTime = Time.time;

		}
		public ClientEnemyProgression(BoltEntity entity)
		{
			if (!EnemyManager.clinetProgressions.ContainsKey(entity))
			{
				EnemyManager.clinetProgressions.Add(entity, this);
			}
		}

		//if (GameSetup.IsMpClient)
		//{
		//	using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
		//	{
		//		using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
		//		{
		//			w.Write(6);
		//			w.Write(Packed);
		//			w.Close();
		//		}
		//		ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
		//		answerStream.Close();
		//	}
		//}

		//}
		//public static ClientEnemyProgression Update(BoltEntity entity, float hp, int ar, int arred)
		//{
		//	if (EnemyManager.clinetProgressions.ContainsKey(entity))
		//	{
		//		var cp = EnemyManager.clinetProgressions[entity];
		//		cp.dynCEP = new DynamicClientEnemyProgression(hp,ar,arred)
		//		return cp;
		//	}
		//	return null;
		//}
		public void Update(BoltEntity entity, string enemyName, int level, float health, float damage, float maxHealth, long expBounty, int armor, int armorReduction, float Steadfast, int[] affixes)
		{
			Entity = entity;
			EnemyName = enemyName;
			if (entity != null)
				Packed = entity.networkId.PackedValue;
			Level = level;
			dynCEP = new DynamicClientEnemyProgression(health, armor, armorReduction, damage);
			MaxHealth = maxHealth;
			ExpBounty = expBounty;
			this.Steadfast = Steadfast;
			Affixes = affixes;
			creationTime = Time.time;
			dynamicCreationTime = Time.time;

		}
	}
}