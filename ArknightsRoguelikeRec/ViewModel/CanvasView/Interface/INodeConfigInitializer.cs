using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.DataModel;

namespace ArknightsRoguelikeRec.ViewModel
{
    public interface INodeConfigInitializer
    {
        void InitializeNodeConfig(Layer layer, NodeView nodeView);
    }
}