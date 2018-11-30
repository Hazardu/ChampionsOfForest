using UnityEngine;

namespace ChampionsOfForest
{
    public class ItemPickUp : MonoBehaviour
    {
        public int ID;
        public int amount;
        public Item item;

        private string label;
        private Rigidbody rb;
        private float DisplayTime;
        private static Camera mainCam;
        private float constantViewTime;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            rb.mass = 10;
            if (mainCam == null)
            {
                mainCam = Camera.main;
            }

            if (amount == 0)
            {
                amount = item.Amount;
            }
            item.Amount = 1;

        }

        public void EnableDisplay()
        {
            DisplayTime = 1;
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
                if (constantViewTime > 1)
                {
                    label += " \n x" + amount;
                }
                if (constantViewTime > 2)
                {
                    label += " \n Level " + item.level;
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

        public bool PickUp()
        {
            if (item.PickUpAll)
            {
                if (Player.Inventory.Instance.AddItem(item, amount))
                {
                    Network.NetworkManager.SendLine("RI" + ID + ";", Network.NetworkManager.Target.Everyone);
                    return true;

                }
            }
            else
            {
                if (Player.Inventory.Instance.AddItem(item))
                {
                    amount--;
                    if (amount <= 0)
                    {
                        Network.NetworkManager.SendLine("RI" + ID + ";", Network.NetworkManager.Target.Everyone);

                    }
                    return true;

                }
            }

            return false;
        }
    }
}
