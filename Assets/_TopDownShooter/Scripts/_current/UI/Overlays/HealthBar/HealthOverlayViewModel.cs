using _current.Core.Systems.DamageSystem;
using _current.Infrastructure.AssetManagement;
using UnityEngine;

namespace _current.UI.Overlays.HealthBar
{
    public class HealthOverlayViewModel : BaseOverlayViewModel
    {
        public override string AssetPath { get; protected set; } = AssetsPath.HealthOverlay;
        public readonly IHealth health;

        public HealthOverlayViewModel(IHealth health, Transform anchorTransform) : base(anchorTransform)
        {
            this.health = health;
        }
    }
}