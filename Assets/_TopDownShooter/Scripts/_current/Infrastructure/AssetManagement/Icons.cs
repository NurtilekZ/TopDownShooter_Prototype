using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace _current.Infrastructure.AssetManagement
{
    public static class Icons
    {
        private static readonly Dictionary<string, Sprite> _icons = new();

        public static Sprite GetIcon(string name)
        {
            if (_icons.TryGetValue(name, out var sprite))
            {
                return sprite;
            }

            
            var spriteAtlases = Resources.LoadAll<SpriteAtlas>("UI/SpriteAtlases");
            foreach (var atlas in spriteAtlases)
            {
                var newSprite = atlas.GetSprite(name);
                if(newSprite == null)
                    continue;
                _icons.Add(name, newSprite);
                return newSprite;
            }

            return null;
        }
    }
}