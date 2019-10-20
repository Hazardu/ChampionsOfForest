using TheForest.Utils;
using TheForest.World;
using UnityEngine;

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
            transform.Translate((Vector3.down * 2 + Vector3.forward) * Time.deltaTime * 25);

        }

        private static AudioClip hitSound, InitSound;

        private void Start()
        {
            if (hitSound == null)
            {
                hitSound = Res.ResourceLoader.instance.LoadedAudio[1005];
                InitSound = Res.ResourceLoader.instance.LoadedAudio[1006];
            }
            src =gameObject.AddComponent<AudioSource>();
            src.clip = InitSound;
            src.Play();
            Destroy(gameObject, 7);
           
        }

        private void OnTriggerEnter(Collider other)
        {
            src.clip = hitSound;
            src.Play();
            LocalPlayer.HitReactions.enableFootShake(1, 0.5f);

            if (other.CompareTag("suitCase") || other.CompareTag("metalProp") || other.CompareTag("animalCollide") || other.CompareTag("Fish") || other.CompareTag("Tree") || other.CompareTag("MidTree") || other.CompareTag("suitCase") || other.CompareTag("SmallTree"))
            {
                other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("Explosion", 0.1f, SendMessageOptions.DontRequireReceiver);

            }
            else if (other.transform.root == LocalPlayer.Transform.root)
            {
                LocalPlayer.Stats.Hit((int)(Damage * (1 - ModdedPlayer.instance.MagicResistance)), true, PlayerStats.DamageType.Physical);
                Player.BuffDB.AddBuff(21, 69, Damage, 60);
                other.SendMessage("Burn", Damage, SendMessageOptions.DontRequireReceiver);

            }
            else if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("BreakableRock") || other.CompareTag("structure"))
            {
                other.SendMessage("Hit", Damage *2, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("LocalizedHit", new LocalizedHitData(transform.position, Damage), SendMessageOptions.DontRequireReceiver);

            }
        }
    }
}
