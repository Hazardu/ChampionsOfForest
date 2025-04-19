using System;
using System.Collections;

using ChampionsOfForest.Effects.Sound_Effects;
using ChampionsOfForest.Network;
using ChampionsOfForest.Network.Commands;

using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Items.ItemDefinition;

using Random = UnityEngine.Random;

namespace ChampionsOfForest.Items
{
	public class ItemPickUp : MonoBehaviour
	{
		public ulong ID;
		public int amount;
		public Item item;
		public float lifetime = 900;	//in seconds
		private int rarity => (int) item.rarity;
		private string label;
		private Rigidbody rb;
		private float displayTime;
		private static Camera mainCam;
		private float constantViewTime;
		private AudioSource src;

		public enum DropSource
		{
			EnemyOnDeath = 120,
			PlayerInventory = 60,
			PlayerDeath = 2000,
			Effigy = 30,
		}

		private Vector3 randomAxis;


		private IEnumerator Start()
		{
			if (mainCam == null)
			{
				mainCam = Camera.main;
			}

			if (amount == 0)
			{
				amount = item.stackedAmount;
			}
			if (item.stackedAmount < 1)
				item.stackedAmount = 1;

			if (ModSettings.IsDedicated)
				yield break;

			rb = GetComponent<Rigidbody>();

			src = gameObject.AddComponent<AudioSource>();
			src.spatialBlend = 1f;
			src.maxDistance = 100f;
			src.volume = 1;
			src.clip = Res.ResourceLoader.instance.LoadedAudio[item.GetDropSoundID()];
			src.pitch = item.GetInvSoundPitch();
			src.Play(10UL);


			switch (rarity)
			{
				case 0:
				case 1:
				case 2:
					yield return CommonAnimation();
					break;
				case 3:
					yield return RareAnimation();
					break;
				case 4:
					yield return LegendaryAnimation();
					break;
				default:
					break;
			}
		}
		private IEnumerator CommonAnimation()
		{
			const float delayForStrongDrag = 2f;
			const float strongDrag = 15;
			const float delayForKinematic = 0.5f;
			const float initalLaunchForce = 2f;
			const float upwardLaunchForce = 4;

			rb.mass = 0.25f;
			rb.drag = 0.15f;
			rb.angularDrag = 0.03f;
			rb.isKinematic = false;
			float scale = rarity * 0.1f + 0.7f;
			transform.localScale = Vector3.one * scale;
			//wait one frame
			yield return null;

			Vector3 randomDirection = new Vector3(Random.value * 2 - 1, 0.25f, Random.value * 2 - 1).normalized;
			rb.AddTorque(randomDirection * 200, ForceMode.VelocityChange);
			rb.AddForce(Vector3.up * upwardLaunchForce + randomDirection * initalLaunchForce, ForceMode.VelocityChange);

			yield return null;

			while (rb.velocity.y > 0f)
			{
				yield return null;
			}
			yield return new WaitForSeconds(delayForStrongDrag);
			while (rb.velocity.y < 0f)
			{
				yield return null;
			}
			rb.drag = strongDrag;
			rb.angularDrag = strongDrag;
			yield return new WaitForSeconds(delayForKinematic);
			rb.isKinematic = true;

		}
		private IEnumerator RareAnimation()
		{
			const float delayForStrongDrag = 2f;
			const float strongDrag = 10;
			const float delayForKinematic = 1;
			const float initalLaunchForce = 2.5f;
			const float upwardLaunchForce = 5;

			rb.mass = 0.25f;
			rb.drag = 0.1f;
			rb.angularDrag = 0.01f;
			rb.isKinematic = false;

			//wait one frame
			yield return null;

			Vector3 randomDirection = new Vector3(Random.value * 2 - 1, 0.25f, Random.value * 2 - 1).normalized;
			rb.AddTorque(randomDirection * 300, ForceMode.VelocityChange);
			rb.AddForce(Vector3.up * upwardLaunchForce + randomDirection * initalLaunchForce, ForceMode.VelocityChange);

			yield return null;

			while (rb.velocity.y > 0f)
			{
				yield return null;
			}

			yield return new WaitForSeconds(delayForStrongDrag);
			while (rb.velocity.y < 0f)
			{
				yield return null;
			}
			rb.drag = strongDrag;
			rb.angularDrag = strongDrag;
			yield return new WaitForSeconds(delayForKinematic);
			rb.isKinematic = true;

		}
		private IEnumerator LegendaryAnimation()
		{
			const float delayForStrongDrag = 1.7f;
			const float strongDrag = 8;
			const float delayForKinematic = 0.5f;
			const float initalLaunchForce = 4;
			const float upwardLaunchForce = 12;
			const float downwardLaunchForce = 25;
			const float bumpForce = 4;

			rb.mass = 0.25f;
			rb.drag = 0.1f;
			rb.angularDrag = 0.01f;
			rb.isKinematic = false;

			//wait one frame
			yield return null;

			Vector3 randomDirection = new Vector3(Random.value * 2 - 1, 0.25f, Random.value * 2 - 1).normalized;
			randomAxis = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, Random.value * 2 - 1).normalized;
			rb.AddTorque(randomDirection * 400, ForceMode.VelocityChange);
			rb.AddForce(Vector3.up * upwardLaunchForce + randomDirection * initalLaunchForce, ForceMode.VelocityChange);

