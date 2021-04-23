using System;
using System.Collections.Generic;

using ChampionsOfForest.Fun;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public class SpellCaster : MonoBehaviour
	{
		#region Instance

		public static SpellCaster instance;

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
		}

		#endregion Instance

		public SpellInfo[] infos;
		public bool[] Ready;
		public static int SpellCount = 6;

		public void SetSpell(int i, Spell spell = null)
		{
			if (infos[i].spell != null)
			{
				if (infos[i].spell.passive != null)
				{
					infos[i].spell.passive(false);
				}

				infos[i].spell.EquippedSlot = -1;
				infos[i].spell = null;
			}
			else if (spell != null)
			{
				infos[i].spell = spell;
				infos[i].spell.EquippedSlot = i;
			}

			if (infos[i].spell != null)
			{
				if (infos[i].spell.passive != null)
				{
					infos[i].spell.passive(true);
				}
			}
			SetMaxCooldowns();
		}

		private void Start()
		{
			try
			{
				infos = new SpellInfo[SpellCount];
				Ready = new bool[SpellCount];
				for (int i = 0; i < SpellCount; i++)
				{
					infos[i] = new SpellInfo()
					{
						spell = null
					};
				}
				SetMaxCooldowns();
			}
			catch (Exception ex)
			{
				ModAPI.Log.Write(ex.ToString());
			}
		}

		private void Update()
		{
			try
			{
				for (int i = 0; i < SpellCount; i++)
				{
					if (!Ready[i])
					{
						if (infos[i].spell != null)
						{
							infos[i].Cooldown -= Time.deltaTime / ModdedPlayer.Stats.cooldown;
							if (infos[i].Cooldown <= 0 || CotfCheats.Cheat_noCooldowns)
							{
								infos[i].Cooldown = 0;
								Ready[i] = true;
								infos[i].spell.CanCast = true;
							}
						}
						else
						{
							Ready[i] = true;
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				ModAPI.Log.Write("Error1 \t" + ex.ToString());
			}
			try
			{
				bool[] castedSpells = new bool[SpellCount];
				for (int i = 0; i < SpellCount; i++)
				{
					if (infos[i].spell != null)
					{
						if (infos[i].spell.usePassiveOnUpdate)
						{
							if (infos[i].spell.passive != null)
								infos[i].spell.passive(true);
						}

						string btnname = "spell" + (i + 1).ToString();
						if ((infos[i].spell.active != null))
						{
							if (ModAPI.Input.GetButton(btnname))
							{
								if (!infos[i].spell.Channeled)
								{
									if (Ready[i] && !ModdedPlayer.Stats.silenced && LocalPlayer.Stats.Energy >= infos[i].spell.EnergyCost * ModdedPlayer.Stats.spellCostEnergyCost * ModdedPlayer.Stats.spellCost && LocalPlayer.Stats.Stamina >= infos[i].spell.EnergyCost * (1-ModdedPlayer.Stats.spellCostEnergyCost) * ModdedPlayer.Stats.spellCost && infos[i].spell.CanCast)
									{
										if (!infos[i].spell.CastOnRelease)
										{
											LocalPlayer.Stats.Energy -= infos[i].spell.EnergyCost * (1 - ModdedPlayer.Stats.SpellCostToStamina) * ModdedPlayer.Stats.spellCost;
											if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
												LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
											LocalPlayer.Stats.Stamina -= infos[i].spell.EnergyCost * ModdedPlayer.Stats.SpellCostToStamina * ModdedPlayer.Stats.spellCost;

											ChampionsOfForest.COTFEvents.Instance.OnAnySpellCast.Invoke();
											InfinityCooldownReduction();
											Ready[i] = false;
											MaxCooldown(i);
											infos[i].spell.active();
											castedSpells[i] = true;
										}
										else
										{
											infos[i].spell.aim?.Invoke();
										}
									}
								}
								else
								{
									if (Ready[i] && !ModdedPlayer.Stats.silenced)
									{
										if (LocalPlayer.Stats.Energy >= 10 && LocalPlayer.Stats.Stamina >= 10 && infos[i].spell.CanCast)
										{
											LocalPlayer.Stats.Energy -= Time.deltaTime * infos[i].spell.EnergyCost * ModdedPlayer.Stats.spellCostEnergyCost * ModdedPlayer.Stats.spellCost;
											if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
												LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
											LocalPlayer.Stats.Stamina -= Time.deltaTime * infos[i].spell.EnergyCost * ModdedPlayer.Stats.SpellCostToStamina * ModdedPlayer.Stats.spellCost;

											ChampionsOfForest.COTFEvents.Instance.OnChanneledSpellCast.Invoke();


											infos[i].spell.active();
											infos[i].spell.ChanneledTime += Time.deltaTime;
											castedSpells[i] = true;
											infos[i].spell.active();
										}
										else
										{
											MaxCooldown(i);
										}
									}
								}
							}
							if (infos[i].spell.CastOnRelease && ModAPI.Input.GetButtonUp(btnname))
							{
								infos[i].spell.aimEnd?.Invoke();
								if (Ready[i] && !ModdedPlayer.Stats.silenced && LocalPlayer.Stats.Energy >= infos[i].spell.EnergyCost * (1 - ModdedPlayer.Stats.SpellCostToStamina) * ModdedPlayer.Stats.spellCost && LocalPlayer.Stats.Stamina >= infos[i].spell.EnergyCost * ModdedPlayer.Stats.SpellCostToStamina * ModdedPlayer.Stats.spellCost && infos[i].spell.CanCast)
								{
									LocalPlayer.Stats.Energy -= infos[i].spell.EnergyCost * (1 - ModdedPlayer.Stats.SpellCostToStamina) * ModdedPlayer.Stats.spellCost;
									if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
										LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
									LocalPlayer.Stats.Stamina -= infos[i].spell.EnergyCost * ModdedPlayer.Stats.SpellCostToStamina * ModdedPlayer.Stats.spellCost;

									ChampionsOfForest.COTFEvents.Instance.OnAnySpellCast.Invoke();

									InfinityCooldownReduction();
									Ready[i] = false;
									MaxCooldown(i);
									infos[i].spell.active();
									castedSpells[i] = true;
								}
							}
						}
					}
					if (!castedSpells[i] && infos[i].spell != null)
					{
						infos[i].spell.ChanneledTime = 0;
					}
				}
			}
			catch (System.Exception ex)
			{
				Debug.Log(ex.ToString());
				ModAPI.Log.Write(ex.ToString());
			}
		}

		public void SetMaxCooldowns()
		{
			for (int i = 0; i < 6; i++)
			{
				MaxCooldown(i);
			}
		}

		public void MaxCooldown(int i)
		{
			if (infos[i].spell != null)
			{
				infos[i].Cooldown = infos[i].spell.Cooldown;
				Ready[i] = false;
			}
		}

		public class SpellInfo
		{
			public Spell spell;
			public float Cooldown;
		}

		#region InfinityPerk


		public void InfinityCooldownReduction()
		{
			if (!ModdedPlayer.Stats.perk_infinityMagic)
				return;
			for (int i = 0; i < infos.Length; i++)
			{
				infos[i].Cooldown *= 0.95f;
			}
		}

		#endregion InfinityPerk


		public static void InfinityLoopEffect()
		{
			if (ModdedPlayer.Stats.i_infinityLoop)
			{
				var keys = new List<int>();
				for (int i = 0; i < SpellCount; i++)
				{
					if (instance.infos[i].Cooldown > 0)
						keys.Add(i);
				}
				if (keys.Count == 0)
					return;
				int randomI = UnityEngine.Random.Range(0, keys.Count);
				instance.infos[keys[randomI]].Cooldown--;
			}
		}

		public static bool RemoveStamina(float cost)
		{
			float realcostS = cost * (1 - ModdedPlayer.Stats.spellCostEnergyCost) * ModdedPlayer.Stats.spellCost;
			float realcostE = cost * ModdedPlayer.Stats.spellCostEnergyCost * ModdedPlayer.Stats.spellCost;
			if (LocalPlayer.Stats.Energy < realcostE && LocalPlayer.Stats.Stamina < realcostS)
				return false;
			LocalPlayer.Stats.Energy -= realcostE;
			if (LocalPlayer.Stats.Stamina > LocalPlayer.Stats.Energy)
				LocalPlayer.Stats.Stamina = LocalPlayer.Stats.Energy;
			LocalPlayer.Stats.Stamina -= realcostS;
			return true;
		}
	}
}