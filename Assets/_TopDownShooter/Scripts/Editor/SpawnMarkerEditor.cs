using _current.Core.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SceneMarker))]
    public class SpawnMarkerEditor : UnityEditor.Editor
    {
        
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SceneMarker spawner, GizmoType type)
        {
            Gizmos.color = spawner.SphereColor;
            var position = spawner.transform.position;
            Gizmos.DrawSphere(position, 0.5f);
            Gizmos.color = spawner.MeshColor;
            Gizmos.DrawMesh(spawner.Mesh,
                position + spawner.MeshPosOffset,
                Quaternion.Euler(spawner.MeshRotationOffset),
                Vector3.one * spawner.MeshScale);
        }
    }
}