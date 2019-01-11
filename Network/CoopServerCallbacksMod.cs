using UnityEngine;

namespace ChampionsOfForest.Network
{
    internal class CoopServerCallbacksMod : CoopServerCallbacks
    {

        public override void OnEvent(PlayerHitEnemy ev)
        {
            if (!this.ValidateSender(ev, global::SenderTypes.Any))
            {
                return;
            }
            if (!ev.Target)
            {
                return;
            }
            if (ev.Hit == 0)
            {
                return;
            }
            try
            {
                if (global::EnemyHealth.CurrentAttacker == null)
                {
                    global::EnemyHealth.CurrentAttacker = ev.Target;
                }
                global::lb_Bird component = ev.Target.GetComponent<global::lb_Bird>();
                global::Fish componentInChildren = ev.Target.GetComponentInChildren<global::Fish>();
                Transform transform;
                if (componentInChildren)
                {
                    transform = componentInChildren.transform;
                }
                else if (ev.Target.GetComponent<global::animalHealth>())
                {
                    transform = ev.Target.transform;
                }
                else if (component)
                {
                    transform = component.transform;
                }
                else
                {
                    global::EnemyHealth componentInChildren2 = ev.Target.GetComponentInChildren<global::EnemyHealth>();
                    if (componentInChildren2)
                    {
                        transform = componentInChildren2.transform;
                    }
                    else
                    {
                        transform = ev.Target.transform.GetChild(0);
                    }
                }
                if (ev.getAttacker == 10 && ev.Weapon)
                {
                    global::ArrowDamage componentInChildren3 = ev.Weapon.GetComponentInChildren<global::ArrowDamage>();
                    if (componentInChildren3.Live)
                    {
                        global::arrowStickToTarget componentInChildren4 = transform.GetComponentInChildren<global::arrowStickToTarget>();
                        Transform target = transform;
                        if (componentInChildren4)
                        {
                            target = componentInChildren4.transform;
                        }
                        componentInChildren3.CheckHit(Vector3.zero, target, false, transform.GetComponent<Collider>());
                    }
                }
                if (ev.explosion)
                {
                    transform.SendMessage("Explosion", -1, SendMessageOptions.DontRequireReceiver);
                }
                if (ev.HitHead)
                {
                    transform.SendMessage("HitHead", SendMessageOptions.DontRequireReceiver);
                }
                if (ev.getStealthAttack)
                {
                    transform.SendMessage("getStealthAttack", SendMessageOptions.DontRequireReceiver);
                }
                if (ev.hitFallDown)
                {
                    global::mutantHitReceiver componentInChildren5 = transform.GetComponentInChildren<global::mutantHitReceiver>();
                    if (componentInChildren5)
                    {
                        componentInChildren5.sendHitFallDown(ev.Hit);
                    }
                }
                else
                {
                    transform.SendMessage("getAttacker", (ev.RaisedBy.UserData as BoltEntity).gameObject, SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("getAttackerType", ev.getAttackerType, SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("getAttackDirection", ev.getAttackDirection, SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("getCombo", ev.getCombo, SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("takeDamage", ev.takeDamage, SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("setSkinDamage", UnityEngine.Random.Range(0, 3), SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("ApplyAnimalSkinDamage", ev.getAttackDirection, SendMessageOptions.DontRequireReceiver);
                    transform.SendMessage("Hit", ev.Hit, SendMessageOptions.DontRequireReceiver);
                    if (ev.HitAxe)
                    {
                        transform.SendMessage("HitAxe", SendMessageOptions.DontRequireReceiver);
                    }
                    if (ev.Burn)
                    {
                        transform.SendMessage("Burn", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            finally
            {
                global::EnemyHealth.CurrentAttacker = null;
            }
        }
    }
}
