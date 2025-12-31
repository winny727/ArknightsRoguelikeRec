using ArknightsRoguelikeRec.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsRoguelikeRec.Helper
{
    public static class ConfigHelper
    {
        public static void InitConfig()
        {
            DefineConfig.LayerConfigDict.Clear();
            DefineConfig.NodeConfigDict.Clear();
            InitLayerDefine();
            InitNodeDefine();
        }

        private static void InitLayerDefine()
        {
            string path = Environment.CurrentDirectory + "/Settings/LayerDefine.txt";
            TableReader tableReader = LoadConfig(path);

            if (tableReader == null)
            {
                return;
            }

            tableReader.ForEach((key, line) =>
            {
                int layerID = line.GetValue<int>("layerID");
                string layerName = line.GetValue("layerName");
                string layerTypes = line.GetValue("layerTypes");
                string nodeTypes = line.GetValue("nodeTypes");
                List<string> layerTypesList = ParseList<string>(layerTypes);
                List<int> nodeTypesList = ParseList<int>(nodeTypes);

                LayerConfig layerConfig = new LayerConfig()
                {
                    ID = layerID,
                    Name = layerName,
                    LayerTypes = layerTypesList,
                    NodeTypes = nodeTypesList,
                };

                DefineConfig.LayerConfigDict[layerID] = layerConfig;
            });
        }


        private static void InitNodeDefine()
        {
            string path = Environment.CurrentDirectory + "/Settings/NodeDefine.txt";
            TableReader tableReader = LoadConfig(path);

            if (tableReader == null)
            {
                return;
            }

            tableReader.ForEach((key, line) =>
            {
                int nodeID = line.GetValue<int>("nodeID");
                string nodeType = line.GetValue("nodeType");
                string subTypes = line.GetValue("subTypes");
                int extraLayer = line.GetValue<int>("extraLayer");
                List<string> subTypesList = ParseList<string>(subTypes);

                NodeConfig nodeConfig = new NodeConfig()
                {
                    ID = nodeID,
                    Type = nodeType,
                    SubTypes = subTypesList,
                    ExtraLayer = extraLayer,
                };

                DefineConfig.NodeConfigDict[nodeID] = nodeConfig;
            });
        }

        private static List<T> ParseList<T>(string text, char split = '|')
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(text))
            {
                return list;
            }

            string[] values = text.Split(split);
            for (int i = 0; i < values.Length; i++)
            {
                if (string.IsNullOrEmpty(values[i]))
                {
                    list.Add(default);
                    continue;
                }

                T value = values[i].ConvertTo<T>(default);
                list.Add(value);
            }

            return list;
        }

        public static TableReader LoadConfig(string path)
        {
            try
            {
                TableReader tableReader = new TableReader(path, Encoding.UTF8);
                return tableReader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static LayerConfig GetLayerConfigByName(string layerName)
        {
            var layerList = DefineConfig.LayerConfigDict.AsList();
            for (int i = 0; i < layerList.Count; i++)
            {
                if (layerList[i].Name == layerName)
                {
                    return layerList[i];
                }
            }
            return null;
        }
    }
}
