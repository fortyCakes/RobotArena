using System.Linq;
using UnityEditor;
using UnityEngine.Tilemaps;
public sealed class TilesManager
{

    private Tile empty;
    private Tile conveyor1Left;
    private Tile conveyor1Straight;
    private Tile conveyor1Right;
    private Tile conveyor2Left;
    private Tile conveyor2Straight;
    private Tile conveyor2Right;
    private Tile pit;
    private Tile repair;
    private Tile rotateCW;
    private Tile rotateACW;
    private Tile upgrade;

    public static Tile Empty => Instance.empty;
    public static Tile Conveyor1Left => Instance.conveyor1Left;
    public static Tile Conveyor1Straight => Instance.conveyor1Straight;
    public static Tile Conveyor1Right => Instance.conveyor1Right;
    public static Tile Conveyor2Left => Instance.conveyor2Left;
    public static Tile Conveyor2Straight => Instance.conveyor2Straight;
    public static Tile Conveyor2Right => Instance.conveyor2Right;
    public static Tile Pit => Instance.pit;
    public static Tile Repair => Instance.repair;
    public static Tile RotateCW => Instance.rotateCW;
    public static Tile RotateACW => Instance.rotateACW;
    public static Tile Upgrade => Instance.upgrade;

    private static TilesManager instance;
    private static readonly object padlock = new object();

    public TilesManager()
    {
        empty = GetTileByLabel("Empty");
        conveyor1Left = GetTileByLabel("Conveyor1Left");
        conveyor1Straight = GetTileByLabel("Conveyor1Straight");
        conveyor1Right = GetTileByLabel("Conveyor1Right");
        conveyor2Left = GetTileByLabel("Conveyor2Left");
        conveyor2Straight = GetTileByLabel("Conveyor2Straight");
        conveyor2Right = GetTileByLabel("Conveyor2Right");
        pit = GetTileByLabel("Pit");
        repair = GetTileByLabel("Repair");
        rotateCW = GetTileByLabel("RotateCW");
        rotateACW = GetTileByLabel("RotateACW");
        upgrade = GetTileByLabel("Upgrade");

    }
    public static TilesManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new TilesManager();
                }
                return instance;
            }
        }
    }
    
    public static Tile GetTileAsset(TileType tileType)
    {
        switch(tileType)
        {
            case TileType.Empty:
                return Empty;
            case TileType.Conveyor1Left:
                return Conveyor1Left;
            case TileType.Conveyor1Right:
                return Conveyor1Right;
            case TileType.Conveyor1Straight:
                return Conveyor1Straight;
            case TileType.Conveyor2Left:
                return Conveyor2Left;
            case TileType.Conveyor2Right:
                return Conveyor2Right;
            case TileType.Conveyor2Straight:
                return Conveyor2Straight;
            case TileType.Pit:
                return Pit;
            case TileType.Repair:
                return Repair;
            case TileType.RotateAnticlockwise:
                return RotateACW;
            case TileType.RotateClockwise:
                return RotateCW;
            case TileType.Upgrade:
                return Upgrade;
            default: throw new System.Exception("Unknown TileType");
        }
    }

    private Tile GetTileByLabel(string assetLabel)
    {
        var assets = AssetDatabase.FindAssets("l:" + assetLabel);
        return (Tile)
            AssetDatabase.LoadAssetAtPath(
            AssetDatabase.GUIDToAssetPath(
            assets.First()), typeof(Tile));
    }
}
