using UnityEngine;

namespace _current.StaticData.ScriptableObjects
{
    [CreateAssetMenu(menuName = "StaticData/Hero", fileName = "HeroData", order = 0)]
    public class HeroStaticData : ScriptableObject
    {
        public int Hp = 150;
        public GameObject PlayerVirtualCamera;
        public GameObject GroupComposer;
    }
}