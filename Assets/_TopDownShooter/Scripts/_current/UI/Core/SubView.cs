using System;
using System.Collections.Generic;
using UnityEngine;

namespace _current.UI.Core
{
    public abstract class SubView<TData> : MonoBehaviour, ISubView
    {
        protected readonly List<IDisposable> _disposables = new();

        public void Show(TData data)
        {
            Bind(data);
        }

        public void Hide()
        {
            Unbind();
            Dispose();
        }
        
        protected abstract void Bind(TData actions);
        protected abstract void Unbind();
        
        public virtual void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();
        }

    }
}