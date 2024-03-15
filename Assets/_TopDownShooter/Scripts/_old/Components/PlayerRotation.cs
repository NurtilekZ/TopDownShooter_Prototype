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
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _maxAngle;
        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxTargetDistance;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private LayerMask _aimLayerMask;
        [SerializeField] private MultiAimConstraint _multiAimConstraint;
        [SerializeField] private RectTransform _crosshairImage;
        
        [SerializeField] private bool _rotate;
        
        private Vector3 _rotationDirection;

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
        
        private void Update()
        {
            AnimateRotation();
            AimToTarget();
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
                if (Physics.Raycast(ray, out var hit, _aimLayerMask)) _rotationDirection = hit.point;
            }
        }

        private void AnimateRotation()
        {
            if (_pawn.Animator.GetBool(AnimationStatics.Idle))
            {
                var forwardNormalized = transform.forward.normalized;
                forwardNormalized.y = 0;
                var angle = Vector3.SignedAngle(forwardNormalized, _rotationDirection.normalized, Vector3.up);
                angle = Mathf.Clamp(angle, -90, 90);
                var dir = Quaternion.AngleAxis(angle, transform.up) * transform.forward;

                _pawn.Animator.SetFloat(AnimationStatics.Rotation, angle, .2f, Time.deltaTime);
                Debug.DrawLine(transform.position, transform.position + dir, Color.cyan);
            }
        }

        private void AimToTarget()
        {
            if (_pawn.PlayerControls.Player.Rotation.triggered) _rotationDirection -= transform.position;

            _crosshairImage.position = Mouse.current.position.value;
            var dis = _rotationDirection + transform.position;

            _targetTransform.position = dis;
            var distance = Vector3.Distance(_rotationDirection, transform.position);

            if (distance > 1)
            {
                var angle = Vector3.SignedAngle(transform.forward.normalized, _rotationDirection, Vector3.up);
                angle = Mathf.Clamp(angle, -90, 90);
                if (Mathf.Abs(angle) > _maxAngle)
                {
                    _rotate = true;
                }
                if (Mathf.Abs(angle) < _minAngle)
                {
                    _rotate = false;
                }

                if (_rotate)
                {
                    var dir = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
                    var rotation = Quaternion.LookRotation(dir, transform.up);
                    var rot = Quaternion.RotateTowards(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);
                    transform.rotation = rot;
                }

            } 
        }

        public override void Dispose()
        {
            _pawn.PlayerControls.Player.Rotation.performed -= AssignRotation;
            base.Dispose();
        }
    }
}