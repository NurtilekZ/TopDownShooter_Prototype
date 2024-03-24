using _current.Units.Player;
using Shaders;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace _current.Units.Components
{
    public class PlayerMovement : PawnComponent<PlayerPawn>
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerJump _playerJump; 
        [SerializeField] private float _walkingSpeed = 3f;
        [SerializeField] private float _runningSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _dampTime = .1f;
        [SerializeField] private float _movementSmoothTime = 0.12f;
        [SerializeField] private float _lookForwardTimeout = 2f;
        [field:SerializeField] public bool IsIdle { get; private set; }
        
        [Space(10)]
        [SerializeField] private  AudioClip[] _footstepAudioClips;
        [SerializeField] [Range(0, 1)] private  float _footstepAudioVolume = 0.5f;
        
            
        private ReactiveProperty<bool> _isSprinting = new();
        private ReactiveProperty<bool> _isWalking = new();

        private Vector3 _movementVelocity;

        // timeout deltatime
        private float _lookForwardTimeoutDelta;
        
        private Vector3 _movementInput;
        private Vector3 _smoothMovement;
        private float CurrentSpeed => _isSprinting.Value ? _sprintSpeed : _isWalking.Value ? _walkingSpeed : _runningSpeed;
        
        public Vector3 MoveDirection { get; private set; }

        protected override void BindInput()
        {
            if (_pawn.PlayerControls == null) return;
            
            _pawn.PlayerControls.Player.Movement.performed += AssignMovement;
            _pawn.PlayerControls.Player.Sprint.performed += AssignSprint;
            _pawn.PlayerControls.Player.Sprint.canceled += UnAssignSprint;
            _pawn.PlayerControls.Player.Walking.performed += AssignWalking;
        }

        protected override void UnbindInput()
        {
            if (_pawn.PlayerControls == null) return;
            
            _pawn.PlayerControls.Player.Movement.performed -= AssignMovement;
            _pawn.PlayerControls.Player.Sprint.performed -= AssignSprint;
            _pawn.PlayerControls.Player.Sprint.canceled -= UnAssignSprint;
            _pawn.PlayerControls.Player.Walking.canceled -= AssignWalking;
        }

        private void AssignMovement(InputAction.CallbackContext ctx)
        {
            var direction = ctx.ReadValue<Vector2>();
            _movementInput = new Vector3(direction.x, 0, direction.y);
        }

        private void AssignSprint(InputAction.CallbackContext ctx)
        {
            _isWalking.Value = false;
            _isSprinting.Value = true;
        }

        private void UnAssignSprint(InputAction.CallbackContext ctx)
        {
            _isSprinting.Value = false;
        }

        private void AssignWalking(InputAction.CallbackContext obj)
        {
            _isWalking.Value = !_isWalking.Value;
            if (_isWalking.Value && _isSprinting.Value) 
                _isSprinting.Value = false;
        }

        protected override void Awake()
        {
            base.Awake();

            _lookForwardTimeoutDelta = _lookForwardTimeout;
            
            _isWalking.Subscribe(x => { _pawn.Animator.SetBool(AnimationStatics.Walking, x); });
            _isSprinting.Subscribe(x => { _pawn.Animator.SetBool(AnimationStatics.Sprinting, x); });
        }

        protected override void Update()
        {
            base.Update();
            HandleIdleState();
            Move();
            UpdateAnimations();
        }

        private void HandleIdleState()
        {
            IsIdle = !_pawn.PlayerControls.Player.Movement.IsInProgress();
            _pawn.Animator.SetBool(AnimationStatics.Idle, IsIdle);
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
            
            MoveDirection = IsIdle ? Vector3.zero : _movementInput.x * rightNormalized + _movementInput.z * upNormalized;
            
            _smoothMovement = Vector3.SmoothDamp(_smoothMovement, MoveDirection.normalized, ref _movementVelocity, _movementSmoothTime);
            _smoothMovement.y = 0;
            
            _controller.Move(_smoothMovement * (CurrentSpeed * Time.deltaTime)
                             + new Vector3(0f,_playerJump.VerticalVelocity, 0f) * Time.deltaTime);

        }

        private void UpdateAnimations()
        {
            var animHor = Vector3.Dot(MoveDirection, transform.right);
            var animVer = Vector3.Dot(MoveDirection, transform.forward);
            _pawn.Animator.SetFloat(AnimationStatics.Horizontal, animHor, _dampTime, Time.deltaTime);
            _pawn.Animator.SetFloat(AnimationStatics.Vertical, animVer, _dampTime, Time.deltaTime);
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
        
        public override void Dispose()
        {
            _isWalking.Dispose();
            _isSprinting.Dispose();

            base.Dispose();
        }
    }
}