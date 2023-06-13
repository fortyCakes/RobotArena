using Assets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArenaMap
{
    public TileType defaultTile;
    public Tilemap tilemap;
    public Tilemap wallmap;

    public ArenaMap(Tilemap tilemap, Tilemap wallmap, TileType defaultTile = TileType.Pit)
    {
        this.tilemap = tilemap;
        this.wallmap = wallmap;
        this.defaultTile = defaultTile;
    }

    public void SetTile(Vector2Int loc, TileType tile, int rotation = 0)
    {
        var pos = new Vector3Int(loc.x, loc.y, 0);

        tilemap.SetTile(pos, TilesManager.GetTileAsset(tile));
        tilemap.SetTileFlags(pos, TileFlags.LockColor);
        var transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, rotation), Vector3.one);
        tilemap.SetTransformMatrix(pos, transform);
    }

    public void EraseTile(Vector2Int loc)
    {
        var pos = new Vector3Int(loc.x, loc.y, 0);

        tilemap.SetTile(pos, null);
    }

    public bool CheckForWalls(Vector3Int loc, Vector2Int moveVector)
    {
        List<Vector2Int> blockedVectors = new List<Vector2Int>();

        var thisTileWalls = wallmap.GetTile(loc);
        var wallTileData = wallmap.GetTransformMatrix(loc);

        if (thisTileWalls != null)
        {
            switch (thisTileWalls.name)
            {
                case "wall":
                    blockedVectors.Add(new Vector2(0, 1).RotateBy(wallTileData.rotation.eulerAngles.z).AsIntVector());
                    break;
                case "wall_corner":
                    blockedVectors.Add(new Vector2(0, 1).RotateBy(wallTileData.rotation.eulerAngles.z).AsIntVector());
                    blockedVectors.Add(new Vector2(1, 0).RotateBy(wallTileData.rotation.eulerAngles.z).AsIntVector());
                    break;
                case "wall_bothsides":
                    blockedVectors.Add(new Vector2(0, 1).RotateBy(wallTileData.rotation.eulerAngles.z).AsIntVector());
                    blockedVectors.Add(new Vector2(0, -1).RotateBy(wallTileData.rotation.eulerAngles.z).AsIntVector());
                    break;
            }
        }


        var nextloc = loc + new Vector3Int(moveVector.x, moveVector.y, 0);
        var nextTile = wallmap.GetTile(nextloc);
        var nextwallTileData = wallmap.GetTransformMatrix(nextloc);

        if (nextTile != null)
        {
            switch (nextTile.name)
            {
                case "wall":
                    blockedVectors.Add(new Vector2(0, -1).RotateBy(nextwallTileData.rotation.eulerAngles.z).AsIntVector());
                    break;
                case "wall_corner":
                    blockedVectors.Add(new Vector2(0, -1).RotateBy(nextwallTileData.rotation.eulerAngles.z).AsIntVector());
                    blockedVectors.Add(new Vector2(-1, 0).RotateBy(nextwallTileData.rotation.eulerAngles.z).AsIntVector());
                    break;
                case "wall_bothsides":
                    blockedVectors.Add(new Vector2(0, 1).RotateBy(nextwallTileData.rotation.eulerAngles.z).AsIntVector());
                    blockedVectors.Add(new Vector2(0, -1).RotateBy(nextwallTileData.rotation.eulerAngles.z).AsIntVector());
                    break;
            }
        }

        return blockedVectors.Any(v => v == moveVector);
    }
}