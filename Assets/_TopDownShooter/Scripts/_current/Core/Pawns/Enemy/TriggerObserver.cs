using System;
using UnityEngine;

namespace _current.Core.Pawns.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        [field:SerializeField] public SphereCollider Collider { get; private set; }
        [field:SerializeField] public Color TriggerColor { get; private set; }
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider other) => 
            TriggerEnter?.Invoke(other);

        private void OnTriggerExit(Collider other) => 
            TriggerExit?.Invoke(other);
    }
}