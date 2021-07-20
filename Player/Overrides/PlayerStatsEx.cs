using System;

using ChampionsOfForest.Player;

using FMOD.Studio;

using TheForest.Items.Inventory;
using TheForest.Tools;
using TheForest.Utils;
using TheForest.Utils.Settings;

using UnityEngine;

namespace ChampionsOfForest
{
	public class PlayerStatsEx : PlayerStats
	{
		protected override void Start()
		{
			base.Start();
			PlayerInventoryMod.SetupComplete = false;
			Effects.BlackFlame.instance.Start();
		}

		public override bool GoToSleep()
		{
			LocalPlayer.Stats.Energy += 100000000;
			return base.GoToSleep();
		}

		protected override void Update()
		{
			try
			{
				if (!(Scene.Atmosphere == null) && !SteamDSConfig.isDedicatedServer)
				{
					float num = Convert.ToSingle(LocalPlayer.Stats.DaySurvived + TheForest.Utils.Scene.Atmosphere.DeltaTimeOfDay);
					if (Mathf.FloorToInt(num) != Mathf.FloorToInt(LocalPlayer.Stats.DaySurvived))
					{
						LocalPlayer.Stats.DaySurvived = num;
						EventRegistry.Player.Publish(TfEvent.SurvivedDay, null);
					}
					else
					{
						LocalPlayer.Stats.DaySurvived = num;
					}
					LocalPlayer.ScriptSetup.targetInfo.isRed = IsRed;
					float num2 = 0f;
					num2 = ((!coldSwitch || LocalPlayer.AnimControl.coldOffsetBool) ? 0f : 1f);
					coldFloatBlend = Mathf.Lerp(coldFloatBlend, num2, Time.deltaTime * 10f);
					if (coldFloatBlend > 0.01f)
					{
						BoltSetReflectedShim.SetFloatReflected(animator, "coldFloat", coldFloatBlend);
					}
					else
					{
						BoltSetReflectedShim.SetFloatReflected(animator, "coldFloat", 0f);
					}
					if (Run && HeartRate < 170)
					{
						HeartRate++;
					}
					else if (!Run && HeartRate > 70)
					{
						HeartRate--;
					}
					if (Sitted)
					{
						Energy += 0.02f * ModdedPlayer.Stats.TotalMaxEnergy * Time.deltaTime + ModdedPlayer.Stats.TotalEnergyRecoveryMultiplier* 6 * Time.deltaTime;
					}
					if (!Clock.Dark && IsCold && !LocalPlayer.IsInCaves && !IsInNorthColdArea())
					{
						goto IL_01cb;
					}
					if (LocalPlayer.IsInEndgame)
					{
						goto IL_01cb;
					}
					goto IL_01e2;
				}
				return;
			IL_01e2:
				if (IsInNorthColdArea() && !Warm)
				{
					SetCold(true);
				}
				if (ShouldDoWetColdRoll && !IsCold && (LocalPlayer.IsInCaves || Clock.Dark))
				{
					if (!LocalPlayer.Buoyancy.InWater)
					{
						ShouldDoWetColdRoll = false;
					}
					else if (LocalPlayer.IsInCaves)
					{
						if (LocalPlayer.AnimControl.swimming)
						{
							if (Time.time - CaveStartSwimmingTime > 12f)
							{
								SetCold(true);
								ShouldDoWetColdRoll = false;
							}
						}
						else
						{
							CaveStartSwimmingTime = Time.time;
						}
					}
					else
					{
						Vector3 position = LocalPlayer.Transform.position;
						if (position.y - LocalPlayer.Buoyancy.WaterLevel < 1f)
						{
							if (UnityEngine.Random.Range(0, 100) < 30)
							{
								SetCold(true);
							}
							ShouldDoWetColdRoll = false;
						}
					}
				}
				if (ShouldDoGotCleanCheck)
				{
					if (!LocalPlayer.Buoyancy.InWater)
					{
						ShouldDoGotCleanCheck = false;
					}
					else
					{
						Vector3 position2 = LocalPlayer.ScriptSetup.hipsJnt.position;
						if (position2.y - LocalPlayer.Buoyancy.WaterLevel < -0.5f)
						{
							ShouldDoGotCleanCheck = false;
							GotCleanReal();
						}
					}
				}
				if (Health <= GreyZoneThreshold && AudioListener.volume > 0.2f)
				{
					AudioListener.volume -= 0.1f * Time.deltaTime;
				}
				else if (AudioListener.volume < 1f)
				{
					AudioListener.volume += 0.1f * Time.deltaTime;
				}
				if (IsHealthInGreyZone)
				{
					Tuts.LowHealthTutorial();
				}
				else
				{
					Tuts.CloseLowHealthTutorial();
				}
				if (Energy < 15f)
				{
					Tuts.LowEnergyTutorial();
				}
				else
				{
					Tuts.CloseLowEnergyTutorial();
				}
				if (Stamina <= 5 && !IsTired)
				{
					base.SendMessage("PlayStaminaBreath");
					IsTired = true;
					Run = false;
				}

				HealthTarget = Mathf.Clamp(HealthTarget, 0, ModdedPlayer.Stats.TotalMaxHealth);
				GreyZoneThreshold = Mathf.RoundToInt(ModdedPlayer.Stats.TotalMaxHealth * 0.05f);
				//if (HealthTarget > ModdedPlayer.Stats.TotalMaxHealth)
				//{
				//    HealthTarget = ModdedPlayer.Stats.TotalMaxHealth;
				//}
				if (Stamina > 5 && IsTired)
				{
					IsTired = false;
				}
				fsmStamina.Value = 100 * Stamina / ModdedPlayer.Stats.TotalMaxEnergy;
				fsmMaxStamina.Value = 100 * Energy / ModdedPlayer.Stats.TotalMaxEnergy;
				HealthResult = Health / ModdedPlayer.Stats.TotalMaxHealth + (ModdedPlayer.Stats.TotalMaxHealth - Health) / ModdedPlayer.Stats.TotalMaxHealth * 0.5f;
				float num3 = HealthTarget / ModdedPlayer.Stats.TotalMaxHealth + (ModdedPlayer.Stats.TotalMaxHealth - HealthTarget) / ModdedPlayer.Stats.TotalMaxHealth * 0.5f;
				if (HealthTargetResult < num3)
				{
					HealthTargetResult = Mathf.MoveTowards(HealthTargetResult, num3, 1f * Time.fixedDeltaTime);
				}
				else
				{
					HealthTargetResult = num3;
				}
				StaminaResult = Stamina / ModdedPlayer.Stats.TotalMaxEnergy + (ModdedPlayer.Stats.TotalMaxEnergy - Stamina) / ModdedPlayer.Stats.TotalMaxEnergy * 0.5f;
				EnergyResult = Energy / ModdedPlayer.Stats.TotalMaxEnergy + (ModdedPlayer.Stats.TotalMaxEnergy - Energy) / ModdedPlayer.Stats.TotalMaxEnergy * 0.5f;
				int num4 = 0;
				int num5 = 0;
				for (int i = 0; i < CurrentArmorTypes.Length; i++)
				{
					switch (CurrentArmorTypes[i])
					{
						case ArmorTypes.DeerSkin:
						case ArmorTypes.Warmsuit:
							num5++;
							break;

						case ArmorTypes.LizardSkin:
						case ArmorTypes.Leaves:
						case ArmorTypes.Bone:
							num4++;
							break;

						case ArmorTypes.Creepy:
							num4++;
							break;
					}
				}
				ColdArmorResult = num5 / 10f / 2f + 0.5f;
				ArmorResult = num4 / 10f / 2f + ColdArmorResult;
				Hud.ColdArmorBar.fillAmount = ColdArmorResult;
				Hud.ArmorBar.fillAmount = ArmorResult;
				Hud.StaminaBar.fillAmount = StaminaResult;
				Hud.HealthBar.fillAmount = HealthResult;
				Hud.HealthBarTarget.fillAmount = HealthTargetResult;
				Hud.EnergyBar.fillAmount = EnergyResult;
				float num6 = (Fullness - 0.2f) / 0.8f;
				TheForest.Utils.Scene.HudGui.Stomach.fillAmount = Mathf.Lerp(0.21f, 0.81f, num6);
				if (num6 < 0.5)
				{
					Hud.StomachOutline.SetActive(true);
					if (!Hud.Tut_Hungry.activeSelf)
					{
						Tuts.HungryTutorial();
					}
				}
				else
				{
					if (Hud.Tut_Hungry.activeSelf)
					{
						Tuts.CloseHungryTutorial();
					}
					Hud.StomachOutline.SetActive(false);
				}
				if (!TheForest.Utils.Scene.Atmosphere.Sleeping || Fullness > StarvationSettings.SleepingFullnessThreshold)
				{
					Fullness -= Convert.ToSingle(TheForest.Utils.Scene.Atmosphere.DeltaTimeOfDay * 1.6500000238418579 * 0.4f * ModdedPlayer.Stats.perk_hungerRate);
				}
				if (!Cheats.NoSurvival)
				{
					if (Fullness < 0.2f)
					{
						if (Fullness < 0.19f)
						{
							Fullness = 0.19f;
						}
						if (DaySurvived >= StarvationSettings.StartDay && !Dead && !TheForest.Utils.Scene.Atmosphere.Sleeping && LocalPlayer.Inventory.enabled)
						{
							if (!TheForest.Utils.Scene.HudGui.StomachStarvation.gameObject.activeSelf)
							{
								if (Starvation == 0f)
								{
									StarvationCurrentDuration = StarvationSettings.Duration;
								}
								TheForest.Utils.Scene.HudGui.StomachStarvation.gameObject.SetActive(true);
							}
							Starvation += Convert.ToSingle(TheForest.Utils.Scene.Atmosphere.DeltaTimeOfDay / StarvationCurrentDuration);
							if (Starvation >= 1f)
							{
								if (!StarvationSettings.TakingDamage)
								{
									StarvationSettings.TakingDamage = true;
									LocalPlayer.Tuts.ShowStarvationTut();
								}
								Hit(StarvationSettings.Damage, true, DamageType.Physical);
								TheForest.Utils.Scene.HudGui.StomachStarvationTween.ResetToBeginning();
								TheForest.Utils.Scene.HudGui.StomachStarvationTween.PlayForward();
								Starvation = 0f;
								StarvationCurrentDuration *= StarvationSettings.DurationDecay;
							}
							TheForest.Utils.Scene.HudGui.StomachStarvation.fillAmount = Mathf.Lerp(0.21f, 0.81f, Starvation);
						}
					}
					else if (Starvation > 0f || TheForest.Utils.Scene.HudGui.StomachStarvation.gameObject.activeSelf)
					{
						Starvation = 0f;
						StarvationCurrentDuration = StarvationSettings.Duration;
						StarvationSettings.TakingDamage = false;
						LocalPlayer.Tuts.StarvationTutOff();
						TheForest.Utils.Scene.HudGui.StomachStarvation.gameObject.SetActive(false);
					}
				}
				else
				{
					Fullness = 1f;
					if (Starvation > 0f || TheForest.Utils.Scene.HudGui.StomachStarvation.gameObject.activeSelf)
					{
						Starvation = 0f;
						StarvationCurrentDuration = StarvationSettings.Duration;
						StarvationSettings.TakingDamage = false;
						TheForest.Utils.Scene.HudGui.StomachStarvation.gameObject.SetActive(false);
					}
				}
				if (Fullness > 1f)
				{
					Fullness = 1f;
				}
				if (!Cheats.NoSurvival)
				{
					if (DaySurvived >= ThirstSettings.StartDay && !Dead && LocalPlayer.Inventory.enabled)
					{
						if (Thirst >= 1f)
						{
							if (!TheForest.Utils.Scene.HudGui.ThirstDamageTimer.gameObject.activeSelf)
							{
								TheForest.Utils.Scene.HudGui.ThirstDamageTimer.gameObject.SetActive(true);
							}
							if (ThirstCurrentDuration <= 0f)
							{
								ThirstCurrentDuration = ThirstSettings.DamageDelay;
								if (!ThirstSettings.TakingDamage)
								{
									ThirstSettings.TakingDamage = true;
									LocalPlayer.Tuts.ShowThirstTut();
								}
								Hit(Mathf.CeilToInt(ModdedPlayer.Stats.TotalMaxHealth * 0.2f * GameSettings.Survival.ThirstDamageRatio), true, DamageType.Physical);
								BleedBehavior.BloodAmount += 0.6f;
								TheForest.Utils.Scene.HudGui.ThirstDamageTimerTween.ResetToBeginning();
								TheForest.Utils.Scene.HudGui.ThirstDamageTimerTween.PlayForward();
							}
							else
							{
								ThirstCurrentDuration -= Time.deltaTime;
								TheForest.Utils.Scene.HudGui.ThirstDamageTimer.fillAmount = 1f - ThirstCurrentDuration / ThirstSettings.DamageDelay;
							}
						}
						else if (Thirst < 0f)
						{
							Thirst = 0f;
						}
						else
						{
							if (!TheForest.Utils.Scene.Atmosphere.Sleeping || Thirst < ThirstSettings.SleepingThirstThreshold)
							{
								Thirst += Convert.ToSingle((TheForest.Utils.Scene.Atmosphere.DeltaTimeOfDay / ThirstSettings.Duration) * 1.1f * GameSettings.Survival.ThirstRatio * ModdedPlayer.Stats.perk_thirstRate * 0.4f);
							}
							if (Thirst > ThirstSettings.TutorialThreshold)
							{
								//LocalPlayer.Tuts.ShowThirstyTut();
								TheForest.Utils.Scene.HudGui.ThirstOutline.SetActive(true);
							}
							else
							{
								LocalPlayer.Tuts.HideThirstyTut();
								TheForest.Utils.Scene.HudGui.ThirstOutline.SetActive(false);
							}
							if (ThirstSettings.TakingDamage)
							{
								ThirstSettings.TakingDamage = false;
								LocalPlayer.Tuts.ThirstTutOff();
							}
							if (TheForest.Utils.Scene.HudGui.ThirstDamageTimer.gameObject.activeSelf)
							{
								TheForest.Utils.Scene.HudGui.ThirstDamageTimer.gameObject.SetActive(false);
							}
						}
						TheForest.Utils.Scene.HudGui.Hydration.fillAmount = 1f - Thirst;
					}
				}
				else if (TheForest.Utils.Scene.HudGui.Hydration.fillAmount != 1f)
				{
					TheForest.Utils.Scene.HudGui.Hydration.fillAmount = 1f;
				}
				bool flag = false;
				bool flag2 = false;
				if (LocalPlayer.WaterViz.ScreenCoverage > AirBreathing.ScreenCoverageThreshold && !Dead)
				{
					if (!TheForest.Utils.Scene.HudGui.AirReserve.gameObject.activeSelf)
					{
						TheForest.Utils.Scene.HudGui.AirReserve.gameObject.SetActive(true);
					}
					if (!AirBreathing.UseRebreather && AirBreathing.RebreatherIsEquipped && AirBreathing.CurrentRebreatherAir > 0f)
					{
						AirBreathing.UseRebreather = true;
					}
					if (AirBreathing.UseRebreather)
					{
						flag = true;
						AirBreathing.CurrentRebreatherAir -= Time.deltaTime;
						TheForest.Utils.Scene.HudGui.AirReserve.fillAmount = AirBreathing.CurrentRebreatherAir / AirBreathing.MaxRebreatherAirCapacity;
						if (AirBreathing.CurrentRebreatherAir < 0f)
						{
							AirBreathing.CurrentLungAir = 0f;
							AirBreathing.UseRebreather = false;
						}
						else if (AirBreathing.CurrentRebreatherAir < AirBreathing.OutOfAirWarningThreshold)
						{
							if (!TheForest.Utils.Scene.HudGui.AirReserveOutline.activeSelf)
							{
								TheForest.Utils.Scene.HudGui.AirReserveOutline.SetActive(true);
							}
						}
						else if (TheForest.Utils.Scene.HudGui.AirReserveOutline.activeSelf)
						{
							TheForest.Utils.Scene.HudGui.AirReserveOutline.SetActive(false);
						}
					}
					else
					{
						if (Time.timeScale > 0f)
						{
							if (!AirBreathing.CurrentLungAirTimer.IsRunning)
							{
								AirBreathing.CurrentLungAirTimer.Start();
							}
						}
						else if (AirBreathing.CurrentLungAirTimer.IsRunning)
						{
							AirBreathing.CurrentLungAirTimer.Stop();
						}
						if (AirBreathing.CurrentLungAir > AirBreathing.MaxLungAirCapacityFinal)
						{
							AirBreathing.CurrentLungAir = AirBreathing.MaxLungAirCapacityFinal;
						}
						if (AirBreathing.CurrentLungAir > AirBreathing.CurrentLungAirTimer.Elapsed.TotalSeconds * Skills.LungBreathingRatio)
						{
							Skills.TotalLungBreathingDuration += Time.deltaTime;
							TheForest.Utils.Scene.HudGui.AirReserve.fillAmount = Mathf.Lerp(TheForest.Utils.Scene.HudGui.AirReserve.fillAmount, AirBreathing.CurrentAirPercent, Mathf.Clamp01((Time.time - Time.fixedTime) / Time.fixedDeltaTime));
							if (!TheForest.Utils.Scene.HudGui.AirReserveOutline.activeSelf)
							{
								TheForest.Utils.Scene.HudGui.AirReserveOutline.SetActive(true);
							}
						}
						else if (!Cheats.NoSurvival)
						{
							flag2 = true;
							AirBreathing.DamageCounter += AirBreathing.Damage * Time.deltaTime;
							if (AirBreathing.DamageCounter >= 1f)
							{
								int dmg = 3 + (int)(ModdedPlayer.Stats.TotalMaxHealth * 0.1f);
								Hit(dmg, true, DamageType.Drowning);
								AirBreathing.DamageCounter -= (int)AirBreathing.DamageCounter;
							}
							if (Dead)
							{
								AirBreathing.DamageCounter = 0f;
								DeadTimes++;
								TheForest.Utils.Scene.HudGui.AirReserve.gameObject.SetActive(false);
								TheForest.Utils.Scene.HudGui.AirReserveOutline.SetActive(false);
							}
							else if (!TheForest.Utils.Scene.HudGui.AirReserveOutline.activeSelf)
							{
								TheForest.Utils.Scene.HudGui.AirReserveOutline.SetActive(true);
							}
						}
					}
				}
				else if (AirBreathing.CurrentLungAir < AirBreathing.MaxLungAirCapacityFinal || TheForest.Utils.Scene.HudGui.AirReserve.gameObject.activeSelf)
				{
					if (GaspForAirEvent.Length > 0 && FMOD_StudioSystem.instance && !Dead)
					{
						FMOD_StudioSystem.instance.PlayOneShot(GaspForAirEvent, base.transform.position, delegate (FMOD.Studio.EventInstance instance)
						{
							float value = 85f;
							if (!AirBreathing.UseRebreather)
							{
								value = (AirBreathing.CurrentLungAir - (float)AirBreathing.CurrentLungAirTimer.Elapsed.TotalSeconds) / AirBreathing.MaxLungAirCapacity * 100f;
							}
							UnityUtil.ERRCHECK(instance.setParameterValue("oxygen", value));
							return true;
						});
					}
					AirBreathing.DamageCounter = 0f;
					AirBreathing.CurrentLungAirTimer.Stop();
					AirBreathing.CurrentLungAirTimer.Reset();
					AirBreathing.CurrentLungAir = AirBreathing.MaxLungAirCapacityFinal;
					TheForest.Utils.Scene.HudGui.AirReserve.gameObject.SetActive(false);
					TheForest.Utils.Scene.HudGui.AirReserveOutline.SetActive(false);
				}
				if (flag)
				{
					UpdateRebreatherEvent();
				}
				else
				{
					StopIfPlaying(RebreatherEventInstance);
				}
				if (flag2)
				{
					UpdateDrowningEvent();
				}
				else
				{
					StopIfPlaying(DrowningEventInstance);
				}
				if (Energy > ModdedPlayer.Stats.TotalMaxEnergy)
				{
					Energy = ModdedPlayer.Stats.TotalMaxEnergy;
				}
				if (Energy < 5)
				{
					Energy = 5;
				}
				if (Health < 0f)
				{
					Health = 0f;
				}
				if (float.IsNaN(Health))
				{
					Health = 0;
				}
				if (Health > ModdedPlayer.Stats.TotalMaxHealth)
				{
					Health = ModdedPlayer.Stats.TotalMaxHealth;
				}
				if (Health < HealthTarget)
				{
					Health = Mathf.MoveTowards(Health, HealthTarget, (GameSettings.Survival.HealthRegenPerSecond + ModdedPlayer.Stats.TotalMaxHealth * 0.0025f + ModdedPlayer.Stats.healthRecoveryPerSecond) * (ModdedPlayer.Stats.healthPerSecRate + 1) * ModdedPlayer.Stats.allRecoveryMult * Time.deltaTime);

					TheForest.Utils.Scene.HudGui.HealthBarTarget.enabled = true;
				}
				else
				{
					TheForest.Utils.Scene.HudGui.HealthBarTarget.enabled = false;
				}
				if (Health < 20f)
				{
					Hud.HealthBarOutline.SetActive(true);
				}
				else
				{
					Hud.HealthBarOutline.SetActive(false);
				}
				if (Energy < 15f || IsCold)
				{
					Hud.EnergyBarOutline.SetActive(true);
				}
				else
				{
					Hud.EnergyBarOutline.SetActive(false);
				}
				if (Stamina < 10f)
				{
					Hud.StaminaBarOutline.SetActive(true);
				}
				else
				{
					Hud.StaminaBarOutline.SetActive(false);
				}
				if (Stamina < 0f)
				{
					Stamina = 0f;
				}
				if (Stamina < Energy)
				{
					if (!LocalPlayer.FpCharacter.running && !(LocalPlayer.FpCharacter.recoveringFromRun > 0f))
					{
						Stamina += ModdedPlayer.Stats.TotalStaminaRecoveryAmount * Time.deltaTime;
						Energy += ModdedPlayer.Stats.energyRecoveryperSecond.Value * ModdedPlayer.Stats.TotalEnergyRecoveryMultiplier * Time.deltaTime;
					}
					else if (LocalPlayer.FpCharacter.recoveringFromRun > 0f && Thirst < 1)
					{
						LocalPlayer.FpCharacter.recoveringFromRun -= Time.deltaTime;
					}
				}
				else
				{
					Stamina = Energy;
					Energy += ModdedPlayer.Stats.energyRecoveryperSecond.Value * ModdedPlayer.Stats.TotalEnergyRecoveryMultiplier * Time.deltaTime;
				}
				if (CheckingBlood && TheForest.Utils.Scene.SceneTracker.proxyAttackers.arrayList.Count > 0)
				{
					StopBloodCheck();
				}
				if (IsCold && !Warm && LocalPlayer.Inventory.enabled)
				{
					if (BodyTemp > 14f)
					{
						BodyTemp -= 1f * (1f - Mathf.Clamp01(ColdArmor));
					}
					if (FrostDamageSettings.DoDeFrost)
					{
						if (FrostScript.coverage > FrostDamageSettings.DeFrostThreshold)
						{
							FrostScript.coverage -= 0.0159999728f * Time.deltaTime / FrostDamageSettings.DeFrostDuration;
						}
						else
						{
							FrostDamageSettings.DoDeFrost = false;
						}
					}
					else if (FrostScript.coverage < 0.49f || ColdArmor >= 1f)
					{
						if (FrostScript.coverage < 0f)
						{
							FrostScript.coverage = 0f;
						}
						FrostScript.coverage += 0.01f * Time.deltaTime * (1f - Mathf.Clamp01(ColdArmor)) * GameSettings.Survival.FrostSpeedRatio;
						if (FrostScript.coverage > 0.492f)
						{
							FrostScript.coverage = 0.491f;
						}
					}
					else if (!Cheats.NoSurvival && TheForest.Utils.Scene.Clock.ElapsedGameTime >= FrostDamageSettings.StartDay && LocalPlayer.Inventory.CurrentView != PlayerInventory.PlayerViews.Book && LocalPlayer.Inventory.CurrentView != PlayerInventory.PlayerViews.Inventory && !LocalPlayer.AnimControl.doShellRideMode)
					{
						if (!LocalPlayer.FpCharacter.jumping && (!LocalPlayer.AnimControl.onRope || !LocalPlayer.AnimControl.VerticalMovement) && !IsLit && LocalPlayer.Rigidbody.velocity.sqrMagnitude < 0.3f && !Dead)
						{
							if (FrostDamageSettings.CurrentTimer >= FrostDamageSettings.Duration)
							{
								if (FrostDamageSettings.DamageChance == 0)
								{
									Hit((int)((ModdedPlayer.Stats.TotalMaxHealth * 0.015f + FrostDamageSettings.Damage) * GameSettings.Survival.FrostDamageRatio), true, DamageType.Frost);
									FrostScript.coverage = 0.506f;
									FrostDamageSettings.DoDeFrost = true;
									FrostDamageSettings.CurrentTimer = 0f;
								}
							}
							else
							{
								FrostDamageSettings.CurrentTimer += Time.deltaTime * ((1f - Mathf.Clamp01(ColdArmor)) * 1f);
							}
						}
						else
						{
							FrostDamageSettings.CurrentTimer = 0f;
						}
					}
				}
				if (Warm)
				{
					if (BodyTemp < 37f)
					{
						BodyTemp += 1f * (1f + Mathf.Clamp01(ColdArmor));
					}
					if (FrostScript.coverage > 0f)
					{
						FrostScript.coverage -= 0.01f * Time.deltaTime * (1f + Mathf.Clamp01(ColdArmor)) * GameSettings.Survival.DefrostSpeedRatio;
						if (FrostScript.coverage < 0f)
						{
							FrostScript.coverage = 0f;
						}
					}
					else
					{
						FrostDamageSettings.TakingDamage = false;
					}
					FrostDamageSettings.CurrentTimer = 0f;
				}
				if (LocalPlayer.IsInCaves)
				{
					Sanity.InCave();
				}
				if (PlayerSfx.MusicPlaying)
				{
					Sanity.ListeningToMusic();
				}
				if (Sitted)
				{
					Sanity.SittingOnBench();
				}
				Calories.Refresh();
				if (DyingEventInstance != null && !flag2 && !Dead)
				{
					UnityUtil.ERRCHECK(DyingEventInstance.set3DAttributes(UnityUtil.to3DAttributes(base.gameObject, null)));
					UnityUtil.ERRCHECK(DyingHealthParameter.setValue(Health));
				}
				if (FireExtinguishEventInstance != null)
				{
					UnityUtil.ERRCHECK(FireExtinguishEventInstance.set3DAttributes(UnityUtil.to3DAttributes(base.gameObject, null)));
				}
				if (Cheats.InfiniteEnergy)
				{
					Energy = ModdedPlayer.Stats.TotalMaxEnergy;
					Stamina = ModdedPlayer.Stats.TotalMaxEnergy;
				}
				if (Cheats.GodMode)
				{
					Health = ModdedPlayer.Stats.TotalMaxHealth;
					HealthTarget = ModdedPlayer.Stats.TotalMaxHealth;
				}
				return;
			IL_01cb:
				SetCold(false);
				FrostScript.coverage = 0f;
				goto IL_01e2;
			}
			catch (Exception E)
			{
				ModAPI.Log.Write(E.ToString());
			}
		}

