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
            if (saveData == null || layer == null || nodeView == null)
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

                    NodeConfig nodeConfig = GlobalDefine.NodeConfigDict[nodeID];
                    if (nodeConfig == null)
                    {
                        continue;
                    }

                    AddMenuItem(contextMenuStrip, nodeConfig.Type, () =>
                    {
                        if (node.Data.Type != nodeConfig.Type)
                        {
                            node.Data.SubType = string.Empty;
                        }

                        node.Data.Type = nodeConfig.Type;
                        nodeView.NodeConfig = nodeConfig;
                        saveData.IsDirty = true;
                        mRefreshCallback?.Invoke();
                    });
                }
            }

            //显示清除选项
            if (!string.IsNullOrEmpty(node.Data.Type) || !string.IsNullOrEmpty(node.Data.SubType))
            {
                AddSeparatedMenuItem(contextMenuStrip, "清除", () =>
                {
                    node.Data.Type = string.Empty;
                    node.Data.SubType = string.Empty;
                    nodeView.NodeConfig = null;
                    saveData.IsDirty = true;
                    mRefreshCallback?.Invoke();
                });
            }

            if (mInputForm != null)
            {
                //显示备注选项
                AddSeparatedMenuItem(contextMenuStrip, "节点备注", () =>
                {
                    mInputForm.Title = "节点备注";
                    mInputForm.Content = node.Data.Comment;
                    if (mInputForm.ShowDialog() == DialogResult.OK)
                    {
                        node.Data.Comment = mInputForm.Content;
                        saveData.IsDirty = true;
                        mRefreshCallback?.Invoke();
                    }
                });
            }

            ShowMenu(contextMenuStrip);
        }

        public void ShowSubTypeMenu(SaveData saveData, Layer layer, NodeView nodeView)
        {
            if (saveData == null || layer == null || nodeView == null)
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

                    AddMenuItem(contextMenuStrip, subType, () =>
                    {
                        node.Data.SubType = subType;
                        saveData.IsDirty = true;
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
                                node.Data.SubType = curLayer.CustomName;
                                saveData.IsDirty = true;
                                mRefreshCallback?.Invoke();
                            });
                        }
                    }
                }

                //显示清除选项
                if (!string.IsNullOrEmpty(node.Data.SubType))
                {
                    AddSeparatedMenuItem(contextMenuStrip, "清除", () =>
                    {
                        node.Data.SubType = string.Empty;
                        saveData.IsDirty = true;
                        mRefreshCallback?.Invoke();
                    });
                }

                //显示备注选项
                if (mInputForm != null)
                {
                    AddSeparatedMenuItem(contextMenuStrip, "节点备注", () =>
                    {
                        mInputForm.Title = "节点备注";
                        mInputForm.Content = node.Data.Comment;
                        if (mInputForm.ShowDialog() == DialogResult.OK)
                        {
                            node.Data.Comment = mInputForm.Content;
                            saveData.IsDirty = true;
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

        private void AddMenuItem(ContextMenuStrip contextMenuStrip, string name, Action onAction)
        {
            contextMenuStrip.Items.Add(name, null, (_sender, _e) =>
            {
                onAction?.Invoke();
            });
        }

        private void AddSeparatedMenuItem(ContextMenuStrip contextMenuStrip, string name, Action onAction)
        {
            AddSeparator(contextMenuStrip);
            AddMenuItem(contextMenuStrip, name, onAction);
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