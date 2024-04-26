using _current.Infrastructure.AssetManagement;
using _current.Services.Input;
using _current.UI.Core.MVVM;

namespace _current.UI.Screens.Pause
{
    public class PauseScreenViewModel : IViewModel
    {
        public bool IsInSceneCanvas => true;
        public string AssetPath => AssetsPath.PauseScreen;

        public readonly IInputService inputService;

        public PauseScreenViewModel(IInputService inputService)
        {
            this.inputService = inputService;
        }

        public void Dispose()
        {
            
        }

        public void ContinueGame()
        {
            
        }

        public void OpenSettings()
        {
            
        }

        public void ExitGame()
        {
            
        }
    }
}