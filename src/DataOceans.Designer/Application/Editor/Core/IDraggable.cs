using SkiaSharp;

namespace WASMApp.Application.Editor.Core;

public interface IDraggable
{
    void OnDragStart(SKPoint start);
    void OnDragUpdate(SKPoint delta);
    void OnDragEnd();
}