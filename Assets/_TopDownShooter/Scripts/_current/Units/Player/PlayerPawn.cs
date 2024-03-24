using _current.Systems.DamageSystem;
using Services.Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace _current.Units.Player
{
    public class PlayerPawn : BasePawn
    {
        [SerializeField] protected Animator _animator;
        [SerializeField] protected AudioSource _audioSource;
        [SerializeField] protected RigBuilder _rigBuilder;
        [field:SerializeField] public Camera PlayerCamera { get; private set; }
        
        public Animator Animator => _animator;
        public AudioSource AudioSource => _audioSource;
        public PlayerControls PlayerControls { get; private set; }
        public override bool IsAlive { get; protected set; }

        [Inject]
        public void Construct(Camera playerCamera)
        {
            PlayerCamera = playerCamera;
        }

        private void OnEnable()
        {
            PlayerControls ??= new PlayerControls();
            PlayerControls.Player.Enable();
            IsAlive = true;
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