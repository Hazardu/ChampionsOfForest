using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TheForest.Utils;

namespace ChampionsOfForest.Network.Commands
{

	public struct UpdateCProgressionCommandParam 
	{
		public ulong packed;
		public float health;
		public int armor, armor_reduction;
	}
	public class Command_UpdateDynamicCP : COTFCommand<UpdateCProgressionCommandParam>
	{
		public static void Initalize()
		{
			if (instance != null)
				return;
			else
			{
				instance = new Command_UpdateDynamicCP();
				instance.commandIndex = CommandReader.curr_cmd_index++;
				CommandReader.commandsObjects_dict.Add(instance.commandIndex, Received);
				ModAPI.Log.Write($"command {instance.commandIndex} registered");
			}
		}
		protected override void OnReceived(UpdateCProgressionCommandParam param, BinaryReader r)
		{
			if (GameSetup.IsMpClient)
			{
				var entity = BoltNetwork.FindEntity(new Bolt.NetworkId(param.packed));
				if (EnemyManager.clinetProgressions.ContainsKey(entity))
				{
					ClinetEnemyProgression cp = EnemyManager.clinetProgressions[entity];
					cp.UpdateDynamic(param.health, param.armor, param.armor_reduction);
				}
				else
				{
					if (EnemyManager.hostDictionary.TryGetValue(param.packed, out var enemy))
					{
						Send(NetworkManager.Target.Clients, new UpdateCProgressionCommandParam()
						{
							health = enemy.extraHealth + enemy.HealthScript.Health,
							armor = enemy.armor,
							armor_reduction = enemy.armorReduction,
							packed = param.packed
						});
					}
				}
			}
		}
	}
}
