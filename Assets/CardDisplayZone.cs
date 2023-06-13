using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayZone : MonoBehaviour
{
    private CardZone CardZone;
    public int margin = 10;

    void Start()
    {
        CardZone = GetComponent<CardZone>();
    }

    void Update()
    {
        var xOffset = margin - GetComponent<RectTransform>().rect.width/2;
        foreach(var card in CardZone.cards)
        {
            int width = (int)card.GetComponent<RectTransform>().rect.width;
            int height = (int)card.GetComponent<RectTransform>().rect.height;
            card.transform.localPosition = new Vector3(xOffset + width / 2, 0, 0);
            xOffset += width + margin;
        }
    }
}
