using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SlottedCardZone : CardZone
{
    public List<GameObject> slots;

    public override List<GameObject> AllCards => 
        slots
            .Select(s => s.GetComponent<CardSlot>())
            .Where(s => s.IsFull)
            .Select(s => s.Card)
            .ToList();

    public override bool CanAdd()
    {
        return slots.Any(s => !s.GetComponent<CardSlot>().IsFull);
    }

    public override bool Add(GameObject card)
    {
        // Adds to lowest slot available
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i].GetComponent<CardSlot>();
            if (slot.IsFull) continue;
            else
            {
                slot.Card = card;
                card.GetComponent<CardBase>().Zone = this;
                return true;
            }
        }
        return false;
    }

    public override void DrawFrom(CardZone zone, int cards = 1)
    {
        for (int i = 0; i < cards; i++)
        {
            if (zone.AllCards.Any())
            {
                bool added = zone.First().GetComponent<CardBase>().MoveTo(this);
                if (!added) break;
            }
        }
    }

    public override GameObject First()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i].GetComponent<CardSlot>();
            if (slot.IsFull) return slot.Card;
        }
        return null;
    }

    public override bool Remove(GameObject card)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i].GetComponent<CardSlot>();
            if (slot.IsFull && slot.Card == card)
            {
                slot.Card.transform.parent = null;
                slot.Card = null;
                return true;
            }
        }
        return false;
    }

    public override void Shuffle()
    {
        System.Random _random = new System.Random();

        GameObject temp;

        int n = slots.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            temp = slots[r].GetComponent<CardSlot>().Card;
            slots[r].GetComponent<CardSlot>().Card = slots[i].GetComponent<CardSlot>().Card;
            slots[i].GetComponent<CardSlot>().Card = temp;
        }
    }
}

