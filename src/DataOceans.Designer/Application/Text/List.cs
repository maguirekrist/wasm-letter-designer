using SkiaSharp;
using Topten.RichTextKit;
using Topten.RichTextKit.Editor;

namespace WASMApp.Application.Text;

public class List
{
    IStyle _defaultStyle = StyleManager.Default.Value.DefaultStyle;
    
    private List<ListItem> _listItems;
    private bool _isLayoutValid;
    private int _measuredHeight;

    public float BulletSize { get; set; } = 10f;
    public float Spacing { get; set; } = 20f;
    
    public List()
    {
        _listItems = [];
    }

    public void AddListItem()
    {
        _listItems.Add(new ListItem(_defaultStyle, BulletType.Circle));
    }

    void Paint(SKCanvas canvas, int fromYCoord, TextPaintOptions options = null)
    {
        foreach (var listItem in _listItems)
        {
            listItem.Paint(canvas, options);
        }
    }
    
    void Layout(TextDocument owner)
    {
        if (_isLayoutValid)
            return;
        _isLayoutValid = true;
        
        var yCoord = 0;
        foreach (var listItem in _listItems)
        {
            listItem.Layout(owner);
                
            // listItem.ContentXCoord = 0;
            // listItem.ContentYCoord = yCoord;
            //
            // yCoord = (int)(listItem.ContentYCoord + listItem.ContentHeight);
        }

        _measuredHeight = yCoord;
    }
}