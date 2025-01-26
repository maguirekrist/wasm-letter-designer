using SkiaSharp;

namespace WASMApp.Application.Design;

public struct Border
{
    public int Width { get; set; }
    public SKColor Color { get; set;  }
    
    public Border(int width, SKColor color)
    {
        Width = width;
        Color = color;
    }
}