		public override void AteMeds()
		{
			NormalizeHealthTarget();
			HealthTarget += ModdedPlayer.Stats.TotalMaxHealth * 0.6f;
			BleedBehavior.BloodReductionRatio = Health / ModdedPlayer.Stats.TotalMaxHealth * 1.5f;
		}

		public override void AteAloe()
		{
			NormalizeHealthTarget();
			HealthTarget += ModdedPlayer.Stats.TotalMaxHealth * 0.06f;
			BleedBehavior.BloodReductionRatio = Health / ModdedPlayer.Stats.TotalMaxHealth;
		}

		//armor now only can absorb 70% of the damage taken;
		public override int HitArmor(int damage)
		{
			int pureDmg = Mathf.RoundToInt(damage * 0.3f);

			return base.HitArmor(damage - pureDmg) + pureDmg;
		}

		//public override void Hit(int damage, bool ignoreArmor, DamageType type)
		//{
		//    //float f = damage * ModdedPlayer.Stats.allDamageTaken;
		//    //if (!ignoreArmor)
		//    //{
		//    //    f *= ModdedPlayer.Stats.armor.valueAdditiveDmgRed;
		//    //}
		//    //if (type == DamageType.Fire)
		//    //{
		//    //    f *= ModdedPlayer.Stats.magicDamageTaken;
		//    //}
		//    //damage = Mathf.RoundToInt(f);
		//    //base.Hit(damage, ignoreArmor, type);
		//    return;
		//}
		public override void HealthChange(float amount)
		{
			if (amount == 0)
			{
				return;
			}

			NormalizeHealthTarget();
			if (amount < 0f)
			{
				amount = ModdedPlayer.instance.DealDamageToShield(-amount);
				Health -= amount;
				HealthTarget -= amount * 3;
				Network.NetworkManager.SendPlayerHitmarker(transform.position, (int)amount);
			}
			else
			{
				float f = ModdedPlayer.Stats.TotalMaxHealth * 0.002f * amount * ModdedPlayer.Stats.allRecoveryMult + amount;
				HealthTarget += f;
			}
		}

