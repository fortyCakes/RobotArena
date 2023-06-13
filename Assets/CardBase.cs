using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    public CardZone Zone;

    public void MoveTo(CardZone zone)
    {
        this.Zone.Remove(this.gameObject);
        zone.Add(this.gameObject);
        if (zone.displayParent != null)
        {
            this.transform.parent = zone.displayParent;
        }
    }

    public void Destroy()
    {
        Zone.Remove(this.gameObject);
    }
}
