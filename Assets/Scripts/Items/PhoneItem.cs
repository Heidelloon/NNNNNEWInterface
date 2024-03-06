using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneItem : Item
{
    public override void GetItem()
    {
        FindObjectOfType<FlashLight>().isAvliableUse = true;
    }

    public override bool UseItem()
    {
        FindObjectOfType<FlashLight>().isAvliableUse = true;
        return false;
    }
}
