using System.Collections.Generic;

namespace ChampionsOfForest
{
    public class ModSettings
    {
        public enum Difficulty { Normal, Hard, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5, Challenge6 , Hell }
        public enum DropsOnDeathMode { All,Equipped,Disabled }
        public static Difficulty difficulty = Difficulty.Normal;
        public static DropsOnDeathMode dropsOnDeath = DropsOnDeathMode.Disabled;
        public static bool DifficultyChoosen = false;
        public static bool FriendlyFire = true;
        public static bool IsDedicated = false;



        public static string Version;
        public const bool RequiresNewFiles = false;
        public const bool ALLNewFiles = false;
        public const bool RequiresNewSave = true;
        public const string RequiresNewSaveVersion = "1.4.1";
        public static readonly List<int> outdatedFiles = new List<int>()
        {
           116
        };
    }
}
