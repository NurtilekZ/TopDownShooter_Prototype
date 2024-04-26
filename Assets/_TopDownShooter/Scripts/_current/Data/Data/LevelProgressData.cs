using System;
using System.Collections.Generic;
using _current.Core.Logic.EnemySpawners;
using UnityEngine;

namespace _current.Data.Data
{
    [Serializable]
    public class LevelProgressData
    {
        public GameObject Hero { get; set; }
        public List<EnemySpawner> EnemySpawners { get; } = new();
        public List<EnemySpawner> MissionPointSpawners { get; } = new();
    }
}