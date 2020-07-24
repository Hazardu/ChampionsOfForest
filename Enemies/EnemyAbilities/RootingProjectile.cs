using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class RootSpell : MonoBehaviour
	{
		public float Duration;
		public Vector3 targetPos;
		public float LifeTime;

		private static readonly float ChainCount = 7;
		private static GameObject ChainInstance;
		private static GameObject RepulsionEffectInstance;

		public static void Create(Vector3 pos, float rootDuration)
		{
			if (ChainInstance == null)
			{
				ChainInstance = new GameObject();
				ChainInstance.AddComponent<RootSpell>();
				ParticleSystem ps = ChainInstance.AddComponent<ParticleSystem>();
				ParticleSystem.MainModule main = ps.main;
				main.startLifetime = 30;
				main.startSpeed = 0;
				main.startSize = 2.5f;
				main.scalingMode = ParticleSystemScalingMode.Hierarchy;
				main.startRotationX = 90;
				main.startRotationY = 90;
				main.startRotationZ = new ParticleSystem.MinMaxCurve(0, 90);
				main.simulationSpace = ParticleSystemSimulationSpace.World;

				ParticleSystem.EmissionModule e = ps.emission;
				e.rateOverDistance = 4;
				e.rateOverTime = 0;

				ParticleSystem.ShapeModule s = ps.shape;
				s.shapeType = ParticleSystemShapeType.SingleSidedEdge;
				s.alignToDirection = true;

				s.radius = 0.0001f;
				s.length = 0.0001f;

				ParticleSystem.ExternalForcesModule ef = ps.externalForces;
				ef.enabled = true;
				ef.multiplier = 2;

				ParticleSystemRenderer r = ps.GetComponent<ParticleSystemRenderer>();

				var mat = new Material(Shader.Find("Particles/Additive"));
				mat.SetColor("_TintColor", new Color(0.2431373f, 0.2051491f, 0, 0.10f));
				r.material = mat;
				r.renderMode = ParticleSystemRenderMode.Mesh;
				r.mesh = Res.ResourceLoader.instance.LoadedMeshes[69];
				r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
				r.alignment = ParticleSystemRenderSpace.World;
				ChainInstance.SetActive(false);

				RepulsionEffectInstance = new GameObject();
				WindZone wz = RepulsionEffectInstance.AddComponent<WindZone>();
				wz.windMain = -10;
				wz.windTurbulence = 5;
				wz.mode = WindZoneMode.Spherical;
				wz.radius = 15;
				RepulsionEffectInstance.SetActive(false);
			}
			ChainInstance.SetActive(true);
			for (int i = 0; i < ChainCount; i++)
			{
				Vector3 rand = new Vector3(Random.value * 2 - 1, Random.value, Random.value * 2 - 1);
				rand *= 10;
				rand += pos;

				GameObject go = Instantiate(ChainInstance, rand, Quaternion.LookRotation(pos - rand));
				go.transform.LookAt(pos);
				RootSpell spell = go.GetComponent<RootSpell>();
				spell.targetPos = pos;
				spell.Duration = rootDuration;
			}
			ChainInstance.SetActive(false);
		}

		private bool ReachedGoal = false;
		private bool createdPush = false;
		private ParticleSystem psys;

		private void Start()
		{
			psys = GetComponent<ParticleSystem>();
			LifeTime = 0;
			createdPush = false;
			ReachedGoal = false;
			transform.LookAt(targetPos);
			//main.startRotationX = 90+transform.rotation.eulerAngles.x;
			//main.startRotationY = 90+transform.rotation.eulerAngles.y;
			//main.startRotationZ= Random.Range(0,90)+transform.rotation.eulerAngles.z;
		}

		private void Update()
		{
			LifeTime += Time.deltaTime;
			if (LifeTime < Duration)
			{
				if (!ReachedGoal)
				{
					transform.LookAt(targetPos);
					transform.position = Vector3.MoveTowards(transform.position, targetPos, 40 * Time.deltaTime);
					if (transform.position == targetPos)
					{
						ReachedGoal = true;
					}
				}
				else if (!psys.isPaused)
				{
					psys.Pause();
				}
			}
			else
			{
				if (psys.isPaused)
				{
					psys.Play();
				}
				if (!createdPush)
				{
					RepulsionEffectInstance.SetActive(true);
					Destroy(Instantiate(RepulsionEffectInstance, targetPos - Vector3.up * 3, Quaternion.identity), 1f);
					RepulsionEffectInstance.SetActive(false);
					Destroy(gameObject, 2);
					createdPush = true;
				}
			}
		}
	}
}