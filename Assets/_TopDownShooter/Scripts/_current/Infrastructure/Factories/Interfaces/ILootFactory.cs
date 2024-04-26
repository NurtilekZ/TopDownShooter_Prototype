using System.Threading.Tasks;
using _current.Core.Pawns.LootSystem;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface ILootFactory : IFactory
    {
        Task<LootView> Create(string assetPath);
    }
}