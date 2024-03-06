using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// InvenItem Ŭ����: �κ��丮�� �ִ� �� �������� ��Ÿ���� Ŭ����
[System.Serializable]
public class InvenItem
{
    public Image highlightImage; // ������ ���� �� ���̶���Ʈ ǥ�ø� ���� �̹���
    public Image iconImage;  // ������ ������ �̹���
    public TextMeshProUGUI countText; // ������ ������ ǥ���ϴ� �ؽ�Ʈ

    public GameObject pickUpItemPrefab { get; set; } // ȹ���� �������� ���� ������


    private FlashLight FlashLight { get; set; }

    public Item item { get; set; } // ������ ����
    public int count { get; set; } // ������ ����

    public void ResetItem()
    {
        if (pickUpItemPrefab != null)
            GameObject.Destroy(pickUpItemPrefab.gameObject);
        item = null;
        count = 0;
    }
}

public class myInventoryClass : MonoBehaviour
{
    public Transform objectCheckTransform; // ������ üũ ��ġ�� ��Ÿ���� Ʈ������
    public float objectCheckRange = 5; // ������ üũ ����

    public InvenItem[] invenList = new InvenItem[10]; // �κ��丮 ���� �迭
    private int currentSelectInven = 0; // ���� ���õ� �κ��丮 ������ �ε���

    public bool IsCardInInventory()
    {
        // Loop through each inventory slot
        for (int i = 0; i < invenList.Length; i++)
        {
            // Check if the item in the slot is a card
            if (invenList[i].item != null && invenList[i].item.CompareTag("Card"))
            {
                return true; // Card found in the inventory
            }
        }
        return false; // Card not found in the inventory
    }


    // ó�� ������ �κ��丮 0��° ������ Ȱ��ȭ.
    private void Start()
    {
        InvenSelect(0);
    }

    private void Update()
    {
        UpdateInventory(); // �κ��丮 ������Ʈ
        ObjectPickUp();  // ������ ȹ�� �õ�
        ObjectThrow();   // ������ ������ �õ�
        InvenSelectInput();  // �κ��丮 ���� ���� �Է�
        InvenUseInput(); // �κ��丮 ���
    }

    // ����ؼ� �κ��丮 ǥ�ø� ������Ʈ��.
    private void UpdateInventory()
    {
        for (int i = 0; i < invenList.Length; i++)
            UpdateShowItemDisplay(i, invenList[i].item?.sprite, invenList[i].count);
    }

    // Sprite�� ������, ������ 0�� �̻��̸� ǥ��.
    private void UpdateShowItemDisplay(int index, Sprite sprite, int count)
    {
        invenList[index].iconImage.sprite = sprite;
        invenList[index].countText.text = count.ToString();

        invenList[index].iconImage.gameObject.SetActive(sprite != null);
        invenList[index].countText.gameObject.SetActive(count > 0);
    }

