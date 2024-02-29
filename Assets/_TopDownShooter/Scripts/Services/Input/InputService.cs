using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input
{
    public class InputService : IInputService
    {
        private PlayerControls _controls;

        public InputService()
        {
            _controls = new PlayerControls();
            _controls.Enable();
            SubscribeOnControls(true);
        }

        public Vector2 MoveAxis { get; private set; }
        public Vector2 AimAxis { get; private set; }
        public event Action AttackPressed;

        ~InputService()
        {
            SubscribeOnControls(false);
            _controls.Disable();
        }

        private void SubscribeOnControls(bool value)
        {
            if (value)
            {
                _controls.Player.Movement.performed += OnMove;
                _controls.Player.Rotation.performed += OnLook;
                _controls.Player.Attack.performed += OnAttack;
                _controls.Player.Movement.canceled += OnMove;
            }
            else
            {
                _controls.Player.Movement.performed -= OnMove;
                _controls.Player.Rotation.performed -= OnLook;
                _controls.Player.Attack.performed -= OnAttack;
                _controls.Player.Movement.canceled -= OnMove;
            }
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveAxis = ctx.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            AimAxis = ctx.ReadValue<Vector2>();
        }

        private void OnAttack(InputAction.CallbackContext ctx)
        {
            AttackPressed?.Invoke();
        }
    }
}