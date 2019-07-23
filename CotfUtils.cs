using System.Text.RegularExpressions;
using UnityEngine;

namespace ChampionsOfForest
{
    public static class CotfUtils
    {
        //removes any non ascii characters from a name of the player
        public static string TrimNonAscii(this string value)
        {
            string pattern = "[^ -~]+";
            Regex reg_exp = new Regex(pattern);
            return reg_exp.Replace(value, "1");
        }

        public static void Log(string s)
        {
            //ModAPI.Console.Write(s);
            Debug.LogWarning(s);
        }
    }
}
