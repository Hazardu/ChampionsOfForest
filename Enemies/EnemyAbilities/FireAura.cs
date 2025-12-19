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
			aura.isOn = true;
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
			isOn = false;
			light.SetActiveSelfSafe(false);
		}

		public void TurnOn()
		{
			isOn = true;
			this.enabled = true;
			light.enabled = true;
		}

		private void Update()
		{
			if (isOn)
			{
				if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < 49)
				{
					float dmgPerTick = Time.deltaTime * damage * ModdedPlayer.Stats.allDamageTaken * ModdedPlayer.Stats.damageFromElites * ModReferences.DamageReduction((int)ModdedPlayer.Stats.TotalArmor);

					if (LocalPlayer.Stats.Health - 1 > dmgPerTick)
						LocalPlayer.Stats.Health -= dmgPerTick;

					BuffDB.AddBuff(10, 72, 0.7f, 5);
					BuffDB.AddBuff(21, 73, Time.deltaTime * damage / 30, 15);
				}
			}
			else
			{
				this.enabled = false;
				light.enabled = false;
			}
		}
	}
}