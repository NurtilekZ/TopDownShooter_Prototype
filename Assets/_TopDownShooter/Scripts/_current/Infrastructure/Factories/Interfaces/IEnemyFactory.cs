using System.Threading.Tasks;
using _current.Core.Pawns.Enemy;
using UnityEngine;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface IEnemyFactory : IFactory
    {
        Task<GameObject> Create(EnemyTypeId enemyType, Transform parent);
    }
}