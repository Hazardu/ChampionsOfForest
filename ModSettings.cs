using System.Collections.Generic;
using System.IO;

using ChampionsOfForest.Network;
using ChampionsOfForest.Network.Commands;

using TheForest.Utils;

namespace ChampionsOfForest
{
	public class ModSettings
	{
		internal static ModSettings instance;
		public enum GameDifficulty
		{
			Easy = 0, Veteran, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5, Challenge6, Hell
		}
		public enum DropsOnDeathModes
		{
			All = 0, Equipped, Disabled, Inventory, Max
		}
		public enum LootLevelRules
		{
			HighestPlayerLevel = 0 , AverageLevel, LowestLevel, ClosestPlayer, HostLevel, Max
		}

		GameDifficulty m_difficulty = GameDifficulty.Easy;

		// non static properties
		// these are saved
		DropsOnDeathModes m_dropsOnDeathMode = DropsOnDeathModes.Disabled;
		LootLevelRules m_lootLevelRule = LootLevelRules.HighestPlayerLevel;
		bool m_friendlyFire = false;
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

		// getters and setters
		public static GameDifficulty Difficulty
		{
			get => instance.m_difficulty;
			set => instance.m_difficulty = value;
		}
		public static DropsOnDeathModes DropsOnDeath
		{
			get => instance.m_dropsOnDeathMode;
			set => instance.m_dropsOnDeathMode = value;
		}
		public static LootLevelRules LootLevelRule
		{
			get => instance.m_lootLevelRule;
			set => instance.m_lootLevelRule = value;
		}
		public static bool FriendlyFire
		{
			get => instance.m_friendlyFire;
			set => instance.m_friendlyFire = value;
		}
		public static bool KillOnDowned
		{
			get => instance.m_killOnDowned;
			set => instance.m_killOnDowned = value;
		}
		public static bool AllowRandomCaveSpawn
		{
			get => instance.m_allowRandomCaveSpawn;
			set => instance.m_allowRandomCaveSpawn = value;
		}
		public static bool AllowCaveRespawn
		{
			get => instance.m_allowCaveRespawn;
			set => instance.m_allowCaveRespawn = value;
		}
		public static int CaveMaxAdditionalEnemies
		{
			get => instance.m_caveMaxAdditionalEnemies;
			set => instance.m_caveMaxAdditionalEnemies = value;
		}
		public static float CaveRespawnDelay
		{
			get => instance.m_caveRespawnDelay;
			set => instance.m_caveRespawnDelay = value;
		}
		public static int MinimumLevelForSocketsToAppear
		{
			get => instance.m_minimumLevelForSocketsToAppear;
			set => instance.m_minimumLevelForSocketsToAppear = value;
		}
		public static float ChanceForFirstSocketToAppear
		{
			get => instance.m_chanceForFirstSocketToAppear;
			set => instance.m_chanceForFirstSocketToAppear = value;
		}
		public static float ChanceForSubsequentSocketsToAppear
		{
			get => instance.m_chanceForSubsequentSocketsToAppear;
			set => instance.m_chanceForSubsequentSocketsToAppear = value;
		}
		public static float MagicFindPerDifficultyLevel
		{
			get => instance.m_magicFindPerDifficultyLevel;
			set => instance.m_magicFindPerDifficultyLevel = value;
		}
		public static bool PrivateLoot
		{
			get => instance.m_privateLoot;
			set => instance.m_privateLoot = value;
		}
		public static float DropQuantityMultiplier
		{
			get => instance.m_dropQuantityMultiplier;
			set => instance.m_dropQuantityMultiplier = value;
		}
		public static float DropQualityMultiplier
		{
			get => instance.m_dropQualityMultiplier;
			set => instance.m_dropQualityMultiplier = value;
		}
		public static float DropChanceMultiplier
		{
			get => instance.m_dropChanceMultiplier;
			set => instance.m_dropChanceMultiplier = value;
		}
		public static float ExpMultiplier
		{
			get => instance.m_expMultiplier;
			set => instance.m_expMultiplier = value;
		}
		public static int EnemyLevelIncreaseGlobal
		{
			get => instance.m_enemyLevelIncreaseGlobal;
			set => instance.m_enemyLevelIncreaseGlobal = value;
		}
		public static int EnemyLevelIncreaseCaves
		{
			get => instance.m_enemyLevelIncreaseCaves;
			set => instance.m_enemyLevelIncreaseCaves = value;
		}
		public static float EnemyDamageMultiplier
		{
			get => instance.m_enemyDamageMultiplier;
			set => instance.m_enemyDamageMultiplier = value;
		}
		public static float EnemyHealthMultiplier
		{
			get => instance.m_enemyHealthMultiplier;
			set => instance.m_enemyHealthMultiplier = value;
		}
		public static float EnemyArmorMultiplier
		{
			get => instance.m_enemyArmorMultiplier;
			set => instance.m_enemyArmorMultiplier = value;
		}
		public static float EnemySpeedMultiplier
		{
			get => instance.m_enemySpeedMultiplier;
			set => instance.m_enemySpeedMultiplier = value;
		}
		public static bool AllowElites
		{
			get => instance.m_allowElites;
			set => instance.m_allowElites = value;
		}
		public static int LootFilterMinRarity
		{
			get => instance.m_lootFilterMinRarity;
			set => instance.m_lootFilterMinRarity = value;
		}
		public static bool CombineHitMarkers
		{
			get => instance.m_combineHitMarkers;
			set => instance.m_combineHitMarkers = value;
		}
		public static bool FullNumberHitMarkers
		{
			get => instance.m_fullNumberHitMarkers;
			set => instance.m_fullNumberHitMarkers = value;
		}
		public static float KeptExperienceAfterDeath
		{
			get => instance.m_keptExperienceAfterDeath;
			set => instance.m_keptExperienceAfterDeath = value;
		}
		public static bool EndMassacreAfterDeath
		{
			get => instance.m_endMassacreAfterDeath;
			set => instance.m_endMassacreAfterDeath = value;
		}


		// static properties
		// these are not saved
		public static bool DifficultyChosen = false;
		public static bool IsDedicated;

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