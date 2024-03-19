using System;
using System.Collections.Generic;
using UnityEngine;

namespace _current.Components
{
    public abstract class PawnComponent<T> : MonoBehaviour, IDisposable, IPawnComponent where T : BasePawn
    {
        protected T _pawn;

        protected List<IDisposable> Disposables = new();

        protected virtual void Start()
        {
            _pawn = GetComponent<T>();
            BindPlayerInput();
        }

        public virtual void Dispose()
        {
            foreach (var disposable in Disposables) disposable.Dispose();
            UnbindPlayerInput();
        }

        public abstract void BindPlayerInput();
        public abstract void UnbindPlayerInput();
    }

    public interface IPawnComponent
    {
        void BindPlayerInput();
        void Dispose();
    }
}