using System;
using System.Reflection;
using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Modifier
{
    public abstract class AbstractValueModifier<T> : IModifier
    {
        public string AttributeName;
        public T Amount;

        public abstract void Apply(WeaponStaticData weapon);

        protected TFieldType GetAttribute<TFieldType>(WeaponStaticData weapon, out object targetObject, out FieldInfo fieldInfo)
        {
            string[] paths = AttributeName.Split("/");
            string attribute = paths[^1];
            
            Type type = weapon.GetType();
            object target = weapon;

            for (int i = 0; i < paths.Length - 1; i++)
            {
                FieldInfo field = type.GetField(paths[i]);
                if (field == null)
                {
                    Debug.Log($"Can't find {AttributeName}");
                    throw new InvalidPathSpecifiedException(AttributeName);
                }
                else
                {
                    target = field.GetValue(target);
                    type = target.GetType();
                }
            }

            FieldInfo attributeField = type.GetField(attribute);
            if (attributeField == null)
            {
                Debug.Log($"Can't find {AttributeName}");
                throw new InvalidPathSpecifiedException(AttributeName);
            }

            fieldInfo = attributeField;
            targetObject = target;
            return (TFieldType)attributeField.GetValue(target);
        }
    }
}