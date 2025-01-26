using SkiaSharp;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using WASMApp.Application.Editor;

namespace WASMApp.Application.Design;

public class Region
{
    //public TextBlock TextBlock { get; set; }
    public TextDocument TextDocument { get; set; }
    public ITextDocumentView DocumentView { get; }
    public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
    public SKPoint Position { get; set; }
    public SKRect Bounds { get; set; }

    public int Width => (int)Bounds.Width;
    public int Height => (int)Bounds.Height;
    
    public Border? Border { get; private set; }
    public SKColor? BackgroundColor { get; private set; }

    public Region()
    {
        Bounds = new SKRect(0, 0, 200, 200);
        Position = new SKPoint(0, 0);
        //TextBlock = new TextBlock();
        TextDocument = new TextDocument();
        DocumentView = new DocumentView();
        TextDocument.PageWidth = this.Bounds.Width;
        TextDocument.RegisterView(DocumentView);
        // TextBlock.Alignment = TextAlignment;
        // TextBlock.MaxWidth = Width;
        // TextBlock.MaxHeight = Height;
    }

    public void UpdatePosition(int x, int y)
    {
        Position = new SKPoint(x, y);
        Bounds = new SKRect(x, y, x + Width, y + Height);
        
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
}