using System;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest
{
	public class MarkEnemy : MarkObject
	{
		public MarkEnemy(Transform t, string enemyName, bool is_elite, BoltEntity entity = null)
		{
			timestamp = Time.time + DURATION;
			pingType = PingType.Enemy;
			transform = t;
			Name = enemyName;
			isElite = is_elite;
			offset = Vector3.up * (6f + transform.lossyScale.y);
			this.entity = entity;
		}

		public Transform transform;
		private BoltEntity entity;
		private string Name;
		private bool isElite;
		private Vector3 offset;
		public bool Outdated()
		{
			return timestamp < Time.time;
		}
		static Texture2D bgTex, arTex, hpTex;
		public static void AssignTextures()
		{
			bgTex = new Texture2D(1, 1);
			bgTex.SetPixel(0, 0, new Color(0.105f, 0.129f, 0.156f, 0.66f));
			bgTex.Apply();

			arTex = new Texture2D(1, 1);
			arTex.SetPixel(0, 0, new Color(0.819f, 0.803f, 0.717f));
			arTex.Apply();

			hpTex = new Texture2D(1, 1);
			hpTex.SetPixel(0, 0, new Color(0.972f, 0.286f, 0.341f));
			hpTex.Apply();

		}
		private void DrawHealthBar(ref Rect pos)
		{
			ClientEnemyProgression cp = entity != null ? EnemyManager.GetCP(entity) : EnemyManager.GetCP(transform);
			float percentageHP = Mathf.Clamp01(cp.Health / cp.MaxHealth);
			float percentageAR = Mathf.Clamp01(1.0f-(cp.ArmorReduction/cp.Armor));
			Rect bg = new Rect(pos.x, pos.y, pos.width, 15f * MainMenu.Instance.screenScale);
			Rect ar = new Rect(bg); 
			ar.width *= percentageAR;
			ar.height = 3f * MainMenu.Instance.screenScale;
			Rect hp = new Rect(bg);
			hp.width *= percentageHP;
			hp.y += ar.height;
			hp.height -= ar.height;
			pos.y += bg.height - 6f * MainMenu.Instance.screenScale;
			
			GUI.DrawTexture(bg,	bgTex);
			GUI.DrawTexture(ar,	arTex);
			GUI.DrawTexture(hp,	hpTex);
		}


		public void Draw()
		{
			try
			{
				Vector3 tPos = transform.position + offset;
				Vector3 heading = tPos - LocalPlayer.Transform.position;
				float sqrMag = (heading).sqrMagnitude;
				if (sqrMag <= MAXRANGE_SQUARED)
				{
					if (Vector3.Dot(Cam.transform.forward, heading) > 0)
					{
						float distance = Vector3.Distance(Camera.main.transform.position, tPos);
						Vector3 pos = Camera.main.WorldToScreenPoint(tPos);
						pos.y = Screen.height - pos.y;
						float size = Mathf.Clamp(700 / distance, 14, 50) / 1.3f;
						size *= MainMenu.Instance.screenScale;


						Rect r = new Rect(0, 0, 3.35f * size, size)
						{
							center = pos
						};
						r.y -= size / 2;
						if (isElite)
						{
							GUI.color = Color.red;
							GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), fontStyle = FontStyle.Bold, font = MainMenu.Instance.mainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
							GUI.color = Color.white;
							r.y += size;
							DrawHealthBar(ref r);
							GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(171));
						}
						else
						{
							GUI.color = Color.red;
							GUI.Label(r, Name, new GUIStyle(GUI.skin.label) { fontSize = ((int)size), font = MainMenu.Instance.mainFont, alignment = TextAnchor.UpperCenter, wordWrap = false, clipping = TextClipping.Overflow });
							GUI.color = Color.white;
							r.y += size + 5;
							DrawHealthBar(ref r);
							GUI.DrawTexture(r, Res.ResourceLoader.GetTexture(172));
						}
					}
				}
			}
			catch (Exception e)
			{
				Debug.Log(e.ToString());
				//timestamp = 0;
			}
		}
	}
}