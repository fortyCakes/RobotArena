using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
public sealed class CardsManager
{
    private GameObject move1;
    private GameObject move2;
    private GameObject move3;
    private GameObject backUp;
    private GameObject uTurn;
    private GameObject turnLeft;
    private GameObject turnRight;

    private static CardsManager instance;
    private static readonly object padlock = new object();

    public CardsManager()
    {
        move1 = GetCardPrefab("Move1");
        move2 = GetCardPrefab("Move2");
        move3 = GetCardPrefab("Move3");
        backUp = GetCardPrefab("BackUp");
        uTurn = GetCardPrefab("UTurn");
        turnLeft = GetCardPrefab("TurnLeft");
        turnRight = GetCardPrefab("TurnRight");

    }
    public static CardsManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new CardsManager();
                }
                return instance;
            }
        }
    }

    public static CardZone GenerateDeck()
    {
        var gameObject = new GameObject("Deck", typeof(CardZone));

        var newDeck = gameObject.GetComponent<CardZone>();

        AddCardsToDeck(newDeck, CardType.UTurn, 6, 0);
        AddCardsToDeck(newDeck, CardType.TurnLeft, 18, 60, 20);
        AddCardsToDeck(newDeck, CardType.TurnRight, 18, 70, 20);
        AddCardsToDeck(newDeck, CardType.BackUp, 6, 420);
        AddCardsToDeck(newDeck, CardType.Move1, 18, 480);
        AddCardsToDeck(newDeck, CardType.Move2, 12, 660);
        AddCardsToDeck(newDeck, CardType.Move3, 6, 800);

        return newDeck;
    }

    private static void AddCardsToDeck(CardZone newDeck, CardType cardType, int copies, int priorityStart, int priorityStep = 10)
    {
        int priority = priorityStart;
        for (int i = 1; i < copies; i++)
        {
            var card = CreateCardOfType(cardType);
            card.GetComponent<CardBase>().Priority = priority;
            priority += priorityStep;
            newDeck.Add(card);
        }
    }

    public static GameObject CreateCardOfType(CardType cardType)
    {
        GameObject cardPrefab;

        switch(cardType)
        {
            case CardType.Move1:
                cardPrefab = Instance.move1; break;
            case CardType.Move2:
                cardPrefab = Instance.move2; break;
            case CardType.Move3:
                cardPrefab = Instance.move3; break;
            case CardType.UTurn:
                cardPrefab = Instance.uTurn; break;
            case CardType.TurnLeft:
                cardPrefab = Instance.turnLeft; break;
            case CardType.TurnRight:
                cardPrefab = Instance.turnRight; break;
            case CardType.BackUp:
                cardPrefab = Instance.backUp; break;
            default: throw new System.Exception("Unknown CardType");
        }

        return GameObject.Instantiate(cardPrefab);
    }

    private GameObject GetCardPrefab(string resourceLabel)
    {
        return (GameObject)Resources.Load("prefabs/Card" + resourceLabel, typeof(GameObject));
    }
}
