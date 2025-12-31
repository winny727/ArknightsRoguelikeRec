using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.Config
{
    [Serializable]
    public class LayerConfig
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<string> LayerTypes { get; set; }
        public List<int> NodeTypes { get; set; }
    }
}
