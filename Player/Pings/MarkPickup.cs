using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class MarkPickup : MarkObject
	{
		public MarkPickup(Transform t, string pingName, int rarity)
		{
			timestamp = Time.time + DURATION;
			pingType = PingType.Item;
			transform = t;
			name = pingName;
			this.rarity = rarity;
		}

		public Transform transform;
		private readonly string name;
		private readonly int rarity;

		public bool Outdated()
		{
			return timestamp < Time.time;
		}

		public void Draw()
		{
			Vector3 dir = transform.position - LocalPlayer.Transform.position;
			float sqrMag = dir.sqrMagnitude;
			if (sqrMag <= MAXRANGE_SQUARED)
			{
				if (Vector3.Dot(Cam.transform.forward, dir) > 0)
				{
					float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
					Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
					pos.y = Screen.height - pos.y;
					float size = Mathf.Clamp(500 / distance, 16, 50);
					size *= MainMenu.Instance.screenScale;

					Rect r = new Rect(0, 0, 1.2f * size, 2.3f * size)
					{
						center = pos
					};
					r.y -= size * 2.4f;

					GUI.color = MainMenu.RarityColors[rarity];
					GUI.Label(r, name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), font = MainMenu.Instance.mainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
					GUI.color = Color.white;
					r.y += size + 10;
					GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(174));
				}
			}
		}
	}
}