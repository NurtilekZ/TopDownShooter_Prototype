using System;

namespace _current.Core.Pawns.LootSystem
{
    [Serializable]
    public enum LootTypeId
    {
        Ammo,
        AidKit,
        Supply
    }

    [Serializable]
    public enum CurrencyTypeId
    {
        Ordinary,
        Rare,
        SuperRare
    }
}