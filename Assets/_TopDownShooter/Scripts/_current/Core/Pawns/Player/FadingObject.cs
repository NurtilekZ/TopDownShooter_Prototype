using System;
using System.Collections.Generic;
using UnityEngine;

namespace _current.Core.Pawns.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshRenderer))]
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private List<Renderer> _renderers = new List<Renderer>();
        [SerializeField] private List<Material> _materials = new List<Material>();

        public List<Material> Materials => _materials;

        public float InitialAlpha { get; private set; }

        private void Awake()
        {
            _position = transform.position;
            
            if (_renderers.Count == 0)
            {
                _renderers.AddRange(GetComponentsInChildren<Renderer>());
            }

            foreach (var renderer1 in _renderers)
            {
                Materials.AddRange(renderer1.materials);
            }
            
            InitialAlpha = Materials[0].color.a;
        }

        public bool Equals(FadingObject other)
        {
            return other != null && _position.Equals(other._position);
        }

        public override int GetHashCode()
        {
            return _position.GetHashCode();
        }
    }
}