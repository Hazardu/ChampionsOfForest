using System.Collections;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class DarkBeam : MonoBehaviour
    {



        private static Material material;
        public static void Initialize()
        {
            material = new Material(Shader.Find("Particles/Multiply"));
            material.SetFloat("_InvFade", 1);
            material.renderQueue = 3000;
            material.mainTexture = Res.ResourceLoader.GetTexture(107);
            material.color = new Color(1,1,1,1);
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
                ParticleSystem.MainModule main = ps.main;
                ParticleSystem.EmissionModule emission = ps.emission;
                ParticleSystem.ShapeModule shape = ps.shape;
                ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();
                emission.enabled = true;
                shape.enabled = true;
                rend.enabled = true;

                rend.material = material;
                rend.renderMode = ParticleSystemRenderMode.Stretch;
                rend.velocityScale = 0.12f;
                rend.lengthScale = 0.6f;
                rend.pivot = new Vector3(0, -25, 0);

                main.startLifetime = 1;
                main.startSpeed = -250;
                main.startSize =3;
                main.duration = 1;
                 main.loop = true;
                main.prewarm = false;
                if (fromEnemy)
                {
                    main.startColor = new ParticleSystem.MinMaxGradient(new Color(1, 0.08f, 0,1));
                }
                else
                {
                    main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.76f, 0.0f, 1,1));
                }
                main.maxParticles = 1000;

                emission.rateOverTime =250;
                emission.rateOverTimeMultiplier = 1;

                shape.shapeType = ParticleSystemShapeType.Cone;
                shape.angle = 0;
                shape.radius = Radius;


                particleObj.transform.rotation = Quaternion.Euler(-60f, Random.Range(0, 360), 0);
                particleObj.transform.position = position;

                Light light = lamp.AddComponent<Light>();
                light.type = LightType.Spot;
                light.spotAngle = 40;   //going towards 50
                light.intensity = 0;   //going towards 30
                light.range = 20;
                if (fromEnemy)
                {
                    light.color = new Color(1, 0.0f, 0);
                }
                else
                {
                    light.color = new Color(0.71f, 0.0f, 1);
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
                //system.Pause();
                //system.Play();
                system.Clear();
                EffectReady = false;
                StartCoroutine(AnimatedBeamCoroutine());
            }
            catch (System.Exception e)
            {

                ModAPI.Console.Write(e.ToString());
            }

        }

        private IEnumerator AnimatedBeamCoroutine()
        {
            StartCoroutine(LightIn());
            while (light.intensity < 5)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.4f);

            StartCoroutine(DoEnemyCheck());

            EffectReady = true;


        }

        private IEnumerator LightIn()
        {
            while (light.intensity < 15)
            {

                light.intensity += 1.5f* Time.deltaTime;

                yield return null;
            }
        }
        private IEnumerator LightOut()
        {
            while (light.intensity > 0)
            {

                light.intensity -= 15 * Time.deltaTime;

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
                            int dmg = Mathf.RoundToInt(Damage / 2);
                            if (fromEnemy)
                            {
                                ep._Health.Health = (int)Mathf.Clamp(ep._Health.Health + Healing / 2, 0, ep.MaxHealth);
                                ep.Slow(6, Boost, 25);
                            }
                            else
                            {
                                ep.HitMagic(dmg);
                                ep.Slow(6, Slow, 10);
                            }
                        }
                        else
                        {
                            ModAPI.Console.Write("No enemy progression");
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        float lifetime = 0;
        private void Update()
        {
            Debug.Log(system.isPaused + " " + system.isStopped + " " + system.isPlaying+" "+system.isEmitting+" "+system.particleCount);
            
            //system.transform.position = transform.position;
            ModAPI.Console.Write(system.isPaused + " " + system.isStopped + " " + system.isPlaying+" "+system.isEmitting+" "+system.particleCount);
            if (EffectReady)
            {
                //if (system.isPaused || system.isStopped)
                //{
                //    system.Play();
                //}

                lifetime += Time.deltaTime;
                if (Duration < lifetime)
                {
                    Stop();
                }

                if (!GameSetup.IsMpClient)
                {
                    if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < Radius * Radius)
                    {
                        if (fromEnemy)
                        {
                            LocalPlayer.Stats.HealthChange(Damage * Time.deltaTime * (1 - ModdedPlayer.instance.MagicResistance));
                            Player.BuffDB.AddBuff(1, 5, Slow, 20);
                        }
                        else
                        {
                            LocalPlayer.Stats.Health += Healing * Time.deltaTime;
                            LocalPlayer.Stats.HealthTarget += Healing * 1.5f * Time.deltaTime;
                            Player.BuffDB.AddBuff(5, 6, Boost, 20);

                        }
                    }
                }
            }
        }

        private void Stop()
        {

            StartCoroutine(LightOut());
            system.Stop();
            EffectReady = false;
            Destroy(gameObject, 2);

        }
    }
}
