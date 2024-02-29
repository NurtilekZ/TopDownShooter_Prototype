using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgressData
    {
        public HashSet<string> CompletedStages { get; set; }
    }
}