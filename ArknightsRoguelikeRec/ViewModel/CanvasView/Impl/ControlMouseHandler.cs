using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ArknightsRoguelikeRec.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel.Impl
{
    public class ControlMouseHandler : IMouseHandler
    {
        private Control mControl;
        private bool mDisposed = false;

        public event Action<Point, MouseButton> MouseDown;
        public event Action<Point, MouseButton> MouseUp;
        public event Action<Point> MouseClick;
        public event Action<Point> MouseMove;

        public ControlMouseHandler(Control control)
        {
            mControl = control ?? throw new ArgumentNullException(nameof(control));
            mControl.MouseDown += OnMouseDown;
            mControl.MouseUp += OnMouseUp;
            mControl.Click += OnMouseClick;
            mControl.MouseMove += OnMouseMove;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (mDisposed) return;
            Point mousePoint = GetMousePoint();
            MouseButton button = (MouseButton)e.Button;
            MouseDown?.Invoke(GetMousePoint(), (MouseButton)e.Button);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (mDisposed) return;
            Point mousePoint = GetMousePoint();
            MouseButton button = (MouseButton)e.Button;
            MouseUp?.Invoke(GetMousePoint(), (MouseButton)e.Button);
        }

        private void OnMouseClick(object sender, EventArgs e)
        {
            if (mDisposed) return;
            Point mousePoint = GetMousePoint();
            MouseClick?.Invoke(mousePoint);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (mDisposed) return;
            Point mousePoint = GetMousePoint();
            MouseMove?.Invoke(mousePoint);
        }

        public Point GetMousePoint()
        {
            if (mDisposed) return default;
            var cursorPos = mControl.PointToClient(Cursor.Position);
            Point point = new Point(cursorPos.X, cursorPos.Y);
            return point;
        }

        public bool IsMouseDown(MouseButton button)
        {
            if (mDisposed) return default;
            return (Control.MouseButtons & (MouseButtons)button) == (MouseButtons)button;
        }

        public void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            mDisposed = true;
            mControl.MouseDown -= OnMouseDown;
            mControl.MouseUp -= OnMouseUp;
            mControl.Click -= OnMouseClick;
            mControl.MouseMove -= OnMouseMove;
            mControl = null;
        }
    }
}