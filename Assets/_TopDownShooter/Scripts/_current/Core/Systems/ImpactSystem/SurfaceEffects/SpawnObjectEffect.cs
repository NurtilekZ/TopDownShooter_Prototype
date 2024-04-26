using UnityEngine;

namespace _current.Core.Systems.ImpactSystem.SurfaceEffects
{
    [CreateAssetMenu(menuName = "Impact System/Surface Effects/Spawn Object Effect", fileName = "SpawnObjectEffect")]
    public class SpawnObjectEffect : ScriptableObject
    {
        public GameObject Prefab;
        public float Probability = 1;
        public bool RandomizeRotation;
        public Vector3 RandomizedRotationMultiplier = Vector3.zero;
    }
}