		public override void Hit(int damage, bool ignoreArmor, DamageType type)
		{
			var hitEventContext = new COTFEvents.GotHitParams(damage, ignoreArmor);
			ChampionsOfForest.COTFEvents.Instance.OnGetHit.Invoke(hitEventContext);
			if (type == DamageType.Physical)
			{
				ChampionsOfForest.COTFEvents.Instance.OnGetHitPhysical.Invoke(hitEventContext);

				if (UnityEngine.Random.value > ModdedPlayer.Stats.getHitChance)
				{
					ChampionsOfForest.COTFEvents.Instance.OnDodge.Invoke();
					if (ModdedPlayer.Stats.i_isWindArmor)
					{
						//grant buffs;
						BuffDB.AddBuff(5, 84, 1.2f, 30);
						BuffDB.AddBuff(9, 85, 1.35f, 10);
						BuffDB.AddBuff(15, 86, 2000, 10);
						HealthTarget += ModdedPlayer.Stats.TotalMaxHealth * 0.05f;
					}
					Sfx.PlayWhoosh();
					return;
				}
			}
			else
			{
				ChampionsOfForest.COTFEvents.Instance.OnGetHitNonPhysical.Invoke(hitEventContext);
			}
			float f = damage * ModdedPlayer.Stats.allDamageTaken;
			if (!ignoreArmor)
			{
				f *= 1 - ModReferences.DamageReduction( Mathf.Max(0,ModdedPlayer.Stats.armor-(int)ModdedPlayer.instance.lostArmor));
			}
			if (type == DamageType.Fire)
			{
				f *= 0.01f * ModdedPlayer.Stats.TotalMaxHealth;
				f *= UnityEngine.Random.Range(0.9f, 1.4f);
				//f *= 1-ModdedPlayer.Stats.magicDamageTaken;
				f *= ModdedPlayer.Stats.fireDamageTaken;
			}
			if (ModdedPlayer.Stats.i_KingQruiesSword)
				BuffDB.AddBuff(22, 80, f, 1);

			base.Hit(damage, ignoreArmor, type);
		}

