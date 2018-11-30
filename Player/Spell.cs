using UnityEngine;
namespace ChampionsOfForest.Player
{
    public class Spell
    {
        public int ID;
      
        public Texture2D icon;
        public int Levelrequirement;
        public float EnergyCost;
        public int BaseCooldown;

        public int EquippedSlot;
        public bool IsEquipped
        {
            get
            {
                if (EquippedSlot != -1)
                {
                    return true;
                }
                return false;
            }
        }
  public delegate void Active();
        public Active active;
        public delegate void Passive();
        public Passive passive;



        public bool Bought;
        public int level;
        public bool CanCast = false;
        public string Name;
        public string Description;


        public Spell()
        {

        }
    }
}
