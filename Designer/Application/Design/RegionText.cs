using SkiaSharp;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using Topten.RichTextKit.Utils;

namespace WASMApp.Application.Design;

public class RegionText
{
    public RegionText()
    {
        _paragraphs = new List<Paragraph>();
        _undoManager = new UndoManager<RegionText>(this);

    }

    public void Paint(SKCanvas canvas)
    {
        
    }

    private void Layout()
    {
        if (_layoutValid)
        {
            return;
        }

        _layoutValid = true;
        
        
    }

    private bool _layoutValid;
    private List<Paragraph> _paragraphs;
    private UndoManager<RegionText> _undoManager;
}