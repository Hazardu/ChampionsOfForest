using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Save;

namespace ChampionsOfForest.Player
{
    public class PlayerRespawnMod : PlayerRespawnMP
    {
        protected override void Respawn()
        {
            base.Respawn();
            try
            {
            ModdedPlayer.ResetAllStats();

            }
            catch (Exception e)
            {

                ModAPI.Log.Write(e.ToString());
            }
            ModdedPlayer.instance.ExpCurrent = 0;
        }
    }
}
