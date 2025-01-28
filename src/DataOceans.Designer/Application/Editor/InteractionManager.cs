using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WASMApp.Application.Render;

namespace WASMApp.Application.Editor;

public interface IInteractionManager
{
    public Task SetCursor(CursorStyle style);
}

public class InteractionManager : IInteractionManager
{
    private readonly IJSRuntime _jsRuntime;
    
    private static readonly Dictionary<CursorStyle, string> _cursorStyles = new()
    {
        { CursorStyle.Default , "default" },
        { CursorStyle.Grab, "grab" },
        { CursorStyle.Grabbing, "grabbing" },
        { CursorStyle.Move, "move" },
        { CursorStyle.Pointer, "pointer" },
        { CursorStyle.Text, "text" },
        { CursorStyle.ResizeNe, "resize-ne" },
        { CursorStyle.ResizeNw, "resize-nw" },
        { CursorStyle.ResizeSe, "resize-se" },
        { CursorStyle.ResizeSw, "resize-sw" }
    };

    public InteractionManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetCursor(CursorStyle style)
    {
        await _jsRuntime.InvokeVoidAsync("changeCursor", _cursorStyles[style]);
    }
}