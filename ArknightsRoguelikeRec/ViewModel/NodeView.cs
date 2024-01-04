using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.DataModel;
using System.Windows.Forms;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class NodeView
    {
        public Node Node { get; private set; }
        public Control View { get; private set; }
        public int ColIndex { get; private set; }
        public int RowIndex { get; private set; }

        public NodeView(Node node, int colIndex, int rowIndex, Control view)
        {
            Node = node;
            View = view;
            ColIndex = colIndex;
            RowIndex = rowIndex;
        }
    }
}
