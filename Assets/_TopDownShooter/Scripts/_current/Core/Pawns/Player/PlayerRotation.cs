using System.Linq;
using _current.Core.Pawns.Components;
using _current.Services.Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Zenject;

namespace _current.Core.Pawns.Player
{
    public class PlayerRotation : PawnComponent
    {
        [SerializeField] private float _rotationSmoothTime = 0.12f;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _minRange;
        [SerializeField] private float _maxTargetDistance;
        [SerializeField] private float _targetMoveSpeed;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private LayerMask _aimLayerMask;
        [SerializeField] private MultiAimConstraint _weaponAimConstraint;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private AnimationRigHandler _animationRig;
        [SerializeField] private PawnAnimationHandler _animationHandler;

        [field: SerializeField] public bool IsRotating { get; private set; }
        public Transform TargetTransform => _targetTransform;
        public Vector3 LookAxis => _inputService.LookAxis;

        private Vector3 _rotationInput;
        private float _angle;
        private bool _isIdleRotate;
        
        private Vector3 _rotationDirection;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        protected override void Bind()
        {
            _camera = Camera.main;
            _weaponAimConstraint.data.sourceObjects.SetTransform(0, _targetTransform);
            _animationRig.BuildRig();
        }

        protected override void Unbind() { }

        private void AssignRotation()
        {
            if (_inputService.LookAxis == Vector2.zero)
            {
                _rotationInput = transform.forward * _maxTargetDistance;
                return;
            }
            
            if (_inputService.InputDevice == Gamepad.current)
            {
                var direction = _inputService.LookAxis * _maxTargetDistance;
                _rotationInput = new Vector3(direction.x, 1, direction.y);
            }
            else if (_inputService.InputDevice == Mouse.current)
            {
                var ray = _camera.ScreenPointToRay(_inputService.LookAxis);
                RaycastHit[] hit = new RaycastHit[1];
                if (Physics.RaycastNonAlloc(ray, hit, 100f,_aimLayerMask.value) > 0)
                {
                    _rotationInput = hit.First().point;
                    _rotationInput -= transform.position;
                }
            }
        }

        private void Update()
        {
            AssignRotation();
            AimToTarget();
            AnimateRotation();
        }

        private void AnimateRotation()
        {
            if (_playerMovement.IsIdle)
            {
                _animationHandler.UpdateRotation(_angle);
            }
        }

        private void AimToTarget()
        {
            _rotationDirection = transform.position + _rotationInput;
            if (_targetTransform)
            {
                _targetTransform.position = Vector3.Lerp(_targetTransform.position, _rotationDirection,
                    _targetMoveSpeed * Time.deltaTime);
            }

            var distance = Vector3.Distance(_rotationInput, transform.position);

            if (distance > 1)
            {
                CalculateAngle();
                if (_playerMovement.IsIdle)
                {
                    RotateDecorIdle();
                }
                else
                {
                    Rotate();
                }
            } 
        }

        private void CalculateAngle()
        {
            var forward = transform.forward.normalized;
            var rotation = _rotationInput.normalized;
            forward.y = 0;
            rotation.y = 0;
            _angle = Vector3.SignedAngle(forward, rotation, Vector3.up);
            _angle = Mathf.Clamp(_angle, -90, 90);
            _angle = Mathf.Round(_angle * 1000f) / 1000f;
        }

        private void RotateDecorIdle()
        {
            if (Mathf.Abs(_angle) > _maxRange)
            {
                _isIdleRotate = true;
            }
            if (Mathf.Abs(_angle) < _minRange)
            {
                _isIdleRotate = false;
            }

            if (_isIdleRotate)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            if (_inputService.IsRotating.Value)
            {
                var rotationInput = _rotationInput.normalized;
                _targetRotation = Mathf.Atan2(rotationInput.x, rotationInput.z) * Mathf.Rad2Deg;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    _rotationSmoothTime);
                
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }
    }
}