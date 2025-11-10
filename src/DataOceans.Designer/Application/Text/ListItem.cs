using SkiaSharp;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using WASMApp.Application.Design;

namespace WASMApp.Application.Text;

public enum BulletType
{
    Circle,
    TextBox,
    Diamond,
    Number
}

public class ListItem : Paragraph
{
    public BulletType BulletType { get; set; }
    
    public ListItem(IStyle style, BulletType bulletType = BulletType.Circle)
    {
        BulletType = bulletType;
        _textBlock = new TextBlock();
        _textBlock.AddText("This is a test, help.", style);
    }

    public override void Layout(TextDocument owner)
    {
        _textBlock.RenderWidth = owner.PageWidth - this.MarginLeft - this.MarginRight;
        // owner.PageWidth
        // - this.MarginLeft - this.MarginRight;
        if (owner.LineWrap)
        {
            _textBlock.MaxWidth = _textBlock.RenderWidth;
        }
        else
            _textBlock.MaxWidth = null;
    }

    public override void Paint(SKCanvas canvas, TextPaintOptions options)
    {
        const float bulletSize = 10f;
        const float spacing = 20f;
        SKPaint bulletPaint = new SKPaint()
        {
            Color = SKColors.Black
        };
        switch (BulletType)
        {
            case BulletType.Circle:
                canvas.DrawCircle(ContentXCoord, ContentYCoord, 5.0f, bulletPaint);
                break;
            case BulletType.TextBox:
                var textBoxRect = new SKRect(ContentXCoord, ContentYCoord, ContentXCoord + bulletSize, ContentYCoord + bulletSize);
                canvas.DrawRect(textBoxRect, bulletPaint);
                break;
            default:
                throw new NotImplementedException();
        }

        _textBlock.Paint(canvas, new SKPoint(ContentXCoord + spacing, ContentYCoord - 5.0f), options);
    }

    /// <inheritdoc />
    public override CaretInfo GetCaretInfo(CaretPosition position) => _textBlock.GetCaretInfo(position);

    /// <inheritdoc />
    public override HitTestResult HitTest(float x, float y) => _textBlock.HitTest(x, y);

    /// <inheritdoc />
    public override HitTestResult HitTestLine(int lineIndex, float x) => _textBlock.HitTestLine(lineIndex, x);

    /// <inheritdoc />
    public override IReadOnlyList<int> CaretIndicies => _textBlock.CaretIndicies;

    /// <inheritdoc />
    public override IReadOnlyList<int> WordBoundaryIndicies => _textBlock.WordBoundaryIndicies;

    /// <inheritdoc />
    public override IReadOnlyList<int> LineIndicies => _textBlock.LineIndicies;

    /// <inheritdoc />
    public override int Length => _textBlock.Length;

    /// <inheritdoc />
    public override float ContentWidth => _textBlock.MeasuredWidth;

    /// <inheritdoc />
    public override float ContentHeight => _textBlock.MeasuredHeight;
    
    public override TextBlock TextBlock => _textBlock;

    TextBlock _textBlock;
}