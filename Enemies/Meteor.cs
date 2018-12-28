using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using TheForest.Utils;
using TheForest.World;
using System.Collections;

namespace ChampionsOfForest.Enemies
{
    public class MeteorSpawner : MonoBehaviour
    {
        public static MeteorSpawner Instance
        {
            get;
            private set;
        }
        void Start()
        {
            Instance = this;
        }
        public void Spawn(Vector3 position, int seed)
        {
            StartCoroutine(DoSpawnCoroutine(position, seed));
        }
        IEnumerator DoSpawnCoroutine(Vector3 position, int seed)
        {
System.Random random = new System.Random(seed);
            for (int i = 0; i < random.Next(2, 6); i++)
            {
                
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.localScale *= 6.4f;
                go.AddComponent<Meteor>();
                Light l = go.AddComponent<Light>();
                l.intensity = 3;
                l.range = 30;
                l.color = Color.red;
                go.transform.position = position+ Vector3.forward * random.Next(-3, 3)*2.5f + Vector3.right * random.Next(-3, 3)*2.5f  + Vector3.up * 80 + Vector3.forward *-40;
                go.GetComponent<SphereCollider>().isTrigger = true;
                yield return new WaitForSeconds(0.7f);
            }
        }

    }
    public class Meteor : MonoBehaviour
    {
        
        public static void CreateEnemy(Vector3 position,int seed)
        {
            MeteorSpawner.Instance.Spawn(position, seed);
        }
        int Damage;

        void Update()
        {
            transform.Translate((Vector3.down*2 + Vector3.forward) * Time.deltaTime * 30 );

        }


        void Start()
        {
            Destroy(gameObject, 5);
            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Normal:
                    Damage = 200;
                    break;
                case ModSettings.Difficulty.Hard:
                    Damage = 1000;

                    break;
                case ModSettings.Difficulty.Elite:
                    Damage = 5000;

                    break;
                case ModSettings.Difficulty.Master:
                    Damage = 25000;

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Damage = 75000;

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Damage = 125000;

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Damage = 300000;

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Damage = 650000;
                    break;
                case ModSettings.Difficulty.Challenge5:
                    Damage = 1000000;
                    break;
                           }

        }

        void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("suitCase") || other.CompareTag("metalProp") || other.CompareTag("animalCollide") || other.CompareTag("Fish") || other.CompareTag("Tree") || other.CompareTag("MidTree") || other.CompareTag("suitCase") ||other.CompareTag("SmallTree") ) {
                other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("Burn", Damage, SendMessageOptions.DontRequireReceiver);
                LocalPlayer.HitReactions.enableFootShake(1, 0.4f);

            }
            else if (other.CompareTag("Player") || other.CompareTag("playerHitDetect"))
            {
                LocalPlayer.Stats.Hit((int)(Damage * ModdedPlayer.instance.MagicResistance),true,PlayerStats.DamageType.Fire);
                other.SendMessage("Burn", Damage, SendMessageOptions.DontRequireReceiver);
                LocalPlayer.HitReactions.enableFootShake(1, 1.2f);

            }
            else if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") ||other.CompareTag("BreakableRock") || other.CompareTag("structure"))
            {
                other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("LocalizedHit", new LocalizedHitData(transform.position,Damage), SendMessageOptions.DontRequireReceiver);
                LocalPlayer.HitReactions.enableFootShake(1, 0.4f);

            }
        }
    }
}
