﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsOfForest.Player.Buffs
{
	public partial class BuffManager
	{
		public enum BuffType
		{
			MOV_SPEED_REDUCTION = 1,
			ATK_SPEED_REDUCTION,
			POISON,
			ROOT_IMMUNITY,
			MOV_SPEED_INCREASED,
			STUN_IMMUNITY,
			DEBUFF_IMMUNITY,
			DEBUFF_RESISTANCE,
			DAMAGE_INCREASED,
			DAMAGE_DECREASED,
			STAMINA_REGEN,
			ITEM_DEATH_PACT,
			MELEE_DAMAGE_INCREASED,
			ATTACK_SPEED_INCREASED,
			ARMOR,
			SKILL_GOLDEN_SKIN,
			SKILL_BERSERK,
			ENERGY_LEAK,
			SKILL_FRENZY,
			SKILL_PREVENT_DEATH_COOLDOWN,
			ARMOR_LOSS,
			MELEE_FLAT_DAMAGE_INCREASE,
			SKILL_PARRY_STACK,
			CRIT_DAMAGE,
			HEALTH_REGEN,
			TOUGHNESS,
			SKILL_FRENZY_FURY_SWIPES,
			CRIT_CHANCE,
			DODGE_CHANCE,
			COOLDOWN_RATE,
			RESOURCE_COST,
			BUFF_COUNT
		}
		void InitBuffs()
		{

		}
	}
}