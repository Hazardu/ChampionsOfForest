using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Items
{
	internal class StatActions
	{
		public static void AddVitality(float f)
		{
			ModdedPlayer.instance.vitality .Add(Mathf.RoundToInt(f));
		}

		public static void RemoveVitality(float f)
		{
			ModdedPlayer.instance.vitality.Add(-Mathf.RoundToInt(f));
		}

		public static void AddStrength(float f)
		{
			ModdedPlayer.instance.strength.Add( Mathf.RoundToInt(f);
		}

		public static void RemoveStrength(float f)
		{
			ModdedPlayer.instance.strength -= Mathf.RoundToInt(f);
		}

		public static void AddAgility(float f)
		{
			ModdedPlayer.instance.agility += Mathf.RoundToInt(f);
		}

		public static void RemoveAgility(float f)
		{
			ModdedPlayer.instance.agility -= Mathf.RoundToInt(f);
		}

		public static void AddIntelligence(float f)
		{
			ModdedPlayer.instance.intelligence += Mathf.RoundToInt(f);
		}

		public static void RemoveIntelligence(float f)
		{
			ModdedPlayer.instance.intelligence -= Mathf.RoundToInt(f);
		}

		public static void AddHealth(float f)
		{
			ModdedPlayer.instance.HealthBonus += Mathf.RoundToInt(f);
		}

		public static void RemoveHealth(float f)
		{
			ModdedPlayer.instance.HealthBonus -= Mathf.RoundToInt(f);
		}

		public static void AddEnergy(float f)
		{
			ModdedPlayer.instance.EnergyBonus += Mathf.RoundToInt(f);
		}

		public static void RemoveEnergy(float f)
		{
			ModdedPlayer.instance.EnergyBonus -= Mathf.RoundToInt(f);
		}

		public static void AddHPRegen(float f)
		{
			ModdedPlayer.instance.LifeRegen += f;
		}

		public static void RemoveHPRegen(float f)
		{
			ModdedPlayer.instance.LifeRegen -= f;
		}

		public static void AddStaminaRegen(float f)
		{
			ModdedPlayer.instance.StaminaRegen += f;
		}

		public static void RemoveStaminaRegen(float f)
		{
			ModdedPlayer.instance.StaminaRegen -= f;
		}

		public static void AddEnergyRegen(float f)
		{
			ModdedPlayer.instance.EnergyPerSecond += f;
		}

		public static void RemoveEnergyRegen(float f)
		{
			ModdedPlayer.instance.EnergyPerSecond -= f;
		}

		public static void AddStaminaRegenPercent(float f)
		{
			ModdedPlayer.instance.StaminaRegenPercent += f;
		}

		public static void RemoveStaminaRegenPercent(float f)
		{
			ModdedPlayer.instance.StaminaRegenPercent -= f;
		}

		public static void AddHealthRegenPercent(float f)
		{
			ModdedPlayer.instance.HealthRegenPercent += f;
		}

		public static void RemoveHealthRegenPercent(float f)
		{
			ModdedPlayer.instance.HealthRegenPercent -= f;
		}

		public static void AddDamageReduction(float f)
		{
			ModdedPlayer.instance.DamageReduction *= 1 - f;
		}

		public static void RemoveDamageReduction(float f)
		{
			ModdedPlayer.instance.DamageReduction /= 1 - f;
		}

		public static void AddCritChance(float f)
		{
			ModdedPlayer.instance.CritChance += f;
		}

		public static void RemoveCritChance(float f)
		{
			ModdedPlayer.instance.CritChance -= f;
		}

		public static void AddCritDamage(float f)
		{
			ModdedPlayer.instance.CritDamage += f * 100;
		}

		public static void RemoveCritDamage(float f)
		{
			ModdedPlayer.instance.CritDamage -= f * 100;
		}

		public static void AddLifeOnHit(float f)
		{
			ModdedPlayer.instance.LifeOnHit += f;
		}

		public static void RemoveLifeOnHit(float f)
		{
			ModdedPlayer.instance.LifeOnHit -= f;
		}

		public static void AddDodgeChance(float f)
		{
			ItemDataBase.AddPercentage(ref ModdedPlayer.instance.DodgeChance, f);
		}

		public static void RemoveDodgeChance(float f)
		{
			ItemDataBase.RemovePercentage(ref ModdedPlayer.instance.DodgeChance, f);
		}

		public static void AddArmor(float f)
		{
			ModdedPlayer.instance.Armor += Mathf.RoundToInt(f);
		}

		public static void RemoveArmor(float f)
		{
			ModdedPlayer.instance.Armor -= Mathf.RoundToInt(f);
		}

		public static void AddMagicResistance(float f)
		{
			ItemDataBase.AddPercentage(ref ModdedPlayer.instance.MagicResistance, f);
		}

		public static void RemoveMagicResistance(float f)
		{
			ItemDataBase.RemovePercentage(ref ModdedPlayer.instance.MagicResistance, f);
		}

		public static void AddAttackSpeed(float f)
		{
			ModdedPlayer.instance.AttackSpeedAdd += f;
		}

		public static void RemoveAttackSpeed(float f)
		{
			ModdedPlayer.instance.AttackSpeedAdd -= f;
		}

		public static void AddExpFactor(float f)
		{
			ModdedPlayer.instance.ExpFactor *= 1 + f;
		}

		public static void RemoveExpFactor(float f)
		{
			ModdedPlayer.instance.ExpFactor /= 1 + f;
		}

		public static void AddMaxMassacreTime(float f)
		{
			ModdedPlayer.instance.MaxMassacreTime += f;
		}

		public static void RemoveMaxMassacreTime(float f)
		{
			ModdedPlayer.instance.MaxMassacreTime -= f;
		}

		public static void AddSpellDamageAmplifier(float f)
		{
			ModdedPlayer.instance.SpellDamageAmplifier_Add += f;
		}

		public static void RemoveSpellDamageAmplifier(float f)
		{
			ModdedPlayer.instance.SpellDamageAmplifier_Add -= f;
		}

		public static void AddMeleeDamageAmplifier(float f)
		{
			ModdedPlayer.instance.MeleeDamageAmplifier_Add += f;
		}

		public static void RemoveMeleeDamageAmplifier(float f)
		{
			ModdedPlayer.instance.MeleeDamageAmplifier_Add -= f;
		}

		public static void AddRangedDamageAmplifier(float f)
		{
			ModdedPlayer.instance.RangedDamageAmplifier_Add += f;
		}

		public static void RemoveRangedDamageAmplifier(float f)
		{
			ModdedPlayer.instance.RangedDamageAmplifier_Add -= f;
		}

		public static void AddSpellDamageBonus(float f)
		{
			ModdedPlayer.instance.SpellDamageBonus += f;
		}

		public static void RemoveSpellDamageBonus(float f)
		{
			ModdedPlayer.instance.SpellDamageBonus -= f;
		}

		public static void AddMeleeDamageBonus(float f)
		{
			ModdedPlayer.instance.MeleeDamageBonus += f;
		}

		public static void RemoveMeleeDamageBonus(float f)
		{
			ModdedPlayer.instance.MeleeDamageBonus -= f;
		}

		public static void AddRangedDamageBonus(float f)
		{
			ModdedPlayer.instance.RangedDamageBonus += f;
		}

		public static void RemoveRangedDamageBonus(float f)
		{
			ModdedPlayer.instance.RangedDamageBonus -= f;
		}

		public static void AddEnergyPerAgility(float f)
		{
			ModdedPlayer.instance.EnergyPerAgility += f;
		}

		public static void RemoveEnergyPerAgility(float f)
		{
			ModdedPlayer.instance.EnergyPerAgility -= f;
		}

		public static void AddHealthPerVitality(float f)
		{
			ModdedPlayer.instance.HealthPerVitality += f;
		}

		public static void RemoveHealthPerVitality(float f)
		{
			ModdedPlayer.instance.HealthPerVitality -= f;
		}

		public static void AddSpellDamageperInt(float f)
		{
			ModdedPlayer.instance.SpellDamageperInt += f;
		}

		public static void RemoveSpellDamageperInt(float f)
		{
			ModdedPlayer.instance.SpellDamageperInt -= f;
		}

		public static void AddDamagePerStrength(float f)
		{
			ModdedPlayer.instance.DamagePerStrength += f;
		}

		public static void RemoveDamagePerStrength(float f)
		{
			ModdedPlayer.instance.DamagePerStrength -= f;
		}

		public static void AddHealingMultipier(float f)
		{
			ModdedPlayer.instance.HealingMultipier *= 1 + f;
		}

		public static void RemoveHealingMultipier(float f)
		{
			ModdedPlayer.instance.HealingMultipier /= 1 + f;
		}

		public static void AddMoveSpeed(float f)
		{
			ModdedPlayer.instance.MoveSpeed += f;
		}

		public static void RemoveMoveSpeed(float f)
		{
			ModdedPlayer.instance.MoveSpeed -= f;
		}

		public static void AddJump(float f)
		{
			ModdedPlayer.instance.JumpPower *= 1 + f;
		}

		public static void RemoveJump(float f)
		{
			ModdedPlayer.instance.JumpPower /= 1 + f;
		}

		//   public static void Add(float f)
		//{
		//    ModdedPlayer.instance. += f;
		//}
		//public static void Remove(float f)
		//{
		//    ModdedPlayer.instance. -= f;
		//}

		public static void PERMANENT_perkPointIncrease(float f)
		{
			ModdedPlayer.instance.PermanentBonusPerkPoints += Mathf.RoundToInt(f);
			ModdedPlayer.instance.MutationPoints += Mathf.RoundToInt(f);
		}

		public static void PERMANENT_expIncrease(float f)
		{
			ModdedPlayer.instance.AddFinalExperience((long)f);
		}

		public static void AddMagicFind(float f)
		{
			ModdedPlayer.instance.MagicFindMultipier += f;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(23);
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
				}
			}
		}

		public static void RemoveMagicFind(float f)
		{
			ModdedPlayer.instance.MagicFindMultipier -= f;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(23);
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
				}
			}
		}

		public static void AddAllStats(float f)
		{
			ModdedPlayer.instance.strength.Add(Mathf.RoundToInt(f));
			ModdedPlayer.instance.vitality.Add(Mathf.RoundToInt(f));
			ModdedPlayer.instance.agility.Add(Mathf.RoundToInt(f));
			ModdedPlayer.instance.intelligence.Add(Mathf.RoundToInt(f));
		}

		public static void RemoveAllStats(float f)
		{
			ModdedPlayer.instance.strength.Add(-Mathf.RoundToInt(f));
			ModdedPlayer.instance.vitality.Add(-Mathf.RoundToInt(f));
			ModdedPlayer.instance.agility.Add(-Mathf.RoundToInt(f));
			ModdedPlayer.instance.intelligence.Add(-Mathf.RoundToInt(f));
		}

		public static int GetMaxSocketAmountOnItem(in BaseItem.ItemType type)
		{
			switch (type)
			{
				case BaseItem.ItemType.Other:
				case BaseItem.ItemType.Material:
					return 0;

				case BaseItem.ItemType.ShoulderArmor:
				case BaseItem.ItemType.Weapon:
				case BaseItem.ItemType.Helmet:
				case BaseItem.ItemType.Glove:
				case BaseItem.ItemType.Boot:
				case BaseItem.ItemType.Amulet:
				case BaseItem.ItemType.Ring:
					return 1;

				case BaseItem.ItemType.Bracer:
				case BaseItem.ItemType.Pants:
				case BaseItem.ItemType.SpellScroll:
				case BaseItem.ItemType.Shield:
				case BaseItem.ItemType.Quiver:
					return 2;

				case BaseItem.ItemType.ChestArmor:
					return 3;

				default:
					return 0;
			}
		}

		public static ItemStat GetSocketedStat(in int rarity, BaseItem.ItemType type, int subtypeOffset)
		{
			float value = 1;
			int statid = 0;
			switch (type)
			{
				case BaseItem.ItemType.Shield:
				case BaseItem.ItemType.Quiver:
				case BaseItem.ItemType.Pants:
				case BaseItem.ItemType.ChestArmor:
				case BaseItem.ItemType.ShoulderArmor:
				case BaseItem.ItemType.Glove:
				case BaseItem.ItemType.Bracer:
				case BaseItem.ItemType.Ring:
				case BaseItem.ItemType.SpellScroll:
					value = GetSocketStatAmount_Other(in rarity);
					statid = 1;
					break;

				case BaseItem.ItemType.Weapon:
					value = GetSocketStatAmount_Weapon(in rarity) - 1;
					statid = 3;
					break;

				case BaseItem.ItemType.Helmet:
					value = GetSocketStatAmount_Helmet(in rarity) - 1;
					statid = 0;
					break;

				case BaseItem.ItemType.Boot:
					value = GetSocketStatAmount_Boots(in rarity) - 1;
					statid = 2;
					break;

				case BaseItem.ItemType.Amulet:
					value = GetSocketStatAmount_Amulet(in rarity);
					statid = 4;
					break;
			}
			statid = 5 * (subtypeOffset - 1) + 3001 + statid;
			ItemStat stat = new ItemStat(ItemDataBase.StatByID(statid));
			stat.Amount = value * stat.Multipier;
			return stat;
		}

		private static float GetSocketStatAmount_Amulet(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 50f;

				case 4:
					return 100f;

				case 5:
					return 200f;

				case 6:
					return 350f;

				case 7:
					return 600f;

				default:
					return 20f;
			}
		}

		private static float GetSocketStatAmount_Weapon(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 1.1f;

				case 4:
					return 1.2f;

				case 5:
					return 1.35f;

				case 6:
					return 1.50f;

				case 7:
					return 1.70f;

				default:
					return 1.05f;
			}
		}

		private static float GetSocketStatAmount_Boots(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 1.065f;

				case 4:
					return 1.125f;

				case 5:
					return 1.18f;

				case 6:
					return 1.24f;

				case 7:
					return 1.30f;

				default:
					return 1.04f;
			}
		}

		private static float GetSocketStatAmount_Helmet(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 1.075f;

				case 4:
					return 1.125f;

				case 5:
					return 1.2f;

				case 6:
					return 1.25f;

				case 7:
					return 1.30f;

				default:
					return 1.04f;
			}
		}

		private static float GetSocketStatAmount_Other(in int rarity)
		{
			switch (rarity)
			{
				case 3:
					return 15f;

				case 4:
					return 30f;

				case 5:
					return 60f;

				case 6:
					return 120f;

				case 7:
					return 250f;

				default:
					return 5f;
			}
		}
	}
}