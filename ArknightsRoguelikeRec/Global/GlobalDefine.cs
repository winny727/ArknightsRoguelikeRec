using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace ArknightsRoguelikeRec
{
    public static class GlobalDefine
    {
        public const int VERSION = 1; //更改存档文件格式后，需将 Version +1

        public const int PRESET_LAYER_COUNT = 5; //预设，自动添加层数

        public const int LAYER_BTN_GAP = 5;
        public const int LAYER_BTN_HEIGHT = 50;

        public const int MAX_COLUMU = 16; //最大列数
        public const int COLUMN_MIN_NODE = 1; //每列最少节点
        public const int COLUMN_MAX_NODE = 4; //每列最多节点

        public static readonly Font TEXT_FONT = new Font("宋体", 12.0f, FontStyle.Bold);

        public static readonly SKTypeface SK_TEXT_FONT =
            LoadEmbeddedFont("ArknightsRoguelikeRec.Assets.Fonts.HarmonyOS_Sans_SC_Regular.ttf");

        public static SKTypeface LoadEmbeddedFont(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Font resource not found: {resourceName}");

            return SKTypeface.FromStream(stream);
        }
    }
}
