using System;

namespace _current.Utils
{
    public class Utilities
    {
        public static void CopyValues<T>(T original, T copy)
        {
            Type type = original.GetType();
            foreach (var field in type.GetFields())
            {
                field.SetValue(copy, field.GetValue(original));
            }
        }
    }
}