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

		private void OnDisable()
		{
			isOn = false;
		}

		public void TurnOn()
		{
			isOn = true;
		}

		private void Update()
		{
			if (isOn)
			{
				if ((LocalPlayer.Transform.position - transform.position).sqrMagnitude < 49)
				{
					var dmgPerTick =  Time.deltaTime * damage * ModdedPlayer.Stats.allDamageTaken * ModdedPlayer.Stats.magicDamageTaken;
					if (LocalPlayer.Stats.Health - 1 > dmgPerTick)
						LocalPlayer.Stats.Health -= dmgPerTick;
					Player.BuffDB.AddBuff(10, 72, 0.7f, 5);
					Player.BuffDB.AddBuff(21, 73, Time.deltaTime * damage / 30, 15);
				}
			}
		}
	}
}