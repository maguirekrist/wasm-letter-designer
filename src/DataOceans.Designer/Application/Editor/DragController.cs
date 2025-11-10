using SkiaSharp;
using WASMApp.Application.Editor.Core;

namespace WASMApp.Application.Editor;

public class DragController
{
    private IDraggable _target;
    private SKPoint _begin;
    private SKPoint _current;

    public DragController(IDraggable target, SKPoint begin)
    {
        _target = target;
        _begin = begin;
        _current = begin;
        StartDrag(begin);
    }
    
    public void StartDrag(SKPoint start) => _target.OnDragStart(start);
    public void UpdateDrag(SKPoint current)
    {
        _current = current;
        var delta = SKPoint.Subtract(_current, _begin);
        _target.OnDragUpdate(delta);
    }

    public void StopDrag() => _target.OnDragEnd();
    
}