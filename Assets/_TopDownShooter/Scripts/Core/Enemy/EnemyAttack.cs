using System;
using Core.Logic;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static Core.Enemy.AttackType;

namespace Core.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        private const float DamageRadius = 1.0f;
        private static int _layerMask;
        private readonly Collider[] _hits = new Collider[1];
        
        private bool _enabled;

        [SerializeField, CanBeNull] private EnemyAnimator animator;
        [SerializeField] private ParticleSystem attackVFX;
        
        [field: SerializeField] public AttackType AttackType { get; set; }
        [field: SerializeField] public float Shield { get; set; }
        [field: SerializeField] public float AttackDamage { get; set; }
        [field: SerializeField] public float Cooldown { get; set; }

        private void Start()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");

            Observable
                .Interval(TimeSpan.FromSeconds(Cooldown))
                .Where(_ => _enabled)
                .TakeWhile(_ => _enabled != false)
                .TakeUntilDestroy(this)
                .Subscribe(_ => OnAttack());
        }

        public void Initialize(GameObject hero) =>
            hero
                .OnDestroyAsObservable()
                .Subscribe(_ => Deactivate());

        public void Activate() =>
            (_enabled) = (true);

        public void Deactivate() =>
            (_enabled) = (false);

        private void OnAttack()
        {
            animator?.PlayAttack();
            attackVFX.Play();
            
            for (var i = 0; i < Hit(from: attackVFX.transform); ++i)
                _hits[i].transform
                    .GetComponentInParent<IHealth>()
                    .TakeDamage(AttackDamage);
        }
        
        private int Hit(Transform from) =>
            AttackType switch
            {
                Direct => Physics.OverlapCapsuleNonAlloc(from.position, from.position + from.forward, DamageRadius, _hits, _layerMask),
                AOE => Physics.OverlapSphereNonAlloc(from.position, DamageRadius, _hits, _layerMask),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}