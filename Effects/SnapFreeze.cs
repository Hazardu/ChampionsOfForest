using System;
using System.Collections;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class SnapFreeze : MonoBehaviour
	{
		public static ParticleSystem hitParticleSystem;

		public static void HostAction(Vector3 pos, float dist, float slowMultipier, float duration, float damage)
		{
			RaycastHit[] hits = Physics.SphereCastAll(pos, dist, Vector3.one);
			for (int i = 0; i < hits.Length; i++)
			{
				if (EnemyManager.enemyByTransform.ContainsKey( hits[i].transform.root))
				{
					EnemyProgression prog = EnemyManager.enemyByTransform[hits[i].transform.root];
					if (prog == null)
						continue;
					prog.HitMagic(damage);
					prog.Slow(20, slowMultipier, duration);
					prog.Slow(21, 0, 0.65f);
				}
			}
		}

		public static void CreateEffect(Vector3 pos, float dist)
		{
			try
			{
				ParticleSystem ps = new GameObject().AddComponent<ParticleSystem>();
				ps.transform.position = pos;
				ps.transform.rotation = Quaternion.Euler(-90, 0, 0);
				ps.gameObject.AddComponent<SnapFreeze>();
				//hitParticleSystem = ps;
				ParticleSystem.MainModule main = ps.main;
				ParticleSystem.EmissionModule emission = ps.emission;
				ParticleSystem.ShapeModule shape = ps.shape;

				ParticleSystem.ColorOverLifetimeModule color = ps.colorOverLifetime;
				ParticleSystem.LimitVelocityOverLifetimeModule limit = ps.limitVelocityOverLifetime;
				ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();

				main.startSize = 0.4f;
				main.startSpeed = new ParticleSystem.MinMaxCurve(dist, dist * 2);
				main.duration = 0.52f;
				main.startLifetime = 1.5f;
				main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.22f, 0.43f, 0.71f, 0.4f));
				main.startRotation = 0;
				main.loop = false;

				emission.rateOverTime = 500;

				shape.shapeType = ParticleSystemShapeType.Circle;
				shape.radius = 0.001f;
				shape.arc = 360;
				shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
				//shape.arcSpeed = 2;

				limit.enabled = true;
				limit.limit = 0;
				limit.dampen = 0.14f;

				color.enabled = true;
				color.color = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0));

				rend.renderMode = ParticleSystemRenderMode.Stretch;
				rend.lengthScale = 1.3f;
				rend.velocityScale = 0.84f;
				rend.normalDirection = 1;
				Material mat1 = new Material(Shader.Find("Particles/Additive"))
				{
					mainTexture = Res.ResourceLoader.GetTexture(128)
				};

				rend.material = mat1;

				ps.Play();
			}
			catch (Exception e)
			{
				ModAPI.Console.Write(e.ToString());
			}

			RaycastHit[] hits = Physics.SphereCastAll(pos, dist, Vector3.one);
			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].transform.CompareTag("enemyCollide"))
				{
					CreateHitEffect(hits[i].point);
				}
			}
		}

		public static void CreateHitEffect(Vector3 pos)
		{
			if (hitParticleSystem == null)
			{
				HitparticlesystemAssign();
			}

			hitParticleSystem.transform.position = pos;
			hitParticleSystem.Emit(1);
		}

		private static void HitparticlesystemAssign()
		{
			ParticleSystem ps = new GameObject().AddComponent<ParticleSystem>();
			hitParticleSystem = ps;
			ParticleSystem.MainModule main = ps.main;
			ParticleSystem.EmissionModule emission = ps.emission;
			ParticleSystem.ShapeModule shape = ps.shape;
			ParticleSystem.RotationOverLifetimeModule rot = ps.rotationOverLifetime;
			ParticleSystem.SizeOverLifetimeModule size = ps.sizeOverLifetime;
			ParticleSystem.ColorOverLifetimeModule color = ps.colorOverLifetime;
			ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();

			main.startSize = 3f;
			main.startSpeed = 0f;
			main.startLifetime = 0.4f;
			main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.4f, 0.5f, 0.51f, 1f), new Color(0.56f, 0.53f, 0.8679245f, 1));
			main.startRotation = new ParticleSystem.MinMaxCurve(0, 360);
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.loop = false;

			emission.enabled = false;

			shape.enabled = false;

			rot.enabled = true;
			rot.z = 360;

			size.enabled = true;
			size.size = new ParticleSystem.MinMaxCurve(1, new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 2, 2), new Keyframe(0.8007382f, 1, 0, 0), new Keyframe(1, 0, 0, 0), }));

			color.enabled = true;
			color.color = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0));

			Material mat1 = new Material(Shader.Find("Particles/Additive"))
			{
				mainTexture = Res.ResourceLoader.GetTexture(129)
			};
			rend.material = mat1;
		}

		private void Start()
		{
			Light light = gameObject.AddComponent<Light>();
			light.shadowStrength = 1;
			light.shadows = LightShadows.Hard;
			light.type = LightType.Point;
			light.range = 25;
			light.color = new Color(0, 0.35f, 1);
			light.intensity = 2f;
			StartCoroutine(Fade(light));
			var src = gameObject.AddComponent<AudioSource>();
			src.clip = Res.ResourceLoader.instance.LoadedAudio[1014];
			src.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
			src.Play();
		}

		private IEnumerator Fade(Light light)
		{
			while (light.intensity > 0)
			{
				light.intensity -= Time.deltaTime * 2.5f;
				yield return null;
			}
		}
	}
}