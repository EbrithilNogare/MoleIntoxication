using UnityEngine;
using UnityEngine.Tilemaps;

public class EngineConnector : MonoBehaviour
{
    public Tilemap tilemapForeground;
    public Tilemap tilemapBackground;
    public Tile[] tiles;

    private GameEngine engine;

    void Start()
    {
        engine = new GameEngine(20, 0);
    }

    void Update()
    {
        for (int y = 0; y < 50; y++)
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                tilemapForeground.SetTile(new Vector3Int(x, y, 0), MapTileToTile(engine.map[y][x]));
            }
        }
    }

    Tile MapTileToTile(MapTile mapTile)
    {
        if(!mapTile.IsVisible)
        {
            return tiles[0];
        }
        switch (mapTile.type)
        {
            case MapTileType.Empty: return null;
            case MapTileType.WaterSource: return tiles[3];
            case MapTileType.MetalSource: return tiles[4];
            case MapTileType.EnergySource: return tiles[5];
            case MapTileType.Toxin: return tiles[6];
        }
        return tiles[0];
    }
}
