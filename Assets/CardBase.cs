using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
    public CardZone Zone;
    public CardType CardType;
    public int Priority = 100;
    public Text PriorityText;

    public void Update()
    {
        PriorityText.text = Priority.ToString();
    }

    public bool MoveTo(CardZone zone)
    {
        if (!zone.CanAdd())
            return false;

        Zone.Remove(this.gameObject);
        bool added = zone.Add(this.gameObject);

        if (!added) return false;


        this.transform.parent = zone.displayParent;


        return true;
    }

    public void RemoveFromZone()
    {
        Zone.Remove(this.gameObject);
        Zone = null;
    }

    public void CardClick()
    {
        if (Zone != null)
        {
            Zone.OnCardClick(gameObject);
        }
    }

    public IEnumerator Execute(ProgrammableRobot programmableRobot)
    {
        switch (CardType)
        {
            case CardType.TurnLeft:
                yield return programmableRobot.RotateLeft();
                break;
            case CardType.TurnRight:
                yield return programmableRobot.RotateRight();
                break;
            case CardType.UTurn:
                yield return programmableRobot.RotateRight();
                yield return programmableRobot.RotateRight();
                break;
            case CardType.Move1:
                yield return programmableRobot.Move(programmableRobot.FacingVector);
                break;
            case CardType.Move2:
                yield return programmableRobot.Move(programmableRobot.FacingVector, speed: 2f);
                yield return programmableRobot.Move(programmableRobot.FacingVector, speed: 2f);
                break;
            case CardType.Move3:
                yield return programmableRobot.Move(programmableRobot.FacingVector, speed: 3f);
                yield return programmableRobot.Move(programmableRobot.FacingVector, speed: 3f);
                yield return programmableRobot.Move(programmableRobot.FacingVector, speed: 3f);
                break;
            case CardType.BackUp:
                yield return programmableRobot.Move(programmableRobot.FacingVector * -1);
                break;
            default:
                throw new Exception("Unknown CardType");
        }
    }
}
