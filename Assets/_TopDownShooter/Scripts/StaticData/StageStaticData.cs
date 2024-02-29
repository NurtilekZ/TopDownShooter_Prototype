using System;
using Newtonsoft.Json;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public record StageStaticData
    {
        public string StageKey { get; set; }
        public string StageTitle { get; set; }
        public string StageDescription { get; set; }
        
        [JsonConverter(typeof(Vector3))]
        public Vector3 PlayerSpawnPoint { get; }
        public EnemySpawnersStaticData[] EnemySpawners { get; }
        public MapStaticData MapData { get; }
    }
}