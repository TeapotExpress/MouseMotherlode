using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OreGenerator
{
    [field: SerializeField] public OreType[] OreTypes  { get; private set; } 

    /// <summary>
    /// Places ore clusters in solid cave tiles using random walkers (drunkard walk).
    /// Each walker moves through adjacent wall tiles, depositing ore
    /// and occasionally branching to create natural-looking vein shapes.
    /// </summary>
    /// <param name="caveMap">2D grid where true = solid tile, false = empty space.</param>
    /// <param name="rng">Seeded random instance shared across the generation pipeline.</param>
    /// <returns>2D grid of ore indices, where 0 = no ore.</returns>
    public int[,] GenerateOres(bool[,] caveMap, System.Random rng)
    {
        int width = caveMap.GetLength(0);
        int height = caveMap.GetLength(1);
        int[,] oreGrid = new int[width, height];
        int oreIndex = 0;
        foreach (var ore in OreTypes)
        {
            oreIndex++;
            for (int i = 0; i < ore.ClusterCount; i++)
            {
                const int maxAttempts = 100;
                int attempts = 0;
                int x, y;
                do
                {
                    x = rng.Next(0, width);
                    y = rng.Next(0, height);
                    attempts++;
                } while (attempts < maxAttempts
                         && (!IsAtCorrectDepth(ore, y)
                             || !IsTilePopulated(caveMap, new Vector2Int(x, y))));

                if (attempts == maxAttempts) continue;

                var walkers = new List<Vector2Int> { new Vector2Int(x, y) };

                int remainingSteps = ore.ClusterSize;
                int maxBranches = Mathf.CeilToInt(ore.BranchChance * remainingSteps);

                while (remainingSteps-- > 0 && walkers.Count > 0)
                {
                    int walkerIndex = rng.Next(0, walkers.Count);
                    var walker = walkers[walkerIndex];
                    oreGrid[walker.x, walker.y] = oreIndex;

                    if (rng.NextDouble() < ore.BranchChance && walkers.Count <= maxBranches)
                    {
                        walkers.Add(walker);
                    }

                    var validNext = new List<Vector2Int>();
                    foreach (var dir in Directions)
                    {
                        var candidate = walker + dir;
                        if (IsWithinBounds(candidate, width, height)
                            && IsAtCorrectDepth(ore, candidate.y)
                            && IsTilePopulated(caveMap, candidate))
                            validNext.Add(candidate);
                    }

                    if (validNext.Count == 0)
                    {
                        walkers.RemoveAt(walkerIndex);
                        continue;
                    }

                    walkers[walkerIndex] = validNext[rng.Next(0, validNext.Count)];
                }
            }
        }

        return oreGrid;
    }

    private bool IsAtCorrectDepth(OreType ore, int depth) =>
        ore.MinDepth <= depth && depth <= ore.MaxDepth;


    private bool IsWithinBounds(Vector2Int position, int width, int height) =>
        position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;

    private bool IsTilePopulated(bool[,] caveMap, Vector2Int position) =>
        caveMap[position.x, position.y];

    private static readonly Vector2Int[] Directions =
        { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
}

[Serializable]
public class OreType
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public RuleTile OreTile { get; private set; }
    [field: SerializeField] public int ClusterCount { get; private set; }
    [field: SerializeField] public int ClusterSize { get; private set; }

    [field: SerializeField]
    [Range(0f, 1f)]
    public float BranchChance { get; private set; }

    [field: SerializeField] public int MinDepth { get; private set; }
    [field: SerializeField] public int MaxDepth { get; private set; }
}