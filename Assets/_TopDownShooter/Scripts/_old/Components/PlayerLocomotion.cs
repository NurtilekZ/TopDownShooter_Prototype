using _old.Player;
using Shaders;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _old.Components
{
    public class PlayerLocomotion : PawnComponent<PlayerPawn>
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _crouchSpeed = 2f;
        [SerializeField] private float _maxVelocityChange = 10f;
        private bool _isCrouching;
        private bool _isSprinting;

        private Vector3 _movementDirection;
        private float CurrentSpeed => _isSprinting ? _sprintSpeed : _isCrouching ? _crouchSpeed : _moveSpeed;

        private void Update()
        {
            UpdateAnimations();
            Move();
        }

        public override void SetupPlayerInput()
        {
            _pawn.PlayerControls.Player.Movement.performed += AssignMovement;
            _pawn.PlayerControls.Player.Movement.canceled += UnAssignMovement;
            // _pawn.PlayerControls.Player.Crouch.performed += AssignCrouch;
            // _pawn.PlayerControls.Player.Sprint.performed += AssignSprint;
            // _pawn.PlayerControls.Player.Sprint.canceled += UnAssignSprint;
        }

        private void AssignMovement(InputAction.CallbackContext ctx)
        {
            var direction = ctx.ReadValue<Vector2>();
            _movementDirection = new Vector3(direction.x, 0, direction.y);
        }

        private void UnAssignMovement(InputAction.CallbackContext ctx)
        {
            _movementDirection = Vector3.zero;
        }

        private void AssignCrouch(InputAction.CallbackContext ctx)
        {
            _isCrouching = !_isCrouching;
            _pawn.Animator.SetBool(AnimationStatics.Crouching, _isCrouching);
        }
        private void AssignSprint(InputAction.CallbackContext ctx)
        {
            _isSprinting = true;
            _pawn.Animator.SetBool(AnimationStatics.Sprinting, _isSprinting);
        }
        private void UnAssignSprint(InputAction.CallbackContext ctx)
        {
            _isSprinting = false;
            _pawn.Animator.SetBool(AnimationStatics.Sprinting, _isSprinting);
        }

        private void UpdateAnimations()
        {
            var animHor = Vector3.Dot(_movementDirection.normalized, transform.right);
            var animVer = Vector3.Dot(_movementDirection.normalized, transform.forward);
            var dir = new Vector3(animHor, 0, animVer);
            Debug.DrawLine(transform.position, transform.position + dir, Color.red);

            _pawn.Animator.SetFloat(AnimationStatics.Vertical, animVer, .25f, Time.deltaTime);
            _pawn.Animator.SetFloat(AnimationStatics.Horizontal, animHor, .25f, Time.deltaTime);
        }

        private void Move()
        {
            var animHor = Vector3.Dot(_movementDirection.normalized, _pawn.PlayerCamera.transform.right);
            var animVer = Vector3.Dot(_movementDirection.normalized, _pawn.PlayerCamera.transform.forward);
            var dir = new Vector3(animHor, 0, animVer);

            var targetVelocity = Vector3.Lerp(Vector3.zero, dir, 1);

            Debug.DrawLine(transform.position, transform.position + targetVelocity, Color.red);

            _pawn.transform.position += targetVelocity * (CurrentSpeed * Time.deltaTime);
        }

        public override void Dispose()
        {
            if (_pawn.PlayerControls != null)
            {
                _pawn.PlayerControls.Player.Movement.performed -= AssignMovement;
                _pawn.PlayerControls.Player.Movement.canceled -= UnAssignMovement;
                // _pawn.PlayerControls.Player.Crouch.performed -= AssignCrouch;
                // _pawn.PlayerControls.Player.Sprint.performed -= AssignSprint;
                // _pawn.PlayerControls.Player.Sprint.canceled -= UnAssignSprint;
            }

            base.Dispose();
        }
    }
}