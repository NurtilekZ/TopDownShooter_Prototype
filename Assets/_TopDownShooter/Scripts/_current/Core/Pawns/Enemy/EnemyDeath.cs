using System.Collections;
using _current.Core.Pawns.Components;
using Cysharp.Threading.Tasks;
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
            if (_agent != null) 
                _agent.enabled = false;
            DestroyTimer();
            base.Die();
        }

        private async void DestroyTimer()
        {
            await UniTask.WaitForSeconds(_destroyDelayTime);
            transform
                .DOMoveY(_deathPositionY, _destroyDelayTime)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}