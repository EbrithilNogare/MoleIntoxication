using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class EngineConnector : MonoBehaviour
{
    public Tilemap tilemapRoots;
    public Tilemap tilemapResources;
    public TileBase tileDarkness;
    public TileBase tileRoots;
    public TileBase tileMetalSource;
    public TileBase tileEnergySource;
    public TileBase tileWaterSource;
    public TileBase tileToxin;

    public GameObject moleGO;

    private GameEngine engine;

    void Start()
    {
        engine = new GameEngine(20, 0);
    }

    void Update()
    {
        float cameraHeight = Camera.main.orthographicSize;
        int minY = (int)(-Camera.main.transform.position.y - cameraHeight) - 3;
        int maxY = (int)(-Camera.main.transform.position.y + cameraHeight) + 3;

        Debug.Log("ENGINE   water: " + engine.availableWater + "   energy: " + engine.availableEnergy + "   metal: " + engine.availableMetal);

        engine.Tick(Time.deltaTime);

        for (int y = System.Math.Max(0, minY); y < System.Math.Min(100, maxY); y++)
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                // Select right tiles
                TileBase rootTile = null;
                TileBase resourceTile = null;
                switch (engine.map[y][x].type)
                {
                    case MapTileType.WaterSource: resourceTile = tileWaterSource; break;
                    case MapTileType.EnergySource: resourceTile = tileEnergySource; break;
                    case MapTileType.MetalSource: resourceTile = tileMetalSource; break;
                }
                if (!engine.map[y][x].IsVisible)
                    rootTile = tileDarkness;
                else if (engine.map[y][x].HasRoots)
                    rootTile = tileRoots;

                // render resources with alpha as depletion ratio
                tilemapResources.SetTile(new Vector3Int(x, -y, 0), resourceTile);
                if (resourceTile != null)
                {
                    tilemapResources.SetTileFlags(new Vector3Int(x, -y, 0), TileFlags.None);
                    tilemapResources.SetColor(new Vector3Int(x, -y, 0), new Color(1, 1, 1, engine.map[y][x].currentResources / engine.map[y][x].capacityResources));
                }

                // render roots and darkness (and sync animation frames
                tilemapRoots.SetTile(new Vector3Int(x, -y, 0), rootTile);
                if (rootTile == tileDarkness)
                {
                    tilemapRoots.SetAnimationFrame(new Vector3Int(x, -y, 0), tilemapRoots.GetAnimationFrame(new Vector3Int(-1, -y, 0)));
                }
            }
        }
    }

    [MyBox.ButtonMethod]
    public void TestMole()
    {
        Mole m = new Mole(4, engine, moleGO.transform);
    }

    public void Click(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(-1)) return;
        Vector2 position = Mouse.current.position.ReadValue();
        ResolveClick(position);
    }

    public void Touch(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) return;
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        ResolveClick(touchPosition);
    }

    public void ResolveClick(Vector2 position)
    {
        float cameraSize = Camera.main.orthographicSize;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, cameraSize));
        int x = (int)System.Math.Round(worldMousePosition.x) + (int)(Constants.MAP_WIDTH / 2);
        int y = -(int)System.Math.Round(worldMousePosition.y + .5);

        if (y < 0 || x < 0 || x >= Constants.MAP_WIDTH)
            return;

        // TODO remove roots removing method
        if (!engine.map[y][x].HasRoots)
            engine.ClickOn_Map(x, y);
        else
            engine.RemoveRoots(x, y);
    }
    public void Scroll(InputAction.CallbackContext context)
    {
        float newY = Camera.main.transform.position.y + context.ReadValue<float>() / 500f;

        newY = System.Math.Clamp(newY, -80, 0); // todo better than 80

        Camera.main.transform.position = new Vector3(
           Camera.main.transform.position.x,
           newY,
           Camera.main.transform.position.z
       );
    }
}
