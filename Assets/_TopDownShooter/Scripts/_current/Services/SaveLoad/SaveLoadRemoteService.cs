﻿using System.Threading.Tasks;
using _current.Data.Data;

namespace _current.Services.SaveLoad
{
    public class SaveLoadRemoteService : ISaveLoadService
    {
        public void SaveProgress()
        {
            throw new System.NotImplementedException();
        }

        public Task<PlayerProgressData> LoadProgress()
        {
            throw new System.NotImplementedException();
        }

        public void SaveEconomy()
        {
            throw new System.NotImplementedException();
        }

        public Task<PlayerEconomyData> LoadEconomy()
        {
            throw new System.NotImplementedException();
        }

        public void SaveSettings()
        {
            //CloudSaveService.Instance.Data.ForceSaveAsync(_dataService.SettingsData);
        }

        public Task<PlayerSettingsData> LoadSettings()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}