using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace _current.Data
{
    [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
    public class GunScriptableObject : ScriptableObject
    {
        public GunType Type;
        public string Name;
        public GameObject ModelPreafab;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        public ShootConfiguration ShootConfig;
        public TrailConfiguration TrailConfig;

        private MonoBehaviour _activeMonoBehaviour;
        private GameObject _model;
        private float _lastShootTime;
        private ParticleSystem _shootSystem;
        private UnityEngine.Pool.ObjectPool<TrailRenderer> _trailPool;

        public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
        {
            _activeMonoBehaviour = activeMonoBehaviour;
            _lastShootTime = 0;
            _trailPool = new UnityEngine.Pool.ObjectPool<TrailRenderer>(CreateTrail);
            _model = Instantiate(ModelPreafab);
            _model.transform.SetParent(parent, false);
            _model.transform.localPosition = SpawnPoint;
            _model.transform.localRotation = Quaternion.Euler(SpawnRotation);

            _shootSystem = _model.GetComponentInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            if (Time.time > ShootConfig.FireRate + _lastShootTime)
            {
                _lastShootTime = Time.time;
                _shootSystem.Play();
                Vector3 shootDirection = 
                    _shootSystem.transform.forward
                    + new Vector3(
                      Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                      Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y),
                      Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z));
                
                shootDirection.Normalize();

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
            
            yield return new WaitForSeconds(TrailConfig.Duration);
            yield return null;
            instance.emitting = false;
            instance.gameObject.SetActive(false);
            _trailPool.Release(instance);
        }

        private TrailRenderer CreateTrail()
        {
            GameObject instance = new GameObject("Bullet trail");
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