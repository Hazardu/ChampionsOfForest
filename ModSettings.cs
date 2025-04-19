using System.Collections.Generic;
using System.IO;

using ChampionsOfForest.Network;
using ChampionsOfForest.Network.Commands;

using TheForest.Utils;

namespace ChampionsOfForest
{
	public class ModSettings
	{
		private static ModSettings instance;
		public enum GameDifficulty
		{
			Easy, Veteran, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5, Challenge6, Hell
		}
		public enum DropsOnDeathModes
		{
			All, Equipped, Disabled, NonEquipped
		}
		public enum LootLevelRules
		{
			HighestPlayerLevel, AverageLevel, LowestLevel, ClosestPlayer, HostLevel
		}

		GameDifficulty m_difficulty = GameDifficulty.Easy;
		public static GameDifficulty Difficulty => instance.m_difficulty;

		// non static properties
		// these are saved
		DropsOnDeathModes m_dropsOnDeathMode = DropsOnDeathModes.Disabled;
		LootLevelRules m_lootLevelRule = LootLevelRules.HighestPlayerLevel;
		bool m_friendlyFire = false;
		bool m_isDedicated = false;
		bool m_killOnDowned = false;
		bool m_allowRandomCaveSpawn = true;
		bool m_allowCaveRespawn = true;
		int m_caveMaxAdditionalEnemies = 2;
		float m_caveRespawnDelay = 120.0f;
		int m_minimumLevelForSocketsToAppear = 20;
		float m_chanceForFirstSocketToAppear = 0.25f;
		float m_chanceForSubsequentSocketsToAppear = 0.35f;

		float m_magicFindPerDifficultyLevel = 0.25f;
		bool m_privateLoot = false;
		float m_dropQuantityMultiplier = 1;
		float m_dropQualityMultiplier = 1;
		float m_dropChanceMultiplier = 1;
		float m_expMultiplier = 1;
		int m_enemyLevelIncreaseGlobal = 0;
		int m_enemyLevelIncreaseCaves = 0;
		float m_enemyDamageMultiplier = 1;
		float m_enemyHealthMultiplier = 1;
		float m_enemyArmorMultiplier = 1;
		float m_enemySpeedMultiplier = 1;
		bool m_allowElites = true;
		int m_lootFilterMinRarity = -1;
		bool m_combineHitMarkers = false;
		bool m_fullNumberHitMarkers = false;
		float m_keptExperienceAfterDeath = 0;
		bool m_endMassacreAfterDeath = true;

		// getters
		public static DropsOnDeathModes DropsOnDeath => instance.m_dropsOnDeathMode;
		public static LootLevelRules LootLevelRule => instance.m_lootLevelRule;
		public static bool FriendlyFire => instance.m_friendlyFire;
		public static bool IsDedicated => instance.m_isDedicated;
		public static bool KillOnDowned => instance.m_killOnDowned;
		public static bool AllowRandomCaveSpawn => instance.m_allowRandomCaveSpawn;
		public static bool AllowCaveRespawn => instance.m_allowCaveRespawn;
		public static int CaveMaxAdditionalEnemies => instance.m_caveMaxAdditionalEnemies;
		public static float CaveRespawnDelay => instance.m_caveRespawnDelay;
		public static int MinimumLevelForSocketsToAppear => instance.m_minimumLevelForSocketsToAppear;
		public static float ChanceForFirstSocketToAppear => instance.m_chanceForFirstSocketToAppear;
		public static float ChanceForSubsequentSocketsToAppear => instance.m_chanceForSubsequentSocketsToAppear;
		public static float MagicFindPerDifficultyLevel => instance.m_magicFindPerDifficultyLevel;
		public static bool PrivateLoot => instance.m_privateLoot;
		public static float DropQuantityMultiplier => instance.m_dropQuantityMultiplier;
		public static float DropQualityMultiplier => instance.m_dropQualityMultiplier;
		public static float DropChanceMultiplier => instance.m_dropChanceMultiplier;
		public static float ExpMultiplier => instance.m_expMultiplier;
		public static int EnemyLevelIncreaseGlobal => instance.m_enemyLevelIncreaseGlobal;
		public static int EnemyLevelIncreaseCaves => instance.m_enemyLevelIncreaseCaves;
		public static float EnemyDamageMultiplier => instance.m_enemyDamageMultiplier;
		public static float EnemyHealthMultiplier => instance.m_enemyHealthMultiplier;
		public static float EnemyArmorMultiplier => instance.m_enemyArmorMultiplier;
		public static float EnemySpeedMultiplier => instance.m_enemySpeedMultiplier;
		public static bool AllowElites => instance.m_allowElites;
		public static int LootFilterMinRarity => instance.m_lootFilterMinRarity;
		public static bool CombineHitMarkers => instance.m_combineHitMarkers;
		public static bool FullNumberHitMarkers => instance.m_fullNumberHitMarkers;
		public static float KeptExperienceAfterDeath => instance.m_keptExperienceAfterDeath;
		public static bool EndMassacreAfterDeath => instance.m_endMassacreAfterDeath;


