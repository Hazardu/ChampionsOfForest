using System.Collections;
using System.Collections.Generic;
using System.IO;

using BuilderCore;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class BlackHole : MonoBehaviour
	{
		public static void CreateBlackHole(Vector3 pos, bool fromEnemy, float damage, float duration = 3, float radius = 20, float pullforce = 12, string SparkOfLightAfterDarkness_PlayerID = null)
		{
			GameObject go = Core.Instantiate(401, pos);
			BlackHole b = go.AddComponent<BlackHole>();
			b.damage = damage;
			b.FromEnemy = fromEnemy;
			b.duration = duration;
			b.radius = radius;
			b.pullForce = pullforce;

			if (fromEnemy)
			{
				go.GetComponent<Light>().color = Color.red;
			}
			else
			{
				if (!string.IsNullOrEmpty(SparkOfLightAfterDarkness_PlayerID))
				{
					b.doSparkOfLight = true;
					b.casterID = SparkOfLightAfterDarkness_PlayerID;
				}
			}
		}

		public bool FromEnemy;
		public float pullForce;
		public float damage;
		public float duration;
		public float radius;
		public const float rotationSpeed = 20f;
		private float scale;
		public SphereCollider col;
		private Dictionary<Transform, EnemyProgression> CoughtEnemies;
		public static Material particleMaterial;
		private ParticleSystem sys;
		private float lifetime;
		private GameObject particleGO;

		public bool startDone = false;
		private bool doSparkOfLight;
		private string casterID;

		private IEnumerator Start()
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = Res.ResourceLoader.instance.LoadedAudio[1000];
			source.volume = 5;
			source.spatialBlend = 1f;
			source.rolloffMode = AudioRolloffMode.Linear;
			source.maxDistance = 150f;
			source.Play();
			RealisticBlackHoleEffect.Add(transform);
			if (particleMaterial == null)
			{
				particleMaterial = Core.CreateMaterial(new BuildingData()
				{
					MainTexture = Res.ResourceLoader.instance.LoadedTextures[22],
					Metalic = 0.2f,
					Smoothness = 0.6f,
					renderMode = BuildingData.RenderMode.Cutout
				});
			}
			if (FromEnemy)
			{
				transform.localScale = Vector3.zero;
				yield return new WaitForSeconds(1.7f);
			}
			Destroy(gameObject, duration);
			if (!FromEnemy && !GameSetup.IsMpClient)
			{
				CoughtEnemies = new Dictionary<Transform, EnemyProgression>();
			}
			if (GameSetup.IsMpClient)
			{
				if (col != null)
				{
					Destroy(col);
				}
			}
			else
			{
				col = gameObject.AddComponent<SphereCollider>();
				col.radius = radius;
				col.isTrigger = true;
			}
			scale = 0;
			particleGO = new GameObject("BlackHoleParticles") { transform = { position = transform.position } };
			particleGO.transform.SetParent(transform);
			sys = particleGO.AddComponent<ParticleSystem>();
			ParticleSystem.ShapeModule shape = sys.shape;
			shape.shapeType = ParticleSystemShapeType.SphereShell;
			shape.radius = radius * 2;
			//shape.radiusMode = ParticleSystemShapeMultiModeValue.Random;
			//shape.length = 0;
			ParticleSystem.MainModule main = sys.main;
			main.startSpeed = -radius * 4;
			main.startLifetime = 0.5f;
			main.loop = true;
			main.prewarm = false;
			main.startSize = 0.4f;
			main.maxParticles = 500;

			ParticleSystem.EmissionModule emission = sys.emission;
			emission.enabled = true;
			emission.rateOverTime = 500;
			Renderer rend = sys.GetComponent<Renderer>();
			rend.material = particleMaterial;

			WindZone wz = gameObject.AddComponent<WindZone>();
			wz.radius = radius * 2;
			wz.mode = WindZoneMode.Spherical;
			wz.windMain = 40;
			startDone = true;
			StartCoroutine(HitEverySecond());
		}

		private void Update()
		{
			if (!startDone)
				return;
			lifetime += Time.deltaTime;

			scale = Mathf.Clamp(scale + Time.deltaTime * radius * 1.5f / duration / 2, 0, radius / 5);
			transform.localScale = Vector3.one * scale / 3;
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, scale * 5, Vector3.one, scale * 5, -10);
			foreach (RaycastHit hit in hits)
			{
				Rigidbody rb = hit.rigidbody;
				if (rb != null)
				{
					if (rb != LocalPlayer.Rigidbody || (FromEnemy && !ModdedPlayer.Stats.perk_blackholePullImmune))
					{
						Vector3 force = transform.position - rb.position;
						force *= 20 / force.magnitude;
						force *= pullForce;
						rb.AddForce(force);
					}
				}
				if (!GameSetup.IsMpClient && !FromEnemy)
				{
					if (hit.transform.tag == "enemyCollide")
					{
						if (!CoughtEnemies.ContainsKey(hit.transform.root))
						{
							var prog = hit.transform.root.GetComponentInChildren<EnemyProgression>();
							if (prog != null)
							{
								CoughtEnemies.Add(hit.transform.root, prog);
							}
						}
					}
				}
			}

			transform.Translate(Vector3.up * 1.55f * Time.deltaTime * (duration - lifetime) / duration);
			transform.Rotate(Vector3.up * rotationSpeed);
			if (FromEnemy)
			{
				if (!ModdedPlayer.Stats.perk_blackholePullImmune)
				{
						if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < scale * 5 * scale * 5)
						{
							Pull(LocalPlayer.Transform);
						}
				}
			}
			else if (!GameSetup.IsMpClient)
			{
				foreach (var t in CoughtEnemies)
				{
					if (!t.Value.CCimmune)
					{
						Pull(t.Key);
					}
				}
			}
		}

		private IEnumerator HitEverySecond()
		{
			while (!FromEnemy)
			{
				StartCoroutine(HitEnemies());
				yield return new WaitForSeconds(0.5f);
			}
			while (FromEnemy)
			{
				if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < scale * scale * 2)
				{
					LocalPlayer.Stats.HealthChange(-damage * ModdedPlayer.Stats.allDamageTaken *  ModdedPlayer.Stats.damageFromElites * 0.5f);
					yield return new WaitForSeconds(0.5f);
				}
				else
				{
					yield return null;
				}
			}
		}

		private IEnumerator HitEnemies()
		{
			foreach (var t in CoughtEnemies)
			{
				if (t.Value != null)
				{
					t.Value.HitMagic(damage);
					yield return null;
				}
			}
		}

		private void Pull(Transform t)
		{
			t.Translate(Vector3.Normalize(t.position - transform.position) * pullForce * -Time.deltaTime, Space.World);
		}

		private static AudioSource blackholeSound;

		private void OnDestroy()
		{
			if (blackholeSound == null)
			{
				GameObject go = new GameObject();

				blackholeSound = go.AddComponent<AudioSource>();
				blackholeSound.spatialBlend = 1f;
				blackholeSound.spatialBlend = 1f;
				blackholeSound.maxDistance = 100f;
				blackholeSound.clip = Res.ResourceLoader.instance.LoadedAudio[1016];
				blackholeSound.maxDistance *= 2;
				blackholeSound.loop = false;
			}
			blackholeSound.Play();
			if (doSparkOfLight && CoughtEnemies.Count >= 5)
			{
				if (BoltNetwork.isRunning)
				{
					if (GameSetup.IsMpServer)
					{
						if (casterID == ModReferences.ThisPlayerID)
						{
							//local Player Callback
							SpellActions.CastBallLightning(transform.position, Vector3.down);
						}
						else
						{
							using (MemoryStream answerStream = new MemoryStream())
							{
								Vector3 pos = transform.position;
								using (BinaryWriter w = new BinaryWriter(answerStream))
								{
									w.Write(38);
									w.Write(casterID);
									w.Write(pos.x);
									w.Write(pos.y);
									w.Write(pos.z);
									w.Close();
								}
								NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Clients);
								answerStream.Close();
							}
						}
					}
				}
				else
				{
					SpellActions.CastBallLightning(transform.position, Vector3.down);
				}
			}
		}
	}
}