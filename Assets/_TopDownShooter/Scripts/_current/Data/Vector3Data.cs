using System;

namespace _current.Data
{
    [Serializable]
    public class Vector3Data
    {
        public float x;
        public float z;
        public float y;

        public Vector3Data(float x, float z, float y)
        {
            this.x = x;
            this.z = z;
            this.y = y;
        }
    }
}