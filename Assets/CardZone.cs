using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CardZone : MonoBehaviour
{
    private List<GameObject> cards;

    public virtual bool CanAdd()
    {
        return true;
    }

    public CardZone ReshuffleWhenEmpty;
    public Transform displayParent;

    public CardZone()
    {
        this.cards = new List<GameObject>();
    }

    public virtual List<GameObject> AllCards => cards;

    public virtual bool Add(GameObject card)
    {
        cards.Add(card);
        card.GetComponent<CardBase>().Zone = this;
        return true;
    }

    public virtual bool Remove(GameObject card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            return true;
        }
        return false;
    }

    public virtual void OnCardClick(GameObject gameObject)
    {
        // implement zone-specific behaviours
    }

    public virtual void DrawFrom(CardZone zone, int cards = 1)
    {
        for (int i = 0; i < cards; i++)
        {
            if (zone.AllCards.Any())
            {
                zone.First().GetComponent<CardBase>().MoveTo(this);
            } else if (zone.ReshuffleWhenEmpty != null && zone.ReshuffleWhenEmpty.AllCards.Any())
            {
                zone.ReshuffleWhenEmpty.Shuffle();
                DrawFrom(zone.ReshuffleWhenEmpty, zone.ReshuffleWhenEmpty.AllCards.Count());
            }
        }
    }

    public virtual GameObject First()
    {
        return cards.First();
    }

    public virtual void Shuffle()
    {
        System.Random _random = new System.Random();

        GameObject temp;

        int n = cards.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)(_random.NextDouble() * (n - i));
            temp = cards[r];
            cards[r] = cards[i];
            cards[i] = temp;
        }
    }
}

