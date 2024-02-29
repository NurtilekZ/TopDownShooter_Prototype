using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.PersistentData;
using Services.SaveLoad;
using Services.StaticData;
using UniRx;

namespace Services.Economy
{
    public class EconomyLocalService : IEconomyService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        private List<string> _availableItems;
        
        public event Action<bool> OnCompletePurchase;
        public IntReactiveProperty PlayerCurrency { get; set; }
        
        public EconomyLocalService(IStaticDataService staticDataService, IPersistentDataService persistentDataService, ISaveLoadService saveLoadService)
        {
            _staticDataService = staticDataService;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetInventoryItems => 
            _persistentDataService.Economy.InventoryItems;

        public List<string> GetAvailableItems() => 
            _availableItems ?? _staticDataService.GetAllItems.Select(x => x.ItemId).ToList();

        public (bool, int) IsItemObtainedAndCount(string itemKey) => 
            (_persistentDataService.Economy.InventoryItems.TryGetValue(itemKey, out var count), count);

        public Task BuyItem(string itemKey)
        {
            var itemPrice = _staticDataService.ForInventoryItem(itemKey).Price;
            if (itemPrice > PlayerCurrency.Value)
            {
                //reject purchase
                OnCompletePurchase?.Invoke(false);
            }
            else
            {
                //proceedPurchase
                _persistentDataService.Economy.InventoryItems.TryGetValue(itemKey, out var currentCount);
                _persistentDataService.Economy.InventoryItems[itemKey] = currentCount + 1;

                PlayerCurrency.Value -= itemPrice;
                OnCompletePurchase?.Invoke(true);
                
                _saveLoadService.SaveEconomy();
            }

            return Task.CompletedTask;
        }
    }
}