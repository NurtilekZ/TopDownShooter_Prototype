using System.Threading.Tasks;
using UnityEngine;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface IHeroFactory : IFactory
    {
        GameObject Hero { get; }
        Task<GameObject> Create(Vector3 at);
    }
}