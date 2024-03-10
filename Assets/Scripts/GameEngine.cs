using System.Collections.Generic;
using UnityEngine;

public class GameEngine
{
    // Map
    public List<MapTile[]> map;  //List<Tile[]> map; where Tile is a struct with variables Is Root Here, Is Bomb Here, etc.(?)
    private List<Vector2Int> directions;

    // Available resources  
    public float availableMetal;
    public float availableWater;
    public float availableEnergy;

    public int rootPrice = 1;

    // All mushrooms(except of Vlada and solar one)
    public int energyOnOffMushPrice = 1;

    // Water mushroom
    public int waterMushMetalPrice = 0;
    public int waterMushWaterPrice = 0;
    public bool IsWaterMushBought = false;
    public bool IsWaterMushEnergized = false;

    // Solar mushroom
    public int solarMushMetalPrice = 0;
    public int solarMushWaterPrice = 0;
    public bool IsSolarMushBought = false;
    public bool IsSolarMushEnergized = false;

    // Locator mushroom
    public int locatorMushMetalPrice = 0;
    public int locatorMushWaterPrice = 0;
    public bool IsLocatorMushBought = false;
    public bool IsLocatorMushEnergized = false;
    private int visibilityStrength = 1; // 1, 2, 3

    // Sonar mushroom
    public int sonarMushMetalPrice = 0;
    public int sonarMushWaterPrice = 0;
    public bool IsSonarMushBought = false;
    public bool IsSonarMushEnergized = false;

    // Bomb
    public int bombMetalPrice = 0;
    public int bombWaterPrice = 0;
    public int bombEnergyPrice = 0;

    private int deepestOptimalization = 5;

