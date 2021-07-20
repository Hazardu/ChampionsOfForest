using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TheForest.UI.Multiplayer;

namespace ChampionsOfForest.Player
{
	public class APlayerNameOverlayMod : PlayerName
	{
		public void SetNameWithLevel(int level)
		{
			this._playerState = this._entity.GetState<IPlayerState>();
			this._overlay._name.text = _playerState.name + $" [Lv{level}]";

		}
	}
}
