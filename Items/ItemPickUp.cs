using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest
{
    public class ItemPickUp : MonoBehaviour
    {
        public ulong ID;
        public int amount;
        public Item item;

        private string label;
        private Rigidbody rb;
        private float DisplayTime;
        private static Camera mainCam;
        private float constantViewTime;
        private float lifetime = 600;
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
            item.Amount = 1;
            lifetime = 2500;
            if (ModSettings.IsDedicated) return;
            rb = GetComponent<Rigidbody>();
            rb.drag = 2.25f;
            rb.angularDrag = 0.1f;
            rb.isKinematic = true;
            Invoke("UnlockPhysics", 1f);
            lifetime = 300;
        }

        public void EnableDisplay()
        {
            DisplayTime = 1;
        }
        public void UnlockPhysics()
        {
            rb.isKinematic = false;
        }
        private void OnGUI()
        {
            if (mainCam == null)
            {
                mainCam = Camera.main;
            }
            if (DisplayTime > 0)
            {
                constantViewTime += Time.deltaTime;
                Vector3 pos = mainCam.WorldToScreenPoint(transform.position);
                pos.y = Screen.height - pos.y;

                Rect r = new Rect(0, 0, 400 * MainMenu.Instance.rr, 200 * MainMenu.Instance.rr)
                {
                    center = pos
                };
                label = item.name;
                if (constantViewTime > 0.5f)
                {
                    label += " \n x" + amount;
                }
                if (constantViewTime > 1f)
                {
                    label += " \n Level " + item.level;
                    if (lifetime < 61)
                    {
                        label += " \n Deleting in " + lifetime.ToString();
                    }
                }

                GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, font = MainMenu.Instance.MainFont, fontSize = Mathf.RoundToInt(40 * MainMenu.Instance.rr) };
                float height = style.CalcHeight(new GUIContent(label), r.width);
                style.margin = new RectOffset(10, 10, 10, 10);
                Rect bg = new Rect(r)
                {
                    height = height
                };
                GUI.color = new Color(MainMenu.RarityColors[item.Rarity].r, MainMenu.RarityColors[item.Rarity].g, MainMenu.RarityColors[item.Rarity].b, DisplayTime);
                GUI.Box(bg, string.Empty);
                GUI.Label(r, label, style);
                GUI.color = new Color(1, 1, 1, 1);
                DisplayTime -= Time.deltaTime;

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

        private void Update()
        {
            if (amount <= 0)
            {
                PickUpManager.RemovePickup(ID);
                Destroy(gameObject);
            }
            if (lifetime > 0)
            {
                lifetime -= Time.deltaTime;

            }
            else
            {
                using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
                {
                    using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
                    {
                        w.Write(4);
                        w.Write(ID);
                    }
                    ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
                }
                PickUpManager.RemovePickup(ID);
                Destroy(gameObject);
            }
        }
        public bool PickUp()
        {
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
                            }
                            ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
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
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
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
                                }
                                ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
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
                        }
                        ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.OnlyServer);
                    }
                }
            }
            return false;
        }
    }
}
