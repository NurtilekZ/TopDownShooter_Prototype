using System.Collections.Generic;
using UnityEngine;

namespace _current.Core.Systems.ImpactSystem.SurfaceEffects
{
    [CreateAssetMenu(menuName = "Impact System/Surface Effects/Play Audio Effect", fileName = "PlayAudioEffect")]
    public class PlayAudioEffect : ScriptableObject
    {
        public AudioSource AudioSourcePrefab;
        public List<AudioClip> AudioClips = new List<AudioClip>();
        
        [Tooltip("Values are clamped to 1-0")]
        public Vector2 VolumeRange = new Vector2(0, 1);
    }
}