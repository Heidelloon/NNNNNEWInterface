using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerItem : Item
{
    public override void GetItem()
    {

    }

    public override bool UseItem()
    {
        FindObjectOfType<Hammer>().ThrowHammerObject();
        return true;
    }
}
