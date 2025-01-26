using SkiaSharp;
using Topten.RichTextKit;
using WASMApp.Application.Design;

namespace WASMApp.Application.Editor;

public class EditorState
{
    public Caret Caret { get; private set; }
    public TextRange Selection { get; set; }
    
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

        var testRegion = new Region();
        _document.AddRegion(testRegion);
    }

    public void MoveCaret(int x, int y)
    {
        if (ActiveRegion != null)
        {
            var htr = ActiveRegion.TextDocument.HitTest(x, y);
            var caretInfo = ActiveRegion.TextDocument.GetCaretInfo(htr.CaretPosition);
            Caret = new Caret(caretInfo);
            Selection = new TextRange(caretInfo.CodePointIndex);
            var extracted = ActiveRegion.TextDocument.Extract(new TextRange(Selection.End, Selection.End - 1));

            if (extracted != null)
            {
                Console.WriteLine(extracted);
                StyleManager.CurrentStyle = extracted.StyleRuns[0].Style;
            }
            // _caretInfo = mainTextBlock.GetCaretInfo(_caretPosition);
            // _caretRect = _caretInfo.CaretRectangle with { Size = new SKSize(1, _caretInfo.CaretRectangle.Height) };
        }
    }

    public void MoveSelectionEnd(int x, int y)
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
    
}