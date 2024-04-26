using System;
using System.Collections.Generic;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public abstract class PawnComponent : MonoBehaviour, IPawnComponent
    {
        protected readonly List<IDisposable> _disposables = new();

        protected virtual void Start() => 
            Bind();

        protected void OnDestroy() => 
            Dispose();

        protected abstract void Bind();
        protected abstract void Unbind();

        public virtual void Dispose()
        {
            Unbind();
            foreach (var disposable in _disposables) 
                disposable?.Dispose();
        }
    }
}