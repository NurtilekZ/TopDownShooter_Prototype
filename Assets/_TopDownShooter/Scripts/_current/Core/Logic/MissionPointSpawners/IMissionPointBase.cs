using _current.StaticData;
using UniRx;

namespace _current.Core.Logic.MissionPointSpawners
{
    public interface IMissionPointBase
    {
        IReadOnlyReactiveProperty<bool> IsComplete { get; }
        MissionPointType MissionType { get; }
        void Initialize(MissionPointSpawnerStaticData staticData);
    }
}