using _current.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _current.Components
{
    public class CameraPlayerFollowBinding : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform _target;
        [SerializeField] private float _minOffsetZ;
        [SerializeField] private float _maxOffsetZ;
        [SerializeField] private float _minOffsetX;
        [SerializeField] private float _maxOffsetX;
        [SerializeField] private float _cameraMoveSpeed;
        [SerializeField] private float _cameraSmoothTime;
        
        private CinemachineTransposer _transposer;
        private float _offsetZ;
        private float _offsetY;
        private Vector3 _movementVelocity;
        private Vector3 _smooth;

        [Inject]
        private void Construct(PlayerPawn pawn, TargetPawn targetPawn)
        {
            _virtualCamera.Follow = pawn.transform;
            _virtualCamera.LookAt = targetPawn.transform;
        }

        private void Awake()
        {
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _offsetZ = _transposer.m_FollowOffset.z;
            _offsetY = _transposer.m_FollowOffset.y;
        }

        private void LateUpdate()
        {
            var offset = _virtualCamera.m_Follow.position - _target.position;
            
            _smooth = Vector3.SmoothDamp(_smooth, offset, ref _movementVelocity, _cameraSmoothTime);

            _transposer.m_FollowOffset = _smooth * (_cameraMoveSpeed * Time.deltaTime);
        }
    }
}