using System.Collections;
using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public class ModReferences : MonoBehaviour
    {
        public static ClinetItemPicker ItemPicker => ClinetItemPicker.Instance;
        public static List<CoopPlayerRemoteSetup> PlayerRemoteSetups
        {
            get;
            private set;
        }
        public static int ThisPlayerID
        {
            get;
            private set;
        }
        private List<CoopPlayerRemoteSetup> _playerSetups;
        private void Start()
        {
            _playerSetups = new List<CoopPlayerRemoteSetup>();
            StartCoroutine(UpdateSetups());

        }
        /// <summary>
        /// Updates the player setups and changes the static variable accordingly
        /// </summary>
        private IEnumerator UpdateSetups()
        {
            if (GameSetup.IsMpServer)
            {
                while (true)
                {
                    _playerSetups.Clear();
                    for (int i = 0; i < Scene.SceneTracker.allPlayerEntities.Count; i++)
                    {
                        BoltEntity o = Scene.SceneTracker.allPlayerEntities[i];
                        if (o.isAttached && o.StateIs<IPlayerState>() && (o.gameObject.activeSelf && o.gameObject.activeInHierarchy))
                        {
                            CoopPlayerRemoteSetup s = o.GetComponent<CoopPlayerRemoteSetup>();
                            if (s != null)
                            {
                                int ID = _playerSetups.Count;
                                _playerSetups.Add(s);
                                if (s.entity == LocalPlayer.Entity)
                                {
                                    ThisPlayerID = ID;
                                }
                            }
                        }
                    }
                    PlayerRemoteSetups = _playerSetups;
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

}
