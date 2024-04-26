using UnityEngine;

namespace _current.StaticData.ScriptableObjects
{
    [CreateAssetMenu(menuName = "StaticData/Mission", fileName = "MissionData")]
    public class MissionPointStaticData : ScriptableObject
    {
        public MissionPointType missionPointType;
    }
}