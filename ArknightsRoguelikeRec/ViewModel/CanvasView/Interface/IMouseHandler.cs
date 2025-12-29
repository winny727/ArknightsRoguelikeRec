using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public interface IMouseHandler : IDisposable
    {
        Point GetMousePoint();
        bool IsMouseDown(MouseButton button);

        event Action<Point, MouseButton> MouseDown;
        event Action<Point, MouseButton> MouseUp;
        event Action<Point> MouseClick;
        event Action<Point> MouseMove;
    }

    namespace DataStruct
    {
        public enum MouseButton
        {
            None = 0,
            Left = 0x100000,
            Right = 0x200000,
            Middle = 0x400000,
            XButton1 = 0x800000,
            XButton2 = 0x1000000
        }
    }
}