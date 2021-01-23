using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

//More health
//More damage
//As if the worm was weak in the first place
namespace ChampionsOfForest.Enemies
{
	public class WormHealthMod : wormHealth
	{
		public void HitMagic(float damage)
		{
			base.Hit(damage>int.MaxValue/2? int.MaxValue/2:(int)damage);
		}

		//public override void Hit(int damage)
		//{
		//   // damage = Mathf.Min(damage, 7);
		//    base.Hit(damage);
		//}
	}

	public class WormHiveControllerMod : wormHiveController
	{
		protected override void Start()
		{
			base.Start();
			maxRespawnAmount = 800000000;
		}

		protected override void Update()
		{
			this.activeWormWalkers.RemoveAll((GameObject o) => o == null);
			this.activeWormTrees.RemoveAll((GameObject o) => o == null);
			this.activeWormSingle.RemoveAll((GameObject o) => o == null);
			this.activeWormAngels.RemoveAll((GameObject o) => o == null);
			if (this.activeWormWalkers.Count > 0 || this.activeWormAngels.Count > 0 || this.activeWormTrees.Count > 0)
			{
				this.anyFormSpawned = true;
			}
			else
			{
				this.anyFormSpawned = false;
			}
			if (this.activeWormSingle.Count == 0 && this.init)
			{
				if (GameSetup.IsMpServer || GameSetup.IsSinglePlayer)
				{
					long Exp;
					switch (ModSettings.difficulty)
					{
						case ModSettings.Difficulty.Easy:
							Exp = 5000;
							break;

						case ModSettings.Difficulty.Veteran:
							Exp = 20000;
							break;

						case ModSettings.Difficulty.Elite:
							Exp = 100000;
							break;

						case ModSettings.Difficulty.Master:
							Exp = 3000000;
							break;

						case ModSettings.Difficulty.Challenge1:
							Exp = 50000000;
							break;

						case ModSettings.Difficulty.Challenge2:
							Exp = 100000000;
							break;

						case ModSettings.Difficulty.Challenge3:
							Exp = 500000000;
							break;

						case ModSettings.Difficulty.Challenge4:
							Exp = 1000000000;
							break;

						case ModSettings.Difficulty.Challenge5:
							Exp = 5000000000;
							break;

						default:
							Exp = 10000000000;
							break;
					}
					if (GameSetup.IsMpServer)
					{
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(10);
								w.Write(Exp);
								w.Close();
							}
							Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
							answerStream.Close();
						}
					}
					else
					{
						ModdedPlayer.instance.AddKillExperience(Exp);
					}
					int itemCount = UnityEngine.Random.Range(15, 26);
					for (int i = 0; i < itemCount; i++)
					{
						Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(Exp), LocalPlayer.Transform.position + Vector3.up * 2);
					}
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	internal class ExplodeMod : Explode
	{
		protected override void RunExplode()
		{
			Vector3 position = base.transform.position;
			Collider[] array = Physics.OverlapSphere(position, this.radius);
			int num = 0;
			int num2 = 0;
			foreach (Collider collider in array)
			{
				bool flag = collider.CompareTag("Fish");
				bool flag2 = !flag && (collider.CompareTag("Tree") || collider.CompareTag("MidTree"));
				if (flag)
				{
					num++;
				}
				else if (flag2)
				{
					num2++;
				}
				if (!this.wormDamageSetup || !collider.GetComponent<global::wormHitReceiver>())
				{
					
					if (collider.transform.root == LocalPlayer.Transform.root)
					{
						int damage = 85;
						switch (ModSettings.difficulty)
						{
							case ModSettings.Difficulty.Veteran:
								damage = 150;
								break;

							case ModSettings.Difficulty.Elite:
								damage = 230;
								break;

							case ModSettings.Difficulty.Master:
								damage = 360;
								break;

							case ModSettings.Difficulty.Challenge1:
								damage = 1500;
								break;

							case ModSettings.Difficulty.Challenge2:
								damage = 5000;
								break;

							case ModSettings.Difficulty.Challenge3:
								damage = 12000;
								break;

							case ModSettings.Difficulty.Challenge4:
								damage = 20000;
								break;

							case ModSettings.Difficulty.Challenge5:
								damage = 30000;
								break;

							case ModSettings.Difficulty.Challenge6:
							case ModSettings.Difficulty.Hell:
								damage = 40000;
								break;
						}
						if (ModdedPlayer.Stats.i_EruptionBow)
							damage /= 10;
						if(ModSettings.FriendlyFire || this.wormDamageSetup) 
							LocalPlayer.Stats.Hit(damage, false);
					}
					if (BoltNetwork.isClient)
					{
						if (collider.CompareTag("playerHitDetect"))
						{
							float num3 = Vector3.Distance(base.transform.position, collider.transform.position);
							collider.SendMessageUpwards("Explosion", num3, SendMessageOptions.DontRequireReceiver);
							collider.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
						}
						else if (collider.CompareTag("SmallTree") || flag2 || collider.CompareTag("BreakableWood") || collider.CompareTag("BreakableRock") || flag)
						{
							float num4 = Vector3.Distance(base.transform.position, collider.transform.position);
							if (collider.CompareTag("lb_bird") || flag)
							{
								collider.gameObject.SendMessage("Explosion", num4, SendMessageOptions.DontRequireReceiver);
							}
							else
							{
								collider.gameObject.SendMessageUpwards("Explosion", num4, SendMessageOptions.DontRequireReceiver);
							}
							collider.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
						}
						else if (collider.CompareTag("structure"))
						{
							float distance = Vector3.Distance(base.transform.position, collider.transform.position);
							collider.gameObject.SendMessage("OnExplode", new global::Explode.Data
							{
								distance = distance,
								explode = this
							}, SendMessageOptions.DontRequireReceiver);
						}
						else if (collider.CompareTag("animalCollide"))
						{
							collider.gameObject.SendMessageUpwards("Explosion", SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						if (collider.CompareTag("enemyCollide") || collider.CompareTag("animalCollide") || collider.CompareTag("lb_bird") || collider.CompareTag("playerHitDetect") || collider.CompareTag("structure") || collider.CompareTag("SLTier1") || collider.CompareTag("SLTier2") || collider.CompareTag("SLTier3") || flag2 || collider.CompareTag("SmallTree") || collider.CompareTag("BreakableWood") || collider.CompareTag("BreakableRock") || flag || collider.CompareTag("jumpObject") || collider.CompareTag("UnderfootWood") || collider.CompareTag("UnderfootRock") || collider.CompareTag("Target") || collider.CompareTag("dummyExplode"))
						{
							float num5 = Vector3.Distance(base.transform.position, collider.transform.position);
							if (collider.CompareTag("lb_bird") || flag)
							{
								collider.gameObject.SendMessage("Explosion", num5, SendMessageOptions.DontRequireReceiver);
							}
							else
							{
								collider.gameObject.SendMessageUpwards("Explosion", num5, SendMessageOptions.DontRequireReceiver);
							}
							collider.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
							if (num5 < this.radius)
							{
								collider.gameObject.SendMessage("OnExplode", new global::Explode.Data
								{
									distance = num5,
									explode = this
								}, SendMessageOptions.DontRequireReceiver);
							}
						}
						else if (collider.CompareTag("TripWireTrigger"))
						{
							collider.SendMessage("OnTripped", SendMessageOptions.DontRequireReceiver);
						}
						if (collider && collider.GetComponent<Rigidbody>())
						{
							if (!collider.gameObject.CompareTag("Tree"))
							{
								float num6 = 10000f;
								if (collider.GetComponent<global::logChecker>())
								{
									num6 *= 5.5f;
								}
								if (collider.CompareTag("Fish"))
								{
									num6 = 1850f;
								}
								if (!collider.CompareTag("Player") && !collider.CompareTag("PlayerNet"))
								{
									collider.GetComponent<Rigidbody>().AddExplosionForce(num6, position, this.radius, 3f, ForceMode.Force);
								}
							}
						}
					}
				}
			}
			if (num > 0)
			{
				TheForest.Tools.EventRegistry.Achievements.Publish(TheForest.Tools.TfEvent.Achievements.FishDynamited, num);
			}
			if (num2 > 0)
			{
				TheForest.Tools.EventRegistry.Achievements.Publish(TheForest.Tools.TfEvent.Achievements.TreeDynamited, num2);
			}
			if (TheForest.Utils.LocalPlayer.GameObject)
			{
				float num7 = Vector3.Distance(TheForest.Utils.LocalPlayer.Transform.position, base.transform.position);
				TheForest.Utils.LocalPlayer.GameObject.SendMessage("enableExplodeShake", num7, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}