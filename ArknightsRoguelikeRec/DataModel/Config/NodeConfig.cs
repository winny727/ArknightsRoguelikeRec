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
    }
}
