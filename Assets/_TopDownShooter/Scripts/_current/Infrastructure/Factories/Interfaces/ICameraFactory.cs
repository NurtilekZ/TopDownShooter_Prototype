using System.Threading.Tasks;
using UnityEngine;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface ICameraFactory : IFactory
    {
        Task<GameObject> CreateHeroCamera(Transform heroTransform);
    }
}