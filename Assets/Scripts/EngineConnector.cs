using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    public SpriteRenderer tileSelector;

    public GameObject moleGO;
    public Transform healthBarMole;

    private GameEngine engine;
    private bool toxinPlacementMode;

    private int ActualHealth = 100;
    private int moleTimeout = 50; // in actions
    private int moleHeight = 5;
    private bool moleMove = false;
    private int rootsToEat = 3;

    void Start()
    {
        engine = new GameEngine(20, 00, 00);
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

        //check death
        if (!engine.map[0][7].HasRoots || (engine.availableWater < 1 && (engine.availableEnergy < 1 || !engine.IsWaterMushBought)))
        {
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }

        if (moleTimeout == 5)
        {
            moleTimeout--;
            moleHeight = 0;
            for (int y = 0; y < engine.map.Count; y++)
                for (int x = 0; x < engine.map[0].Length; x++)
                    if (engine.map[y][x].HasRoots)
                        moleHeight = y;

            moleHeight = System.Math.Max(0, moleHeight + Random.Range(-5, 0));

            PrepareMole();
        }

        if (moleTimeout < 0)
        {
            moleTimeout = 20;
            TestMole();
        }

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
                    case MapTileType.Toxin: resourceTile = tileToxin; break;
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

    public void PrepareMole()
    {
        int precision = engine.IsLocatorMushBought && engine.IsLocatorMushEnergized ? 3 : 10;
        float randomOffset = Random.Range(-precision / 2f, precision / 2f);

        dangerZone.transform.position = new Vector3(-7.5f, -System.Math.Max(precision / 2f, moleHeight + randomOffset + 1), transform.position.z); ;

        dangerZone.GetComponent<SpriteRenderer>().size = new Vector2(.5f, precision + 2);
    }

    [MyBox.ButtonMethod]
    public void TestMole()
    {
        moleMove = true;
        Mole m = new Mole(moleHeight, engine, moleGO.transform, healthBarMole, ActualHealth, (x) => ActualHealth = x, () => moleMove = false, rootsToEat);
        rootsToEat += 1;
        dangerZone.transform.position = new Vector3(-17.5f, 0, transform.position.z); ;
    }
    public void OnLocatorMushroomClick()
    {
        if (engine.IsLocatorMushBought)
            engine.ClickOn_LocatorMushOnOff();
        // todo make it glow on / off - also all others
        else
        {
            SFXManager.Instance.PlayMushroomPurchase();
            engine.ClickOn_LocatorMushroom();
        }
    }
    public void OnSonarMushroomClick()
    {
        if (engine.IsSonarMushBought)
            engine.ClickOn_SonarMushOnOff();
        else
        {
            SFXManager.Instance.PlayMushroomPurchase();
            engine.ClickOn_SonarMushroom();
        }
    }
    public void OnVladaMushroomClick()
    {
        if (!toxinPlacementMode
            && engine.bombEnergyPrice <= engine.availableEnergy
            && engine.bombMetalPrice <= engine.availableMetal
            && engine.bombWaterPrice <= engine.availableWater
            )
        {
            toxinPlacementMode = true;
            tileSelector.color = new Color(206 / 255f, 39 / 255f, 181 / 255f);
        }
        else
        {
            toxinPlacementMode = false;
            tileSelector.color = Color.white;
        }
    }
    public void OnSolarMushroomClick()
    {
        if (engine.IsSolarMushBought)
            engine.ClickOn_SolarMushOnOff();
        else
        {
            SFXManager.Instance.PlayMushroomPurchase();
            engine.ClickOn_SolarMushroom();
        }
    }
    public void OnWaterMushroomClick()
    {

        if (engine.IsWaterMushBought)
            engine.ClickOn_WaterMushOnOff();
        else
        {
            SFXManager.Instance.PlayMushroomPurchase();
            engine.ClickOn_WaterMushroom();
        }
    }
    private void RerenderMushrooms()
    {
        Color notBuilt = new Color(1, 1, 1, .5f);
        Color notActive = new Color(.5f, .5f, .5f, 1);
        Color active = new Color(1, 1, 1, 1);

        if (engine.IsLocatorMushBought)
        {
            LocatorMushroom.GetComponent<SpriteRenderer>().color = engine.IsLocatorMushEnergized ? active : notActive;
            LocatorMushroom.GetComponent<Animator>().SetBool("playing", engine.IsLocatorMushEnergized);
        }
        else
        {
            LocatorMushroom.GetComponent<SpriteRenderer>().color = notBuilt;
        }

        if (engine.IsSonarMushBought)
        {
            SonarMushroom.GetComponent<SpriteRenderer>().color = engine.IsSonarMushEnergized ? active : notActive;
            SonarMushroom.GetComponent<Animator>().SetBool("playing", engine.IsSonarMushEnergized);
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
            WaterMushroom.GetComponent<Animator>().SetBool("playing", engine.IsWaterMushEnergized);
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
            if (toxinPlacementMode)
            {
                engine.ClickOn_Bomb(x, y);
                toxinPlacementMode = false;
                tileSelector.color = Color.white;
                moleTimeout--;
            }
            else
            {
                engine.ClickOn_Map(x, y);
                moleTimeout--;
            }
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
