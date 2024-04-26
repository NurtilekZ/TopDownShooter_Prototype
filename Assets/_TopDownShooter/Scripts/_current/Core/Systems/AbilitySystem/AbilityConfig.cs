using UnityEngine;
using UnityEngine.InputSystem;

namespace _current.Core.Systems.AbilitySystem
{
    public class AbilityConfig : ScriptableObject
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public float CooldownTime { get; private set; }
        [field: SerializeField] public float Cost { get; private set; }
        [field: SerializeField] public InputAction KeyBind { get; private set; }

        public virtual AbilityBuilder GetBuilder() => new AbilityBuilder(this);
    }
}