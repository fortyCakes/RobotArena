using System.Collections.Generic;
using UnityEngine;

public class ArenaMap
{
    public Dictionary<Vector2Int, TileType> Tiles;
    public TileType defaultTile;

    public ArenaMap(TileType defaultTile = TileType.Pit)
    {
        this.defaultTile = defaultTile;
        Tiles = new Dictionary<Vector2Int, TileType>();
    }

    public TileType TileAt(Vector2Int loc) => Tiles.ContainsKey(loc) ? Tiles[loc] : defaultTile;
    public void SetTile(Vector2Int loc, TileType tile)
    {
        Tiles[loc] = tile;
    }
    public void EraseTile(Vector2Int loc)
    {
        if (Tiles.ContainsKey(loc))
        {
            Tiles.Remove(loc);
        }
    }
}