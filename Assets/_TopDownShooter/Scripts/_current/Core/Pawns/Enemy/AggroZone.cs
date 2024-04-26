using System.Collections;
using UnityEngine;

namespace _current.Core.Pawns.Enemy
{
    public class AggroZone : MonoBehaviour
    {
        [SerializeField] private float _cooldownTime = 1;
        [SerializeField] private TriggerObserver _triggerObserver;
        [SerializeField] private AgentMoveToPlayer _agentMoveToPlayer;
        
        private Coroutine _switchAgentCooldown;
        private bool _hasAggroTarget;

        public void Initialize(float aggroRadius)
        {
            _triggerObserver.Collider.radius = aggroRadius;
            _triggerObserver.TriggerEnter += TriggerEnter;
            _triggerObserver.TriggerExit += TriggerExit;
            SwitchAgent(false);
        }

        private void TriggerEnter(Collider obj)
        {
            if (!_hasAggroTarget)
            {
                _hasAggroTarget = true;
                StopCooldownCoroutine();
                SwitchAgent(true);
            }
        }

        private void TriggerExit(Collider obj)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;
                _switchAgentCooldown = StartCoroutine(SwitchAgentCooldown(false));
            }
        }

        private IEnumerator SwitchAgentCooldown(bool enable)
        {
            yield return new WaitForSeconds(_cooldownTime);
            SwitchAgent(enable);
        }

        private void StopCooldownCoroutine()
        {
            if (_switchAgentCooldown != null)
            {
                StopCoroutine(_switchAgentCooldown);
                _switchAgentCooldown = null;
            }
        }

        private void SwitchAgent(bool enable)
        {
            _agentMoveToPlayer.enabled = enable;
        }

        private void OnDestroy()
        {
            if (_triggerObserver != null)
            {
                _triggerObserver.TriggerEnter -= TriggerEnter;
                _triggerObserver.TriggerExit -= TriggerExit;
            }
        }
    }
}