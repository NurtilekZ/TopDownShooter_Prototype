using System.Collections;
using _current.Data;
using _current.Services.Economy;
using TMPro;
using UnityEngine;
using Zenject;

namespace _current.Core.Pawns.LootSystem
{
    public class LootView : MonoBehaviour
    {
        public GameObject pickupModel;
        public GameObject pickupVFX;
        public TMP_Text lootText;
        public GameObject pickupPopup;
        
        private Loot _loot;
        private bool _isPicked;
        private IEconomyService _economyService;

        [Inject]
        private void Construct(IEconomyService economyService)
        {
            _economyService = economyService;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) => 
            Pickup();

        private void Pickup()
        {
            if (_isPicked) 
                return;
            
            _isPicked = true;
            
            UpdateInventory();
            HideModel();
            PlayPickupFX();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateInventory()
        {
            _economyService.BuyItem(_loot);
        }

        private void HideModel() => 
            pickupModel.SetActive(false);

        private void PlayPickupFX() => 
            Instantiate(pickupVFX, transform);

        private void ShowText()
        {
            lootText.text = $"{_loot.value}";
            pickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}