using System.Collections;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class MeteorSpawner : MonoBehaviour
    {
        public static MeteorSpawner Instance
        {
            get;
            private set;
        }

        private void Start()
        {
            Instance = this;
        }
        public void Spawn(Vector3 position, int seed)
        {
            StartCoroutine(DoSpawnCoroutine(position, seed));
        }

        private IEnumerator DoSpawnCoroutine(Vector3 position, int seed)
        {
            System.Random random = new System.Random(seed);
            for (int i = 0; i < random.Next(5, 11); i++)
            {

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.localScale *= 7.5f;
                go.AddComponent<Meteor>();
                Light l = go.AddComponent<Light>();
                l.intensity = 2;
                l.range = 35;
                l.color = Color.red;
                go.GetComponent<Renderer>().material.color = Color.black;
                go.transform.position = position + Vector3.forward * random.Next(-3, 3) * 2.5f + Vector3.right * random.Next(-3, 3) * 2.5f + Vector3.up * 80 + Vector3.forward * -40;
                go.GetComponent<SphereCollider>().isTrigger = true;
                yield return new WaitForSeconds(0.6f);
            }
        }

    }
}
