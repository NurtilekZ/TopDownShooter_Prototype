using System.Collections;
using _current.Systems.DamageSystem;
using _current.Systems.ImpactSystem;
using _current.Systems.WeaponSystem.Configs;
using _current.Systems.WeaponSystem.Data;
using _current.Units;
using UnityEngine;
using UnityEngine.Rendering;
using AudioConfiguration = _current.Systems.WeaponSystem.Configs.AudioConfiguration;

namespace _current.Systems.WeaponSystem
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject
    {
        public ImpactType ImpactType;
        public GunType Type;
        public string Name;
        public GameObject ModelPreafab;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        public DamageConfiguration DamageConfig;
        public AmmoConfiguration ammoConfig;
        public ShootConfiguration ShootConfig;
        public TrailConfiguration TrailConfig;
        public AudioConfiguration audioConfig;

        private MonoBehaviour _activeMonoBehaviour;
        private BasePawn _activeSender;
        private GameObject _model;
        private AudioSource _shootingAudioSource;
        private float _lastShootTime;
        private ParticleSystem _shootSystem;
        private UnityEngine.Pool.ObjectPool<TrailRenderer> _trailPool;


        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            _activeMonoBehaviour = activeMonoBehaviour;
            _activeSender = _activeMonoBehaviour.GetComponent<BasePawn>();
            _lastShootTime = 0;
            _trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
            _model = Instantiate(ModelPreafab, parent, false);
            _model.transform.SetLocalPositionAndRotation(SpawnPoint, Quaternion.Euler(SpawnRotation));

            _shootSystem = _model.GetComponentInChildren<ParticleSystem>();
            _shootingAudioSource = _model.GetComponent<AudioSource>();
        }

        public void Shoot()
        {
            if (Time.time > ShootConfig.FireRate + _lastShootTime)
            {
                _lastShootTime = Time.time;
                _shootSystem.Emit(1);
                audioConfig.PlayShootingClip(_shootingAudioSource, ammoConfig.CurrentClipAmmo.Value == 1);
                
                Vector3 shootDirection = 
                    _shootSystem.transform.forward
                    + new Vector3(
                      Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                      Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y),
                      Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z));
                
                shootDirection.Normalize();

                ammoConfig.CurrentClipAmmo.Value--;

                if (Physics.Raycast(_shootSystem.transform.position, shootDirection, out var hit, float.MaxValue, ShootConfig.HitMask))
                {
                    _activeMonoBehaviour.StartCoroutine(PlayTrail(
                        _shootSystem.transform.position,
                        hit.point, hit));
                }
                else
                {
                    _activeMonoBehaviour.StartCoroutine(PlayTrail(
                        _shootSystem.transform.position,
                        _shootSystem.transform.position + (shootDirection * TrailConfig.MissDistance),
                        new RaycastHit()));
                }
            }
        }

        public void Reload()
        {
            if (CanReload())
            {
                ammoConfig.Reload();
            }
        }

        public bool CanReload()
        {
            return ammoConfig.CanReload();
        }

        public void PlayReloadSound()
        {
            audioConfig.PlayReloadClip(_shootingAudioSource);
        }

        public void PlayOutOfAmmoSound()
        {
            audioConfig.PlayOutOfAmmoClip(_shootingAudioSource);
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

                remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

                yield return null;
            }

            instance.transform.position = endPoint;

            if (hit.collider)
            {
                SurfaceManager.Instance.HandleImpact(
                    hit.transform.gameObject,
                    endPoint,
                    hit.normal,
                    ImpactType,
                    0);
                
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_activeSender, DamageConfig.GetDamage(distance));
                }
            }

            yield return new WaitForSeconds(TrailConfig.Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            _trailPool.Release(instance);
        }

        private TrailRenderer CreateTrail()
        {
            GameObject instance = new GameObject("Bullet trail");
            instance.transform.SetParent(_model.transform);
            TrailRenderer trail = instance.AddComponent<TrailRenderer>();
            trail.colorGradient = TrailConfig.Color;
            trail.material = TrailConfig.Material;
            trail.widthCurve = TrailConfig.WidthCurve;
            trail.time = TrailConfig.Duration;
            trail.minVertexDistance = TrailConfig.MinVertexDistance;
            trail.emitting = false;
            trail.shadowCastingMode = ShadowCastingMode.Off;

            return trail;
        }
    }
}