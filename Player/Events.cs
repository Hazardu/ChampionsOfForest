using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

namespace ChampionsOfForest.Player
{
	public class Events
	{
		public static Events Instance;
		[System.Serializable]
		public class GotHitByEnemyParams
		{
			public float damage;
			public bool ignoreArmor;
			public object hitBy;

			public GotHitByEnemyParams(float damage, bool ignoreArmor, object hitBy)
			{
				this.damage = damage;
				this.ignoreArmor = ignoreArmor;
				this.hitBy = hitBy;
			}
		}
		[System.Serializable]
		public class GotHitByEnemyEvent : UnityEvent<GotHitByEnemyParams>
		{
		}
		[System.Serializable]
		public class GotHitParams
		{
			public float damage;
			public bool ignoreArmor;

			public GotHitParams(float damage, bool ignoreArmor)
			{
				this.damage = damage;
				this.ignoreArmor = ignoreArmor;
			}
		}
		[System.Serializable]
		public class GotHitEvent : UnityEvent<GotHitParams>
		{
		}

		[System.Serializable]
		public class HitOtherParams
		{
			public float damage;
			public bool isCrit;
			public object hitTarget;
			public object hitSource;

			public HitOtherParams(float damage, bool isCrit, object hitTarget, object hitSource)
			{
				this.damage = damage;
				this.isCrit = isCrit;
				this.hitTarget = hitTarget;
				this.hitSource = hitSource;
			}
		}
		[System.Serializable]
		public class HitOtherEvent : UnityEvent<HitOtherParams>
		{
		}
		[System.Serializable]
		public class HeadshotParams
		{
			public float damage;
			public object hitTarget;
			public object hitSource;
			public bool randomTrigger;

			public HeadshotParams(float damage, object hitTarget, object hitSource, bool randomTrigger)
			{
				this.damage = damage;
				this.hitTarget = hitTarget;
				this.hitSource = hitSource;
				this.randomTrigger = randomTrigger;
			}
		}
		[System.Serializable]
		public class HeadshotEvent : UnityEvent<HeadshotParams>
		{
		}

		public UnityEvent OnDodge = new UnityEvent();
		public GotHitEvent OnGetHit = new GotHitEvent();
		public GotHitEvent OnGetHitPhysical = new GotHitEvent();
		public GotHitEvent OnGetHitNonPhysical = new GotHitEvent();
		public GotHitByEnemyEvent OnGetHitByEnemy = new GotHitByEnemyEvent();
		public UnityEvent OnGetHitByBurning = new UnityEvent();
		public HitOtherEvent OnHitMelee = new HitOtherEvent();
		public HitOtherEvent OnHitSpell = new HitOtherEvent();
		public HitOtherEvent OnHitRanged = new HitOtherEvent();
		public HitOtherEvent OnHitEnemy = new HitOtherEvent();
		public UnityEvent OnTakeLethalDamage = new UnityEvent();
		public UnityEvent OnDeath = new UnityEvent();
		public UnityEvent OnDowned= new UnityEvent();
		public UnityEvent OnGainExp = new UnityEvent();
		public UnityEvent OnGainLevel = new UnityEvent();
		public UnityEvent OnKill = new UnityEvent();
		public UnityEvent OnStun = new UnityEvent();
		public UnityEvent OnLootPickup = new UnityEvent();
		public HeadshotEvent OnHeadshot = new HeadshotEvent();
		public UnityEvent OnFriendlyFire = new UnityEvent();
		public UnityEvent OnAttackRanged = new UnityEvent();
		public UnityEvent OnAttackRangedCrossbow = new UnityEvent();
		public UnityEvent OnAttackMelee = new UnityEvent();
		public UnityEvent OnAnySpellCast = new UnityEvent();
		public UnityEvent OnChanneledSpellCast = new UnityEvent();
		public UnityEvent OnJump = new UnityEvent();
		public UnityEvent OnSprint = new UnityEvent();
		public UnityEvent OnLand = new UnityEvent();
		public UnityEvent OnWeaponEquip = new UnityEvent();
		public UnityEvent OnIgniteMelee = new UnityEvent();
		public UnityEvent OnIgniteRanged = new UnityEvent();
		public UnityEvent OnIgniteSelf = new UnityEvent();
		public UnityEvent OnExtingishSelf = new UnityEvent();
		public UnityEvent OnExplodeSelf = new UnityEvent();

		public static void ClearEvents()
		{
			Instance = new Events();
		}
	}
}
