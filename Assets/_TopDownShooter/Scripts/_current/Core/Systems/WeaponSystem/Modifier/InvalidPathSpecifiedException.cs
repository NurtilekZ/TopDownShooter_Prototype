using System;

namespace _current.Core.Systems.WeaponSystem.Modifier
{
    public class InvalidPathSpecifiedException : Exception
    {
        public InvalidPathSpecifiedException(string attributeName) : base($"Attribute {attributeName} does not exists at the provided path!") { }
    }
}