		// static properties
		// these are not saved
		static bool DifficultyChosen = false;

		public static string Version;
		public const bool RequiresNewFiles = false;
		public const bool ALLNewFiles = false;
		public const bool RequiresNewSave = true;
		public const string RequiresNewSaveVersion = "1.6.0.2";

		public static readonly List<int> outdatedFiles = new List<int>(); // files to remove from disk


		public void Reset()
		{
			m_dropsOnDeathMode = DropsOnDeathModes.Disabled;
			m_lootLevelRule = LootLevelRules.HighestPlayerLevel;
			m_friendlyFire = false;
			m_isDedicated = false;
			m_killOnDowned = false;
			m_allowRandomCaveSpawn = true;
			m_allowCaveRespawn = true;
			m_caveMaxAdditionalEnemies = 2;
			m_caveRespawnDelay = 120.0f;
			m_minimumLevelForSocketsToAppear = 20;
			m_chanceForFirstSocketToAppear = 0.25f;
			m_chanceForSubsequentSocketsToAppear = 0.35f;
			m_magicFindPerDifficultyLevel = 0.25f;
			m_privateLoot = false;
			m_dropQuantityMultiplier = 1;
			m_dropQualityMultiplier = 1;
			m_dropChanceMultiplier = 1;
			m_expMultiplier = 1;
			m_enemyLevelIncreaseGlobal = 0;
			m_enemyLevelIncreaseCaves = 0;
			m_enemyDamageMultiplier = 1;
			m_enemyHealthMultiplier = 1;
			m_enemyArmorMultiplier = 1;
			m_enemySpeedMultiplier = 1;
			m_allowElites = true;
			m_lootFilterMinRarity = -1;
			m_combineHitMarkers = false;
			m_fullNumberHitMarkers = false;
			m_keptExperienceAfterDeath = 0;
			m_endMassacreAfterDeath = true;
		}


		public static void BroadCastSettingsToClients()
		{
			if (GameSetup.IsMpServer)
			{
				COTFCommand<BroadcastModSettings>.Send(NetworkManager.Target.Clients, new BroadcastModSettings()
				{
					dieOnDowned = instance.m_killOnDowned,
					dropsOnDeath = instance.m_dropsOnDeathMode,
					difficulty = instance.m_difficulty,
					friendlyFire = instance.m_friendlyFire,
					endMassacreAfterDeath = instance.m_endMassacreAfterDeath,
					expKeptAfterDeath = instance.m_keptExperienceAfterDeath
				});
			}
		}
		public static void ReceivedSettingsFromServer(BroadcastModSettings receivedSettings)
		{
			if (!GameSetup.IsMpClient || ModSettings.IsDedicated)
				return;
			instance.m_difficulty = receivedSettings.difficulty;
			instance.m_dropsOnDeathMode = receivedSettings.dropsOnDeath;
			instance.m_killOnDowned = receivedSettings.dieOnDowned;
			instance.m_friendlyFire = receivedSettings.friendlyFire;
			instance.m_endMassacreAfterDeath = receivedSettings.endMassacreAfterDeath;
			instance.m_keptExperienceAfterDeath = receivedSettings.expKeptAfterDeath;

			if (!ModSettings.DifficultyChosen)
			{
				LocalPlayer.FpCharacter.UnLockView();
				LocalPlayer.FpCharacter.MovementLocked = false;
				Cheats.GodMode = false;
				MainMenu.Instance.ClearDiffSelectionObjects();
			}
			ModSettings.DifficultyChosen = true;
		}

		const string PATH = "Mods/Champions of the Forest/Settings.save";
		public static void SaveSettings()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (BinaryWriter buf = new BinaryWriter(stream))
				{
					buf.Write(Utils.GetBytesFromObject(instance));
				}
				File.WriteAllBytes(PATH, stream.ToArray());
			}
		}
		public static void LoadSettings()
		{
			if (File.Exists(PATH))
				try
				{
					using (FileStream stream = new FileStream(PATH, FileMode.Open))
					{
						using (BinaryReader reader = new BinaryReader(stream))
						{
							instance = Utils.GetObjectFromBytes<ModSettings>(reader);
						}
					}
				}
				catch (System.Exception)
				{
					Utils.Log("Failed loading settings");
				}
		}
	}
}