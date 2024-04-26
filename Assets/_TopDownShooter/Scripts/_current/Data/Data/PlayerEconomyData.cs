using System;
using System.Collections.Generic;
using _current.Core.Pawns.LootSystem;

namespace _current.Data.Data
{
    [Serializable]
    public class PlayerEconomyData
    {
        public Dictionary<CurrencyTypeId, int> PlayerCurrency { get; set; }
        public Dictionary<LootTypeId, int> InventoryItems { get; set; }
    }
}