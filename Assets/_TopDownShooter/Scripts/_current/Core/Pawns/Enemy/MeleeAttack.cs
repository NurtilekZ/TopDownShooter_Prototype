using System;
using System.Linq;
using _current.Core.Pawns.Components;
using _current.Core.Systems.DamageSystem;
using _current.Infrastructure.Factories.Interfaces;
using _current.StaticData.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _current.Core.Pawns.Enemy
{
    public class MeleeAttack : AbstractDamageSender
    {
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackRadius = 1f;
        [SerializeField] private int _layerMask;
        [SerializeField] private float _effectiveDistance = 0.5f;
        [SerializeField] private AnimationHandler _animationHandler;
        [SerializeField] private Transform _target;

        private float _attackCooldownTimeout;
        private bool _isAttacking;
        private bool _attackIsActive;
        private IHeroFactory _heroFactory;
        private Collider[] _hits = new Collider[1];

        public override event Action<IDamageSender, float> OnAttack;

        [Inject]
        private void Construct(IHeroFactory target)
        {
            _target = target.Hero.transform;
        }

        protected override void Bind()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        protected override void Unbind() { }

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
                StartAttack();
        }

        private void OnAttackAnimEvent()
        {
            OnAttack?.Invoke(this, _damageValue);
            if (Hit(out Collider hit))
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.red, 1f);
                hit.GetComponent<IHealth>().TakeDamage(_damageValue, this);
                Debug.Log($"{name} Attack {hit.gameObject.name}");
            }
        }

        private void OnAttackEndedAnimEvent()
        {
            _attackCooldownTimeout = _attackCooldown;
            _isAttacking = false;
        }

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private bool Hit(out Collider hit)
        {
            var hitsCount = Physics.OverlapSphereNonAlloc(GetStartPoint(), _attackRadius, _hits, _layerMask);
            hit = _hits.FirstOrDefault();
            return hitsCount > 0;
        }

        private Vector3 GetStartPoint()
        {
            var position = transform.position;
            var startPoint = new Vector3(position.x, position.y, position.z) + transform.forward * _effectiveDistance;
            return startPoint;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackCooldownTimeout -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.DOLookAt(_target.position, 1f)
                .OnComplete(() =>
                {
                    _animationHandler.PlayAttack();
                    _isAttacking = true;
                });
        }

        private bool CooldownIsUp() => 
            _attackCooldownTimeout <= 0;

        private bool CanAttack() => 
            _attackIsActive && !_isAttacking && CooldownIsUp();

        public void Initialize(EnemyStaticData enemyData)
        {
            _damageValue = enemyData.Damage;
            _attackRadius = enemyData.AttackRadius;
            _effectiveDistance = enemyData.EffectiveDistance;
            _attackCooldown = enemyData.AttackCooldown;
        }
    }
}