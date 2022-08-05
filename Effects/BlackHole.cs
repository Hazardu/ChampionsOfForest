using System.Collections;
using System.Collections.Generic;
using System.IO;

using BuilderCore;

using ChampionsOfForest.Effects;
using ChampionsOfForest.Enemies;
using ChampionsOfForest.Network;
using ChampionsOfForest.Network.CommandParams;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class BlackHole : MonoBehaviour
	{
		public static void Create(Vector3 pos, params_SPELL_BLACK_HOLE param)
		{
			GameObject go = Core.Instantiate(401, pos);
			BlackHole b = go.AddComponent<BlackHole>();
		}
		public static void Create(Vector3 pos, bool fromNPC, float damage, float duration, float radius, float pullforce, ulong casterID)
		{


		}

		private float damage, duration, radius, pullforce;
		private ulong casterID;
		private bool hostile, explode, giveDamageBuff, stun, damagePlayer;

		private float scale;
		private float lifetime;

		public const float rotationSpeed = 20f;
		private Dictionary<Transform, EnemyProgression> caughtEnemies;
		public static Material particleMaterial;
		private ParticleSystem sys;
		private GameObject particleGO;
		public bool startDone = false;

		private IEnumerator Start()
		{
			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = Res.ResourceLoader.instance.LoadedAudio[1000];
			source.volume = 16;
			source.spatialBlend = 1f;
			source.rolloffMode = AudioRolloffMode.Linear;
			source.maxDistance = 150f;
			source.Play();
			RealisticBlackHoleEffect.Add(transform);
			damagePlayer = false;
			if (hostile)
			{
				damagePlayer = true;
				transform.localScale = Vector3.zero;
				yield return new WaitForSeconds(1.5f);
			}
			else if(ModSettings.FriendlyFire && ModSettings.FriendlyFireMagic)
			{
				if (casterID != ModdedPlayer.PlayerID)
				{
					damagePlayer = true;
				}
			}
			Destroy(gameObject, duration);
			if (!hostile && !GameSetup.IsMpClient)
			{
				caughtEnemies = new Dictionary<Transform, EnemyProgression>();
			}
			scale = 0;
			//if (particleMaterial == null)
			//{
			//	particleMaterial = Core.CreateMaterial(new BuildingData()
			//	{
			//		MainTexture = Res.ResourceLoader.instance.LoadedTextures[22],
			//		Metalic = 0.2f,
			//		Smoothness = 0.6f,
			//		renderMode = BuildingData.RenderMode.Cutout
			//	});
			//}
			//particleGO = new GameObject("BlackHoleParticles") { transform = { position = transform.position } };
			//particleGO.transform.SetParent(transform);
			//sys = particleGO.AddComponent<ParticleSystem>();
			//ParticleSystem.ShapeModule shape = sys.shape;
			//shape.shapeType = ParticleSystemShapeType.SphereShell;
			//shape.radius = radius * 2;
			////shape.radiusMode = ParticleSystemShapeMultiModeValue.Random;
			////shape.length = 0;
			//ParticleSystem.MainModule main = sys.main;
			//main.startSpeed = -radius * 4;
			//main.startLifetime = 0.5f;
			//main.loop = true;
			//main.prewarm = false;
			//main.startSize = 0.4f;
			//main.maxParticles = 500;

			//ParticleSystem.EmissionModule emission = sys.emission;
			//emission.enabled = true;
			//emission.rateOverTime = 500;
			//Renderer rend = sys.GetComponent<Renderer>();
			//rend.material = particleMaterial;

			//WindZone wz = gameObject.AddComponent<WindZone>();
			//wz.radius = radius * 2;
			//wz.mode = WindZoneMode.Spherical;
			//wz.windMain = 40;
			startDone = true;
			StartCoroutine(HitEverySecond());
		}

		private void FixedUpdate()
		{
			if (!startDone)
				return;
			lifetime += Time.fixedDeltaTime;

			scale = Mathf.Clamp(scale + Time.fixedDeltaTime * radius * 1.5f / duration / 2, 0, radius / 5);
			transform.localScale = Vector3.one * scale / 4;
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, scale * 5, Vector3.one, scale * 5, -10);
			foreach (RaycastHit hit in hits)
			{
				Rigidbody rb = hit.rigidbody;
				if (rb != null)
				{
					if (rb != LocalPlayer.Rigidbody || 
						(hostile || damagePlayer) && (!ModdedPlayer.Stats.perk_blackholePullImmune || ModdedPlayer.Stats.stunImmunity > 0))
					{
						Vector3 force = transform.position - rb.position;
						force *= 20 / force.magnitude;
						force *= pullforce;
						rb.AddForce(force);
					}
				}
				if (!GameSetup.IsMpClient && !hostile)
				{
					if (hit.transform.tag == "enemyCollide")
					{
						if (!caughtEnemies.ContainsKey(hit.transform.root))
						{
							var prog = hit.transform.root.GetComponentInChildren<EnemyProgression>();
							if (prog != null)
							{
								caughtEnemies.Add(hit.transform.root, prog);
							}
						}
					}
				}
			}

			transform.Translate(Vector3.up * 1.55f * Time.deltaTime * (duration - lifetime) / duration);
			transform.Rotate(Vector3.up * rotationSpeed);
			if (hostile || damagePlayer)
			{
				if (!ModdedPlayer.Stats.perk_blackholePullImmune && ModdedPlayer.Stats.stunImmunity == 0)
				{
					if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < scale * 5 * scale * 5)
					{
						Pull(LocalPlayer.Transform);
					}
				}
			}
			else if (!GameSetup.IsMpClient)
			{
				foreach (var t in caughtEnemies)
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
			while (true)
			{

				if (!hostile && !GameSetup.IsMpClient)
				{
					StartCoroutine(HitEnemies());
					yield return new WaitForSeconds(0.5f);
				}
				if (hostile || damagePlayer)
				{
					if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < scale * 4 * scale * 4)
					{
						ModdedPlayer.instance.HitMagic(-damage * ModdedPlayer.Stats.allDamageTaken * ModdedPlayer.Stats.magicDamageTaken * 0.5f);
						yield return new WaitForSeconds(0.5f);
					}
				}
				yield return null;

			}
		}

		private IEnumerator HitEnemies()
		{
			foreach (var t in caughtEnemies)
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
			t.Translate(Vector3.Normalize(t.position - transform.position) * pullforce * -Time.deltaTime, Space.World);
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
			if (doSparkOfLight && caughtEnemies.Count >= 5)
			{
				if (BoltNetwork.isRunning)
				{
					if (GameSetup.IsMpServer)
					{
						if (casterID == ModReferences.ThisPlayerID)
						{
							//local Player Callback
							SpellActions.CastBallLightningVisual(transform.position, Vector3.down);
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
					SpellActions.CastBallLightningVisual(transform.position, Vector3.down);
				}
			}
		}
	}
}