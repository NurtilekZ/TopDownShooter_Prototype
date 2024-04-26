using System;
using _current.Infrastructure.AssetManagement;
using _current.Services.Input;
using _current.UI.Core.MVVM;
using UnityEngine.InputSystem;

namespace _current.UI.Screens.Loading
{
    public class LoadingScreenViewModel : IViewModel
    {
        public bool IsInSceneCanvas => false;
        public string AssetPath => AssetsPath.LoadingScreen;

        private readonly IInputService _inputService;
        public event Action OnClickNextTip;

        public LoadingScreenViewModel(IInputService inputService)
        {
            if (inputService != null)
            {
                _inputService = inputService;
                _inputService.UI.Submit.performed += ShowNextTip;
            }
        }

        private void ShowNextTip(InputAction.CallbackContext obj)
        {
            OnClickNextTip?.Invoke();
        }

        public void Dispose()
        {
            if (_inputService != null) 
                _inputService.UI.Submit.performed -= ShowNextTip;
        }
    }
}