using System.Collections.Generic;

public class GameEngine
{
    // Map
    public List<MapTile[]> map;  //List<Tile[]> map; where Tile is a struct with variables Is Root Here, Is Bomb Here, etc.(?)
    private List<int[]> directions;

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

    // Mole mushroom
    public int moleMushMetalPrice = 0;
    public int moleMushWaterPrice = 0;
    private bool IsMoleMushBought = false;
    private bool IsMoleMushEnergized = false;

    // Bomb
    public int bombMetalPrice = 0;
    public int bombWaterPrice = 0;
    public int bombEnergyPrice = 0;

    public GameEngine(int startWater, int startMetal)
    {
        this.availableMetal = startMetal;
        this.availableWater = startWater;
        this.directions = new List<int[]> { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
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
        if (availableWater >= rootPrice && map[y][x].IsVisible && !map[y][x].HasRoots)
        {
            map[y][x].HasRoots = true;
            MakeNeighborsVisible(x, y);
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
    public void RemoveRoots(int x, int y)
    {
        if (!map[y][x].HasRoots)
            throw new System.Exception("no roots");


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
        foreach (int[] direction in directions)
        {
            int newX = centerX + direction[1];
            int newY = centerY + direction[0];

            if (newX >= 0 && newX < map[0].Length &&
                newY >= 0 && newY < map.Count)
            {
                if (!map[newY][newX].IsVisible)
                {
                    map[newY][newX].IsVisible = true;
                }
            }
        }

    }
}
