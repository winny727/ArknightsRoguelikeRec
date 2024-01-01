using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ArknightsRoguelikeRec.DataModel;

namespace ArknightsRoguelikeRec.Helper
{
    public static class DataHelper
    {
        public static Data CreateData(string userName, int dataID)
        {
            Data data = new Data()
            {
                UserName = userName,
                DataID = dataID,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            return data;
        }

        public static Data LoadData(string path)
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
                Data data = JsonConvert.DeserializeObject<Data>(fileText);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static void SaveData(Data data, string path)
        {
            if (data == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            data.UpdateTime = DateTime.Now;
            string fileText = JsonConvert.SerializeObject(data, Formatting.Indented);
            string fullPath = Path.Combine(path, data.UserName + "_" + data.DataID);
            File.WriteAllText(fullPath, fileText);
        }

        public static void AddLayer(Data data, string layerName)
        {
            if (data == null)
            {
                return;
            }

            Layer layer = new Layer()
            {
                Name = layerName,
            };
            data.Layers.Add(layer);
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

        public static void AddConnection(Layer layer, Node startNode, Node endNode)
        {
            if (layer == null)
            {
                return;
            }

            int startIndex = IndexOfNode(layer, startNode);
            int endIndex = IndexOfNode(layer, endNode);

            if (startIndex == endIndex)
            {
                return;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                if ((connection.Key == startIndex && connection.Value == endIndex) || (connection.Key == endIndex && connection.Value == startIndex))
                {
                    return;
                }
            }

            layer.Connections.Add(new KeyValuePair<int, int>(endIndex, startIndex));
        }
    }
}
