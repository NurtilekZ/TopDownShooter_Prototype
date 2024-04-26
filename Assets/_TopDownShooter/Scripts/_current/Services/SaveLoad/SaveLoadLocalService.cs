using System.Threading.Tasks;
using _current.Data.Data;
using _current.Services.PersistentData;
using Newtonsoft.Json;
using UnityEngine;

namespace _current.Services.SaveLoad
{
    public class SaveLoadLocalService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";
        private const string SettingsKey = "Settings";
        private const string EconomyKey = "Economy";

        private readonly IPersistentDataService _persistentDataService;

        public SaveLoadLocalService(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void SaveProgress()
        {
            SaveData(_persistentDataService.Progress, ProgressKey);
        }

        public void SaveEconomy()
        {
            SaveData(_persistentDataService.Economy, EconomyKey);
        }

        public void SaveSettings()
        {
            SaveData(_persistentDataService.Settings, SettingsKey);
        }

        public Task<PlayerProgressData> LoadProgress()
        {
            return LoadData<PlayerProgressData>(ProgressKey);
        }

        public Task<PlayerEconomyData> LoadEconomy()
        {
            return LoadData<PlayerEconomyData>(EconomyKey);
        }

        public Task<PlayerSettingsData> LoadSettings()
        {
            return LoadData<PlayerSettingsData>(SettingsKey);
        }

        private void SaveData<T>(T dataType, string dataKey)
        {
            var data = JsonConvert.SerializeObject(dataType);
            PlayerPrefs.SetString(dataKey, data);
        }

        private Task<T> LoadData<T>(string dataKey)
        {
            var data = JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(dataKey));
            return Task.FromResult(data);
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}