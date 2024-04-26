using System;

namespace _current.Data.Data
{
    [Serializable]
    public class PlayerSettingsData
    {
        public byte MusicVolume { get; set; }
        public byte SfxVolume { get; set; }
        public bool DebugEnabled { get; set; }
    }
}