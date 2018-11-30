//USING THIS CLASS WOULD CAUSE BUGS WITH MODAPI. GAME WOULDNT LAUNCH
//THIS IS ITS LEGACY,
//MASSACRED BY COMMENTS 

//using Bolt;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace ChampionsOfForest.Enemies
//{
//   public  class WaterDeathEx : mutantWaterDetect
//    {
//        protected override void Update()
//        {
//            if (waterHeight > 4f)
//            {
//                underWater = true;
//                if (Time.time > inWaterTimer - 1f && !drowned)
//                {
//                    if (BoltNetwork.isClient)
//                    {
//                        PlayerHitEnemy playerHitEnemy = PlayerHitEnemy.Create(GlobalTargets.OnlyServer);
//                        playerHitEnemy.Target = base.GetComponent<BoltEntity>();
//                        playerHitEnemy.Hit = 1000000;
//                        playerHitEnemy.Send();
//                    }
//                    else
//                    {
//                        setup.health.HitReal(1000000);
//                    }
//                    drowned = true;
//                }
//            }
//        }
//    }
//}
