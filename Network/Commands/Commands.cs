using System.IO;
using TheForest.Utils;

namespace ChampionsOfForest.Network.Commands
{

	public struct UpdateCProgressionCommandParam
	{
		public ulong packed;
		public float health, damage;
		public int armor, armor_reduction;
	}
	public class Command_UpdateDynamicCP : COTFCommand<UpdateCProgressionCommandParam>
	{
		public static void Initialize() => Init(typeof(Command_UpdateDynamicCP));
		
		protected override void OnReceived(UpdateCProgressionCommandParam param, BinaryReader r)
		{
			if (GameSetup.IsMpClient)
			{
				var entity = BoltNetwork.FindEntity(new Bolt.NetworkId(param.packed));
				if (EnemyManager.clinetProgressions.ContainsKey(entity))
				{
					ClientEnemyProgression cp = EnemyManager.clinetProgressions[entity];
					cp.UpdateDynamic(param.health, param.armor, param.armor_reduction, param.damage);
				}
			}
			else
			{
				if (EnemyManager.hostDictionary.TryGetValue(param.packed, out var enemy))
				{
					Send(NetworkManager.Target.Clients, new UpdateCProgressionCommandParam()
					{
						health = enemy.HP,
						damage = enemy.DamageTotal,
						armor = enemy.armor,
						armor_reduction = enemy.armorReduction,
						packed = param.packed
					});
				}
			}
		}
	}
}
