using _current.UI.Core;
using UnityEngine;

namespace _current.UI.Overlays
{
    public abstract class BaseOverlayViewModel : IOverlayViewModel
    {
        public bool IsInSceneCanvas => true;
        public abstract string AssetPath { get; protected set; }

        public Transform AnchorTransform { get; }

        protected BaseOverlayViewModel(Transform anchorTransform)
        {
            this.AnchorTransform = anchorTransform;
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}