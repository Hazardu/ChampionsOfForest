using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuilderCore;
using System.Collections;
using TheForest.Items.World;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Effects
{
    public class MagicArrow : MonoBehaviour
    {
        //public static void Create(Vector3 pos, Vector3 dir, int Damage);






        public IEnumerator Animate()
        {
            transform.localScale = new Vector3(3, 3, 0);

            yield return null;
            while (transform.localScale.z<3)
            {
                transform.localScale +=Vector3.forward* Time.deltaTime;
            }



        }



    }
}
