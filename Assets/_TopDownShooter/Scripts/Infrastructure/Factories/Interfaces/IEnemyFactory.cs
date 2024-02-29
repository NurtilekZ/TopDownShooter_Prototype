using System.Threading.Tasks;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factories.Interfaces
{
    public interface IEnemyFactory : IFactory
    {
        Task<GameObject> Create(EnemyType enemyType, Transform parent);
    }
}