using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings.SplashScreen;

public class DropSlot : MonoBehaviour, IDropHandler
{
    static public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (Slot.dragSlot != null)
        {
            Debug.Log("아이템 드랍");

            if (Slot.dragMode == "moveAll")
            {
                Slot.dragSlot.myInventory.RemoveAllItem(Slot.dragSlot);

            }
            else if (Slot.dragMode == "moveOne")
            {
                Slot.dragSlot.myInventory.RemoveItem(Slot.dragSlot, 1);

            }
            else if (Slot.dragMode == "moveHalf")
            {
                int moveCount = Mathf.FloorToInt((float)Slot.dragSlot.count * 0.5f);
                Slot.dragSlot.myInventory.RemoveItem(Slot.dragSlot, moveCount);
            }

            image.enabled = false;
        }
    }

}
