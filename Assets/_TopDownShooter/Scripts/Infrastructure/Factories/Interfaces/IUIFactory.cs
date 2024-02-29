using System.Threading.Tasks;
using Meta.Hud;

namespace Infrastructure.Factories.Interfaces
{
    public interface IUIFactory : IFactory
    {
        Task CreateUIRoot();
        Task<HUDController> CreateHud();
    }
}