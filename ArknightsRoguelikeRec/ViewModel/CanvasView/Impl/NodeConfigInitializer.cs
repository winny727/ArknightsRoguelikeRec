using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Helper;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class NodeConfigInitializer : INodeConfigInitializer
    {
        public void InitializeNodeConfig(Layer layer, NodeView nodeView)
        {
            if (layer == null)
            {
                throw new ArgumentNullException(nameof(layer));
            }

            if (nodeView == null)
            {
                throw new ArgumentNullException(nameof(nodeView));
            }

            LayerConfig layerConfig = ConfigHelper.GetLayerConfigByName(layer.Name);
            if (layerConfig == null || layerConfig.NodeTypes == null)
            {
                return;
            }

            for (int i = 0; i < layerConfig.NodeTypes.Count; i++)
            {
                int nodeID = layerConfig.NodeTypes[i];
                NodeConfig nodeConfig = DefineConfig.NodeConfigDict[nodeID];
                if (nodeConfig != null && nodeConfig.Type == nodeView.Node.Type)
                {
                    nodeView.NodeConfig = nodeConfig;
                    break;
                }
            }
        }
    }
}