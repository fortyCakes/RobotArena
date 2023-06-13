using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CardZone : MonoBehaviour
{
    public List<GameObject> cards;
    public Transform displayParent;

    public CardZone()
    {
        this.cards = new List<GameObject>();
    }


    public virtual void Add(GameObject card)
    {
        cards.Add(card);
        card.GetComponent<CardBase>().Zone = this;
    }

    public virtual void Remove(GameObject card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
        }
    }

    public void DrawFrom(CardZone zone, int cards = 1)
    {
        for (int i = 0; i < cards; i++)
        {
            if (zone.cards.Any())
            {
                zone.First().GetComponent<CardBase>().MoveTo(this);
            }
        }
    }

    public GameObject First()
    {
        return cards.First();
    }

    public void Shuffle()
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

