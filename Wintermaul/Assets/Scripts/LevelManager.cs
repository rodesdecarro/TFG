using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform map;

    public Point StartPoint { get; private set; }
    public Point GoalPoint { get; private set; }

    public Dictionary<Point, TileScript> Tiles { get; set; }

    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        int mapX = mapData[0].Length;
        int mapY = mapData.Length;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++)
        {
            for (int x = 0; x < mapX; x++)
            {
                PlaceTile((int)char.GetNumericValue(mapData[y][x]), x, y, worldStart);
            }
        }

        Vector3 maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
    }

    private string[] ReadLevelText()
    {
        TextAsset data = (TextAsset)Resources.Load("Level");

        return data.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    private void PlaceTile(int tileType, int x, int y, Vector3 worldStart)
    {
        if (tileType == 4)
        {
            StartPoint = new Point(x, y);
        }

        if (tileType == 2)
        {
            GoalPoint = new Point(x, y);
        }

        TileScript newTile = Instantiate(tilePrefabs[tileType]).GetComponent<TileScript>();

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + x * TileSize, worldStart.y - y * TileSize, 1), map);
    }
}
