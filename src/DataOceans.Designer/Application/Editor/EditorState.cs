using System.Collections.ObjectModel;
using SkiaSharp;
using Topten.RichTextKit;
using WASMApp.Application.Design;
using WASMApp.Application.Editor.Core;
using WASMApp.Application.Editor.Elements;

namespace WASMApp.Application.Editor;

public class EditorState
{
    public Caret Caret { get; private set; }
    public TextRange Selection { get; set; }

    public ObservableCollection<IInteractiveElement> SelectedElements { get; } = new();
    
    public float Scale { get; set; } = 1.0f;
    
    private LetterDocument _document;

    public LetterDocument Document => _document;
    public StyleManager StyleManager { get; init; }
    
    public Region? ActiveRegion { get; set; }
    
    public EditorState()
    {
        _document = new LetterDocument();
        StyleManager = new StyleManager();
        Caret = new Caret();
        Selection = new TextRange(0, 10);

        AddRegion("test-region", 20, 20);
    }

    public void MoveCaret(float x, float y)
    {
        if (ActiveRegion != null)
        {
            var htr = ActiveRegion.TextDocument.HitTest(x, y);
            var caretInfo = ActiveRegion.TextDocument.GetCaretInfo(htr.CaretPosition);
            Caret = new Caret(caretInfo);
            Selection = new TextRange(caretInfo.CodePointIndex);
            var newRange = new TextRange(Selection.End, Selection.End - 1);
            Console.WriteLine(newRange);

            if (newRange.End >= 0)
            {
                var extracted = ActiveRegion.TextDocument.Extract(newRange);

                if (extracted != null)
                {
                    Console.WriteLine($"test: {extracted}");
                    StyleManager.CurrentStyle = extracted.StyleRuns[0].Style;
                }   
            }
            // _caretInfo = mainTextBlock.GetCaretInfo(_caretPosition);
            // _caretRect = _caretInfo.CaretRectangle with { Size = new SKSize(1, _caretInfo.CaretRectangle.Height) };
        }
    }

    public void MoveSelectionEnd(float x, float y)
    {
        if (ActiveRegion != null)
        {
            var htr = ActiveRegion.TextDocument.HitTest(x, y);
            var caretInfo = ActiveRegion.TextDocument.GetCaretInfo(htr.CaretPosition);
            Selection = new TextRange(Caret.CodePointIndex, caretInfo.CodePointIndex);
        }
    }

    public void ChangeSelection(int amount)
    {
        Selection = new TextRange(Caret.CodePointIndex + amount);
        var caretInfo = ActiveRegion.TextDocument.GetCaretInfo(new CaretPosition(Caret.CodePointIndex + amount));
        Caret = new Caret(caretInfo);
    }

    public void AddRegion(string name, float x, float y)
    {
        var newRegion = new Region(name);
        newRegion.UpdatePosition(x, y);
        _document.AddRegion(newRegion);
    }
    
}