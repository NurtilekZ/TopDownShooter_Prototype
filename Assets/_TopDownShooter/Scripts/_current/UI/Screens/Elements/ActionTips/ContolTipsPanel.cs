using System.Collections.Generic;
using System.Linq;
using _current.Services.Input;
using _current.UI.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _current.UI.Elements.ActionTips
{
    public class ControlTipsPanelView : SubView<List<ActionTipViewData>>
    {
        [SerializeField] private Transform _actionsHolder;
        [SerializeField] private ActionTipView _actionTipPrefab;

        private readonly List<ActionTipView> _actionTipViews = new();
        private IInputService _inputService;

        private void Awake()
        {
            foreach (Transform child in transform) 
                Destroy(child.gameObject);
        }

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        protected override void Bind(List<ActionTipViewData> data)
        {
            if (_actionTipViews.Count > 0)
                return;
            
            foreach (var actionTipViewModel in data)
            {
                var actionView = Instantiate(_actionTipPrefab, _actionsHolder);
                actionView.Show(actionTipViewModel);
                _actionTipViews.Add(actionView);
            }

            // var playerInput = _inputService.PlayerInput;
            // if (playerInput != null)
            // {
            //     playerInput.onControlsChanged += OnDeviceChange;
            //     OnDeviceChange(playerInput);
            // }
        }

        private void OnDeviceChange(PlayerInput input)
        {
            var currentDeviceName = input.devices.First().displayName;
            foreach (var actionTipView in _actionTipViews) 
                actionTipView.UpdateButtonIcon(currentDeviceName);
        }

        protected override void Unbind()
        {
            // if (_inputService.PlayerInput != null) 
            //     _inputService.PlayerInput.onControlsChanged -= OnDeviceChange;
        }
    }
}