using UnityEngine;

namespace _current.Core.Logic.EnemySpawners
{
    public class SceneMarker : MonoBehaviour
    {
        [Header("Gizmos Settings")]
        public Mesh Mesh;
        public Color SphereColor;
        public Color MeshColor;
        public float MeshScale;
        public Vector3 MeshPosOffset;
        public Vector3 MeshRotationOffset;
    }
}