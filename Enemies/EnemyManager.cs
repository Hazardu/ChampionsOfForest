using System.Collections.Generic;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest
{
    public static class EnemyManager
    {

        public static Dictionary<ulong, EnemyProgression> hostDictionary;
        public static Dictionary<ulong, BoltEntity> allboltEntities;
        public static Dictionary<BoltEntity, ClinetEnemyProgression> clinetProgressions;
        public static Dictionary<Transform, ClinetEnemyProgression> spProgression;

        private static float LastAskedTime = 0;
        private static readonly float AskFrequency = 0.5f;

        public static void Initialize()
        {
            if (BoltNetwork.isRunning)
            {
                hostDictionary = new Dictionary<ulong, EnemyProgression>();
                allboltEntities = new Dictionary<ulong, BoltEntity>();
                clinetProgressions = new Dictionary<BoltEntity, ClinetEnemyProgression>();
                GetAllEntities();
            }
            else
            {
                spProgression = new Dictionary<Transform, ClinetEnemyProgression>();
            }
        }

        public static void AddHostEnemy(EnemyProgression ep)
        {
            hostDictionary.Add(ep.entity.networkId.PackedValue, ep);
        }
        //Gets all attached bolt entities
        public static void GetAllEntities()
        {

            allboltEntities.Clear();
            BoltEntity[] entities = GameObject.FindObjectsOfType<BoltEntity>();

            foreach (BoltEntity entity in entities)
            {
                try
                {
                    if (entity.isAttached)
                    {
                        allboltEntities.Add(entity.networkId.PackedValue, entity);
                    }
                }
                catch (System.Exception ex)
                {
                    ModAPI.Log.Write(ex.ToString());
                }
            }

        }
        //Returns clinet progression for Singleplayer 
        public static ClinetEnemyProgression GetCP(Transform tr)
        {
            if (spProgression.ContainsKey(tr))
            {
                ClinetEnemyProgression cp = spProgression[tr];
                if (Time.time <= cp.creationTime + ClinetEnemyProgression.LifeTime)
                {
                    return cp;
                }
                {
                    spProgression.Remove(tr);
                }
            }
            else
            {
                ClinetEnemyProgression cpr = new ClinetEnemyProgression(tr);
                spProgression.Add(tr, cpr);
                return cpr;
            }
            return null;
        }
        //Returns clinet progression for Multiplayer
        public static ClinetEnemyProgression GetCP(BoltEntity e)
        {
            if (e == null)
            {
                return null;
            }
            if (clinetProgressions.ContainsKey(e))
            {
                ClinetEnemyProgression cp = clinetProgressions[e];
                if (Time.time <= cp.creationTime + ClinetEnemyProgression.LifeTime)
                {
                    return cp;
                }
                else
                {
                    clinetProgressions.Remove(e);
                    return null;
                }
            }
            if (!GameSetup.IsMpClient)
            {
                return new ClinetEnemyProgression(e);
            }
            if (Time.time > LastAskedTime + AskFrequency)
            {

                Network.NetworkManager.SendLine("EE" + e.networkId.PackedValue.ToString() + ";", Network.NetworkManager.Target.OnlyServer);
                LastAskedTime = Time.time;
            }
            return null;
        }

        public static void RemoveEnemy(EnemyProgression ep)
        {
            if (ep.entity != null)
            {
                if (ep.entity.networkId != null)
                {
                    if (hostDictionary.ContainsKey(ep.entity.networkId.PackedValue))
{
                        hostDictionary.Remove(ep.entity.networkId.PackedValue);
                    }
                }
            }

        }
    }
}
