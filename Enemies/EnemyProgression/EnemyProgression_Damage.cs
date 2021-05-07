using System;
using System.Collections.Generic;
using System.Linq;

using Bolt;

using ChampionsOfForest.Enemies.EnemyAbilities;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
	public partial class EnemyProgression : MonoBehaviour
	{
		public void HitMagic(float damage)
		{
			damage = ClampDamage(false, damage, true);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, damage, new Color(0f, 0, 1f, 0.8f));

			damage -= 1f;
			DealDamage(damage);
			if (abilities.Contains(Abilities.Juggernaut))
				if (HealthScript.Health > 10)
					return;
			HealthScript.HitReal(1);
		}
		private void DealDamage(float damage)
		{
			if (extraHealth > 0)
			{
				float i = Mathf.Min(extraHealth, damage);
				extraHealth -= i;
				damage -= i;
			}
			if (damage > 0)
			{
				if (damage > HealthScript.Health)

					HealthScript.Health = 0;

				else
					HealthScript.Health -= (int)damage;
			}
		}
		public void HitPhysicalSilent(float damage)
		{
			damage = ClampDamage(false, damage, true);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, damage, new Color(0.8f, 1, 0.10f, 0.8f));

			if (extraHealth > 0)
			{
				float i = Mathf.Min(extraHealth, damage);
				extraHealth -= i;
				damage -= i;
			}
			if (damage > 0)
			{
				if (damage > HealthScript.Health)
					HealthScript.Health = 0;
				else
					HealthScript.Health -= (int)damage;
				if (HealthScript.Health < 1)
					HealthScript.HitReal(25);
			}
		}

		public float ClampDamage(bool pure, float damage, bool magic)
		{
			if (abilities.Contains(Abilities.Shielding))
			{
				if (shieldingON > 0)
				{
					return 0;
				}
				else if (shieldingCD <= 0)
				{
					switch (ModSettings.difficulty)
					{
						case ModSettings.Difficulty.Easy:
							shieldingCD = 60;
							break;

						case ModSettings.Difficulty.Veteran:
							shieldingCD = 55;

							break;

						case ModSettings.Difficulty.Elite:
							shieldingCD = 50;

							break;

						case ModSettings.Difficulty.Master:
							shieldingCD = 45;

							break;

						case ModSettings.Difficulty.Challenge1:
							shieldingCD = 40;

							break;

						case ModSettings.Difficulty.Challenge2:
							shieldingCD = 35;

							break;

						case ModSettings.Difficulty.Challenge3:
							shieldingCD = 30;

							break;

						case ModSettings.Difficulty.Challenge4:
							shieldingCD = 25;
							break;

						case ModSettings.Difficulty.Challenge5:
							shieldingCD = 20;
							break;

						default:
							shieldingCD = 15;
							break;
					}
					normalColor = HealthScript.MySkin.material.color;
					HealthScript.MySkin.material.color = Color.black;
					shieldingON = 3;
					return 0f;
				}
			}

			damage = damage * dmgTakenIncrease;
			if (pure)
			{
				return Mathf.Abs(damage);
			}

			float reduction = ModReferences.DamageReduction(Armor - ArmorReduction);
			if (magic)
			{
				reduction /= 1.5f;
			}

			float dmg = damage * (1 - reduction);
			if (Steadfast != 100)
			{
				dmg = Mathf.Min(dmg, steadfastCap);
			}

			return Mathf.Abs(dmg);
		}
		public void HitPure(float damage)
		{
			float dmg = ClampDamage(true, damage, false);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, dmg, new Color(1, 1f, 1f, 1f));
			damage -= 1f;
			DealDamage(dmg);
			HealthScript.HitReal(1);
		}
		public void HitPhysical(float damage)
		{
			float dmg = ClampDamage(false, damage, false);
			Network.NetworkManager.SendHitmarker(transform.position + Vector3.up, dmg, new Color(1, 0.4f, 0, 1f));
			damage -= 1f;
			DealDamage(dmg);
			if (abilities.Contains(Abilities.Juggernaut))
				if (HealthScript.Health > 10)
					return;
			HealthScript.HitReal(1);
		}
		public int ClampDamage(bool pure, int damage)
		{
			if (abilities.Contains(Abilities.Shielding))
			{
				if (shieldingON > 0)
				{
					return 0;
				}
				else if (shieldingCD <= 0)
				{
					switch (ModSettings.difficulty)
					{
						case ModSettings.Difficulty.Easy:
							shieldingCD = 60;
							break;

						case ModSettings.Difficulty.Veteran:
							shieldingCD = 55;

							break;

						case ModSettings.Difficulty.Elite:
							shieldingCD = 50;

							break;

						case ModSettings.Difficulty.Master:
							shieldingCD = 45;

							break;

						case ModSettings.Difficulty.Challenge1:
							shieldingCD = 40;

							break;

						case ModSettings.Difficulty.Challenge2:
							shieldingCD = 35;

							break;

						case ModSettings.Difficulty.Challenge3:
							shieldingCD = 30;

							break;

						case ModSettings.Difficulty.Challenge4:
							shieldingCD = 25;
							break;

						case ModSettings.Difficulty.Challenge5:
							shieldingCD = 20;
							break;

						default:
							shieldingCD = 15;
							break;
					}
					normalColor = HealthScript.MySkin.material.color;
					HealthScript.MySkin.material.color = Color.black;
					shieldingON = 3;
					return 0;
				}
			}
			float dmgF = damage * dmgTakenIncrease;
			damage =  Mathf.CeilToInt(Mathf.Min(dmgF,int.MaxValue));
			if (pure)
			{
				if (damage > steadfastCap)
				{
					if (Steadfast == 100)
					{
						return damage;
					}

					if (damage > steadfastCap)
					{
						if (steadfastCap >= int.MaxValue)
							return int.MaxValue;
						return (int)steadfastCap;
					}
				}
				return damage;
			}

			float reduction = ModReferences.DamageReduction(Mathf.Clamp(Armor - ArmorReduction, 0, int.MaxValue));
			int dmg = Mathf.CeilToInt(damage * (1 - reduction));
			if (Steadfast != 100)
			{
				dmg = Mathf.Min(dmg, (int)steadfastCap);
			}

			return dmg;
		}

	}
}
