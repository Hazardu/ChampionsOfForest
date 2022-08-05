using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class FireAura : MonoBehaviour
	{
		public static void Cast(GameObject go, float dmg)
		{
			var aura = go.GetComponent<FireAura>();
			if (aura == null)
			{
				aura = go.AddComponent<FireAura>();
			}
			aura.damage = dmg;
			aura.Enable();
		}

		private bool isOn = false;
		private float damage;
		Light light;
		void Start()
		{
			light = gameObject.AddComponent<Light>();
			light.color = new Color(0.96f,0.62f,0.0f);
			light.range = 7f;
			light.intensity = 1.5f;
			light.shadows = LightShadows.Soft;
			light.type = LightType.Point;

		}
		private void OnDisable()
		{
			Disable();
		}
		void OnEnable()
		{
			if (light != null)
				Disable();
		}

		public void Enable()
		{
			isOn = true;
			light.enabled = true;
			this.enabled = true;
		}
		public void Disable()
		{
			isOn = false;

			light.enabled = false;
			this.enabled = false;
		}

		private void Update()
		{
			if (isOn)
			{
				if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < 49)
				{
					float dmgPerTick = Time.deltaTime * damage * ModdedPlayer.Stats.allDamageTaken * ModdedPlayer.Stats.magicDamageTaken * ModReferences.DamageReduction((int)ModdedPlayer.Stats.TotalArmor);

					if (LocalPlayer.Stats.Health - 1 > dmgPerTick)
						LocalPlayer.Stats.Health -= dmgPerTick;

					BuffManager.GiveBuff(10, 72, 0.7f, 5);
					BuffManager.GiveBuff(21, 73, Time.deltaTime * damage / 30, 15);
				}
			}
			else
			{
				Disable();
			}
		}
	}
}