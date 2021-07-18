using Bolt;

using ChampionsOfForest.Player;

using TheForest.Utils;
using TheForest.World;

using UnityEngine;

using Math = System.Math;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
	public class Meteor : MonoBehaviour
	{
		private AudioSource src;

		public static void CreateEnemy(Vector3 position, int seed)
		{
			MeteorSpawner.Instance.Spawn(position, seed);
		}

		public int Damage;

		private void Update()
		{
			transform.Translate((Vector3.down * 2 + Vector3.forward) * Time.deltaTime * 30);
		}

		private static AudioClip hitSound, InitSound;

		private void Start()
		{
			if (hitSound == null)
			{
				hitSound = Res.ResourceLoader.instance.LoadedAudio[1005];
				InitSound = Res.ResourceLoader.instance.LoadedAudio[1006];
			}
			src = gameObject.AddComponent<AudioSource>();
			src.spatialBlend = 1f;
			src.clip = InitSound;
			src.rolloffMode = AudioRolloffMode.Linear;
			src.maxDistance = 100;
			src.Play();
			Destroy(gameObject, 4);
		}

		private void OnTriggerEnter(Collider other)
		{
			float distance = Vector3.Distance(LocalPlayer.Transform.position,this.transform.position);
			
			if (distance < 100)
			{
				src.clip = hitSound;
				src.Play();
				LocalPlayer.HitReactions.enableFootShake(1, Math.Min(30 / distance,0.5f));
			}
			if (other.CompareTag("suitCase") || other.CompareTag("metalProp") || other.CompareTag("animalCollide") ||
			    other.CompareTag("Fish") || other.CompareTag("Tree") || other.CompareTag("MidTree") ||
			    other.CompareTag("suitCase") || other.CompareTag("SmallTree"))
			{
				other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
				other.SendMessage("Explosion", 0.1f, SendMessageOptions.DontRequireReceiver);
			}
			else if (other.transform.root == LocalPlayer.Transform.root)
			{
				LocalPlayer.Stats.Hit((int)(Damage), false, PlayerStats.DamageType.Fire);
				ModdedPlayer.instance.Stun(3f);
				Player.BuffDB.AddBuff(21, 69, Damage/3, 60);
				other.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);

			}
			else if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("BreakableRock") || other.CompareTag("structure"))
			{
				other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
				other.SendMessage("LocalizedHit", new LocalizedHitData(transform.position, Damage), SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}