    public GameEngine(int startWater, int startMetal)
    {
        this.availableMetal = startMetal;
        this.availableWater = startWater;
        this.directions = new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(-1, 0) };
        GenerateMap();
    }

    private void GenerateMap()
    {
        map = new List<MapTile[]>();
        for (int y = 0; y < 100; y++)
        {
            map.Add(new MapTile[Constants.MAP_WIDTH]);
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                var randomPercentage = UnityEngine.Random.Range(0f, 100f);
                if (randomPercentage < 10f)
                {
                    map[y][x].type = MapTileType.WaterSource;
                    map[y][x].capacityResources = 10;
                }
                else if (randomPercentage < 15f)
                {
                    map[y][x].type = MapTileType.MetalSource;
                    map[y][x].capacityResources = 10;
                }
                else if (randomPercentage < 20f)
                {
                    map[y][x].type = MapTileType.EnergySource;
                    map[y][x].capacityResources = 10;
                }
                else
                {
                    map[y][x].type = MapTileType.Empty;
                    map[y][x].capacityResources = 0;
                }
                map[y][x].IsVisible = false;
                map[y][x].HasRoots = false;
                map[y][x].currentResources = map[y][x].capacityResources;
            }
        }

        map[0][7].type = MapTileType.Empty;
        map[0][7].IsVisible = true;
        map[0][7].HasRoots = true;

        MakeNeighborsVisible(7, 0);
    }

    public void Tick(float timeDelta)
    {
        UpdateResources(timeDelta);
    }
    public void ClickOn_Map(int x, int y)
    {
        if (availableWater >= rootPrice && !map[y][x].HasRoots && NearestRootDistance(x, y, 2) <= 1)
        {
            availableWater -= rootPrice;
            map[y][x].HasRoots = true;

            MakeNeighborsVisible(x, y);
            if (deepestOptimalization < y + 5)
                deepestOptimalization = y + 5;
        }
    }
    public void ClickOn_Bomb(int x, int y)
    {
        if (availableWater >= bombWaterPrice && availableMetal >= bombMetalPrice && availableEnergy >= bombEnergyPrice)
        {
            if (map[y][x].HasRoots)
            {
                map[y][x].type = MapTileType.Toxin;

                availableEnergy -= bombEnergyPrice;
                availableMetal -= bombMetalPrice;
                availableWater -= bombWaterPrice;
            }
        }
    }
    private int NearestRootDistance(int x, int y, int maxDepth = 5)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(new Vector2Int(x, y));
        visited.Add(new Vector2Int(x, y));

        int[] maxDepthManhatan = { 1, 5, 13, 25, 41, 61 };

        while (queue.Count > 0 && queue.Count < maxDepthManhatan[maxDepth])
        {
            Vector2Int current = queue.Dequeue();

            if (map[current.y][current.x].HasRoots)
            {
                return Mathf.Abs(current.x - x) + Mathf.Abs(current.y - y);
            }

            foreach (Vector2Int direction in directions)
            {
                Vector2Int next = current + direction;

                if (next.x >= 0 && next.x < map[0].Length &&
                    next.y >= 0 && next.y < map.Count &&
                    !visited.Contains(next))
                {
                    queue.Enqueue(next);
                    visited.Add(next);
                }
            }
        }

        return 100;
    }
    public void RemoveRoots(int x, int y)
    {
        if (!map[y][x].HasRoots)
            Debug.LogError("no roots");

        map[y][x].HasRoots = false;

        foreach (Vector2Int direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            if (newX >= 0 && newX < map[0].Length &&
                newY >= 0 && newY < map.Count)
            {
                if (map[newY][newX].HasRoots)
                {
                    List<Vector2Int> rootsOnWay = FindRoots(newX, newY);

                    bool containsMainRoot = rootsOnWay.Exists(item => item.y == 0 && item.x == 7);

                    if (!containsMainRoot)
                    {
                        foreach (Vector2Int rootCoordinates in rootsOnWay)
                        {
                            map[rootCoordinates.y][rootCoordinates.x].HasRoots = false;
                        }

                        RecomputeVisibility();
                    }
                }
            }
        }
    }
    public void RecomputeVisibility()
    {
        for (int y = 0; y < System.Math.Min(deepestOptimalization, map.Count); y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                map[y][x].IsVisible = NearestRootDistance(x, y, visibilityStrength) <= visibilityStrength;
            }
        }
    }
    public List<Vector2Int> FindRoots(int x, int y)
    {
        List<Vector2Int> alreadyVisited = new List<Vector2Int>();
        List<Vector2Int> toVisit = new List<Vector2Int> { new Vector2Int(x, y) };

        while (toVisit.Count > 0)
        {
            foreach (Vector2Int direction in directions)
            {
                var directionToProbe = toVisit[0] + direction;

                if (directionToProbe.x >= 0
                    && directionToProbe.x < map[0].Length
                    && directionToProbe.y >= 0
                    && directionToProbe.y < map.Count
                    && map[directionToProbe.y][directionToProbe.x].HasRoots
                    && !alreadyVisited.Exists(item => item.y == directionToProbe.y && item.x == directionToProbe.x)
                    && !toVisit.Exists(item => item.y == directionToProbe.y && item.x == directionToProbe.x)
                )
                {
                    toVisit.Add(directionToProbe);
                }
            }
            alreadyVisited.Add(toVisit[0]);
            toVisit.RemoveAt(0);
        }

        return alreadyVisited;
    }
    public void ClickOn_SolarMushOnOff()
    {
        if (IsSolarMushEnergized)
        {
            // Off
            IsSolarMushEnergized = !IsSolarMushEnergized;
            availableEnergy += energyOnOffMushPrice;
        }
        else
        {
            // On
            if (availableEnergy >= 1)
            {
                IsSolarMushEnergized = !IsSolarMushEnergized;
                availableEnergy -= energyOnOffMushPrice;
            }
        }
    }
    public void ClickOn_WaterMushOnOff()
    {
        if (IsWaterMushEnergized)
        {
            // Off
            IsWaterMushEnergized = !IsWaterMushEnergized;
            availableEnergy += energyOnOffMushPrice;
        }
        else
        {
            // On
            if (availableEnergy >= 1)
            {
                IsWaterMushEnergized = !IsWaterMushEnergized;
                availableEnergy -= energyOnOffMushPrice;
            }
        }
    }
    public void ClickOn_LocatorMushOnOff()
    {
        if (IsLocatorMushEnergized)
        {
            // Off
            IsLocatorMushEnergized = !IsLocatorMushEnergized;
            visibilityStrength = 2;
            availableEnergy += energyOnOffMushPrice;
        }
        else
        {
            //On
            if (availableEnergy >= 1)
            {
                IsLocatorMushEnergized = !IsLocatorMushEnergized;
                visibilityStrength = 3;
                availableEnergy -= energyOnOffMushPrice;
            }
        }
    }
    public void ClickOn_MoleMushOnOff()
    {
        if (IsSonarMushEnergized)
        {
            // Off
            IsSonarMushEnergized = !IsSonarMushEnergized;
            availableEnergy += energyOnOffMushPrice;
        }
        else
        {
            //On
            if (availableEnergy >= 1)
            {
                IsSonarMushEnergized = !IsSonarMushEnergized;
                availableEnergy -= energyOnOffMushPrice;
            }
        }
    }
    public void ClickOn_SolarMushroom()
    {
        if (availableWater >= solarMushWaterPrice && availableMetal >= solarMushMetalPrice)
        {
            availableWater -= solarMushWaterPrice;
            availableMetal -= solarMushMetalPrice;

            IsSolarMushBought = true;
        }
    }
    public void ClickOn_WaterMushroom()
    {
        if (availableWater >= waterMushWaterPrice && availableMetal >= waterMushMetalPrice)
        {
            availableWater -= waterMushWaterPrice;
            availableMetal -= waterMushMetalPrice;

            IsWaterMushBought = true;
        }
    }
    public void ClickOn_LocatorMushroom()
    {
        if (availableWater >= locatorMushWaterPrice && availableMetal >= locatorMushMetalPrice)
        {
            availableWater -= locatorMushWaterPrice;
            availableMetal -= locatorMushMetalPrice;

            IsLocatorMushBought = true;
            visibilityStrength = 2;
        }
    }
    public void ClickOn_SonarMushroom()
    {
        if (availableWater >= sonarMushWaterPrice && availableMetal >= sonarMushMetalPrice)
        {
            availableWater -= sonarMushWaterPrice;
            availableMetal -= sonarMushMetalPrice;
            IsSonarMushBought = true;
        }
    }
    public void UpdateResources(float timeDelta)
    {
        for (int y = 0; y < System.Math.Min(deepestOptimalization, map.Count); y++)
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                if (map[y][x].HasRoots)
                {
                    switch (map[y][x].type)
                    {
                        case MapTileType.WaterSource:
                            map[y][x].currentResources -= timeDelta;
                            availableWater += timeDelta;
                            break;
                        case MapTileType.MetalSource:
                            map[y][x].currentResources -= timeDelta;
                            availableMetal += timeDelta;
                            break;
                        case MapTileType.EnergySource:
                            map[y][x].currentResources -= timeDelta;
                            availableEnergy += timeDelta;
                            break;
                        case MapTileType.Toxin:
                            map[y][x].currentResources -= timeDelta;
                            break;
                    }
                    if (map[y][x].currentResources <= 0)
                    {
                        map[y][x].type = MapTileType.Empty;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Makes neighboring cells visible relative to a given central cell with coordinates map[y][x] 
    /// Parameters: (x, y)
    /// </summary>
    public void MakeNeighborsVisible(int centerX, int centerY)
    {
        for (int x = centerX - 3; x <= centerX + 3; x++)
            for (int y = centerY - 3; y <= centerY + 3; y++)
                if (x >= 0 && y >= 0 && x < map[0].Length && y < map.Count && Mathf.Abs(centerX - x) + Mathf.Abs(centerY - y) <= visibilityStrength)
                    map[y][x].IsVisible = true;
    }
}
