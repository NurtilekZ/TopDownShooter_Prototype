using _current.StaticData;
using UniRx;
using UnityEngine;

namespace _current.Core.Logic.MissionPointSpawners
{
    public abstract class MissionPointBase : MonoBehaviour, IMissionPointBase
    {
        [SerializeField] protected MissionPointType _missionType;
        [SerializeField] protected ReactiveProperty<bool> _isComplete = new();
        private MissionPointSpawnerStaticData _missionPointSpawnerStaticData;


        public IReadOnlyReactiveProperty<bool> IsComplete => _isComplete;

        public MissionPointType MissionType => _missionType;
        
        public virtual void Initialize(MissionPointSpawnerStaticData staticData)
        {
            _missionPointSpawnerStaticData = staticData;
        }
    }
}