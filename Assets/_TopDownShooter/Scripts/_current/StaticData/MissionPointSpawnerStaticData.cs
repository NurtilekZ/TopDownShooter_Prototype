using System;
using UnityEngine;

namespace _current.StaticData
{
    [Serializable]
    public class MissionPointSpawnerStaticData
    {
        public string UnitId { get; }
        public MissionPointType MissionPointType { get; }
        public Vector3 Position { get; }

        public MissionPointSpawnerStaticData(string unitId, MissionPointType missionPointType, Vector3 position)
        {
            UnitId = unitId;
            MissionPointType = missionPointType;
            Position = position;
        }
    }
}