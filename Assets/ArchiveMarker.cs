using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArchiveMarker : MonoBehaviour
{
    public ProgrammableRobot Bot;
    public Vector2Int ArenaPosition;

    private SpriteRenderer sr;
    private float height;
    private float width;
    private Tilemap Tilemap;
    private ArenaController arena;

    public virtual void Start()
    {
        arena = FindObjectOfType<ArenaController>();
        Tilemap = arena.tilemap;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent = Tilemap.gameObject.transform;

        height = sr.bounds.size.y;
        width = sr.bounds.size.x;
        transform.localPosition = Tilemap.CellToLocal(new Vector3Int(ArenaPosition.x, ArenaPosition.y, 0)) + new Vector3(width / 2, height / 2, -5);
    }
}
