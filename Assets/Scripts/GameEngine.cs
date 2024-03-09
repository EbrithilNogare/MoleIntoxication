using System.Collections.Generic;
using UnityEngine;

public class GameEngine
{
    // Map
    public List<MapTile[]> map;  //List<Tile[]> map; where Tile is a struct with variables Is Root Here, Is Bomb Here, etc.(?)
    private List<Vector2Int> directions;

    // Available resources  
    public int availableMetal;
    public int availableWater;
    public int availableEnergy;

    public int rootPrice = 1;

    // Water mushroom
    public int waterMushMetalPrice = 0;
    public int waterMushWaterPrice = 0;
    private bool IsWaterMushBought = false;
    private bool IsWaterMushEnergized = false;

    // Solar mushroom
    public int solarMushMetalPrice = 0;
    public int solarMushWaterPrice = 0;
    private bool IsSolarMushBought = false;
    private bool IsSolarMushEnergized = false;

    // Locator mushroom
    public int locatorMushMetalPrice = 0;
    public int locatorMushWaterPrice = 0;
    private bool IsLocatorMushBought = false;
    private bool IsLocatorMushEnergized = false;
    private int visibilityStrength = 3; // 1, 2, 3

    // Mole mushroom
    public int moleMushMetalPrice = 0;
    public int moleMushWaterPrice = 0;
    private bool IsMoleMushBought = false;
    private bool IsMoleMushEnergized = false;

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

    public void Tick()
    {
        UpdateResources();
    }
    public void ClickOn_Map(int x, int y)
    {
        if (availableWater >= rootPrice && !map[y][x].HasRoots && NearestRootDistance(x, y, 2) <= 1)
        {
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
        }
        else
        {
            // On
            if (availableEnergy >= 1)
            {
                IsSolarMushEnergized = !IsSolarMushEnergized;
            }
        }
    }
    public void ClickOn_WaterMushOnOff()
    {
        if (IsWaterMushEnergized)
        {
            // Off
            IsWaterMushEnergized = !IsWaterMushEnergized;
        }
        else
        {
            // On
            if (availableEnergy >= 1)
            {
                IsWaterMushEnergized = !IsWaterMushEnergized;
            }
        }
    }
    public void ClickOn_LocatorMushOnOff()
    {
        if (IsLocatorMushEnergized)
        {
            // Off
            IsLocatorMushEnergized = !IsLocatorMushEnergized;
        }
        else
        {
            //On
            if (availableEnergy >= 1)
            {
                IsLocatorMushEnergized = !IsLocatorMushEnergized;
            }
        }
    }
    public void ClickOn_MoleMushOnOff()
    {
        if (IsMoleMushEnergized)
        {
            // Off
            IsMoleMushEnergized = !IsMoleMushEnergized;
        }
        else
        {
            //On
            if (availableEnergy >= 1)
            {
                IsMoleMushEnergized = !IsMoleMushEnergized;
            }
        }
    }
    public void ClickOn_SolarMushroom()
    {
        if (availableWater >= solarMushWaterPrice && availableMetal >= solarMushMetalPrice)
        {
            IsSolarMushBought = true;
        }
    }
    public void ClickOn_WaterMushroom()
    {
        if (availableWater >= waterMushWaterPrice && availableMetal >= waterMushMetalPrice)
        {
            IsWaterMushBought = true;
        }
    }
    public void ClickOn_LocatorMushroom()
    {
        if (availableWater >= locatorMushWaterPrice && availableMetal >= locatorMushMetalPrice)
        {
            IsLocatorMushBought = true;
        }
    }
    public void ClickOn_MoleMushroom()
    {
        if (availableWater >= moleMushWaterPrice && availableMetal >= moleMushMetalPrice)
        {
            IsMoleMushBought = true;
        }
    }
    public void UpdateResources()
    {
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < Constants.MAP_WIDTH; x++)
            {
                if (map[y][x].HasRoots)
                {
                    switch (map[y][x].type)
                    {
                        case MapTileType.WaterSource:
                            map[y][x].currentResources -= 1;
                            break;
                        case MapTileType.MetalSource:
                            map[y][x].currentResources -= 1;
                            break;
                        case MapTileType.EnergySource:
                            map[y][x].currentResources -= 1;
                            break;
                        case MapTileType.Toxin:
                            map[y][x].currentResources -= 1;
                            break;
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
            {
                Debug.Log("here");
                if (x >= 0 && y >= 0 && x < map[0].Length && y < map.Count && Mathf.Abs(centerX - x) + Mathf.Abs(centerY - y) <= visibilityStrength)
                    map[y][x].IsVisible = true;
            }
    }
}
