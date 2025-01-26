using SkiaSharp;
using Topten.RichTextKit;
using WASMApp.Application.Design;
using WASMApp.Application.Editor;

namespace WASMApp.Application.Render;

public class DocumentRenderer
{
    public static void Render(SKCanvas canvas, EditorState editor)
    {
        foreach (var region in editor.Document.Regions)
        {
            RenderRegion(canvas, region, editor);
        }
        
        RenderCaret(canvas, editor.Caret);
    }

    private static void RenderRegion(SKCanvas canvas, Region region, EditorState editor)
    {
        //Draw Text
        region.TextDocument.Paint(canvas, region.Position.Y, region.Position.Y + region.TextDocument.Length, new TextPaintOptions
        {
            Selection = editor.Selection,
            SelectionColor = SKColors.CornflowerBlue.WithAlpha(126)
        });
        
        //Draw Design
        if (region.Border.HasValue)
        {
            canvas.DrawRect(region.Bounds, new SKPaint
            {
                IsStroke = true,
                StrokeWidth = region.Border.Value.Width,
                Color = region.Border.Value.Color
            });
        }
        
        if (editor.ActiveRegion == region)
        {
            canvas.DrawRect(region.Bounds, new SKPaint
            {
                IsStroke = true,
                StrokeWidth = 1.0f,
                Color = SKColors.Blue
            });
        }
    }

    private static void RenderCaret(SKCanvas canvas, Caret caret)
    {
        canvas.DrawRect(caret.Bounds, new SKPaint
        {
            Color = SKColors.Black
        });
    }
}