using UnityEngine;

[System.Serializable]
public class CaveGenerator
{
    [Header("Generator Settings")]
    [field: SerializeField]
    public Vector2Int MapSize { get; private set; }

    [SerializeField] [Range(0f, 1f)] private float fillProbability;
    [SerializeField] private int iterations = 5, birthCeiling = 4, survivalThreshold = 4;

    private bool[,] cellMap;

    /// <summary>
    /// Generates a cave map using cellular automata simulation.
    /// Initialises a random grid, runs the simulation for the configured number of iterations,
    /// then seals the bottom border to prevent open edges.
    /// </summary>
    /// <param name="rng">Seeded random instance shared across the generation pipeline.</param>
    /// <returns>2D bool grid where true = solid tile, false = empty space.</returns>
    public bool[,] Generate(System.Random rng)
    {
        InitCellMap(rng);
        for (int i = 0; i < iterations; i++)
        {
            DoSimulationStep(cellMap);
        }

        // Ensure there are no gaps in the bottom map border
        for (int x = 0; x < MapSize.x; x++)
        {
            cellMap[x, 0] = true;
        }

        return cellMap;
    }

    private void InitCellMap(System.Random rng)
    {
        cellMap = new bool[MapSize.x, MapSize.y];
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int y = 0; y < MapSize.y; y++)
            {
                cellMap[x, y] = (rng.NextDouble() < fillProbability);
            }
        }
    }

    private void DoSimulationStep(bool[,] map)
    {
        bool[,] newMap = new bool [MapSize.x, MapSize.y];
        for (int x = 1; x < MapSize.x - 1; x++)
        {
            for (int y = 1; y < MapSize.y - 1; y++)
            {
                int neighbors = CountCellNeighbors(map, x, y);
                if (map[x, y])
                {
                    newMap[x, y] = neighbors > survivalThreshold;
                }
                else
                {
                    newMap[x, y] = neighbors < birthCeiling;
                }
            }
        }

        cellMap = newMap;
    }

    private static int CountCellNeighbors(bool[,] map, int cellX, int cellY)
    {
        int neighbors = 0;
        for (int x = cellX - 1; x <= cellX + 1; x++)
        {
            for (int y = cellY - 1; y <= cellY + 1; y++)
            {
                if (x == cellX && y == cellY) continue;
                if (map[x, y]) neighbors++;
            }
        }

        return neighbors;
    }
}