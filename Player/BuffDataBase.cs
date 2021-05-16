using System.Collections.Generic;

using ChampionsOfForest.Effects;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public static class BuffDB
	{
		public static Dictionary<int, Buff> activeBuffs = new Dictionary<int, Buff>();
		public static Dictionary<int, Buff> BuffsByID = new Dictionary<int, Buff>();
		public static bool ForceEndBuff(int source)
		{
			if (activeBuffs.TryGetValue(source, out Buff b))
			{
				b.ForceEndBuff(source);
				return true;
			}
			return false;
		}

		public static bool AddBuff(int id, int source, float amount, float duration)
		{
			try
			{
				if (BuffsByID.ContainsKey(id))
				{
					if (BuffsByID[id].isNegative && BuffsByID[id].DispellAmount <= 1 && (ModdedPlayer.Stats.debuffImmunity > 0 || ModdedPlayer.Stats.debuffResistance > 0))
						return false;
					if (BuffsByID[id].isNegative && BuffsByID[id].DispellAmount <= 2 && (ModdedPlayer.Stats.debuffImmunity > 0))
						return false;
					if (activeBuffs.ContainsKey(source))
					{
						activeBuffs[source].duration = duration;
						if (activeBuffs[source].OnAddOverrideAmount)
						{
							activeBuffs[source].amount = amount;
						}
						else
						{
							if (activeBuffs[source].AccumulateEffect)
							{
								activeBuffs[source].amount += amount;
							}
							else if (activeBuffs[source].amount < amount)
							{
								activeBuffs[source].amount = amount;
							}
						}
						return true;
					}
					else
					{
						Buff b = new Buff(id, amount, duration);
						activeBuffs.Add(source, b);
						if (b.OnStart != null)
						{
							b.OnStart(b.amount);
						}

						return true;
					}
				}
				else
				{
					ModAPI.Log.Write("Couldnt add a buff, no buff with id " + id + " in the database");
					return false;
				}
			}
			catch (System.Exception e)
			{
				ModAPI.Log.Write("Error when adding buff... " + e.ToString());
			}
			return false;
		}

		public static void FillBuffList()
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

				new Buff(11, 160, "Energy Regen Amp", false, false, 0, f => ModdedPlayer.Stats.staminaPerSecRate.valueAdditive-= f, f => ModdedPlayer.Stats.staminaPerSecRate.Add(f));

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
						ModdedPlayer.Stats.movementSpeed.valueMultiplicative/= 1 + f * 0.05f;

					ModdedPlayer.Stats.spell_frenzyStacks.valueAdditive=0;
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

				new Buff(21, 147, "Armor Corruption", true, true, 1, null,null) { DisplayAsPercent = false };

				new Buff(22, 151, "Increased Damage", false, false, 0, f => ModdedPlayer.Stats.meleeFlatDmg.Substract(f), f => ModdedPlayer.Stats.meleeFlatDmg.Add(f)) { DisplayAsPercent = false };
				new Buff(23, 151, "Counter Strike", false, true, 0, f => ModdedPlayer.Stats.perk_parryCounterStrikeDamage.valueAdditive = 0) { DisplayAsPercent = false };
				new Buff(24, 151, "Critical Damage", false, false, 0, f => ModdedPlayer.Stats.critDamage.Substract(f), f => ModdedPlayer.Stats.critDamage.Add(f) );
				new Buff(25, 146, "Life Regeneration", false, false, 0, f => ModdedPlayer.Stats.healthRecoveryPerSecond.Add(- f), f => ModdedPlayer.Stats.healthRecoveryPerSecond.Add(f)) { DisplayAsPercent = false };
				new Buff(26, 146, "Resistance", false, false, 0, f => ModdedPlayer.Stats.allDamageTaken.Divide(1 - f), f => ModdedPlayer.Stats.allDamageTaken.Multiply( 1 - f));
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


			}
			catch (System.Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}

		public class Buff
		{
			public int _ID;
			public float amount;
			public float duration;
			public string BuffName;
			public bool isNegative;
			public bool AccumulateEffect;

			public delegate void onBuffEnd(float f);

			public delegate void onBuffStart(float f);

			public onBuffEnd OnEnd;
			public onBuffStart OnStart;
			public int DispellAmount;
			public bool DisplayAsPercent = true;
			public bool DisplayAmount = true;
			public bool OnAddOverrideAmount = false;
			public int IconID;

			public Buff(int id, float amount, float duration)
			{
				_ID = id;
				Buff b = BuffDB.BuffsByID[id];
				OnEnd = b.OnEnd;
				OnStart = b.OnStart;
				BuffName = b.BuffName;
				isNegative = b.isNegative;
				DispellAmount = b.DispellAmount;
				AccumulateEffect = b.AccumulateEffect;
				OnAddOverrideAmount = b.OnAddOverrideAmount;
				DisplayAsPercent = b.DisplayAsPercent;
				this.amount = amount;
				this.duration = duration;
			}

			public Buff(int BuffID, int iconID, string name, bool IsNegative, bool accumulate, int dispellamount = 0, onBuffEnd END = null, onBuffStart START = null)
			{
				_ID = BuffID;
				AccumulateEffect = accumulate;
				isNegative = IsNegative;
				BuffName = name;
				OnEnd = END;
				OnStart = START;
				DispellAmount = dispellamount;
				amount = 1;
				duration = 1;
				IconID = iconID;
				BuffDB.BuffsByID.Add(BuffID, this);
			}

			public void ForceEndBuff(int source)
			{
				BuffDB.activeBuffs.Remove(source);
				if (OnEnd != null)
				{
					OnEnd(amount);
				}
			}

			/// <summary>
			/// determines if the buff is already over. Call this in ModdedPlayer update
			/// </summary>
			public void UpdateBuff(int source)
			{
				if (isNegative && DispellAmount <= 1 && (ModdedPlayer.Stats.debuffImmunity > 0 || ModdedPlayer.Stats.debuffResistance > 0))
					duration = 0;
				if (isNegative && DispellAmount <= 2 && (ModdedPlayer.Stats.debuffImmunity > 0))
					duration = 0;

				if (duration > 0)
				{
					duration -= Time.deltaTime;
				}
				else
				{
					BuffDB.activeBuffs.Remove(source);
					OnEnd?.Invoke(amount);
				}
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
//40002 & 40001 - immunity to cc healing dome
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