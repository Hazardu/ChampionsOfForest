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
        private float lifetime = 300;
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
            rb.drag = 1.5f;
            rb.angularDrag = 0.4f;
            rb.isKinematic = true;
            Invoke("UnlockPhysics", 0.5f);
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
                Network.NetworkManager.SendLine("RI" + ID + ";", Network.NetworkManager.Target.Everyone);
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
                        Network.NetworkManager.SendLine("RI" + ID + ";", Network.NetworkManager.Target.Everyone);
                        PickUpManager.RemovePickup(ID);
                        Destroy(gameObject);
                        return true;
                    }
                }
                else if (Player.Inventory.Instance.HasSpaceFor(item, amount))
                {
                    Network.NetworkManager.SendLine("AF" + ID + ";"+amount+";" + ModReferences.ThisPlayerPacked + ";", Network.NetworkManager.Target.OnlyServer); //asks for the item
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
                            Network.NetworkManager.SendLine("RI" + ID + ";", Network.NetworkManager.Target.Everyone);
                            PickUpManager.RemovePickup(ID);
                            Destroy(gameObject);
                        }
                        return true;

                    }
                }
                else if (Player.Inventory.Instance.HasSpaceFor(item))
                {
                    Network.NetworkManager.SendLine("AF" + ID + ";" + 1 + ";" + ModReferences.ThisPlayerPacked + ";", Network.NetworkManager.Target.OnlyServer); //asks for the item

                }
            }
            return false;
        }
    }
}
