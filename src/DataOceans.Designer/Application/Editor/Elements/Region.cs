using SkiaSharp;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using WASMApp.Application.Design;
using WASMApp.Application.Editor.Core;
using WASMApp.Application.Render;
using WASMApp.Application.Text;

namespace WASMApp.Application.Editor.Elements;

public class Region : InteractiveElement, IDraggable
{
    private CursorStyle _cursorStyle = CursorStyle.Pointer;

    private SKPoint? _originPosition = null;
    
    public override IList<IInteractiveElement> Children { get; } = new List<IInteractiveElement>();

    public override CursorStyle CursorStyle => _cursorStyle;

    public TextDocument TextDocument { get; }
    public ITextDocumentView DocumentView { get; }
    public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
    public SKPoint Position => new SKPoint(Bounds.Left, Bounds.Top);

    public int Width => (int)Bounds.Width;
    public int Height => (int)Bounds.Height;
    
    public Border? Border { get; private set; }
    public SKColor? BackgroundColor { get; private set; }
    
    public string Name { get; private set; }
    
    public bool Focused { get; set; }

    public Region(string name)
    {
        Bounds = new SKRect(0, 0, 200, 200);
        TextDocument = new TextDocument();
        DocumentView = new DocumentView();
        TextDocument.AddParagraph(new ListItem(StyleManager.Default.Value.DefaultStyle, BulletType.Circle));
        TextDocument.AddParagraph(new ListItem(StyleManager.Default.Value.DefaultStyle, BulletType.Circle));
        TextDocument.AddParagraph(new ListItem(StyleManager.Default.Value.DefaultStyle, BulletType.Circle));
        TextDocument.SetBounds(Bounds);
        TextDocument.RegisterView(DocumentView);
        Name = name;
        CreateDragHandles();
    }
    
    public override void OnClick(SKPoint point)
    {
        Focused = true;
        UpdateCursor(point);
    }

    private void CreateDragHandles()
    {
        Children.Add(new DragHandle(this, DragHandlePosition.TopLeft));
        Children.Add(new DragHandle(this, DragHandlePosition.TopRight));
        Children.Add(new DragHandle(this, DragHandlePosition.BottomLeft));
        Children.Add(new DragHandle(this, DragHandlePosition.BottomRight));
    }

    public void UpdatePosition(float x, float y)
    {
        Bounds = new SKRect(x, y, x + Width, y + Height);
        CompositeUpdate();
    }

    public void Resize(float newWidth, float newHeight)
    {
        Bounds = Bounds with { Size = new SKSize(newWidth, newHeight) };
        CompositeUpdate();
    }

    public void SetBounds(SKRect bounds)
    {
        Bounds = bounds;
        CompositeUpdate();
    }

    public void SetBackgroundColor(SKColor color)
    {
        BackgroundColor = color;
    }

    public void SetBorder(Border border)
    {
        Border = border;
    }

    private void CompositeUpdate()
    {
        TextDocument.SetBounds(Bounds);
        foreach (var handles in Children.OfType<DragHandle>())
        {
            handles.UpdatePosition();
        }
    }

    public override bool HitTest(SKPoint point)
    {
        if (Bounds.Contains(point) || Children.Any(x => x.HitTest(point)))
        {
            UpdateCursor(point);
            return true;
        }
        
        return false;
    }

    private void UpdateCursor(SKPoint point)
    {
        if (TextDocument.Bounds.Contains(point) && Focused && InteractionManager.Instace.InteractionMode == InteractionMode.TextFocus)
        {
            _cursorStyle = CursorStyle.Text;
        }
        else
        {
            _cursorStyle = CursorStyle.Move;
        }
    }

    public override void Draw(SKCanvas canvas, EditorState editorState)
    {
        
        TextDocument.Paint(
            canvas,
            Position.Y,
            Position.Y + TextDocument.Length,
            (editorState.ActiveRegion == this && InteractionManager.Instace.InteractionMode == InteractionMode.TextFocus) ? new TextPaintOptions
        {
            Selection = editorState.Selection,
            SelectionColor = SKColors.CornflowerBlue.WithAlpha(126)
        } : null);
        
        if (Border.HasValue)
        {
            canvas.DrawRect(Bounds, new SKPaint
            {
                IsStroke = true,
                StrokeWidth = Border.Value.Width,
                Color = Border.Value.Color
            });
        }
        else
        {
            canvas.DrawRect(Bounds, new SKPaint
            {
                IsStroke = true,
                StrokeWidth = 1,
                Color = SKColors.LightGray
            });
        }

        if (Focused)
        {
            foreach (var child in Children)
            {
                child.Draw(canvas, editorState);
            }   
        }
    }

    public void OnDragStart(SKPoint start)
    {
        _originPosition = Position;
    }

    public void OnDragUpdate(SKPoint delta)
    {
        if (InteractionManager.Instace.InteractionMode == InteractionMode.Default && _originPosition.HasValue)
        {
            UpdatePosition(_originPosition.Value.X + delta.X, _originPosition.Value.Y + delta.Y);   
        }
    }

    public void OnDragEnd()
    {
        _originPosition = null;
    }
}