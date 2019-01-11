using TheForest.Utils;
using TheForest.World;
using UnityEngine;

namespace ChampionsOfForest.Enemies.EnemyAbilities
{
    public class Meteor : MonoBehaviour
    {

        public static void CreateEnemy(Vector3 position, int seed)
        {
            MeteorSpawner.Instance.Spawn(position, seed);
        }

        private int Damage;

        private void Update()
        {
            transform.Translate((Vector3.down * 2 + Vector3.forward) * Time.deltaTime * 25);
            LocalPlayer.HitReactions.enableFootShake(1, 0.5f);

        }

        private void Start()
        {
            Destroy(gameObject, 7);
            switch (ModSettings.difficulty)
            {
                case ModSettings.Difficulty.Normal:
                    Damage = 200;
                    break;
                case ModSettings.Difficulty.Hard:
                    Damage = 1000;

                    break;
                case ModSettings.Difficulty.Elite:
                    Damage = 5000;

                    break;
                case ModSettings.Difficulty.Master:
                    Damage = 25000;

                    break;
                case ModSettings.Difficulty.Challenge1:
                    Damage = 75000;

                    break;
                case ModSettings.Difficulty.Challenge2:
                    Damage = 125000;

                    break;
                case ModSettings.Difficulty.Challenge3:
                    Damage = 300000;

                    break;
                case ModSettings.Difficulty.Challenge4:
                    Damage = 650000;
                    break;
                case ModSettings.Difficulty.Challenge5:
                    Damage = 1000000;
                    break;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("suitCase") || other.CompareTag("metalProp") || other.CompareTag("animalCollide") || other.CompareTag("Fish") || other.CompareTag("Tree") || other.CompareTag("MidTree") || other.CompareTag("suitCase") || other.CompareTag("SmallTree"))
            {
                other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("Burn", Damage, SendMessageOptions.DontRequireReceiver);

            }
            else if (other.transform.root == LocalPlayer.Transform.root)
            {
                LocalPlayer.Stats.Hit((int)(Damage * (1 - ModdedPlayer.instance.MagicResistance)), true, PlayerStats.DamageType.Fire);
                other.SendMessage("Burn", Damage, SendMessageOptions.DontRequireReceiver);

            }
            else if (other.CompareTag("BreakableWood") || other.CompareTag("BreakableRock") || other.CompareTag("BreakableRock") || other.CompareTag("structure"))
            {
                other.SendMessage("Hit", Damage, SendMessageOptions.DontRequireReceiver);
                other.SendMessage("LocalizedHit", new LocalizedHitData(transform.position, Damage), SendMessageOptions.DontRequireReceiver);

            }
        }
    }
}
