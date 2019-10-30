using UnityEngine;
using TheForest.Utils;

namespace ChampionsOfForest
{
    public class MarkPickup : MarkObject
    {
        public MarkPickup(Transform t, string name, int rarity)
        {
            Timestamp = Time.time + Duration;
            pingType = PingType.Item;
            transform = t;
            Name = name;
            Rarity = rarity;
        }


        public Transform transform;
        private string Name;
        private int Rarity;
        const float MaxRange = 100 * 100;   //100 meters
        const float Duration = 60;     // 1 min
        public bool Outdated()
        {
            return Timestamp < Time.time;
        }
        public void Draw()
        {
            Vector3 heading = transform.position - LocalPlayer.Transform.position;
            float sqrMag = (heading).sqrMagnitude;
            if (sqrMag <= MaxRange)
            {
                if (Vector3.Dot(Cam.transform.forward, heading) > 0)
                {
                    float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
                    Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                    pos.y = Screen.height - pos.y;
                    float size = Mathf.Clamp(500 / distance, 16, 50);
                    size *= ChampionsOfForest.MainMenu.Instance.rr;
                    
                    Rect r = new Rect(0, 0, 1.3f * size,2.4f* size)
                    {
                        center = pos
                    };
                  
                        GUI.color = MainMenu.RarityColors[Rarity];
                        GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), font = MainMenu.Instance.MainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
                        GUI.color = Color.white;
                        r.y += size;
                        GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(174));
                    
                }
            }
        }
    }
}

