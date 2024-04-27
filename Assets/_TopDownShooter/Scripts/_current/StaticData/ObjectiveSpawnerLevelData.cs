using System;
using UnityEngine;

namespace _current.StaticData
{
    [Serializable]
    public class ObjectiveSpawnerLevelData
    {
        public string UnitId { get; }
        public MissionPointType MissionPointType { get; }
        public Vector3 Position { get; }

        public ObjectiveSpawnerLevelData(string unitId, MissionPointType missionPointType, Vector3 position)
        {
            UnitId = unitId;
            MissionPointType = missionPointType;
            Position = position;
        }
    }
}