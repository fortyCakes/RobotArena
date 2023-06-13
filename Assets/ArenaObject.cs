using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArenaObject : MonoBehaviour
{
    public Vector2Int ArenaPosition;
    public Vector2 ArenaPositionOffset;
    private SpriteRenderer sr;
    private float height;
    private float width;

    private Tilemap Tilemap;
    public Vector2 FacingVector;
    private ArenaController arena;

    public virtual void Start()
    {
        arena = FindObjectOfType<ArenaController>();
        arena.RegisterObject(this);
        Tilemap = arena.tilemap;
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void Update()
    {
        transform.parent = Tilemap.gameObject.transform;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector2(FacingVector.x, FacingVector.y));

        height = sr.bounds.size.y;
        width = sr.bounds.size.x;
        transform.localPosition = Tilemap.CellToLocal(new Vector3Int(ArenaPosition.x, ArenaPosition.y, 0)) + new Vector3(width / 2, height / 2, -1)
            + new Vector3(ArenaPositionOffset.x, ArenaPositionOffset.y, 0);
    }

    public IEnumerator Move(Vector2 moveVector, bool hasForce = true, float speed = 1)
    {
        var moveResult = GetMoveResult(moveVector, hasForce); // Bounce, normal move, shove;

        float duration;
        float time;
        Vector2 initialPosition;
        Vector2 endPosition;
        ArenaObject shovedObject = null;

        switch(moveResult)
        {
            case MoveResult.Bounce:
                initialPosition = Vector2.zero;
                endPosition = moveVector * 0.2f;

                duration = 0.5f / speed;
                time = 0;
                while (time < duration)
                {
                    ArenaPositionOffset = new Vector2(Mathf.Lerp(initialPosition.x, endPosition.x, time / duration), Mathf.Lerp(initialPosition.y, endPosition.y, time / duration));
                    yield return null;
                    time += Time.deltaTime;
                }
                time = 0;
                while (time < duration)
                {
                    ArenaPositionOffset = new Vector2(Mathf.Lerp(endPosition.x, initialPosition.x,  time / duration), Mathf.Lerp(endPosition.y, initialPosition.y,  time / duration));
                    yield return null;
                    time += Time.deltaTime;
                }
                break;
            case MoveResult.Move:
                duration = 1f / speed;
                time = 0f;
                initialPosition = Vector2.zero;
                endPosition = moveVector;
                while (time < duration)
                {
                    ArenaPositionOffset = new Vector2(Mathf.Lerp(initialPosition.x, endPosition.x, time / duration), Mathf.Lerp(initialPosition.y, endPosition.y, time / duration));
                    yield return null;
                    time += Time.deltaTime;
                }
                ArenaPosition = ArenaPosition + moveVector.AsIntVector();
                break;
            case MoveResult.Shove:
                shovedObject = arena.RegisteredObjects.Single(o => o.ArenaPosition == ArenaPosition + moveVector);
                duration = 1f / speed;
                time = 0f;
                initialPosition = Vector2.zero;
                endPosition = moveVector;
                while (time < duration)
                {
                    ArenaPositionOffset = new Vector2(Mathf.Lerp(initialPosition.x, endPosition.x, time / duration), Mathf.Lerp(initialPosition.y, endPosition.y, time / duration));
                    shovedObject.ArenaPositionOffset = ArenaPositionOffset;
                    yield return null;
                    time += Time.deltaTime;
                }

                ArenaPosition = ArenaPosition + moveVector.AsIntVector();
                shovedObject.ArenaPosition = shovedObject.ArenaPosition + moveVector.AsIntVector();
                break;
        }

        ArenaPositionOffset = Vector2.zero;
        if (shovedObject != null)
        {
            shovedObject.ArenaPositionOffset = Vector2.zero;
        }

        yield return null;
    }

    public MoveResult GetMoveResult(Vector2 moveVector, bool hasForce = true)
    {
        var intendedLocation = this.ArenaPosition + moveVector.AsIntVector();

        var loc = new Vector3Int(ArenaPosition.x, ArenaPosition.y, 0);

        var blockedByWall = arena.arenaMap.CheckForWalls(loc, moveVector.AsIntVector());

        if (blockedByWall)
        {
            return MoveResult.Bounce;
        }

        // Check for objects blocking the way
        foreach (var arenaObject in arena.RegisteredObjects)
        {
            if (arenaObject.ArenaPosition == intendedLocation)
            {
                if (hasForce)
                {
                    // Try to shove if we can. HasForce is no longer true; you can't shove if the thing you're shoving is blocked.
                    var shove = arenaObject.GetMoveResult(moveVector, false);
                    if (shove == MoveResult.Bounce)
                        return MoveResult.Bounce;
                    else
                        return MoveResult.Shove;
                }
                else
                {
                    return MoveResult.Bounce;
                }
            }
        }

        return MoveResult.Move;
    }

    public IEnumerator RotateLeft()
    {
        var initialVector = FacingVector;
        var endVector = FacingVector.RotateLeft();
        float duration = 1;
        float time = 0;

        while (time < duration)
        {
            FacingVector = new Vector2(
                Mathf.Lerp(initialVector.x, endVector.x, time / duration),
                Mathf.Lerp(initialVector.y, endVector.y, time / duration));
            yield return null;
            time += Time.deltaTime;
        }
        FacingVector = endVector;
        yield return null;
    }
    public IEnumerator RotateRight()
    {
        var initialVector = FacingVector;
        var endVector = FacingVector.RotateRight();
        float duration = 1;
        float time = 0;

        while (time < duration)
        {
            FacingVector = new Vector2(
                Mathf.Lerp(initialVector.x, endVector.x, time / duration),
                Mathf.Lerp(initialVector.y, endVector.y, time / duration));
            yield return null;
            time += Time.deltaTime;
        }
        FacingVector = endVector;
        yield return null;
    }

}
