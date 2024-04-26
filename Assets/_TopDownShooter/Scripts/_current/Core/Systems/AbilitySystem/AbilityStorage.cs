using System.Collections.Generic;
using UnityEngine;

namespace _current.Core.Systems.AbilitySystem
{
    public class AbilityStorage : MonoBehaviour
    {
        [SerializeField] private AbilityConfig[] _abilityConfigs;

        private List<IAbility> _abilities = new();

        public void Init()
        {
            for (int i = 0; i < _abilityConfigs.Length; i++)
            {
                var builder = _abilityConfigs[i].GetBuilder();
                
                builder.Build();
                _abilities.Add(builder.GetResult());
            }
        }

        public IAbility[] GetAbilities() => _abilities.ToArray();
    }
}