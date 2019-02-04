using Bolt;
using UnityEngine;

namespace ChampionsOfForest.Network
{
    internal class CoopServerCallbacksMod : CoopServerCallbacks
    {

        public override void OnEvent(PlayerHitEnemy ev)
        {
            //this needed to be changed.
            //the command would send damage using hitReal - pure damage.
            //this made clinets attack ignore armor and deal too much dmg
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

        public override void OnEvent(ChatEvent evnt)
        {

            //the host would resend all commands to the clients. 
            //this is okay for chat messages
            //but with commands sent directly there is no need. This adds unnecessary latency.
            //additionally all events like experience after a kill would be proc twice for clients.


            if (!this.ValidateSender(evnt, global::SenderTypes.Any))
            {
                return;
            }
            if (evnt.Sender == ChatBoxMod.ModNetwokrID) { return; }

            for (int i = 0; i < TheForest.Utils.Scene.SceneTracker.allPlayerEntities.Count; i++)
            {
                if (TheForest.Utils.Scene.SceneTracker.allPlayerEntities[i].source == evnt.RaisedBy)
                {
                    if (TheForest.Utils.Scene.SceneTracker.allPlayerEntities[i].networkId == evnt.Sender)
                    {
                        ChatEvent chatEvent = ChatEvent.Create(GlobalTargets.AllClients);
                        chatEvent.Sender = evnt.Sender;
                        chatEvent.Message = evnt.Message;
                        chatEvent.Send();
                    }
                    return;
                }
            }
            if (BoltNetwork.isServer && evnt.RaisedBy == null)
            {
                ChatEvent chatEvent2 = ChatEvent.Create(GlobalTargets.AllClients);
                chatEvent2.Sender = evnt.Sender;
                chatEvent2.Message = evnt.Message;
                chatEvent2.Send();
            }
        }
    }
}
