using System;
using _current.Core.Systems.DamageSystem;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public abstract class AbstractDamageSender : PawnComponent, IDamageSender
    {
        [SerializeField] protected float _damageValue = 5f;
        public abstract event Action<IDamageSender, float> OnAttack;
    }
}