using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class ControlMouseHandler : IMouseHandler
    {
        private Control mControl;
        private bool Disposed = false;

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
            EnsureNotDisposed();
            Point mousePoint = GetMousePoint();
            MouseButton button = (MouseButton)e.Button;
            MouseDown?.Invoke(GetMousePoint(), (MouseButton)e.Button);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            EnsureNotDisposed();
            Point mousePoint = GetMousePoint();
            MouseButton button = (MouseButton)e.Button;
            MouseUp?.Invoke(GetMousePoint(), (MouseButton)e.Button);
        }

        private void OnMouseClick(object sender, EventArgs e)
        {
            EnsureNotDisposed();
            Point mousePoint = GetMousePoint();
            MouseClick?.Invoke(mousePoint);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            EnsureNotDisposed();
            Point mousePoint = GetMousePoint();
            MouseMove?.Invoke(mousePoint);
        }

        public Point GetMousePoint()
        {
            EnsureNotDisposed();
            var clientPos = mControl.PointToClient(Cursor.Position);
            var localPos = new Point(clientPos.X - mControl.Location.X, clientPos.Y - mControl.Location.Y);
            return localPos;
        }

        public bool IsMouseDown(MouseButton button)
        {
            EnsureNotDisposed();
            return (Control.MouseButtons & (MouseButtons)button) == (MouseButtons)button;
        }

        public void Dispose()
        {
            Disposed = true;
            mControl.MouseDown -= OnMouseDown;
            mControl.MouseUp -= OnMouseUp;
            mControl.Click -= OnMouseClick;
            mControl.MouseMove -= OnMouseMove;
            mControl = null;
        }

        private void EnsureNotDisposed()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(ControlMouseHandler));
            }
        }
    }
}