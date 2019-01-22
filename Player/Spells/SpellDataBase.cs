using System;
using System.Collections.Generic;

namespace ChampionsOfForest.Player
{
    public static class SpellDataBase
    {
        public static Dictionary<int, Spell> spellDictionary = new Dictionary<int, Spell>();


        public static void Initialize()
        {
            try
            {
                spellDictionary = new Dictionary<int, Spell>();
                FillSpells();
                ModAPI.Log.Write("SETUP: SPELL DB");

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        public static void FillSpells()
        {
            Spell bh = new Spell(1, 22, 15, 50, 90, "Black Hole", "Creates a black hole that pulls enemies in and damages them every second")
            {
                active = SpellActions.CreatePlayerBlackHole,

            };
            Spell healingDome = new Spell(2, 22, 6, 60, 60, "Healing Dome", "Creates a sphere of vaporized aloe that heals all allies inside. Items can further expand this ability to cleanese debuffs. Scales with healing multipier and spell amplification.")
            {
                active = SpellActions.CreateHealingDome,

            };
            new Spell(3, 22, 2, 25, 15, "Blink", "Short distance teleportation")
            {
                active = SpellActions.DoBlink,

            };
            new Spell(4, 22, 8, 100, 45, "Flare", "A magic collumn heals players inside and gives them +25% movement speed, while slowing damaging enemies. Slow amount is equal to 25%")
            {
                active = SpellActions.CastFlare,
                
            };
        }
    }
}
