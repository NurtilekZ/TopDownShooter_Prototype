using _current.Core.Pawns.Player;
using _current.Services.Logging;
using _current.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace _current.Core.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private Color _color;
        
        private ISaveLoadService _saveLoadService;
        private ILoggingService _loggingService;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _saveLoadService = saveLoadService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerHealth>(out _)) return;
            _saveLoadService.SaveProgress();
            _loggingService.LogMessage("Progress saved in checkpoint", this, LoggingTag.Game);
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if (!_boxCollider) 
                return;
            
            Gizmos.color = _color;
            Gizmos.DrawCube(transform.position + _boxCollider.center, _boxCollider.size);
        }
    }
}