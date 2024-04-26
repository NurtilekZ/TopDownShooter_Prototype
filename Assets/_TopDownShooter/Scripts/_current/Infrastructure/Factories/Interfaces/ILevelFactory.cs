using System.Threading.Tasks;
using _current.Core.Logic.EnemySpawners;
using _current.Core.Logic.MissionPointSpawners;
using _current.StaticData;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface ILevelFactory : IFactory
    {
        Task<EnemySpawner> CreateSpawner(EnemySpawnerStaticData spawnerData);
        Task<IMissionPointBase> CreateMissionPoint(MissionPointSpawnerStaticData missionPointSpawner);
    }
}