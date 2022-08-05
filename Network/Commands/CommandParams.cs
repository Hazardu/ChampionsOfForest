using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static ChampionsOfForest.ModSettings;

namespace ChampionsOfForest.Network.CommandParams
{
	[Serializable]
	public struct params_DIFFICULTY_INFO_ANSWER
	{
		public params_DIFFICULTY_INFO_ANSWER(int difficulty, int dropsOnDeath float expMultiplier, float enemyDamageMultiplier, float friendlyFireDamage, bool friendlyFire, bool killOnDowned, bool friendlyFireMagic)
		{
			Difficulty = difficulty;
			DropsOnDeath = dropsOnDeath;
			ExpMultiplier = expMultiplier;
			EnemyDamageMultiplier = enemyDamageMultiplier;
			FriendlyFireDamage = friendlyFireDamage;
			FriendlyFire = friendlyFire;
			KillOnDowned = killOnDowned;
			FriendlyFireMagic = friendlyFireMagic;
		}

		public int Difficulty
		{
			get; set;
		}
		public int DropsOnDeath
		{
			get; set;
		}
		public float ExpMultiplier
		{
			get; set;
		}
		public float EnemyDamageMultiplier
		{
			get; set;
		}
		public float FriendlyFireDamage
		{
			get; set;
		}
		public bool FriendlyFire
		{
			get; set;
		}
		public bool KillOnDowned
		{
			get; set;
		}
		public bool FriendlyFireMagic
		{
			get; set;
		}
	}
	[Serializable]
	public struct params_SPELL_BLACK_HOLE
	{
		public float Damage
		{
			get; set;
		}
		public float Lifetime
		{
			get; set;
		}
		public float Radius
		{
			get; set;
		}
		public float Pullforce
		{
			get; set;
		}
		public ulong casterID
		{
			get; set;
		}
		public bool Hostile
		{
			get; set;
		}
		public bool Explode
		{
			get; set;
		}
		public bool GiveDamageBuff
		{
			get; set;
		}
		public bool Stun
		{
			get; set;
		}

	}
	[Serializable]
	public struct params_SPELL_SANCTUARY
	{

	}
	[Serializable]
	public struct params_SPELL_SUNFLARE
	{

	}
	[Serializable]
	public struct params_SPELL_BLACKFLAME
	{

	}
	[Serializable]
	public struct params_SPELL_WARCRY
	{

	}
	[Serializable]
	public struct params_SPELL_PORTAL
	{

	}
	[Serializable]
	public struct params_SPELL_MAGIC_ARROW
	{

	}
	[Serializable]
	public struct params_SPELL_PURGE
	{

	}
	[Serializable]
	public struct params_SPELL_CATACLYSM
	{

	}
	[Serializable]
	public struct params_SPELL_BALLLIGHTNING
	{
		public float Damage
		{
			get; set;
		}
		public uint ID
		{
			get; set;
		}
		public ulong casterID
		{
			get; set;
		}
		public bool ImmediateExplode
		{
			get; set;
		}
		public bool Crit
		{
			get; set;
		}
	}
	[Serializable]
	public struct params_SPELL_BALLLIGHTNING_REQUEST
	{

	}
	[Serializable]
	public struct params_SPELL_BALL_LIGHTNING_TRIGGER
	{

	}
	[Serializable]
	public struct params_SPELL_PARRY
	{

	}
	[Serializable]
	public struct params_SPELL_FART
	{

	}
	[Serializable]
	public struct params_SPELL_TAUNT
	{

	}
	[Serializable]
	public struct params_SPELL_BASH_HIT_ENEMY
	{

	}
	[Serializable]
	public struct params_ITEM_REMOVE_PICKUP
	{

	}
	[Serializable]
	public struct params_ITEM_SPAWN_PICKUP
	{

	}
	[Serializable]
	public struct params_ENEMY_STATS_INFO_BROADCAST
	{

	}
	[Serializable]
	public struct params_ENEMY_STATS_INFO_RECEIVE
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_BLIZZARD
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_RADIANCE
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_CHAINS
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_CHAINS_VISUAL
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_TRAP_SPHERE
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_LASAER
	{

	}
	[Serializable]
	public struct params_SPELL_ENEMY_METEOR
	{

	}
	[Serializable]
	public struct params_BUFF_GIVE_PLAYER
	{

	}
	[Serializable]
	public struct params_STUN_PLAYER
	{

	}
	[Serializable]
	public struct params_EXPERIENCE_KILL
	{

	}
	[Serializable]
	public struct params_EXPERIENCE_FINAL
	{

	}
	[Serializable]
	public struct params_PLAYER_STATS_INFO_REQUEST
	{

	}
	[Serializable]
	public struct params_PLAYER_STATS_INFO_RECEIVE
	{

	}
	[Serializable]
	public struct params_HITMARKER_COLOR
	{

	}
	[Serializable]
	public struct params_ENEMY_DEBUFF_SLOW
	{

	}
	[Serializable]
	public struct params_ENEMY_DEBUFF_FIRE
	{

	}
	[Serializable]
	public struct params_ENEMY_DEBUFF_DOT
	{

	}
	[Serializable]
	public struct params_ENEMY_DEBUFF_DAMAGE_TAKEN
	{

	}
	[Serializable]
	public struct params_ENEMY_KNOCKBACK
	{

	}
	[Serializable]
	public struct params_ENEMY_STATS_UPDATE_VOLATILE_REQUEST
	{

	}
	[Serializable]
	public struct params_ENEMY_STATS_UPDATE_VOLATILE_ANSWER
	{

	}
	[Serializable]
	public struct params_ITEM_PICKUP_PLAYER_COLLECTED
	{

	}
	[Serializable]
	public struct params_ITEM_ADD_TO_INVENTORY
	{

	}
	[Serializable]
	public struct params_EQUIP_CUSTOM_WEAPON
	{

	}
	[Serializable]
	public struct params_ENEMY_SYNC_APPEARANCE_REQUEST
	{

	}
	[Serializable]
	public struct params_ENEMY_SYNC_APPEARANCE
	{

	}
	[Serializable]
	public struct params_PING_CLEAR
	{

	}
	[Serializable]
	public struct params_PING_PLACE
	{

	}
	[Serializable]
	public struct params_PING_ENEMY_INFO
	{

	}
	[Serializable]
	public struct params_SPELL_BALLLIGHTNING_CLIENT_VISUAL
	{

	}
	[Serializable]
	public struct params_ITEM_ARCHANGEL_FF
	{

	}
	[Serializable]
	public struct params_BUFF_ADD
	{

	}
	[Serializable]
	public struct params_BUFF_ADD_AOE
	{

	}
	[Serializable]
	public struct params_BUFF_ADD_GLOBAL
	{

	}
}
