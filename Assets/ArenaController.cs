using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * TODO: 
 * Archive markers.
 * Pits kill you and respawn at archive.
 * Health.
 * Lasers.
 * Locked Registers/Less cards.
 * Random AI bots.
 * Fastforward resolution.
 * Flags.
 * Camera pan/zoom.
 */

public class ArenaController : MonoBehaviour
{
    private const bool FORCE_CARDS_IN_ALL_SLOTS = true;

    public Tilemap tilemap;
    public Tilemap wallmap;
    private Grid grid;
    public ArenaMap arenaMap;
    CardZone deck;
    CardZone discard;
    public CardZone playerHand;
    public List<ProgrammableRobot> RegisteredBots;
    public List<ArenaObject> RegisteredObjects;

    public bool isExecuting;


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();

        arenaMap = GenerateArena();

        deck = CardsManager.GenerateDeck();
        deck.Shuffle();

        var discardGameObject = new GameObject("Discard", typeof(CardZone));

        discard = discardGameObject.GetComponent<CardZone>();

        deck.ReshuffleWhenEmpty = discard;

        playerHand.DrawFrom(deck, 9);
    }
    public void RegisterObject(ArenaObject arenaObject)
    {
        RegisteredObjects.Add(arenaObject);
    }
    public void DeregisterObject(ArenaObject arenaObject)
    {
        RegisteredObjects.Remove(arenaObject);
    }

    public void StartExecutingPrograms()
    {
        StartCoroutine(ExecutePrograms());
    }

    public IEnumerator ExecutePrograms()
    {
        isExecuting = true;

        if (FORCE_CARDS_IN_ALL_SLOTS)
        {
            foreach (var bot in RegisteredBots)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (bot.Program.CardInSlot(i) == null)
                    {
                        bot.Program.DrawFrom(deck, 1);
                    }
                }
            }
        }

        // One register at a time
        for (int i = 0; i < 5; i++)
        {
            yield return ExecuteProgramCards(i);
            yield return ArenaElementsMove(i);
            yield return FireLasers();
            yield return TouchObjectives();
        }

        DiscardAndDraw();

        isExecuting = false;
    }

    private IEnumerator ExecuteProgramCards(int i)
    {
        Dictionary<CardBase, ProgrammableRobot> registerCards = new Dictionary<CardBase, ProgrammableRobot>();
        foreach (var bot in RegisteredBots)
        {
            var card = bot.Program.CardInSlot(i);

            if (card != null)
            {
                registerCards.Add(card, bot);
            }
        }

        var cards = new List<CardBase>(registerCards.Keys);
        cards.OrderByDescending(c => c.Priority);

        foreach (CardBase card in cards)
        {
            yield return card.Execute(registerCards[card]);
            card.MoveTo(discard);
        }

        yield return null;
    }

    private void DiscardAndDraw()
    {
        foreach (var card in playerHand.AllCards.ToList())
        {
            card.GetComponent<CardBase>().MoveTo(discard);
        }

        playerHand.DrawFrom(deck, 9);
    }

    

    private IEnumerator ArenaElementsMove(int i)
    {
        Console.WriteLine("Arena elements move");

        // first move for just express conveyors
        foreach (ArenaObject arenaObject in RegisteredObjects)
        {
            Vector3Int arenaPosition = arenaObject.ArenaPosition.ToVector3Int();
            var tileType = GetTileType(arenaObject);
            switch (tileType)
            {
                case TileType.Conveyor2Left:
                case TileType.Conveyor2Right:
                case TileType.Conveyor2Straight:
                    var rotation = tilemap.GetTransformMatrix(arenaPosition).rotation.eulerAngles.z;
                    var vector = VectorHelpers.FromRotation(-rotation).AsIntVector();
                    yield return ConveyorMove(arenaObject, vector);
                    break;
                default:
                    // do nothing;
                    break;
            }
        }

        foreach (ArenaObject arenaObject in RegisteredObjects)
        {
            Vector3Int arenaPosition = arenaObject.ArenaPosition.ToVector3Int();
            var tileType = GetTileType(arenaObject);
            switch (tileType)
            {
                case TileType.Conveyor1Left:
                case TileType.Conveyor1Right:
                case TileType.Conveyor1Straight:
                case TileType.Conveyor2Left:
                case TileType.Conveyor2Right:
                case TileType.Conveyor2Straight:
                    var rotation = tilemap.GetTransformMatrix(arenaPosition).rotation.eulerAngles.z;
                    var vector = VectorHelpers.FromRotation(-rotation).AsIntVector();
                    yield return ConveyorMove(arenaObject, vector);
                    break;
                default:
                    // do nothing;
                    break;
            }
        }

        // TODO PUSHERS PUSH

        foreach(ArenaObject arenaObject in RegisteredObjects)
        {
            Vector3Int arenaPosition = arenaObject.ArenaPosition.ToVector3Int();
            var tileType = GetTileType(arenaObject);
            switch (tileType)
            {
                case TileType.RotateClockwise:
                    yield return arenaObject.RotateRight();
                    break;
                case TileType.RotateAnticlockwise:
                    yield return arenaObject.RotateLeft();
                    break;
                default:
                    // do nothing;
                    break;
            }
        }

        // TODO CHECK IF IN PIT

        yield return null;
    }

    private IEnumerator ConveyorMove(ArenaObject arenaObject, Vector2Int vector)
    {
        var result = arenaObject.GetMoveResult(vector, false);
        yield return arenaObject.Move(vector, false);

        if (result != MoveResult.Bounce)
        {
            // check if moved onto turning conveyor from "in" side
            var tileType = GetTileType(arenaObject);
            Vector3Int arenaPosition = arenaObject.ArenaPosition.ToVector3Int();
            var rotation = tilemap.GetTransformMatrix(arenaPosition).rotation.eulerAngles.z;

            switch (tileType)
            {
                case TileType.Conveyor1Left:
                case TileType.Conveyor2Left:
                    // if coming from (rotated) -x then turn left
                    var inVectorLeft = new Vector2(1, 0).RotateBy(rotation);
                    if (vector == inVectorLeft)
                    {
                        yield return arenaObject.RotateLeft();
                    }
                    break;
                case TileType.Conveyor1Right:
                case TileType.Conveyor2Right:
                    // if coming from (rotated) +x then turn right
                    var inVectorRight = new Vector2(-1, 0).RotateBy(rotation);
                    if (vector == inVectorRight)
                    {
                        yield return arenaObject.RotateRight();
                    }
                    break;
            }
        }
    }

    private TileType GetTileType(ArenaObject arenaObject)
    {
        var arenaPosition = arenaObject.ArenaPosition.ToVector3Int();
        var tile = tilemap.GetTile(arenaPosition);
        return (TileType)Enum.Parse(typeof(TileType), tile.name.Replace("_", ""), true);
    }

    private IEnumerator FireLasers()
    {
        Console.WriteLine("Fire lasers");
        //throw new NotImplementedException();
        yield return null;
    }

    private IEnumerator TouchObjectives()
    {
        Console.WriteLine("Touch objectives");
        //throw new NotImplementedException();
        yield return null;
    }

    ArenaMap GenerateArena()
    {
        var map = new ArenaMap(tilemap, wallmap);

        return map;
    }
}
