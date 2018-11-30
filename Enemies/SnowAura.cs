using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Enemies
{
    public class SnowAura : MonoBehaviour
    {
        private readonly float _radius = 20;
        private readonly float _duration = 60;
        public Transform followTarget;

        private static Material _particleMaterial;

        private void Start()
        {
            if (_particleMaterial == null)
            {
                _particleMaterial = new Material(Shader.Find("Particles/Additive"))
                {
                    color = Color.white,
                    mainTexture = Res.ResourceLoader.instance.LoadedTextures[26]
                };
            }

            Destroy(gameObject, _duration);

            //Creating particle effect
            ParticleSystem p = gameObject.AddComponent<ParticleSystem>();
            Renderer r = p.GetComponent<Renderer>();
            r.material = _particleMaterial;
            ParticleSystem.ShapeModule s = p.shape;
            s.arcMode = ParticleSystemShapeMultiModeValue.Random;
            s.arcSpeed = 3.4f;
            s.shapeType = ParticleSystemShapeType.Cone;
            s.radius = _radius;
            ParticleSystem.EmissionModule e = p.emission;
            e.rateOverTime = 100;
            var main = p.main;
            main.startSize = 0.3f;
            main.startSpeed = 0;
            main.gravityModifier = -0.5f;


        }

        private void Update()
        {
            transform.position = followTarget.position;                                         //copies position of the caster
            transform.Rotate(Vector3.up * 720 * Time.deltaTime, Space.World);                   //rotates
            if (Vector3.Distance(LocalPlayer.Transform.position, transform.position) < _radius) //if player is in range, slows him
            {
                BuffDataBase.AddBuff(1, 30, 0.35f, 5);
                BuffDataBase.AddBuff(2, 30, 0.35f, 5);
            }
        }
    }
}
