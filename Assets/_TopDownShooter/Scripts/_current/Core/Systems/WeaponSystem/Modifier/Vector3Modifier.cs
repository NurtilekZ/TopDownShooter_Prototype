using System.Reflection;
using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Modifier
{
    public class Vector3Modifier : AbstractValueModifier<Vector3>
    {
        public override void Apply(WeaponStaticData weapon)
        {
            try
            {
                Vector3 value = GetAttribute<Vector3>(weapon, out object targetObject, out FieldInfo fieldInfo);
                value = new(
                    value.x * Amount.x,
                    value.y * Amount.y,
                    value.z * Amount.z);
                fieldInfo.SetValue(targetObject, value);
            }
            catch (InvalidPathSpecifiedException e) { }
        }
    }
}