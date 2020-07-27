using System.Collections;

using ChampionsOfForest.Player;

using TheForest.Utils;
using TheForest.World;

using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class EnemyLaserBeam : MonoBehaviour
	{
		public void Initialize(int dmgPerSecond)
		{
			dmg = dmgPerSecond;
			StartCoroutine(DoAction());
		}

		private int dmg;

		public IEnumerator DoAction()
		{
			while (true)
			{
				RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.one * 0.5f, transform.forward, transform.rotation, 50);
				foreach (RaycastHit hit in hits)
				{
					if (hit.transform != null)
					{
						if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("PlayerNet"))
						{
							if (hit.transform.root == LocalPlayer.Transform.root)
							{
								LocalPlayer.Stats.Hit((int)(dmg * 0.3f * (1 - ModdedPlayer.Stats.magicDamageTaken)), false, PlayerStats.DamageType.Fire);
								BuffDB.AddBuff(10, 67, 0.5f, 15);
								BuffDB.AddBuff(2, 66, 0.5f, 15);
								BuffDB.AddBuff(3, 68, dmg / 13, 5);
								hit.transform.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
							}
						}
						else if (hit.transform.CompareTag("structure"))// && (!BoltNetwork.isRunning || BoltNetwork.isServer || !BoltNetwork.isClient || !PlayerPreferences.NoDestructionRemote))
						{
							hit.transform.SendMessage("Hit", dmg / 2, SendMessageOptions.DontRequireReceiver);
							hit.transform.SendMessage("LocalizedHit", new LocalizedHitData(hit.point, dmg / 2), SendMessageOptions.DontRequireReceiver);
						}
					}
				}
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}