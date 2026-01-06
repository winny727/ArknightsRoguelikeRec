using ArknightsRoguelikeRec.Config;
using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.DataStruct;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class NodeView
    {
        public Node Node { get; private set; }
        public int ColIndex { get; private set; }
        public int RowIndex { get; private set; }
        public Rect Rect { get; private set; }

        public NodeConfig NodeConfig { get; set; }
        public int RefreshIndex { get; set; } = -1; // TODO 更新后设置NodeConfig

        public NodeView(Node node, Rect rect, int colIndex, int rowIndex)
        {
            Node = node;
            Rect = rect;
            ColIndex = colIndex;
            RowIndex = rowIndex;
        }
    }
}
