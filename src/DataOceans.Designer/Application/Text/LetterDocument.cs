using WASMApp.Application.Editor.Elements;

namespace WASMApp.Application.Design;

public class LetterDocument
{
    private List<Region> _regions = [];

    public IReadOnlyList<Region> Regions => _regions;

    public void AddRegion(Region region)
    {
        _regions.Add(region);
    }

    public void RemoveRegion(Region region)
    {
        _regions.Remove(region);
    }
}