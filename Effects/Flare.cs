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
				light.color = fromEnemy ? new Color(1, 0.0f, 0) : new Color(1f, 0.5f, 0);
				light.shadows = LightShadows.None;

				DarkBeam comp = parent.AddComponent<DarkBeam>();
				comp.light = light;
				comp.system = ps;
				comp.fromEnemy = fromEnemy;
				comp.damageAmount = Damage;
				comp.healAmount = Healing;
				comp.boostAmount = Boost;
				comp.slowAmount = Slow;
				comp.duration = duration;
				comp.effectRadius = Radius;
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write(e.ToString());
			}
		}

		public Light light;
		public ParticleSystem system;
		public bool fromEnemy;
		public float healAmount;
		public float damageAmount;
		public float slowAmount;
		public float boostAmount;
		public float duration;
		public float effectRadius;

		private bool effectReady;

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
				main.startColor = fromEnemy ? new Color(1, 0.08f, 0, 1) : new Color(0.76f, 0.0f, 1, 1);
				shape.shapeType = ParticleSystemShapeType.ConeVolume;
				shape.angle = 0;
				shape.radius = effectRadius;
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

				effectReady = false;
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
			effectReady = true;
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
			while (effectReady)
			{
				RaycastHit[] hits = Physics.SphereCastAll(transform.position, effectRadius, Vector3.one);
				foreach (RaycastHit hit in hits)
				{
					if (hit.transform.CompareTag("enemyCollide"))
					{
						EnemyProgression ep = hit.transform.GetComponentInParent<EnemyProgression>();
						if (ep != null)
						{

							if (fromEnemy)
							{
								ep.Slow(6, boostAmount, 25);
								ep.DmgTakenDebuff(6, 0.5f, 15);
							}
							else
							{
								ep.HitMagic(damageAmount);
								ep.Slow(6, slowAmount, 10);
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

			if (effectReady)
			{
				lifetime += Time.deltaTime;
				if (duration < lifetime)
				{
					Pause();
				}

				if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < effectRadius * effectRadius)
				{
					if (fromEnemy)
					{
						if (Random.value <= ModdedPlayer.Stats.getHitChance)
						{
							LocalPlayer.Stats.HealthChange(-damageAmount * Time.deltaTime * ( ModdedPlayer.Stats.damageFromElites) * ModdedPlayer.Stats.allDamageTaken);
							BuffDB.AddBuff(1, 5, slowAmount, 20);
							LocalPlayer.Stats.Burn();
						}
						else
							COTFEvents.Instance.OnDodge.Invoke();
					}
					else
					{
						LocalPlayer.Stats.Health += healAmount * Time.deltaTime;
						LocalPlayer.Stats.HealthTarget += healAmount * 1.5f * Time.deltaTime;
						BuffDB.AddBuff(5, 6, boostAmount, 30);
						BuffDB.AddBuff(26, 94, 0.5f, 10);
					}
				}
			}
		}

		private void Pause()
		{
			StartCoroutine(LightOut());
			system.Stop();
			effectReady = false;
			Destroy(gameObject, 2);
		}
	}
}