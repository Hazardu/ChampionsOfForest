using UnityEngine;

namespace ChampionsOfForest
{
    public class ClinetItemPicker : MonoBehaviour
    {
        public static Transform cam;
        public static float radius = 6.5f;

        #region Instance
        public static ClinetItemPicker Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        #endregion

        private void Update()
        {
            if (cam == null)            //waits for main cam to be created. Its not there instantly after loading into the scene
            {
                if (Camera.main.transform != null)
                {
                    cam = Camera.main.transform;
                }

                return;
            }
            RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, radius);
            for (int i = 0; i < hits.Length; i++)
            {
                ItemPickUp pu = hits[i].transform.root.GetComponent<ItemPickUp>();
                if (pu != null)
                {
                    pu.EnableDisplay();
                    if (ModAPI.Input.GetButtonDown("ItemPickUp"))
                    {
                        pu.PickUp();
                    }
                    return;
                }
            }
        }
    }
}

