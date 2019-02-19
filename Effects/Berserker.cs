using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class Berserker
    {

        public static bool on;
        public static bool active;
        public static float LastProcTime;
        public const float Cooldown = 120;


        public static void TryProc()
        {
            if (on)
            {
                if (Time.time > LastProcTime + Cooldown)
                {

                }
            }
        }
        static void Proc()
        {
            LastProcTime = Time.time;

        }
        public static void OnEnable()
        {
            active = true;
        }
        public static void OnDisable()
        {
            active = false;
        }
    }
}
