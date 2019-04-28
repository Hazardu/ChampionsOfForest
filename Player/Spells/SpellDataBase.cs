using System;
using System.Linq;
using System.Collections.Generic;
using ChampionsOfForest.Effects;

namespace ChampionsOfForest.Player
{
    public static class SpellDataBase
    {
        public static Dictionary<int, Spell> spellDictionary = new Dictionary<int, Spell>();
        public static int[] SortedSpellIDs;

        public static void Initialize()
        {
            try
            {
                spellDictionary = new Dictionary<int, Spell>();
                FillSpells();
               List<int> SortedSpellIDsTemp = new List<int>(spellDictionary.Keys);
                SortedSpellIDsTemp.Sort((x, y) => spellDictionary[x].Levelrequirement.CompareTo(spellDictionary[y].Levelrequirement));
                SortedSpellIDs = SortedSpellIDsTemp.ToArray();
               // ModAPI.Log.Write("SETUP: SPELL DB");

            }
            catch (Exception ex)
            {

                ModAPI.Log.Write(ex.ToString());
            }
        }

        public static void FillSpells()
        {
            Spell bh = new Spell(1, 119, 20, 50, 90, "Black Hole", "Creates a black hole that pulls enemies in and damages them every second")
            {
                active = SpellActions.CreatePlayerBlackHole,

            };
            Spell healingDome = new Spell(2, 122, 6, 60, 60, "Healing Dome", "Creates a sphere of vaporized aloe that heals all allies inside. Items can further expand this ability to cleanese debuffs. Scales with healing multipier and spell amplification.")
            {
                active = SpellActions.CreateHealingDome,

            };
            new Spell(3, 121, 3, 25, 15, "Blink", "Short distance teleportation")
            {
                active = SpellActions.DoBlink,

            };
            new Spell(4, 120, 8, 100, 45, "Flare", "A magic collumn heals players inside and gives them +25% movement speed, while slowing damaging enemies. Slow amount is equal to 25%")
            {
                active = SpellActions.CastFlare,
                
            };
            new Spell(5, 118, 4, 50, "Sustain Shield", "Channeling this spell consumes energy but grants you a protective, absorbing shield. The shield's power increases every second untill reaching max value. Upon ending the channeling by any source, the shield persist for a short amount of time, and after that it rapidly decreases.")
            {
                active = SpellActions.CastSustainShieldActive,
                passive = SpellActions.CastSustainShielPassive,
                usePassiveOnUpdate = true,
            
            };
            new Spell(6, 117, 2, 15, 1, "Wide Reach", "Picks up all resources in a small radius around you.")
            {
                active = AutoPickupItems.DoPickup,            
            };
            new Spell(7, 115, 5, 25, 2, "Black Flame", "Ignites your weapon with a dark flame that empowers all attacks.")
            {
                active =BlackFlame.Toggle,            
            };
            new Spell(8,123, 12, 75, 45, "War Cry", "Empowers you and nearby allies for 2 minutes.")
            {
                active =SpellActions.CastWarCry,            
            };
            new Spell(9, 114, 15, 120, 100, "Portal", "Creates a wormhole, that links 2 locations. Allows the player and items to pass through.")
            {
                active =SpellActions.CastPortal,            
            };
            new Spell(10, 125, 30, 80, 20, "Magic Arrow", "A large arrow is shot where you're looking at. Slows any enemies on hit and deals big damage")
            {
                active =SpellActions.CastMagicArrow,            
            };
            new Spell(11, 127, 35, 1, 1, "Multishot", "An attack modifier. Enchants your ranged weapons to shoot multipe projectiles. Upgradeable by perks. Energy is consumed upon firing and depends on the amount of projectiles fired")
            {
                active =SpellActions.ToggleMultishot,            
            };
            new Spell(12, 133, 40,50, 200, "Gold", "For 20 seconds you turn completely immune to stuns and attack speed increases by 20%")
            {
                active =Gold.Cast,            
            };
            new Spell(13, 132, 6, 65, 15, "Purge", "Everyone in your surroudings gets cleansed of their negative debuffs. Those debuffs can be poison. For every debuff purged, the player looses 20% of his current health and energy")
            {
                active =SpellActions.CastPurge,            
            };
            new Spell(14, 128, 20, 200, 45, "Snap Freeze", "Enemies around you get slowed for 12 seconds by 80% you deal magic damage to them")
            {
                active =SpellActions.CastSnapFreeze,            
            };
            new Spell(15, 131, 25, 15, 180, "Berserk", "For short amount of time, gain damage, attack speed and movement speed, take additional damage and have unlimited stamina.")
            {
                active =Berserker.Cast,            
            };
            new Spell(16, 130, 42, 300, 80, "Ball Lightning", "A slow moving, bouncing ball of lightning travels forward, dealing damage to hit enemies, and upon contact or when it lasts too long, it explodes. Scales with 400% spell damage.")
            {
                active =SpellActions.CastBallLightning,            
            };
        }
    }
}
