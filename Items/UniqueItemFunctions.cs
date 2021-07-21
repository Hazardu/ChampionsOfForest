using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Items
{
	public static class UniqueItemFunctions
	{
		private static float ResistDeathTimeStamp;
		public static void ResistDeath()
		{
			if (ResistDeathTimeStamp < Time.time)
			{
				if (ResistDeathTimeStamp + 60 <= Time.time)
				{
					ResistDeathTimeStamp = Time.time;
					LocalPlayer.Stats.Health = ModdedPlayer.Stats.TotalMaxHealth;
					BuffDB.AddBuff(6, 82, 1, 5);
					BuffDB.AddBuff(26, 108, 0.02f, 5);   //98% damage reduction
					BuffDB.AddBuff(25, 99, 35, 5);     //+35 hp/s
				}

			}
			else
			{
				ResistDeathTimeStamp = 0;
			}
		}
		public static void EnemyBleedForPlayerHP(COTFEvents.HitOtherParams param)
		{
			if (param is null)
			{
				UnityEngine.Debug.Log("Params are null in EnemyBleedForPlayerHP(...)");
				return;
			}
			if (GameSetup.IsMpClient)
			{
				var entity = (BoltEntity)param.hitTarget;
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(32);
						w.Write(entity.networkId.PackedValue);
						w.Write(ModdedPlayer.Stats.TotalMaxHealth);
						w.Write(15f);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
			}
			else
			{
				EnemyProgression prog = param.hitTarget as EnemyProgression;
				prog.DoDoT(ModdedPlayer.Stats.maxHealth, 15f);

			}
		}

		public static void Gain1EnergyOnHit(COTFEvents.HitOtherParams param)
		{
			float stamGain = param.isCrit ? 2 : 1;
			stamGain *= ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier;
			LocalPlayer.Stats.Stamina += stamGain;
		}
		public static void Gain10EnergyOnHit(COTFEvents.HitOtherParams param)
		{
			float stamGain = param.isCrit ? 20 : 10;
			stamGain *= ModdedPlayer.Stats.TotalStaminaRecoveryMultiplier;
			LocalPlayer.Stats.Stamina += stamGain;
		}
	}
}
