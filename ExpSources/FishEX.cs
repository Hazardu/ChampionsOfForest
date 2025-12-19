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
			int xp = 1500;
			if (GameSetup.IsMultiplayer)
			{
				using (System.IO.MemoryStream answerStream = new System.IO.MemoryStream())
				{
					using (System.IO.BinaryWriter w = new System.IO.BinaryWriter(answerStream))
					{
						w.Write(10);
						w.Write((long)xp);
					}
					Network.NetworkManager.SendLine(answerStream.ToArray(), Network.NetworkManager.Target.Everyone);
				}
			}
			else
			{
				ModdedPlayer.instance.AddKillExperience(xp);
			}
			if (!GameSetup.IsMpClient)
			{
					Network.NetworkManager.SendItemDrop(ItemDatabase.GetRandomItem(270,transform.position), LocalPlayer.Transform.position + Vector3.up * 6f, ItemPickUp.DropSource.EnemyOnDeath);
					Network.NetworkManager.SendItemDrop(ItemDatabase.GetRandomItem(310, transform.position), LocalPlayer.Transform.position + Vector3.up * 6f, ItemPickUp.DropSource.EnemyOnDeath);
					Network.NetworkManager.SendItemDrop(ItemDatabase.GetRandomItem(370, transform.position), LocalPlayer.Transform.position + Vector3.up * 6f, ItemPickUp.DropSource.EnemyOnDeath);
			}
		}
	}
}
