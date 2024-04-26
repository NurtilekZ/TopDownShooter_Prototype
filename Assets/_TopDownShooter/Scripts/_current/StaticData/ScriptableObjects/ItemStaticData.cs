using _current.Core.Pawns.LootSystem;
using UnityEngine;

namespace _current.StaticData.ScriptableObjects
{
    [CreateAssetMenu(menuName = "StaticData/Items", fileName = "ItemData")]
    public class ItemStaticData : ScriptableObject
    {
        public LootTypeId ItemId;
        public string Title;
        public string Description;
        public int MaxCount;
        public int Price;
        public CurrencyTypeId PriceCurrencyType;
    }
}