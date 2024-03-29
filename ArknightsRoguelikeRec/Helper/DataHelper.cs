﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;

namespace ArknightsRoguelikeRec.Helper
{
    public static class DataHelper
    {
        public static SaveData CreateData(string userName, string dataID)
        {
            SaveData saveData = new SaveData()
            {
                UserName = userName,
                DataID = dataID,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Version = GlobalDefine.VERSION,
            };

            //预设，自动添加一层到五层
            var layerList = DefineConfig.LayerConfigDict.AsList();
            for (int i = 0; i < GlobalDefine.PRESET_LAYER_COUNT; i++)
            {
                AddLayer(saveData, layerList[i].Name);
            }

            return saveData;
        }

        public static SaveData LoadData(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (!File.Exists(path))
            {
                return null;
            }

            try
            {
                string fileText = File.ReadAllText(path);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(fileText);
                return saveData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static string SaveData(SaveData saveData, string path, Formatting formatting = Formatting.Indented)
        {
            if (saveData == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            saveData.UpdateTime = DateTime.Now;
            saveData.Version = GlobalDefine.VERSION;
            string fileText = JsonConvert.SerializeObject(saveData, formatting);
            string fileName = $"{saveData.UserName}_data{saveData.DataID}_{saveData.CreateTime:yyyyMMddHHmmss}.json";
            string fullPath = Path.Combine(path, fileName);
            File.WriteAllText(fullPath, fileText);
            return fullPath;
        }

        public static void AddLayer(SaveData saveData, string layerName)
        {
            if (saveData == null)
            {
                return;
            }

            string customName = layerName;
            int index = 0;
            while (saveData.Layers.Exists(t => t.CustomName == customName))
            {
                customName = $"{layerName}({++index})";
            }

            Layer layer = new Layer()
            {
                Name = layerName,
                CustomName = customName,
            };
            saveData.Layers.Add(layer);
        }

        public static void AddColume(Layer layer)
        {
            if (layer == null)
            {
                return;
            }

            layer.Nodes.Add(new List<Node>());
        }

        public static void AddNode(Layer layer, int colIndex)
        {
            if (layer == null)
            {
                return;
            }

            if (colIndex >= 0 && colIndex < layer.Nodes.Count)
            {
                layer.Nodes[colIndex].Add(new Node());
            }
        }

        private static int IndexOfNode(Layer layer, Node node)
        {
            int index = -1;
            for (int i = 0; i < layer.Nodes.Count; i++)
            {
                for (int j = 0; j < layer.Nodes[i].Count; j++)
                {
                    index++;
                    if (node == layer.Nodes[i][j])
                    {
                        return index;
                    }
                }
            }
            return -1;
        }

        public static void AddConnection(Layer layer, Node node1, Node node2)
        {
            if (layer == null)
            {
                return;
            }

            int index1 = IndexOfNode(layer, node1);
            int index2 = IndexOfNode(layer, node2);

            if (index1 == index2)
            {
                return;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                if ((connection.Idx1 == index1 && connection.Idx2 == index2) || (connection.Idx1 == index2 && connection.Idx2 == index1))
                {
                    return;
                }
            }

            layer.Connections.Add(new Connection()
            {
                Idx1 = index1,
                Idx2 = index2,
            });
        }

        public static void RemoveConnection(Layer layer, Connection connection)
        {
            if (layer == null || connection == null)
            {
                return;
            }

            layer.Connections.Remove(connection);
        }
    }
}
