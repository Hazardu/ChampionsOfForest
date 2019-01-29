using System.Collections.Generic;

namespace ChampionsOfForest
{
    public class ModSettings
    {
        public enum Difficulty { Normal, Hard, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5 }
        public static Difficulty difficulty = Difficulty.Normal;
        public static bool DifficultyChoosen = false;
        public static bool FriendlyFire = true;
        public static bool IsDedicated = false;



        public static string Version = "0.7";
        public static bool RequiresNewFiles = false;
        public static bool RequiresNewSave = false;
        public static List<int> outdatedFiles = new List<int>()
        {
           
        };
    }
}
