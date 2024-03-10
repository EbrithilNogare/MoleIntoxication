public enum MapTileType
{
    Empty,
    WaterSource,
    MetalSource,
    EnergySource,
    Toxin
}
public struct MapTile
{
    public MapTileType type;
    public bool IsVisible;
    public bool HasRoots;

    public float capacityResources;
    public float currentResources;
}