using System;
using System.Collections.Generic;
using _current.Core.Pawns.Components;
using _current.Core.Systems.DamageSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _current.Core.Systems.AbilitySystem
{
    public class PlayerAbilityHandler : PawnComponent
    {
        [SerializeField] private AbilityStorage _abilityStorage;

        private List<IAbility> _abilities = new();
        private IAbility _currentAbility;
        
        private Camera _camera;
        [SerializeField] private LayerMask _targetLayer;

        protected override void Bind()
        {
            _camera = Camera.main;
            _abilityStorage.Init();
            _abilities.AddRange(_abilityStorage.GetAbilities());

            foreach (var ability in _abilities)
            {
                ability.KeyBind.performed += StartUse(ability);
            }
        }

        private Action<InputAction.CallbackContext> StartUse(IAbility ability)
        {
            _currentAbility?.Cancel();
            switch (ability.Status)
            {
                case EAbilityStatus.Ready:
                    _currentAbility = ability;
                    _currentAbility.Activate();
                    break;
                case EAbilityStatus.Cooldown:
                case EAbilityStatus.NotReady:
                    break;
            }

            return null;
        }

        protected void Update()
        {
            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].Tick(Time.deltaTime);
            }

            if (_currentAbility != null)
            {
                if (Mouse.current.rightButton.isPressed)
                {
                    _currentAbility.Cancel();
                    _currentAbility = null;
                    return;
                }

                var location = Vector3.zero;
                IHealth target = null;

                Ray ray = _camera.ScreenPointToRay(Mouse.current.position.value);
                RaycastHit[] hitResult = Physics.RaycastAll(ray, 500.0f, _targetLayer);

                for (int i = 0; i < hitResult.Length; i++)
                {
                    hitResult[i].collider.TryGetComponent(out target);
                    if (hitResult[i].collider.CompareTag("Ground"))
                    {
                        location = hitResult[i].point;
                    }
                }

                if (_currentAbility.Status == EAbilityStatus.Ready)
                {
                    
                }
            }
        }

        protected override void Unbind()
        {
        }
    }
}