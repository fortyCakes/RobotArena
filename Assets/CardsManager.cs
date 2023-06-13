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
