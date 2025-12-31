using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class NodeView
    {
        public Node Node { get; private set; }
        public int ColIndex { get; private set; }
        public int RowIndex { get; private set; }
        public Rect Rect { get; set; }

        public NodeConfig NodeConfig { get; set; }


        public NodeView(Node node, Rect rect, int colIndex, int rowIndex)
        {
            Node = node;
            Rect = rect;
            ColIndex = colIndex;
            RowIndex = rowIndex;
        }
    }
}
