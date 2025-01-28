using SkiaSharp;
using Topten.RichTextKit;

namespace WorkerService1;

public class GeneratePdf
{
    
    public static void Generate()
    {
        string filePath = "output.pdf";
        using var stream = new MemoryStream();
        using var pdfDoc = SKDocument.CreatePdf(stream);

        var pdfCanvas = pdfDoc.BeginPage(612, 792);
        
        var mainTextBlock = new TextBlock();
        mainTextBlock.MaxWidth = 400;
        mainTextBlock.Alignment = TextAlignment.Center;
        mainTextBlock.AddText("Hello world!", new Style
        {
            FontFamily = "Arial",
            FontSize = 14
        });
        var borderRect = SKRect.Create(mainTextBlock.MaxWidth.Value, mainTextBlock.MaxHeight ?? 300);
        mainTextBlock.Paint(pdfCanvas);
        pdfCanvas.DrawRect(borderRect, new SKPaint
        {
            IsStroke = true,
            StrokeWidth = 1.0f,
            Color = SKColors.Red
        });
        
        pdfDoc.EndPage();
        pdfDoc.Close();
    
        File.WriteAllBytes(filePath, stream.ToArray());
        
        Console.WriteLine($"PDF saved to {Path.GetFullPath(filePath)}");

        // if (_caretRect != null)
        // {
        //     //Console.WriteLine("draw!");
        //     canvas.DrawRect(_caretRect.Value, new SKPaint
        //     {
        //         Color = SKColors.Black
        //     });
        // }
    }
}