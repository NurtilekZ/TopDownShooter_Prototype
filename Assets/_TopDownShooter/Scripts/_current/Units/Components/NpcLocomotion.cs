using _current.Units.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace _current.Units.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NpcLocomotion : PawnComponent<EnemyPawn>
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _target;


        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        protected override void Update()
        {
            base.Update();
            _navMeshAgent.SetDestination(_target.position);
        }

        protected override void BindInput()
        {
            
        }

        protected override void UnbindInput()
        {
            
        }
    }
}