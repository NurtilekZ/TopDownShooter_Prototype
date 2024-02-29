using System;
using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        Vector2 MoveAxis { get; }
        Vector2 AimAxis { get; }

        event Action AttackPressed;
    }
}