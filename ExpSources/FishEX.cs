using ChampionsOfForest.Player;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.ExpSources
{
	public class FishEX : Fish
	{
		protected override void Die()
		{
			if (!this.Dead)
			{
				if (this.typeShark)
				{
					SharkKilled();
				}
			}
				base.Die();
		}

		public void SharkKilled()
		{
			int xp = 3000;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(10);
						w.Write(xp);
					}
					ChampionsOfForest.Network.NetworkManager.SendLine(answerStream.ToArray(), ChampionsOfForest.Network.NetworkManager.Target.Everyone);
				}
			}
			else
			{
				ModdedPlayer.instance.AddKillExperience(xp);
			}
			if (!GameSetup.IsMpClient)
			{
				Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(270), LocalPlayer.Transform.position + Vector3.up * 6f);
				Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(270), LocalPlayer.Transform.position + Vector3.up * 6f);
				Network.NetworkManager.SendItemDrop(ItemDataBase.GetRandomItem(370), LocalPlayer.Transform.position + Vector3.up * 6f);
			}
		}
	}
}
