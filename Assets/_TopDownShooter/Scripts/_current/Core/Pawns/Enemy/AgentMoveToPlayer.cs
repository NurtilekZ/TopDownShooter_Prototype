using _current.Core.Pawns.Components;
using _current.Infrastructure.Factories.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace _current.Core.Pawns.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentMoveToPlayer : PawnComponent
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private PawnAnimationHandler _animationHandler;
        [SerializeField] private Transform _target;
        [SerializeField] private float _minDistance = 1f;
        [SerializeField] private float _dampTime = 0.1f;

        private bool _isIdle;

        private IHeroFactory _heroFactory;

        [Inject]
        private void Construct(IHeroFactory heroFactory)
        {
            _heroFactory = heroFactory;
            _target = heroFactory.Hero.transform;
        }

        protected override void Bind() { }
        protected override void Unbind() { }

        private void Update()
        {
            if (_target != null & HeroNotReachable())
            {
                _navMeshAgent.SetDestination(_target.position);
                HandleIdleState();
                UpdateAnimations();
            }
        }

        private bool HeroNotReachable() => 
            Vector3.Distance(_navMeshAgent.transform.position, _target.transform.position) >= _minDistance;

        private void HandleIdleState()
        {
            _isIdle = _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
            _animationHandler.SetIdle(_isIdle);
        }

        private void UpdateAnimations()
        {
            var moveDirection = transform.InverseTransformDirection(_navMeshAgent.destination - transform.position);

            _animationHandler.UpdateMovements(moveDirection, _dampTime);
        }
    }
}