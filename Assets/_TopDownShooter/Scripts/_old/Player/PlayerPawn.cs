using Services.Input;
using UnityEngine;
using Zenject;

namespace _old.Player
{
    public class PlayerPawn : BasePawn
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected Rigidbody _rigidbody;
        public Animator Animator => _animator;
        public Rigidbody RigidBody => _rigidbody;

        public Camera PlayerCamera { get; private set; }
        public PlayerControls PlayerControls { get; private set; }

        private void OnEnable()
        {
            PlayerControls.Player.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Player.Disable();
        }

        [Inject]
        public void Construct(Camera playerCamera)
        {
            PlayerCamera = playerCamera;
            PlayerControls = new PlayerControls();
        }

        public override void Dispose()
        {
            PlayerControls.Dispose();
            base.Dispose();
        }
    }
}