using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using Zenject;

namespace Services.Economy
{
    public interface IEconomyService : IInitializable
    {
        IntReactiveProperty PlayerCurrency { get; set; }
        event Action<bool> OnCompletePurchase;
        
        Dictionary<string, int> GetInventoryItems { get; }
        List<string> GetAvailableItems();
        (bool, int) IsItemObtainedAndCount(string itemKey);
        Task BuyItem(string itemKey);
    }
}