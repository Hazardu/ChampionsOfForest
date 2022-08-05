using System.Collections;
using System.Collections.Generic;

using ChampionsOfForest.Network;
using ChampionsOfForest.Network.CommandParams;
using ChampionsOfForest.Player;

using HutongGames.PlayMaker.Actions;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class BallLightning : MonoBehaviour
	{
		public static Dictionary<uint, BallLightning> list = new Dictionary<uint, BallLightning>();
		public static uint lastID;

		public static GameObject prefab;

		public static void InitPrefab()
		{
			string _name = "OrbPrefab.prefab";

			AssetBundle bundle = ChampionsOfForest.Res.ResourceLoader.GetAssetBundle(2000);
			if (bundle == null)
			{
				ModAPI.Log.Write("Couldn't load asset bundle");
				return;
			}

			prefab = bundle.LoadAsset<GameObject>(_name);
		}

		public static void Create(Vector3 position, Vector3 speed, params_SPELL_BALLLIGHTNING param)
		{
			GameObject o = GameObject.Instantiate(prefab, position, Quaternion.identity);
			o.tag = "enemyCollide";
			BallLightning b = o.AddComponent<BallLightning>();
			b.dmg = param.Damage;
			b.ID = param.ID;
			b.speed = speed * 2;
			b.immediateExplosion = param.ImmediateExplode;
			b.crit = param.Crit;
			b.casterID = param.casterID;
			if (!param.ImmediateExplode)
				list.Add(param.ID, b);
		}

		internal uint ID;
		private ulong casterID;
		private GameObject explosionFX;
		private GameObject mainFX;
		private Rigidbody rb;
		private float dmg;
		private Vector3 speed;
		private bool triggered, immediateExplosion, crit;

		// Use this for initialization
		private void Start()
		{
			if (explosionFX == null)
			{
				explosionFX = transform.Find("ExplosionFX").gameObject;
			}
			if (mainFX == null)
			{
				mainFX = transform.Find("mainFX").gameObject;
			}
			rb = GetComponent<Rigidbody>();
			var trigger = new GameObject { transform = { position = transform.position, parent = transform } };
			var collider = trigger.AddComponent<SphereCollider>();
			collider.radius = 3;
			collider.isTrigger = true;
			var balltrigger = trigger.AddComponent<BallLightningTrigger>();
			balltrigger.ball = this;
			if (immediateExplosion)
			{
				triggered = true;
				Explode();
			}
			else
				Invoke("Trigger", 30);
		}

		public void Explode()
		{
			StartCoroutine(ExplodeCoroutine());
		}

		public void Trigger()
		{
			if (!triggered)
			{
				SyncTriggerPosition();
				rb.isKinematic = true;
				triggered = true;
				Explode();
				list.Remove(ID);
			}
		}

		private void SyncTriggerPosition()
		{
			if (!BoltNetwork.isRunning)
				return;
			Vector3 pos = transform.position;
			var cmd = new CommandStream(Commands.CommandType.SPELL_BALL_LIGHTNING_TRIGGER);
			cmd.Write(31);
			cmd.Write(ID);
			cmd.Write(pos);
			cmd.Send(NetworkManager.Target.Others);
		}

		public void CoopTrigger(Vector3 newPos)
		{
			transform.position = newPos;
			triggered = true;
			rb.isKinematic = true;
			Explode();
			list.Remove(ID);

		}

		public IEnumerator ExplodeCoroutine()
		{
			explosionFX.SetActive(true);
			yield return new WaitForSeconds(4.55f);
			mainFX.SetActive(false);
			OnExplode();
			Destroy(gameObject, 5);
		}

		public void OnExplode()
		{
			//deal damage to enemies, apply force to rigidbodies etc

			StartCoroutine(ExplosionDamageAsync());

		}

		private IEnumerator ExplosionDamageAsync()
		{
			RaycastHit[] hits = Physics.SphereCastAll(transform.position, 28, Vector3.one, 30);
			int i = 0;
			foreach (RaycastHit hit in hits)
			{

				hit.rigidbody?.AddExplosionForce(500, transform.position, 30, 1.2f, ForceMode.Impulse);
				if (hit.transform.CompareTag("enemyCollide"))
				{
					HitEnemy(hit.transform);
					hit.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
					if ((++i & 7) == 0)
						yield return null;
				}
				else if (!hit.transform.CompareTag("Player") && hit.transform.root != LocalPlayer.Transform)
				{
					hit.transform.SendMessageUpwards("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
					hit.transform.SendMessage("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
					if ((hit.transform.position - transform.position).sqrMagnitude < 100)
					{
						hit.transform.SendMessageUpwards("Explosion", (int)hit.distance, SendMessageOptions.DontRequireReceiver);
						hit.transform.SendMessage("Explosion", (int)hit.distance, SendMessageOptions.DontRequireReceiver);
					}
					hit.transform.SendMessage("CutDown", SendMessageOptions.DontRequireReceiver);
					hit.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
					if ((++i & 7) == 0)
						yield return null;
				}
			}
			
		}

		public void HitEnemy(Transform t)
		{
			if (!GameSetup.IsMpClient)
			{
				t.root.SendMessage("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
			}
			if (casterID == ModdedPlayer.PlayerID)
			{
				COTFEvents.Instance.OnHitSpell.Invoke(new COTFEvents.HitOtherParams(dmg, crit, t, this));
				COTFEvents.Instance.OnIgniteSpell.Invoke();
			}
		}
		public void HitPlayer(Transform t)
		{
			if (!GameSetup.IsMpClient)
			{
				t.root.SendMessage("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
			}
			if (casterID == ModdedPlayer.PlayerID)
			{
				COTFEvents.Instance.OnHitSpell.Invoke(new COTFEvents.HitOtherParams(dmg, crit, t, this));
				COTFEvents.Instance.OnIgniteSpell.Invoke();
			}
		}
		public void Hit(int damage)
		{
			Trigger();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.CompareTag("enemyCollide"))
			{
				HitEnemy(collision.transform);
				Trigger();
			}
			else if (!(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerNet")))
			{
				collision.transform.SendMessageUpwards("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
				collision.transform.SendMessage("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
				collision.transform.SendMessageUpwards("Explosion", 0, SendMessageOptions.DontRequireReceiver);
				collision.transform.SendMessage("Explosion", 0, SendMessageOptions.DontRequireReceiver);
				Vector3 normal = collision.contacts[0].normal;
				Vector3 newSpeed = Vector3.Reflect(speed, normal);
				speed = newSpeed * 1.02f;
			}
			else
			{
				if (ModSettings.FriendlyFireMagic)
				{
					if (collision.transform.root == LocalPlayer.Transform.root)
					{
						if (ModdedPlayer.PlayerID != casterID)
						{
							ModdedPlayer.instance.HitMagic(dmg);
							COTFEvents.Instance.OnFriendlyFire.Invoke();
						}
					}
				}
			}
		}

		private void Update()
		{
			if (!triggered)
			{
				transform.Translate(speed * Time.deltaTime, Space.World);
				speed.y -= Time.deltaTime * 2.5f;
			}
		}
	}

	public class BallLightningTrigger : MonoBehaviour
	{
		public BallLightning ball;

		private void OnTriggerEnter(Collider other)
		{
			if (!GameSetup.IsMpClient && other.CompareTag("enemyCollide"))
			{
				ball.HitEnemy(other.transform);

				ball.Trigger();
			}
		}
	}
}