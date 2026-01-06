using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.DataStruct;

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
}