using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _current.Core.Pawns.LootSystem;
using _current.Data;
using UniRx;
using Zenject;

namespace _current.Services.Economy
{
    public interface IEconomyService : IInitializable
    {
        IReadOnlyReactiveDictionary<CurrencyTypeId, int> PlayerCurrency { get; }
        event Action<bool> OnCompletePurchase;
        
        Dictionary<LootTypeId, int> GetInventoryItems { get; }
        List<LootTypeId> GetAvailableItems();
        (bool, int) IsItemObtainedAndCount(LootTypeId itemKey);
        Task BuyItem(Loot loot);
        void InitializeCurrency(Dictionary<CurrencyTypeId,int> economyPlayerCurrency);
    }
}