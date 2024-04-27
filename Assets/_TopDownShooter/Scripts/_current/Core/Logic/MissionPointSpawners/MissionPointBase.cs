using _current.StaticData;
using UniRx;
using UnityEngine;

namespace _current.Core.Logic.MissionPointSpawners
{
    public abstract class MissionPointBase : MonoBehaviour, IMissionPointBase
    {
        protected readonly ReactiveProperty<bool> _isComplete = new();
        private ObjectiveSpawnerLevelData _levelData;
        
        public IReadOnlyReactiveProperty<bool> IsComplete => _isComplete;
        public virtual MissionPointType MissionType => MissionPointType.Destroy;
        
        public virtual void Initialize(ObjectiveSpawnerLevelData levelData)
        {
            _levelData = levelData;
        }
    }
}