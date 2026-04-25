using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class MapRenderer
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap oreTilemap;
    [SerializeField] private DepthLayer[] depthLayers;
    [SerializeField] private float jaggedAmplitude;
    [SerializeField] private float jaggedFrequency;


    /// <summary>
    /// Clears and redraws both tilemaps from the provided cell and ore data.
    /// Depth-layer boundaries are perlin-warped using a random offset for visual variety.
    /// </summary>
    /// <param name="cellMap">2D grid where true = solid tile, false = empty space.</param>
    /// <param name="oreMap">2D grid of ore indices matching the oreTypes array (0 = no ore).</param>
    /// <param name="oreTypes">Ordered array of ore definitions used to resolve tile references.</param>
    /// <param name="rng">Seeded random instance used to generate the perlin warp offset.</param>
    public void DrawMap(bool[,] cellMap, int[,] oreMap, OreType[] oreTypes, System.Random rng)
    {
        tilemap.ClearAllTiles();
        oreTilemap.ClearAllTiles();

        int width = cellMap.GetLength(0);
        int height = cellMap.GetLength(1);

        var perlinOffsetX = (float)rng.NextDouble() * 10000;
        var perlinOffsetY = (float)rng.NextDouble() * 10000;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!cellMap[x, y]) continue;

                var layer = GetLayerForDepth(x, y, perlinOffsetX, perlinOffsetY);
                if (layer != null)
                    tilemap.SetTile(new Vector3Int(x, y, 0), layer.LayerTile);
            }
        }

        for (int x = 0; x < oreMap.GetLength(0); x++)
        {
            for (int y = 0; y < oreMap.GetLength(1); y++)
            {
                int oreIndex = oreMap[x, y];
                if (oreIndex == 0) continue;
                oreTilemap.SetTile(new Vector3Int(x, y, 0), oreTypes[oreIndex - 1].OreTile);
            }
        }
    }

    private float GetWarpedDepth(int x, int y, float perlinOffsetX = 0f, float perlinOffsetY = 0f)
    {
        float noise = Mathf.PerlinNoise((x + perlinOffsetX) * jaggedFrequency, (y + perlinOffsetY) * jaggedFrequency);
        return y + noise * jaggedAmplitude;
    }

    private DepthLayer GetLayerForDepth(int x, int y, float perlinOffsetX, float perlinOffsetY)
    {
        foreach (var layer in depthLayers)
        {
            int minThreshold = Mathf.CeilToInt(GetWarpedDepth(x, layer.MinDepth, perlinOffsetX, perlinOffsetY));
            int maxThreshold = Mathf.FloorToInt(GetWarpedDepth(x, layer.MaxDepth, perlinOffsetX, perlinOffsetY));
            if (y >= minThreshold && y <= maxThreshold)
                return layer;
        }

        return null;
    }
}

[System.Serializable]
public class DepthLayer
{
    [field: SerializeField] public int MinDepth { get; private set; }
    [field: SerializeField] public int MaxDepth { get; private set; }
    [field: SerializeField] public RuleTile LayerTile { get; private set; }
}