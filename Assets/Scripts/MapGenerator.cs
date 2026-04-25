using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private CaveGenerator caveGenerator;
    [SerializeField] private OreGenerator oreGenerator;
    [SerializeField] private MapRenderer mapRenderer;
    [SerializeField] private int? mapSeed;

    private void Start()
    {
        if (mapSeed is null) mapSeed = System.Environment.TickCount;
        var rng  = new System.Random((int)mapSeed);
        GenerateMap(rng);
    }

    private void GenerateMap(System.Random rng)
    {
        var cellMap = caveGenerator.Generate(rng);
        var oreMap = oreGenerator.GenerateOres(cellMap, rng);
        var oreTypes = oreGenerator.OreTypes;
        mapRenderer.DrawMap(cellMap, oreMap, oreTypes, rng);
    }
}