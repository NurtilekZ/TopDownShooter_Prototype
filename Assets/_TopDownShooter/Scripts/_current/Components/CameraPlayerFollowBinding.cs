using System;
using _old.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _old.Components
{
    public class CameraPlayerFollowBinding : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float _minOffsetZ;
        [SerializeField] private float _maxOffsetZ;
        [SerializeField] private float _minOffsetX;
        [SerializeField] private float _maxOffsetX;
        [SerializeField] private float _cameraMoveSpeedX;
        [SerializeField] private float _cameraMoveSpeedZ;
        
        private CinemachineTransposer _transposer;
        private float _offsetZ;
        private float _offsetY;

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
            var offset = _virtualCamera.m_Follow.position - _virtualCamera.LookAt.position;
            
            var animHor = Mathf.Clamp(Vector3.Dot(offset, transform.right) * _cameraMoveSpeedX, _minOffsetX,_maxOffsetX);
            var animVer = Mathf.Clamp(Vector3.Dot(offset, transform.forward) * _cameraMoveSpeedZ, _minOffsetZ,_maxOffsetZ);
            var off = new Vector3(animVer, _offsetY,_offsetZ + animHor);

            _transposer.m_FollowOffset = off;
        }
    }
}