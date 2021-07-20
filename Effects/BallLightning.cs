using System.Collections;
using System.Collections.Generic;

using ChampionsOfForest;

using TheForest.Utils;

using UnityEngine;

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
			ModAPI.Log.Write("Couldnt load asset bundle");
			return;
		}

		prefab = bundle.LoadAsset<GameObject>(_name);
	}

	public static void Create(Vector3 position, Vector3 speed, float damage, uint id)
	{
		GameObject o = GameObject.Instantiate(prefab, position, Quaternion.identity);
		o.tag = "enemyCollide";
		BallLightning b = o.AddComponent<BallLightning>();
		b.dmg = damage;
		b.ID = id;
		b.speed = speed*2;
		list.Add(id, b);
	}

	public uint ID;
	public GameObject explosionFX;
	public GameObject mainFX;
	public Rigidbody rb;
	public float dmg;
	public Vector3 speed;
	private bool _triggered;

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
		var trigger = new GameObject();
		trigger.transform.position = transform.position;
		trigger.transform.parent = transform;
		var collider = trigger.AddComponent<SphereCollider>();
		collider.radius = 3;
		collider.isTrigger = true;
		var balltrigger = trigger.AddComponent<BallLightningTrigger>();
		balltrigger.ball = this;

		Invoke("Trigger", 30);
	}

	public void Explode()
	{
		StartCoroutine(ExplodeCoroutine());
	}

	public void Trigger()
	{
		if (!_triggered)
		{
			SyncTriggerPosition();
			rb.isKinematic = true;
			_triggered = true;
			Explode();
			list.Remove(ID);
		}
	}

	private void SyncTriggerPosition()
	{
		if (!BoltNetwork.isRunning)
			return;
		Vector3 pos = transform.position;
		using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
		{
			using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
			{
				w.Write(31);
				w.Write(ID);
				w.Write(pos.x);
				w.Write(pos.y);
				w.Write(pos.z);
				w.Close();
			}
			ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Others);
			answerStream.Close();
		}
	}

	public void CoopTrigger(Vector3 newPos)
	{
		if (!_triggered)
		{
			transform.position = newPos;
			_triggered = true;
			rb.isKinematic = true;
			Explode();
			list.Remove(ID);
		}
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

		if (!GameSetup.IsMpClient)
		{
			foreach (RaycastHit hit in hits)
			{

				hit.rigidbody?.AddExplosionForce(500, transform.position, 30, 1.2f, ForceMode.Impulse);
				if (hit.transform.CompareTag("enemyCollide"))
				{
					HitEnemy(hit.transform);
					hit.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

					yield return null;
				}
				else if (!hit.transform.CompareTag("Player") && hit.transform.root != LocalPlayer.Transform)
				{
					hit.transform.SendMessageUpwards("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
					hit.transform.SendMessage("Hit", (int)dmg, SendMessageOptions.DontRequireReceiver);
					if ((hit.transform.position - transform.position).sqrMagnitude < 36)
					{
						hit.transform.SendMessageUpwards("Explosion", (int)hit.distance, SendMessageOptions.DontRequireReceiver);
						hit.transform.SendMessage("Explosion", (int)hit.distance, SendMessageOptions.DontRequireReceiver);
					}
					hit.transform.SendMessage("CutDown", SendMessageOptions.DontRequireReceiver);
					hit.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

					yield return null;
				}
			}
		}
	}

	public void HitEnemy(Transform t)
	{
		t.root.SendMessage("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
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
			if (normal == null)
			{
				return;
			}

			Vector3 newSpeed = Vector3.Reflect(speed, normal);
			speed = newSpeed * 1.02f;
		}
	}

	private void Update()
	{
		if (!_triggered)
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