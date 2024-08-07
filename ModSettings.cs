﻿using System.Collections.Generic;
using System.IO;

using TheForest.Utils;

namespace ChampionsOfForest
{
	public class ModSettings
	{
		public enum Difficulty
		{
			Easy, Veteran, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5, Challenge6, Hell
		}

		public enum DropsOnDeathMode
		{
			All, Equipped, Disabled, NonEquipped
		}
		public enum LootLevelPolicy
		{
			HighestPlayerLevel, AverageLevel, LowestLevel, ClosestPlayer, HostLevel
		}
		public static Difficulty difficulty = Difficulty.Easy;
		public static DropsOnDeathMode dropsOnDeath = DropsOnDeathMode.Disabled;
		public static bool DifficultyChosen = false;
		public static bool FriendlyFire = true;
		public static bool IsDedicated = false;
		public static bool killOnDowned = false;
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
		public static bool AllowElites = true;
		public static LootLevelPolicy lootLevelPolicy = LootLevelPolicy.HighestPlayerLevel;
		public static int LootFilterMinRarity = 0;
		public static void Reset()
		{
			DropQuantityMultiplier = 1;
			DropChanceMultiplier = 1;
			ExpMultiplier = 1;
			EnemyLevelIncrease = 0;
			EnemyDamageMultiplier = 1;
			EnemyHealthMultiplier = 1;
			EnemyArmorMultiplier = 1;
			EnemySpeedMultiplier = 1;
			AllowElites = true;
			lootLevelPolicy = LootLevelPolicy.HighestPlayerLevel;
			AllowRandomCaveSpawn = true;
			AllowCaveRespawn = true;
			CaveMaxAdditionalEnemies = 1;
			CaveRespawnDelay = 1;
			LootFilterMinRarity = 0;
		}


		public static void BroadCastSettingsToClients()
		{
			if (GameSetup.IsMpServer)
			{
				using (MemoryStream answerStream = new MemoryStream())
				{
					using (BinaryWriter w = new BinaryWriter(answerStream))
					{
						w.Write(2);
						w.Write((int)ModSettings.difficulty);
						w.Write(ModSettings.FriendlyFire);
						w.Write((int)ModSettings.dropsOnDeath);
						w.Write(ModSettings.killOnDowned);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Clients);
					answerStream.Close();
				}
			}
		}

		const string PATH = "Mods/Champions of the Forest/Settings.save";
		public static void SaveSettings()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (BinaryWriter buf = new BinaryWriter(stream))
				{
					buf.Write(FriendlyFire);
					buf.Write(killOnDowned);
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
					buf.Write(LootFilterMinRarity);
					File.WriteAllBytes(PATH, stream.ToArray());
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
							killOnDowned = buf.ReadBoolean();
							DropQuantityMultiplier = buf.ReadSingle();
							DropChanceMultiplier = buf.ReadSingle();
							ExpMultiplier = buf.ReadSingle();
							EnemyLevelIncrease = buf.ReadInt32();
							EnemyDamageMultiplier = buf.ReadSingle();
							EnemyHealthMultiplier = buf.ReadSingle();
							EnemyArmorMultiplier = buf.ReadSingle();
							EnemySpeedMultiplier = buf.ReadSingle();
							AllowElites = buf.ReadBoolean();
							dropsOnDeath = (DropsOnDeathMode)buf.ReadInt32();
							lootLevelPolicy = (LootLevelPolicy)buf.ReadInt32();
							AllowRandomCaveSpawn = buf.ReadBoolean();
							AllowCaveRespawn = buf.ReadBoolean();
							CaveMaxAdditionalEnemies = buf.ReadInt32();
							CaveRespawnDelay = buf.ReadSingle();
							LootFilterMinRarity = buf.ReadInt32();
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