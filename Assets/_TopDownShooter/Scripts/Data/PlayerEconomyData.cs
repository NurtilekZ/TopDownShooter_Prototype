using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerEconomyData
    {
        public int PlayerCurrency { get; set; }
        public Dictionary<string, int> InventoryItems { get; set; }
    }
}