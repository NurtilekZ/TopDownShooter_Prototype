using System.Threading.Tasks;
using _current.UI.Core.MVVM;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory : IFactory
    {
        Task CreateRootCanvas();
        Task CreateSceneRootCanvas();
        Task<IView> GetOrCreateView(IViewModel viewModel);
    }
}