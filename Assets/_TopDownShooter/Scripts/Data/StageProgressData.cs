using System.Collections.Generic;
using Core.Logic;
using UnityEngine;

namespace Data
{
    public class StageProgressData
    {
        public GameObject Hero { get; set; }
        public List<EnemySpawner> EnemySpawners { get; } = new();
    }
}