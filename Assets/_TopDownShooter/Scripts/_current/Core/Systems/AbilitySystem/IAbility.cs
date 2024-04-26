using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _current.Core.Systems.AbilitySystem
{
    public interface IAbility
    {
        string Title { get; }
        string Description { get; }
        Sprite Image { get; }
        float CooldownTime { get; }
        float Cost { get; }
        EAbilityStatus Status { get; }
        public InputAction KeyBind { get; }

        void Activate();
        bool IsReady();
        void Apply();
        void Tick(float deltaTime);
        void Cancel();
    }

    public class Ability : IAbility
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Sprite Image { get; private set; }
        public float CooldownTime { get; private set; }
        public float CooldownTimer { get; private set; }
        public float Cost { get; private set; }
        public EAbilityStatus Status { get; private set; }
        public InputAction KeyBind { get; private set; }

        public event Action<float, float> OnCooldownTimerChange; 

        public Ability(string title, string description, Sprite image, float cooldownTime, float cost, InputAction keyBind)
        {
            Title = title;
            Description = description;
            Image = image;
            CooldownTime = cooldownTime;
            Cost = cost;
            KeyBind = keyBind;
        }

        public virtual void Activate()
        {
            
        }

        public virtual bool IsReady()
        {
            return Status == EAbilityStatus.Ready;
        }

        public virtual void Apply()
        {
            CooldownTimer = CooldownTime;
        }

        public virtual void Tick(float deltaTime)
        {
            CooldownTimer += deltaTime;
        }

        public virtual void Cancel()
        {
            
        }
    }

    public enum EAbilityStatus
    {
        Ready,
        Cooldown,
        NotReady
    }
}