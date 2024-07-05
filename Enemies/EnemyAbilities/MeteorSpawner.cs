using System.Collections;

using ChampionsOfForest.Effects.Sound_Effects;

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
		static Material meteorMaterial;

		private void Start()
		{
			meteorMaterial = BuilderCore.Core.CreateMaterial(new BuilderCore.BuildingData()
			{
				EmissionColor= new Color(0.3f,0,0,0.5f),
				MainColor= Color.black,
				Metalic = 1,
				Smoothness=1,
			});
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
					Damage = Random.Range(40, 50);
					break;

				case ModSettings.Difficulty.Veteran:
					Damage = Random.Range(100, 150);
					break;

				case ModSettings.Difficulty.Elite:
					Damage = Random.Range(300, 355);
					break;

				case ModSettings.Difficulty.Master:
					Damage = Random.Range(660, 700);
					break;

				case ModSettings.Difficulty.Challenge1:
					Damage = 3000;
					break;

				case ModSettings.Difficulty.Challenge2:
					Damage = 9000;
					break;

				case ModSettings.Difficulty.Challenge3:
					Damage = 14000;
					break;

				case ModSettings.Difficulty.Challenge4:
					Damage = 25000;
					break;

				case ModSettings.Difficulty.Challenge5:
					Damage = 50000;
					break;

				case ModSettings.Difficulty.Challenge6:
					Damage = 65000;
					break;

				case ModSettings.Difficulty.Hell:
					Damage = 75000;
					break;
			}

			Vector3 startVector = position + ( Vector3.up * 80 );
			//Trace down to the ground
			Vector3 soundEmitterPositon = Physics.Raycast(startVector, Vector3.down, out var raycastHit) ? raycastHit.transform.position : position;
			int x = random.Next(16, 33);

			GameObject soundEmitter = new GameObject();
			MeteorSoundEmitter emitter = soundEmitter.AddComponent<MeteorSoundEmitter>();
			soundEmitter.transform.position = soundEmitterPositon;
			//emitter.PlaySpawnSound();
			emitter.nMeteors = x;
			for (int i = 0; i < x; i++)
			{
				GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				go.transform.localScale *= 4f;

				Light l = go.AddComponent<Light>();
				l.type = LightType.Spot;
				l.intensity = 1.3f;
				l.range = 40;
				l.color = Color.red;
				yield return null;
				go.GetComponent<Renderer>().material = meteorMaterial;
				go.transform.position = position + Vector3.forward * random.Next(-6, 6) * 2.5f + Vector3.right * random.Next(-6, 6) * 2.5f + Vector3.up * 80 + Vector3.forward * -40;
				go.GetComponent<SphereCollider>().isTrigger = true;
				Meteor metorComponent = go.AddComponent<Meteor>();
				metorComponent.Damage = Damage;
				metorComponent.soundEmitter = emitter;
				yield return new WaitForSeconds((float)random.Next(10, 45) / 100f);
			}
		}
	}
}