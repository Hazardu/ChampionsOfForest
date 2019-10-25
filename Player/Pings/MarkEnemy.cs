using UnityEngine;
using TheForest.Utils;

namespace ChampionsOfForest
{
    public class MarkEnemy : MarkObject
    {
        public MarkEnemy(Transform t, string name, bool elite)
        {
            Timestamp = Time.time + Duration;
            pingType = PingType.Enemy;
            transform = t;
            Name = name;
            Elite = elite;
        }


        public Transform transform;
        private string Name;
        private bool Elite;
        const float MaxRange = 200 * 200;   //200 meters
        const float Duration = 300;     // 5 min
        public bool Outdated()
        {
            return Timestamp  < Time.time;
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
                    float size = Mathf.Clamp(500 / distance, 8, 50);
                    size *= ChampionsOfForest.MainMenu.Instance.rr;
                    if (Elite) size *= 1.1f;
                    Rect r = new Rect(0, 0, 3.35f * size, size)
                    {
                        center = pos
                    };
                    if (Elite)
                    {
                        GUI.color = Color.red;
                        GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), fontStyle= FontStyle.Bold, font = MainMenu.Instance.MainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
                        GUI.color = Color.white;
                        r.y += size;
                        GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(171));
                    }
                    else
                    {
                        GUI.color = Color.red;
                        GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), font = MainMenu.Instance.MainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
                        GUI.color = Color.white;
                        r.y += size;
                        GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(172));
                    }
                }
            }
        }
    }
}

