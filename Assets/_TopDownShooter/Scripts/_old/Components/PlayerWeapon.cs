using System.Collections;
using _old.Player;
using _old.Sound;
using _old.Weapon;
using Shaders;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Zenject;

namespace _old.Components
{
    public class PlayerWeapon : PawnComponent<PlayerPawn>
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private LayerMask _aimLayerMask;
        [SerializeField] private float _maxTargetDistance;
        [SerializeField] private TwoBoneIKConstraint _constraintR;
        [SerializeField] private TwoBoneIKConstraint _constraintL;
        [SerializeField] private MultiAimConstraint[] _aimConstraints;
        private RectTransform _crosshairImage;

        private Vector3 _rotationDirection;

        private WeaponPawn _weaponPawn;

        protected override void Start()
        {
            base.Start();
            _pawn.Animator.runtimeAnimatorController = _weaponPawn.ReloadAnimTrigger;
            _constraintR.data.target = _weaponPawn.RightArmConstraints;
            _constraintL.data.target = _weaponPawn.LeftArmConstraints;
            _rigBuilder.Build();
            SetWeaponWeightsActive(true);
        }

        private void Update()
        {
            AimToTarget();
        }

        [Inject]
        public void Construct(WeaponPawn weaponPawn, RectTransform crosshair, Transform targetPoint)
        {
            _targetTransform = targetPoint;
            _weaponPawn = weaponPawn;
            _crosshairImage = crosshair;
            _weaponPawn.OnFire += AnimateOnFire;
            _weaponPawn.OnReload += AnimateReload;
            _weaponPawn.OnEndReload += ResetWeights;
        }

        public override void SetupPlayerInput()
        {
            _pawn.PlayerControls.Player.Attack.performed += AssignAttack;
            _pawn.PlayerControls.Player.Rotation.performed += AssignRotation;
        }

        private void AssignAttack(InputAction.CallbackContext ctx)
        {
            StartCoroutine(Attack());
        }

        private void AssignRotation(InputAction.CallbackContext ctx)
        {
            if (ctx.control.device == Gamepad.current)
            {
                var direction = ctx.ReadValue<Vector2>() * _maxTargetDistance;
                _rotationDirection = new Vector3(direction.x, 1, direction.y);
            }
            else if (ctx.control.device == Mouse.current)
            {
                var ray = _pawn.PlayerCamera.ScreenPointToRay(ctx.ReadValue<Vector2>());
                if (Physics.Raycast(ray, out var hit, _aimLayerMask)) _rotationDirection = hit.point;
            }
        }

        private void AimToTarget()
        {
            if (_pawn.PlayerControls.Player.Rotation.triggered) _rotationDirection -= transform.position;

            _crosshairImage.position = Mouse.current.position.value;
            var dis = _rotationDirection + transform.position;

            _targetTransform.position = dis;
            var distance = Vector3.Distance(_rotationDirection, transform.position);

            if (distance > 1) transform.forward = new Vector3(_rotationDirection.x, 0, _rotationDirection.z);
        }

        private IEnumerator Attack()
        {
            while (_pawn.PlayerControls.Player.Attack.phase == InputActionPhase.Performed)
            {
                _weaponPawn.Fire();
                yield return null;
            }

            StopFire();
        }

        private void StopFire()
        {
            _weaponPawn.StopFire();
        }

        private void AnimateOnFire()
        {
            _pawn.Animator.SetTrigger(AnimationStatics.Shoot);
            SoundFX.PlaySoundAtPoint(_weaponPawn.ShootSound, _weaponPawn.transform.position);
        }

        private void AnimateReload()
        {
            SetWeaponWeightsActive(false);
            _pawn.Animator.SetTrigger(AnimationStatics.Reload);
        }

        private void ResetWeights()
        {
            SetWeaponWeightsActive(true);
        }

        private void SetWeaponWeightsActive(bool active)
        {
            foreach (var constraint in _aimConstraints) constraint.weight = active ? 1 : 0;
            _constraintR.weight = active ? 1 : 0;
            _constraintL.weight = active ? 1 : 0;
        }

        public override void Dispose()
        {
            if (_weaponPawn != null)
            {
                _weaponPawn.OnFire -= AnimateOnFire;
                _weaponPawn.OnReload -= AnimateReload;
                _weaponPawn.OnEndReload -= ResetWeights;
            }

            if (_pawn.PlayerControls != null)
            {
                _pawn.PlayerControls.Player.Attack.performed -= AssignAttack;
                _pawn.PlayerControls.Player.Rotation.performed -= AssignRotation;
            }

            base.Dispose();
        }
    }
}