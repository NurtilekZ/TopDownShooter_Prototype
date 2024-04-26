using System.Threading.Tasks;
using _current.Data.Data;

namespace _current.Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        Task<PlayerProgressData> LoadProgress();
        
        void SaveEconomy();
        Task<PlayerEconomyData> LoadEconomy();
        
        void SaveSettings();
        Task<PlayerSettingsData> LoadSettings();
    }
}