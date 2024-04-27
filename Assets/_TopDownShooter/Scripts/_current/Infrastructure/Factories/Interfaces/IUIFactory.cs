using System.Threading.Tasks;
using _current.UI.Core.MVVM;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory : IFactory
    {
        Task WarmUpForState(string[] assetPaths);
        void CleanUpForState();
        Task CreateRootCanvas();
        Task CreateSceneRootCanvas();
        Task<IView> GetOrCreateView(IViewModel viewModel);
    }
}