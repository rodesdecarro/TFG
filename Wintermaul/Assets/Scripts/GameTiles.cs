using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
    public static GameTiles instance;
    public Tilemap Tilemap;

    public Dictionary<Vector3, WorldTile> tiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        GetWorldTiles();
    }

    // Use this for initialization
    private void GetWorldTiles()
    {
        tiles = new Dictionary<Vector3, WorldTile>();

        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
        {
            // Exclude map borders
            if (pos.x < -4 || pos.x > 4 || pos.y > 2 || pos.y < -40) continue;

            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = Tilemap.CellToWorld(localPlace),
                TileBase = Tilemap.GetTile(localPlace),
                TilemapMember = Tilemap,
                Name = localPlace.x + "," + localPlace.y,
                Buildable = true,
                Cost = 1
            };

            // Startregion
            if (pos.x > -3 && pos.x < 3 && pos.y > -3 && pos.y < 3)
            {
                tile.Buildable = false;
            }

            // Endregion
            if (pos.x > -3 && pos.x < 3 && pos.y > -41 && pos.y < -35)
            {
                tile.Buildable = false;
            }

            tiles.Add(tile.WorldLocation, tile);
        }
    }
}