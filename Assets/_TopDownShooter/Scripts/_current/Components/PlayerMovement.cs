using System;
using _old.Player;
using DG.Tweening;
using Shaders;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace _old.Components
{
    public class PlayerMovement : PawnComponent<PlayerPawn>
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _dampTime = .1f;
        [SerializeField] private float _movementSmoothTime = 0.12f;
        [SerializeField] private float _lookForwardTimeout = 2f;
        [field:SerializeField] public bool IsIdle { get; private set; }
        
        [Space(10)]
        [SerializeField] private  AudioClip _landingAudioClip;
        [SerializeField] private  AudioClip[] _footstepAudioClips;
        [SerializeField] [Range(0, 1)] private  float _footstepAudioVolume = 0.5f;
        
        [Space(10)]
        [SerializeField] private  float _jumpHeight = 1.2f;
        [SerializeField] private  float _gravity = -15.0f;
        
        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private  float _jumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private  float _fallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField] private  bool _isGrounded = true;

        [Tooltip("Useful for rough ground")]
        [SerializeField] private  float _groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField] private  float _groundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField] private LayerMask _groundLayers;
        
        private bool _isSprinting;
        private bool _isJumping;


        // player
        private Vector3 _movementVelocity;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _targetRotation = 0.0f;
        private float _lookForwardTimeoutDelta;
        private float _rotationSmoothTime = .12f;


        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private Vector2 _movementInput;
        private Vector3 _smoothMovement;
        private float CurrentSpeed => _isSprinting ? _sprintSpeed : _moveSpeed;
        
        public Vector3 MoveDirection { get; private set; }
        
        protected override void Start()
        {
            base.Start();
            // reset our timeouts on start
            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;
            _lookForwardTimeoutDelta = _lookForwardTimeout;
        }

        public override void SetupPlayerInput()
        {
            _pawn.PlayerControls.Player.Movement.performed += AssignMovement;
            _pawn.PlayerControls.Player.Movement.canceled += UnAssignMovement;
            _pawn.PlayerControls.Player.Jump.performed += AssignJump;
            _pawn.PlayerControls.Player.Sprint.performed += AssignSprint;
            _pawn.PlayerControls.Player.Sprint.canceled += UnAssignSprint;
        }

        private void AssignMovement(InputAction.CallbackContext ctx)
        {
            var direction = ctx.ReadValue<Vector2>();
            _movementInput = direction;
        }

        private void UnAssignMovement(InputAction.CallbackContext ctx)
        {
            // _movementInput = Vector3.zero;
        }

        private void AssignJump(InputAction.CallbackContext ctx)
        {
            if (!_isJumping) _isJumping = true;
        }

        private void AssignSprint(InputAction.CallbackContext ctx)
        {
            _isSprinting = true;
            _pawn.Animator.SetBool(AnimationStatics.Sprinting, _isSprinting);
            _pawn.Animator.SetFloat(AnimationStatics.MotionSpeed, 1.5f);
        }

        private void UnAssignSprint(InputAction.CallbackContext ctx)
        {
            _isSprinting = false;
            _pawn.Animator.SetBool(AnimationStatics.Sprinting, _isSprinting);
            _pawn.Animator.SetFloat(AnimationStatics.MotionSpeed, 1);
        }

        private void Update()
        {
            HandleIdleState();
            JumpAndGravity();
            GroundedCheck();
            Move();
            UpdateAnimations();
        }

        private void HandleIdleState()
        {
            IsIdle = !_pawn.PlayerControls.Player.Movement.IsInProgress();
            _pawn.Animator.SetBool(AnimationStatics.Idle, IsIdle);
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
                transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            _pawn.Animator.SetBool(AnimationStatics.Grounded, _isGrounded);
        }

        private void Move()
        {
            if (!IsIdle)
            {
                _lookForwardTimeoutDelta = _lookForwardTimeout;
            }
            
            var rightNormalized = _pawn.PlayerCamera.transform.right;
            var upNormalized = _pawn.PlayerCamera.transform.up;
            rightNormalized.y = 0;
            upNormalized.y = 0;
            
            MoveDirection = IsIdle ? Vector3.zero : _movementInput.x * rightNormalized + _movementInput.y * upNormalized;
            
            _smoothMovement = Vector3.SmoothDamp(_smoothMovement, MoveDirection.normalized, ref _movementVelocity, _movementSmoothTime);
            _smoothMovement.y = 0;
            
            _controller.Move(_smoothMovement * (CurrentSpeed * Time.deltaTime) +
                             new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
        }

        private void UpdateAnimations()
        {
            var animHor = Vector3.Dot(IsIdle ? Vector3.zero : _smoothMovement.normalized, transform.right.normalized);
            var animVer = Vector3.Dot(IsIdle ? Vector3.zero : _smoothMovement.normalized, transform.forward.normalized);

            _pawn.Animator.SetFloat(AnimationStatics.Horizontal, animHor, _dampTime, Time.deltaTime);
            _pawn.Animator.SetFloat(AnimationStatics.Vertical, animVer, _dampTime, Time.deltaTime);
        }

        private void JumpAndGravity()
        {
            if (_isGrounded)
            {
                _fallTimeoutDelta = _fallTimeout;

                _pawn.Animator.SetBool(AnimationStatics.Jump, false);
                _pawn.Animator.SetBool(AnimationStatics.FreeFall, false);
                
                if (_verticalVelocity < 0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_isJumping && _jumpTimeoutDelta <= 0f)
                {
                    _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2 * _gravity);

                    _pawn.Animator.SetBool(AnimationStatics.Jump, true);
                }

                if (_jumpTimeoutDelta >= 0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = _jumpTimeout;

                if (_fallTimeoutDelta >= 0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _pawn.Animator.SetBool(AnimationStatics.FreeFall, true);
                }
                _isJumping = false;
            }
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _smoothMovement * CurrentSpeed);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z),
                _groundedRadius);
        }
        
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (_footstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, _footstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(_footstepAudioClips[index], transform.TransformPoint(_controller.center), _footstepAudioVolume);
                }
            }
        }
        
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(_landingAudioClip, transform.TransformPoint(_controller.center), _footstepAudioVolume);
            }
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