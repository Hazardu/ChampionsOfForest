using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class MarkPostion : MarkObject
	{
		public MarkPostion(Vector3 pos)
		{
			timestamp = Time.time + DURATION;
			pingType = PingType.Location;
			this.position = pos;
		}

		public Vector3 position;

		public bool Outdated()
		{
			return timestamp < Time.time;
		}

		public void Draw()
		{
			Vector3 heading = position - LocalPlayer.Transform.position;

			if (Vector3.Dot(Cam.transform.forward, heading) > 0)
			{
				float distance = Vector3.Distance(Camera.main.transform.position, position);
				Vector3 pos = Camera.main.WorldToScreenPoint(position);
				pos.y = Screen.height - pos.y;
				float size = Mathf.Clamp(700 / distance, 16, 50);
				size *= MainMenu.Instance.screenScale;

				Rect r = new Rect(0, 0, 3.35f * size, size)
				{
					center = pos
				};

				GUI.Label(r, (distance/3.28f).ToString("N0") + 'm', new GUIStyle(GUI.skin.label) { fontSize = ((int)size-2), font = MainMenu.Instance.mainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
				r.y += size + 5;
				GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(173));
			}
		}
	}
}