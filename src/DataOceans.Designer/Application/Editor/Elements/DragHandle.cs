using SkiaSharp;
using WASMApp.Application.Editor.Core;

namespace WASMApp.Application.Editor.Elements;

public enum DragHandlePosition
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

public class DragHandle : InteractiveElement, IDraggable
{

    private readonly Region _parentRegion;
    private readonly SKPaint _handlePaint = new() { Color = SKColors.Blue };
    private readonly DragHandlePosition _position;
    private const float HandleSize = 8;
    
    public DragHandle(Region parent, DragHandlePosition position)
    {
        _parentRegion = parent;
        _position = position;
        Parent = parent;
    }

    public void UpdatePosition()
    {
        var parentBounds = _parentRegion.Bounds;
        Bounds = _position switch
        {
            DragHandlePosition.TopLeft => SKRect.Create(parentBounds.Left - HandleSize / 2,
                parentBounds.Top - HandleSize / 2, HandleSize, HandleSize),
            DragHandlePosition.TopRight => SKRect.Create(parentBounds.Right - (HandleSize / 2),
                parentBounds.Top - HandleSize / 2, HandleSize, HandleSize),
            DragHandlePosition.BottomLeft => SKRect.Create(parentBounds.Left - HandleSize / 2,
                parentBounds.Bottom - HandleSize / 2, HandleSize, HandleSize),
            DragHandlePosition.BottomRight => SKRect.Create(parentBounds.Right - HandleSize / 2,
                parentBounds.Bottom - HandleSize / 2, HandleSize, HandleSize),
            _ => throw new ArgumentOutOfRangeException(nameof(_position), _position, null)
        };
    }

    public override bool HitTest(SKPoint point) => Bounds.Contains(point);

    public override void Draw(SKCanvas canvas, EditorState editorState)
    {
        canvas.DrawCircle(Bounds.MidX, Bounds.MidY, HandleSize / 2, _handlePaint);
    }

    public void OnDragStart(SKPoint start)
    {
        throw new NotImplementedException();
    }

    public void OnDragUpdate(SKPoint delta)
    {
        throw new NotImplementedException();
    }

    public void OnDragEnd()
    {
        throw new NotImplementedException();
    }
}