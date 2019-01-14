using BuilderCore;
using System.Collections;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class EnemyLaser : MonoBehaviour
    {

        public static void CreateLaser(Vector3 pos, Vector3 dir)
        {
            Transform t = new GameObject("Laser").transform;
            t.position = pos;
            t.gameObject.AddComponent<EnemyLaser>().Direction = dir;
        }

        public ParticleSystem particleSystem;
        public static Material mat;
        public static Material mat1;
        private float rotSpeed = 0;
        public Vector3 Direction;

        private IEnumerator DoPreparation()
        {
            float f = 0;
            for (int i = 0; i < 10; i++)
            {

                particleSystem.Emit(new ParticleSystem.EmitParams() { position = transform.forward * Mathf.Sin(f) * 2.5f + transform.right * Mathf.Cos(f) * 2.5f }, 10);

                yield return new WaitForSeconds(0.1f);
                f += Mathf.PI * 0.2f;

            }
            rotSpeed = 10;

            yield return new WaitForSeconds(1f);
            rotSpeed = 30;
            float up = 1;
            for (int a = 0; a < 10; a++)
            {

                float f1 = Mathf.PI * 0.2f * 0.2f * up;
                for (int i = 0; i < 10; i++)
                {
                    particleSystem.Emit(new ParticleSystem.EmitParams() { position = transform.forward * Mathf.Sin(f1) * (2.5f - up / 5f) + transform.right * Mathf.Cos(f1) * (2.5f - up / 5f) + Vector3.up * up, velocity = Vector3.up * up / 10 }, 1);
                    f1 += Mathf.PI * 0.2f;

                }
                yield return new WaitForSeconds(0.1f);
                rotSpeed += 15;
                up++;
            }
            yield return new WaitForSeconds(1f);
            rotSpeed += 40;
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(1f);
            Direction.RotateY(-180f);
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(1f);
            Direction.RotateY(-180f);
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(1f);
            Direction.RotateY(-180f);
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(15f);
            Destroy(gameObject);

        }
        private IEnumerator Shoot()
        {
            GameObject go = new GameObject();
            go.transform.position = transform.position + Vector3.up * 13;
            go.transform.LookAt(Direction);
            int dmg = 200;
            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Hard:
                    dmg = 400;
                    break;
                case ModSettings.Difficulty.Elite:
                    dmg = 1200;
                    break;
                case ModSettings.Difficulty.Master:
                    dmg = 2000;
                    break;
                case ModSettings.Difficulty.Challenge1:
                    dmg = 5000;
                    break;
                case ModSettings.Difficulty.Challenge2:
                    dmg = 20000;
                    break;
                case ModSettings.Difficulty.Challenge3:
                    dmg = 55000;
                    break;
                case ModSettings.Difficulty.Challenge4:
                    dmg = 100000;
                    break;
                case ModSettings.Difficulty.Challenge5:
                    dmg = 250000;
                    break;
            }
            go.AddComponent<EnemyLaserBeam>().Initialize(dmg);
            ParticleSystem ps = go.AddComponent<ParticleSystem>();
            ParticleSystemRenderer rend = go.GetComponent<ParticleSystemRenderer>();
            rend.renderMode = ParticleSystemRenderMode.Billboard;
            rend.trailMaterial = mat1;
            rend.alignment = ParticleSystemRenderSpace.View;
            rend.material = mat1;

            ParticleSystem.ShapeModule shape = ps.shape;
            shape.enabled = false;
            ParticleSystem.MainModule m = ps.main;
            m.simulationSpace = ParticleSystemSimulationSpace.Local;
            m.startSpeed = 100;
            m.startLifetime = 0.5f;
            m.startSize = 0.1f;
            yield return null;

            ParticleSystem.EmissionModule e = ps.emission;
            e.rateOverTime = 10f;
            ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
            sol.enabled = true;
            sol.sizeMultiplier = 3;
            sol.size = new ParticleSystem.MinMaxCurve(4, new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f, 11.34533f, 11.34533f), new Keyframe(0.1365601f, 1f, 0.4060947f, 0.4060947f), new Keyframe(1f, 0.2881376f, -0.8223371f, -0.8223371f) }));
            yield return null;
            ParticleSystem.TrailModule trail = ps.trails;
            trail.enabled = true;
            trail.dieWithParticles = false;
            trail.inheritParticleColor = true;
            trail.ratio = 0.5f;
            trail.textureMode = ParticleSystemTrailTextureMode.Stretch;
            float time = 0;
            while (time < 5)
            {
                time += Time.deltaTime;
                go.transform.Rotate(Vector3.up * Time.deltaTime * 25, Space.World);
                go.transform.Rotate(Vector3.right * Time.deltaTime * 4 * Mathf.Sin(time), Space.Self);

                yield return null;

            }
            Destroy(go);

        }


        private void Start()
        {
            if (mat == null)
            {
                mat = Core.CreateMaterial(new BuildingData() { renderMode = BuildingData.RenderMode.Transparent, EmissionColor = new Color(0, 0.15f, 0.20f), MainColor = new Color(0.2f, 1, 0, 0.5f), Metalic = 0.3f, Smoothness = 0.67f });
                mat1 = new Material(Shader.Find("Unlit/Transparent"))
                {
                    color = new Color(0.203f, 1, 0.629f, 0.2117647f),
                    mainTexture = Res.ResourceLoader.GetTexture(71),
                    renderQueue = 2000,
                    doubleSidedGI = true,

                };
            }

            particleSystem = gameObject.AddComponent<ParticleSystem>();
            ParticleSystemRenderer rend = gameObject.GetComponent<ParticleSystemRenderer>();
            rend.renderMode = ParticleSystemRenderMode.Mesh;
            rend.mesh = Res.ResourceLoader.instance.LoadedMeshes[70];
            rend.alignment = ParticleSystemRenderSpace.World;
            rend.material = mat;
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            ParticleSystem.MainModule m = particleSystem.main;
            m.simulationSpace = ParticleSystemSimulationSpace.Local;

            m.startRotation3D = true;
            m.startSpeed = 0;
            m.startLifetime = 11;
            m.duration = 30;
            m.startSize = 0.3f;
            ParticleSystem.EmissionModule e = particleSystem.emission;
            e.rateOverTime = 0;
            e.enabled = false;
            StartCoroutine(DoPreparation());

        }
        private void Update()
        {

            transform.Rotate(Vector3.up * rotSpeed * 1.4f * Time.deltaTime);
        }
    }
}
