using BuilderCore;
using System.Collections;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public class BlackHole : MonoBehaviour
    {
        public static void CreateBlackHole(Vector3 pos, bool fromEnemy, float damage, float duration = 3, float radius = 20, float pullforce = 12)
        {
            GameObject go = Core.Instantiate(401, pos);
            BlackHole b = go.AddComponent<BlackHole>();
            b.damage = damage;
            b.FromEnemy = fromEnemy;
            b.duration = duration;
            b.radius = radius;
            b.pullForce = pullforce;
        }


        public bool FromEnemy;
        public float pullForce;
        public float damage;
        public float duration;
        public float radius;
        public static float rotationSpeed = 15f;
        private float scale;
        public SphereCollider col;
        private List<Transform> CoughtEnemies;
        public static Material particleMaterial;
        private ParticleSystem sys;
        private float lifetime;
        private GameObject particleGO;

        public bool startDone = false;

        private IEnumerator Start()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = Res.ResourceLoader.instance.LoadedAudio[1000];
            source.volume = 16;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.maxDistance = 100;
            source.Play();
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
                yield return new WaitForSeconds(1.5f);
            }
            Destroy(gameObject, duration);
            if (!FromEnemy && !GameSetup.IsMpClient)
            {
                CoughtEnemies = new List<Transform>();

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
            particleGO = new GameObject("BlackHoleParticles");
            particleGO.transform.position = transform.position;
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
            if (!startDone) return;
            lifetime += Time.deltaTime;



            scale = Mathf.Clamp(scale + Time.deltaTime * radius * 1.5f / duration / 2, 0, radius / 5);
            transform.localScale = Vector3.one * scale / 3;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, scale * 5, Vector3.one, scale * 5);
            foreach (RaycastHit hit in hits)
            {
                Rigidbody rb = hit.rigidbody;
                if (rb != null)
                {
                    if (!(!FromEnemy && rb == LocalPlayer.Rigidbody))
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
                        if (!CoughtEnemies.Contains(hit.transform.root))
                        {
                            if (hit.transform.root.GetComponentInChildren<EnemyHealth>() != null)
                            {
                                CoughtEnemies.Add(hit.transform.root);
                            }
                        }
                    }
                }
            }

            transform.Translate(Vector3.up * 0.55f * Time.deltaTime * (Mathf.Pow(duration - (lifetime), 2)) / duration);
            transform.Rotate(Vector3.up * rotationSpeed);
            if (FromEnemy)
            {
                if (Vector3.Distance(LocalPlayer.Transform.position, transform.position) < scale * 5)
                {
                    if(ModdedPlayer.instance.StunImmune==0&&ModdedPlayer.instance.DebuffImmune==0)
                    Pull(LocalPlayer.Transform);
                    LocalPlayer.Stats.HealthChange(-damage * Time.deltaTime * ModdedPlayer.instance.DamageReductionTotal * (1 - ModdedPlayer.instance.MagicResistance));
                }
            }
            else if (!GameSetup.IsMpClient)
            {
                foreach (Transform t in CoughtEnemies)
                {
                    EnemyProgression p = t.GetComponentInChildren<EnemyProgression>();
                    if (!p.CCimmune)
                    {
                        Pull(t);
                    }
                }
            }
        }

        private IEnumerator HitEverySecond()
        {
            while (true)
            {


                foreach (Transform t in CoughtEnemies)
                {
                    EnemyProgression ep = t.GetComponentInChildren<EnemyProgression>();
                    if (ep != null)
                    {
                        DamageMath.DamageClamp(damage, out int d, out int a);
                        for (int i = 0; i < a; i++)
                        {
                            ep.HitMagic(Mathf.RoundToInt(d / 2));
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Pull(Transform t)
        {
            t.position += Vector3.Normalize(t.position - transform.position) * pullForce * -Time.deltaTime;
        }

    }
}


