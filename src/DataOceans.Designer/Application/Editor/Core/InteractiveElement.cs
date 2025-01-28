using Microsoft.AspNetCore.Components.Web;
using SkiaSharp;

namespace WASMApp.Application.Editor.Core;

public interface IInteractiveElement
{
    bool HitTest(SKPoint point);
    void Draw(SKCanvas canvas, EditorState editorState);
    IInteractiveElement? Parent { get; set; }
    IList<IInteractiveElement>? Children { get; }
}

public abstract class InteractiveElement : IInteractiveElement
{
    public SKRect Bounds { get; protected set; }
    public IInteractiveElement? Parent { get; set; }
    public virtual IList<IInteractiveElement>? Children => null;

    public abstract bool HitTest(SKPoint point);
    public abstract void Draw(SKCanvas canvas, EditorState editorState);
}