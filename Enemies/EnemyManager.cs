using System.Collections.Generic;
using System.IO;
using System.Linq;

using ChampionsOfForest.Enemies;
using ChampionsOfForest.Network;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public static class EnemyManager
	{
		public static Dictionary<ulong, EnemyProgression> hostDictionary;
		public static Dictionary<Transform, EnemyProgression> enemyByTransform;
		public static Dictionary<ulong, ClientEnemy> clientEnemies;
		public static Dictionary<BoltEntity, ClientEnemyProgression> clinetProgressions;
		public static Dictionary<Transform, ClientEnemyProgression> spProgression;
		private static float scanEnemyLastRequestTimestamp = 0;
		private static readonly float scanEnemyFrequency = 0.1f;

		public static void Initialize()
		{
			if (GameSetup.IsMpClient)
			{
				clinetProgressions = new Dictionary<BoltEntity, ClientEnemyProgression>();
				clientEnemies = new Dictionary<ulong, ClientEnemy>();
			}
			else
			{
				hostDictionary = new Dictionary<ulong, EnemyProgression>();
				enemyByTransform = new Dictionary<Transform, EnemyProgression>();
				spProgression = new Dictionary<Transform, ClientEnemyProgression>();
			}
		}

		public static void AddHostEnemy(EnemyProgression ep)
		{
			if (!hostDictionary.ContainsKey(ep.entity.networkId.PackedValue))
				hostDictionary.Add(ep.entity.networkId.PackedValue, ep);
			else
				hostDictionary[ep.entity.networkId.PackedValue] = ep;
		}

		//Returns clinet progression for Singleplayer
		public static ClientEnemyProgression GetCP(Transform tr)
		{
			if (spProgression.ContainsKey(tr.root))
			{
				ClientEnemyProgression cp = spProgression[tr.root];
				if (cp.Health <= 0)
				{
					spProgression.Remove(tr.root);
					return null;
				}
				if (Time.time <= cp.creationTime + ClientEnemyProgression.LifeTime)
				{
					if (cp.DynamicOutdated)
					{
						Debug.Log("Outdated dynamic CP");
						var e = tr.GetComponentInParent<EnemyProgression>();
						cp.UpdateDynamic((float)e.HP, e.armor,
						e.armorReduction, e.DamageTotal);
						if (GameSetup.IsMultiplayer)
						{
						using (MemoryStream answerStream = new MemoryStream())
						{
							using (BinaryWriter w = new BinaryWriter(answerStream))
							{
								w.Write(44);
								w.Write(e.entity.networkId.PackedValue);
								w.Write((float)e.HP);
								w.Write(e.armor);
								w.Write(e.armorReduction);
								w.Write(e.DamageAmp);
								w.Close();
							}
							NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Clients);
							answerStream.Close();
						}
						}
					}
				}
				else
				{
					Debug.Log("Outdated static CP");
					var e = tr.GetComponentInParent<EnemyProgression>();
					cp.Update(null, e.enemyName, e.level, (float)e.HP, e.DamageTotal, (float)e.maxHealth, e.bounty, e.armor, e.armorReduction, e.Steadfast, e.abilities.Count > 0 ? e.abilities.Select(x => (int)x).ToArray() : new int[0]);
				}
				return cp;
			}
			else
			{
				EnemyProgression p = tr.root.GetComponent<EnemyProgression>();
				if (p == null)
				{
					p = tr.root.GetComponentInChildren<EnemyProgression>();
				}

				if (p != null)
				{
					ClientEnemyProgression cpr = new ClientEnemyProgression(tr.root);
					spProgression.Add(tr.root, cpr);
					return cpr;
				}
				else
				{
					{
						mutantScriptSetup setup = tr.root.GetComponentInChildren<mutantScriptSetup>();
						if (setup == null)
						{
							setup = tr.root.GetComponent<mutantScriptSetup>();
						}
						if (setup != null)
						{
							p = setup.health.gameObject.AddComponent<EnemyProgression>();
							if (p != null)
							{
								p.HealthScript = setup.health;
								p.AIScript = setup.ai;
								p.entity = setup.GetComponent<BoltEntity>();
								p.setup = setup;
							}
						}
					}
				}
			}
			return null;
		}

		//Returns clinet progression for Multiplayer
		public static ClientEnemyProgression GetCP(BoltEntity e)
		{
			ClientEnemyProgression cp = null;
			if (!GameSetup.IsMpClient)
			{
				return GetCP(e.transform);
			}
			if (e == null)
			{
				return null;
			}
			if (clinetProgressions.ContainsKey(e))
			{
				cp = clinetProgressions[e];
				if (Time.time <= cp.creationTime + ClientEnemyProgression.LifeTime + 0.1f)
				{
					if (cp.DynamicOutdated)
						cp.RequestDynamicUpdate();
				}
				return cp;
			}
			if (Time.time > scanEnemyLastRequestTimestamp + scanEnemyFrequency)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(6);
						w.Write(e.networkId.PackedValue);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
				scanEnemyLastRequestTimestamp = Time.time;
			}

			return cp;
		}

		public static void RemoveEnemy(EnemyProgression ep)
		{
			try
			{
				if (ep.entity != null)
				{
					if (hostDictionary.ContainsKey(ep.entity.networkId.PackedValue))
					{
						hostDictionary.Remove(ep.entity.networkId.PackedValue);
					}
				}
				if (spProgression != null)
				{
					if (spProgression.ContainsKey(ep.transform.root))
					{
						spProgression.Remove(ep.transform.root);
					}
				}
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}
	}
}