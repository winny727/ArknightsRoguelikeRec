using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ArknightsRoguelikeRec.Helper
{
    public static class UIHelper
    {
        /// <summary>
        /// 添加层数按钮
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="layerName"></param>
        /// <param name="onClick"></param>
        public static Button CreateLayerBtn(Panel panel, string layerName, Action onClick = null)
        {
            int gap = GlobalDefine.LAYER_BTN_GAP;
            int height = GlobalDefine.LAYER_BTN_HEIGHT;

            Button btnLayer = new Button();
            panel.Controls.Add(btnLayer);
            btnLayer.Text = layerName;
            //btnLayer.Font = GlobalDefine.TEXT_FONT;

            btnLayer.Size = new Size(panel.Width - 2 * gap, height);
            btnLayer.Location = new Point(gap, gap + (panel.Controls.Count - 1) * height);

            btnLayer.Click += (sender, e) => onClick?.Invoke();

            return btnLayer;
        }

        public static void AddSeparator(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Items.Add(new ToolStripSeparator());
            }
        }

        public static void AddSeparatedMenuItem(ContextMenuStrip contextMenuStrip, string name, Action onAction)
        {
            AddSeparator(contextMenuStrip);
            contextMenuStrip.Items.Add(name, null, (_sender, _e) =>
            {
                onAction?.Invoke();
            });
        }

        public static void ShowMenu(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Show(Cursor.Position);
            }
        }
    }
}