using System.ComponentModel;
using Core.Logic;
using UniRx;
using UnityEngine;

namespace Core.Hero
{
    public class HeroHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private HeroAnimator animator;
        
        [field: SerializeField, ReadOnly(true)] public FloatReactiveProperty CurrentHP { get; set; } = new();
        [field: SerializeField] public float MaxHP { get; set; }

        public void TakeDamage(float damage)
        {
            if (CurrentHP.Value <= 0)
                return;
            
            CurrentHP.Value -= damage;
            
            animator.PlayHit();
            
            if (CurrentHP.Value < 0)
                CurrentHP.Value = 0;
        }
    }
}