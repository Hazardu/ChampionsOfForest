using System;

using TheForest.Utils;

using UnityEngine;

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
		private const float MaxRange = 1000 * 1000;   //1000 meters
		private const float Duration = 300;     // 5 min

		public bool Outdated()
		{
			return Timestamp < Time.time;
		}

		public void Draw()
		{
			try
			{


				if (transform == null)
					return;
				Vector3 tPos = transform.position + Vector3.up * 3f;
				Vector3 heading = tPos - LocalPlayer.Transform.position;
				float sqrMag = (heading).sqrMagnitude;
				if (sqrMag <= MaxRange)
				{
					if (Vector3.Dot(Cam.transform.forward, heading) > 0)
					{
						float distance = Vector3.Distance(Camera.main.transform.position, tPos);
						Vector3 pos = Camera.main.WorldToScreenPoint(tPos);
						pos.y = Screen.height - pos.y;
						float size = Mathf.Clamp(700 / distance, 14, 50) / 1.3f;
						size *= ChampionsOfForest.MainMenu.Instance.screenScale;
						if (Elite)
							size *= 1.1f;
						Rect r = new Rect(0, 0, 3.35f * size, size)
						{
							center = pos
						};
						r.y -= size / 2;
						if (Elite)
						{
							GUI.color = Color.red;
							GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), fontStyle = FontStyle.Bold, font = MainMenu.Instance.mainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
							GUI.color = Color.white;
							r.y += size;
							GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(171));
						}
						else
						{
							GUI.color = Color.red;
							GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), font = MainMenu.Instance.mainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
							GUI.color = Color.white;
							r.y += size + 5;
							GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(172));
						}
					}
				}

			}
			catch (Exception e)
			{
				Timestamp = 0;
			}
		}
	}
}