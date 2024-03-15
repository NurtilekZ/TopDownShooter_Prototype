using Services.Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace _old.Player
{
    public class PlayerPawn : BasePawn
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected RigBuilder _rigBuilder;
        public Animator Animator => _animator;

        [field:SerializeField] public Camera PlayerCamera { get; private set; }
        public PlayerControls PlayerControls { get; private set; }

        [Inject]
        public void Construct(Camera playerCamera)
        {
            PlayerCamera = playerCamera;
        }

        private void OnEnable()
        {
            PlayerControls ??= new PlayerControls();
            PlayerControls.Player.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Player.Disable();
        }

        public override void Dispose()
        {
            PlayerControls.Dispose();
            base.Dispose();
        }

        public void BuildRig()
        {
            _rigBuilder.Build();
        }
    }
}