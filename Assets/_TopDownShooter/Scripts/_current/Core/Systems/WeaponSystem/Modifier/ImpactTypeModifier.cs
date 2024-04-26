using _current.Core.Systems.ImpactSystem;

namespace _current.Core.Systems.WeaponSystem.Modifier
{
    public class ImpactTypeModifier : AbstractValueModifier<ImpactType>
    {
        public override void Apply(WeaponStaticData weapon)
        {
            weapon.ImpactType = Amount;
        }
    }
}