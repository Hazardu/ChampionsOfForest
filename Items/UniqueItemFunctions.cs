using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChampionsOfForest.Player;

namespace ChampionsOfForest.Items
{
	public static class UniqueItemFunctions
	{
		public static void EnemyBleedForPlayerHP(Player.COTFEvents.HitOtherParams param)
		{
			if (param is null)
				return;
			if (param.hitTarget is BoltEntity)
			{
				var entity = param.hitTarget as BoltEntity;
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(32);
						w.Write(entity.networkId.PackedValue);
						w.Write(ModdedPlayer.Stats.TotalMaxHealth);
						w.Write(10f);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
					answerStream.Close();
				}
			}
			else if (param.hitTarget is EnemyProgression)
			{
				var prog = param.hitTarget as EnemyProgression;
				prog.DoDoT(ModdedPlayer.Stats.maxHealth, 10f);
			}
		}
	}
}
