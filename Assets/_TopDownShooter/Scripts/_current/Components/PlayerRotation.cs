using _old.Player;
using Shaders;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Zenject;

namespace _old.Components
{
    public class PlayerRotation : PawnComponent<PlayerPawn>
    {
        [SerializeField] private float _rotationSmoothTime = 0.12f;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _minRange;
        [SerializeField] private float _maxTargetDistance;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private LayerMask _aimLayerMask;
        [SerializeField] private MultiAimConstraint _weaponAimConstraint;
        [SerializeField] private RectTransform _crosshairImage;
        [SerializeField] private PlayerMovement _playerMovement;

        [field: SerializeField] public bool IsRotating { get; private set; }

        private Vector3 _rotationInput;
        private float _angle;
        private bool _isIdleRotate;
        
        private Vector3 _rotationDirection;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;

        [Inject]
        public void Construct(TargetPawn targetPoint, CrosshairUI crosshair)
        {
            _targetTransform = targetPoint.transform;
            _crosshairImage = crosshair.GetComponent<RectTransform>();
        }
        
        public override void SetupPlayerInput()
        {
            _pawn.PlayerControls.Player.Rotation.performed += AssignRotation;
            _weaponAimConstraint.data.sourceObjects.SetTransform(0, _targetTransform);
            _pawn.BuildRig();
        }

        private void AssignRotation(InputAction.CallbackContext ctx)
        {
            if (ctx.control.device == Gamepad.current)
            {
                var direction = ctx.ReadValue<Vector2>() * _maxTargetDistance;
                _rotationInput = new Vector3(direction.x, 1, direction.y);
            }
            
            if (ctx.control.device == Mouse.current)
            {
                var ray = _pawn.PlayerCamera.ScreenPointToRay(ctx.ReadValue<Vector2>());
                if (Physics.Raycast(ray, out var hit, _aimLayerMask))
                {
                    _rotationInput = hit.point;
                    _rotationInput -= transform.position;
                }
            }
        }

        private void Update()
        {
            AimToTarget();
            AnimateRotation();
        }

        private void AnimateRotation()
        {
            if (_playerMovement.IsIdle)
            {
                _pawn.Animator.SetFloat(AnimationStatics.Rotation, _angle);
            }
        }

        private void AimToTarget()
        {
            _crosshairImage.position = Mouse.current.position.value;
            _rotationDirection = transform.position + _rotationInput;
            _targetTransform.position = _rotationDirection;
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
            if (_pawn.PlayerControls.Player.Rotation.IsInProgress())
            {
                var rotationInput = _rotationInput.normalized;
                _targetRotation = Mathf.Atan2(rotationInput.x, rotationInput.z) * Mathf.Rad2Deg;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    _rotationSmoothTime);
                
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _rotationInput * 2);
        }

        public override void Dispose()
        {
            _pawn.PlayerControls.Player.Rotation.performed -= AssignRotation;
            base.Dispose();
        }
    }
}