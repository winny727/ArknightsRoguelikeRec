using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.Helper;
using ArknightsRoguelikeRec.DataStruct;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel
{
    public enum EditModeType
    {
        None,
        Connections,
        Routes,
    }

    public class CanvasView : IDisposable
    {
        private enum CanvasLayerType
        {
            BackgroundGrid,
            Routes,
            Connections,
            EditPreview,
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
        public event Action OnApplyCanvas;

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
        public Color EditValidNodeColor { get; set; } = Color.Green;
        public Color EditInvalidNodeColor { get; set; } = Color.Red;
        public Color DelBtnBorderColor { get; set; } = Color.Black;
        public Color DelBtnBackgroundColor { get; set; } = Color.LightGray;
        public Color DelBtnIconColor { get; set; } = Color.Red;
        public Color RouteColor { get; set; } = Color.Blue;

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

        private EditModeType mEditMode = EditModeType.None;
        public EditModeType EditMode
        {
            get
            {
                return mEditMode;
            }
            set
            {
                if (mEditMode == value) return;
                mEditMode = value;
                GetCanvasLayer(CanvasLayerType.ButtonState)?.Clear();
                GetCanvasLayer(CanvasLayerType.EditPreview)?.Clear();
                OnConnectionEnd(null);
                UpdateDelConnectionBtns();
                UpdateRoutes();
                ApplyCanvas();
            }
        }
        public bool IsEditing => EditMode != EditModeType.None;

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
            UpdateRoutes();

            ApplyCanvas();
        }

        public void ApplyCanvas()
        {
            if (mDisposed) return;
            //using CodeTimer codeTimer = new CodeTimer("ApplyCanvas");
            mCanvas.ApplyCanvas();
            OnApplyCanvas?.Invoke();
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
            GetCanvasLayer(CanvasLayerType.Nodes)?.Clear();

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
                if (IsConnecting || EditMode != EditModeType.None)
                {
                    return;
                }

                GetCanvasLayer(CanvasLayerType.ButtonState)?.Clear();
                mOptionBuilder.ShowTypeMenu(SaveData, CurrentLayer, nodeView);
            };

            //初始化节点次级类型选择按钮
            Rect rectContent = new Rect(nodeX, nodeY + titleHeight, width, titleHeight);
            ButtonView btnContent = RegButton(CanvasLayerType.Nodes, rectContent, contentBgColor, node.Data.SubType, contentTextColor);
            btnContent.Click += (button) =>
            {
                if (IsConnecting || IsEditing)
                {
                    return;
                }

                GetCanvasLayer(CanvasLayerType.ButtonState)?.Clear();
                mOptionBuilder.ShowSubTypeMenu(SaveData, CurrentLayer, nodeView);
            };

            ButtonView buttonView = new ButtonView(rect, mMouseHandler);
            buttonView.Click += (button) =>
            {
                if (IsConnecting)
                {
                    OnConnectionEnd(nodeView);
                }
                else if (EditMode == EditModeType.Connections)
                {
                    mConnectionNodeView = nodeView;
                }
                else if (EditMode == EditModeType.Routes)
                {
                    Layer layer = CurrentLayer;
                    if (layer == null || layer.Routes == null)
                    {
                        return;
                    }
                    int nodeIdx = DataAPI.GetNodeIdxByNode(layer, node);
                    if (nodeIdx >= 0)
                    {
                        int routeIndex = layer.Routes.IndexOf(nodeIdx);
                        if (routeIndex == -1)
                        {
                            if (DataAPI.CheckRouteValid(layer, node))
                            {
                                layer.Routes.Add(nodeIdx);
                                SaveData.IsDirty = true;
                                UpdateRoutes();
                                ApplyCanvas();
                            }
                        }
                        else
                        {
                            // 移除该节点及其之后的路线
                            for (int i = layer.Routes.Count - 1; i >= routeIndex; i--)
                            {
                                layer.Routes.RemoveAt(i);
                            }
                            SaveData.IsDirty = true;
                            UpdateRoutes();
                            ApplyCanvas();
                        }
                    }
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

            GetCanvasLayer(CanvasLayerType.Connections)?.Clear();

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
            DrawBezier(canvasLayer, x1, y1, x2, y2, ConnectionColor, 2f);
        }

        public void UpdateEditPreview()
        {
            if (mDisposed) return;

            Layer layer = CurrentLayer;
            if (layer == null)
            {
                return;
            }

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.EditPreview);
            if (canvasLayer == null)
            {
                return;
            }
            canvasLayer.Clear();

            if (EditMode == EditModeType.Connections)
            {
                NodeView nodeView = mConnectionNodeView;
                Point mousePoint = mMouseHandler.GetMousePoint();
                float borderWidth = 4f;

                if (nodeView == null)
                {
                    // 绘制节点高亮
                    foreach (var targetNodeView in mNodeViews)
                    {
                        if (targetNodeView.Rect.Contains(mousePoint))
                        {
                            Rect targetRect = targetNodeView.Rect;
                            Rect targetNodeHighlightRect = new Rect(targetRect.X - borderWidth / 2, targetRect.Y - borderWidth / 2, targetRect.Width + borderWidth, targetRect.Height + borderWidth);
                            canvasLayer.DrawRectangle(targetNodeHighlightRect, EditValidNodeColor, borderWidth);
                            break;
                        }
                    }
                    return;
                }

                // 绘制连接预览线
                float x1 = nodeView.Rect.X + nodeView.Rect.Width / 2;
                float y1 = nodeView.Rect.Y + nodeView.Rect.Height / 2;
                float x2 = mousePoint.X;
                float y2 = mousePoint.Y;
                DrawBezier(canvasLayer, x1, y1, x2, y2, ConnectionPreviewColor, 2f);

                // 绘制已选择节点高亮
                Rect rect = nodeView.Rect;
                Rect nodeHighlightRect = new Rect(rect.X - borderWidth / 2, rect.Y - borderWidth / 2, rect.Width + borderWidth, rect.Height + borderWidth);
                canvasLayer.DrawRectangle(nodeHighlightRect, EditValidNodeColor, borderWidth);

                // 绘制是否可连接的节点高亮
                foreach (var otherNodeView in mNodeViews)
                {
                    if (otherNodeView.Rect.Contains(mousePoint) && otherNodeView != nodeView)
                    {
                        bool isValid = DataAPI.CheckConnectionValid(layer, nodeView.Node, otherNodeView.Node);
                        Color color = isValid ? EditValidNodeColor : EditInvalidNodeColor;
                        Rect otherRect = otherNodeView.Rect;
                        Rect otherNodeHighlightRect = new Rect(otherRect.X - borderWidth / 2, otherRect.Y - borderWidth / 2, otherRect.Width + borderWidth, otherRect.Height + borderWidth);
                        canvasLayer.DrawRectangle(otherNodeHighlightRect, color, borderWidth);
                        break;
                    }
                }
            }
            else if (EditMode == EditModeType.Routes)
            {
                Point mousePoint = mMouseHandler.GetMousePoint();
                float borderWidth = 4f;

                // 绘制是否可连接路线节点高亮
                foreach (var targetNodeView in mNodeViews)
                {
                    if (targetNodeView.Rect.Contains(mousePoint))
                    {
                        bool isValid = DataAPI.CheckRouteValid(layer, targetNodeView.Node);
                        Color color = isValid ? EditValidNodeColor : EditInvalidNodeColor;
                        Rect targetRect = targetNodeView.Rect;
                        Rect targetNodeHighlightRect = new Rect(targetRect.X - borderWidth / 2, targetRect.Y - borderWidth / 2, targetRect.Width + borderWidth, targetRect.Height + borderWidth);
                        canvasLayer.DrawRectangle(targetNodeHighlightRect, color, borderWidth);
                        break;
                    }
                }

                // 绘制预览连接线
                if (layer.Nodes != null && layer.Routes != null && layer.Routes.Count > 0)
                {
                    int lastIdx = layer.Routes[layer.Routes.Count - 1];
                    NodeView lastNodeView = GetNodeViewByIdx(lastIdx);
                    int colIndex = lastNodeView.ColIndex;
                    bool hasRoute = false;
                    if (colIndex == layer.Nodes.Count - 1)
                    {
                        // 检测若在最后一列且没有可连接的路线，就不显示预览连接线
                        List<Node> colNodes = layer.Nodes[colIndex];
                        foreach (var node in colNodes)
                        {
                            if (node == lastNodeView.Node)
                            {
                                continue;
                            }
                            int nodeIdx = DataAPI.GetNodeIdxByNode(layer, node);
                            if (!layer.Routes.Contains(nodeIdx) && DataAPI.CheckRouteValid(layer, node))
                            {
                                hasRoute = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        hasRoute = true;
                    }
                    if (lastNodeView != null && hasRoute)
                    {
                        float x1 = lastNodeView.Rect.X + lastNodeView.Rect.Width / 2;
                        float y1 = lastNodeView.Rect.Y + lastNodeView.Rect.Height / 2;
                        float x2 = mousePoint.X;
                        float y2 = mousePoint.Y;
                        DrawBezier(canvasLayer, x1, y1, x2, y2, RouteColor, 2f);
                    }
                }
            }
        }

        public void UpdateDelConnectionBtns()
        {
            if (mDisposed) return;

            DisposeButtonViews(CanvasLayerType.DeleteConnectionButtons);
            GetCanvasLayer(CanvasLayerType.DeleteConnectionButtons)?.Clear();

            if (EditMode != EditModeType.Connections) return;

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
            if (mDisposed) return;

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
                GetCanvasLayer(CanvasLayerType.ButtonState)?.Clear();
                DataAPI.RemoveConnection(CurrentLayer, connection);
                SaveData.IsDirty = true;
                UpdateConnections();
                UpdateDelConnectionBtns();
                ApplyCanvas();
            };
        }

        public void UpdateRoutes()
        {
            if (mDisposed) return;

            GetCanvasLayer(CanvasLayerType.Routes)?.Clear();

            if (EditMode != EditModeType.Routes) return;

            Layer layer = CurrentLayer;
            if (layer == null || layer.Routes == null)
            {
                return;
            }

            if (layer.Routes.Count > 0)
            {
                for (int i = 0; i < layer.Routes.Count - 1; i++)
                {
                    int nodeIdx1 = layer.Routes[i];
                    int nodeIdx2 = layer.Routes[i + 1];
                    DrawRoutes(nodeIdx1, nodeIdx2);
                }
                int lastIdx = layer.Routes[layer.Routes.Count - 1];
                DrawRoutes(lastIdx, -1);
            }
        }

        private void DrawRoutes(int nodeIdx1, int nodeIdx2)
        {
            if (mDisposed) return;

            NodeView nodeView1 = GetNodeViewByIdx(nodeIdx1);
            NodeView nodeView2 = GetNodeViewByIdx(nodeIdx2);

            ICanvasLayer canvasLayer = GetCanvasLayer(CanvasLayerType.Routes);
            if (canvasLayer == null)
            {
                return;
            }

            float borderWidth = 4f;
            if (nodeView1 != null)
            {
                Rect rect = nodeView1.Rect;
                Rect nodeHighlightRect = new Rect(rect.X - borderWidth / 2, rect.Y - borderWidth / 2, rect.Width + borderWidth, rect.Height + borderWidth);
                canvasLayer.DrawRectangle(nodeHighlightRect, RouteColor, borderWidth);

                if (nodeView2 != null)
                {
                    float x1 = nodeView1.Rect.X + nodeView1.Rect.Width / 2;
                    float y1 = nodeView1.Rect.Y + nodeView1.Rect.Height / 2;
                    float x2 = nodeView2.Rect.X + nodeView2.Rect.Width / 2;
                    float y2 = nodeView2.Rect.Y + nodeView2.Rect.Height / 2;
                    DrawBezier(canvasLayer, x1, y1, x2, y2, RouteColor, borderWidth);
                }
            }
        }

        private NodeView GetNodeViewByIdx(int nodeIdx)
        {
            if (nodeIdx < 0 || nodeIdx >= mNodeViews.Count)
            {
                return null;
            }
            return mNodeViews[nodeIdx];
        }

        private void DrawBezier(ICanvasLayer canvasLayer, float x1, float y1, float x2, float y2, Color color, float width)
        {
            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);
            canvasLayer.DrawBezier(pt1, pt2, pt3, pt4, color, width);
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
            if (mDisposed) return;
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
                DisposeButtonViews(canvasLayerType);
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

            GetCanvasLayer(CanvasLayerType.EditPreview)?.Clear();

            if (nodeView != null)
            {
                if (DataAPI.CheckConnectionValid(CurrentLayer, nodeView.Node, mConnectionNodeView.Node) && 
                    DataAPI.AddConnection(CurrentLayer, nodeView.Node, mConnectionNodeView.Node))
                {
                    SaveData.IsDirty = true;
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
            if (EditMode != EditModeType.None)
            {
                UpdateEditPreview();
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
