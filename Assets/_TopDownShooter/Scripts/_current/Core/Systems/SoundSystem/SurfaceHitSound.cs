using System;
using UnityEngine;

namespace _current.Core.Systems.SoundSystem
{
    [CreateAssetMenu(menuName = "Sound", fileName = "SurfaceHitSound")]
    [Serializable]
    public class SurfaceHitSound : ScriptableObject
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private AudioClip _sound;

        public LayerMask Layer => _layer;
        public AudioClip Sound => _sound;
    }
}