    // �κ��丮�� ������� �ߺ��Ǵ� �������� �����ϴ��� Ȯ���ϰ� �ش������ ����־���.
    private void ObjectPickUp()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Collider[] colliders = Physics.OverlapSphere(objectCheckTransform.position, objectCheckRange);
            for (int i = 0; i < colliders.Length; i++)
            {
                Item pickupItem = colliders[i].GetComponent<Item>();

                if (pickupItem == null) continue;
                {
                    int pickupInvenIndex = GetEmptySlot();

                    if (pickupInvenIndex != -1)
                    {
                        if (pickupItem.isOverlap)
                        {
                            int overlapInvenIndex = GetOverlapSlot(pickupItem);
                            if (overlapInvenIndex != -1)
                                pickupInvenIndex = overlapInvenIndex;
                        }

                        AddItem(pickupInvenIndex, pickupItem, pickupItem.gameObject);
                        pickupItem.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void ObjectThrow()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (invenList[currentSelectInven].item != null)
            {
                // �������� ���� �� ���ٸ� return
                if (!invenList[currentSelectInven].item.isCanThrow) return;

                // Throw�� ���� Prefab�� �����ؼ� �������� �߰�.
                // ������ �����ϰ� �ִ� ���� Prefab�� ���� ��� ���̸� �ȵǱ⿡ ResetItem���� ����.
                if (invenList[currentSelectInven].pickUpItemPrefab != null)
                    Instantiate(invenList[currentSelectInven].pickUpItemPrefab, objectCheckTransform.position, objectCheckTransform.rotation).SetActive(true);

                RemoveItem(currentSelectInven);
            }
        }
    }

    private void AddItem(int inventorySlotIndex, Item item, GameObject itemPrefab)
    {
        // Pickup ���� �� Throw�� ���ؼ� ���� GameObject�� �����س���.
        invenList[inventorySlotIndex].pickUpItemPrefab = itemPrefab.gameObject;
        invenList[inventorySlotIndex].item = item;

        if (item.isOverlap)
            invenList[inventorySlotIndex].count++;

        invenList[inventorySlotIndex].item.GetItem();
    }

    private void RemoveItem(int inventorySlotIndex)
    {
        if (invenList[inventorySlotIndex].item.isOverlap)
        {
            invenList[inventorySlotIndex].count -= 1;
            if (invenList[inventorySlotIndex].count <= 0)
                invenList[inventorySlotIndex].ResetItem();
        }
        else
        {
            invenList[inventorySlotIndex].ResetItem();
        }
    }

    // Item�� ������ Item���� �Ǻ��ϰ� �ߺ� ���������� ������ ī��Ʈ�� �ö󰡵��� �ϱ� ���ؼ� name���� ����.
    // ������ �ִ� Item�� ���� ���� Item ���� ��ü�� ������ ���� �ٸ� Item�̱� ������ ���� ������ �񱳰� ���� ����.
    // �׷��� ������ ���� �����Ͱ� ���� name���� �Ǻ���.
    public int GetOverlapSlot(Item item)
    {
        for (int i = 0; i < invenList.Length; i++)
        {
            if (invenList[i].item?.itemName == item.itemName)
                return i;
        }
        return -1;
    }

    // ���� ���� ���� ���õ� inventory���� �˻��ϰ� �̹� �������� �ִٸ� slot�� �ϳ��� ���ư��鼭 ����ִ� ������ ã��.
    public int GetEmptySlot()
    {
        if (invenList[currentSelectInven].item == null)
        {
            return currentSelectInven;
        }
        else
        {
            for (int i = 0; i < invenList.Length; i++)
            {
                if (invenList[i].item == null)
                    return i;
            }

            return -1;
        }
    }


    // ���� Ű �Է����� �κ��丮 ���� ���� �޼��� 
    private void InvenSelectInput()
    {
        for (int i = 0; i < invenList.Length; i++)
        {
            KeyCode keyCode = KeyCode.Alpha1 + i;
            keyCode = keyCode <= KeyCode.Alpha9 ? keyCode : KeyCode.Alpha0;
            if (Input.GetKeyDown(keyCode))
                InvenSelect(i);
        }
    }

    // ���� ������ �κ��� ���̶���Ʈ ���� ���� ������ �κ��� ���̶���Ʈ�� Ŵ.
    private void InvenSelect(int index)
    {
        invenList[currentSelectInven].highlightImage.gameObject.SetActive(false);
        invenList[index].highlightImage.gameObject.SetActive(true);
        currentSelectInven = index;
    }
    private void InvenUseInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Item item = invenList[currentSelectInven].item;

            bool itemUsed = false;
            
            // ���� ���õ� �κ��� ���� �ִٸ� ����.
            if (item != null)
            {
                // ���õ� �������� �±װ� hammer�� ���
                if (invenList[currentSelectInven].item.tag == "hammer")
                {
                    // hammer Ŭ���� ��������
                    hammer gha = invenList[currentSelectInven].item.GetComponent<hammer>();

                    // �������� Ŭ�� ����
                    GameObject itemClone = Instantiate(invenList[currentSelectInven].pickUpItemPrefab, gha.handPos.transform.position, gha.handPos.transform.rotation);

                    Debug.Log("�� ����");

                    // �������� handPos�� �ڽ����� ����
                    itemClone.transform.parent = gha.handPos.transform;

                    // ������ Ȱ��ȭ
                    itemClone.SetActive(true);

                    // hammer ��ũ��Ʈ�� SetEgint �޼��� ȣ��
                    gha.SetEgint(itemClone, true);

                    itemClone.GetComponent<Rigidbody>().isKinematic = true;

                   itemClone.GetComponent<Rigidbody>().useGravity = true;    

                    // ������ ��뿡 ����
                    itemUsed = true;

                    pinput pinput = GetComponent<pinput>();

                    pinput.GetComponent<pinput>().onhand = true;
                }

                else
                {
                    // ������ ��뿡 �����ߴ��� ���θ� �����ϴ� ����
                    item.GetComponent<Item>().UseItem();

                    itemUsed = true;

                }

                // ������ ��뿡 �����ϰ� �������� ������� �� �������� �������̸� �ش� �κ� ������ ����
                if (itemUsed && item.isUseRemoveItem)
                    RemoveItem(currentSelectInven);
            }
        }
    }


    // Gizmos�� �̿��� ������Ʈ üũ ������ �ð������� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(objectCheckTransform.position, objectCheckRange);
    }

  
}

