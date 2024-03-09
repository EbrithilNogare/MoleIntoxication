using UnityEngine;
using UnityEngine.Tilemaps;

public class EngineConnector : MonoBehaviour
{
    public Tilemap tilemap;
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
                tilemap.SetTile(new Vector3Int(x, y, 0), MapTileToTile(engine.map[y][x]));
            }
        }
    }

    Tile MapTileToTile(MapTile mapTile)
    {
        switch (mapTile)
        {
            case MapTile.Darkness: return tiles[0];
            case MapTile.EmptyVisible: return null;
            case MapTile.Roots: return tiles[2];
            case MapTile.WaterSource: return tiles[3];
            case MapTile.MetalSource: return tiles[4];
            case MapTile.EnergySource: return tiles[5];
            case MapTile.Toxin: return tiles[6];
        }
        return tiles[0];
    }
}
