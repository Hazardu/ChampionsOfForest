using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ChampionsOfForest.Network.Commands
{
	public struct GetPlayerStateParams
	{
		public ulong entityNetworkID;
		public string playerID;
		public int level;
		public float health;
		public float maxHealth;
		public long xp;
	}

	public struct BroadcastModSettings
	{
		public ModSettings.DropsOnDeathModes dropsOnDeath;
		public ModSettings.GameDifficulty difficulty;
		public bool dieOnDowned;
		public bool friendlyFire;

	}

	public struct DestroyItemPickup
	{
		public ulong pickupID;

		public DestroyItemPickup(ulong pickupID)
		{
			this.pickupID = pickupID;
		}
	}

	public struct CreateHitMarker
	{
		public float damage;
		public float x, y, z;
		public float r, g, b, a;

		public CreateHitMarker(float damage, Vector3 pos, Color color)
		{
			this.damage = damage;
			this.x = pos.x;
			this.y = pos.y;
			this.z = pos.z;
			this.r = color.r;
			this.g = color.g;
			this.b = color.b;
			this.a = color.a;
		}

		public Color GetColor()
		{
			return new Color(r, g, b, a);
		}
		public Vector3 GetPosition()
		{
			return new Vector3(x, y, z);
		}
	}

}
