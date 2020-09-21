using System.Collections.Generic;

using ChampionsOfForest.Enemies;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public static class EnemyManager
	{
		public static Dictionary<ulong, EnemyProgression> hostDictionary;
		public static Dictionary<Transform, EnemyProgression> enemyByTransform;
		public static Dictionary<ulong, BoltEntity> allboltEntities;
		public static Dictionary<ulong, ClientEnemy> clientEnemies;
		public static Dictionary<BoltEntity, ClinetEnemyProgression> clinetProgressions;
		public static Dictionary<Transform, ClinetEnemyProgression> spProgression;
		private static float LastAskedTime = 0;
		private static readonly float AskFrequency = 0.5f;

		public static void Initialize()
		{
			if (BoltNetwork.isRunning)
			{
				if (GameSetup.IsMpServer)
				{
					hostDictionary = new Dictionary<ulong, EnemyProgression>();
					enemyByTransform = new Dictionary<Transform, EnemyProgression>();
				}
				allboltEntities = new Dictionary<ulong, BoltEntity>();
				clinetProgressions = new Dictionary<BoltEntity, ClinetEnemyProgression>();
				clientEnemies = new Dictionary<ulong, ClientEnemy>();
				GetAllEntities();
			}
			else
			{
				enemyByTransform = new Dictionary<Transform, EnemyProgression>();

				spProgression = new Dictionary<Transform, ClinetEnemyProgression>();
			}
		}

		public static void AddHostEnemy(EnemyProgression ep)
		{
			if (!hostDictionary.ContainsKey(ep.entity.networkId.PackedValue))
			{
				hostDictionary.Add(ep.entity.networkId.PackedValue, ep);
			}
			else
			{
				hostDictionary[ep.entity.networkId.PackedValue] = ep;
			}
		}

		//Gets all attached bolt entities
		public static void GetAllEntities()
		{
			allboltEntities.Clear();
			BoltEntity[] entities = GameObject.FindObjectsOfType<BoltEntity>();

			foreach (BoltEntity entity in entities)
			{
				try
				{
					if (entity.isAttached)
					{
						allboltEntities.Add(entity.networkId.PackedValue, entity);
					}
				}
				catch (System.Exception ex)
				{
					ModAPI.Log.Write(ex.ToString());
				}
			}
		}

		//Returns clinet progression for Singleplayer
		public static ClinetEnemyProgression GetCP(Transform tr)
		{
			if (spProgression.ContainsKey(tr.root))
			{
				ClinetEnemyProgression cp = spProgression[tr.root];
				if (Time.time <= cp.creationTime + ClinetEnemyProgression.LifeTime)
				{
					return cp;
				}
				else
				{
					spProgression.Remove(tr.root);
				}
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
					ClinetEnemyProgression cpr = new ClinetEnemyProgression(tr.root);
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
		public static ClinetEnemyProgression GetCP(BoltEntity e)
		{
			if (e == null)
			{
				return null;
			}
			if (clinetProgressions.ContainsKey(e))
			{
				ClinetEnemyProgression cp = clinetProgressions[e];
				if (Time.time <= cp.creationTime + ClinetEnemyProgression.LifeTime)
				{
					return cp;
				}
				else
				{
					clinetProgressions.Remove(e);
					return null;
				}
			}
			if (!GameSetup.IsMpClient)
			{
				return new ClinetEnemyProgression(e);
			}
			if (Time.time > LastAskedTime + AskFrequency)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(6);
						w.Write(e.networkId.PackedValue);
						w.Close();
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
				LastAskedTime = Time.time;
			}
			return null;
		}

		public static void RemoveEnemy(EnemyProgression ep)
		{
			try
			{
				if (ep.entity != null)
				{
					if (ep.entity.networkId != null)
					{
						if (hostDictionary.ContainsKey(ep.entity.networkId.PackedValue))
						{
							hostDictionary.Remove(ep.entity.networkId.PackedValue);
						}
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