		public override void hitFromEnemy(int getDamage)
		{
			ModdedPlayer.instance.OnGetHit();
			base.hitFromEnemy(getDamage);
		}

		protected override void AteBlueBerry()
		{
			base.AteBlueBerry();
		}

		public override void AteBurnt(bool isLimb, float size, int calories)
		{
			base.AteBurnt(isLimb, size, calories);
		}

		public override void AteChocBar()
		{
			base.AteChocBar();
		}

		public override void AteCustom(float fullness, float health, float energy, bool isFresh, bool isMeat, bool isLimb, int calories)
		{
			base.AteCustom(fullness, health, energy, isFresh, isMeat, isLimb, calories);
		}

		public override void AteEdibleMeat(bool isLimb, float size, int calories)
		{
			base.AteEdibleMeat(isLimb, size, calories);
		}

		public override void AteFreshMeat(bool isLimb, float size, int calories)
		{
			base.AteFreshMeat(isLimb, size, calories);
		}

		protected override void AteMealRabbit()
		{
			base.AteMealRabbit();
		}

		protected override void AteMushroomAman()
		{
			base.AteMushroomAman();
		}

		protected override void AteMushroomChant()
		{
			base.AteMushroomChant();
		}

		protected override void AteMushroomDeer()
		{
			base.AteMushroomDeer();
		}

