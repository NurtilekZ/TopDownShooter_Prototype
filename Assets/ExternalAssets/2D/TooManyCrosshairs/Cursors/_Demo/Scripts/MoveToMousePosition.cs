using UnityEngine;

namespace ExternalAssets._2D.TooManyCrosshairs.Cursors._Demo.Scripts
{
    public class MoveToMousePosition : MonoBehaviour
    {
        void Update()
        {
            this.transform.position = Input.mousePosition;
        }
    }
}
