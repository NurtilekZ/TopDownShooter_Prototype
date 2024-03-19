using System;
using System.Collections.Generic;
using UnityEngine;

namespace _current
{
    public abstract class BasePawn : MonoBehaviour, IDisposable
    {
        protected List<IDisposable> Disposables = new();

        public virtual void Dispose()
        {
            foreach (var disposable in Disposables) disposable.Dispose();
        }
    }
}