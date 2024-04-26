using _current.Core.Pawns.Components;
using _current.Services.Input;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace _current.Core.Pawns.Player
{
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerDeath : PawnDeath
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerRotation _playerRotation;
        [SerializeField] private PlayerWeaponSpawner _weaponSpawner;
        [SerializeField] private RigBuilder _rigBuilder;
        [SerializeField] private AnimationRigHandler _animationRigHandler;
        [SerializeField] private RagDollController _ragDollController;
        
        private IInputService _inputService;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        protected override void Die()
        {
            _inputService.Player.Disable();
            _playerMovement.enabled = false;
            _playerRotation.enabled = false;
            _animationRigHandler.SetAimWeights(0);
            _animationRigHandler.SetHandsWeights(0);
            foreach (var rigLayer in _rigBuilder.layers) 
                rigLayer.active = false;
            _ragDollController.ActivateRagdoll();
            _weaponSpawner.DropWeapon();
            base.Die();
        }
    }
}