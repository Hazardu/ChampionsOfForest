using System.Collections.Generic;

namespace ChampionsOfForest
{
    public class ModSettings
    {
        public enum Difficulty { Normal, Hard, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5 }
        public enum DropsOnDeathMode { All,Equipped,Disabled }
        public static Difficulty difficulty = Difficulty.Normal;
        public static DropsOnDeathMode dropsOnDeath = DropsOnDeathMode.All;
        public static bool DifficultyChoosen = false;
        public static bool FriendlyFire = true;
        public static bool IsDedicated = false;



        public static string Version;
        public static bool RequiresNewFiles = true;
        public static bool ALLNewFiles = false;
        public static bool RequiresNewSave = false;
        public static List<int> outdatedFiles = new List<int>()
        {
           116
        };
    }
}