			yield return null;

			while (rb.velocity.y > 0f)
			{
				yield return null;
			}

			rb.AddForce(Vector3.down * downwardLaunchForce, ForceMode.VelocityChange);

			yield return new WaitForSeconds(delayForStrongDrag);

			while (rb.velocity.y < 0f)
			{
				yield return null;
			}
			rb.drag = strongDrag;
			rb.angularDrag = strongDrag;
			yield return new WaitForSeconds(delayForKinematic);
			rb.drag = 0.1f;
			rb.angularDrag = 0.01f;
			rb.AddForce(Vector3.up * bumpForce, ForceMode.VelocityChange);
			yield return null;
			while (rb.velocity.y > 0f)
			{
				yield return null;
			}
			rb.isKinematic = true;
		}


		public void EnableDisplay()
		{
			displayTime = 1.5f;
		}

		private void OnGUI()
		{
			if (mainCam == null)
			{
				mainCam = Camera.main;
			}
			if (displayTime > 0)
			{
				constantViewTime += Time.deltaTime;
				Vector3 pos = mainCam.WorldToScreenPoint(transform.position);
				pos.y = Screen.height - pos.y;
				if (pos.z < 0f)
				{
					return;
				}
				Rect r = new Rect(0, 0, 400 * MainMenu.Instance.screenScale, 200 * MainMenu.Instance.screenScale)
				{
					center = pos
				};
				label = item.name;
				label += " \n Level " + item.level;
				if (constantViewTime > 0.5f && amount > 1)
				{
					label += " \n x" + amount;
				}
				if (constantViewTime > 1f)
				{
					if (lifetime < 61)
					{

						Rect expirationRect = new Rect(r.x, r.y, r.width * lifetime / 60f, 14 * MainMenu.Instance.screenScale);
						GUI.DrawTexture(expirationRect, Texture2D.whiteTexture);
						GUI.Label(new Rect(r.x, r.y, r.width, 14 * MainMenu.Instance.screenScale), "disappearing in " + lifetime.ToString("N") + "s", new GUIStyle(GUI.skin.label) { font = MainMenu.Instance.mainFont, fontSize = Mathf.RoundToInt(11 * MainMenu.Instance.screenScale), alignment = TextAnchor.UpperCenter });
					}
				}

				GUI.color = new Color(item.RarityColor.r, item.RarityColor.g, item.RarityColor.b, displayTime);

				GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, font = MainMenu.Instance.mainFont, fontSize = Mathf.RoundToInt(40 * MainMenu.Instance.screenScale) };
				float titleHeight = style.CalcHeight(new GUIContent(label), r.width);
				style.margin = new RectOffset(10, 10, 10, 10);

				GUI.Label(r, label, style);
				//Item stats
				GUIStyle statStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft, font = MainMenu.Instance.secondaryFont, fontSize = Mathf.RoundToInt(16 * MainMenu.Instance.screenScale) };
				statStyle.margin = new RectOffset(10, 10, 10, 10);
				

