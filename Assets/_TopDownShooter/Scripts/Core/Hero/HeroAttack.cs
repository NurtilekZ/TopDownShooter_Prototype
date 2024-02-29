using System;
using Core.Enemy;
using Core.Logic;
using Services.Input;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Hero
{
    public class HeroAttack : MonoBehaviour
    {
        private const float DamageRadius = 1.0f;
        private static int _layerMask;
        private const AttackType AttackType = Enemy.AttackType.Direct;
        private readonly Collider[] _hits = new Collider[3];

        private IInputService _inputService;

        [SerializeField] private HeroAnimator animator;
        [SerializeField] private ParticleSystem attackVFX;
        
        [field: SerializeField] public float Shield { get; set; }
        [field: SerializeField] public FloatReactiveProperty AttackDamage { get; set; } = new();

        [Inject]
        private void Construct(IInputService inputService) 
            => _inputService = inputService;

        private void Start()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Hittable");

            _inputService.AttackPressed += OnAttack;
            
            AttackDamage.Subscribe(_ =>
            {
                var emission = attackVFX.emission;
                emission.rateOverTimeMultiplier = AttackDamage.Value;
            });
        }

        private void OnDestroy()
        {
            _inputService.AttackPressed -= OnAttack;
        }
        private void OnAttack()
        {
            animator.PlayAttack();
            attackVFX.Play();

            for (var i = 0; i < Hit(from: attackVFX.transform); ++i)
                _hits[i].transform
                    .GetComponentInParent<IHealth>()
                    .TakeDamage(AttackDamage.Value);
        }

        private int Hit(Transform from) =>
            AttackType switch
            {
                AttackType.Direct => Physics.OverlapCapsuleNonAlloc(from.position, from.position + from.forward, DamageRadius, _hits, _layerMask),
                AttackType.AOE => Physics.OverlapSphereNonAlloc(from.position, DamageRadius, _hits, _layerMask),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}