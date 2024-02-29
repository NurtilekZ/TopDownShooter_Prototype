using System;
using Newtonsoft.Json;
using UnityEngine;

namespace StaticData
{
    [Serializable]
    public class EnemySpawnersStaticData
    {
        public EnemyType EnemyType { get; set; }
        
        [JsonConverter(typeof(Vector3))]
        public Vector3 Position { get; set; }
    }
}