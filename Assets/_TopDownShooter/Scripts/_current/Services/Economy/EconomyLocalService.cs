using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _current.Core.Pawns.LootSystem;
using _current.Data;
using _current.Services.PersistentData;
using _current.Services.SaveLoad;
using _current.Services.StaticData;
using UniRx;

namespace _current.Services.Economy
{
    public class EconomyLocalService : IEconomyService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        private List<LootTypeId> _availableItems;
        private readonly ReactiveDictionary<CurrencyTypeId, int> _playerCurrency = new();

        public event Action<bool> OnCompletePurchase;

        public IReadOnlyReactiveDictionary<CurrencyTypeId, int> PlayerCurrency => _playerCurrency;

        public EconomyLocalService(IStaticDataService staticDataService, IPersistentDataService persistentDataService, ISaveLoadService saveLoadService)
        {
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            
        }

        public Dictionary<LootTypeId, int> GetInventoryItems => 
            _persistentDataService.Economy.InventoryItems;

        public List<LootTypeId> GetAvailableItems() => 
            _availableItems ?? _staticDataService.GetAllItems.Select(x => x.ItemId).ToList();

        public (bool, int) IsItemObtainedAndCount(LootTypeId itemKey) => 
            (_persistentDataService.Economy.InventoryItems.TryGetValue(itemKey, out var count), count);

        public Task BuyItem(Loot loot)
        {
            var item = _staticDataService.ForInventoryItem(loot.lootTypeId);
            if (item.Price > PlayerCurrency[item.PriceCurrencyType])
            {
                //reject purchase
                OnCompletePurchase?.Invoke(false);
            }
            else
            {
                //proceed purchase
                ObtainItem(loot);
                
                _playerCurrency[item.PriceCurrencyType] -= item.Price;
                OnCompletePurchase?.Invoke(true);
                
                _saveLoadService.SaveEconomy();
            }

            return Task.CompletedTask;
        }

        public void ObtainItem(Loot loot)
        {
            _persistentDataService.Economy.InventoryItems.TryGetValue(loot.lootTypeId, out var currentValue);
            _persistentDataService.Economy.InventoryItems[loot.lootTypeId] = currentValue + loot.value;
        }

        public void InitializeCurrency(Dictionary<CurrencyTypeId, int> economyPlayerCurrency)
        {
            foreach (var currencyItem in economyPlayerCurrency)
            {
                _playerCurrency[currencyItem.Key] = currencyItem.Value;
            }
        }
    }
}