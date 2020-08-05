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
			int Damage = 10;
			switch (ModSettings.difficulty)
			{
				case ModSettings.Difficulty.Easy:
					Damage = Random.Range(20, 30);
					break;

				case ModSettings.Difficulty.Veteran:
					Damage = Random.Range(60, 110);
					break;

				case ModSettings.Difficulty.Elite:
					Damage = Random.Range(160, 255);
					break;

				case ModSettings.Difficulty.Master:
					Damage = Random.Range(360, 500);
					break;

				case ModSettings.Difficulty.Challenge1:
					Damage = 1000;

					break;

				case ModSettings.Difficulty.Challenge2:
					Damage = 2500;

					break;

				case ModSettings.Difficulty.Challenge3:
					Damage = 4000;

					break;

				case ModSettings.Difficulty.Challenge4:
					Damage = 9000;
					break;

				case ModSettings.Difficulty.Challenge5:
					Damage = 15000;
					break;

				case ModSettings.Difficulty.Challenge6:
					Damage = 21000;
					break;

				case ModSettings.Difficulty.Hell:
					Damage = 30000;
					break;
			}
			int x = random.Next(16, 33);
			for (int i = 0; i < x; i++)
			{
				GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				go.transform.localScale *= 5.5f;
				Light l = go.AddComponent<Light>();
				l.intensity = 1.3f;
				l.range = 30;
				l.color = Color.red;
				go.GetComponent<Renderer>().material.color = Color.black;
				go.transform.position = position + Vector3.forward * random.Next(-6, 6) * 2.5f + Vector3.right * random.Next(-6, 6) * 2.5f + Vector3.up * 80 + Vector3.forward * -40;
				yield return null;
				go.GetComponent<SphereCollider>().isTrigger = true;
				go.AddComponent<Meteor>().Damage = Damage;
				yield return new WaitForSeconds((float)random.Next(10, 35) / 100f);
			}
		}
	}
}