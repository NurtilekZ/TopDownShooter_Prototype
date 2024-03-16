using _old.Player;
using DG.Tweening;
using Shaders;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Zenject;

namespace _old.Components
{
    public class PlayerRotation : PawnComponent<PlayerPawn>
    {
        [SerializeField] private float _followSpeed = 2f;
        [SerializeField] private float _lookForwardDelay = 2f;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _minRange;
        [SerializeField] private float _animDamp;
        [SerializeField] private float _maxTargetDistance;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private LayerMask _aimLayerMask;
        [SerializeField] private MultiAimConstraint _multiAimConstraint;
        [SerializeField] private RectTransform _crosshairImage;
        [SerializeField] private PlayerLocomotion _playerLocomotion;
        
        [SerializeField] private bool _rotate;
        
        private Vector3 _rotationDirection;
        private float _angle;
        private float _lookForwardTime;
        private Vector3 _direction;

        [Inject]
        public void Construct(TargetPawn targetPoint, CrosshairUI crosshair)
        {
            _targetTransform = targetPoint.transform;
            _crosshairImage = crosshair.GetComponent<RectTransform>();
        }
        
        public override void SetupPlayerInput()
        {
            _pawn.PlayerControls.Player.Rotation.performed += AssignRotation;
            _multiAimConstraint.data.sourceObjects.SetTransform(0, _targetTransform);
            _pawn.BuildRig();
        }

        private void AssignRotation(InputAction.CallbackContext ctx)
        {
            if (ctx.control.device == Gamepad.current)
            {
                var direction = ctx.ReadValue<Vector2>() * _maxTargetDistance;
                _rotationDirection = new Vector3(direction.x, 1, direction.y);
            }
            
            if (ctx.control.device == Mouse.current)
            {
                var ray = _pawn.PlayerCamera.ScreenPointToRay(ctx.ReadValue<Vector2>());
                if (Physics.Raycast(ray, out var hit, _aimLayerMask))
                {
                    _rotationDirection = hit.point;
                    _rotationDirection -= transform.position;
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
            if (_pawn.Animator.GetBool(AnimationStatics.Idle))
            {
                var dir = Quaternion.AngleAxis(_angle, _pawn.transform.up) * _pawn.transform.forward;

                _pawn.Animator.SetFloat(AnimationStatics.Rotation, _angle, _animDamp, Time.deltaTime);
                Debug.DrawLine(_pawn.transform.position, _pawn.transform.position + dir * 10, Color.cyan);
            }
        }

        private void AimToTarget()
        {
            _crosshairImage.position = Mouse.current.position.value;
            var dis = _rotationDirection + transform.position;

            _targetTransform.position = dis;
            var distance = Vector3.Distance(_rotationDirection, _pawn.transform.position);

            if (distance > 1)
            {
                CalculateAngle();
                if (_pawn.Animator.GetBool(AnimationStatics.Idle))
                {
                    RotateIdle();
                }
                else
                {
                    Rotate();
                }
            } 
        }

        private void CalculateAngle()
        {
            var forward = _pawn.transform.forward.normalized;
            var rotation = _rotationDirection.normalized;
            forward.y = 0;
            rotation.y = 0;
            _angle = Vector3.SignedAngle(forward, rotation, Vector3.up);
            _angle = Mathf.Clamp(_angle, -90, 90);
        }

        private void Rotate()
        {
            _direction = Quaternion.AngleAxis(_angle, Vector3.up) * _pawn.transform.forward;
            _direction.y = 0;
            var rotation = Quaternion.LookRotation(_direction, Vector3.up);
            var rot = Quaternion.RotateTowards(_pawn.transform.rotation, rotation, _followSpeed * Time.deltaTime);
            _pawn.transform.rotation = rot;
        }

        private void RotateIdle()
        {
            if (Mathf.Abs(_angle) > _maxRange)
            {
                _rotate = true;
            }
            if (Mathf.Abs(_angle) < _minRange)
            {
                _rotate = false;
            }

            if (_rotate)
            {
                Rotate();
            }
        }

        public override void Dispose()
        {
            _pawn.PlayerControls.Player.Rotation.performed -= AssignRotation;
            base.Dispose();
        }
    }
}