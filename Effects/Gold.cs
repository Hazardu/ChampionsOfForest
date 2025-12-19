using BuilderCore;

using ChampionsOfForest.Player;

using TheForest.Utils;

using UnityEngine;

using static ChampionsOfForest.Player.BuffDB;

namespace ChampionsOfForest.Effects
{
	public class GoldenSkin
	{
		public static Material originalMaterial;
		public static Material goldMaterial;
		public static Renderer renderer;

		public static float Duration = 40;

		public static void Cast()
		{
			AddBuff(16, 39, 1, Duration);
		}

		public static void Enable()
		{
			if (renderer == null)
			{
				var rends = LocalPlayer.GameObject.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < rends.Length; i++)
				{
					if (rends[i].gameObject.name.StartsWith("char_arms"))
						renderer = rends[i];
				}
				//renderer = LocalPlayer.PlayerBase.GetComponent<Renderer>();
				originalMaterial = renderer.material;
			}
			if (renderer == null)
			{
				ModAPI.Console.Write("GOLD: no renderer");
				return;
			}
			//ModAPI.Console.Write("GOLD: " + renderer.ToString());
			if (goldMaterial == null)
			{
				goldMaterial = Core.CreateMaterial(new BuildingData() { MainColor = new Color(1, 0.74f, 0.2122f), Metalic = 1, Smoothness = 1f, EmissionColor = new Color(0.125f, 0.1f, 0.025f, 0.1f) });
			}
			renderer.material = goldMaterial;
			//var rends = LocalPlayer.GameObject.GetComponentsInChildren<Renderer>();
			//for (int i = 0; i < rends.Length; i++)
			//{
			//    ModAPI.Log.Write(rends[i].ToString());
			//}
			ModdedPlayer.Stats.stunImmunity.valueAdditive++;
			ModdedPlayer.Stats.debuffResistance.Add(1);
			ModdedPlayer.Stats.attackSpeed.Multiply( 1.2f);
			if (ModdedPlayer.Stats.perk_goldenResolve)
				ModdedPlayer.Stats.allDamageTaken.Multiply( 0.5f);
		}

		public static void Disable()
		{
			renderer.material = originalMaterial;
			ModdedPlayer.Stats.stunImmunity.Sub(1);
			ModdedPlayer.Stats.debuffResistance.Sub(1);

			ModdedPlayer.Stats.attackSpeed .Divide( 1.2f);
			if (ModdedPlayer.Stats.perk_goldenResolve)
				ModdedPlayer.Stats.allDamageTaken .Divide( 0.5f);
		}
	}
}