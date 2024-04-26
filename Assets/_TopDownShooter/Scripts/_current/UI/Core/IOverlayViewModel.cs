using _current.UI.Core.MVVM;
using UnityEngine;

namespace _current.UI.Core
{
    public interface IOverlayViewModel : IViewModel
    {
        Transform AnchorTransform { get; }
    }
}