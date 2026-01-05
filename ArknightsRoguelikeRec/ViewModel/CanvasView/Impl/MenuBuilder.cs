using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Helper;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public class MenuBuilder : IOptionBuilder
    {
        private readonly InputForm mInputForm;
        private readonly Action mRefreshCallback;

        public MenuBuilder(InputForm inputForm, Action refreshCallback)
        {
            mInputForm = inputForm;
            mRefreshCallback = refreshCallback;
        }

        public void ShowTypeMenu(SaveData saveData, Layer layer, NodeView nodeView)
        {
            if (nodeView == null)
            {
                return;
            }

            Node node = nodeView.Node;
            LayerConfig layerConfig = ConfigHelper.GetLayerConfigByName(layer.Name);

            //选择节点类型
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            if (layerConfig?.NodeTypes != null)
            {
                for (int i = 0; i < layerConfig.NodeTypes.Count; i++)
                {
                    int nodeID = layerConfig.NodeTypes[i];
                    if (nodeID == 0)
                    {
                        AddSeparator(contextMenuStrip);
                        continue;
                    }

                    NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                    if (nodeConfig == null)
                    {
                        continue;
                    }

                    contextMenuStrip.Items.Add(nodeConfig.Type, null, (sender, e) =>
                    {
                        if (node.Type != nodeConfig.Type)
                        {
                            node.SubType = string.Empty;
                        }

                        node.Type = nodeConfig.Type;
                        nodeView.NodeConfig = nodeConfig;
                        mRefreshCallback?.Invoke();
                    });
                }
            }

            //显示清除选项
            if (!string.IsNullOrEmpty(node.Type) || !string.IsNullOrEmpty(node.SubType))
            {
                AddSeparatedMenuItem(contextMenuStrip, "清除", () =>
                {
                    node.Type = string.Empty;
                    nodeView.NodeConfig = null;

                    node.SubType = string.Empty;
                    mRefreshCallback?.Invoke();
                });
            }

            if (mInputForm != null)
            {
                //显示备注选项
                AddSeparatedMenuItem(contextMenuStrip, "节点备注", () =>
                {
                    mInputForm.Title = "节点备注";
                    mInputForm.Content = node.Comment;
                    if (mInputForm.ShowDialog() == DialogResult.OK)
                    {
                        node.Comment = mInputForm.Content;
                        mRefreshCallback?.Invoke();
                    }
                });
            }

            ShowMenu(contextMenuStrip);
        }

        public void ShowSubTypeMenu(SaveData saveData, Layer layer, NodeView nodeView)
        {
            if (nodeView == null)
            {
                return;
            }

            Node node = nodeView.Node;
            NodeConfig nodeConfig = nodeView.NodeConfig;

            //选择节点次级类型
            if (nodeConfig != null)
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                for (int i = 0; i < nodeConfig.SubTypes.Count; i++)
                {
                    string subType = nodeConfig.SubTypes[i];

                    if (string.IsNullOrEmpty(subType))
                    {
                        AddSeparator(contextMenuStrip);
                        continue;
                    }

                    contextMenuStrip.Items.Add(subType, null, (_sender, _e) =>
                    {
                        node.SubType = subType;
                        mRefreshCallback?.Invoke();
                    });
                }

                //树洞层类型
                if (nodeConfig.ExtraLayer > 0)
                {
                    for (int i = 0; i < saveData.Layers.Count; i++)
                    {
                        Layer curLayer = saveData.Layers[i];
                        if (curLayer == layer)
                        {
                            continue;
                        }

                        LayerConfig curLayerConfig = ConfigHelper.GetLayerConfigByName(curLayer.Name);
                        if (curLayerConfig != null && curLayerConfig.ID == nodeConfig.ExtraLayer)
                        {
                            contextMenuStrip.Items.Add(curLayer.CustomName, null, (sender, e) =>
                            {
                                node.SubType = curLayer.CustomName;
                                mRefreshCallback?.Invoke();
                            });
                        }
                    }
                }

                //显示清除选项
                if (!string.IsNullOrEmpty(node.SubType))
                {
                    AddSeparatedMenuItem(contextMenuStrip, "清除", () =>
                    {
                        node.SubType = string.Empty;
                        mRefreshCallback?.Invoke();
                    });
                }

                //显示备注选项
                if (mInputForm != null)
                {
                    AddSeparatedMenuItem(contextMenuStrip, "节点备注", () =>
                    {
                        mInputForm.Title = "节点备注";
                        mInputForm.Content = node.Comment;
                        if (mInputForm.ShowDialog() == DialogResult.OK)
                        {
                            node.Comment = mInputForm.Content;
                            mRefreshCallback?.Invoke();
                        }
                    });
                }

                //显示菜单
                ShowMenu(contextMenuStrip);
            }
        }

        private void AddSeparator(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Items.Add(new ToolStripSeparator());
            }
        }

        private void AddSeparatedMenuItem(ContextMenuStrip contextMenuStrip, string name, Action onAction)
        {
            AddSeparator(contextMenuStrip);
            contextMenuStrip.Items.Add(name, null, (_sender, _e) =>
            {
                onAction?.Invoke();
            });
        }

        private void ShowMenu(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }
    }
}