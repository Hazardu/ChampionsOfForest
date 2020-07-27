using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Effects
{
	public class WarCry
	{
		public static Material particleMaterial;

		public static void Cast(Vector3 pos, float radius, float speed, float damage, bool GiveDamage, bool GiveArmor = false, int ArmorAmount = 1)
		{
			if (ModSettings.IsDedicated)
			{
				return;
			}
			if ((LocalPlayer.Transform.position - pos).sqrMagnitude < radius * radius)
			{
				GiveEffect(speed, damage, GiveDamage, GiveArmor, ArmorAmount);
			}

			SpawnEffect(pos, radius);
		}

		public static void GiveEffect(float speed, float damage, bool giveEffect2, bool giveEffect3, int ArmorAmount = 1)
		{
			BuffDB.AddBuff(5, 45, speed, 120);
			BuffDB.AddBuff(14, 46, speed, 120);
			if (giveEffect2)
			{
				BuffDB.AddBuff(9, 47, damage, 120);
			}
			if (giveEffect3)
			{
				BuffDB.AddBuff(15, 48, ArmorAmount, 120);
			}
		}

		public static void SpawnEffect(Vector3 pos, float radius)
		{
			GameObject o = new GameObject("__SHOUTPARTICLES__");

			o.transform.position = pos + Vector3.down;
			o.transform.rotation = Quaternion.Euler(90, 0, 0);
			o.transform.localScale = Vector3.one * radius / 50;

			GameObject.Destroy(o, 1);
			ParticleSystem ps = o.AddComponent<ParticleSystem>();
			ParticleSystem.MainModule main = ps.main;
			ParticleSystem.EmissionModule emission = ps.emission;
			ParticleSystem.ShapeModule shape = ps.shape;
			ParticleSystem.LimitVelocityOverLifetimeModule limit = ps.limitVelocityOverLifetime;
			ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();

			main.loop = false;
			main.duration = 0.5f;
			main.startLifetime = 0.5f;
			main.startSpeed = 200;
			main.startSize = 4;
			main.startColor = new Color(0.8f, 0.255f, 0.0545f, 0.49f);
			main.gravityModifier = -2;

			emission.rateOverTime = 2000;

			shape.shapeType = ParticleSystemShapeType.Circle;

			limit.dampen = 0.2f;
			limit.limit = 1;

			if (particleMaterial == null)
			{
				particleMaterial = new Material(Shader.Find("Particles/Additive"))
				{
					mainTexture = Res.ResourceLoader.GetTexture(111),
					mainTextureScale = new Vector2(30, 1)
				};

				particleMaterial.SetColor("_TintColor", new Color(0.7f, 0.5f, 0f, 0.6f));
			}
			rend.material = particleMaterial;
			rend.renderMode = ParticleSystemRenderMode.Stretch;
			rend.lengthScale = 2.5f;
		}
	}
}