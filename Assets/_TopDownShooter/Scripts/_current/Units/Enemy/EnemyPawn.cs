using System.Collections;
using _current.Systems.DamageSystem;
using UnityEngine;

namespace _current.Units.Enemy
{
    public class EnemyPawn : BasePawn
    {
        [SerializeField] private Renderer _renderer;

        private Coroutine _damageColorCoroutine;

        public override bool IsAlive { get; protected set; } = true;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            OnTakeDamage += OnTakenDamage;
            OnDeath += Die;
        }

        private void Die(Vector3 obj)
        {
            
        }

        private void OnTakenDamage(int obj)
        {
            
        }

        public override void DoDamage(IDamageable target, int damage)
        {
            base.DoDamage(target, damage);
        }

        public override void TakeDamage(IDamageSender sender, int damage)
        {
            if (_damageColorCoroutine != null)
                StopCoroutine(_damageColorCoroutine);
            _damageColorCoroutine = StartCoroutine(ChangeColorFromDamage());
            
            base.TakeDamage(sender, damage);
        }

        private IEnumerator ChangeColorFromDamage()
        {
            var material = _renderer.material;
            material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            material.color = Color.white;
        }
    }
}