using _current.Infrastructure.AssetManagement;
using _current.Services.Input;
using _current.UI.Core.MVVM;

namespace _current.UI.Screens.GameOver
{
    public class GameOverScreenViewModel : IViewModel
    {
        public bool IsInSceneCanvas => true;
        public string AssetPath => AssetsPath.GameOverScreen;
        public readonly IInputService InputService;

        public GameOverScreenViewModel(IInputService inputService)
        {
            InputService = inputService;
        }

        public void RestartLevel()
        {
            
        }

        public void GoToMenu()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}