		protected override void AteMushroomJack()
		{
			base.AteMushroomJack();
		}

		protected override void AteMushroomLibertyCap()
		{
			base.AteMushroomLibertyCap();
		}

		protected override void AteMushroomPuffMush()
		{
			base.AteMushroomPuffMush();
		}

		protected override void AtePlaneFood()
		{
			base.AtePlaneFood();
		}

		public override void PoisonMe()
		{
			this.Hit(Mathf.CeilToInt((ModdedPlayer.Stats.TotalMaxHealth * 0.02f + 2f) * TheForest.Utils.Settings.GameSettings.Survival.PoisonDamageRatio), true, global::PlayerStats.DamageType.Poison);
		}

		public override void HitWaterDelayed(int damage)
		{
			base.HitWaterDelayed(damage);
			this.Hit(Mathf.CeilToInt((ModdedPlayer.Stats.TotalMaxHealth * 0.01f * damage) * TheForest.Utils.Settings.GameSettings.Survival.PolutedWaterDamageRatio), true, global::PlayerStats.DamageType.Poison);
		}
		public override void Burn()
		{
			ChampionsOfForest.COTFEvents.Instance.OnIgniteSelf.Invoke();
			base.Burn();
		}
		protected override void StopBurning()
		{
			ChampionsOfForest.COTFEvents.Instance.OnExtingishSelf.Invoke();
			base.StopBurning();
		}
		protected override void Explosion(float getDist)
		{
			ChampionsOfForest.COTFEvents.Instance.OnExplodeSelf.Invoke();
			base.Explosion(getDist);
		}
		protected override void HitFire()
		{
			ChampionsOfForest.COTFEvents.Instance.OnGetHitByBurning.Invoke();

			this.Hit(Mathf.CeilToInt((ModdedPlayer.Stats.TotalMaxHealth * 0.01f + 3) * this.Flammable * TheForest.Utils.Settings.GameSettings.Survival.FireDamageRatio), false, DamageType.Fire);
			if (TheForest.Utils.LocalPlayer.AnimControl.skinningAnimal)
			{
				TheForest.Utils.LocalPlayer.SpecialActions.SendMessage("forceSkinningReset");
			}
			TheForest.Utils.LocalPlayer.Animator.SetBool("skinAnimal", false);
		}

