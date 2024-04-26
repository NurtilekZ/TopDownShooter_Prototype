using _current.Core.Pawns.Enemy;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TriggerObserver))]
    public class TriggerObserverEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(TriggerObserver observer, GizmoType type)
        {
            Gizmos.color = observer.TriggerColor;
            Gizmos.DrawWireSphere(observer.Collider.transform.position, observer.Collider.radius);
        }
    }
}