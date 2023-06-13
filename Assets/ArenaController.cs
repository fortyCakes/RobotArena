using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArenaController : MonoBehaviour
{
    Tilemap tilemap;
    ArenaMap arenaMap;
    CardZone deck;
    public CardZone playerHand;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = FindObjectOfType<Tilemap>();

        arenaMap = GenerateArena();
        DisplayArena(tilemap, arenaMap);

        deck = GenerateDeck();
        deck.Shuffle();

        playerHand.DrawFrom(deck, 9);
    }

    private CardZone GenerateDeck()
    {
        var newDeck = new CardZone();

        AddCardsToDeck(newDeck, CardType.UTurn, 6);
        AddCardsToDeck(newDeck, CardType.TurnLeft, 18);
        AddCardsToDeck(newDeck, CardType.TurnRight, 18);
        AddCardsToDeck(newDeck, CardType.Move1, 18);
        AddCardsToDeck(newDeck, CardType.Move2, 12);
        AddCardsToDeck(newDeck, CardType.Move3, 6);
        AddCardsToDeck(newDeck, CardType.BackUp, 6);

        return newDeck;
    }

    private static void AddCardsToDeck(CardZone newDeck, CardType cardType, int copies)
    {
        for (int i = 1; i < copies; i++)
        {
            var card = CardsManager.CreateCardOfType(cardType);
            newDeck.Add(card);
        }
    }

    private void DisplayArena(Tilemap tilemap, ArenaMap arenaMap)
    {
        foreach(Vector2Int key in arenaMap.Tiles.Keys)
        {
            tilemap.SetTile(new Vector3Int(key.x, key.y, 0), TilesManager.GetTileAsset(arenaMap.TileAt(key)));
        }
    }

    ArenaMap GenerateArena()
    {
        var map = new ArenaMap();
        for (int x = -10; x <= 10; x++)
        {
            for (int y = -10; y <= 10; y++)
            {
                map.SetTile(new Vector2Int(x, y), TileType.Empty);
            }
        }

        return map;
    }
}
