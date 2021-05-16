using System.Collections.Generic;
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

		public static Difficulty difficulty = Difficulty.Easy;
		public static DropsOnDeathMode dropsOnDeath = DropsOnDeathMode.Disabled;
		public static bool DifficultyChoosen = false;
		public static bool FriendlyFire = true;
		public static bool IsDedicated = false;
		public static bool killOnDowned = false;

		public static string Version;
		public const bool RequiresNewFiles = false;
		public const bool ALLNewFiles = false;
		public const bool RequiresNewSave = true;
		public const string RequiresNewSaveVersion = "1.6.0.2";

		public static readonly List<int> outdatedFiles = new List<int>()
		{

		};

		public static float DropQuantityMultiplier = 1;
		public static float DropChanceMultiplier = 1;
		public static float ExpMultiplier = 1;
		public static int EnemyLevelIncrease = 0;
		public static float EnemyDamageMultiplier = 1;
		public static float EnemyHealthMultiplier = 1;
		public static float EnemyArmorMultiplier = 1;
		public static float EnemySpeedMultiplier = 1;
		public static bool AllowElites = true;



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
	}
}