using System;
using System.Collections;
using _current.Core.Pawns.Components;
using _current.Core.Systems.DamageSystem;
using _current.Core.Systems.WeaponSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Data;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.Input;
using _current.Services.PersistentData;
using _current.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace _current.Core.Pawns.Player
{
    [DisallowMultipleComponent]
    public class PlayerWeaponSpawner : AbstractDamageSender
    {
        [SerializeField] private PrimaryWeaponTypeId _primaryWeapon;
        [SerializeField] private SecondaryWeaponTypeId _secondaryWeapon;
        [SerializeField] private Transform _gunHolder;
        [SerializeField] private PawnAnimationHandler _animationHandler;
        [SerializeField] private AnimationRigHandler _animationRigHandler;
        [SerializeField] private bool _autoReload;
        [SerializeField] private bool _isReloading;

        [Space] 
        [Header("Runtime Filled")]
        public WeaponPawn activeWeapon;
        
        private Stats _stats;
        private IInputService _inputService;
        private IWeaponFactory _factory;
        private IPersistentDataService _persistentDataService;

        public override event Action<IDamageSender, float> OnAttack;
        public event Action<PlayerWeaponSpawner> OnGunInit;
        public event Action<float> OnReload;

        [Inject]
        private void Construct(IWeaponFactory factory, IInputService inputService, IPersistentDataService persistentDataService, ISaveLoadService saveLoadService)
        {
            _persistentDataService = persistentDataService;
            _factory = factory;
            _inputService = inputService;
        }

        protected override void Bind()
        {
            Spawn();
            _inputService.OnAttackPressed += TryAttack;
            _inputService.OnReloadPressed += Reload;
        }

        private void TryAttack()
        {
            if (_inputService.IsAttackStillPressed && CanShoot())
            {
                StartCoroutine(Shoot());
            }
        }

        protected override void Unbind()
        {
            _inputService.OnAttackPressed -= TryAttack;
            _inputService.OnReloadPressed -= Reload;
        }


        private IEnumerator Shoot()
        {
            _animationHandler.SetIsAttacking(true);
            while (CanShoot() && _inputService.IsAttackStillPressed)
            {
                activeWeapon.Shoot();
                yield return null;
            }
            _animationHandler.SetIsAttacking(false);

            if (!CanShoot())
            {
                activeWeapon.PlayOutOfAmmoSound();
            }
            
            if (ShouldAutoReload() && !_isReloading)
            {
                yield return StartCoroutine(DoReload());
            }
        }

        private bool CanShoot()
        {
            return !_isReloading && activeWeapon.CurrentAmmo.Value > 0;
        }

        private void Reload()
        {
            if (!_isReloading && activeWeapon.CanReload())
            {
                StartCoroutine(DoReload());
            }
        }

        private IEnumerator DoReload()
        {
            _isReloading = true;
            OnReload?.Invoke(activeWeapon.ReloadTime);
            activeWeapon.PlayReloadSound();
            _animationHandler.SetAnimatorOverride(activeWeapon.AnimatorReloadOverride);
            _animationHandler.PlayReload();
            _animationRigHandler.SetHandsWeights(0);
            _animationRigHandler.SetAimWeights(0);
            yield return new WaitForSeconds(activeWeapon.ReloadTime);
            EndReload();
        }

        private void EndReload()
        {
            activeWeapon.Reload();
            activeWeapon.PlayEndReloadSound();
            _animationRigHandler.SetHandsWeights(1);
            _animationRigHandler.SetAimWeights(1);
            _isReloading = false;
        }

        private bool ShouldAutoReload()
        {
            return !_isReloading 
                   && _autoReload 
                   && activeWeapon.CurrentClip.Value == 0 
                   && activeWeapon.CanReload();
        }

        private async void Spawn()
        {
            foreach (var weaponData in _persistentDataService.Progress.AvailableWeapons)
            {
                activeWeapon = await _factory.Create(this, _gunHolder, weaponData);

                _animationRigHandler.SetHandGrips(activeWeapon.RigGripR, activeWeapon.RigGripL);
                OnGunInit?.Invoke(this);
            }
        }

        public void DropWeapon()
        {
            activeWeapon.GetComponent<Rigidbody>().isKinematic = false;
            activeWeapon.GetComponentInChildren<Collider>().enabled = true;
        }
    }
}