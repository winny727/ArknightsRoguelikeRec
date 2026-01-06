using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.ViewModel;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.Helper
{
    public static class DataAPI
    {
        public static void AddLayer(SaveData saveData, string layerName)
        {
            if (saveData == null || saveData.Layers == null)
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
            if (layer == null || layer.Nodes == null)
            {
                return;
            }

            layer.Nodes.Add(new List<Node>());
        }

        public static void AddNode(Layer layer, int colIndex)
        {
            if (layer == null || layer.Nodes == null)
            {
                return;
            }

            if (colIndex >= 0 && colIndex < layer.Nodes.Count)
            {
                layer.Nodes[colIndex].Add(new Node());
            }
        }

        public static int IdxOfNode(Layer layer, Node node)
        {
            if (layer == null || layer.Nodes == null || node == null)
            {
                return -1;
            }

            int idx = -1;
            for (int i = 0; i < layer.Nodes.Count; i++)
            {
                for (int j = 0; j < layer.Nodes[i].Count; j++)
                {
                    idx++;
                    if (node == layer.Nodes[i][j])
                    {
                        return idx;
                    }
                }
            }
            return -1;
        }

        public static bool AddConnection(Layer layer, Node node1, Node node2)
        {
            if (layer == null || layer.Nodes == null || layer.Connections == null)
            {
                return false;
            }

            if (node1 == null || node2 == null)
            {
                return false;
            }

            int nodeIdx1 = IdxOfNode(layer, node1);
            int nodeIdx2 = IdxOfNode(layer, node2);

            if (nodeIdx1 == nodeIdx2)
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

            layer.Connections.Add(new Connection()
            {
                Idx1 = nodeIdx1,
                Idx2 = nodeIdx2,
            });

            return true;
        }

        public static void RemoveConnection(Layer layer, Connection connection)
        {
            if (layer == null || layer.Connections == null || connection == null)
            {
                return;
            }

            layer.Connections.Remove(connection);
        }

        public static bool CheckConnectionValid(Layer layer, NodeView nodeView1, NodeView nodeView2)
        {
            if (layer == null || layer.Nodes == null || layer.Connections == null || nodeView1 == null || nodeView2 == null)
            {
                return false;
            }

            if (nodeView1 == null || nodeView2 == null || nodeView1.Node == null || nodeView2.Node == null)
            {
                return false;
            }

            if (nodeView1.ColIndex == nodeView2.ColIndex && nodeView1.RowIndex == nodeView2.RowIndex)
            {
                return false;
            }

            int nodeIdx1 = IdxOfNode(layer, nodeView1.Node);
            int nodeIdx2 = IdxOfNode(layer, nodeView2.Node);

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

        public static bool CheckLayerValid(Layer layer, out string errMsg)
        {
            if (layer == null || layer.Nodes == null || layer.Connections == null)
            {
                errMsg = "参数无效";
                return false;
            }

            int totalNodeCount = 0;
            int colCount = layer.Nodes.Count;
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
                totalNodeCount += layer.Nodes[colIndex].Count;
            }

            var nodeInfos = new (int colIndex, int rowIndex)[totalNodeCount];
            HashSet<int> tempConnectedNodes = new HashSet<int>();

            (int, int) GetNodeInfoByIdx(int idx)
            {
                if (idx < 0 || idx >= nodeInfos.Length)
                {
                    return (-1, -1);
                }
                return nodeInfos[idx];
            }

            // 按Idx顺序排列所有节点
            int idx = -1;
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
                var colNodes = layer.Nodes[colIndex];
                for (int rowIndex = 0; rowIndex < colNodes.Count; rowIndex++)
                {
                    idx++;
                    nodeInfos[idx] = (colIndex, rowIndex);
                }
            }

            // 检测每个节点都至少有一个左连接和右连接
            for (int i = 0; i < nodeInfos.Length; i++)
            {
                (int colIndex, int _) = nodeInfos[i];
                tempConnectedNodes.Clear();

                for (int j = 0; j < layer.Connections.Count; j++)
                {
                    Connection connection = layer.Connections[j];
                    int idx1 = connection.Idx1;
                    int idx2 = connection.Idx2;
                    if (i == idx1)
                    {
                        if (!tempConnectedNodes.Contains(idx2))
                        {
                            tempConnectedNodes.Add(idx2);
                        }
                    }
                    else if (i == idx2)
                    {
                        if (!tempConnectedNodes.Contains(idx1))
                        {
                            tempConnectedNodes.Add(idx1);
                        }
                    }
                }

                bool hasLeftConnection = false;
                bool hasRightConnection = false;
                foreach (int connectedNodeIdx in tempConnectedNodes)
                {
                    var (connectedColIndex, connectedRowIndex) = GetNodeInfoByIdx(connectedNodeIdx);
                    if (connectedColIndex == -1 || connectedRowIndex == -1)
                    {
                        errMsg = "连接索引无效";
                        return false;
                    }
                    if (connectedColIndex == colIndex - 1)
                    {
                        hasLeftConnection = true;
                    }
                    else if (connectedColIndex == colIndex + 1)
                    {
                        hasRightConnection = true;
                    }
                }

                if ((colIndex > 0 && !hasLeftConnection) || (colIndex < colCount - 1 && !hasRightConnection))
                {
                    errMsg = "存在节点缺失左/右连接";
                    return false;
                }
            }

            // 检测连接不交叉
            for (int i = 0; i < layer.Connections.Count - 1; i++)
            {
                for (int j = i + 1; j < layer.Connections.Count; j++)
                {
                    Connection connection1 = layer.Connections[i];
                    Connection connection2 = layer.Connections[j];
                    var (colIndex1A, rowIndex1A) = GetNodeInfoByIdx(connection1.Idx1);
                    var (colIndex2A, rowIndex2A) = GetNodeInfoByIdx(connection1.Idx2);
                    var (colIndex1B, rowIndex1B) = GetNodeInfoByIdx(connection2.Idx1);
                    var (colIndex2B, rowIndex2B) = GetNodeInfoByIdx(connection2.Idx2);
                    if (colIndex1A == -1 || rowIndex1A == -1 ||
                        colIndex2A == -1 || rowIndex2A == -1 ||
                        colIndex1B == -1 || rowIndex1B == -1 ||
                        colIndex2B == -1 || rowIndex2B == -1)
                    {
                        errMsg = "连接索引无效";
                        return false;
                    }

                    // 规范化 A 连接方向（左 -> 右）
                    if (colIndex1A > colIndex2A)
                    {
                        (colIndex1A, colIndex2A) = (colIndex2A, colIndex1A);
                        (rowIndex1A, rowIndex2A) = (rowIndex2A, rowIndex1A);
                    }

                    // 规范化 B 连接方向（左 -> 右）
                    if (colIndex1B > colIndex2B)
                    {
                        (colIndex1B, colIndex2B) = (colIndex2B, colIndex1B);
                        (rowIndex1B, rowIndex2B) = (rowIndex2B, rowIndex1B);
                    }

                    // 相隔一列
                    int colDelta = colIndex2A - colIndex1A;
                    if (colDelta == 1 && colIndex1A == colIndex1B && colIndex2A == colIndex2B)
                    {
                        // 交叉
                        if ((rowIndex1A < rowIndex1B && rowIndex2A > rowIndex2B) ||
                            (rowIndex1A > rowIndex1B && rowIndex2A < rowIndex2B))
                        {
                            errMsg = "存在交叉连接";
                            return false;
                        }
                    }
                }
            }

            errMsg = string.Empty;
            return true;
        }
    }
}
