using System.Collections.Generic;
using _current.Systems.ImpactSystem.SurfaceEffects;
using UnityEngine;

namespace _current.Systems.ImpactSystem
{
    [CreateAssetMenu(menuName = "Impact System/Surface Effect", fileName = "SurfaceEffect")]
    public class SurfaceEffect : ScriptableObject
    {
        public List<SpawnObjectEffect> SpawnObjectEffects = new List<SpawnObjectEffect>();
        public List<PlayAudioEffect> PlayAudioEffects = new List<PlayAudioEffect>();
    }
}