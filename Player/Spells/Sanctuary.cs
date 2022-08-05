using ChampionsOfForest.Effects;
using ChampionsOfForest.Network;
using ChampionsOfForest.Player.Buffs;

using TheForest.Utils;

using UnityEngine;

namespace ChampionsOfForest.Player
{
	public partial class SpellActions
	{

		public void CreateSanctuary()
		{
			Vector3 pos = LocalPlayer.Transform.position;
			float radius = 14f;
			float healing = (ModdedPlayer.Stats.healthRecoveryPerSecond * ModdedPlayer.Stats.healthRecoveryPerSecond * 10 + 43.5f) * ModdedPlayer.Stats.allRecoveryMult;
			healing += ModdedPlayer.Stats.spellFlatDmg * ModdedPlayer.Stats.SpellDamageMult * 0.1f;
			var cmd = new CommandStream(Commands.CommandType.SPELL_SANCTUARY);
					w.Write(pos.x);
					w.Write(pos.y);
					w.Write(pos.z);
					w.Write(radius);
					w.Write(healing);
					w.Write(ModdedPlayer.Stats.spell_sanctuaryGivesImmunity.value);
					w.Write(ModdedPlayer.Stats.spell_sanctuaryRegEnergy.value);
					w.Write(ModdedPlayer.Stats.spell_sanctuaryCooldownRate);
					w.Write(ModdedPlayer.Stats.spell_sanctuarySpellCostReduction);
					w.Write(ModdedPlayer.Stats.spell_sanctuaryDamageIncrease);
					w.Write(ModdedPlayer.Stats.spell_sanctuaryDamageResistance);
					w.Write(ModdedPlayer.Stats.spell_sanctuaryDuration);
					w.Close();
				}
				NetworkManager.SendLine(answerStream.ToArray(), NetworkManager.Target.Everyone);
				answerStream.Close();
			}
		}
	}
}
