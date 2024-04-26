using System;

namespace _current.UI.Core.MVVM
{
    public interface IViewModel : IDisposable
    {
        bool IsInSceneCanvas { get; }
        string AssetPath { get; }
    }
}