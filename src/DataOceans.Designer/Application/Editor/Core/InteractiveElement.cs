using Microsoft.AspNetCore.Components.Web;
using SkiaSharp;
using WASMApp.Application.Render;

namespace WASMApp.Application.Editor.Core;

public interface IInteractiveElement
{
    bool HitTest(SKPoint point);
    void Draw(SKCanvas canvas, EditorState editorState);
    IInteractiveElement? Parent { get; set; }
    IList<IInteractiveElement>? Children { get; }
    void OnClick(SKPoint point);
    CursorStyle CursorStyle { get; }
}

public abstract class InteractiveElement : IInteractiveElement
{
    public SKRect Bounds { get; protected set; }
    public IInteractiveElement? Parent { get; set; }
    public virtual IList<IInteractiveElement>? Children => null;
    public virtual CursorStyle CursorStyle => CursorStyle.Default;
    public abstract bool HitTest(SKPoint point);
    public abstract void Draw(SKCanvas canvas, EditorState editorState);
    public abstract void OnClick(SKPoint point);
}