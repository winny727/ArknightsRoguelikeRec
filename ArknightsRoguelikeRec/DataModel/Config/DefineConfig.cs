using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.Config
{
    public static class DefineConfig
    {
        public static MapList<int, LayerConfig> LayerConfigDict = new MapList<int, LayerConfig>();
        public static MapList<int, NodeConfig> NodeConfigDict = new MapList<int, NodeConfig>();
    }
}
