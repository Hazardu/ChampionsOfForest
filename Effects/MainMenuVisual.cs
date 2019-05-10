using ChampionsOfForest.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class MainMenuVisual : MonoBehaviour
    {

        public static void Create()
        {
            new GameObject().AddComponent<MainMenuVisual>();
        }

        private IEnumerator Start()
        {
            do
            {
                yield return null;
                yield return null;
            }
            while (!Res.ResourceLoader.instance.FinishedLoading);


            Light light1 = new GameObject().AddComponent<Light>();
            light1.color = Color.white;
            light1.shadows = LightShadows.Hard;
            light1.range = 30;
            light1.transform.position = new Vector3(0, 0, -40);

           
        }
    }
}
