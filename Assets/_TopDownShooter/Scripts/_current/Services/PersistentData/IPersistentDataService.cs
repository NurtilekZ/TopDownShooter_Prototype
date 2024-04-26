using _current.Data.Data;

namespace _current.Services.PersistentData
{
    public interface IPersistentDataService
    {
        PlayerSettingsData Settings { get; set; }
        PlayerProgressData Progress { get; set; }
        PlayerEconomyData Economy { get; set; }
    }
}