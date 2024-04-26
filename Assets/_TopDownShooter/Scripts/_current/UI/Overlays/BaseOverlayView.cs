using _current.UI.Core;
using UnityEngine;

namespace _current.UI.Overlays
{
    public abstract class BaseOverlayView<TViewModel> : BaseView<TViewModel> where TViewModel : IOverlayViewModel
    {
        private Camera _camere;

        private void Start() => 
            _camere = Camera.main;

        protected virtual void Update() => 
            transform.position = _camere.WorldToScreenPoint(_viewModel.AnchorTransform.position);
    }
}