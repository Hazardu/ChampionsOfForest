using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class MainMenuVisual : MonoBehaviour
    {
        
        public static void Create()
        {
            new GameObject().AddComponent<MainMenuVisual>();
        }

        void Start()
        {
            var light1 = new GameObject().AddComponent<Light>();
            light1.color = Color.red;
            light1.shadows = LightShadows.Hard;
            light1.range = 30;
            light1.transform.position = new Vector3(0, 0, -40);
        }
    }
}
