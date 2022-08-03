using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ChampionsOfForest.Localization;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Player.BuffDB;

using ResourceLoader = ChampionsOfForest.Res.ResourceLoader;

namespace ChampionsOfForest
{
	public partial class MainMenu : MonoBehaviour
	{
		//HUD variables
		private float ScanTime = 0;

		private float ScanRotation = 0;
		private Transform scannedTransform;
		private BoltEntity scannedEntity;
		private bool HideHud = false;
		private float ProgressBarAmount = 0;
		private Rect HUDenergyLabelRect;
		private Rect HUDHealthLabelRect;
		private Rect HUDShieldLabelRect;
		private GUIStyle HUDStatStyle;

		private Texture2D _SpellFrame;
		private Texture2D _SpellCoolDownFill;
		private Texture2D _expBarFrameTex;
		private Texture2D _combatDurationTex;
		private Texture2D _expBarFillTex;
		private Texture2D _expBarBackgroundTex;
		private Texture2D _SpellBG;

		internal int LevelsToGain = 0;

		private const float BuffSize = 50;

		private void DrawBuff(float x, float y, Texture2D tex, string amount, string time, bool isPositive, float durationInSeconds)
		{
			Rect r = new Rect(x * screenScale, y * screenScale, BuffSize * screenScale, BuffSize * screenScale);

			GUI.DrawTexture(r, ResourceLoader.GetTexture(143));
			if (isPositive)
			{
				GUI.color = new Color(1, 1, 1, 0.25f + 0.5f * Mathf.Sin(durationInSeconds * 3));
				GUI.DrawTexture(r, ResourceLoader.GetTexture(145));
				GUI.color = new Color(1, 1, 1, 1);
			}
			else
			{
				GUI.color = new Color(1, 0, 0, 0.25f + 0.5f * Mathf.Sin(durationInSeconds * 3));
				GUI.DrawTexture(r, ResourceLoader.GetTexture(145));
				GUI.color = new Color(1, 1, 1, 1);
			}

			GUI.DrawTexture(r, tex);

			GUI.DrawTexture(r, ResourceLoader.GetTexture(144));
			GUI.Label(r, amount, new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(16 * screenScale), font = mainFont, fontStyle = FontStyle.Italic, normal = new GUIStyleState() { textColor = Color.red }, richText = true, clipping = TextClipping.Overflow, alignment = TextAnchor.UpperLeft });
			GUI.Label(r, time, new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(15 * screenScale), font = mainFont, fontStyle = FontStyle.Normal, richText = true, clipping = TextClipping.Overflow, alignment = TextAnchor.LowerRight });
		}

		//Hit markers
		public static void CreateHitMarker(float dmg, Vector3 p, Color color)
		{
			var marker = Instance.hitMarkers.Where(x => x.Player == false && (x.worldPosition - p).sqrMagnitude < 4 && x.color == color);
			if (marker.Count() > 0)
			{
				var m = marker.First();
				m.lifetime = 4;
				m.dmg += dmg;
				m.txt = m.dmg.ToString("N0");
				m.worldPosition = p;
			}
			else
			{
				new HitMarker(dmg, p, color);
			}
		}

		public readonly List<HitMarker> hitMarkers = new List<HitMarker>();

		public class HitMarker
		{
			public Color color;
			public const float StartLifetime = 5; //5
			public float dmg;
			public string txt;
			public Vector3 worldPosition;
			public float lifetime;
			public bool Player;

			public void Delete(int i)
			{
				Instance.hitMarkers.RemoveAt(i);
			}

			public HitMarker(float t, Vector3 p, Color c)
			{
				color = c;
				dmg = t;
				txt = t.ToString("N0");
				worldPosition = p;
				lifetime = StartLifetime;
				Instance.hitMarkers.Add(this);
			}

			public HitMarker(int t, Vector3 p, bool Player)
			{
				txt = t.ToString("N0");
				worldPosition = p;
				lifetime = StartLifetime;
				this.Player = Player;
				if (Player)
					color = new Color(0, 0.75f, 0, 0.75f);
				Instance.hitMarkers.Add(this);
			}
		}

		#region HUDMethods

		private float SmoothSlideIn(float timeAppearStart)
		{
			const float appearanceTime = 0.8f;
			float tEnd = appearanceTime + timeAppearStart;
			if (ScanTime > tEnd)
				return 0f;
			float t = (ScanTime - appearanceTime) / appearanceTime;
			t = 1.0f - t;
			if (t < 0)
				return 0f;
			t = t * t * t*t;

			return t * -250f * screenScale;
		}

		private void DrawHUD()
		{
			if (HideHud)
			{
				return;
			}
			GUI.color = new Color(1, 0.5f, 0.7f, 0.5f);
			GUIStyle HitmarkerStyle = new GUIStyle(GUI.skin.label) { font = secondaryFont, clipping = TextClipping.Overflow, wordWrap = true, alignment = TextAnchor.MiddleCenter };
			for (int i = 0; i < hitMarkers.Count; i++)
			{
				hitMarkers[i].lifetime -= Time.unscaledDeltaTime * 2;
				if (hitMarkers[i].lifetime < 0 || i - hitMarkers.Count + 10 < 0)
				{
					hitMarkers[i].Delete(i);
				}
				else
				{
					if (hitMarkers[i].Player)
					{
						GUI.color = new Color(0.15f, 1, 0.15f, 0.5f);
					}
					else
					{
						GUI.color = hitMarkers[i].color;
					}

					float distance = Vector3.Distance(Camera.main.transform.position, hitMarkers[i].worldPosition);
					Vector3 pos = Camera.main.WorldToScreenPoint(hitMarkers[i].worldPosition);
					pos.y = Screen.height - pos.y;
					float size = Mathf.Clamp(600 / distance, 10, 50);
					size *= screenScale;
					Rect r = new Rect(0, 0, 400, size)
					{
						center = pos
					};
					//do not draw labels behind you!
					if (pos.z < 0f)
					{
						continue;
					}
					GUI.Label(r, hitMarkers[i].txt, new GUIStyle(HitmarkerStyle) { fontSize = Mathf.RoundToInt(size) });
					
					
				}
			}
			GUI.color = Color.white;


			//buffs 
			float BuffOffsetX = 0;
			float BuffOffsetY = 1080 - BuffSize;
			const float MaxX = 540;

			if (ModdedPlayer.Stats.rooted)
			{
				TimeSpan span = TimeSpan.FromSeconds(ModdedPlayer.instance.RootDuration);
				DrawBuff(BuffOffsetX, BuffOffsetY, ResourceLoader.GetTexture(162), "", (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()), false, ModdedPlayer.instance.RootDuration);
				BuffOffsetX += BuffSize;
				if (BuffOffsetX > MaxX)
				{
					BuffOffsetX = 0;
					BuffOffsetY -= BuffSize;
				}
			}
			if (ModdedPlayer.Stats.stunned)
			{
				TimeSpan span = TimeSpan.FromSeconds(ModdedPlayer.instance.StunDuration);
				DrawBuff(BuffOffsetX, BuffOffsetY, ResourceLoader.GetTexture(163), "", (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()), false, ModdedPlayer.instance.StunDuration);
				BuffOffsetX += BuffSize;
				if (BuffOffsetX > MaxX)
				{
					BuffOffsetX = 0;
					BuffOffsetY -= BuffSize;
				}
			}
			foreach (KeyValuePair<int, Buff> buff in activeBuffs)
			{
				TimeSpan span = TimeSpan.FromSeconds(buff.Value.duration);
				string valueText = "";
				if (buff.Value.DisplayAmount)
				{
					if (buff.Value.DisplayAsPercent)
					{
						valueText += buff.Value.amount > 0 ? ((buff.Value.amount - 1) * 100).ToString("F2") + "%" : "-" + ((buff.Value.amount - 1) * 100).ToString("F2") + "%";
					}
					else
					{
						valueText += "" + buff.Value.amount.ToString("F2");
					}
				}
				if (valueText.EndsWith(".00") || valueText.EndsWith(",00") || valueText.EndsWith(".00%") || valueText.EndsWith(",00%"))
				{
					valueText = valueText.TrimEnd('%').TrimEnd('0').TrimEnd(',');
				}
				DrawBuff(BuffOffsetX, BuffOffsetY, ResourceLoader.GetTexture(BuffsByID[buff.Value._ID].IconID), valueText, (span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString()), !buff.Value.isNegative, buff.Value.duration);
				BuffOffsetX += BuffSize;
				if (BuffOffsetX > MaxX)
				{
					BuffOffsetX = 0;
					BuffOffsetY -= BuffSize;
				}
			}



			GUI.color = Color.blue;
			GUI.Label(HUDenergyLabelRect, Mathf.Floor(LocalPlayer.Stats.Stamina).ToString("N0") + "/" + Mathf.Floor(ModdedPlayer.Stats.TotalMaxEnergy).ToString("N0"), HUDStatStyle);
			GUI.color = new Color(0.8f, 0.0f, 0.0f);

			GUI.Label(HUDHealthLabelRect, Mathf.Floor(LocalPlayer.Stats.Health).ToString("N0") + "/" + Mathf.Floor(ModdedPlayer.Stats.TotalMaxHealth).ToString("N0"), HUDStatStyle);
			if (ModdedPlayer.instance.DamageAbsorbAmount > 0)
			{
				GUI.color = new Color(1f, 0.15f, 0.8f);
				GUI.Label(HUDShieldLabelRect, Mathf.Floor(ModdedPlayer.instance.DamageAbsorbAmount).ToString("N0"), HUDStatStyle);
			}
			GUI.color = Color.white;


			//spells

			float SquareSize = 45 * screenScale;
			for (int i = 0; i < SpellCaster.SpellCount; i++)
			{
				Rect r = new Rect(
					Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f) + i * SquareSize,
					Screen.height - SquareSize,
					SquareSize,
					SquareSize
					);

				GUI.DrawTexture(r, _SpellBG);
				if (SpellCaster.instance.infos[i].spell == null || SpellCaster.instance.infos[i].spell.icon == null)
				{
				}
				else
				{
					GUI.color = new Color(1, 1, 1, 0.4f);
					GUI.color = new Color(1, 1, 1, 1f);
					if (ModdedPlayer.Stats.silenced)
					{
						GUI.color = Color.black;
					}
					else if (!(SpellCaster.instance.Ready[i] && !ModdedPlayer.Stats.silenced && LocalPlayer.Stats.Energy >= SpellCaster.instance.infos[i].spell.EnergyCost * ModdedPlayer.Stats.spellCostEnergyCost * ModdedPlayer.Stats.spellCost && LocalPlayer.Stats.Stamina >= SpellCaster.instance.infos[i].spell.EnergyCost * ModdedPlayer.Stats.SpellCostToStamina * ModdedPlayer.Stats.spellCost && SpellCaster.instance.infos[i].spell.CanCast))
					{
						GUI.color = Color.blue;
					}
					GUI.DrawTexture(r, SpellCaster.instance.infos[i].spell.icon);

					GUI.Label(r, SpellCaster.instance.infos[i].key.ToString(), new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.RoundToInt(screenScale * 15), fontStyle = FontStyle.Normal, alignment = TextAnchor.MiddleCenter });
					GUI.color = Color.white;
					if (!SpellCaster.instance.infos[i].spell.Bought)
					{
						SpellCaster.instance.SetSpell(i);
					}
					else if (!SpellCaster.instance.Ready[i])
					{
						Rect fillr = new Rect(r);
						float f = SpellCaster.instance.infos[i].Cooldown / SpellCaster.instance.infos[i].spell.Cooldown;
						fillr.height *= f;
						fillr.y += SquareSize * (1 - f);
						GUI.DrawTexture(fillr, _SpellCoolDownFill, ScaleMode.ScaleAndCrop);

						TimeSpan span = TimeSpan.FromSeconds(SpellCaster.instance.infos[i].Cooldown * ModdedPlayer.Stats.cooldown);
						string formattedTime = span.Minutes > 0 ? span.Minutes + ":" + span.Seconds : span.Seconds.ToString();
						GUI.Label(r, formattedTime, new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(screenScale * 20), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
					}
				}

				GUI.DrawTexture(r, _SpellFrame);
			}
			float width = SpellCaster.SpellCount * SquareSize;
			float height = width * 0.1f;
			float combatHeight = width * 33 / 1500;
			//Defining rectangles to later use to draw HUD elements
			Rect XPbar = new Rect(Screen.width / 2f - (SquareSize * SpellCaster.SpellCount / 2f), Screen.height - height - SquareSize, width, height);
			Rect XPbarFill = new Rect(XPbar);
			XPbarFill.width *= ProgressBarAmount;
			Rect CombatBar = new Rect(XPbar.x, 0, SpellCaster.SpellCount * SquareSize * (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.Stats.maxMassacreTime), combatHeight);
			Rect CombatBarCount = new Rect(XPbar.x, 30 * screenScale, SpellCaster.SpellCount * SquareSize, combatHeight);

			float cornerDimension = Screen.height - XPbar.y;
			Rect LeftCorner = new Rect(XPbar.x - cornerDimension, XPbar.y, cornerDimension, cornerDimension);
			Rect RightCorner = new Rect(XPbar.xMax, XPbar.y, cornerDimension, cornerDimension);
			Rect CombatBarText = new Rect(CombatBarCount)
			{
				y = CombatBar.yMax + 40 * screenScale,
				height = 100 * screenScale
			};
			Rect CombatBarTimer = new Rect(CombatBar.xMax, CombatBar.y, 300, combatHeight);
			GUIStyle CombatCountStyle = new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.FloorToInt(19 * screenScale), alignment = TextAnchor.MiddleCenter };
			GUI.DrawTexture(XPbar, _expBarBackgroundTex, ScaleMode.ScaleToFit, true, 1500 / 150);
			GUI.DrawTextureWithTexCoords(XPbarFill, _expBarFillTex, new Rect(0, 0, ProgressBarAmount, 1));
			GUI.DrawTexture(XPbar, _expBarFrameTex, ScaleMode.ScaleToFit, true, 1500 / 150);
			GUI.DrawTexture(LeftCorner, ResourceLoader.GetTexture(106));
			guiMatrixBackup = GUI.matrix;
			GUIUtility.ScaleAroundPivot(new Vector2(-1, 1), RightCorner.center);
			GUI.DrawTexture(RightCorner, ResourceLoader.GetTexture(106));
			GUI.matrix = guiMatrixBackup;

			if (ModdedPlayer.instance.TimeUntillMassacreReset > 0)
			{
				GUI.DrawTextureWithTexCoords(CombatBar, _combatDurationTex, new Rect(0, 0, (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.Stats.maxMassacreTime), 1));
				GUI.color = new Color(0.7f, 0.4f, 0.4f, 1f);
				if (ModdedPlayer.instance.MassacreText != "")
				{
					GUI.DrawTexture(CombatBarText, ResourceLoader.GetTexture(142));
				}

				GUI.Label(CombatBarCount, "+" + ModdedPlayer.instance.NewlyGainedExp.ToString("N0") + " EXP\tx" + ModdedPlayer.instance.MassacreMultiplier, CombatCountStyle);
				GUI.color = new Color(1, 0f, 0f, (ModdedPlayer.instance.TimeUntillMassacreReset / ModdedPlayer.Stats.maxMassacreTime) + 0.2f);
				string content = ModdedPlayer.instance.MassacreText;
				if (ModdedPlayer.instance.MassacreKills > 5)
				{
					content += Translations.MainMenu_HUD_2/*og:\t*/ + ModdedPlayer.instance.MassacreKills + Translations.MainMenu_HUD_1/*og: kills*/; //tr
				}

				GUI.Label(CombatBarText, content, new GUIStyle(GUI.skin.label) { font = mainFont, fontSize = Mathf.FloorToInt(45 * screenScale), alignment = TextAnchor.UpperCenter, clipping = TextClipping.Overflow, richText = true, wordWrap = false });
				GUI.color = new Color(1, 1, 1, 1f);
			}

			if (LocalPlayer.FpCharacter.crouching)
			{
				try
				{


					bool scanning = false;
					RaycastHit[] hits = Physics.BoxCastAll(Camera.main.transform.position, Vector3.one * 2.5f, Camera.main.transform.forward, Camera.main.transform.rotation, 500);
					int enemyHit = -1;
					for (int i = 0; i < hits.Length; i++)
					{
						if (hits[i].transform.CompareTag("enemyCollide"))
						{
							enemyHit = i;
							break;
						}
					}
					if (enemyHit != -1)
					{
						ScanTime += Time.unscaledDeltaTime * 1.5f;
						if (hits[enemyHit].transform.root == scannedTransform)
						{
							if (BoltNetwork.isRunning && scannedEntity != null)
							{
								cp = EnemyManager.GetCP(scannedEntity);
							}
							else
							{
								cp = EnemyManager.GetCP(hits[enemyHit].transform.root);
							}
							if (cp != null)
							{
								scanning = true;

								GUIStyle infoStyle = new GUIStyle(GUI.skin.label)
								{
									font = mainFont,
									fontSize = Mathf.RoundToInt(28 * screenScale),
									alignment = TextAnchor.MiddleRight,
									wordWrap = false,
									clipping = TextClipping.Overflow,
									richText = true,
								};
								GUIStyle smallinfoStyle = new GUIStyle(GUI.skin.label)
								{
									font = mainFont,
									fontSize = Mathf.RoundToInt(15 * screenScale),
									alignment = TextAnchor.MiddleLeft,
									wordWrap = false,
									clipping = TextClipping.Overflow,
									richText = true,
									fontStyle = FontStyle.Italic
								};


								Vector2 origin = wholeScreenRect.center;
								origin.y -= 400 * screenScale;
								origin.x = 200 * screenScale;
								float y = 0;
								var slide = SmoothSlideIn(0.7f);
								DrawScannedEnemyLabel(cp.EnemyName, new Rect(origin.x + slide, origin.y + y, 250 * screenScale, 66 * screenScale), infoStyle);
								y += screenScale * 60;
								DrawScannedEnemyLabel(Translations.MainMenu_Inventory_16/*og:Level*/+ Translations.MainMenu_Guide_1/*og:: */ + cp.Level, new Rect(origin.x + slide, origin.y + y, 250 * screenScale, 65 * screenScale), infoStyle); //tr
								y += screenScale * 60;
								if (ScanTime > 0.8f)
								{
									slide = SmoothSlideIn(1f);
									DrawScannedEnemyLabel(cp.Health.ToString("N0") + "/" + cp.MaxHealth.ToString("N0") + "♥", new Rect(origin.x + slide, origin.y + y, 250 * screenScale, 65 * screenScale), infoStyle);
									y += screenScale * 60;
								}
								if (ScanTime > 1.6f)
								{
									slide = SmoothSlideIn(1.8f);
									DrawScannedEnemyLabel(Translations.MainMenu_Guide_17/*og:Armor*/ + Translations.MainMenu_Guide_1/*og:: */ + cp.Armor.ToString("N0"), new Rect(origin.x + slide, origin.y + y, 250 * screenScale, 65 * screenScale), infoStyle); //tr
									if (cp.ArmorReduction > 0)
									{
										y += screenScale * 40;
										DrawScannedEnemyLabel(Translations.MainMenu_Guide_100/*og:Armor reduction*/ + //tr
											": -" + cp.ArmorReduction.ToString("N0"), new Rect(origin.x, origin.y + y, 250 * screenScale, 65 * screenScale), infoStyle);
									}
									else if (cp.Armor > 10 && ModSettings.difficulty <= ModSettings.Difficulty.Veteran)
									{
										Rect rOffset = new Rect(origin.x + slide + 255f * screenScale, origin.y + y, 250 * screenScale, 65 * screenScale);
										GUI.Label(rOffset, Translations.MainMenu_HUD_29, smallinfoStyle);//tr
										rOffset.y += screenScale * 18;
										GUI.Label(rOffset, "Use fire to melt enemy armor.", smallinfoStyle);
									}
									y += screenScale * 60;
								}
								if (ScanTime > 2.2f)
								{
									slide = SmoothSlideIn(2.4f);
									DrawScannedEnemyLabel(Translations.MainMenu_HUD_30 + Translations.MainMenu_Guide_1 + cp.Damage.ToString("N0"), new Rect(origin.x, origin.y + y, 250 * screenScale, 65 * screenScale), infoStyle);//tr
									y += screenScale * 60;
									DrawScannedEnemyLabel(Translations.MainMenu_HUD_31 + Translations.MainMenu_Guide_1 + cp.ExpBounty.ToString("N0"), new Rect(origin.x, origin.y + y, 250 * screenScale, 65 * screenScale), infoStyle);//tr
									if (ModSettings.difficulty <= ModSettings.Difficulty.Veteran)
									{
										Rect rOffset = new Rect(origin.x + slide + 255f * screenScale, origin.y + y, 250 * screenScale, 65 * screenScale);
										rOffset.x -= 10;
										GUI.Label(rOffset, Translations.MainMenu_HUD_32, smallinfoStyle);//tr
									}
									y += screenScale * 85;
								}
								if (ScanTime > 3.0f)
								{
									if (cp.Affixes.Length > 0)
									{
										DrawScannedEnemyLabel("☠️ " +
											Translations.MainMenu_HUD_3/*og:ELITE*/ //tr
											+ " ☠️", new Rect(origin.x, origin.y + y, 250 * screenScale, 65 * screenScale), new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(33 * screenScale), alignment = TextAnchor.MiddleRight });
										y += screenScale * 30;
										infoStyle.fontSize -= 4;
										Array arr = Enum.GetValues(typeof(EnemyProgression.Abilities));
										float fdelay = 0;
										foreach (int i in cp.Affixes)
										{
											slide = SmoothSlideIn(3.2f + fdelay);
											fdelay += 0.1f;
											Rect rName = new Rect(origin.x + slide, origin.y + y, 250 * screenScale, 55 * screenScale);
											Rect rDesc = new Rect((origin.x + slide + 255 * screenScale), origin.y + y + 24 * screenScale, 250 * screenScale, 65 * screenScale);
											EnemyProgression.Abilities ability = (EnemyProgression.Abilities)arr.GetValue(i);
											switch (ability)
											{
												case EnemyProgression.Abilities.Poisonous:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_4/*og:Poisonous*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_33, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Steadfast:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_5/*og:Steadfast*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_34, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.BossSteadfast:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_6/*og:Boss Steadfast*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_35, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.EliteSteadfast:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_7/*og:Elite Steadfast*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_36, smallinfoStyle);//tr
													break;
												//case EnemyProgression.Abilities.Molten:
												//    DrawScannedEnemyLabel("Nothing yet", new Rect(origin.x, origin.y + y, 250 * rr, 55 * rr), infoStyle);
												//    break;
												case EnemyProgression.Abilities.Blizzard:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_8/*og:Blizzard*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_37, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Radiance:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_9/*og:Radiance*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_38, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Chains:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_10/*og:Chains*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_39, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.BlackHole:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_11/*og:Black Hole*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_40, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Trapper:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_12/*og:Trapper*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_41, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Juggernaut:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_13/*og:Juggernaut*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_42, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Gargantuan:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_14/*og:Gargantuan*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_43, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Tiny:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_15/*og:Tiny*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_44, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.ExtraDamage:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_16/*og:Extra deadly*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_45, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.ExtraHealth:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_17/*og:Extra tough*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_46, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Basher:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_18/*og:Basher*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_47, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Warp:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_19/*og:Warp*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_48, smallinfoStyle);//tr
													break;
												//case EnemyProgression.Abilities.Thunder:
												//    DrawScannedEnemyLabel("Nothing yet", new Rect(origin.x, origin.y + y, 250 * rr, 55 * rr), infoStyle);
												//    break;
												case EnemyProgression.Abilities.RainEmpowerment:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_20/*og:Rain Empowerment*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_49, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Shielding:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_21/*og:Shielding*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_50, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Meteor:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_22/*og:Meteor*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_51, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Flare:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_23/*og:Flare*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_52, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Undead:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_24/*og:Undead*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_53, smallinfoStyle);//tr
													break;

												case EnemyProgression.Abilities.Laser:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_25/*og:Laser tower*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_54, smallinfoStyle);//tr
													break;
												case EnemyProgression.Abilities.Avenger:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_26/*og:Avenger*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_55, smallinfoStyle);//tr
													break;
												case EnemyProgression.Abilities.Sacrifice:
													DrawScannedEnemyLabel(Translations.MainMenu_HUD_27/*og:Sacrifice*/, rName, infoStyle); //tr
													GUI.Label(rDesc, Translations.MainMenu_HUD_56, smallinfoStyle);//tr
													break;
												default:
													DrawScannedEnemyLabel(ability.ToString(), rName, infoStyle);
													break;
											}
											y += screenScale * 40;
										}
									}
								}
							}
							else
							{
								Debug.Log("Scanned enemy does not have CP");
							}
						}
						else
						{
							cp = null;

							scannedTransform = hits[enemyHit].transform.root;
							scannedEntity = scannedTransform.GetComponentInChildren<BoltEntity>();
							if (scannedEntity == null)
							{
								scannedEntity = scannedTransform.GetComponent<BoltEntity>();
							}
							ScanTime = 0;
						}

					}


					if (scanning)
					{
						Rect scanRect = new Rect(0, 0, 60 * screenScale, 60 * screenScale)
						{
							center = wholeScreenRect.center
						};
						ScanRotation += Time.deltaTime * 45;
						Matrix4x4 matrix4X4 = GUI.matrix;
						GUIUtility.RotateAroundPivot(ScanRotation, scanRect.center);
						GUI.DrawTexture(scanRect, ResourceLoader.instance.LoadedTextures[24]);

						GUI.matrix = matrix4X4;
					}
					else
					{
						Rect scanRect = new Rect(0, 0, 20 * screenScale, 20 * screenScale)
						{
							center = wholeScreenRect.center
						};
						//ScanRotation += Time.deltaTime * 45;
						GUI.DrawTexture(scanRect, ResourceLoader.instance.LoadedTextures[24]);
					}
				}
				catch (Exception ex)	
				{
					Debug.LogWarning(ex.ToString());
				}
			}
			else
			{
				ScanTime = 0;
			}

			PingGUIDraw();

			if (LevelUpDuration > 0)
			{
				float y = Mathf.Cos(Mathf.PI * (LevelUpDuration / (2 * lvlUpDuration))) * 50 * screenScale;
				//Level up icon
				Rect r = new Rect(710 * screenScale, y, 500 * screenScale, 300 * screenScale);

				float opacity = 1;
				if (LevelUpDuration < lvlUpFadeDuration)
				{
					opacity = Mathf.Sin(LevelUpDuration * Mathf.PI / (2 * lvlUpFadeDuration));
				}
				else if (LevelUpDuration > lvlUpDuration - lvlUpFadeDuration)
				{
					opacity = Mathf.Sin((lvlUpDuration - LevelUpDuration) * Mathf.PI / (2 * lvlUpFadeDuration));
				}
				GUI.color = new Color(1, 1, 1, opacity);
				GUI.DrawTexture(r, ResourceLoader.GetTexture(164));
				GUI.Label(r, LevelUpText, new GUIStyle(GUI.skin.label) { font = mainFont, clipping = TextClipping.Overflow, wordWrap = true, alignment = TextAnchor.MiddleCenter, fontSize = Mathf.RoundToInt(46 * screenScale), });
				GUI.color = new Color(1, 1, 1, 1);
			}
		}

		private ClientEnemyProgression cp = null;

		private bool pingBlocked = false;

		private void UnlockPing()
		{
			pingBlocked = false;
		}

		public void LockPing()
		{
			pingBlocked = true;
			Invoke("UnlockPing", 1f);
		}

		//called at update, shows or hides ping menu, and casts ping commands
		private void PingUpdate()
		{
			try
			{
				if (otherPlayerPings != null)
				{
					string toRem = null;
					foreach (var item in otherPlayerPings)
					{
						if (item.Value.pingType == MarkObject.PingType.Enemy)
						{
							if (((MarkEnemy)item.Value).Outdated())
							{
								//remove ping
								toRem = item.Key;
								break;
							}
						}
						else if (item.Value.pingType == MarkObject.PingType.Location)
						{
							if (((MarkPostion)item.Value).Outdated())
							{
								//remove ping
								toRem = item.Key;
								break;
							}
						}
						else if (item.Value.pingType == MarkObject.PingType.Item)
						{
							if (((MarkPickup)item.Value).Outdated())
							{
								//remove ping
								toRem = item.Key;
								break;
							}
						}
					}
					if (!string.IsNullOrEmpty(toRem))
						otherPlayerPings.Remove(toRem);
				}
				if (localPlayerPing != null)
				{
					if (localPlayerPing.pingType == MarkObject.PingType.Enemy)
					{
						if (((MarkEnemy)localPlayerPing).Outdated())
						{
							localPlayerPing = null;
						}
					}
					else if (localPlayerPing.pingType == MarkObject.PingType.Location)
					{
						if (((MarkPostion)localPlayerPing).Outdated())
						{
							localPlayerPing = null;
						}
					}
					else if (localPlayerPing.pingType == MarkObject.PingType.Item)
					{
						if (((MarkPickup)localPlayerPing).Outdated())
						{
							localPlayerPing = null;
						}
					}
				}
			}
			catch (Exception exc)
			{
				//ModAPI.Log.Write(exc.ToString());
			}

			drawPingPreview = false;
			if (!pingBlocked && (ModAPI.Input.GetButton("ping") || UnityEngine.Input.GetMouseButton(2)))
			{
				if (localPlayerPing != null)
				{
					SendClearMyPing();
					localPlayerPing = null;
					LockPing();
					return;
				}

				//is holding middle mouse btn
				if (Physics.Raycast(MainCamera.position + MainCamera.forward, MainCamera.forward, out RaycastHit hit, 3000))
				{
					drawPingPreview = true;
					previewPingPos = hit.point;
					previewPingDist = hit.distance;
					if (ScanTime > 0)
					{
						previewPingType = MarkObject.PingType.Enemy;
						previewPingPos = scannedTransform.position + Vector3.up;

						if (UnityEngine.Input.GetMouseButtonDown(1))
						{
							LockPing();
							if (GameSetup.IsMultiplayer)
							{
								if (cp != null && cp.Level > 0)
								{
									if (GameSetup.IsMpClient)
									{
										using (MemoryStream answerStream = new MemoryStream())
										{
											using (BinaryWriter w = new BinaryWriter(answerStream))
											{
												w.Write(37);
												w.Write(ModReferences.ThisPlayerID);
												w.Write(scannedEntity.networkId.PackedValue);
												w.Close();
											}
											NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
											answerStream.Close();
										}
									}
									else
									{
										if (EnemyManager.hostDictionary.ContainsKey(scannedEntity.networkId.PackedValue))
										{
											using (MemoryStream answerStream = new MemoryStream())
											{
												using (BinaryWriter w = new BinaryWriter(answerStream))
												{
													w.Write(36);
													w.Write(ModReferences.ThisPlayerID);
													w.Write(0);
													w.Write(scannedEntity.networkId.PackedValue);
													w.Write(cp.Affixes.Length > 0);
													w.Write(cp.EnemyName);
													w.Close();
												}
												NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
												answerStream.Close();
											}
											localPlayerPing = new MarkEnemy(scannedTransform, cp.EnemyName, cp.Affixes.Length > 0, scannedEntity);
										}
									}
								}
							}
							else
							{
								var enemy = hit.transform.root.GetComponent<EnemyProgression>();
								if (enemy == null)
								{
									enemy = hit.transform.root.GetComponentInChildren<EnemyProgression>();
								}
								if (enemy != null)
								{
									localPlayerPing = new MarkEnemy(enemy.transform, enemy.enemyName, enemy._rarity != EnemyProgression.EnemyRarity.Normal);
								}
								else
								{
									localPlayerPing = new MarkEnemy(enemy.transform, Translations.MainMenu_HUD_28/*og:Enemy*/, false);//tr
								}
							}
						}
					}
					else if (hit.transform.CompareTag("enemyCollide"))
					{
						//indicate pinging enemyCollide
						previewPingType = MarkObject.PingType.Enemy;
						if (UnityEngine.Input.GetMouseButtonDown(1))
						{
							LockPing();
							if (GameSetup.IsMultiplayer)
							{
								var entity = hit.transform.GetComponentInParent<BoltEntity>();
								if (entity != null)
								{
									if (GameSetup.IsMpClient)
									{
										using (MemoryStream answerStream = new MemoryStream())
										{
											using (BinaryWriter w = new BinaryWriter(answerStream))
											{
												w.Write(37);
												w.Write(ModReferences.ThisPlayerID);
												w.Write(entity.networkId.PackedValue);
												w.Close();
											}
											NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.OnlyServer);
											answerStream.Close();
										}
									}
									else
									{
										if (EnemyManager.hostDictionary.ContainsKey(entity.networkId.PackedValue))
										{
											var enemy = EnemyManager.hostDictionary[entity.networkId.PackedValue];
											using (MemoryStream answerStream = new MemoryStream())
											{
												using (BinaryWriter w = new BinaryWriter(answerStream))
												{
													w.Write(36);
													w.Write(ModReferences.ThisPlayerID);
													w.Write(0);
													w.Write(entity.networkId.PackedValue);
													w.Write(enemy._rarity != EnemyProgression.EnemyRarity.Normal);
													w.Write(enemy.enemyName);
													w.Close();
												}
												NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
												answerStream.Close();
											}
											localPlayerPing = new MarkEnemy(enemy.transform, enemy.enemyName, enemy._rarity != EnemyProgression.EnemyRarity.Normal);
										}
									}
								}
							}
							else
							{
								if (cp != null)
								{
									localPlayerPing = new MarkEnemy(scannedTransform, cp.EnemyName, cp.Affixes.Length > 0);
								}
								else
								{
									ModAPI.Console.Write("Cp is null");
								}
							}
						}
					}
					else
					{
						var pu = hit.transform.GetComponent<ItemPickUp>();
						if (pu != null)
						{
							//pickup marker
							previewPingType = MarkObject.PingType.Item;
							if (UnityEngine.Input.GetMouseButtonDown(1))
							{
								LockPing();
								if (GameSetup.IsMultiplayer)
								{
									using (MemoryStream answerStream = new MemoryStream())
									{
										using (BinaryWriter w = new BinaryWriter(answerStream))
										{
											w.Write(36);
											w.Write(ModReferences.ThisPlayerID);
											w.Write(2);
											w.Write(pu.ID);
											w.Close();
										}
										NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
										answerStream.Close();
									}
									localPlayerPing = new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity);
								}
								else
								{
									localPlayerPing = new MarkPickup(pu.transform, pu.item.name, pu.item.Rarity);
								}
							}
						}
						else
						{
							//location marker
							previewPingType = MarkObject.PingType.Location;
							if (UnityEngine.Input.GetMouseButtonDown(1))
							{
								LockPing();
								if (GameSetup.IsMultiplayer)
								{
									using (MemoryStream answerStream = new MemoryStream())
									{
										using (BinaryWriter w = new BinaryWriter(answerStream))
										{
											w.Write(36);
											w.Write(ModReferences.ThisPlayerID);
											w.Write(1);
											w.Write(hit.point.x);
											w.Write(hit.point.y + 1);
											w.Write(hit.point.z);
											w.Close();
										}
										NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Others);
										answerStream.Close();
									}
									localPlayerPing = new MarkPostion(hit.point + Vector3.up);
								}
								else
								{
									localPlayerPing = new MarkPostion(hit.point + Vector3.up);
								}
							}
						}
					}
				}
			}
		}

		private void SendClearMyPing()
		{
			if (GameSetup.IsMultiplayer)
			{
				using (MemoryStream answerStream = new MemoryStream())
				{
					using (BinaryWriter w = new BinaryWriter(answerStream))
					{
						w.Write(35);
						w.Write(ModReferences.ThisPlayerID);
						w.Close();
					}
					NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
					answerStream.Close();
				}
			}
		}

		private bool drawPingPreview;
		private Vector3 previewPingPos;
		private MarkObject.PingType previewPingType;
		private float previewPingDist;

		private void PingGUIDraw()
		{
			if (otherPlayerPings != null)
			{
				foreach (var item in otherPlayerPings.Values)
				{
					if (item.pingType == MarkObject.PingType.Enemy)
					{
						((MarkEnemy)item).Draw();
					}
					else if (item.pingType == MarkObject.PingType.Location)
					{
						((MarkPostion)item).Draw();
					}
					else if (item.pingType == MarkObject.PingType.Item)
					{
						((MarkPickup)item).Draw();
					}
				}
			}
			if (localPlayerPing != null)
			{
				DrawPings();
			}
			else if (drawPingPreview)
			{
				DrawPingPreview();
			}
		}
		void DrawPings()
		{
			if (localPlayerPing.pingType == MarkObject.PingType.Enemy)
			{
				((MarkEnemy)localPlayerPing).Draw();
			}
			else if (localPlayerPing.pingType == MarkObject.PingType.Location)
			{
				((MarkPostion)localPlayerPing).Draw();
			}
			else if (localPlayerPing.pingType == MarkObject.PingType.Item)
			{
				((MarkPickup)localPlayerPing).Draw();
			}
		}
		void DrawPingPreview()
		{
			try
			{
				Vector3 pos = Camera.main.WorldToScreenPoint(previewPingPos);
				pos.y = Screen.height - pos.y;
				float size = Mathf.Clamp(700 / previewPingDist, 10, 40)/2;
				size *= screenScale;
				Rect r = previewPingType != MarkObject.PingType.Item ?
					new Rect(0, 0, size * 3.34f, size)
					{
						center = pos
					} :
					new Rect(0, 0, size * 1.3f, size * 2.4f)
					{
						center = pos
					};

				GUI.color = new Color(1, 1, 1, 0.3f);
				switch (previewPingType)
				{
					case MarkObject.PingType.Enemy:
						GUI.DrawTexture(r, ResourceLoader.GetTexture(172));
						break;

					case MarkObject.PingType.Location:
						GUI.DrawTexture(r, ResourceLoader.GetTexture(173));
						break;

					case MarkObject.PingType.Item:
						GUI.DrawTexture(r, ResourceLoader.GetTexture(174));
						break;

					default:
						break;
				}

				GUI.color = Color.white;
				GUI.Label(new Rect(Screen.width / 2 - 300 * screenScale, 10 * screenScale, 1000 * screenScale, 100), "Right click to place marker. When placed, press middle mouse or ping key to clear", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperCenter, clipping = TextClipping.Overflow, fontStyle = FontStyle.Italic, fontSize = (int)(15f * screenScale), font = mainFont });
			}
			catch (Exception e)
			{
				ModAPI.Console.Write(e.ToString());
			}
		}

		public Dictionary<ulong, MarkObject> otherPlayerPings;
		public MarkObject localPlayerPing;

		private const float lvlUpDuration = 3;
		private const float lvlUpFadeDuration = 1;
		public float LevelUpDuration;
		public string LevelUpText;
		public AudioSource lvlUpAudio;

		public void LevelUpAction()
		{
			LevelUpDuration = lvlUpDuration;
			LevelUpText = "\n" + ModdedPlayer.instance.level.ToString();


			if (lvlUpAudio == null)
			{
				lvlUpAudio = new GameObject("LvlupAudio").AddComponent<AudioSource>();
				lvlUpAudio.clip = ResourceLoader.instance.LoadedAudio[1001];
				lvlUpAudio.rolloffMode = AudioRolloffMode.Linear;
				lvlUpAudio.maxDistance = 1000;
				lvlUpAudio.transform.position = LocalPlayer.Transform.position;
				lvlUpAudio.Play();
			}
			else
			{
				lvlUpAudio.transform.position = LocalPlayer.Transform.position;
				lvlUpAudio.Play();
			}
		}

		private void DrawScannedEnemyLabel(string content, Rect r, GUIStyle style)
		{
			GUI.DrawTexture(r, ResourceLoader.instance.LoadedTextures[25]);
			Rect rOffset = new Rect(r);
			rOffset.x -= 30;
			GUI.Label(rOffset, content, style);
		}

		#endregion HUDMethods
	}
}