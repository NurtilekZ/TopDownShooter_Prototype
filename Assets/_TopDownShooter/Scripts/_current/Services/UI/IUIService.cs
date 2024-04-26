using System.Threading.Tasks;
using _current.UI.Core.MVVM;

namespace _current.Services.UI
{
    public interface IUIService : IService
    {
        Task Open(IViewModel viewModel);
        void Close<TViewModel>() where TViewModel : IViewModel;
    }
}