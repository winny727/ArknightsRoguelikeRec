using System;
using System.Collections.Generic;
using ArknightsRoguelikeRec.ViewModel.DataStruct;

namespace ArknightsRoguelikeRec.ViewModel
{
    public interface ICanvas : IDisposable
    {
        void InitCanvas(float width, float height, Color? backgroundColor = null);
        void ApplyCanvas();
        void ClearCanvas();
        ICanvasLayer NewCanvasLayer();
        void ClearCanvasLayer();
    }
}