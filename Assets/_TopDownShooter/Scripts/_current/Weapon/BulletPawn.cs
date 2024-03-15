using System;
using System.Collections;
using _old.Data;
using UnityEngine;

namespace _old.Weapon
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletPawn : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        
        private Coroutine _delayedDisable;
        private float _damage;
        private float _bulletSpeed;
        private float _bulletDestroyTime;
        private WeaponData _weaponData;

        public event Action<BulletPawn> OnDisabled;
        public event Action<GameObject, Vector3> OnHit;

        public void SetupBullet(WeaponData weaponCmp, Transform aimTransform)
        {
            _weaponData = weaponCmp;
            Transform transform1;
            (transform1 = transform).SetPositionAndRotation(aimTransform.position, aimTransform.rotation);
            transform1.forward = aimTransform.transform.forward;
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            if (_weaponData)
            {
                gameObject.SetActive(false);
                return;
            }
            
            _rigidbody.velocity = transform.forward * _weaponData.BulletSpeed;
            
            if (_delayedDisable != null) 
                StopCoroutine(_delayedDisable);
            _delayedDisable = StartCoroutine(DelayedDisable());
        }

        private IEnumerator DelayedDisable()
        {
            if (!gameObject.activeSelf) yield return null;
            yield return new WaitForSeconds(_weaponData.BulletDestroyTime);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit?.Invoke(other.gameObject, transform.position);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.Sleep();
            OnDisabled?.Invoke(this);
        }
    }
}