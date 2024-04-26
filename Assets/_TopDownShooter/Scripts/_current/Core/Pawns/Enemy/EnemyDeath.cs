using System.Collections;
using _current.Core.Pawns.Components;
using DG.Tweening;
using UnityEngine;

namespace _current.Core.Pawns.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyDeath : PawnDeath
    {
        [SerializeField] private AgentMoveToPlayer _agent;
        [SerializeField] private float _destroyDelayTime = 5f;
        [SerializeField] private float _deathPositionY = -5;

        protected override void Die()
        {
            _agent.enabled = false;
            StartCoroutine(DestroyTimer());
            base.Die();
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_destroyDelayTime);
            transform
                .DOMoveY(_deathPositionY, _destroyDelayTime)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}