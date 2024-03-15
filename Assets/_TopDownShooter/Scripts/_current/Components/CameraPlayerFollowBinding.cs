using _old.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _old.Components
{
    public class CameraPlayerFollowBinding : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [Inject]
        private void Construct(PlayerPawn pawn)
        {
            _virtualCamera.Follow = pawn.transform;
            _virtualCamera.LookAt = pawn.transform;
        }
    }
}