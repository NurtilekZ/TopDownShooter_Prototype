using System;
using System.Collections;
using System.Collections.Generic;
using _old.Data;
using _old.Properties;
using _old.Sound;
using UnityEngine;

namespace _old.Weapon
{
    public abstract class WeaponPawnBase : DamageSender
    {
        [SerializeField] protected int _currentAmmoCount;
        [SerializeField] private bool _isFiring;
        [SerializeField] private bool _isReloading;
        [SerializeField] protected BulletPawn bulletPawnPrefab;
        [SerializeField] private ParticleSystem[] _muzzleFlash;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private Transform _aimTransform;
        [SerializeField] private Transform _rightArmConstraints;
        [SerializeField] private Transform _leftArmConstraints;
        [SerializeField] protected WeaponData _weaponData;
        private Coroutine _reloadCoroutine;
        private bool _canFire = true;
        private Coroutine _canFireCoroutine;
        private Queue<BulletPawn> _ammoQueue = new Queue<BulletPawn>();
        public Transform RightArmConstraints => _rightArmConstraints;
        public Transform LeftArmConstraints => _leftArmConstraints;
        public AnimatorOverrideController ReloadAnimTrigger => _weaponData.WeaponAnimOverride;
        public AudioClip ShootSound => _weaponData.ShootSound;
        public Transform AimTransform => _aimTransform;
        public virtual event Action OnFire;
        public virtual event Action OnStopFire;
        public virtual event Action OnReload;
        public virtual event Action OnEndReload;
        
        protected virtual void Start()
        {
            _currentAmmoCount = _weaponData.AmmoCount;
            for (int i = 0; i < _currentAmmoCount; i++)
            {
                var bulletView = Instantiate(bulletPawnPrefab, transform);
                bulletView.gameObject.SetActive(false);
                bulletView.OnDisabled += PushBullet;
                bulletView.OnHit += OnBulletHit;
                PushBullet(bulletView);
            }
        }

        protected virtual void PushBullet(BulletPawn bulletPawn)
        {
            _ammoQueue.Enqueue(bulletPawn);
        }

        public virtual void Fire()
        {
            if (_isReloading) return;
            if (!_canFire) return;
            FireBullet();
            _isFiring = true;
            ShowMuzzleFlash();
            OnFire?.Invoke();
            _canFire = false;
            
            if (_canFireCoroutine != null) 
                StopCoroutine(_canFireCoroutine);
            _canFireCoroutine = StartCoroutine((IEnumerator)FireInterval());

            if (_currentAmmoCount > 0) return;
            if (_reloadCoroutine != null) 
                StopCoroutine(_reloadCoroutine);
            StopFire();
            _reloadCoroutine = StartCoroutine((IEnumerator)ReloadCoroutine());
        }

        private IEnumerator FireInterval()
        {
            yield return new WaitForSeconds(_weaponData.FireIntervalTime);
            _canFire = true;
        }

        private void FireBullet()
        {
            _currentAmmoCount--;
            var bullet = _ammoQueue.Dequeue();
            bullet.SetupBullet(_weaponData, _aimTransform);
        }

        protected void OnBulletHit(GameObject other, Vector3 hitPosition)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                DoDamage(target, _weaponData.Damage);
            }
            else
            {
                DoSurfaceHitFX(other, hitPosition);
            }
        }

        public override void DoDamage(IDamageable target, float damage)
        {
            base.DoDamage(target, damage);
            target.TakeDamage(this, damage);
        }

        private void DoSurfaceHitFX(GameObject other, Vector3 hitPosition)
        {
            var transform1 = _hitEffect.transform;
            transform1.position = hitPosition;
            transform1.forward = hitPosition - other.transform.position;
            _hitEffect.Emit(1);
            SoundFX.PlayBulletHitSoundAtPoint(other.layer, hitPosition);
        }

        private IEnumerator ReloadCoroutine()
        {
            _isReloading = true;
            OnReload?.Invoke();
            yield return new WaitForSeconds(_weaponData.ReloadTime);
            _currentAmmoCount = _weaponData.AmmoCount;
            _isReloading = false;
            OnEndReload?.Invoke();
        }

        private void ShowMuzzleFlash()
        { 
            foreach (var muzzle in _muzzleFlash)
            {
                muzzle.Emit(1);
            }
        }

        public void StopFire()
        {
            _isFiring = false;
            OnStopFire?.Invoke();
        }

        public override void Dispose()
        {
            foreach (var bulletView in _ammoQueue)
            {
                bulletView.OnDisabled -= PushBullet;
                bulletView.OnHit -= OnBulletHit;
            }
            base.Dispose();
        }
    }

    public class WeaponPawn : WeaponPawnBase
    {
        public override event Action OnFire;
        public override event Action OnStopFire;
        public override event Action OnReload;
        public override event Action OnEndReload;

        private void Start()
        {
            _currentAmmoCount = _weaponData.AmmoCount;
            for (int i = 0; i < _currentAmmoCount; i++)
            {
                var bulletView = Instantiate(bulletPawnPrefab, transform);
                bulletView.gameObject.SetActive(false);
                bulletView.OnDisabled += PushBullet;
                bulletView.OnHit += OnBulletHit;
                PushBullet(bulletView);
            }
        }
    }
}