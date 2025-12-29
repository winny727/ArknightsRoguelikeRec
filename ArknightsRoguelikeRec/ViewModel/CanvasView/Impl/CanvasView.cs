using ArknightsRoguelikeRec.DataModel;
using ArknightsRoguelikeRec.ViewModel.DataStruct;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class CanvasView : IDisposable
    {
        protected ICanvas mCanvas;
        protected IMouseHandler mMouseHandler;
        protected IMenuBuilder mMenuBuilder;
        protected INodeConfigInitializer mNodeConfigInitializer;
        protected bool mDisposed = false;

        public SaveData SaveData { get; protected set; }
        public Layer CurrentLayer { get; protected set; }
        public float CanvasWidth { get; protected set; }
        public float CanvasHeight { get; protected set; }

        public Color BackgroundColor { get; set; } = Color.White;
        public Color GridColor { get; set; } = Color.LightGray;
        public Color NodeTitleBackgroundColor { get; set; } = Color.Gray;
        public Color NodeTitleTextColor { get; set; } = Color.Black;
        public Color NodeContentBackgroundColor { get; set; } = Color.White;
        public Color NodeContentTextColor { get; set; } = Color.Black;
        public Color ConnectionColor { get; set; } = Color.Red;

        public float GridStep { get; set; } = 20f;
        public float NodeGapHorizontal { get; set; } = 120f;
        public float NodeGapVertical { get; set; } = 50f;
        public float NodeWidth { get; set; } = 150f;
        public float NodeHeight { get; set; } = 80f;
        public float NodeTitleHeight { get; set; } = 40f;
        public float NodePortWidth { get; set; } = 10f;
        public float NodePortHeight { get; set; } = 10f;

        protected List<NodeView> mNodeViews = new List<NodeView>();

        private readonly Dictionary<Rect, List<Action<Point, MouseButton>>> mMouseDownCallbacks = new Dictionary<Rect, List<Action<Point, MouseButton>>>();
        private readonly Dictionary<Rect, List<Action<Point, MouseButton>>> mMouseUpCallbacks = new Dictionary<Rect, List<Action<Point, MouseButton>>>();
        private readonly List<Rect> mMouseHoverRects = new List<Rect>();
        private readonly Dictionary<Rect, List<Action<Point>>> mMouseClickCallbacks = new Dictionary<Rect, List<Action<Point>>>();
        private readonly Dictionary<Rect, List<Action<Point>>> mMouseEnterCallbacks = new Dictionary<Rect, List<Action<Point>>>();
        private readonly Dictionary<Rect, List<Action<Point>>> mMouseExitCallbacks = new Dictionary<Rect, List<Action<Point>>>();

        public CanvasView(ICanvas canvas, IMouseHandler mouseHandler, IMenuBuilder menuBuilder, INodeConfigInitializer nodeConfigInitializer = null)
        {
            mCanvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            mMouseHandler = mouseHandler ?? throw new ArgumentNullException(nameof(mouseHandler));
            mMenuBuilder = menuBuilder ?? throw new ArgumentNullException(nameof(menuBuilder));
            mNodeConfigInitializer = nodeConfigInitializer;

            mMouseHandler.MouseMove += OnMouseMove;
            mMouseHandler.MouseDown += OnMouseDown;
            mMouseHandler.MouseUp += OnMouseUp;
            mMouseHandler.MouseClick += OnMouseClick;
        }

        public virtual void InitCanvas(SaveData saveData, Layer layer)
        {
            EnsureNotDisposed();
            ClearCanvas();

            SaveData = saveData;
            CurrentLayer = layer;

            Size defaultSize = mCanvas.GetCanvasDefaultSize();
            if (layer == null || layer.Nodes == null || layer.Nodes.Count == 0)
            {
                CanvasWidth = defaultSize.Width;
                CanvasHeight = defaultSize.Height;
                mCanvas.InitCanvas(CanvasWidth, CanvasHeight, BackgroundColor);
                return;
            }

            int columnCount = layer.Nodes.Count;

            // 横向尺寸
            float width =
                NodeGapHorizontal +                                   // 左边距
                columnCount * NodeWidth +
                (columnCount - 1) * NodeGapHorizontal +
                NodeGapHorizontal;                                    // 右边距

            float height = defaultSize.Height;

            CanvasWidth = width;
            CanvasHeight = height;
            mCanvas.InitCanvas(CanvasWidth, CanvasHeight, BackgroundColor);
        }

        public virtual void RefreshCanvas()
        {
            EnsureNotDisposed();
            InitCanvas(SaveData, CurrentLayer);

            DrawBackgroundGrid();
            DrawNodes();
            DrawConnections();

            mCanvas.RefreshCanvas();
        }

        public virtual void ClearCanvas()
        {
            EnsureNotDisposed();
            ClearMouseEvent();
            mCanvas.ClearCanvas();
            mNodeViews.Clear();
            CurrentLayer = null;
        }

        protected virtual void DrawBackgroundGrid()
        {
            EnsureNotDisposed();
            mCanvas.SetDrawStatic();

            float step = GridStep;
            for (float x = 0; x <= CanvasWidth; x += step)
            {
                Point p1 = new Point(x, 0);
                Point p2 = new Point(x, CanvasHeight);
                float width = (x / step) % 5 == 0 ? 2f : 1f;
                mCanvas.DrawLine(p1, p2, GridColor, width);
            }

            for (float y = 0; y <= CanvasHeight; y += step)
            {
                Point p1 = new Point(0, y);
                Point p2 = new Point(CanvasWidth, y);
                float width = (y / step) % 5 == 0 ? 2f : 1f;
                mCanvas.DrawLine(p1, p2, GridColor, width);
            }
        }

        protected virtual void DrawNodes()
        {
            EnsureNotDisposed();
            Layer layer = CurrentLayer;
            if (layer == null || layer.Nodes == null)
            {
                return;
            }

            mCanvas.SetDrawStatic();
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
                    mNodeConfigInitializer?.InitializeNodeConfig(layer, nodeView);
                    DrawPort(nodeView, colCount, rowCount);
                    mNodeViews.Add(nodeView);
                }
            }
        }

        protected virtual NodeView DrawNode(Node node, int colIndex, int rowIndex, int rowCount)
        {
            EnsureNotDisposed();
            mCanvas.SetDrawStatic();

            float hGap = NodeGapHorizontal;
            float vGap = NodeGapVertical;
            float width = NodeWidth;
            float height = NodeHeight;

            //初始化节点
            float nodeX = hGap + colIndex * (width + hGap);
            float nodeY = CanvasHeight / 2 - (rowCount * height + (rowCount - 1) * vGap) / 2 + rowIndex * (height + vGap);
            Rect rect = new Rect(nodeX, nodeY, width, height);
            mCanvas.DrawRectangle(rect, NodeTitleBackgroundColor, 2f);

            float titleHeight = NodeTitleHeight;

            //初始化节点类型选择按钮
            Rect rectTitle = new Rect(nodeX, nodeY, width, titleHeight);
            mCanvas.FillRectangle(rectTitle, NodeTitleBackgroundColor);
            mCanvas.DrawString(node.Type, rectTitle, NodeTitleTextColor);

            //初始化节点次级类型选择按钮
            Rect rectContent = new Rect(nodeX, nodeY + titleHeight, width, titleHeight);
            mCanvas.FillRectangle(rectContent, NodeContentBackgroundColor);
            mCanvas.DrawString(node.SubType, rectContent, NodeContentTextColor);

            NodeView nodeView = new NodeView(node, rect, colIndex, rowIndex);

            RegMouseClickEvent(rectTitle, (point) =>
            {
                mMenuBuilder.ShowTypeMenu(SaveData, CurrentLayer, nodeView);
            });

            RegMouseClickEvent(rectContent, (point) =>
            {
                mMenuBuilder.ShowSubTypeMenu(SaveData, CurrentLayer, nodeView);
            });

            return nodeView;
        }

        protected virtual void DrawConnections()
        {
            EnsureNotDisposed();
            mCanvas.SetDrawStatic();

            Layer layer = CurrentLayer;
            if (layer == null || layer.Connections == null)
            {
                return;
            }

            for (int i = 0; i < layer.Connections.Count; i++)
            {
                var connection = layer.Connections[i];
                NodeView nodeView1 = GetNodeViewByIdx(connection.Idx1);
                NodeView nodeView2 = GetNodeViewByIdx(connection.Idx2);
                DrawConnection(nodeView1, nodeView2);
            }
        }

        protected virtual void DrawPort(NodeView nodeView, int colCount, int rowCount)
        {
            EnsureNotDisposed();
            mCanvas.SetDrawStatic();

            float width = NodePortWidth;
            float height = NodePortHeight;
            Rect nodeRect = nodeView.Rect;

            if (nodeView.ColIndex > 0)
            {
                Rect leftRect = new Rect(nodeRect.X - width,
                                            nodeRect.Y + nodeRect.Height / 2 - height / 2,
                                            width,
                                            height);
                mCanvas.FillRectangle(leftRect, NodeTitleBackgroundColor);
            }

            if (nodeView.ColIndex < colCount - 1)
            {
                Rect rightRect = new Rect(nodeRect.X + nodeRect.Width,
                                        nodeRect.Y + nodeRect.Height / 2 - height / 2,
                                        width,
                                        height);
                mCanvas.FillRectangle(rightRect, NodeTitleBackgroundColor);
            }

            if (nodeView.RowIndex > 0)
            {
                Rect topRect = new Rect(nodeRect.X + nodeRect.Width / 2 - width / 2,
                                        nodeRect.Y - height,
                                        width,
                                        height);
                mCanvas.FillRectangle(topRect, NodeTitleBackgroundColor);
            }

            if (nodeView.RowIndex < rowCount - 1)
            {
                Rect bottomRect = new Rect(nodeRect.X + nodeRect.Width / 2 - width / 2,
                                        nodeRect.Y + nodeRect.Height,
                                        width,
                                        height);
                mCanvas.FillRectangle(bottomRect, NodeTitleBackgroundColor);
            }
        }

        protected virtual void DrawConnection(NodeView nodeView1, NodeView nodeView2)
        {
            EnsureNotDisposed();
            mCanvas.SetDrawStatic();

            NodeView nodeViewLT; // left top
            NodeView nodeViewRB; // right bottom

            if (nodeView1.ColIndex < nodeView2.ColIndex ||
                (nodeView1.ColIndex == nodeView2.ColIndex && nodeView1.RowIndex < nodeView2.RowIndex))
            {
                nodeViewLT = nodeView1;
                nodeViewRB = nodeView2;
            }
            else
            {
                nodeViewLT = nodeView2;
                nodeViewRB = nodeView1;
            }

            if (nodeView1.ColIndex == nodeView2.ColIndex)
            {
                Point start = new Point(nodeViewLT.Rect.X + nodeViewLT.Rect.Width / 2, nodeViewLT.Rect.Y + nodeViewLT.Rect.Height);
                Point end = new Point(nodeViewRB.Rect.X + nodeViewRB.Rect.Width / 2, nodeViewRB.Rect.Y);
                mCanvas.DrawLine(start, end, ConnectionColor, 2f);
            }
            else
            {
                float offset = NodeWidth / 2 + NodePortWidth;
                float x1 = nodeViewLT.Rect.X + nodeViewLT.Rect.Width / 2 + offset;
                float y1 = nodeViewLT.Rect.Y + nodeViewLT.Rect.Height / 2;
                float x2 = nodeViewRB.Rect.X + nodeViewRB.Rect.Width / 2 - offset;
                float y2 = nodeViewRB.Rect.Y + nodeViewRB.Rect.Height / 2;

                Point pt1 = new Point(x1, y1);
                Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
                Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
                Point pt4 = new Point(x2, y2);

                mCanvas.DrawBezier(pt1, pt2, pt3, pt4, ConnectionColor, 2f);
            }
        }

        protected virtual void DrawConnectionPreview(NodeView nodeView, Point locaction)
        {
            EnsureNotDisposed();
            mCanvas.SetDrawDynamic();

            float offset = NodeWidth / 2 + NodePortWidth * 2;
            float x1 = nodeView.Rect.X + nodeView.Rect.Width / 2 + offset;
            float y1 = nodeView.Rect.Y + nodeView.Rect.Height / 2;
            float x2 = locaction.X;
            float y2 = locaction.Y;

            Point pt1 = new Point(x1, y1);
            Point pt2 = new Point(x2 - (x2 - x1) / 4, y1);
            Point pt3 = new Point(x1 + (x2 - x1) / 4, y2);
            Point pt4 = new Point(x2, y2);

            mCanvas.DrawBezier(pt1, pt2, pt3, pt4, ConnectionColor, 2f);
        }

        protected NodeView GetNodeViewByIdx(int nodeIdx)
        {
            if (nodeIdx < 0 || nodeIdx >= mNodeViews.Count)
            {
                return null;
            }
            return mNodeViews[nodeIdx];
        }

        protected void RegMouseDownEvent(Rect rect, Action<Point, MouseButton> callback)
        {
            if (!mMouseDownCallbacks.ContainsKey(rect))
            {
                mMouseDownCallbacks[rect] = new List<Action<Point, MouseButton>>();
            }
            mMouseDownCallbacks[rect].Add(callback);
        }

        protected void RegMouseUpEvent(Rect rect, Action<Point, MouseButton> callback)
        {
            if (!mMouseUpCallbacks.ContainsKey(rect))
            {
                mMouseUpCallbacks[rect] = new List<Action<Point, MouseButton>>();
            }
            mMouseUpCallbacks[rect].Add(callback);
        }

        protected void RegMouseClickEvent(Rect rect, Action<Point> callback)
        {
            if (!mMouseClickCallbacks.ContainsKey(rect))
            {
                mMouseClickCallbacks[rect] = new List<Action<Point>>();
            }
            mMouseClickCallbacks[rect].Add(callback);
        }

        protected void RegMouseEnterEvent(Rect rect, Action<Point> callback)
        {
            if (!mMouseEnterCallbacks.ContainsKey(rect))
            {
                mMouseEnterCallbacks[rect] = new List<Action<Point>>();
            }
            mMouseEnterCallbacks[rect].Add(callback);
        }

        protected void RegMouseExitEvent(Rect rect, Action<Point> callback)
        {
            if (!mMouseExitCallbacks.ContainsKey(rect))
            {
                mMouseExitCallbacks[rect] = new List<Action<Point>>();
            }
            mMouseExitCallbacks[rect].Add(callback);
        }

        private void OnMouseMove(Point point)
        {
            foreach (var item in mMouseEnterCallbacks)
            {
                Rect rect = item.Key;
                List<Action<Point>> callbacks = item.Value;
                if (rect.Contains(point) && !mMouseHoverRects.Contains(rect))
                {
                    mMouseHoverRects.Add(rect);
                    foreach (var callback in callbacks)
                    {
                        callback.Invoke(point);
                    }
                }
            }

            foreach (var item in mMouseExitCallbacks)
            {
                Rect rect = item.Key;
                List<Action<Point>> callbacks = item.Value;
                if (!rect.Contains(point) && mMouseHoverRects.Contains(rect))
                {
                    mMouseHoverRects.Remove(rect);
                    foreach (var callback in callbacks)
                    {
                        callback.Invoke(point);
                    }
                }
            }
        }

        private void OnMouseDown(Point point, MouseButton button)
        {
            foreach (var item in mMouseDownCallbacks)
            {
                Rect rect = item.Key;
                List<Action<Point, MouseButton>> callbacks = item.Value;
                if (rect.Contains(point))
                {
                    foreach (var callback in callbacks)
                    {
                        callback.Invoke(point, button);
                    }
                }
            }
        }

        private void OnMouseUp(Point point, MouseButton button)
        {
            foreach (var item in mMouseUpCallbacks)
            {
                Rect rect = item.Key;
                List<Action<Point, MouseButton>> callbacks = item.Value;
                if (rect.Contains(point))
                {
                    foreach (var callback in callbacks)
                    {
                        callback.Invoke(point, button);
                    }
                }
            }
        }

        private void OnMouseClick(Point point)
        {
            foreach (var item in mMouseClickCallbacks)
            {
                Rect rect = item.Key;
                List<Action<Point>> callbacks = item.Value;
                if (rect.Contains(point))
                {
                    foreach (var callback in callbacks)
                    {
                        callback.Invoke(point);
                    }
                }
            }
        }

        protected void ClearMouseEvent()
        {
            mMouseDownCallbacks.Clear();
            mMouseUpCallbacks.Clear();
            mMouseHoverRects.Clear();
            mMouseClickCallbacks.Clear();
            mMouseEnterCallbacks.Clear();
            mMouseExitCallbacks.Clear();
        }

        public virtual void Dispose()
        {
            mDisposed = true;
            mCanvas?.Dispose();
            mCanvas = null;
        }

        protected void EnsureNotDisposed()
        {
            if (mDisposed)
            {
                throw new ObjectDisposedException(nameof(CanvasView));
            }
        }
    }
}