				float lineheight = statStyle.CalcHeight(new GUIContent(" "), r.width);
				Rect bg = new Rect(r)
				{
					height = titleHeight + (lineheight * item.stats.Count + 1 )
				};
				GUI.Box(bg, string.Empty);
				for (int i = 0; i < item.stats.Count; i++)
				{
					ItemStat stat = item.stats[i];
					double amount = stat.amount;
					if (stat.displayAsPercent)
					{
						amount *= 100;
					}

					amount = Math.Round(amount, stat.roundingCount);
					string statslabel = $" {stat.name}";
					string statsvalue;

					if (stat.displayAsPercent)
					{
						statsvalue = amount.ToString("N" + stat.roundingCount) + "% ";
					}
					else
					{
						statsvalue = amount.ToString("N" + stat.roundingCount) + " ";
					}
					GUI.color = Color.white;
					//Name
					statStyle.alignment = TextAnchor.UpperLeft;

					GUI.Label(new Rect(r.x, r.y + titleHeight + (i * lineheight), r.width, r.height), statslabel, statStyle);
					//Value
					statStyle.alignment = TextAnchor.UpperRight;
					GUI.Label(new Rect(r.x, r.y + titleHeight + (i * lineheight), r.width, r.height), statsvalue, statStyle);

				}
				GUI.color = new Color(1, 1, 1, 1);
			}
			else
			{
				constantViewTime = 0;
			}
		}

		public void Remove()
		{
			Destroy(gameObject);
		}

		public void OnDestroy()
		{
			if((LocalPlayer.Transform.position-transform.position).sqrMagnitude < 150f)
				GlobalSFX.Play(GlobalSFX.SFX.Pickup);
		}

		private void Update()
		{
			if (amount <= 0)
			{
				PickUpManager.RemovePickup(ID);
				Destroy(gameObject);
			}
			if (displayTime > 0)
				displayTime -= Time.deltaTime;
			if (lifetime > 0)
				lifetime -= Time.deltaTime;
			else
			{
				COTFCommand<DestroyItemPickup>.Send(NetworkManager.Target.Others, new DestroyItemPickup(ID));
				PickUpManager.RemovePickup(ID);
				Destroy(gameObject);
			}
		}

		public bool PickUp()
		{
			COTFEvents.Instance.OnLootPickup.Invoke();

			if (item.PickUpAll)
			{
				if (!GameSetup.IsMpClient)
				{
					if (Player.Inventory.Instance.AddItem(item, amount))
					{
						COTFCommand<DestroyItemPickup>.Send(NetworkManager.Target.Others, new DestroyItemPickup(ID));
						PickUpManager.RemovePickup(ID);
						Destroy(gameObject);
						return true;
					}
				}
				else if (Player.Inventory.Instance.HasSpaceFor(item, amount))
				{
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(25);
							w.Write(ID);
							w.Write(amount);
							w.Write(ModReferences.ThisPlayerID);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
						answerStream.Close();
					}
				}
			}
			else
			{
				if (!GameSetup.IsMpClient)
				{
					if (Player.Inventory.Instance.AddItem(item))
					{
						amount--;
						if (amount <= 0)
						{
							COTFCommand<DestroyItemPickup>.Send(NetworkManager.Target.Others, new DestroyItemPickup(ID));
							PickUpManager.RemovePickup(ID);
							Destroy(gameObject);
						}
						return true;
					}
				}
				else if (Player.Inventory.Instance.HasSpaceFor(item))
				{
					using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
					{
						using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
						{
							w.Write(25);
							w.Write(ID);
							w.Write(1);
							w.Write(ModReferences.ThisPlayerID);
							w.Close();
						}
						Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.OnlyServer);
						answerStream.Close();
					}
				}
			}
			return false;
		}
	}
}