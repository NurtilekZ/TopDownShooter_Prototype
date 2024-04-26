using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace _current.StaticData.ScriptableObjects
{
    [CreateAssetMenu(menuName = "StaticData/Level", fileName = "LevelData")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelTitle;
        public string LevelDescription;
        public string LevelKey;

        [JsonConverter(typeof(Vector3))] 
        public Vector3 PlayerSpawnPoint;

        public List<MissionPointSpawnerStaticData> MissionPointSpawners = new();
        public List<EnemySpawnerStaticData> EnemySpawners = new();
    }
}