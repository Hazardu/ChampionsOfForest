﻿using System.Collections.Generic;

using ChampionsOfForest.Effects;

using UnityEngine;

namespace ChampionsOfForest.Player.Buffs
{
	public partial class BuffManager
	{
		struct AppliedBuff
		{
			public BuffTemplate buff;
			float time, value;
			int source;
		}

		List<AppliedBuff> effects;
		List<BuffTemplate> templates;

		const int MAX_ACTIVE_BUFFS = 100;
		public BuffManager()
		{
			templates = new List<BuffTemplate>();
			effects = new List<AppliedBuff>(MAX_ACTIVE_BUFFS);
			InitBuffs();
			InitDebuffs();
		}

		public bool EndBuff(BuffTemplate template, int source)
		{
		
		}
		public bool EndBuff(BuffTemplate template)
		{

		}
		public bool EndBuff(int source)
		{

		}
		public bool EndBuff(BuffType source)
		{

		}
		public bool EndBuff(DebuffType source)
		{

		}

		public bool GiveBuff(BuffType type, float amount, float duration)
		{

		}
		public bool GiveBuff(BuffType type, float amount, float duration, int source)
		{

		}


		public bool GiveDebuff(DebuffType type, float amount, float duration)
		{

		}
		public bool GiveDebuff(DebuffType type, float amount, float duration, int source)
		{

		}



		//public static bool GiveBuff(BuffType id, int source, float amount, float duration)
		//{
		//	return GiveBuff((int)id, source, amount, duration);
		//}
		//public static bool GiveBuff(int id, int source, float amount, float duration)
		//{
		//	try
		//	{
		//		if (BuffsByID.ContainsKey(id))
		//		{
		//			if (BuffsByID[id].isNegative && BuffsByID[id].dispellThreshold <= 1 && (ModdedPlayer.Stats.debuffImmunity > 0 || ModdedPlayer.Stats.debuffResistance > 0))
		//				return false;
		//			if (BuffsByID[id].isNegative && BuffsByID[id].dispellThreshold <= 2 && (ModdedPlayer.Stats.debuffImmunity > 0))
		//				return false;
		//			if (activeBuffs.ContainsKey(source))
		//			{
		//				activeBuffs[source].duration = duration;
		//				if (activeBuffs[source].OnAddOverrideAmount)
		//				{
		//					activeBuffs[source].amount = amount;
		//				}
		//				else
		//				{
		//					if (activeBuffs[source].accumulateEffect)
		//					{
		//						activeBuffs[source].amount += amount;
		//					}
		//					else if (activeBuffs[source].amount < amount)
		//					{
		//						activeBuffs[source].amount = amount;
		//					}
		//				}
		//				return true;
		//			}
		//			else
		//			{
		//				Buff b = new Buff(id, amount, duration);
		//				activeBuffs.Add(source, b);
		//				if (b.OnStart != null)
		//				{
		//					b.OnStart(b.amount);
		//				}

		//				return true;
		//			}
		//		}
		//		else
		//		{
		//			ModAPI.Log.Write("Couldnt add a buff, no buff with id " + id + " in the database");
		//			return false;
		//		}
		//	}
		//	catch (System.Exception e)
		//	{
		//		ModAPI.Log.Write("Error when adding buff... " + e.ToString());
		//	}
		//	return false;
		//}

