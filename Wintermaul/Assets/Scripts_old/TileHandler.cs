using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHandler : MonoBehaviour
{
    private WorldTile _tile;

    // Update is called once per frame
    private void Update()
    {
        // Reset previous color
        if (_tile != null)
        {
            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.white);
        }

        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);

        var tiles = GameTiles.instance.tiles; // This is our Dictionary of tiles

        if (tiles.TryGetValue(worldPoint, out _tile))
        {
            print("Tile " + _tile.Name + " costs: " + _tile.Cost);
            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);

            if (_tile.Buildable)
            {
                _tile.TilemapMember.SetColor(_tile.LocalPlace, new Color(0.5f, 1f, 0.5f, 1f));
            }
            else
            {
                _tile.TilemapMember.SetColor(_tile.LocalPlace, new Color(1f, 0.5f, 0.5f, 1f));
            }
        }
    }
}