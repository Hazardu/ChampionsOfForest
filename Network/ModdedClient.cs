using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ChampionsOfForest.Network
{
	public class ModdedClient
	{
		public GameObject gameObject;
		public Transform transform;
		public GameObject rightHand, leftHand, feet;
		public BoltEntity entity;
		public string username;
		public int level;
		public float hp, maxHp, energy, maxEnergy;
		public ulong Packed => entity.networkId.PackedValue;
		public long experience;


		internal ModdedClient(BoltEntity entity)
		{
			this.entity = entity;
			gameObject = entity.gameObject;
			transform = gameObject.transform;

			rightHadsand = rightHand;
			leftHand = leftHand;
			feet = feet;
			username = username;
			level = level;
		}
	}
}
