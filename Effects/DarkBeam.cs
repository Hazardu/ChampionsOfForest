using System.Collections;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class DarkBeam : MonoBehaviour
	{
		public static int DesiredLayer;

		private static Material material;

		public static void Initialize()
		{
			//material = new Material(Shader.Find("Particles/Additive"));
			material = new Material(Shader.Find("Particles/Multiply"));
			material.SetFloat("_InvFade", 1);
			material.renderQueue = 3000;
			material.mainTexture = Res.ResourceLoader.GetTexture(107);
			material.color = new Color(1, 1, 1, 1);
		}

		/// <summary>
		/// creates a beam of energy.
		/// </summary>
		public static void Create(Vector3 position, bool fromEnemy, float Damage, float Healing, float Slow, float Boost, float duration = 10f, float Radius = 3.5f)
		{
			try
			{
				GameObject parent = new GameObject();
				GameObject particleObj = new GameObject();
				GameObject lamp = new GameObject();

				if (material == null)
				{
					Initialize();
				}

				particleObj.transform.SetParent(parent.transform);
				lamp.transform.SetParent(parent.transform);

				parent.transform.position = position;
				lamp.transform.position = position + new Vector3(0, 12, 0);
				lamp.transform.rotation = Quaternion.Euler(90f, 0, 0);

				ParticleSystem ps = particleObj.AddComponent<ParticleSystem>();

				particleObj.transform.rotation = Quaternion.Euler(-60f, Random.Range(0, 360), 0);
				particleObj.transform.position = position;

				Light light = lamp.AddComponent<Light>();
				light.type = LightType.Spot;
				light.spotAngle = Mathf.Atan(Radius / 12) * 90;
				light.intensity = 0;   //going towards 30
				light.range = 20;
				if (fromEnemy)
				{
					light.color = new Color(1, 0.0f, 0);
				}
				else
				{
					light.color = new Color(1f, 0.5f, 0);
				}
				light.shadows = LightShadows.None;

				DarkBeam comp = parent.AddComponent<DarkBeam>();
				comp.light = light;
				comp.system = ps;
				comp.fromEnemy = fromEnemy;
				comp.Damage = Damage;
				comp.Healing = Healing;
				comp.Boost = Boost;
				comp.Slow = Slow;
				comp.Duration = duration;
				comp.Radius = Radius;
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		public Light light;
		public ParticleSystem system;
		public bool fromEnemy;
		public float Healing;
		public float Damage;
		public float Slow;
		public float Boost;
		public float Duration;
		public float Radius;

		private bool EffectReady;

		private void Start()
		{
			try
			{
				ParticleSystem ps = system;
				ParticleSystem.MainModule main = ps.main;
				ParticleSystem.EmissionModule emission = ps.emission;
				ParticleSystem.ShapeModule shape = ps.shape;
				ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();
				emission.enabled = true;
				shape.enabled = true;
				rend.enabled = true;

				main.loop = true;
				main.startLifetime = 0.9f;
				main.startSpeed = -200;
				main.startSize = 3;
				main.duration = 10;

				main.maxParticles = 100000;
				main.simulationSpace = ParticleSystemSimulationSpace.Local;
				if (fromEnemy)
				{
					main.startColor = new Color(1, 0.08f, 0, 1);
				}
				else
				{
					main.startColor = new Color(0.76f, 0.0f, 1, 1);
				}
				shape.shapeType = ParticleSystemShapeType.ConeVolume;
				shape.angle = 0;
				shape.radius = Radius;
				shape.length = 1;
				shape.radiusMode = ParticleSystemShapeMultiModeValue.Random;
				shape.alignToDirection = true;

				rend.renderMode = ParticleSystemRenderMode.Stretch;
				rend.velocityScale = 0.11f;
				rend.lengthScale = 0.87f;
				rend.pivot = new Vector3(0, -20, 0);
				rend.normalDirection = 1;
				rend.sharedMaterial = material;
				rend.maxParticleSize = 60000;
				rend.alignment = ParticleSystemRenderSpace.World;

				emission.rateOverTime = 180;
				emission.rateOverTimeMultiplier = 180;

				EffectReady = false;
				StartCoroutine(AnimatedBeamCoroutine());
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		private IEnumerator AnimatedBeamCoroutine()
		{
			StartCoroutine(LightIn());
			yield return null;
			system.Pause();
			while (light.intensity < 25)
			{
				yield return null;
			}

			StartCoroutine(DoEnemyCheck());
			system.Play();
			EffectReady = true;
		}

		private IEnumerator LightIn()
		{
			while (light.intensity < 25)
			{
				light.intensity += 10f * Time.deltaTime;

				yield return null;
			}
		}

		private IEnumerator LightOut()
		{
			while (light.intensity > 0)
			{
				light.intensity -= 25 * Time.deltaTime;

				yield return null;
			}
		}

		private IEnumerator DoEnemyCheck()
		{
			yield return null;
			yield return null;
			while (EffectReady)
			{
				RaycastHit[] hits = Physics.SphereCastAll(transform.position, Radius, Vector3.one);
				foreach (RaycastHit hit in hits)
				{
					if (hit.transform.CompareTag("enemyCollide"))
					{
						EnemyProgression ep = hit.transform.GetComponentInParent<EnemyProgression>();
						if (ep != null)
						{

							if (fromEnemy)
							{
								ep.HealthScript.Health = (int)Mathf.Clamp(ep.HealthScript.Health + Healing / 2, 0, ep.maxHealth);
								ep.Slow(6, Boost, 25);
							}
							else
							{
								ep.HitMagic(Damage);
								ep.Slow(6, Slow, 10);
							}
						}
					}
				}
				yield return new WaitForSeconds(0.5f);
			}
		}

		private float lifetime = 0;

		private void Update()
		{
			if (gameObject.layer != DesiredLayer)
			{
				gameObject.layer = DesiredLayer;
			}

			if (EffectReady)
			{
				lifetime += Time.deltaTime;
				if (Duration < lifetime)
				{
					Pause();
				}

				if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < Radius * Radius)
				{
					if (fromEnemy)
					{
						LocalPlayer.Stats.HealthChange(-Damage * Time.deltaTime * ( ModdedPlayer.Stats.magicDamageTaken) * ModdedPlayer.Stats.allDamageTaken);
						Player.BuffDB.AddBuff(1, 5, Slow, 20);
						LocalPlayer.Stats.Burn();
					}
					else
					{
						LocalPlayer.Stats.Health += Healing * Time.deltaTime;
						LocalPlayer.Stats.HealthTarget += Healing * 1.5f * Time.deltaTime;
						Player.BuffDB.AddBuff(5, 6, Boost, 30);
						Player.BuffDB.AddBuff(26, 94, 0.5f, 10);
					}
				}
			}
		}

		private void Pause()
		{
			StartCoroutine(LightOut());
			system.Stop();
			EffectReady = false;
			Destroy(gameObject, 2);
		}
	}
}