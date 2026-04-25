using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;


[System.Serializable]
public class DepthLayer
{
    public int minDepth;
    public int maxDepth;
    public RuleTile layerTile;
}

[System.Serializable]
public class oreType
{
    public string name;
    public RuleTile tile;
    public int clusters;
    public int clusterSize;
    [Range(0f, 1f)] public float branchChance;
    public int minDepth, maxDepth;
}


public class BlockGenerator : MonoBehaviour
{
    [Header("Generator settings")]
    [SerializeField] private int width = 200;
    [SerializeField] private int height = 200;
    [SerializeField] [Range(0f, 1f)] private float fillProb = 0.45f;
    [SerializeField] private int iterations = 5, birthLimit = 4, deathLimit = 4;

    [Header("Tile i tilemapa")]
    [SerializeField] Tilemap tilemap;
    [SerializeField] RuleTile tile;

    [SerializeField] DepthLayer[] depthLayers;


    [Header("Transitions")]
    public float jaggedness = 5f;
    public float jaggedScale = 0.1f;

    [Header("Ore settings")]
    [SerializeField] private Tilemap oreTilemap;
    [SerializeField] private oreType[] oreTypes;
    private int[,] oreGrid;

    private int[,] map;

    private void Start()
    {
        Generate();
        GenerateOres();
        DrawMap();
    }


    public void Generate()
    {
        InitMap();
        for (int i = 0; i < iterations; i++)
        {
            map = DoSimulationStep(map);
        }
        // Ensures that bottom-most layer is without gaps
        for (int x = 0; x < width; x++)
        {
            map[x, 0] = 1;
        }
    }

    private void InitMap()
    {
        map = new int [width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                map[x, y] = (Random.value < fillProb) ? 1 : 0;
            }
        }
    }

    int[,] DoSimulationStep(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for(int y = 0;y < height; y++)
            {
                int neighbours = CountWallNeighbours(oldMap, x, y);

                if (oldMap[x, y] == 1)
                {
                    newMap[x, y] = (neighbours < deathLimit) ? 0 : 1;

                }
                else
                {
                    newMap[x, y] = (neighbours > birthLimit) ? 1 : 0;
                }
            }
        }
        return newMap;
    }

    private int CountWallNeighbours(int[,] map, int x, int y)
    {
        int count = 0;
        for (int xc = x-1; xc <= x + 1; xc++)
        {
            for (int yc = y-1; yc <= y + 1; yc++)
            {
                if (x == xc && y == yc) continue;
                if (xc < 0 ||  yc < 0 || xc >= width || yc >= height) count++;
                else if (map[xc, yc] == 1) count++;
            }
        }
        return count;
    }
    float GetJaggedThreshold(int x, int baseDepth)
    {
        float noise = Mathf.PerlinNoise(x * jaggedScale, baseDepth * 0.1f) - 0.0f;
        return baseDepth + noise * jaggedness;
    }
    DepthLayer GetLayerForDepth(int x, int y)
    {
        foreach (var layer in depthLayers)
        {
            int minTh = Mathf.CeilToInt(GetJaggedThreshold(x, layer.minDepth));
            int maxTh = Mathf.FloorToInt(GetJaggedThreshold(x, layer.maxDepth));
            if (y >= minTh && y <= maxTh)
                return layer;
        }
        return null;
    }

    void DrawMap()
    {
        tilemap.ClearAllTiles();
        oreTilemap.ClearAllTiles();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    var layer = GetLayerForDepth(x, y);
                    if (layer != null)
                        tilemap.SetTile(pos, layer.layerTile);
                }
            }
        }
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (oreGrid != null && oreGrid[x, y] != 0)
                {
                    var pos = new Vector3Int(x, y, 0);
                    // finds ore by type
                    int i = 0;
                    foreach (var ore in oreTypes)
                    {
                        i++;
                        if (oreGrid[x, y] == i)
                        {
                            oreTilemap.SetTile(pos, ore.tile);
                        }
                    }
                }
    }

    #region Ores
    void GenerateOres()
    {
        // Picks random starting point for the walkers
        oreGrid = new int[width, height];
        int o = 0;
        foreach (var ore in oreTypes)
        {
            o++;
            for (int c = 0; c < ore.clusters; c++)
            {
                int x = Random.Range(0, width);
                int y = Random.Range(0, height);
                if (map[x,y] != 1 || y > ore.maxDepth || y < ore.minDepth)
                {
                    c--;
                    continue;
                }

                // Creates walkers
                List<Vector2Int> walkers = new List<Vector2Int> { new Vector2Int(x, y) };

                int steps = ore.clusterSize;
                int maxBranches = Mathf.CeilToInt(ore.branchChance * steps);

                while (steps-- > 0 && walkers.Count > 0)
                {
                    // Picks random walker
                    int i = Random.Range(0, walkers.Count);
                    var pos = walkers[i];
                    oreGrid[pos.x, pos.y] = o;

                    // Branching of walker
                    if(Random.value < ore.branchChance && walkers.Count < maxBranches)
                    {
                        walkers.Add(pos);
                    }

                    // Random movement
                    Vector2Int[] directions = {Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right};
                    var dir = directions[Random.Range(0, directions.Length)];
                    var np = pos + dir;

                    // Check if out of bounds
                    if (np.x >= width || np.x < 0 || np.y >= height || np.y < 0 || map[np.x,np.y] != 1 || np.y > ore.maxDepth || np.y < ore.minDepth)
                    {
                        walkers.RemoveAt(i);
                        continue;
                    }
                    walkers[i] = np;
                }
            }    
        }
    }

    #endregion

}
