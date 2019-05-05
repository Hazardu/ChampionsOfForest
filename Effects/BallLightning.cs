using ChampionsOfForest;
using System.Collections;
using System.Collections.Generic;
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

    public static void Create(Vector3 position, Vector3 speed, float damage,uint id)
    {
        GameObject o = GameObject.Instantiate(prefab, position, Quaternion.identity);
        o.tag = "enemyCollide";
        BallLightning b = o.AddComponent<BallLightning>();

        DamageMath.DamageClamp(damage, out int dmg, out int rep);
        b.dmg = dmg;
        b.rep = rep;
        b.ID = id;
        b.speed = speed;
    }






    public uint ID;
    public GameObject explosionFX;
    public GameObject mainFX;
    public Rigidbody rb;
    public float dmg;
    public float rep;
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
        trigger.transform.SetParent(gameObject);
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
        if (!BoltNetwork.isRunning) return;
        string msg = "AK";
        Vector3 pos = transform.position;
        msg += ID + ";" + pos.x + ";" + pos.y + ";" + pos.z + ";";
        ChampionsOfForest.Network.NetworkManager.SendLine(msg, ChampionsOfForest.Network.NetworkManager.Target.Others);
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
    //TODO  
    public void OnExplode()
    {
        //deal damage to enemies, apply force to rigidbodies etc

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 28, Vector3.one, 30);

        if (!GameSetup.IsMpClient)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("enemyCollide"))
                {
                    HitEnemy(hit.transform);
                    hit.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);
                }
                else if (!hit.transform.CompareTag("Player") || !hit.transform.CompareTag("structure"))
                {
                    hit.transform.SendMessageUpwards("Hit", dmg, SendMessageOptions.DontRequireReceiver);
                    hit.transform.SendMessage("Hit", dmg, SendMessageOptions.DontRequireReceiver);
                    hit.transform.SendMessageUpwards("Explosion", hit.distance, SendMessageOptions.DontRequireReceiver);
                    hit.transform.SendMessage("Explosion", hit.distance, SendMessageOptions.DontRequireReceiver);
                    //Explosionhit.transform.SendMessage("CutDown", SendMessageOptions.DontRequireReceiver);
                    //hit.transform.SendMessageUpwards("Burn", SendMessageOptions.DontRequireReceiver);

                }
            }

        }

        foreach (RaycastHit hit in hits)
        {
            hit.rigidbody?.AddExplosionForce(500, transform.position, 30,1.2f,ForceMode.Impulse);
        }
    }

    public void HitEnemy(Transform t)
    {
        for (int i = 0; i < rep; i++)
        {
            t.SendMessageUpwards("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
            t.SendMessage("HitMagic", dmg, SendMessageOptions.DontRequireReceiver);
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
        else if (!(collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("PlayerNet")))
        {
            collision.transform.SendMessageUpwards("Hit", dmg, SendMessageOptions.DontRequireReceiver);
            collision.transform.SendMessage("Hit", dmg, SendMessageOptions.DontRequireReceiver);
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
            transform.Translate(speed * Time.deltaTime,Space.World);
            speed.y -= Time.deltaTime * 0.5f;
        }
    }

}

public class BallLightningTrigger : MonoBehaviour
{
    public BallLightning ball;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyCollide")||(other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock")))
        {
            ball.HitEnemy(other.transform);
            
            ball.Trigger();
        }

    }
}
