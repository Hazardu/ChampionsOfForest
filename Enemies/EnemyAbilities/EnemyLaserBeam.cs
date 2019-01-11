using System.Collections;
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


                RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 50);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform != null)
                    {
                        if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("PlayerNet"))
                        {
                            if (hit.transform.root == LocalPlayer.Transform.root)
                            {
                                LocalPlayer.Stats.Hit((int)(dmg * (1 - ModdedPlayer.instance.MagicResistance) / 10), false, PlayerStats.DamageType.Fire);
                                hit.transform.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                            }
                        }
                        else if (hit.transform.CompareTag("structure"))// && (!BoltNetwork.isRunning || BoltNetwork.isServer || !BoltNetwork.isClient || !PlayerPreferences.NoDestructionRemote))
                        {
                            hit.transform.SendMessage("Hit", dmg / 10, SendMessageOptions.DontRequireReceiver);
                            hit.transform.SendMessage("LocalizedHit", new LocalizedHitData(hit.point, dmg / 10), SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                yield return new WaitForSeconds(0.1f);

            }
        }
    }
}
