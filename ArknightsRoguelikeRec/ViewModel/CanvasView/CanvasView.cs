using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Helper;
using ArknightsRoguelikeRec.ViewModel.DataStruct;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class CanvasView : IDisposable
    {
        private enum CanvasLayerType
        {
            BackgroundGrid,
            Connections,
            ConnectionPreview,
            Nodes,
            DeleteConnectionButtons,
            ButtonState,
        }

        private ICanvas mCanvas;
        private IMouseHandler mMouseHandler;
        private IOptionBuilder mOptionBuilder;
        private INodeConfigInitializer mNodeConfigInitializer;
        private bool mDisposed = false;

        public SaveData SaveData { get; private set; }
        public Layer CurrentLayer { get; private set; }
        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }

        public Size DefaultSize { get; set; } = new Size(800f, 600f);
        public Color BackgroundColor { get; set; } = Color.White;
        public Color GridColor { get; set; } = Color.LightGray;
        public Color NodeBorderColor { get; set; } = Color.Black;
        public Color NodeTitleBackgroundColor { get; set; } = Color.Gray;
        public Color NodeTitleTextColor { get; set; } = Color.Black;
        public Color NodeContentBackgroundColor { get; set; } = Color.White;
        public Color NodeContentTextColor { get; set; } = Color.Black;
        public Color ConnectionColor { get; set; } = Color.Black;
        public Color ConnectionPreviewColor { get; set; } = Color.Red;
        public Color ConnectionValidNodeColor { get; set; } = Color.Green;
        public Color ConnectionInvalidNodeColor { get; set; } = Color.Red;
        public Color DelBtnBorderColor { get; set; } = Color.Black;
        public Color DelBtnBackgroundColor { get; set; } = Color.LightGray;
        public Color DelBtnIconColor { get; set; } = Color.Red;

        public float GridStep { get; set; } = 20f;
        public float NodeGapHorizontal { get; set; } = 120f;
        public float NodeGapVertical { get; set; } = 50f;
        public float NodeWidth { get; set; } = 150f;
        public float NodeHeight { get; set; } = 80f;
        public float NodeTitleHeight { get; set; } = 40f;
        public float DelBtnWidth { get; set; } = 20f;
        public float DelBtnHeight { get; set; } = 20f;

        private readonly List<NodeView> mNodeViews = new List<NodeView>();
        private readonly Dictionary<CanvasLayerType, List<ButtonView>> mButtonViews = new Dictionary<CanvasLayerType, List<ButtonView>>();

        private readonly Dictionary<CanvasLayerType, ICanvasLayer> mCanvasLayers = new Dictionary<CanvasLayerType, ICanvasLayer>();

        private NodeView mConnectionNodeView = null;
        public bool IsConnecting => mConnectionNodeView != null;

        private bool mIsEditMode = false;
        public bool IsEditMode
        {
            get
            {
                return mIsEditMode;
            }
            set
            {
                if (mIsEditMode == value) return;
                mIsEditMode = value;
                ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.ButtonState);
                canvasLayer?.Clear();
                OnConnectionEnd(null);
                UpdateDelConnectionBtns();
                ApplyCanvas();
            }
        }


        public CanvasView(ICanvas canvas, IMouseHandler mouseHandler, IOptionBuilder optionBuilder, INodeConfigInitializer nodeConfigInitializer = null)
        {
            mCanvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            mMouseHandler = mouseHandler ?? throw new ArgumentNullException(nameof(mouseHandler));
            mOptionBuilder = optionBuilder ?? throw new ArgumentNullException(nameof(optionBuilder));
            mNodeConfigInitializer = nodeConfigInitializer;

            mouseHandler.MouseUp += OnMouseUp;
        }

        public void InitCanvas(SaveData saveData, Layer layer)
        {
            if (mDisposed) return;
            ClearCanvas();

            SaveData = saveData;
            CurrentLayer = layer;

            if (layer == null || layer.Nodes == null || layer.Nodes.Count == 0)
            {
                CanvasWidth = DefaultSize.Width;
                CanvasHeight = DefaultSize.Height;
                mCanvas.InitCanvas(CanvasWidth, CanvasHeight, BackgroundColor);
                mCanvasLayers[CanvasLayerType.BackgroundGrid] = mCanvas.NewCanvasLayer();
                return;
            }

            int columnCount = layer.Nodes.Count;

            // 横向尺寸
            float width =
                NodeGapHorizontal +                                   // 左边距
                columnCount * NodeWidth +
                (columnCount - 1) * NodeGapHorizontal +
                NodeGapHorizontal;                                    // 右边距

            float height = DefaultSize.Height;

            CanvasWidth = width;
            CanvasHeight = height;
            mCanvas.InitCanvas(CanvasWidth, CanvasHeight, BackgroundColor);

            foreach (CanvasLayerType type in Enum.GetValues(typeof(CanvasLayerType)))
            {
                ICanvasLayer canvasLayer = mCanvas.NewCanvasLayer();
                mCanvasLayers[type] = canvasLayer;
            }
        }

        public void UpdateCanvas()
        {
            if (mDisposed) return;
            InitCanvas(SaveData, CurrentLayer);

            UpdateBackgroundGrid();
            UpdateNodes();
            UpdateConnections();
            UpdateDelConnectionBtns();

            ApplyCanvas();
        }

        public void ApplyCanvas()
        {
            //using CodeTimer codeTimer = new CodeTimer("ApplyCanvas");
            mCanvas.ApplyCanvas();
        }

        public void ClearCanvas()
        {
            if (mDisposed) return;
            DisposeButtonViews();
            mCanvas.ClearCanvas();
            mNodeViews.Clear();
            mCanvasLayers.Clear();
            CurrentLayer = null;
            mConnectionNodeView = null;
        }

        private ICanvasLayer GetCanvasLayer(CanvasLayerType type)
        {
            if (mDisposed) return null;
            if (mCanvasLayers.TryGetValue(type, out var canvasLayer))
            {
                return canvasLayer;
            }
            return null;
        }

        public void UpdateBackgroundGrid()
        {
            if (mDisposed) return;

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.BackgroundGrid);
            if (canvasLayer == null)
            {
                return;
            }
            canvasLayer.Clear();

            float step = GridStep;
            for (float x = 0; x <= CanvasWidth; x += step)
            {
                Point p1 = new Point(x, 0);
                Point p2 = new Point(x, CanvasHeight);
                float width = (x / step) % 5 == 0 ? 2f : 1f;
                canvasLayer.DrawLine(p1, p2, GridColor, width);
            }

            for (float y = 0; y <= CanvasHeight; y += step)
            {
                Point p1 = new Point(0, y);
                Point p2 = new Point(CanvasWidth, y);
                float width = (y / step) % 5 == 0 ? 2f : 1f;
                canvasLayer.DrawLine(p1, p2, GridColor, width);
            }
        }

        public void UpdateNodes()
        {
            if (mDisposed) return;
            Layer layer = CurrentLayer;
            if (layer == null || layer.Nodes == null)
            {
                return;
            }

            DisposeButtonViews(CanvasLayerType.Nodes);

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.Nodes);
            canvasLayer?.Clear();

            int colCount = layer.Nodes.Count;
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
                List<Node> colNodes = layer.Nodes[colIndex];
                if (colNodes == null)
                {
                    continue;
                }
                int rowCount = colNodes.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    Node node = colNodes[rowIndex];
                    if (node == null)
                    {
                        continue;
                    }
                    NodeView nodeView = DrawNode(node, colIndex, rowIndex, rowCount);
                    if (nodeView != null)
                    {
                        mNodeViews.Add(nodeView);
                    }
                }
            }
        }

        private NodeView DrawNode(Node node, int colIndex, int rowIndex, int rowCount)
        {
            if (mDisposed) return null;

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.Nodes);
            if (canvasLayer == null)
            {
                return null;
            }

            float hGap = NodeGapHorizontal;
            float vGap = NodeGapVertical;
            float width = NodeWidth;
            float height = NodeHeight;

            //初始化节点
            float borderWidth = 2f;
            float nodeX = hGap + colIndex * (width + hGap);
            float nodeY = CanvasHeight / 2 - (rowCount * height + (rowCount - 1) * vGap) / 2 + rowIndex * (height + vGap);
            Rect rect = new Rect(nodeX - borderWidth / 2, nodeY - borderWidth / 2, width + borderWidth, height + borderWidth);

            NodeView nodeView = new NodeView(node, rect, colIndex, rowIndex);
            mNodeConfigInitializer?.InitializeNodeConfig(CurrentLayer, nodeView);

            Color borderColor = nodeView.NodeConfig?.NodeColor ?? NodeBorderColor;
            Color titleBgColor = nodeView.NodeConfig?.NodeColor ?? NodeTitleBackgroundColor;
            Color titleTextColor = nodeView.NodeConfig?.TextColor ?? NodeTitleTextColor;
            Color contentBgColor = NodeContentBackgroundColor;
            Color contentTextColor = NodeContentTextColor;

            canvasLayer.DrawRectangle(rect, borderColor, borderWidth);

            float titleHeight = NodeTitleHeight;

            //初始化节点类型选择按钮
            Rect rectTitle = new Rect(nodeX, nodeY, width, titleHeight);
            ButtonView btnTitle = RegButton(CanvasLayerType.Nodes, rectTitle, titleBgColor, node.Data.Type, titleTextColor);
            btnTitle.Click += (button) =>
            {
                if (IsConnecting || IsEditMode)
                {
                    return;
                }

                ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.ButtonState);
                canvasLayer?.Clear();
                mOptionBuilder.ShowTypeMenu(SaveData, CurrentLayer, nodeView);
            };

            //初始化节点次级类型选择按钮
            Rect rectContent = new Rect(nodeX, nodeY + titleHeight, width, titleHeight);
            ButtonView btnContent = RegButton(CanvasLayerType.Nodes, rectContent, contentBgColor, node.Data.SubType, contentTextColor);
            btnContent.Click += (button) =>
            {
                if (IsConnecting || IsEditMode)
                {
                    return;
                }

                ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.ButtonState);
                canvasLayer?.Clear();
                mOptionBuilder.ShowSubTypeMenu(SaveData, CurrentLayer, nodeView);
            };

            ButtonView buttonView = new ButtonView(rect, mMouseHandler);
            buttonView.Click += (button) =>
            {
                if (IsConnecting)
                {
                    OnConnectionEnd(nodeView);
                }
                else if (IsEditMode)
                {
                    mConnectionNodeView = nodeView;
                }
            };

            AddButtonView(CanvasLayerType.Nodes, buttonView);

            return nodeView;
        }

        public void UpdateConnections()
        {
            if (mDisposed) return;

            Layer layer = CurrentLayer;
            if (layer == null || layer.Connections == null)
            {
                return;
            }

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.Connections);
            canvasLayer?.Clear();

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                NodeView nodeView1 = GetNodeViewByIdx(connection.Idx1);
                NodeView nodeView2 = GetNodeViewByIdx(connection.Idx2);
                DrawConnection(nodeView1, nodeView2);
            }
        }

        private void DrawConnection(NodeView nodeView1, NodeView nodeView2)
        {
            if (mDisposed) return;

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.Connections);
            if (canvasLayer == null)
            {
                return;
            }

            float x1 = nodeView1.Rect.X + nodeView1.Rect.Width / 2;
            float y1 = nodeView1.Rect.Y + nodeView1.Rect.Height / 2;
            float x2 = nodeView2.Rect.X + nodeView2.Rect.Width / 2;
            float y2 = nodeView2.Rect.Y + nodeView2.Rect.Height / 2;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);

            canvasLayer.DrawBezier(pt1, pt2, pt3, pt4, ConnectionColor, 2f);
        }

        public void UpdateConnectionPreview()
        {
            if (mDisposed) return;

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.ConnectionPreview);
            if (canvasLayer == null)
            {
                return;
            }
            canvasLayer.Clear();

            NodeView nodeView = mConnectionNodeView;
            Point mousePoint = mMouseHandler.GetMousePoint();
            float borderWidth = 4f;

            if (nodeView == null)
            {
                foreach (var targetNodeView in mNodeViews)
                {
                    if (targetNodeView.Rect.Contains(mousePoint))
                    {
                        Rect targetRect = targetNodeView.Rect;
                        Rect targetNodeHighlightRect = new Rect(targetRect.X - borderWidth / 2, targetRect.Y - borderWidth / 2, targetRect.Width + borderWidth, targetRect.Height + borderWidth);
                        canvasLayer.DrawRectangle(targetNodeHighlightRect, ConnectionValidNodeColor, borderWidth);
                        break;
                    }
                }
                return;
            }

            float x1 = nodeView.Rect.X + nodeView.Rect.Width / 2;
            float y1 = nodeView.Rect.Y + nodeView.Rect.Height / 2;
            float x2 = mousePoint.X;
            float y2 = mousePoint.Y;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);

            canvasLayer.DrawBezier(pt1, pt2, pt3, pt4, ConnectionPreviewColor, 2f);

            Rect rect = nodeView.Rect;
            Rect nodeHighlightRect = new Rect(rect.X - borderWidth / 2, rect.Y - borderWidth / 2, rect.Width + borderWidth, rect.Height + borderWidth);
            canvasLayer.DrawRectangle(nodeHighlightRect, ConnectionValidNodeColor, borderWidth);

            foreach (var otherNodeView in mNodeViews)
            {
                if (otherNodeView.Rect.Contains(mousePoint) && otherNodeView != nodeView)
                {
                    bool isValid = DataAPI.CheckConnectionValid(CurrentLayer, nodeView, otherNodeView);
                    Color color = isValid ? ConnectionValidNodeColor : ConnectionInvalidNodeColor;
                    Rect otherRect = otherNodeView.Rect;
                    Rect otherNodeHighlightRect = new Rect(otherRect.X - borderWidth / 2, otherRect.Y - borderWidth / 2, otherRect.Width + borderWidth, otherRect.Height + borderWidth);
                    canvasLayer.DrawRectangle(otherNodeHighlightRect, color, borderWidth);
                    break;
                }
            }
        }

        private void UpdateDelConnectionBtns()
        {
            if (mDisposed) return;

            DisposeButtonViews(CanvasLayerType.DeleteConnectionButtons);

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.DeleteConnectionButtons);
            canvasLayer?.Clear();

            if (!mIsEditMode) return;

            Layer layer = CurrentLayer;
            if (layer == null || layer.Connections == null)
            {
                return;
            }

            foreach (var connection in layer.Connections)
            {
                DrawDelConnectionBtn(connection);
            }
        }

        private void DrawDelConnectionBtn(Connection connection)
        {
            if (connection == null)
            {
                return;
            }

            NodeView nodeView1 = GetNodeViewByIdx(connection.Idx1);
            NodeView nodeView2 = GetNodeViewByIdx(connection.Idx2);

            if (nodeView1 == null || nodeView2 == null)
            {
                return;
            }

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.DeleteConnectionButtons);
            if (canvasLayer == null)
            {
                return;
            }

            float borderWidth = 2f;
            float width = DelBtnWidth;
            float height = DelBtnHeight;

            float x1 = nodeView1.Rect.X + nodeView1.Rect.Width / 2;
            float y1 = nodeView1.Rect.Y + nodeView1.Rect.Height / 2;
            float x2 = nodeView2.Rect.X + nodeView2.Rect.Width / 2;
            float y2 = nodeView2.Rect.Y + nodeView2.Rect.Height / 2;

            float btnX = x1 + (x2 - x1) / 2 - width / 2;
            float btnY = y1 + (y2 - y1) / 2 - height / 2;

            Rect rect = new Rect(btnX - borderWidth / 2, btnY - borderWidth / 2, width + borderWidth, height + borderWidth);
            canvasLayer.DrawRectangle(rect, DelBtnBorderColor, borderWidth);

            Rect rectDel = new Rect(btnX, btnY, width, height);
            ButtonView btnDel = RegButton(CanvasLayerType.DeleteConnectionButtons, rectDel, DelBtnBackgroundColor, "X", DelBtnIconColor, 2f);
            btnDel.Click += (button) =>
            {
                ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.ButtonState);
                canvasLayer?.Clear();

                DataAPI.RemoveConnection(CurrentLayer, connection);
                UpdateConnections();
                UpdateDelConnectionBtns();
                ApplyCanvas();
            };
        }

        private NodeView GetNodeViewByIdx(int nodeIdx)
        {
            if (nodeIdx < 0 || nodeIdx >= mNodeViews.Count)
            {
                return null;
            }
            return mNodeViews[nodeIdx];
        }

        private ButtonView RegButton(CanvasLayerType type, Rect rect, Color color, string text = null, Color? textColor = null, float textScale = 1f)
        {
            if (mDisposed) return null;

            ButtonView buttonView = new ButtonView(rect, mMouseHandler);
            AddButtonView(type, buttonView);

            textColor ??= Color.Black;

            float textWidth = rect.Width * textScale;
            float textHeight = rect.Height * textScale;

            Rect textRect = new Rect(rect.X - (textWidth - rect.Width) / 2, 
                                    rect.Y - (textHeight - rect.Height) / 2,
                                    textWidth,
                                    textHeight);

            ICanvasLayer buttonLayer = GetCanvasLayer(type);
            if (buttonLayer != null)
            {
                buttonLayer.FillRectangle(rect, color);
                buttonLayer.DrawString(text, textRect, textColor);
            }

            ICanvasLayer buttonStateLayer = GetCanvasLayer(CanvasLayerType.ButtonState);
            if (buttonStateLayer == null)
            {
                return buttonView;
            }

            // 注册按钮状态变化事件
            buttonView.PointerEnter += () =>
            {
                if (buttonView.IsPressed)
                {
                    buttonStateLayer.FillRectangle(rect, ButtonColorHelper.GetPressedColor(color));
                    buttonStateLayer.DrawString(text, textRect, ButtonColorHelper.GetPressedColor(textColor.Value));
                }
                else
                {
                    buttonStateLayer.FillRectangle(rect, ButtonColorHelper.GetHoverColor(color));
                    buttonStateLayer.DrawString(text, textRect, ButtonColorHelper.GetHoverColor(textColor.Value));
                }
                ApplyCanvas();
            };
            buttonView.PointerExit += () =>
            {
                buttonStateLayer.FillRectangle(rect, color);
                buttonStateLayer.DrawString(text, textRect, textColor.Value);
                ApplyCanvas();
            };
            buttonView.MouseDown += (button) =>
            {
                buttonStateLayer.FillRectangle(rect, ButtonColorHelper.GetPressedColor(color));
                buttonStateLayer.DrawString(text, textRect, ButtonColorHelper.GetPressedColor(textColor.Value));
                ApplyCanvas();
            };
            buttonView.MouseUp += (button) =>
            {
                if (buttonView.IsPressed)
                {
                    // 还有别的鼠标按键还按着，不改变状态
                    return;
                }

                if (buttonView.IsHover)
                {
                    buttonStateLayer.FillRectangle(rect, ButtonColorHelper.GetHoverColor(color));
                    buttonStateLayer.DrawString(text, textRect, ButtonColorHelper.GetHoverColor(textColor.Value));
                }
                else
                {
                    buttonStateLayer.FillRectangle(rect, color);
                    buttonStateLayer.DrawString(text, textRect, textColor.Value);
                }
                ApplyCanvas();
            };

            return buttonView;
        }

        private void AddButtonView(CanvasLayerType type, ButtonView buttonView)
        {
            if (!mButtonViews.TryGetValue(type, out var buttonViewList))
            {
                buttonViewList = new List<ButtonView>();
                mButtonViews[type] = buttonViewList;
            }

            buttonViewList.Add(buttonView);
        }

        private void DisposeButtonViews(CanvasLayerType type)
        {
            if (mDisposed) return;
            if (mButtonViews.TryGetValue(type, out var buttonViewList))
            {
                foreach (var buttonView in buttonViewList)
                {
                    buttonView.Dispose();
                }
                buttonViewList.Clear();
            }
        }

        private void DisposeButtonViews()
        {
            if (mDisposed) return;
            foreach (var item in mButtonViews)
            {
                var canvasLayerType = item.Key;
                DisposeButtonViews(item.Key);
            }
            mButtonViews.Clear();
        }

        private void OnConnectionEnd(NodeView nodeView)
        {
            if (mDisposed) return;

            if (!IsConnecting)
            {
                return;
            }

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.ConnectionPreview);
            canvasLayer?.Clear();

            if (nodeView != null)
            {
                if (DataAPI.CheckConnectionValid(CurrentLayer, nodeView, mConnectionNodeView) && 
                    DataAPI.AddConnection(CurrentLayer, nodeView.Node, mConnectionNodeView.Node))
                {
                    DrawConnection(nodeView, mConnectionNodeView);
                    UpdateDelConnectionBtns();
                }
            }

            mConnectionNodeView = null;
            ApplyCanvas();
        }

        private void OnMouseUp(Point point, MouseButton button)
        {
            if (mDisposed) return;

            foreach (var item in mButtonViews)
            {
                var buttonViewList = item.Value;
                foreach (var buttonView in buttonViewList)
                {
                    if (buttonView.Rect.Contains(point))
                    {
                        return;
                    }
                }
            }

            // 点击到空白区域
            if (IsConnecting)
            {
                OnConnectionEnd(null);
            }
        }

        public void Tick()
        {
            if (mDisposed) return;

            // 在外部驱动Tick
            if (IsEditMode)
            {
                UpdateConnectionPreview();
                ApplyCanvas();
            }
        }

        public virtual void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            if (mMouseHandler != null)
            {
                mMouseHandler.MouseUp -= OnMouseUp;
            }

            mDisposed = true;
            mCanvas?.Dispose();
            mMouseHandler?.Dispose();
            mCanvas = null;
            mMouseHandler = null;
            mOptionBuilder = null;
            mNodeConfigInitializer = null;
        }
    }
}