		public void FillBuffList()
		{
			try
			{
				activeBuffs.Clear();
				BuffsByID.Clear();
				new Buff(1, 156, "Move speed reduced", true, false, 1, SpellActions.BUFF_DivideMS, SpellActions.BUFF_MultMS);
				new Buff(2, 148, "Attack speed reduced", true, false, 1, SpellActions.BUFF_DivideAS, SpellActions.BUFF_MultAS);
				new Buff(3, 157, "Poisoned", true, true, 2)
				{
					DisplayAsPercent = false
				};
				new Buff(4, 158, "Root Immune", false, false, 0, (f) => ModdedPlayer.Stats.rootImmunity.valueAdditive--, f => ModdedPlayer.Stats.rootImmunity.valueAdditive++)
				{
					DisplayAmount = false
				};

				new Buff(5, 150, "Move speed increased", false, false, 1, SpellActions.BUFF_DivideMS, SpellActions.BUFF_MultMS);

				new Buff(6, 155, "Stun Immune", false, false, 0, (f) => ModdedPlayer.Stats.stunImmunity.valueAdditive--, f => ModdedPlayer.Stats.stunImmunity.valueAdditive++)
				{
					DisplayAmount = false
				};
				new Buff(7, 154, "Debuff Immune", false, false, 0, (f) => ModdedPlayer.Stats.debuffImmunity.valueAdditive--, f => ModdedPlayer.Stats.debuffImmunity.valueAdditive++)
				{
					DisplayAmount = false
				};
				new Buff(8, 155, "Debuff Resistant", false, false, 0, (f) => ModdedPlayer.Stats.debuffResistance.valueAdditive--, f => ModdedPlayer.Stats.debuffResistance.valueAdditive++)
				{
					DisplayAmount = false
				};

				new Buff(9, 151, "Increased Damage", false, false, 0, f => ModdedPlayer.Stats.allDamage.valueMultiplicative /= f, f => ModdedPlayer.Stats.allDamage.valueMultiplicative *= f);

				new Buff(10, 152, "Decreased Damage", true, false, 2, f => ModdedPlayer.Stats.allDamage.valueMultiplicative /= f, f => ModdedPlayer.Stats.allDamage.valueMultiplicative *= f);

				new Buff(11, 160, "Stamina regen", false, false, 0, f => ModdedPlayer.Stats.staminaPerSecRate.Substract(f), f => ModdedPlayer.Stats.staminaPerSecRate.Add(f));

				new Buff(12, 153, "Death Pact Damage", false, false) { OnAddOverrideAmount = true };
				new Buff(13, 151, "Increased Melee Damage", false, false, 0, f => ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative /= f, f => ModdedPlayer.Stats.meleeIncreasedDmg.valueMultiplicative *= f);
				new Buff(14, 149, "Attack speed increased", false, false, 1, SpellActions.BUFF_DivideAS, SpellActions.BUFF_MultAS);

				new Buff(15, 146, "Armor", false, false, 1, f => ModdedPlayer.Stats.armor.valueAdditive -= Mathf.RoundToInt(f), f => ModdedPlayer.Stats.armor.valueAdditive += Mathf.RoundToInt(f)) { DisplayAsPercent = false };

				new Buff(16, 133, "Golden Skin", false, false, 1, f => GoldenSkin.Disable(), f => GoldenSkin.Enable()) { DisplayAmount = false };

				new Buff(17, 131, "Berserk", false, false, 1, f => Berserker.OnDisable(), f => Berserker.OnEnable()) { DisplayAmount = false };

				new Buff(18, 161, "Energy Leak", true, false, 1, f => ModdedPlayer.Stats.energyRecoveryperSecond.valueAdditive += f, f => ModdedPlayer.Stats.energyRecoveryperSecond.valueAdditive -= f) { DisplayAmount = false };

				new Buff(19, 136, "Frenzy", false, false, 1, f =>
				{
					ModdedPlayer.Stats.attackSpeed.valueMultiplicative /= 1 + f * ModdedPlayer.Stats.spell_frenzyAtkSpeed;
					ModdedPlayer.Stats.allDamage.valueMultiplicative /= 1 + f * ModdedPlayer.Stats.spell_frenzyDmg;
					if (ModdedPlayer.Stats.spell_frenzyMS)
						ModdedPlayer.Stats.movementSpeed.valueMultiplicative /= 1 + f * 0.05f;

					ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive = 0;
				}, f =>
				{
					ModdedPlayer.Stats.attackSpeed.valueMultiplicative *= 1 + f * ModdedPlayer.Stats.spell_frenzyAtkSpeed;
					ModdedPlayer.Stats.allDamage.valueMultiplicative *= 1 + f * ModdedPlayer.Stats.spell_frenzyDmg;
					if (ModdedPlayer.Stats.spell_frenzyMS)
						ModdedPlayer.Stats.movementSpeed.valueMultiplicative *= 1 + f * 0.05f;
				})
				{
					DisplayAsPercent = false
				};

				new Buff(20, 159, "Near Death Experience", false, false, 5, f => ModdedPlayer.Stats.perk_nearDeathExperienceTriggered.value = false, f => ModdedPlayer.Stats.perk_nearDeathExperienceTriggered.value = true) { DisplayAmount = false };

				new Buff(21, 147, "Armor Corruption", true, true, 1, null, null) { DisplayAsPercent = false };

				new Buff(22, 151, "Increased Flat Melee Damage", false, false, 0, f => ModdedPlayer.Stats.meleeFlatDmg.Substract(f), f => ModdedPlayer.Stats.meleeFlatDmg.Add(f)) { DisplayAsPercent = false };
				new Buff(23, 151, "Counter Strike", false, true, 0, f => ModdedPlayer.Stats.perk_parryCounterStrikeDamage.valueAdditive = 0) { DisplayAsPercent = false };
				new Buff(24, 151, "Critical Damage", false, false, 0, f => ModdedPlayer.Stats.critDamage.Substract(f), f => ModdedPlayer.Stats.critDamage.Add(f));
				new Buff(25, 146, "Life Regeneration", false, false, 0, f => ModdedPlayer.Stats.healthRecoveryPerSecond.Add(-f), f => ModdedPlayer.Stats.healthRecoveryPerSecond.Add(f)) { DisplayAsPercent = false };
				new Buff(26, 146, "Resistance", false, false, 0, f => ModdedPlayer.Stats.allDamageTaken.Divide(1 - f), f => ModdedPlayer.Stats.allDamageTaken.Multiply(1 - f));
				new Buff(27, 136, "Fury Swipes", false, true, 1, f =>
				{
					ModdedPlayer.instance.FurySwipesLastHit = null;
					ModdedPlayer.Stats.rangedFlatDmg.valueAdditive -= ModdedPlayer.instance.FurySwipesDmg;
					ModdedPlayer.Stats.spellFlatDmg.valueAdditive -= ModdedPlayer.instance.FurySwipesDmg;
					ModdedPlayer.Stats.meleeFlatDmg.valueAdditive -= ModdedPlayer.instance.FurySwipesDmg;
					ModdedPlayer.instance.FurySwipesDmg = 0;
				})
				{
					DisplayAsPercent = false
				};
				new Buff(28, 151, "Critical Chance", false, false, 0, f => ModdedPlayer.Stats.critChance.Substract(f - 1), f => ModdedPlayer.Stats.critChance.Add(f - 1)) { DisplayAsPercent = true };

				new Buff(29, 151, "Dodge Chance", false, false, 0, f => ModdedPlayer.Stats.getHitChance.Divide(f), f => ModdedPlayer.Stats.getHitChance.Multiply(f)) { DisplayAsPercent = true };

				new Buff(30, 151, "Cooldown Rate", false, false, 0, f => ModdedPlayer.Stats.cooldownRate.Divide(f), f => ModdedPlayer.Stats.cooldownRate.Multiply(f)) { DisplayAsPercent = true };

				new Buff(31, 151, "Resource Cost", false, false, 0, f => { ModdedPlayer.Stats.attackStaminaCost.Divide(f); ModdedPlayer.Stats.spellCost.Divide(f); }, f => { ModdedPlayer.Stats.attackStaminaCost.Multiply(f); ModdedPlayer.Stats.spellCost.Multiply(f); }) { DisplayAsPercent = true };


			}
			catch (System.Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}
	}
}

//SOURCES of debuffs
//5 - flare slow
//6 - flare speed
//30 & 31 - absolute zero
//32 - poison from enemy hit
//33 - poisons slow
//41 - Hexing Pants dmg amp
//42 - Hexing Pants regen amp
//43 - Death Pact dmg amp
//44 - Black flame dmg amp
//45 - warcry ms
//46 - warcry as
//47 - warcry dmg
//48 - warcry armor
//49 - golden skin
//50 - berserker
//51 - berserker energy leak
//60 - frenzy
//61 - near death experience
//62 - parry immunity
//63 - cataclysm negative armor
//64 - cataclysm ms
//65 - cataclysm as
//65 - cataclysm cast debuff ms
//66 laser beam slow
//67 laser beam dmg debuff
//68 laser poison
//69 meteor armor corruption
//70...73 enemy abilities debuff
//80 king qruies damage
//81 smokeys quiver atk speed
//82,83 near death buffs
//84,85,86 wind armor buffs
//87 momentum transfer
//88 counter strike passive
//89 bash crit dmg passive
//90 purge damage buff
//91,92 archangel bow buff
//93 archangel bow buff
//94 - flare resistance
//95 triple damage Blood Infused Arrow energy leak
//96 fart slow
//97 - blink cc break
//98 fury swipes counter
//99 near death experience health regen
//100 - idk
//101 - bash active
//102 - focus active
//103 - parry active
//104 - true aim crit chance
//105 parry attack speed
//106 berserk set 4pc buff
//107 berserk set 5pc buff
//108 resist death dmg reduction
//109 frenzy energy regen
//40002 ... 40010 - healing dome