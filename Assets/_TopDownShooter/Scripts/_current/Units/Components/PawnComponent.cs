using System;
using System.Collections.Generic;
using UnityEngine;

namespace _current.Units.Components
{
    public abstract class PawnComponent<T> : MonoBehaviour, IPawnComponent where T : BasePawn
    {
        protected T _pawn;

        protected readonly List<IDisposable> _disposables = new();

        protected virtual void Awake()
        {
            _pawn = GetComponent<T>();
            BindInput();
        }
        
        protected abstract void BindInput();
        protected abstract void UnbindInput();

        public virtual void Dispose()
        {
            UnbindInput();
            foreach (var disposable in _disposables) 
                disposable.Dispose();
        }

        protected virtual void Update()
        {
            if (!_pawn.IsAlive) 
                enabled = false;
        }
    }

    public interface IPawnComponent : IDisposable
    {
    }
}