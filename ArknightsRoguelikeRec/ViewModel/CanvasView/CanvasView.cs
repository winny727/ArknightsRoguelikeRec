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
        public Color ConnectionColor { get; set; } = Color.Gray;

        public float GridStep { get; set; } = 20f;
        public float NodeGapHorizontal { get; set; } = 120f;
        public float NodeGapVertical { get; set; } = 50f;
        public float NodeWidth { get; set; } = 150f;
        public float NodeHeight { get; set; } = 80f;
        public float NodeTitleHeight { get; set; } = 40f;

        private readonly List<NodeView> mNodeViews = new List<NodeView>();
        private readonly List<ButtonView> mButtonViews = new List<ButtonView>();

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

            foreach (CanvasLayerType layerType in Enum.GetValues(typeof(CanvasLayerType)))
            {
                ICanvasLayer canvasLayer = mCanvas.NewCanvasLayer();
                mCanvasLayers[layerType] = canvasLayer;
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

        private ICanvasLayer GetCanvasLayer(CanvasLayerType layerType)
        {
            if (mDisposed) return null;
            if (mCanvasLayers.TryGetValue(layerType, out var canvasLayer))
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

            DisposeButtonViews();

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
                        mNodeConfigInitializer?.InitializeNodeConfig(layer, nodeView);
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
            float nodeX = hGap + colIndex * (width + hGap);
            float nodeY = CanvasHeight / 2 - (rowCount * height + (rowCount - 1) * vGap) / 2 + rowIndex * (height + vGap);
            Rect rect = new Rect(nodeX - 1f, nodeY - 1f, width + 2f, height + 2f);
            canvasLayer.DrawRectangle(rect, NodeBorderColor, 2f);

            NodeView nodeView = new NodeView(node, rect, colIndex, rowIndex);

            float titleHeight = NodeTitleHeight;

            //初始化节点类型选择按钮
            Rect rectTitle = new Rect(nodeX, nodeY, width, titleHeight);
            ButtonView btnTitle = RegButton(CanvasLayerType.Nodes, rectTitle, NodeTitleBackgroundColor, node.Type, NodeTitleTextColor);
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
            ButtonView btnContent = RegButton(CanvasLayerType.Nodes, rectContent, NodeContentBackgroundColor, node.SubType, NodeTitleTextColor);
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
            mButtonViews.Add(buttonView);
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
            if (nodeView == null)
            {
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

            canvasLayer.DrawBezier(pt1, pt2, pt3, pt4, ConnectionColor, 2f);
        }

        private void UpdateDelConnectionBtns()
        {
            if (mDisposed) return;
            if (!mIsEditMode) return;

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.DeleteConnectionButtons);
            if (canvasLayer == null)
            {
                return;
            }

            //TODO: 实现删除连接按钮
            //TODO 切换读取文件时GC耗时？

            canvasLayer.Clear();
        }

        private NodeView GetNodeViewByIdx(int nodeIdx)
        {
            if (nodeIdx < 0 || nodeIdx >= mNodeViews.Count)
            {
                return null;
            }
            return mNodeViews[nodeIdx];
        }

        private ButtonView RegButton(CanvasLayerType buttonLayerType, Rect rect, Color color, string text = null, Color? textColor = null)
        {
            if (mDisposed) return null;

            ButtonView buttonView = new ButtonView(rect, mMouseHandler);
            mButtonViews.Add(buttonView);

            textColor ??= Color.Black;

            ICanvasLayer buttonLayer = GetCanvasLayer(buttonLayerType);
            if (buttonLayer != null)
            {
                buttonLayer.FillRectangle(rect, color);
                buttonLayer.DrawString(text, rect, textColor);
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
                    buttonStateLayer.DrawString(text, rect, ButtonColorHelper.GetPressedColor(textColor.Value));
                }
                else
                {
                    buttonStateLayer.FillRectangle(rect, ButtonColorHelper.GetHoverColor(color));
                    buttonStateLayer.DrawString(text, rect, ButtonColorHelper.GetHoverColor(textColor.Value));
                }
                ApplyCanvas();
            };
            buttonView.PointerExit += () =>
            {
                buttonStateLayer.FillRectangle(rect, color);
                buttonStateLayer.DrawString(text, rect, textColor.Value);
                ApplyCanvas();
            };
            buttonView.MouseDown += (button) =>
            {
                buttonStateLayer.FillRectangle(rect, ButtonColorHelper.GetPressedColor(color));
                buttonStateLayer.DrawString(text, rect, ButtonColorHelper.GetPressedColor(textColor.Value));
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
                    buttonStateLayer.DrawString(text, rect, ButtonColorHelper.GetHoverColor(textColor.Value));
                }
                else
                {
                    buttonStateLayer.FillRectangle(rect, color);
                    buttonStateLayer.DrawString(text, rect, textColor.Value);
                }
                ApplyCanvas();
            };

            return buttonView;
        }

        private void DisposeButtonViews()
        {
            if (mDisposed) return;
            foreach (var buttonView in mButtonViews)
            {
                buttonView.Dispose();
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
                }
            }

            mConnectionNodeView = null;
            ApplyCanvas();
        }

        private void OnMouseUp(Point point, MouseButton button)
        {
            if (mDisposed) return;

            foreach (var buttonView in mButtonViews)
            {
                if (buttonView.Rect.Contains(point))
                {
                    return;
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
            if (IsConnecting)
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
