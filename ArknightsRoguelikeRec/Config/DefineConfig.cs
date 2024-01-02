using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.Helper;

namespace ArknightsRoguelikeRec.Config
{
    public static class DefineConfig
    {
        public static MapList<int, LayerConfig> LayerConfigDict = new MapList<int, LayerConfig>();
        public static MapList<int, NodeConfig> NodeConfigDict = new MapList<int, NodeConfig>();

        public static void InitConfig()
        {
            LayerConfigDict.Clear();
            NodeConfigDict.Clear();
            InitLayerDefine();
            InitNodeDefine();
        }

        private static void InitLayerDefine()
        {
            string path = Environment.CurrentDirectory + "/Settings/LayerDefine.txt";
            TableReader tableReader = ConfigHelper.LoadConfig(path);

            if (tableReader == null)
            {
                return;
            }

            tableReader.ForEach((line) =>
            {
                int layerID = line.GetValue<int>("layerID");
                string layerName = line.GetValue("layerName");
                string nodeTypes = line.GetValue("nodeTypes");
                List<int> nodeTypesList = ConfigHelper.ParseList<int>(nodeTypes);

                LayerConfig layerConfig = new LayerConfig()
                {
                    ID = layerID,
                    Name = layerName,
                    NodeTypes = nodeTypesList,
                };

                if (!LayerConfigDict.ContainsKey(layerID))
                {
                    LayerConfigDict.Add(layerID, layerConfig);
                }
            });
        }


        private static void InitNodeDefine()
        {
            string path = Environment.CurrentDirectory + "/Settings/NodeDefine.txt";
            TableReader tableReader = ConfigHelper.LoadConfig(path);

            if (tableReader == null)
            {
                return;
            }

            tableReader.ForEach((line) =>
            {
                int nodeID = line.GetValue<int>("nodeID");
                string nodeType = line.GetValue("nodeType");
                string subTypes = line.GetValue("subTypes");
                int extraLayer = line.GetValue<int>("extraLayer");
                List<string> subTypesList = ConfigHelper.ParseList<string>(subTypes);

                NodeConfig nodeConfig = new NodeConfig()
                {
                    ID = nodeID,
                    Type = nodeType,
                    SubTypes = subTypesList,
                    ExtraLayer = extraLayer,
                };

                if (!NodeConfigDict.ContainsKey(nodeID))
                {
                    NodeConfigDict.Add(nodeID, nodeConfig);
                }
            });
        }
    }
}
