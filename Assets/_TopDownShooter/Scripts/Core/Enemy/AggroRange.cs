using System.Collections.Generic;
using Core.Logic;
using UnityEngine;

namespace Core.Enemy
{
    public class AggroRange : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _aggroTrigger;
        [SerializeField] private List<EnemyFollowBase> _enemyFollowComponents;

        private void Start()
        {
            _aggroTrigger.TriggerEnter += TriggerEnter;
            _aggroTrigger.TriggerExit += TriggerExit;
        }

        private void OnDestroy()
        {
            _aggroTrigger.TriggerEnter -= TriggerEnter;
            _aggroTrigger.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider obj)
        {
            _enemyFollowComponents.ForEach(fc => fc.FollowTo());
        }

        private void TriggerExit(Collider obj)
        {
            _enemyFollowComponents.ForEach(fc=>fc.Stop());
        }
    }
}