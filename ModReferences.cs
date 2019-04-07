using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public class ModReferences : MonoBehaviour
    {
        private float LevelRequestCooldown = 10;
        private float MFindRequestCooldown = 300;

        public static ClinetItemPicker ItemPicker => ClinetItemPicker.Instance;
        public static List<GameObject> Players
        {
            get;
            private set;
        }

        public static string ThisPlayerID
        {
            get;
            private set;
        }
        public static List<BoltEntity> AllPlayerEntities = new List<BoltEntity>();
        public static Dictionary<string, int> PlayerLevels = new Dictionary<string, int>();
        public static Dictionary<string, Transform> PlayerHands = new Dictionary<string, Transform>();
        public static Transform rightHandTransform = null;

        private void Start()
        {
            if (BoltNetwork.isRunning)
            {
                Players = new List<GameObject>();
                StartCoroutine(UpdateSetups());
                if (GameSetup.IsMpServer && BoltNetwork.isRunning)
                {
                    InvokeRepeating("SlowUpdate", 1, 1);
                }
                StartCoroutine(InitPlayerID());
            }
            else
            {
                Players = new List<GameObject>() { LocalPlayer.GameObject };
            }
            

        }
        
        IEnumerator InitPlayerID()
        {
            if (ModSettings.IsDedicated) yield break;

            while (LocalPlayer.Entity==null)
            {
                yield return null;
            }
            ThisPlayerID = LocalPlayer.Entity.GetState<IPlayerState>().name;
            ThisPlayerID.Replace(';', '0');
            ThisPlayerID.TrimNonAscii();
   
        }
    

        private void SlowUpdate()
        {
            if (Players.Count > 1)
            {
                LevelRequestCooldown -= 1;
                if (LevelRequestCooldown < 0)
                {
                    LevelRequestCooldown = 90;
                    Network.NetworkManager.SendLine("RLx", Network.NetworkManager.Target.Clinets);

                }
                else if (Players.Count != PlayerLevels.Count + 1)
                {
                    LevelRequestCooldown = 90;
                    PlayerLevels.Clear();
                    //PlayerLevels.Add(ThisPlayerPacked, ModdedPlayer.instance.Level);
                    Network.NetworkManager.SendLine("RLx", Network.NetworkManager.Target.Clinets);
                }
                MFindRequestCooldown--;
                if (MFindRequestCooldown <= 0)
                {
                    Network.NetworkManager.SendLine("AD", Network.NetworkManager.Target.Everyone);
                    MFindRequestCooldown = 300;

                }
            }
            else
            {
                PlayerLevels.Clear();

            }
        }
        public static void Host_RequestLevels()
        {
            if (GameSetup.IsMpServer)
            {
                Network.NetworkManager.SendLine("RLx", Network.NetworkManager.Target.Clinets);
            }
        }
        public static float DamageReduction(int armor)
        {
            float x = armor;
            x *= 0.00000714285f;
            x += 0.01f;

            float f = -1f / x;
            f += 100;
            f /= 100f;
            return f;
        }



        /// <summary>
        /// Updates the player setups and changes the static variable accordingly
        /// </summary>
        private IEnumerator UpdateSetups()
        {

            while (true)
            {
                yield return null;
                Players.Clear();
                AllPlayerEntities.Clear();
                PlayerHands.Clear();

                //if (!ModSettings.IsDedicated)
                //{
                //    //ThisPlayerPacked = LocalPlayer.Entity.networkId.PackedValue ;
                //    //if(ThisPlayerPacked==0)
                //    //ThisPlayerPacked = LocalPlayer.GameObject.GetComponent<BoltEntity>().networkId.PackedValue;
                //    //if (ThisPlayerPacked == 0)
                //    //    ModAPI.Console.Write("Still 0");
                    
                    
                //}

                //bool search = false;
                //if (PlayerHands.ContainsValue(null) || PlayerHands.Count != Scene.SceneTracker.allPlayers.Count || PlayerHands.Count == 0)
                //{
                //    PlayerHands.Clear();
                //    //search = true;
                //}

                for (int i = 0; i < Scene.SceneTracker.allPlayers.Count; i++)
                {
                    Players.Add(Scene.SceneTracker.allPlayers[i]);
                    //if (!ModSettings.IsDedicated)
                    //{
                    //    if (Scene.SceneTracker.allPlayers[i].transform.root == LocalPlayer.Transform.root)
                    //    {
                    //        ThisPlayerID = i;
                    //    }
                    //}
                    BoltEntity b = Scene.SceneTracker.allPlayers[i].GetComponent<BoltEntity>();
                    if (b != null)
                    {
                        AllPlayerEntities.Add(b);
                        //try
                        //{

                        //    if (UnityEngine.Input.GetKey(KeyCode.F8))
                        //    {
                        //        ModAPI.Console.Write("PLAYER " + b.transform.root.name);
                        //        ModAPI.Log.Write("PLAYER " + "\n\n" + ModReferences.ListAllChildren(b.transform.root, ""));
                        //    }
                        //}
                        //catch (Exception exc)
                        //{
                        //    ModAPI.Console.Write(exc.ToString());
                        //}
                        //if (search)
                        //{
                        //    Transform hand = FindDeepChild(b.transform, "rightHandHeld");
                        //    if (hand != null)
                        //    {
                        //        PlayerHands.Add(b.networkId.PackedValue, hand);
                        //        Debug.Log("FOUND HAND TRANSFORM for player " + b.networkId.PackedValue);

                        //    }
                        //    else
                        //    {
                        //        Debug.LogWarning("Couldnt find hand for player " + b.networkId.PackedValue);
                        //    }
                        //}
                    }
                }
                


                yield return new WaitForSeconds(10);

            }
        }

        public static string ListAllChildren(Transform tr, string prefix)
        {
            string s = prefix + "•" + tr.name + "\n";
            foreach (Transform child in tr)
            {
                s += ListAllChildren(child, prefix + "\t");
            }
            return s;
        }
        public static Transform FindDeepChild(Transform aParent, string aName)
        {
            Transform result = aParent.Find(aName);
            if (result != null)
            {
                return result;
            }

            foreach (Transform child in aParent)
            {
                result = FindDeepChild(child, aName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        public static Transform FindHandRetardedWay(Transform root)
        {
            try
            {
 return root.Find("player_BASE").Find("char_Hips").Find("char_Spine").Find("char_Spine1").Find("char_Spine2").Find("char_RightShoulder").Find("char_RightArm").Find("char_RightForeArm").Find("har_RightHand").Find("char_RightHandWeapon").Find("rightHandHeld");
            }
            catch (Exception e)
            {

                ModAPI.Console.Write("couldnt find hand "+e.ToString());

            }
            return null;
           
        }
    }
    public static class CotfUtils
    {
        //removes any non ascii characters from a name of the player
        public static string TrimNonAscii(this string value)
        {
            string pattern = "[^ -~]+";
            Regex reg_exp = new Regex(pattern);
            return reg_exp.Replace(value, "1");
        }
    }
}
