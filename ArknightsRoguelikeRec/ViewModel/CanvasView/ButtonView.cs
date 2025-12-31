using ArknightsRoguelikeRec.ViewModel.DataStruct;
using System;
using System.Collections.Generic;

namespace ArknightsRoguelikeRec.ViewModel
{
    public class ButtonView : IDisposable
    {
        public Rect Rect { get; private set; }
        private IMouseHandler mMouseHandler;
        private bool mDisposed = false;

        private readonly Dictionary<MouseButton, bool> mPressedStates = new Dictionary<MouseButton, bool>();
        public bool IsHover { get; private set; } = false;

        public bool IsPressed
        {
            get
            {
                foreach (var state in mPressedStates.Values)
                {
                    if (state)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public event Action<MouseButton> PointerDown; // 按钮范围内按下鼠标
        public event Action<MouseButton> PointerUp; // 按钮范围内抬起鼠标
        public event Action<MouseButton> Click; // 按钮范围内点击鼠标
        public event Action<MouseButton> Cancel; // 在按钮范围外抬起鼠标取消点击
        public event Action PointerEnter; // 鼠标进入按钮范围
        public event Action PointerExit; // 鼠标离开按钮范围

        public ButtonView(Rect rect, IMouseHandler mouseHandler)
        {
            Rect = rect;
            mMouseHandler = mouseHandler ?? throw new ArgumentNullException(nameof(mouseHandler));

            mMouseHandler.MouseDown += OnMouseDown;
            mMouseHandler.MouseUp += OnMouseUp;
            mMouseHandler.MouseMove += OnMouseMove;
        }

        public bool IsButtonPressed(MouseButton button)
        {
            if (mPressedStates.TryGetValue(button, out bool isPressed))
            {
                return isPressed;
            }
            return false;
        }

        private void SetIsPressed(MouseButton button, bool isPressed)
        {
            mPressedStates[button] = isPressed;
        }

        private void OnMouseDown(Point point, MouseButton button)
        {
            if (mDisposed) return;

            if (!Rect.Contains(point))
            {
                return;
            }

            bool isPressed = IsButtonPressed(button);
            SetIsPressed(button, true);

            PointerDown?.Invoke(button);
        }

        private void OnMouseUp(Point point, MouseButton button)
        {
            if (mDisposed) return;

            bool contains = Rect.Contains(point);
            bool isPressed = IsButtonPressed(button);

            SetIsPressed(button, false);

            if (contains)
            {
                PointerUp?.Invoke(button);
            }

            if (isPressed)
            {
                if (contains)
                {
                    Click?.Invoke(button);
                }
                else
                {
                    Cancel?.Invoke(button);
                }
            }
        }

        private void OnMouseMove(Point point)
        {
            if (mDisposed) return;
            bool contains = Rect.Contains(point);
            if (contains && !IsHover)
            {
                IsHover = true;
                PointerEnter?.Invoke();
            }
            else if (!contains && IsHover)
            {
                IsHover = false;
                PointerExit?.Invoke();
            }
        }

        public virtual void Dispose()
        {
            if (mDisposed)
            {
                return;
            }

            mDisposed = true;

            if (mMouseHandler != null)
            {
                mMouseHandler.MouseDown -= OnMouseDown;
                mMouseHandler.MouseUp -= OnMouseUp;
                mMouseHandler.MouseMove -= OnMouseMove;
            }

            mPressedStates.Clear();
            mMouseHandler = null;
        }
    }
}
