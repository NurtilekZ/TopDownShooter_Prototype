using System;
using Services.Input;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _current.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 MoveAxis { get; }
        Vector2 LookAxis { get; }
        IReadOnlyReactiveProperty<bool> IsMoving { get; }
        IReadOnlyReactiveProperty<bool> IsRotating { get; }
        bool IsAttackStillPressed { get; }
        InputDevice InputDevice { get; }
        PlayerControls.PlayerActions Player { get; }
        PlayerControls.UIActions UI { get; }
        event Action OnAttackPressed;
        event Action OnReloadPressed;
        event Action OnChangeWeaponPressed;
        event Action OnInteractPressed;
        event Action OnSprintPressed;
        event Action OnSkillPressed;
        event Action OnRollPressed;
    }
}