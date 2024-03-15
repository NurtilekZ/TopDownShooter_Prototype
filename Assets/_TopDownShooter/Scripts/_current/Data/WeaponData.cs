using System;
using UnityEngine;

namespace _old.Data
{
    [CreateAssetMenu(menuName = "Data", fileName = "WeaponData")]
    [Serializable]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private int _ammoCount = 30;
        [SerializeField] private float _aimDelayTime = 0.5f;
        [SerializeField] private float _fireIntervalTime = 0.5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _reloadTime = 2f;
        [SerializeField] private float _bulletSpeed = 200f;
        [SerializeField] private float _bulletDestroyTime = 2f;
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AnimatorOverrideController _weaponAnimOverride;

        public int AmmoCount => _ammoCount;
        public float AimDelayTime => _aimDelayTime;
        public float FireIntervalTime => _fireIntervalTime;
        public float Damage => _damage;
        public float ReloadTime => _reloadTime;
        public float BulletSpeed => _bulletSpeed;
        public float BulletDestroyTime => _bulletDestroyTime;
        public AudioClip ShootSound => _shootSound;
        public AnimatorOverrideController WeaponAnimOverride => _weaponAnimOverride;
    }
}