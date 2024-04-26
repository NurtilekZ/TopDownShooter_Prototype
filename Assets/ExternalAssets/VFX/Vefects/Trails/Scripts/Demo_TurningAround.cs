using UnityEngine;

namespace ExternalAssets.VFX.Vefects.Trails.Scripts
{
    public class Demo_TurningAround : MonoBehaviour
    {
        public float rotSpeed_X;
        public float rotSpeed_Y;
        public float rotSpeed_Z;

        public float globalSpeed = 1f;


        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(rotSpeed_X, rotSpeed_Y, rotSpeed_Z) * globalSpeed * Time.deltaTime);
        }
    }
}
