using System;
using System.Collections.Generic;
using System.Drawing;

namespace ArknightsRoguelikeRec
{
    public static class GlobalDefine
    {
        public const int PRESET_LAYER_COUNT = 5;

        public const int LAYER_BTN_GAP = 5;
        public const int LAYER_BTN_HEIGHT = 50;

        public const int BACKGROUND_GRID_STEP = 20;

        public const int NODE_VIEW_H_GAP = 80;
        public const int NODE_VIEW_V_GAP = 60;
        public const int NODE_VIEW_WIDTH = 150;
        public const int NODE_VIEW_HEIGHT = 80;
        public const int NODE_VIEW_BTN_GAP = 2;
        public const int NODE_VIEW_SCROLL_GAP = 20;

        public const int CONNECTION_DELETE_BTN_SIZE = 25;

        public const int ROW_MIN_NODE = 1;
        public const int ROW_MAX_NODE = 4;

        public static readonly Font TEXT_FONT = new Font("黑体", 10.0f);
    }
}
