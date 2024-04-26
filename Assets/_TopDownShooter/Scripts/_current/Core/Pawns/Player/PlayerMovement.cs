using _current.Core.Pawns.Components;
using _current.Data;
using _current.Services.Input;
using _current.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Random = UnityEngine.Random;

namespace _current.Core.Pawns.Player
{
    public class PlayerMovement : PawnComponent
    {
        [SerializeField] private float _walkingSpeed = 3f;
        [SerializeField] private float _runningSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _speedChangeRate = 10.0f;
        [SerializeField] private float _dampTime = 0.1f;
        [SerializeField] private float _footstepTimeout = 1.5f;
        [SerializeField] private Camera _camera;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AnimationHandler _animationHandler;
        [SerializeField] private CharacterController _controller;

        [SerializeField] private  AudioClip[] _footstepAudioClips;
        [SerializeField] [Range(0, 1)] private  float _footstepAudioVolume = 0.5f;
        
        private float _speed;
        private Vector3 _movementVelocity;
        private float _footstepTimeoutDelta;
        private Vector3 _smoothMovement;
        private float _animMotionSpeed;
        private IInputService _inputService;

        private Vector3 MoveDirection { get; set; }
        public bool IsIdle => !_inputService.IsMoving.Value;

        private float CurrentSpeed => 
            // _inputService.IsSprintClicked.Value ? _sprintSpeed : _inputService.IsWalkClicked.Value ? _walkingSpeed : 
            _runningSpeed;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        protected override void Bind()
        {
            _camera = Camera.main;
            _footstepTimeoutDelta = _footstepTimeout;
            // _inputService.IsSprintClicked.Subscribe(_animationHandler.SetIsSprinting).AddTo(_disposables);
            // _inputService.IsWalkClicked.Subscribe(_animationHandler.SetIsWalking).AddTo(_disposables);
            _inputService.IsMoving.Subscribe(x=> _animationHandler.SetIdle(!x)).AddTo(_disposables);
        }

        protected override void Unbind() { }

        private void Update()
        {
            Move();
            UpdateAnimations();
        }
        
        private void Move()
        {
            float targetSpeed = CurrentSpeed;
            
            if (_inputService.MoveAxis == Vector2.zero) targetSpeed = 0.0f;

            var velocity = _controller.velocity;
            float currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

            float speedOffset = 0.1f;
            _animMotionSpeed = _inputService.InputDevice == Gamepad.current ? _inputService.MoveAxis.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * _animMotionSpeed,
                    Time.deltaTime * _speedChangeRate);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            
            MoveDirection = new Vector3(_inputService.MoveAxis.x, 0.0f, _inputService.MoveAxis.y);

            var targetDirection = Vector3.zero;
            
            var isIdle = IsIdle;
            if (!isIdle)
            {
                var targetRotation =
                    Mathf.Atan2(MoveDirection.normalized.x, MoveDirection.normalized.z) * Mathf.Rad2Deg +
                    _camera.transform.eulerAngles.y;

                targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            }

            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));
                             // + new Vector3(0.0f, 0.0f, 0.0f) * Time.deltaTime);

            if (!isIdle)
            {
                _footstepTimeoutDelta -= CurrentSpeed * Time.deltaTime;
                if (_footstepTimeoutDelta <= 0)
                {
                    OnFootstep();
                }
            }
        }

        private void UpdateAnimations()
        {
            _animationHandler.UpdateMovements(MoveDirection, _dampTime);
            _animationHandler.UpdateMotionSpeed(_animMotionSpeed);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        
        private void OnFootstep()
        {
            _footstepTimeoutDelta = _footstepTimeout;
            if (_footstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, _footstepAudioClips.Length);
                _audioSource.PlayOneShot(_footstepAudioClips[index], _footstepAudioVolume);
            }
        }
        
        private void Warp(Vector3Data to)
        {
            _controller.enabled = false;
            transform.position = to.AsUnityVector3().AddY(_controller.height);
            _controller.enabled = true;
        }
    }
}