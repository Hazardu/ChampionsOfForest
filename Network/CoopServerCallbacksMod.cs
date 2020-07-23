using Bolt;
using TheForest.Utils;
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
                var packed = ev.Target.networkId.PackedValue;
                if (EnemyManager.hostDictionary.ContainsKey(packed))
                {
                    var enemy = EnemyManager.hostDictionary[packed];
                    if (ev.explosion)
                    {
                        enemy._Health.Explosion(-1);
                    }
                    if (ev.getStealthAttack && ev.HitAxe)
                    {
                        //ghost hit 
                        //sure replaces a the stealth hits with an axe to not deal the bonus 100 damage points, but who cares, noone does stealth hits with axes. bows FTW
                        enemy.HitPhysicalSilent(ev.Hit);
                    }
                    else
                    {
                        if (ev.Hit > 0)
                        {
                            //just in case i ever need this
                            //this is how to get the player object which raised the event (ev.RaisedBy.UserData as BoltEntity)
                            enemy._Health.getAttackDirection(ev.getAttackerType);
                            var attackerGO = (ev.RaisedBy.UserData as BoltEntity).gameObject;
                            enemy.setup.search.switchToNewTarget(attackerGO);
                            enemy.setup.hitReceiver.getAttackDirection(ev.getAttackDirection);
                            enemy.setup.hitReceiver.getCombo(ev.getCombo);
                            enemy._Health.takeDamage(ev.takeDamage);
                            enemy._Health.setSkinDamage(1);
                            enemy._Health.Hit(ev.Hit);
                            if (ev.Burn)
                            {
                                enemy._Health.Burn();
                            }
                        }
                        else
                        {
                            enemy.ReduceArmor(ev.Hit);

                        }
                    }
                    return;
                }

                //Fuck all of this spaghetti below
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
            if (evnt.Sender == ChatBoxMod.ModNetworkID) { return; }

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


        public override void EntityDetached(BoltEntity entity)
        {
            if (entity.StateIs<IPlayerState>() && TheForest.Utils.Scene.SceneTracker &&GameSetup.IsMpServer)
            {
                ModdedPlayer.instance.SendLeaveMessage(entity.GetState<IPlayerState>().name);
            }
            base.EntityDetached(entity);
        }
        public override void EntityReceived(BoltEntity entity)
        {
            if (entity.StateIs<IPlayerState>() && GameSetup.IsMpServer)
                NetworkManager.SendText("IIA champion approaches... \n" + entity.GetState<IPlayerState>().name, NetworkManager.Target.Everyone);

            base.EntityReceived(entity);
        }


        public override void Disconnected(BoltConnection connection)
        {
            if (BoltNetwork.isClient)
            {
                ModAPI.Console.Write("Saving client data to avoid item duping");
                Serializer.EmergencySave();
            }
            base.Disconnected(connection);
        }
    }
    public class BoltConnectionEx : BoltConnection
    {
        public BoltConnectionEx(UdpKit.UdpConnection c) : base(c)
        {
        
        }
        public override void Disconnect()
        {
            Serializer.EmergencySave();
            base.Disconnect();
        }
    }

}
