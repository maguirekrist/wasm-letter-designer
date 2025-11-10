using SkiaSharp;
using WASMApp.Application.Editor.Core;
using WASMApp.Application.Render;

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
    private readonly SKPaint _handlePaint = new() { Color = SKColors.White };
    private readonly SKPaint _handleStroke = new() { Color = SKColors.Blue, StrokeWidth = 2, IsStroke = true,};
    private readonly DragHandlePosition _position;
    private const float HANDLE_SIZE = 8;

    private SKRect? _originalRegionBounds = null;

    public override void OnClick(SKPoint point)
    {
        throw new NotImplementedException();
    }

    public override CursorStyle CursorStyle => _position switch
    {
        DragHandlePosition.TopLeft => CursorStyle.ResizeNw,
        DragHandlePosition.TopRight => CursorStyle.ResizeNe,
        DragHandlePosition.BottomLeft => CursorStyle.ResizeSw,
        DragHandlePosition.BottomRight => CursorStyle.ResizeSe,
        _ => throw new ArgumentOutOfRangeException()
    };

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
            DragHandlePosition.TopLeft => SKRect.Create(parentBounds.Left - HANDLE_SIZE / 2,
                parentBounds.Top - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE),
            DragHandlePosition.TopRight => SKRect.Create(parentBounds.Right - (HANDLE_SIZE / 2),
                parentBounds.Top - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE),
            DragHandlePosition.BottomLeft => SKRect.Create(parentBounds.Left - HANDLE_SIZE / 2,
                parentBounds.Bottom - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE),
            DragHandlePosition.BottomRight => SKRect.Create(parentBounds.Right - HANDLE_SIZE / 2,
                parentBounds.Bottom - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE),
            _ => throw new ArgumentOutOfRangeException(nameof(_position), _position, null)
        };
    }

    public override bool HitTest(SKPoint point) => Bounds.Contains(point);

    public override void Draw(SKCanvas canvas, EditorState editorState)
    {
        canvas.DrawCircle(Bounds.MidX, Bounds.MidY, HANDLE_SIZE / 2, _handlePaint);
        canvas.DrawCircle(Bounds.MidX, Bounds.MidY, HANDLE_SIZE / 2, _handleStroke);
    }

    public void OnDragStart(SKPoint start)
    {
        _originalRegionBounds = _parentRegion.Bounds;
    }

    public void OnDragUpdate(SKPoint delta)
    {
        if (_originalRegionBounds.HasValue)
        {
            switch (_position)
            {
                case DragHandlePosition.BottomRight:
                    _parentRegion.Resize(_originalRegionBounds.Value.Width + delta.X, _originalRegionBounds.Value.Height + delta.Y);
                    break;
                case DragHandlePosition.BottomLeft:
                    _parentRegion.SetBounds(_originalRegionBounds.Value with
                    {
                        Left = _originalRegionBounds.Value.Left + delta.X, 
                        Size = new SKSize(_originalRegionBounds.Value.Width - delta.X, _originalRegionBounds.Value.Height + delta.Y)
                    });
                    break;
                case DragHandlePosition.TopLeft:
                    _parentRegion.SetBounds(_originalRegionBounds.Value with
                    {
                        Left = _originalRegionBounds.Value.Left + delta.X, 
                        Top = _originalRegionBounds.Value.Top + delta.Y,
                        Size = new SKSize(_originalRegionBounds.Value.Width - delta.X, _originalRegionBounds.Value.Height - delta.Y)
                    });
                    break;
                case DragHandlePosition.TopRight:
                    _parentRegion.SetBounds(_originalRegionBounds.Value with
                    {
                        Top = _originalRegionBounds.Value.Top + delta.Y, 
                        Size = new SKSize(_originalRegionBounds.Value.Width + delta.X, _originalRegionBounds.Value.Height - delta.Y)
                    });
                    break;
            }
        }
    }

    public void OnDragEnd()
    {
        _originalRegionBounds = null;
    }
}