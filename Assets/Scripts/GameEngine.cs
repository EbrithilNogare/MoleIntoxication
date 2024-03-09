using System.Collections.Generic;

public class GameEngine
{
    // Map
    public List<MapTile[]> map;  //List<Tile[]> map; where Tile is a class with variables Is Root Here, Is Bomb Here, etc.(?)

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
    public bool IsBombPlaced;


    public GameEngine(int startWater, int startMetal)
    {
        this.availableMetal = startWater;
        this.availableWater = startMetal;
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
                    map[y][x] = MapTile.WaterSource;
                else if (randomPercentage < 15f)
                    map[y][x] = MapTile.MetalSource;
                else if (randomPercentage < 20f)
                    map[y][x] = MapTile.EnergySource;
                else
                    map[y][x] = MapTile.Darkness;
            }
        }

        map[0][7] = MapTile.Roots;
    }

    public void Tick()
    {
        UpdateEnergy();
        UpdateMetal();
        UpdateWater();
    }
    public void ClickOn_Map(int x, int y)
    {
        if (availableWater >= rootPrice)
        {
            map[x][y] = MapTile.Roots;
        }
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
            else
            {
                // Not enough energy!!!
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
            else
            {
                // Not enough energy!!!
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
            else
            {
                // Not enough energy!!!
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
            else
            {
                // Not enough energy!!!
            }
        }
    }
    public void ClickOn_SolarMushroom()
    {
        if (availableWater >= solarMushWaterPrice && availableMetal >= solarMushMetalPrice)
        {
            IsSolarMushBought = true;
        }
        else
        {
            // Is not enough resources!!!
        }
    }
    public void ClickOn_WaterMushroom()
    {
        if (availableWater >= waterMushWaterPrice && availableMetal >= waterMushMetalPrice)
        {
            IsWaterMushBought = true;
        }
        else
        {
            // Is not enough resources!!!
        }
    }
    public void ClickOn_LocatorMushroom()
    {
        if (availableWater >= locatorMushWaterPrice && availableMetal >= locatorMushMetalPrice)
        {
            IsLocatorMushBought = true;
        }
        else
        {
            // Is not enough resources!!!
        }
    }
    public void ClickOn_MoleMushroom()
    {
        if (availableWater >= moleMushWaterPrice && availableMetal >= moleMushMetalPrice)
        {
            IsMoleMushBought = true;
        }
        else
        {
            // Is not enough resources!!!
        }
    }
    public void ClickOn_Bomb()
    {
        if (availableWater >= bombWaterPrice && availableMetal >= bombMetalPrice && availableEnergy >= bombEnergyPrice)
        {
            IsBombPlaced = true;
        }
        else
        {
            // Is not enough resources!!!
        }
    }

    public void UpdateWater()
    {
        //TODO:
    }
    public void UpdateMetal()
    {
        //TODO:
    }
    public void UpdateEnergy()
    {
        //TODO:
    }
}

