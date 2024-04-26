using System.Reflection;
using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Modifier
{
    public class DamageModifier : AbstractValueModifier<float>
    {
        public override void Apply(WeaponStaticData weapon)
        {
            try
            {
                ParticleSystem.MinMaxCurve damageCurve =
                    GetAttribute<ParticleSystem.MinMaxCurve>(weapon, out var targetObject, out FieldInfo fieldInfo);

                switch (damageCurve.mode)
                {
                    case ParticleSystemCurveMode.TwoConstants:
                        damageCurve.constantMin *= Amount;
                        damageCurve.constantMax *= Amount;
                        break;
                    case ParticleSystemCurveMode.Curve:
                    case ParticleSystemCurveMode.TwoCurves:
                        damageCurve.curveMultiplier *= Amount;
                        break;
                    case ParticleSystemCurveMode.Constant:
                        damageCurve.constant *= Amount;
                        break;
                }
                
                fieldInfo.SetValue(targetObject, damageCurve);
            }
            catch (InvalidPathSpecifiedException e) { }
        }
    }
}