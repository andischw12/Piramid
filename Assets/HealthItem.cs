using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : FloorPickUpItem
{
    public override void PickUp()
    {
        PlayerManager.instance.ChangeHealth(value);
    }
}
