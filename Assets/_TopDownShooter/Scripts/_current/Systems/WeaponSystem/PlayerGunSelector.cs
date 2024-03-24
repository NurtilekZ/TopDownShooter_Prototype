using System;
using System.Collections;
using System.Collections.Generic;
using _current.Systems.WeaponSystem.Data;
using _current.Units.Components;
using _current.Units.Player;
using Shaders;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

namespace _current.Systems.WeaponSystem
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : PawnComponent<PlayerPawn>
    {
        [SerializeField] private GunType _gunType;
        [SerializeField] private Transform _gunParent;
        [SerializeField] private Animator _pawnAnimator;
        [SerializeField] private bool _autoReload;
        [SerializeField] private bool _isReloading;
        [SerializeField] private List<MultiAimConstraint> _aimConstraints;
        [SerializeField] private List<TwoBoneIKConstraint> _handsConstraints;
        [SerializeField] private List<GunScriptableObject> _guns;

        [Space] 
        [Header("Runtime Filled")]
        public GunScriptableObject ActiveGun;

        public event Action<float> OnReload;

        protected override void Awake()
        {
            base.Awake();
            var gun = _guns.Find(gun => gun.Type == _gunType);

            if (gun == null)
            {
                Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
                return;
            }

            ActiveGun = gun;
            gun.Spawn(_gunParent, this);
        }

        protected override void BindInput()
        {
            _pawn.PlayerControls.Player.Attack.performed += Shoot;
            _pawn.PlayerControls.Player.Reload.performed += Reload;
        }

        private void Shoot(InputAction.CallbackContext obj)
        {
            if (CanShoot())
            {
                StartCoroutine(Attack());
            }
        }

        private IEnumerator Attack()
        {
            _pawnAnimator.SetBool(AnimationStatics.Shoot, true);
            while (CanShoot() && _pawn.PlayerControls.Player.Attack.phase == InputActionPhase.Performed)
            {
                ActiveGun.Shoot();
                yield return null;
            }
            _pawnAnimator.SetBool(AnimationStatics.Shoot, false);

            if (!CanShoot())
            {
                ActiveGun.PlayOutOfAmmoSound();
            }
            
            if (ShouldAutoReload() && !_isReloading)
            {
                yield return StartCoroutine(DoReload());
            }
        }

        private bool CanShoot()
        {
            return !_isReloading && ActiveGun.ammoConfig.CurrentClipAmmo.Value > 0;
        }

        private void Reload(InputAction.CallbackContext obj)
        {
            if (!_isReloading && ActiveGun.CanReload())
            {
                StartCoroutine(DoReload());
            }
        }

        private IEnumerator DoReload()
        {
            ActiveGun.PlayReloadSound();
            OnReload?.Invoke(ActiveGun.ammoConfig.ReloadTime);
            _isReloading = true;
            _pawnAnimator.runtimeAnimatorController = ActiveGun.ammoConfig.AnimatorReloadOverride;
            _pawnAnimator.SetTrigger(AnimationStatics.Reload);
            foreach (var constraint in _handsConstraints)
            {
                constraint.weight = 0.25f;
            }

            foreach (var aimConstraint in _aimConstraints)
            {
                aimConstraint.weight = 0.25f;
            }

            yield return new WaitForSeconds(ActiveGun.ammoConfig.ReloadTime);
            EndReload();
        }

        private void EndReload()
        {
            ActiveGun.Reload();
            foreach (var constraint in _handsConstraints)
            {
                constraint.weight =1f;
            }

            foreach (var aimConstraint in _aimConstraints)
            {
                aimConstraint.weight = 1f;
            }
            _isReloading = false;
        }

        private bool ShouldAutoReload()
        {
            return !_isReloading 
                   && _autoReload 
                   && ActiveGun.ammoConfig.CurrentClipAmmo.Value == 0 
                   && ActiveGun.CanReload();
        }

        protected override void UnbindInput()
        {
            _pawn.PlayerControls.Player.Attack.performed -= Shoot;
            _pawn.PlayerControls.Player.Attack.performed -= Reload;
        }
    }
}