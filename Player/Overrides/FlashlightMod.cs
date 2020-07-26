using TheForest.Items.World;
using TheForest.Utils;

using UnityEngine;

using Random = UnityEngine.Random;

namespace ChampionsOfForest.Player
{
	public class FlashlightMod : BatteryBasedLight
	{
		public override void SetIntensity(float intensity)
		{
			_mainLight.intensity = ModdedPlayer.Stats.perk_flashlightIntensity * intensity;

			float num = intensity / 2f;
			if (intensity < 0.3f)
			{
				num = intensity / 3f;
			}
			if (num > 0.5f)
			{
				num = 0.5f;
			}

			if ((bool)_fillLight)
			{
				_fillLight.intensity = ModdedPlayer.Stats.perk_flashlightIntensity * num;
			}
		}

		protected override void Update()
		{
			if (!BoltNetwork.isRunning || (BoltNetwork.isRunning && (bool)base.entity && base.entity.isAttached && base.entity.isOwner))
			{
				LocalPlayer.Stats.BatteryCharge -= _batterieCostPerSecond * Time.deltaTime / ModdedPlayer.Stats.perk_flashlightBatteryDrain;

				if (LocalPlayer.Stats.BatteryCharge > 50f)
				{
					SetIntensity(_highBatteryIntensity);
				}
				else if (LocalPlayer.Stats.BatteryCharge < 20f)
				{
					if (LocalPlayer.Stats.BatteryCharge < 10f)
					{
						if (LocalPlayer.Stats.BatteryCharge < 5f)
						{
							if (LocalPlayer.Stats.BatteryCharge < 3f && Time.time > _animCoolDown && !_skipNoBatteryRoutine)
							{
								LocalPlayer.Animator.SetBool("noBattery", true);
								_animCoolDown = Time.time + (float)Random.Range(30, 60);
								base.Invoke("resetBatteryBool", 1.5f);
							}
							if (LocalPlayer.Stats.BatteryCharge <= 0f)
							{
								LocalPlayer.Stats.BatteryCharge = 0f;
								if (_skipNoBatteryRoutine)
								{
									SetEnabled(false);
								}
								else
								{
									TorchLowerLightEvenMore();
									if (!_doingStash)
									{
										base.StartCoroutine("stashNoBatteryRoutine");
									}
									_doingStash = true;
								}
							}
							else
							{
								SetEnabled(true);
							}
						}
						else
						{
							TorchLowerLightMore();
							SetEnabled(true);
						}
					}
					else
					{
						TorchLowerLight();
						SetEnabled(true);
					}
				}
				if (BoltNetwork.isRunning)
				{
					base.state.BatteryTorchIntensity = ModdedPlayer.Stats.perk_flashlightIntensity * _mainLight.intensity;
					base.state.BatteryTorchEnabled = _mainLight.enabled;
					base.state.BatteryTorchColor = _mainLight.color;
				}
			}
			TheForestQualitySettings.UserSettings.ApplyQualitySetting(_mainLight, LightShadows.Hard);
		}
	}
}