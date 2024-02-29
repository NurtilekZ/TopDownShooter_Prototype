using UnityEngine;
using UnityEngine.AI;

namespace Core.Enemy
{
    public class EnemyFollowMove : EnemyFollowBase
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyAnimator _animator;

        private void Update()
        {
            _animator.PlayMove(_agent.velocity.magnitude);

            if (Enabled)
                _agent.destination = HeroTransform.position;
        }
    }
}