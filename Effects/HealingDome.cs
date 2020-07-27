using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class HealingDome : MonoBehaviour
	{
		public static Material material;

		public float radius;
		public float healing;
		public bool GrantImmunity;
		public bool regenEnergy;

		public static void CreateHealingDome(Vector3 pos, float radius, float healing, bool grantImmunity, bool regensenergy, float duration)
		{
			if (material == null)
			{
				material = new Material(Shader.Find("Particles/Additive"));
				//{
				//    color = new Color(0, 0.7f, 0, 0.5f)
				//};
				material.SetColor("_TintColor", new Color(0, 0.15f, 0, 0.3f));
			}
			GameObject go = new GameObject();
			go.AddComponent<MeshFilter>().mesh = Res.ResourceLoader.instance.LoadedMeshes[68];
			go.AddComponent<MeshRenderer>().material = material;
			go.transform.localScale = Vector3.one * radius;
			go.transform.position = pos;
			HealingDome d = go.AddComponent<HealingDome>();
			d.radius = radius;
			d.healing = healing;
			d.GrantImmunity = grantImmunity;
			d.regenEnergy = regensenergy;

			var light = go.AddComponent<Light>();
			light.shadowStrength = 1;
			light.shadows = LightShadows.Hard;
			light.type = LightType.Point;
			light.range = 20;
			light.color = new Color(0.2f, 1f, 0.2f);
			light.intensity = 0.6f;
			Destroy(go, duration);
		}

		private float timeShift = 0;

		private void Update()
		{
			timeShift += Time.deltaTime * 2.3f;
			transform.localScale = Vector3.one * (radius + Mathf.Sin(timeShift * 3.14f * 4f));

			if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < radius * radius)
			{
				LocalPlayer.Stats.HealthTarget += healing * Time.deltaTime;
				LocalPlayer.Stats.Health += healing * Time.deltaTime;
				if (GrantImmunity)
				{
					BuffDB.AddBuff(4, 40002, 0, 3f);
					BuffDB.AddBuff(6, 40001, 0, 3f);
				}
				if (regenEnergy)
				{
					LocalPlayer.Stats.Energy += healing * Time.deltaTime / 10;
				}
			}
		}
	}
}