using _current.Data;
using UnityEngine;

namespace _current.Utils
{
    public static class DataExtensions
    {
        public static Vector3Data AsVectorData(this Vector3 vector) =>
            new Vector3Data(vector.x, vector.y, vector.z);
        public static Vector3 AsUnityVector3(this Vector3Data vector) =>
            new Vector3(vector.x, vector.y, vector.z);

        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);

        public static T ToDeserialized<T>(this string json) => 
            JsonUtility.FromJson<T>(json);
        
        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y += y;
            return vector;
        }
    }
}