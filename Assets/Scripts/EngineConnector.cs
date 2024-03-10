using TMPro;
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

    public TMP_Text waterValue;
    public TMP_Text metalValue;
    public TMP_Text energyValue;
    public GameObject dangerZone;

    public GameObject LocatorMushroom;
    public GameObject SonarMushroom;
    public GameObject VladaMushroom;
    public GameObject SolarMushroom;
    public GameObject WaterMushroom;

    public GameObject moleGO;

    private GameEngine engine;
    private bool toxinPlacementMode;

    void Start()
    {
        engine = new GameEngine(100, 100, 100);
        toxinPlacementMode = false;
        RerenderMushrooms();
    }

    void Update()
    {
        float cameraHeight = Camera.main.orthographicSize;
        int minY = (int)(-Camera.main.transform.position.y - cameraHeight) - 3;
        int maxY = (int)(-Camera.main.transform.position.y + cameraHeight) + 3;

        // render GUI
        waterValue.SetText(((int)(engine.availableWater)).ToString());
        metalValue.SetText(((int)(engine.availableMetal)).ToString());
        energyValue.SetText(((int)(engine.availableEnergy)).ToString());

        // render mushrooms
        RerenderMushrooms();

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
        int moleHeight = 10;
        int precision = 10;
        float randomOffset = Random.Range(-precision / 2f, precision / 2f);

        dangerZone.transform.position = new Vector3(0, -System.Math.Max(precision / 2f, moleHeight + randomOffset + 1), transform.position.z); ;

        Vector3 scale = dangerZone.transform.localScale;
        scale.y = precision + 2;
        dangerZone.transform.localScale = scale;

        Mole m = new Mole(moleHeight, engine, moleGO.transform);
    }
    public void OnLocatorMushroomClick()
    {
        if (engine.IsLocatorMushBought)
            engine.ClickOn_LocatorMushOnOff();
        // todo make it glow on / off - also all others
        else
            engine.ClickOn_LocatorMushroom();
    }
    public void OnSonarMushroomClick()
    {
        if (engine.IsSonarMushBought)
            engine.ClickOn_SonarMushOnOff();
        else
            engine.ClickOn_SonarMushroom();
    }
    public void OnVladaMushroomClick()
    {
        if (!toxinPlacementMode && true)
        {
            toxinPlacementMode = true;
        }
        else
        {
            toxinPlacementMode = false;
        }
    }
    public void OnSolarMushroomClick()
    {
        if (engine.IsSolarMushBought)
            engine.ClickOn_SolarMushOnOff();
        else
            engine.ClickOn_SolarMushroom();
    }
    public void OnWaterMushroomClick()
    {

        if (engine.IsWaterMushBought)
            engine.ClickOn_WaterMushOnOff();
        else
            engine.ClickOn_WaterMushroom();
    }
    private void RerenderMushrooms()
    {
        Color notBuilt = new Color(1, 1, 1, .5f);
        Color notActive = new Color(.5f, .5f, .5f, 1);
        Color active = new Color(1, 1, 1, 1);

        if (engine.IsLocatorMushBought)
        {
            LocatorMushroom.GetComponent<SpriteRenderer>().color = engine.IsLocatorMushEnergized ? active : notActive;
        }
        else
        {
            LocatorMushroom.GetComponent<SpriteRenderer>().color = notBuilt;
        }

        if (engine.IsSonarMushBought)
        {
            SonarMushroom.GetComponent<SpriteRenderer>().color = engine.IsSonarMushEnergized ? active : notActive;
        }
        else
        {
            SonarMushroom.GetComponent<SpriteRenderer>().color = notBuilt;
        }

        if (engine.IsSolarMushBought)
        {
            SolarMushroom.GetComponent<SpriteRenderer>().color = active;
        }
        else
        {
            SolarMushroom.GetComponent<SpriteRenderer>().color = notBuilt;
        }

        if (engine.IsWaterMushBought)
        {
            WaterMushroom.GetComponent<SpriteRenderer>().color = engine.IsWaterMushEnergized ? active : notActive;
        }
        else
        {
            WaterMushroom.GetComponent<SpriteRenderer>().color = notBuilt;
        }
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

        if (y >= 0 && x >= 0 && x < Constants.MAP_WIDTH)
        {
            // map hit
            // TODO remove roots removing method
            if (!engine.map[y][x].HasRoots)
                engine.ClickOn_Map(x, y);
            else
                engine.RemoveRoots(x, y);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, ~LayerMask.NameToLayer("Mushrooms"));

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("LocatorMushroom"))
                    OnLocatorMushroomClick();
                else if (hit.collider.CompareTag("SonarMushroom"))
                    OnSonarMushroomClick();
                else if (hit.collider.CompareTag("VladaMushroom"))
                    OnVladaMushroomClick();
                else if (hit.collider.CompareTag("SolarMushroom"))
                    OnSolarMushroomClick();
                else if (hit.collider.CompareTag("WaterMushroom"))
                    OnWaterMushroomClick();
            }
        }
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
