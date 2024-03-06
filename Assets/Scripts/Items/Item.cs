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

    // �������� ����� ��
    public virtual void GetItem()
    {

    }

    // �������� ������� ��
    public virtual bool UseItem()
    {
        return false;
    }
}
