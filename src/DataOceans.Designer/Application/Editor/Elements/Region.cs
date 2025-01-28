using Microsoft.AspNetCore.Components.Web;
using SkiaSharp;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using WASMApp.Application.Design;
using WASMApp.Application.Editor.Core;

namespace WASMApp.Application.Editor.Elements;

public class Region : InteractiveElement
{
    //public TextBlock TextBlock { get; set; }
    public override IList<IInteractiveElement> Children { get; } = new List<IInteractiveElement>();
    
    public TextDocument TextDocument { get; }
    public ITextDocumentView DocumentView { get; }
    public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
    public SKPoint Position { get; private set; }

    public int Width => (int)Bounds.Width;
    public int Height => (int)Bounds.Height;
    
    public Border? Border { get; private set; }
    public SKColor? BackgroundColor { get; private set; }
    
    public string Name { get; private set; }
    
    public bool Focused { get; set; }

    public Region(string name)
    {
        Bounds = new SKRect(0, 0, 200, 200);
        Position = new SKPoint(0, 0);
        //TextBlock = new TextBlock();
        TextDocument = new TextDocument();
        DocumentView = new DocumentView();
        TextDocument.PageWidth = this.Bounds.Width;
        TextDocument.SetBounds(Bounds);
        TextDocument.RegisterView(DocumentView);
        Name = name;
        // TextBlock.Alignment = TextAlignment;
        // TextBlock.MaxWidth = Width;
        // TextBlock.MaxHeight = Height;
        CreateDragHandles();
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
        Position = new SKPoint(x, y);
        Bounds = new SKRect(x, y, x + Width, y + Height);
        TextDocument.SetBounds(Bounds);
        foreach (var handles in Children.OfType<DragHandle>())
        {
            handles.UpdatePosition();
        }
    }

    public void Resize(int newWidth, int newHeight)
    {
        Bounds = Bounds with { Size = new SKSize(newWidth, newHeight) };
    }

    public void SetBackgroundColor(SKColor color)
    {
        BackgroundColor = color;
    }

    public void SetBorder(Border border)
    {
        Border = border;
    }

    public override bool HitTest(SKPoint point)
    {
        return Bounds.Contains(point);
    }

    public override void Draw(SKCanvas canvas, EditorState editorState)
    {
        TextDocument.Paint(canvas, Position.Y, Position.Y + TextDocument.Length, new TextPaintOptions
        {
            Selection = editorState.Selection,
            SelectionColor = SKColors.CornflowerBlue.WithAlpha(126)
        });
        
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
                Color = SKColors.Gray
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
    
}