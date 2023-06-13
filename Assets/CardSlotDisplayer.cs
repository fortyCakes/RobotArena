using UnityEngine;

public class CardSlotDisplayer : MonoBehaviour
{
    void Update()
    {
        var slot = GetComponent<CardSlot>();

        if (slot.IsFull)
        {
            slot.Card.transform.parent = this.transform;
            slot.Card.transform.localPosition = Vector3.zero;
        }
    }
}
