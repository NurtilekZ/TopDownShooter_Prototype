using System.Threading.Tasks;
using Core.Logic;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factories.Interfaces
{
    public interface IStageFactory : IFactory
    {
        Task<EnemySpawner> CreateEnemySpawner(EnemyType enemyType, Vector3 at);
    }
}