using System.Collections;
using _current.Core.Systems.DamageSystem;
using _current.Core.Systems.ImpactSystem;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Data;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering;

namespace _current.Core.Systems.WeaponSystem
{
    public class WeaponPawn : MonoBehaviour
    {
        [SerializeField] private float _lastShootTime;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _shootVFX;
        [SerializeField] private Transform _rigGripR;
        [SerializeField] private Transform _rigGripL;
        [SerializeField] private ReactiveProperty<int> _currentAmmo = new();
        [SerializeField] private ReactiveProperty<int> _currentClip = new();
        
        private WeaponStaticData _weaponStaticData;
        private IDamageSender _owner;
        private UnityEngine.Pool.ObjectPool<TrailRenderer> _trailPool;

        public IReadOnlyReactiveProperty<int> CurrentClip => _currentClip;
        public IReadOnlyReactiveProperty<int> CurrentAmmo => _currentAmmo;

        public Transform RigGripR => _rigGripR;
        public Transform RigGripL => _rigGripL;
        public IDamageSender Owner => _owner;
        public WeaponTypeId WeaponTypeId => _weaponStaticData.WeaponTypeId;
        public float ReloadTime => _weaponStaticData.AmmoConfig.ReloadTime;
        public float MaxAmmo => _weaponStaticData.AmmoConfig.MaxAmmo;
        public float MaxClip => _weaponStaticData.AmmoConfig.ClipSize;
        public int HitMask => _weaponStaticData.ShootConfig.HitMask;
        public AnimatorOverrideController AnimatorReloadOverride => _weaponStaticData.AmmoConfig.AnimatorReloadOverride;

        public void Initialize(IDamageSender owner, WeaponData weaponData, WeaponStaticData weaponStaticData)
        {
            _owner = owner;
            _weaponStaticData = weaponStaticData;
            
            _currentAmmo.Value = weaponData.ammoCount;
            _currentClip.Value = weaponData.clipCount;
            _lastShootTime = 0;
            _trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
            transform.SetLocalPositionAndRotation(weaponStaticData.SpawnPoint, Quaternion.Euler(weaponStaticData.SpawnRotation));
        }

        public void Shoot()
        {
            if (Time.time > _weaponStaticData.ShootConfig.FireRate + _lastShootTime)
            {
                _lastShootTime = Time.time;
                _shootVFX.Emit(1);
                _weaponStaticData.AudioConfig.PlayShootingClip(_audioSource, _currentClip.Value == 1);
                
                Vector3 shootDirection = 
                    _shootVFX.transform.forward
                    + new Vector3(
                      Random.Range(-_weaponStaticData.ShootConfig.Spread.x, _weaponStaticData.ShootConfig.Spread.x),
                      Random.Range(-_weaponStaticData.ShootConfig.Spread.y, _weaponStaticData.ShootConfig.Spread.y),
                      Random.Range(-_weaponStaticData.ShootConfig.Spread.z, _weaponStaticData.ShootConfig.Spread.z));
                
                shootDirection.Normalize();

                DecrementAmmo();

                if (Physics.Raycast(_shootVFX.transform.position, shootDirection, out var hit, float.MaxValue, _weaponStaticData.ShootConfig.HitMask))
                {
                    StartCoroutine(PlayTrail(_shootVFX.transform.position, hit.point, hit));
                }
                else
                {
                    var position = _shootVFX.transform.position;
                    StartCoroutine(PlayTrail(position, position + (shootDirection * _weaponStaticData.TrailConfig.MissDistance), new RaycastHit()));
                }
            }
        }

        private void DecrementAmmo()
        {
            _currentAmmo.Value--;
        }

        public void Reload()
        {
            if (!CanReload()) return;
            
            int maxReloadAmount = Mathf.Min(_weaponStaticData.AmmoConfig.ClipSize, _currentAmmo.Value);
            int availableBulletsInCurrentClip = _weaponStaticData.AmmoConfig.ClipSize - _currentClip.Value;
            int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
            
            _currentClip.Value += reloadAmount;
            _currentAmmo.Value -= reloadAmount;
        }

        public bool CanReload()
        {
            return _currentClip.Value < _weaponStaticData.AmmoConfig.ClipSize && _currentAmmo.Value > 0;
        }

        public void PlayReloadSound()
        {
            _weaponStaticData.AudioConfig.PlayReloadClip(_audioSource);
        }

        public void PlayEndReloadSound()
        {
            _weaponStaticData.AudioConfig.PlayEndReloadClip(_audioSource);
        }

        public void PlayOutOfAmmoSound()
        {
            _weaponStaticData.AudioConfig.PlayOutOfAmmoClip(_audioSource);
        }

        private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
        {
            TrailRenderer instance = _trailPool.Get();
            instance.gameObject.SetActive(true);
            instance.transform.position = startPoint;
            yield return null;

            instance.emitting = true;

            float distance = Vector3.Distance(startPoint, endPoint);
            float remainingDistance = distance;
            while (remainingDistance > 0)
            {
                instance.transform.position = Vector3.Lerp(
                    startPoint,
                    endPoint,
                    Mathf.Clamp01(1 - (remainingDistance / distance)));

                remainingDistance -= _weaponStaticData.TrailConfig.SimulationSpeed * Time.deltaTime;

                yield return null;
            }

            instance.transform.position = endPoint;

            if (hit.collider)
            {
                SurfaceManager.Instance.HandleImpact(
                    hit.transform.gameObject,
                    endPoint,
                    hit.normal,
                    _weaponStaticData.ImpactType,
                    0);
                
                if (hit.collider.TryGetComponent(out IHealth damageable))
                {
                    damageable.TakeDamage(_weaponStaticData.DamageConfig.GetDamage(distance), Owner);
                }

                foreach (var handler in _weaponStaticData.bulletImpactEffects)
                {
                    handler.HandleEffect(hit.collider, endPoint, hit.normal, this);
                }
            }

            yield return new WaitForSeconds(_weaponStaticData.TrailConfig.Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            _trailPool.Release(instance);
        }

        private TrailRenderer CreateTrail()
        {
            GameObject instance = new GameObject("Bullet trail");
            instance.transform.SetParent(transform);
            TrailRenderer trail = instance.AddComponent<TrailRenderer>();
            trail.colorGradient = _weaponStaticData.TrailConfig.Color;
            trail.material = _weaponStaticData.TrailConfig.Material;
            trail.widthCurve = _weaponStaticData.TrailConfig.WidthCurve;
            trail.time = _weaponStaticData.TrailConfig.Duration;
            trail.minVertexDistance = _weaponStaticData.TrailConfig.MinVertexDistance;
            trail.emitting = false;
            trail.shadowCastingMode = ShadowCastingMode.Off;

            return trail;
        }
    }
}