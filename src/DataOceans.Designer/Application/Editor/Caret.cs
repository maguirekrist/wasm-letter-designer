using SkiaSharp;
using Topten.RichTextKit;

namespace WASMApp.Application.Editor;

public readonly struct Caret
{
    //private CaretPosition _caretPosition;
    private readonly CaretInfo _caretInfo;
    public SKRect Bounds => _caretInfo.CaretRectangle with { Size = new SKSize(1, _caretInfo.CaretRectangle.Height) };

    public int CodePointIndex => _caretInfo.CodePointIndex;

    public CaretPosition Position => new CaretPosition(CodePointIndex);
    
    public Caret(CaretInfo info)
    {
        _caretInfo = info;
    }
    
}