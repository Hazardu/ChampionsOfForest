using System.Collections.Generic;
using System.Linq;

using ChampionsOfForest.Network.Commands;

using UnityEngine;

namespace ChampionsOfForest
{
	public class ServerPlayerState
	{
		public class ModdedPlayerState
		{
			public int level;
			public float maxHealth;
			public float health;
			public Transform hand;
			public GameObject gameObject;
			public string playerID;
			public BoltEntity entity;
			public IPlayerState State => entity.GetState<IPlayerState>();

			public void FindHand()
			{
				hand = ModReferences.FindDeepChild(gameObject.transform.root, "rightHandHeld");
			}
			public void UpdatePlayerLevel()
			{
				var playerNameComponent = gameObject.GetComponentInChildren<TheForest.UI.Multiplayer.PlayerName>();
				playerNameComponent.SendMessage("SetNameWithLevel", level);
			}
		}

		private Dictionary<string, ModdedPlayerState> players_lookup_name = new Dictionary<string, ModdedPlayerState>();
		private Dictionary<GameObject, ModdedPlayerState> players_lookup_gameobject = new Dictionary<GameObject, ModdedPlayerState>();
		private Dictionary<BoltEntity, ModdedPlayerState> players_lookup_entity = new Dictionary<BoltEntity, ModdedPlayerState>();

		public ModdedPlayerState[] All => players_lookup_name.Values.ToArray();

		public ModdedPlayerState GetPlayerState(string playerID)
		{
			if (players_lookup_name.TryGetValue(playerID, out var state))
			{
				return state;
			}
			else
			{
				return null;
			}
		}
		public ModdedPlayerState GetPlayerState(GameObject go)
		{
			if (players_lookup_gameobject.TryGetValue(go, out var state))
			{
				return state;
			}
			else
			{
				return null;
			}
		}

		public ModdedPlayerState GetPlayerState(BoltEntity entity)
		{
			if (players_lookup_entity.TryGetValue(entity, out var state))
			{
				return state;
			}
			else
			{
				return null;
			}
		}

		public bool AddPlayerState(ModdedPlayerState state)
		{
			if (state.entity != null && state.gameObject != null)
			{
				if (state.hand == null)
					state.FindHand();

				if (players_lookup_entity.ContainsKey(state.entity))
					players_lookup_entity[state.entity] = state;
				else
					players_lookup_entity.Add(state.entity, state);

				if (players_lookup_gameobject.ContainsKey(state.gameObject))
					players_lookup_gameobject[state.gameObject] = state;
				else
					players_lookup_gameobject.Add(state.gameObject, state);

				if (players_lookup_name.ContainsKey(state.playerID))
					players_lookup_name[state.playerID] = state;
				else
					players_lookup_name.Add(state.playerID, state);

				return true;
			}
			return false;
		}
		public void UpdateOrAddPlayerState(GetPlayerStateParams commandparams)
		{
			var state = GetPlayerState(commandparams.playerID);
			if (state == null || state.entity == null || state.gameObject == null)
			{
				var _entity = BoltNetwork.FindEntity(new Bolt.NetworkId(commandparams.entityNetworkID));
				AddPlayerState(new ModdedPlayerState()
				{
					level = commandparams.level,
					maxHealth = (int)commandparams.maxHealth,
					health = (int)commandparams.health,
					playerID = commandparams.playerID,
					entity = _entity,
					gameObject = _entity.gameObject,
				});
			}
			else
			{
				//avoid calling FindHand() if not needed
				state.level = commandparams.level;
				state.maxHealth = commandparams.maxHealth;
				state.health = commandparams.health;
				state.playerID = commandparams.playerID;
			}
		}
	}
}