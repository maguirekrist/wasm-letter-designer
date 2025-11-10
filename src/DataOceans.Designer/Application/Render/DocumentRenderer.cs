using SkiaSharp;
using Topten.RichTextKit;
using WASMApp.Application.Design;
using WASMApp.Application.Editor;
using WASMApp.Application.Editor.Elements;

namespace WASMApp.Application.Render;

public class DocumentRenderer
{
    public static void Render(SKCanvas canvas, EditorState editor)
    {
        foreach (var element in editor.Elements)
        {
            element.Draw(canvas, editor);
        }
        
        if (editor.ActiveRegion != null)
        {
            canvas.DrawRect(editor.ActiveRegion.Bounds, new SKPaint
            {
                IsStroke = true,
                StrokeWidth = 1.0f,
                Color = SKColors.Blue
            });

            if (InteractionManager.Instace.InteractionMode == InteractionMode.TextFocus)
            {
                RenderCaret(canvas, editor.Caret);
            }
        }
    }

    private static void RenderRegion(SKCanvas canvas, Region region, EditorState editor)
    {
        //Draw Text
        region.Draw(canvas, editor);
        
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