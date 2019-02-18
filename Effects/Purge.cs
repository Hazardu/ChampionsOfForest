using ChampionsOfForest.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class Purge : MonoBehaviour
    {
        private float speed = -30;
        private float radius = 1;
        private static Material mat;
        public static void Cast(Vector3 pos, float radius)
        {
            if (mat == null)
            {
                mat = new Material(Shader.Find("Particles/Additive"));
                mat.SetColor("_TintColor", new Color(1, 1, 0.0f, 0.5f));
                mat.mainTexture = Res.ResourceLoader.GetTexture(126);
            }


            var o = GameObject.CreatePrimitive(PrimitiveType.Quad);
            o.transform.position = pos + Vector3.up * 10 ;
            o.transform.rotation = Quaternion.Euler(90, 0, 0);
            o.RemoveComponent(typeof(Collider));
            o.GetComponent<Renderer>().material = mat;
            Purge p1 =o.AddComponent<Purge>();
            p1.radius = radius;

            var p2 = Instantiate(o);
            var pu2 = p2.GetComponent<Purge>();
            pu2.speed *= 30;
            pu2.radius = radius;
            p2.transform.position -=Vector3.up* 20;

            if ((LocalPlayer.Transform.position - pos).sqrMagnitude < radius * radius)
            {
                PurgeLocalPlayer();
            }
           
        }
        private static void PurgeLocalPlayer()
        {
            var keys = BuffDB.activeBuffs.Keys.ToArray();
            int a = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (BuffDB.activeBuffs[keys[i]].isNegative)
                {
                    BuffDB.activeBuffs[keys[i]].ForceEndBuff(keys[i]);
                    a++;
                }
            }
            for (int i = 0; i < a; i++)
            {
                LocalPlayer.Stats.Health *= 0.8f;
                LocalPlayer.Stats.HealthTarget = LocalPlayer.Stats.Health;
                LocalPlayer.Stats.Energy *= 0.8f;
            }
        }

        void Start()
        {
            Destroy(gameObject, 2);
            transform.localScale = Vector3.one*radius/2;
        }

        void Update()
        {
            transform.localScale += Vector3.one * Time.deltaTime * radius/2;
            transform.position += Vector3.up * Time.deltaTime * speed;
            transform.Rotate(Vector3.up * 40 * speed * Time.deltaTime,Space.World);
        }

    }
}
