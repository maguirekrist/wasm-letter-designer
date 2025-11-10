using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WASMApp.Application.Render;

namespace WASMApp.Application.Editor;

public interface IInteractionManager
{
    public Task SetCursor(CursorStyle style);
}

public enum InteractionMode
{
    Default,
    TextFocus
}

public class InteractionManager : IInteractionManager
{
    private readonly IJSRuntime _jsRuntime;

    public static InteractionManager Instace { get; private set; }

    private static readonly Dictionary<CursorStyle, string> _cursorStyles = new()
    {
        { CursorStyle.Default , "default" },
        { CursorStyle.Grab, "grab" },
        { CursorStyle.Grabbing, "grabbing" },
        { CursorStyle.Move, "move" },
        { CursorStyle.Pointer, "pointer" },
        { CursorStyle.Text, "text" },
        { CursorStyle.ResizeNe, "ne-resize" },
        { CursorStyle.ResizeNw, "nw-resize" },
        { CursorStyle.ResizeSe, "se-resize" },
        { CursorStyle.ResizeSw, "sw-resize" }
    };

    public InteractionMode InteractionMode { get; private set; } = InteractionMode.Default;

    public InteractionManager(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;

        Instace = this;
    }

    public async Task SetCursor(CursorStyle style)
    {
        //Console.WriteLine($"Set Cursor Style: {_cursorStyles[style]}");
        await _jsRuntime.InvokeVoidAsync("changeCursor", _cursorStyles[style]);
    }

    public void SetInteractionMode(InteractionMode mode)
    {
        Console.WriteLine($"Set Interaction Mode: {mode.ToString()}");
        InteractionMode = mode;
    }
}