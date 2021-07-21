using System.Collections;

using BuilderCore;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class EnemyLaser : MonoBehaviour
	{
		public static void CreateLaser(Vector3 pos, Vector3 dir)
		{
			Transform t = new GameObject("Laser").transform;
			t.position = pos;
			t.gameObject.AddComponent<EnemyLaser>().Direction = dir;
		}

		public ParticleSystem particleSystem;
		public static Material mat;
		public static Material mat1;
		private float rotSpeed = 0;
		public Vector3 Direction;

		private IEnumerator DoPreparation()
		{
			float f = 0;
			for (int i = 0; i < 10; i++)
			{
				particleSystem.Emit(new ParticleSystem.EmitParams() { position = transform.forward * Mathf.Sin(f) * 2.5f + transform.right * Mathf.Cos(f) * 2.5f }, 10);

				yield return new WaitForSeconds(0.2f);
				f += Mathf.PI * 0.2f;
			}
			rotSpeed = 10;

			yield return new WaitForSeconds(1f);
			rotSpeed = 30;
			float up = 1;
			for (int a = 0; a < 10; a++)
			{
				float f1 = Mathf.PI * 0.2f * 0.2f * up;
				for (int i = 0; i < 10; i++)
				{
					particleSystem.Emit(new ParticleSystem.EmitParams() { position = transform.forward * Mathf.Sin(f1) * (2.5f - up / 5f) + transform.right * Mathf.Cos(f1) * (2.5f - up / 5f) + Vector3.up * up, velocity = Vector3.up * up / 10 }, 1);
					f1 += Mathf.PI * 0.2f;
				}
				yield return new WaitForSeconds(0.1f);
				rotSpeed += 15;
				up++;
			}
			yield return new WaitForSeconds(1f);
			rotSpeed += 40;
			int index = 0;
			for (int i = 0; i < 5; i++)
			{
				StartCoroutine(Shoot(index));
				yield return new WaitForSeconds(1f);
				Direction.RotateY(-180f);
				index++;
			}

			yield return new WaitForSeconds(20f);
			Destroy(gameObject);
		}

		private IEnumerator Shoot(int index)
		{
			GameObject go = new GameObject();
			go.transform.position = transform.position + Vector3.up * 13;
			go.transform.LookAt(Direction);
			int dmg = Random.Range(30, 40);

			switch (ModSettings.difficulty)
			{
				case ModSettings.Difficulty.Veteran:
					dmg = Random.Range(85, 105);
					break;

				case ModSettings.Difficulty.Elite:
					dmg = Random.Range(221, 234);
					break;

				case ModSettings.Difficulty.Master:
					dmg = Random.Range(444, 470);
					break;

				case ModSettings.Difficulty.Challenge1:
					dmg = 4124;
					break;

				case ModSettings.Difficulty.Challenge2:
					dmg = 15653;
					break;

				case ModSettings.Difficulty.Challenge3:
					dmg = 72346;
					break;

				case ModSettings.Difficulty.Challenge4:
					dmg = 85932;
					break;

				case ModSettings.Difficulty.Challenge5:
					dmg = 124636;
					break;

				case ModSettings.Difficulty.Challenge6:
					dmg = 164636;
					break;

				case ModSettings.Difficulty.Hell:
					dmg = 224636;
					break;
			}
			go.AddComponent<EnemyLaserBeam>().Initialize(dmg);
			ParticleSystem ps = go.AddComponent<ParticleSystem>();
			ParticleSystemRenderer rend = go.GetComponent<ParticleSystemRenderer>();
			rend.renderMode = ParticleSystemRenderMode.Billboard;
			rend.trailMaterial = mat1;
			rend.alignment = ParticleSystemRenderSpace.View;
			rend.material = mat1;

			ParticleSystem.ShapeModule shape = ps.shape;
			shape.enabled = false;
			ParticleSystem.MainModule m = ps.main;
			m.simulationSpace = ParticleSystemSimulationSpace.Local;
			m.startSpeed = 100;
			m.startLifetime = 0.5f;
			m.startSize = 0.1f;
			yield return null;

			ParticleSystem.EmissionModule e = ps.emission;
			e.rateOverTime = 10f;
			ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
			sol.enabled = true;
			sol.sizeMultiplier = 3;
			sol.size = new ParticleSystem.MinMaxCurve(20, new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 11.34533f, 11.34533f), new Keyframe(0.1365601f, 1f, 0.4060947f, 0.4060947f), new Keyframe(1f, 0.2881376f, -0.8223371f, -0.8223371f) }));
			yield return null;
			ParticleSystem.TrailModule trail = ps.trails;
			trail.enabled = true;
			trail.dieWithParticles = false;
			trail.inheritParticleColor = true;
			trail.ratio = 0.5f;
			trail.textureMode = ParticleSystemTrailTextureMode.Stretch;
			float time = 0;
			while (time < 8)
			{
				time += Time.deltaTime;
				if (index != 0)
				{
					float mult = index % 2 == 0 ? 1 : -1;
					go.transform.Rotate(Vector3.up * Time.deltaTime * 100 * mult, Space.World);
					go.transform.Rotate(go.transform.right * Time.deltaTime * 40 * Mathf.Sin(time * 6), Space.World);
				}
				yield return null;
			}
			Destroy(go);
		}

		private void Start()
		{
			if (mat == null)
			{
				mat = Core.CreateMaterial(new BuildingData() { renderMode = BuildingData.RenderMode.Transparent, EmissionColor = new Color(0, 0.15f, 0.20f), MainColor = new Color(0.2f, 1, 0, 0.5f), Metalic = 0.3f, Smoothness = 0.67f });
				mat1 = new Material(Shader.Find("Particles/Additive"))
				{
					mainTexture = Res.ResourceLoader.GetTexture(71),
				};
				mat1.SetColor("_TintColor", new Color(0.203f, 1, 0.629f, 0.2117647f));
			}

			particleSystem = gameObject.AddComponent<ParticleSystem>();
			ParticleSystemRenderer rend = gameObject.GetComponent<ParticleSystemRenderer>();
			rend.renderMode = ParticleSystemRenderMode.Mesh;
			rend.mesh = Res.ResourceLoader.instance.LoadedMeshes[70];
			rend.alignment = ParticleSystemRenderSpace.World;
			rend.material = mat;
			rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

			ParticleSystem.MainModule m = particleSystem.main;
			m.simulationSpace = ParticleSystemSimulationSpace.Local;

			m.startRotation3D = true;
			m.startSpeed = 0;
			m.startLifetime = 11;
			m.duration = 30;
			m.startSize = 0.3f;
			ParticleSystem.EmissionModule e = particleSystem.emission;
			e.rateOverTime = 0;
			e.enabled = false;
			StartCoroutine(DoPreparation());
		}

		private void Update()
		{
			transform.Rotate(Vector3.up * rotSpeed * 1.4f * Time.deltaTime);
		}
	}
}