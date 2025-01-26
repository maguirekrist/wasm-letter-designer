using SkiaSharp;
using Topten.RichTextKit;

namespace WASMApp.Application.Design;

public class List
{
    private List<ListItem> _listItems;
    private bool _isLayoutValid;
    private int _measuredHeight;

    public float BulletSize { get; set; } = 10f;
    public float Spacing { get; set; } = 20f;
    
    public List()
    {
        _listItems = [];
    }

    void Paint(SKCanvas canvas, int fromYCoord, TextPaintOptions options = null)
    {
        foreach (var listItem in _listItems)
        {
            listItem.Paint(canvas, options);
        }
    }
    
    void Layout(RegionText owner)
    {
        if (_isLayoutValid)
            return;
        _isLayoutValid = true;
        
        var yCoord = 0;
        foreach (var listItem in _listItems)
        {
            //listItem.Layout(this);

            listItem.ContentXCoord = 0;
            listItem.ContentYCoord = yCoord;

            yCoord = (int)(listItem.ContentYCoord + listItem.ContentHeight);
        }

        _measuredHeight = yCoord;
    }
}