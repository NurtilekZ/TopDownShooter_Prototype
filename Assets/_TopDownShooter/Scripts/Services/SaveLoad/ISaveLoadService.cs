using System.Threading.Tasks;
using Data;

namespace Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        Task<PlayerProgressData> LoadProgress();
        
        void SaveEconomy();
        Task<PlayerEconomyData> LoadEconomy();
        
        void SaveSettings();
        Task<PlayerSettingsData> LoadSettings();
    }
}