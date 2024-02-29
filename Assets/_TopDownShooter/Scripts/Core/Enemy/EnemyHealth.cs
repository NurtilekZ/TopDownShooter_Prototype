using Core.Logic;
using UniRx;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _animator;
        
        [field: SerializeField] public FloatReactiveProperty CurrentHP { get; set; }
        [field: SerializeField] public float MaxHP { get; set; }
        
        public void TakeDamage(float damage)
        {
            if (CurrentHP.Value <= 0)
                return;

            CurrentHP.Value -= damage;
            
            _animator?.PlayHit();

            if (CurrentHP.Value < 0)
                CurrentHP.Value = 0;
        }
    }
}