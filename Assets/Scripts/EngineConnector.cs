using UnityEngine;
using UnityEngine.Tilemaps;

public class EngineConnector : MonoBehaviour
{
    public Tilemap tilemapRoots;
    public Tilemap tilemapResources;
    public Tile tile_darkness;
    public Tile tile_roots;
    public Tile tileMetalSource;
    public Tile tileEnergySource;
    public Tile tileWaterSource;
    public Tile tileToxin;

    private GameEngine engine;

    void Start()
    {
        engine = new GameEngine(20, 0);
    }

    void Update2()
    {
        for (int y = 0; y < 50; y++)
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                Tile rootTile = null;
                Tile resourceTile = null;

                tilemapRoots.SetTile(new Vector3Int(x, -y, 0), rootTile);
                tilemapResources.SetTile(new Vector3Int(x, -y, 0), resourceTile);
            }
        }
    }
}
