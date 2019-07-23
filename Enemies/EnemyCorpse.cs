using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Enemies
{
    class EnemyCorpse : chopEnemy
    {
        protected override void Start()
        {
            base.Start();
            ModAPI.Console.Write("ChopEnemy type applied on " + transform.name +
                "\nTag is " + gameObject.tag +
                "\nLayer is "+ gameObject.layer);
          
        }
    }
}
