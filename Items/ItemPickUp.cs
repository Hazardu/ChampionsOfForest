using System;

using ChampionsOfForest.Effects.Sound_Effects;

using TheForest.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest
{
	public class ItemPickUp : MonoBehaviour
	{
		public ulong ID;
		public int amount;
		public Item item;
		public float lifetime = 900;	//in seconds

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
			PlayerDeath = 900,
			Effigy = 30,
		}

		private void Start()
		{
			if (mainCam == null)
			{
				mainCam = Camera.main;
			}

			if (amount == 0)
			{
				amount = item.Amount;
			}
			if (item.Amount < 1)
				item.Amount = 1;

			if (ModSettings.IsDedicated)
				return;
			rb = GetComponent<Rigidbody>();
			rb.drag = 2.1f;
			rb.angularDrag = 0.01f;
			rb.isKinematic = true;
			Invoke("UnlockPhysics", 1f);
			src = gameObject.AddComponent<AudioSource>();
			src.spatialBlend = 1f;
			src.maxDistance = 150f;
			src.volume = 3;
			src.clip = Res.ResourceLoader.instance.LoadedAudio[item.GetDropSoundID()];
			src.Play(100UL);
		}

		public void EnableDisplay()
		{
			displayTime = 1.5f;
		}

		public void UnlockPhysics()
		{
			rb.isKinematic = false;

			Vector3 randomv3 = new Vector3(Random.value, 0, Random.value);
			rb.AddTorque(randomv3 * 100, ForceMode.VelocityChange);
			rb.AddForce(randomv3 * 2, ForceMode.VelocityChange);
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
						GUI.DrawTexture(new Rect(r.x, r.y, r.width * lifetime / 60f, 14 * MainMenu.Instance.screenScale), Texture2D.whiteTexture);
					}
				}

				GUI.color = new Color(MainMenu.RarityColors[item.Rarity].r, MainMenu.RarityColors[item.Rarity].g, MainMenu.RarityColors[item.Rarity].b, displayTime);

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
					height = titleHeight + (lineheight * item.Stats.Count + 1 )
				};
				GUI.Box(bg, string.Empty);
				for (int i = 0; i < item.Stats.Count; i++)
				{
					ItemStat stat = item.Stats[i];
					double amount = stat.Amount;
					if (stat.DisplayAsPercent)
					{
						amount *= 100;
					}

					amount = Math.Round(amount, stat.RoundingCount);
					string statslabel = $" {stat.Name}";
					string statsvalue;

					if (stat.DisplayAsPercent)
					{
						statsvalue = amount.ToString("N" + stat.RoundingCount) + "% ";
					}
					else
					{
						statsvalue = amount.ToString("N" + stat.RoundingCount) + " ";
					}
					GUI.color = MainMenu.RarityColors[stat.Rarity];
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
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(4);
						w.Write(ID);
						w.Close();
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
					answerStream.Close();
				}
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
						using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
						{
							using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
							{
								w.Write(4);
								w.Write(ID);
								w.Close();
							}
							Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Others);
							answerStream.Close();
						}
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
							using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
							{
								using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
								{
									w.Write(4);
									w.Write(ID);
									w.Close();
								}
								Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
								answerStream.Close();
							}
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