using System;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public class RagDollController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        [SerializeField] private Rigidbody[] _rigidbodies;
        [SerializeField] private Collider[] _colliders;

        private void Awake()
        {
            if (_rigidbodies.Length == 0 || _colliders.Length == 0) 
                CollectCollidersAndRBs();

            DeactivateRagdoll();
        }

        [ContextMenu("CollectColliders")]
        private void CollectCollidersAndRBs()
        {
            Array.Clear(_rigidbodies, 0, _rigidbodies.Length);
            Array.Clear(_colliders, 0, _colliders.Length);
            
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        private void DeactivateRagdoll()
        {
            SetCollidersEnabled(false);
            SetRBKinematic(true);

            if (_collider) _collider.enabled = true;
            if (_rigidbody) _rigidbody.isKinematic = false;
            if (_animator) _animator.enabled = true;
        }

        public void ActivateRagdoll()
        {
            if (_collider) _collider.enabled = false;
            if (_rigidbody) _rigidbody.isKinematic = false;
            if (_animator) _animator.enabled = false;
            
            SetCollidersEnabled(true);
            SetRBKinematic(false);
        }

        private void SetCollidersEnabled(bool value)
        {
            foreach (var collider1 in _colliders)
            {
                collider1.enabled = value;
            }
        }

        private void SetRBKinematic(bool isKinematic)
        {
            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = isKinematic;
            }
        }
    }
}