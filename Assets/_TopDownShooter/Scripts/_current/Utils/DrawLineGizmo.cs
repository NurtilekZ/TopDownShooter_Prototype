using UnityEngine;

namespace _current.Utils
{
    public class DrawLineGizmo : MonoBehaviour
    {
        public Transform from;
        public Transform to;
        private void OnDrawGizmos()
        {
            Debug.DrawLine(from.position, to.position, Color.green);
        }
    }
}
