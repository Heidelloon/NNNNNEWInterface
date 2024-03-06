using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    public Sprite sprite;

    public bool isOverlap = false;
    public bool isCanThrow = true;

    public bool isUseRemoveItem = true;

    // 아이템을 얻었을 때
    public virtual void GetItem()
    {

    }

    // 아이템을 사용했을 때
    public virtual bool UseItem()
    {
        return false;
    }
}
