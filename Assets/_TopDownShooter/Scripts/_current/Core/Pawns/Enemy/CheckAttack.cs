using UnityEngine;

namespace _current.Core.Pawns.Enemy
{
    [RequireComponent(typeof(MeleeAttack))]
    public class CheckAttack : MonoBehaviour
    {
        [SerializeField] private MeleeAttack _meleeAttack;
        [SerializeField] private TriggerObserver _triggerObserver;

        private void Start()
        {
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;
            
            _meleeAttack.DisableAttack();
        }

        private void TriggerExit(Collider obj)
        {
            _meleeAttack.DisableAttack();
        }

        private void TriggerEnter(Collider obj)
        {
            _meleeAttack.EnableAttack();
        }

        private void OnDestroy()
        {
            if (_triggerObserver == null) return;
            _triggerObserver.TriggerEnter -= TriggerEnter;
            _triggerObserver.TriggerExit -= TriggerExit;
        }

        public void Initialize(float attackRadius)
        {
            _triggerObserver.Collider.radius = attackRadius;
        }
    }
}