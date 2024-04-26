using System.Collections.Generic;
using _current.Services.Input;
using _current.UI.Core;
using _current.UI.Elements.ActionTips;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _current.UI.Screens.Pause
{
    public class PauseScreenView : BaseView<PauseScreenViewModel>
    {
        [SerializeField] private Button _continue;
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
                new("Select", _inputService.UI.Submit),
                new("Back", _inputService.UI.Cancel)
            };

            _controlTipsPanelView.Show(actions);
            _continue.onClick.AddListener(_viewModel.ContinueGame);
            _settings.onClick.AddListener(_viewModel.OpenSettings);
            _exitGame.onClick.AddListener(_viewModel.ExitGame);
        }

        protected override void Unbind()
        {
            _continue.onClick.RemoveListener(_viewModel.ContinueGame);
            _settings.onClick.RemoveListener(_viewModel.OpenSettings);
            _exitGame.onClick.RemoveListener(_viewModel.ExitGame);
        }
    }
}