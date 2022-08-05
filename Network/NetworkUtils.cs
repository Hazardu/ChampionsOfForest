﻿using System;

using UnityEngine;

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
	}
}
}
