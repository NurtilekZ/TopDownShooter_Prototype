using _current.UI.Core;
using UnityEngine.InputSystem;

namespace _current.UI.Screens.GameOver
{
    public class GameOverView : BaseView<GameOverScreenViewModel>
    {
        protected override void Bind()
        {
            _viewModel.InputService.UI.Submit.performed += GoToMainMenu;
            _viewModel.InputService.UI.Cancel.performed += Restart;
        }

        private void Restart(InputAction.CallbackContext obj)
        {
            _viewModel.RestartLevel();
        }

        private void GoToMainMenu(InputAction.CallbackContext obj)
        {
            _viewModel.GoToMenu();
        }

        protected override void Unbind()
        {
            
        }
    }
}