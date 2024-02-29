using Data;

namespace Services.PersistentData
{
    public interface IPersistentDataService
    {
        PlayerSettingsData Settings { get; set; }
        PlayerProgressData Progress { get; set; }
        PlayerEconomyData Economy { get; set; }
    }
}