using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotionItem : Item
{
    public int recoverAmount = 10;

    public override void GetItem()
    {

    }

    public override bool UseItem()
    {
        FindObjectOfType<PlayerHealth>()?.AddHealth(recoverAmount);
        return true;
    }
}
