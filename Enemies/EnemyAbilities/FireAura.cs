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
					LocalPlayer.Stats.Health -= Time.deltaTime * damage * ModdedPlayer.Stats.allDamageTakenTotal * (1 - ModdedPlayer.instance.ArmorDmgRed) * (1 - ModdedPlayer.Stats.magicDamageTaken);
					Player.BuffDB.AddBuff(10, 72, 0.7f, 5);
					Player.BuffDB.AddBuff(21, 73, Time.deltaTime * damage / 10, 40);
				}
			}
		}
	}
}