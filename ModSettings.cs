namespace ChampionsOfForest
{
    public class ModSettings
    {
        public enum Difficulty { Normal, Hard, Elite, Master, Challenge1, Challenge2, Challenge3, Challenge4, Challenge5 }
        public static Difficulty difficulty = Difficulty.Normal;
        public static bool DifficultyChoosen = false;
        public static string Version = "0.0.0.2";
    }
}
