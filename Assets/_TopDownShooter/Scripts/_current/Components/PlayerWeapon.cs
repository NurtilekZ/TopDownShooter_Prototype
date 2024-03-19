using System.Collections;
using _current.Player;
using _current.Weapon;
using Shaders;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Zenject;

namespace _current.Components
{
    public class PlayerWeapon : PawnComponent<PlayerPawn>
    {
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private TwoBoneIKConstraint _constraintR;
        [SerializeField] private TwoBoneIKConstraint _constraintL;
        [SerializeField] private MultiAimConstraint[] _aimConstraints;
        [SerializeField] private WeaponPawn _weaponPawn;
        
        private bool isFiring;

        protected override void Start()
        {
            base.Start();
            _pawn.Animator.runtimeAnimatorController = _weaponPawn.AnimOverride;
            _constraintR.data.target = _weaponPawn.RightArmConstraints;
            _constraintL.data.target = _weaponPawn.LeftArmConstraints;
            _rigBuilder.Build();
            SetWeaponWeightsActive(true);
        }

        [Inject]
        public void Construct(WeaponPawn weaponPawn, RectTransform crosshair, Transform targetPoint)
        {
            _weaponPawn = weaponPawn;
            _weaponPawn.OnFire += AnimateOnFire;
            _weaponPawn.OnReload += AnimateReload;
            _weaponPawn.OnEndReload += ResetWeights;
        }

        public override void BindPlayerInput()
        {
            if (_pawn.PlayerControls == null) return;
            _pawn.PlayerControls.Player.Attack.performed += AssignAttack;
        }

        public override void UnbindPlayerInput()
        {
            if (_pawn.PlayerControls == null) return;
            _pawn.PlayerControls.Player.Attack.performed -= AssignAttack;
        }

        private void AssignAttack(InputAction.CallbackContext ctx)
        {
            StartCoroutine(Attack());
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
            foreach (var constraint in _aimConstraints) 
                constraint.weight = active ? 1 : 0;
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
            }

            base.Dispose();
        }
    }
}