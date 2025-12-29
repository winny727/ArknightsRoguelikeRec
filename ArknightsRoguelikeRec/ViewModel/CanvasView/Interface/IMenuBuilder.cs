using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.DataModel;

namespace ArknightsRoguelikeRec.ViewModel
{
    public interface IMenuBuilder
    {
        void ShowTypeMenu(SaveData saveData, Layer layer, NodeView nodeView);
        void ShowSubTypeMenu(SaveData saveData, Layer layer, NodeView nodeView);
    }
}