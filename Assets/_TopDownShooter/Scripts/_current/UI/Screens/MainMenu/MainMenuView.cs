using System.Collections.Generic;
using _current.Services;
using _current.Services.Input;
using _current.UI.Core;
using _current.UI.Elements.ActionTips;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _current.UI.Screens.MainMenu
{
    public class MainMenuView : BaseView<MenuScreenViewModel>
    {
        [SerializeField] private Button _newGame;
        [SerializeField] private Button _continue;
        [SerializeField] private Button _selectLevel;
        [SerializeField] private Button _gameMode;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exitGame;
        [SerializeField] private ControlTipsPanelView _controlTipsPanelView;
        
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }
        
        protected override void Bind()
        {
            var actions = new List<ActionTipViewData>
            {
                new("Navigation", _inputService.UI.Navigate),
                new("Select", _inputService.UI.Submit)
            };

            _controlTipsPanelView.Show(actions);
            _newGame.onClick.AddListener(_viewModel.StartNewGame);
            _continue.onClick.AddListener(_viewModel.ContinueGame);
            _selectLevel.onClick.AddListener(_viewModel.OpenSelectLevel);
            _gameMode.onClick.AddListener(_viewModel.OpenSelectGameMode);
            _settings.onClick.AddListener(_viewModel.OpenSettings);
            _exitGame.onClick.AddListener(_viewModel.ExitGame);
        }

        protected override void Unbind()
        {
            _newGame.onClick.RemoveListener(_viewModel.StartNewGame);
            _continue.onClick.RemoveListener(_viewModel.ContinueGame);
            _selectLevel.onClick.RemoveListener(_viewModel.OpenSelectLevel);
            _gameMode.onClick.RemoveListener(_viewModel.OpenSelectGameMode);
            _settings.onClick.RemoveListener(_viewModel.OpenSettings);
            _exitGame.onClick.RemoveListener(_viewModel.ExitGame);
        }
    }
}