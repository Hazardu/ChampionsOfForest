using ChampionsOfForest.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class SeekingArrow : MonoBehaviour
    {
        public static GameObject prefab;
        void Start()
        {
            if(prefab == null)
            {
                prefab = Res.ResourceLoader.GetAssetBundle(2002).LoadAsset<GameObject>("DeathMark.prefab");

            }
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }

        void Update()
        {
            transform.LookAt(Camera.main.transform);
            if (!SpellActions.SeekingArrow) transform.gameObject.SetActive(false);
        }
    }
}
