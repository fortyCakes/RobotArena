
using System;
using UnityEngine;

public class ProgramCardZone : SlottedCardZone 
{
    public PlayerHand PlayerHand;
    private ArenaController arena;

    public void Start()
    {
        arena = FindObjectOfType<ArenaController>();
    }

    public override void OnCardClick(GameObject gameObject)
    {
        if (!arena.isExecuting)
        {
            gameObject.GetComponent<CardBase>().MoveTo(PlayerHand);
        }
    }

    public CardBase CardInSlot(int i)
    {
        if (slots.Count >= i && slots[i].GetComponent<CardSlot>().IsFull)
        {
            return slots[i].GetComponent<CardSlot>().Card.GetComponent<CardBase>();
        }
        return null;
    }
}