		protected override void CheckDeath()
		{
			if (global::Cheats.GodMode)
			{
				return;
			}
			if (this.Health <= 0f && !this.Dead)
				ChampionsOfForest.COTFEvents.Instance.OnTakeLethalDamage.Invoke();
			if (this.Health <= 0f && !this.Dead)
			{

				if (ModdedPlayer.Stats.perk_nearDeathExperienceUnlocked && !ModdedPlayer.Stats.perk_nearDeathExperienceTriggered)
				{
					Health = ModdedPlayer.Stats.TotalMaxHealth;
					BuffDB.AddBuff(20, 61, 0, 600);
					BuffDB.AddBuff(6, 82, 1, 10);
					BuffDB.AddBuff(26, 83, 0.1f, 10);	//90% damage reduction
					BuffDB.AddBuff(25, 99, 35, 10);		//+35 hp/s
					BuffDB.AddBuff(11, 100, 10, 10);		//+10 ep/s
					return;
				}

			

				if (TheForest.Utils.LocalPlayer.AnimControl.swimming)
				{	
					ChampionsOfForest.COTFEvents.Instance.OnDeath.Invoke();
					switch (ModSettings.dropsOnDeath)
					{
						case ModSettings.DropsOnDeathMode.All:
							Inventory.Instance.DropAll();
							break;

						case ModSettings.DropsOnDeathMode.Equipped:
							Inventory.Instance.DropEquipped();
							break;

						case ModSettings.DropsOnDeathMode.NonEquipped:
							Inventory.Instance.DropNonEquipped();
							break;
					}
					this.DeathInWater(0);
					return;
				}

				this.Dead = true;
				this.Player.enabled = false;
				if (ModSettings.killOnDowned)
					TheForest.Save.PlayerRespawnMP.KillPlayer();
				else
					this.FallDownDead();
			}
		}
		protected override void FallDownDead()
		{
			ChampionsOfForest.COTFEvents.Instance.OnDowned.Invoke();
			base.FallDownDead();
		}
	}
}