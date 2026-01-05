using ArknightsRoguelikeRec.ViewModel.DataStruct;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.Config
{
    [Serializable]
    public class NodeConfig
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public List<string> SubTypes { get; set; }
        public int ExtraLayer { get; set; }
        public Color NodeColor { get; set; } = Color.Gray;
        public Color TextColor { get; set; } = Color.Black;
    }
}
