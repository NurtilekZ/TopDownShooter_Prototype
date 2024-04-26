using System;
using Services.Input;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _current.Services.Input
{
    public class InputService : IInputService
    {
        private Vector2 _moveAxis;
        private Vector2 _lookAxis;
        public Vector2 MoveAxis => _moveAxis;
        public Vector2 LookAxis => _lookAxis;
        
        private readonly ReactiveProperty<bool> _isMoving = new();
        private readonly ReactiveProperty<bool> _isRotating = new();
        public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
        public IReadOnlyReactiveProperty<bool> IsRotating => _isRotating;
        public bool IsAttackStillPressed => Player.Attack.IsInProgress();

        public InputDevice InputDevice { get; private set; }
        public PlayerControls.PlayerActions Player { get; private set; }
        public PlayerControls.UIActions UI { get; private set; }
        private PlayerControls _playerControls;
        public event Action OnAttackPressed;
        public event Action OnReloadPressed;
        public event Action OnChangeWeaponPressed;
        public event Action OnInteractPressed;
        public event Action OnSprintPressed;
        public event Action OnSkillPressed;
        public event Action OnRollPressed;

        public InputService()
        {
            _playerControls = new PlayerControls();

            _playerControls.Enable();
            InputSystem.onDeviceChange += OnDeviceChange;
            Player = _playerControls.Player;
            UI = _playerControls.UI;
            
            BindInput();
        }

        private void BindInput()
        {
            Player.Movement.performed += AssignMovement;
            Player.Movement.canceled += UnAssignMovement;
            Player.Rotation.canceled += UnAssignRotation;
            Player.Rotation.performed += AssignRotation;
            
            // Player.Sprint.started += AssignSprint;
            // Player.Sprint.canceled += UnAssignSprint;
            // Player.Walking.started += AssignWalking;
            // Player.Jump.started += AssignJump;
            
            Player.Attack.started += AssignAttack;
            Player.Reload.started += AssignReload;
        }

        private void UnbindInput()
        {
            Player.Movement.canceled -= UnAssignMovement;
            Player.Movement.performed -= AssignMovement;
            Player.Rotation.canceled -= UnAssignRotation;
            Player.Rotation.performed -= AssignRotation;

            // Player.Sprint.started -= AssignSprint;
            // Player.Sprint.canceled -= UnAssignSprint;
            // Player.Walking.started -= AssignWalking;
            // Player.Jump.started -= AssignJump;
            
            Player.Attack.started -= AssignAttack;
            Player.Reload.started -= AssignReload;
        }

        private void OnDeviceChange(InputDevice inputDevice, InputDeviceChange change)
        {
            InputDevice = inputDevice;
        }

        private void AssignMovement(InputAction.CallbackContext ctx) => _moveAxis = ctx.ReadValue<Vector2>();

        private void UnAssignMovement(InputAction.CallbackContext obj)
        {
            _moveAxis = Vector2.zero;
            _isMoving.Value = false;
        }


        private void AssignRotation(InputAction.CallbackContext ctx)
        {
            _lookAxis = ctx.ReadValue<Vector2>();
            _isRotating.Value = true;
        }

        private void UnAssignRotation(InputAction.CallbackContext obj)
        {
            _lookAxis = Vector2.zero;
            _isRotating.Value = false;
        }


        private void AssignAttack(InputAction.CallbackContext obj) => OnAttackPressed?.Invoke();
        private void AssignReload(InputAction.CallbackContext obj) => OnReloadPressed?.Invoke();

        public void Dispose()
        {
            _isMoving?.Dispose();
            _isRotating?.Dispose();
            
            UnbindInput();
            _playerControls?.Dispose();
        }
    }
}