using ChampionsOfForest.Player;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class SnowAura : MonoBehaviour
    {
        private readonly float _radius = 20;
        private readonly float _duration = 25;
        public Transform followTarget;

        private static Material _particleMaterial;

        private void Start()
        {
            if (_particleMaterial == null)
            {
                _particleMaterial = new Material(Shader.Find("Particles/Additive"))
                {
                    mainTexture = Res.ResourceLoader.instance.LoadedTextures[26]
                };
            }

            Destroy(gameObject, _duration);

            //Creating particle effect
            ParticleSystem p = gameObject.AddComponent<ParticleSystem>();
            p.transform.Rotate(Vector3.right * 90);
            Renderer r = p.GetComponent<Renderer>();
            r.material = _particleMaterial;
            ParticleSystem.ShapeModule s = p.shape;
            s.shapeType = ParticleSystemShapeType.Circle;
           
            s.radius = _radius;
            ParticleSystem.EmissionModule e = p.emission;
            e.rateOverTime = 350;
            var main = p.main;
            main.startSize = 0.15f;
            main.startSpeed = 0;
            main.gravityModifier = -0.6f;
            main.prewarm = false;
            main.startLifetime = 2;
            var vel = p.velocityOverLifetime;
            vel.enabled = true;
            vel.space = ParticleSystemSimulationSpace.World;
            vel.y = new ParticleSystem.MinMaxCurve(3,0);
            var siz = p.sizeOverLifetime;
            siz.size = new ParticleSystem.MinMaxCurve(2, 0);
            armorReduction = Mathf.Pow((int)ModSettings.difficulty, 5);
        }


        private float armorReduction;


        private void Update()
        {
            transform.position = followTarget.position;                                         //copies position of the caster
            transform.Rotate(Vector3.up * 720 * Time.deltaTime, Space.World);                   //rotates
            if ((LocalPlayer.Transform.position- transform.position).sqrMagnitude < _radius* _radius) //if player is in range, slows him
            {
                BuffDB.AddBuff(1, 30, 0.6f, 5);
                BuffDB.AddBuff(2, 31, 0.3f, 5);
                BuffDB.AddBuff(21, 70, armorReduction*Time.deltaTime, 20);
            }
        }
    }
}
