using System;
using System.Collections.Generic;
using UnityEngine;

namespace _old.Components
{
    public abstract class PawnComponent<T> : MonoBehaviour, IDisposable, IPawnComponent where T : BasePawn
    {
        protected T _pawn;

        protected List<IDisposable> Disposables = new();

        protected virtual void Start()
        {
            _pawn = GetComponent<T>();
            SetupPlayerInput();
        }

        public virtual void Dispose()
        {
            foreach (var disposable in Disposables) disposable.Dispose();
        }

        public abstract void SetupPlayerInput();
    }

    public interface IPawnComponent
    {
        void SetupPlayerInput();
        void Dispose();
    }
}