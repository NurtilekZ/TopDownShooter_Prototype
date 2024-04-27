using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.Logging;
using _current.UI.Core.MVVM;

namespace _current.Services.UI
{
    public class UIService : IUIService
    {
        private readonly IUIFactory _factory;
        private readonly ILoggingService _loggingService;
        private Dictionary<IViewModel, IView> _views = new();

        public UIService(IUIFactory factory, ILoggingService loggingService)
        {
            _factory = factory;
            _loggingService = loggingService;
        }

        public async Task Open(IViewModel viewModel)
        {
            if (_views.TryGetValue(viewModel, out var cachedView))
            {
                cachedView.ShowAndBind(viewModel);
                return;
            }

            var newView = await _factory.GetOrCreateView(viewModel);
            newView.ShowAndBind(viewModel);
            _views[viewModel] = newView;
            _loggingService.LogMessage($"Open {viewModel.GetType().Name}", this, LoggingTag.UI);
        }

        public void Close<TViewModel>() where TViewModel : IViewModel
        {
            var screen = _views.FirstOrDefault(x=>x.Key is TViewModel).Value;
            screen?.HideAndUnbind();
            _loggingService.LogMessage($"Close {typeof(TViewModel).Name}", this, LoggingTag.UI);
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}