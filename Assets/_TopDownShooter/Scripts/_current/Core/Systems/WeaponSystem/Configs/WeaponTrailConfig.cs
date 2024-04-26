using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "StaticData/WeaponConfig", fileName = "TrailConfig", order = 3)]
    public class WeaponTrailConfig : ScriptableObject
    {
        public Material Material;
        public AnimationCurve WidthCurve;
        public float Duration = 0.5f;
        public float MinVertexDistance = 0.1f;
        public Gradient Color;

        public float MissDistance = 100f;
        public float SimulationSpeed = 100f;
    }
}