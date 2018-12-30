using System.Collections.Generic;

namespace ChampionsOfForest
{
    public class ModSettings
    {
        public enum Difficulty { Normal, Hard, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5 }
        public static Difficulty difficulty = Difficulty.Normal;
        public static bool DifficultyChoosen = false;
        public static bool FriendlyFire = true;



        public static string Version = "0.4";
        public static bool RequiresNewFiles = true;
        public static List<int> outdatedFiles = new List<int>()
        {
            1,
            5,
            6,
            12,
            13,
            20,
            27,
            28,
        };
    }
}
