using System;

using ChampionsOfForest.Enemies;
using ChampionsOfForest.Player;
using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Network.NetworkManager;

namespace ChampionsOfForest.Network
{
	internal class NetworkUtils
	{

		public const int SILENTattackerType = 2000000;
		public const int SILENTattackerTypeMagic = 2000001;
		public const int CONVERTEDFLOATattackerType = 1000000;
		public const int PURE = 3000001;

		public static void ReduceDamageToSendOverNet(float damage, out int outdamage, out int repetitions)
		{
			if (damage < int.MaxValue / 5)
			{
				outdamage = Mathf.FloorToInt(damage);
				repetitions = 1;
				return;
			}
			repetitions = Mathf.FloorToInt(damage / (int.MaxValue / 5f)) + 1;
			outdamage = Mathf.RoundToInt(damage / repetitions);
		}

		//cast a float to int without altering bits.
		public static int FloatToInt(float damage)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(damage), 0);

		}

		private static ulong lastDropID = 10;

		/// <summary>
		/// Sends a command to create a item drop for all players.
		/// </summary>
		/// <param name="item">A reference to the item object. Things like level and stats will be read off it</param>
		/// <param name="pos">Where to spawn the item at</param>
		/// <param name="amount">How many of this item should be spawned</param>
		public static void SendItemDrop(Item item, Vector3 pos, ItemPickUp.DropSource dropSource, int amount = 1)
		{
			if (item == null)
				return;
			ulong id = lastDropID + 1;
			while (PickUpManager.PickUps.ContainsKey(id))
			{
				id++;
			}
			lastDropID = id;
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(5);
					w.Write(item.ID);
					w.Write(id);
					w.Write(item.level);
					w.Write(amount);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write((int)dropSource);
					foreach (ItemStat stat in item.Stats)
					{
						w.Write(stat.id);
						w.Write(stat.possibleStatsIndex);
						w.Write(stat.amount);
					}
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}

		public static void SendItemToPlayer(Item item, string playerID, int amount = 1)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(26);
					w.Write(playerID);
					w.Write(item.ID);
					w.Write(amount);
					w.Write(item.level);
					foreach (ItemStat stat in item.Stats)
					{
						w.Write(stat.id);
						w.Write(stat.possibleStatsIndex);
						w.Write(stat.amount);
					}
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}

		public static void SendHitmarker(Vector3 pos, float amount, Color c)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(20);
					w.Write(amount);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(c.r);
					w.Write(c.g);
					w.Write(c.b);
					w.Write(c.a);
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}

		public static void SendPlayerHitmarker(Vector3 pos, int amount)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(21);
					w.Write(amount);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}

		/// <summary>
		/// Sends a buff commands to players close to a location
		/// </summary>
		/// <param name="pos">position, where the buff was casted</param>
		/// <param name="range">range of the buff effect</param>
		public static void SendBuff(Vector3 pos, float range, int buffId, int sourceId, float amount, float duration)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(41);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(range);
					w.Write(buffId);
					w.Write(sourceId);
					w.Write(amount);
					w.Write(duration);
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}

		/// <summary>
		/// Send buff to a player with a name
		/// </summary>
		/// <param name="id">Name identifier of the other player</param>
		public static void SendBuff(string id, int buffId, int sourceId, float amount, float duration)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(40);

					w.Write(id);
					w.Write(buffId);
					w.Write(sourceId);
					w.Write(amount);
					w.Write(duration);
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}

		/// <summary>
		/// Send buff to everyone, globally
		/// </summary>
		public static void SendBuff(int buffId, int sourceId, float amount, float duration)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(42);
					w.Write(buffId);
					w.Write(sourceId);
					w.Write(amount);
					w.Write(duration);
					w.Close();
				}
				SendLine(answerStream.ToArray(), Target.Everyone);
				answerStream.Close();
			}
		}



		public static void SendRandomItemDrops(int count, EnemyProgression.Enemy type, long bounty, ModSettings.GlobalDifficulty difficulty, Vector3 position)
		{
			RCoroutines.Instance.StartCoroutine(RCoroutines.Instance.AsyncSendRandomItemDrops(count, type, bounty, difficulty, position));
		}

		float lastLevelCheckTimestamp;
		private int lastPlayerLevelCount;
		public static void UpdatePlayerInfo(ulong playerID, int level, long exp, float hp, float maxhp, float energy, float maxenergy)
		{
			var player = NetworkManager.GetModdedClient(playerID);
			for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
			{
				var packed = Scene.SceneTracker.allPlayerEntities[i].networkId.PackedValue;
				if (packed == playerID)
				{
					Scene.SceneTracker.allPlayerEntities[i].GetComponentInChildren<TheForest.UI.Multiplayer.PlayerName>().SendMessage("SetNameWithLevel", level);
					break;
				}
			}
		}

	}
	public static void RemoveUnusedPlayerLevels()
	{
		List<string> stringsToRemove = new List<string>();
		foreach (var item in PlayerLevels)
		{
			if (!PlayerStates.Any(x => x.name == item.Key))
			{
				stringsToRemove.Add(item.Key);
			}
		}
		foreach (var item in stringsToRemove)
		{
			Debug.Log("Removing player: " + item);
			PlayerLevels.Remove(item);
		}
	}

	public static void RequestAllPlayerLevels()
	{
		if (Time.time - instance.lastLevelCheckTimestamp > 120 || instance.lastPlayerLevelCount != Players.Count)
		{
			RemoveUnusedPlayerLevels();
			instance.LevelRequestCooldown = 120;
			instance.lastLevelCheckTimestamp = Time.time;
			instance.lastPlayerLevelCount = Players.Count;
			Host_RequestLevels();

		}
	}
	private void UpdateLevelData()
	{
		if (Players.Count > 1)
		{
			LevelRequestCooldown -= 1;
			if (LevelRequestCooldown < 0 || lastPlayerLevelCount != Players.Count)
			{
				LevelRequestCooldown = 120;
				lastLevelCheckTimestamp = Time.time;
				lastPlayerLevelCount = Players.Count;
				RemoveUnusedPlayerLevels();
				Host_RequestLevels();
			}

			MFindRequestCooldown--;
			if (MFindRequestCooldown <= 0)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(23);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
					answerStream.Close();
				}
				MFindRequestCooldown = 300;
			}

			if (PlayerHands.ContainsValue(null))
			{
				PlayerHands.Clear();
			}
		}
		else
		{
			//PlayerLevels.Clear();
		}
	}

	public static void Host_RequestLevels()
	{
		if (GameSetup.IsMpServer)
		{
			using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
			{
				using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
				{
					w.Write(18);
					w.Close();
				}
				Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
				answerStream.Close();
			}
		}
	}


	//invalid il code error happens here. i have no clue why, so im randomly changing it so maybe it fixes itself
	//im trying splitting to more classes
	public static void FindHands()
	{
		PlayerHands.Clear();
		for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
		{
			if (Scene.SceneTracker.allPlayers[i].transform.root != LocalPlayer.Transform)
			{
				//BoltEntity entity = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
				//if (entity == null) continue;
				//IPlayerState state = entity.GetState<IPlayerState>();
				//if (state == null) continue;

				string playerName = getName(Scene.SceneTracker.allPlayers[i]);
				if (playerName != "")
				{
					Transform hand = FindDeepChild(Scene.SceneTracker.allPlayers[i].transform.root, "rightHandHeld");
					if (hand != null)
					{
						PlayerHands.Add(playerName, hand.parent);
					}
				}
			}
		}
	}

	private static string getName(GameObject gameObject)
	{
		BoltEntity entity = gameObject.GetComponent<BoltEntity>();
		if (entity == null)
			return "";
		else
		{
			IPlayerState state = entity.GetState<IPlayerState>();
			if (state == null)
				return "";
			else
				return state.name;
		}
	}

	private IEnumerator UpdateSetups()
	{
		while (true)
		{
			yield return null;
			if (Players.Any(x => x == null))
			{
				Players.Clear();
				AllPlayerEntities.Clear();
				PlayerStates.Clear();
				Players.AddRange(Scene.SceneTracker.allPlayers);
				for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
				{
					BoltEntity b = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
					if (b != null)
					{
						AllPlayerEntities.Add(b);
						PlayerStates.Add(b.GetState<IPlayerState>());
					}
				}
			}

			yield return new WaitForSeconds(10);
		}
	}
}
}
}
