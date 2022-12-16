using System.Collections.Generic;
using System.IO;

using ChampionsOfForest.Network;
using ChampionsOfForest.Network.CommandParams;

using TheForest.Utils;

namespace ChampionsOfForest
{
	public class ModSettings
	{
		public enum GlobalDifficulty
		{
			Easy, Veteran, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5, Challenge6, Hell
		}

		public enum PlayerDropsOnDeath
		{
			All, Equipped, Disabled, NonEquipped
		}
		public enum LootLevelPolicy
		{
			HighestPlayerLevel, AverageLevel, LowestLevel, ClosestPlayer, HostLevel
		}
		public static GlobalDifficulty difficulty = GlobalDifficulty.Easy;
		public static PlayerDropsOnDeath dropsOnDeath = PlayerDropsOnDeath.Disabled;
		public static bool DifficultyChosen = false;
		public static bool FriendlyFire = true;
		public static bool IsDedicated = false;
		public static bool KillOnDowned = false;
		public static bool FriendlyFireMagic = false;
		public static bool AllowRandomCaveSpawn = true;
		public static bool AllowCaveRespawn = true;
		public static int CaveMaxAdditionalEnemies = 1;	
		public static float CaveRespawnDelay = 1;
		public static string Version;
		public const bool RequiresNewFiles = false;
		public const bool ALLNewFiles = false;
		public const bool RequiresNewSave = true;
		public const string RequiresNewSaveVersion = "1.6.0.2";
		public static readonly List<int> outdatedFiles = new List<int>();

		public static float DropQuantityMultiplier = 1;
		public static float DropChanceMultiplier = 1;
		public static float ExpMultiplier = 1;
		public static int EnemyLevelIncrease = 0;
		public static float EnemyDamageMultiplier = 1;
		public static float EnemyHealthMultiplier = 1;
		public static float EnemyArmorMultiplier = 1;
		public static float EnemySpeedMultiplier = 1;
		public static float FriendlyFireDamage = 1;
		public static bool AllowElites = true;
		public static LootLevelPolicy lootLevelPolicy = LootLevelPolicy.HighestPlayerLevel;

		public static void Default()
		{
			DropQuantityMultiplier = 1;
			DropChanceMultiplier = 1;
			ExpMultiplier = 1;
			EnemyLevelIncrease = 0;
			EnemyDamageMultiplier = 1;
			EnemyHealthMultiplier = 1;
			EnemyArmorMultiplier = 1;
			EnemySpeedMultiplier = 1;
			FriendlyFireDamage = 1;
			AllowElites = true;
			lootLevelPolicy = LootLevelPolicy.HighestPlayerLevel;
			AllowRandomCaveSpawn = true;
			AllowCaveRespawn = true;
			CaveMaxAdditionalEnemies = 1;
			CaveRespawnDelay = 1;

		}


		public static void BroadCastSettingsToClients()
		{
			if (GameSetup.IsMpServer)
			{
				var cmd = new CommandStream(Commands.CommandType.DIFFICULTY_INFO_ANSWER);
				cmd.Write(
					   new params_DIFFICULTY_INFO_ANSWER(
					(int)difficulty,
					(int)dropsOnDeath,
					ExpMultiplier,
					EnemyDamageMultiplier,
					FriendlyFireDamage,
					FriendlyFire,
					KillOnDowned,
					FriendlyFireMagic
					));
				cmd.Send(NetworkManager.Target.Clients);
			}
		}

		const string PATH = "Mods/Champions of the Forest/Settings.save";
		public static void SaveSettings()
		{
		
			using (FileStream stream = new FileStream(PATH, FileMode.OpenOrCreate))
			{
				using (BinaryWriter buf = new BinaryWriter(stream))
				{
					buf.Write(FriendlyFire);
					buf.Write(KillOnDowned);
					buf.Write(DropQuantityMultiplier);
					buf.Write(DropChanceMultiplier);
					buf.Write(ExpMultiplier);
					buf.Write(EnemyLevelIncrease);
					buf.Write(EnemyDamageMultiplier);
					buf.Write(EnemyHealthMultiplier);
					buf.Write(EnemyArmorMultiplier);
					buf.Write(EnemySpeedMultiplier);
					buf.Write(AllowElites);
					buf.Write((int)dropsOnDeath);
					buf.Write((int)lootLevelPolicy);
					buf.Write(AllowRandomCaveSpawn);
					buf.Write(AllowCaveRespawn);
					buf.Write(CaveMaxAdditionalEnemies);
					buf.Write(CaveRespawnDelay);
				}
			}
		}
		public static void LoadSettings()
		{
			if (File.Exists(PATH))
				try
				{
					using (FileStream stream = new FileStream(PATH, FileMode.Open))
					{
						using (BinaryReader buf = new BinaryReader(stream))
						{
							FriendlyFire = buf.ReadBoolean();
							KillOnDowned = buf.ReadBoolean();
							DropQuantityMultiplier = buf.ReadSingle();
							DropChanceMultiplier = buf.ReadSingle();
							ExpMultiplier = buf.ReadSingle();
							EnemyLevelIncrease = buf.ReadInt32();
							EnemyDamageMultiplier = buf.ReadSingle();
							EnemyHealthMultiplier = buf.ReadSingle();
							EnemyArmorMultiplier = buf.ReadSingle();
							EnemySpeedMultiplier = buf.ReadSingle();
							AllowElites = buf.ReadBoolean();
							dropsOnDeath = (PlayerDropsOnDeath)buf.ReadInt32();
							lootLevelPolicy = (LootLevelPolicy)buf.ReadInt32();
							AllowRandomCaveSpawn = buf.ReadBoolean();
							AllowCaveRespawn = buf.ReadBoolean();
							CaveMaxAdditionalEnemies = buf.ReadInt32();
							CaveRespawnDelay = buf.ReadSingle();
						}
					}
				}
				catch (System.Exception)
				{

					CotfUtils.Log("Failed loading settings");
				}
			
		}
	}
}