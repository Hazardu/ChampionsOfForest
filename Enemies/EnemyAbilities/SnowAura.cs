using System.Collections;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class SnowAura : MonoBehaviour
	{
		private readonly float _radius = 20;
		private readonly float _duration = 15;
		private AudioSource src;
		public Transform followTarget;

		private static Material _particleMaterial;
		public static Material ParticleMaterial
		{
			get
			{
				if (_particleMaterial == null)
				{
					_particleMaterial = new Material(Shader.Find("Particles/Additive"))
					{
						mainTexture = Res.ResourceLoader.instance.LoadedTextures[26]
					};
				}
				return _particleMaterial;
			}
		}
		private void Start()
		{

			//Creating particle effect
			ParticleSystem p = gameObject.AddComponent<ParticleSystem>();
			p.transform.Rotate(Vector3.right * 120);
			Renderer r = p.GetComponent<Renderer>();
			r.material = ParticleMaterial;
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
			vel.y = new ParticleSystem.MinMaxCurve(3, 0);
			var siz = p.sizeOverLifetime;
			siz.size = new ParticleSystem.MinMaxCurve(2, 0);
			armorReduction = Mathf.Pow((int)ModSettings.difficulty, 5);

			src = gameObject.AddComponent<AudioSource>();
			src.clip = Res.ResourceLoader.instance.LoadedAudio[1015];
			src.loop = true;
			src.volume = 0.0f;
			src.Play();
			StartCoroutine(Lifetime());
		}

		private float armorReduction;

		private IEnumerator Lifetime()
		{
			float time = 0;
			while (time < 1)
			{
				time += Time.deltaTime;
				src.volume = time;
				yield return null;
			}
			yield return new WaitForSeconds(_duration);
			while (time > 0)
			{
				time -= Time.deltaTime;
				src.volume = time;
				yield return null;
			}
			Destroy(gameObject, _duration);
		}

		private void Update()
		{
			transform.position = followTarget.position;                                         //copies position of the caster
			transform.Rotate(Vector3.up * 720 * Time.deltaTime, Space.World);                   //rotates
			if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < _radius * _radius) //if player is in range, slows him
			{
				if (ModdedPlayer.Stats.perk_blizzardSlowReduced)
				{
					BuffDB.AddBuff(1, 30, 0.8f, 1);
					BuffDB.AddBuff(2, 31, 0.9f, 1);
					BuffDB.AddBuff(21, 70, armorReduction * Time.deltaTime / 2, 10);
				}
				else
				{
					BuffDB.AddBuff(1, 30, 0.5f, 6);
					BuffDB.AddBuff(2, 31, 0.25f, 6);
					BuffDB.AddBuff(21, 70, armorReduction * Time.deltaTime, 20);
				}
			}
		}
	}
}