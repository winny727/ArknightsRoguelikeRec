using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public interface ICanvasLayer : IDisposable
    {
        void Clear(Color? color = null);
        void DrawLine(Point start, Point end, Color? color = null, float width = 1f);
        void DrawRectangle(Rect rect, Color? color = null, float width = 1f);
        void FillRectangle(Rect rect, Color? color = null);
        void DrawCircle(Point center, float radius, Color? color = null, float width = 1f);
        void FillCircle(Point center, float radius, Color? color = null);
        void DrawBezier(Point p1, Point p2, Point p3, Point p4, Color? color = null, float width = 1f);
        void DrawPoint(Point point, Color? color = null, float size = 1f);
        void DrawString(string text, Rect layoutRect, Color? color = null, TextAlignment alignment = TextAlignment.Center);
    }
}