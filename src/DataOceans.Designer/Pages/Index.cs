using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using SkiaSharp;
using SkiaSharp.Views.Blazor;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;
using WASMApp.Application.Editor;
using WASMApp.Application.Editor.Core;
using WASMApp.Application.Editor.DOM;
using WASMApp.Application.Editor.Elements;
using WASMApp.Application.Render;

namespace WASMApp.Pages;



public partial class Index
{
    [Inject] private IJSRuntime JS { get; set; }
    private EditorState _editor = null!;
    private ElementReference CanvasContainer;
    private SKCanvasView CanvasView;
    private DOMRect _canvasBounds;
    private DragController? _dragController;
    private (float, float) _mousePosition;
    private bool _isMouseDown = false;
    
    [Inject] public InteractionManager _interactionManager { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        _editor = new EditorState();
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _canvasBounds = await JS.InvokeAsync<DOMRect>("getBoundingClientRect", CanvasContainer);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);
       
        canvas.Scale(1);
        DocumentRenderer.Render(canvas, _editor);
    }

    private void OnMouseDown(MouseEventArgs e)
    {
        
        Console.WriteLine($"OnMouseDown x: {e.ClientX}, y: {e.ClientY}");
        var newX = (float)(e.ClientX - _canvasBounds.Left);
        var newY = (float)(e.ClientY - _canvasBounds.Top);
        var clickPoint = new SKPoint(newX, newY);
        _isMouseDown = true;

        var element = _editor.HitTest(clickPoint);

        if (element is Region region)
        {
            if (_editor.ActiveRegion != null)
            {
                _editor.ActiveRegion.Focused = false;
            }
            
            region.OnClick(clickPoint);
            _editor.ActiveRegion = region;
            _editor.MoveCaret(clickPoint.X, clickPoint.Y);
        }

        if (element is IDraggable draggable)
        {
            _dragController = new DragController(draggable, clickPoint);
        }

        if (element is null)
        {
            HandleUnFocus();
        }
    }

    private void OnDoubleClick(MouseEventArgs args)
    {
        if (_editor.ActiveRegion is not null)
        {
            _interactionManager.SetInteractionMode(InteractionMode.TextFocus);
        }
        else
        {
            _interactionManager.SetInteractionMode(InteractionMode.Default);
        }
            
    }

    private void OnMouseUp(MouseEventArgs e)
    {
        if (_isMouseDown)
        {
            var newX = (int)(e.ClientX - _canvasBounds.Left);
            var newY = (int)(e.ClientY - _canvasBounds.Top);
            _mousePosition = (newX, newY);
            _editor.MoveSelectionEnd(newX, newY);
            _dragController?.StopDrag();
        }

        _dragController = null;
        _isMouseDown = false;
    }

    private async Task OnMouseMove(MouseEventArgs e)
    {
        var newX = (float)(e.ClientX - _canvasBounds.Left);
        var newY = (float)(e.ClientY - _canvasBounds.Top);
        var mousePoint = new SKPoint(newX, newY);
        
        if (_isMouseDown)
        {
            _mousePosition = (newX, newY);
            _editor.MoveSelectionEnd(newX, newY);
            _dragController?.UpdateDrag(mousePoint);
        }

        var hoveredElement = _editor.HitTest(mousePoint);
        
        if (hoveredElement != null)
        {
            //Console.WriteLine($"hovered: {hoveredElement.GetType().Name}");
            await _interactionManager.SetCursor(hoveredElement.CursorStyle);
        }
        else
        {
            await _interactionManager.SetCursor(CursorStyle.Default);
        }
    }
    
    private void OnMouseWheel(WheelEventArgs e)
    {
        Console.WriteLine(e.DeltaY);
        // _scale += (float)e.DeltaY;
    }

    private void HandleKeyDown(KeyboardEventArgs e)
    {
        Console.WriteLine($"Keyboard pressed with key: {e.Key} of type: {e.Type}, isCtrlKey: {e.CtrlKey}");
        if (_editor.ActiveRegion != null)
        {
            if (e.CtrlKey || e.MetaKey)
            {
                if (e.Key == "z")
                {
                    _editor.ActiveRegion.TextDocument.Undo(_editor.ActiveRegion.DocumentView);
                }

                if (e.Key == "y")
                {
                    _editor.ActiveRegion.TextDocument.Redo(_editor.ActiveRegion.DocumentView);
                }
            }
            else
            {
                if (e.Key.Length == 1)
                {
                    _editor.ActiveRegion.TextDocument.ReplaceText(_editor.ActiveRegion.DocumentView, _editor.Selection, e.Key, EditSemantics.Typing, _editor.StyleManager.CurrentStyle);  
                    _editor.ChangeSelection(1);
                }

                if (e.Key == "Backspace")
                {
                    _editor.ActiveRegion.TextDocument.ReplaceText(_editor.ActiveRegion.DocumentView, _editor.Selection.Length == 0 ? new TextRange(_editor.Selection.End, _editor.Selection.End - 1) : _editor.Selection, "", EditSemantics.Backspace); 
                    _editor.ChangeSelection(_editor.Selection.Length == 0 ? -1 : _editor.Selection.Length);
                }   
            }
        }
    }

    private void GetDocumentText()
    {
        if (_editor.ActiveRegion != null)
        {
            Console.WriteLine(_editor.ActiveRegion.TextDocument.Text);
            var styledText = _editor.ActiveRegion.TextDocument.Extract(new TextRange(0, _editor.ActiveRegion.TextDocument.Length));
            Console.WriteLine(styledText);
        }
    }

    private async Task ConvertToPdf()
    {
        using var stream = new MemoryStream();
        using var pdfDoc = SKDocument.CreatePdf(stream);

        var pdfCanvas = pdfDoc.BeginPage(612, 792);

        DocumentRenderer.Render(pdfCanvas, _editor);
        
        pdfDoc.EndPage();
        pdfDoc.Close();

        var base64Pdf = Convert.ToBase64String(stream.ToArray());

        await JS.InvokeVoidAsync("downloadFile", "test.pdf", base64Pdf);
    }

    private void ChangeStyle()
    {
        _editor.StyleManager.CurrentStyle = new Style()
        {
            FontSize = 12,
            TextColor = SKColors.Red
        };
    }

    private void HandleUnFocus()
    {
        if (_editor.ActiveRegion != null)
        {
            _editor.ActiveRegion.Focused = false;
            _editor.ActiveRegion = null;   
            _interactionManager.SetInteractionMode(InteractionMode.Default);
        }
        _editor.Selection = new TextRange();
    }
}