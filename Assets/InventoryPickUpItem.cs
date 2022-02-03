using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPickUpItem : PickUpItem
{
    public InventoryItemType type;
    public override void PickUp()
    {
        PickUpEffect();
    }
}
