using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArknightsRoguelikeRec.Helper
{
    public static class DataAPI
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

        public static void RefreshInfo(this SaveData saveData)
        {
            if (saveData == null)
            {
                return;
            }

            saveData.UpdateTime = DateTime.Now;
            saveData.Version = GlobalDefine.VERSION;
        }

        public static string GenerateFileName(this SaveData saveData)
        {
            return $"{saveData.UserName}_data{saveData.DataID}_{saveData.CreateTime:yyyy-MM-dd_HH-mm-ss}";
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

        public static int IndexOfNode(Layer layer, Node node)
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

            int nodeIdx1 = IndexOfNode(layer, node1);
            int nodeIdx2 = IndexOfNode(layer, node2);

            if (nodeIdx1 == nodeIdx2)
            {
                return;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                if ((connection.Idx1 == nodeIdx1 && connection.Idx2 == nodeIdx2) || (connection.Idx1 == nodeIdx2 && connection.Idx2 == nodeIdx1))
                {
                    return;
                }
            }

            layer.Connections.Add(new Connection()
            {
                Idx1 = nodeIdx1,
                Idx2 = nodeIdx2,
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

        public static bool CheckConnectionValid(Layer layer, NodeView nodeView1, NodeView nodeView2)
        {
            if (nodeView1.ColIndex == nodeView2.ColIndex && nodeView1.RowIndex == nodeView2.RowIndex)
            {
                return false;
            }

            int nodeIdx1 = IndexOfNode(layer, nodeView1.Node);
            int nodeIdx2 = IndexOfNode(layer, nodeView2.Node);

            if (nodeIdx1 < 0 || nodeIdx2 < 0)
            {
                return false;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                if ((connection.Idx1 == nodeIdx1 && connection.Idx2 == nodeIdx2) || (connection.Idx1 == nodeIdx2 && connection.Idx2 == nodeIdx1))
                {
                    return false;
                }
            }

            int colDelta = Math.Abs(nodeView1.ColIndex - nodeView2.ColIndex);
            int rowDelta = Math.Abs(nodeView1.RowIndex - nodeView2.RowIndex);
            if ((colDelta == 0 && rowDelta > 1) || colDelta > 1)
            {
                return false;
            }

            return true;
        }
    }
}
