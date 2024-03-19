using System.Collections;
using _current.Properties;
using UnityEngine;
using UnityEngine.AI;

namespace _current.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcLocomotion : DamagablePawn
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _target;
        [SerializeField] private Renderer _renderer;

        private Coroutine _damageColorCoroutine;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            _navMeshAgent.SetDestination(_target.position);
        }

        public override void TakeDamage(DamageSender sender, float damage)
        {
            base.TakeDamage(sender, damage);
            if (_damageColorCoroutine != null)
                StopCoroutine(_damageColorCoroutine);

            _damageColorCoroutine = StartCoroutine(ChangeColorFromDamage());
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