using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// InvenItem 클래스: 인벤토리에 있는 각 아이템을 나타내는 클래스
[System.Serializable]
public class InvenItem
{
    public Image highlightImage; // 아이템 선택 시 하이라이트 표시를 위한 이미지
    public Image iconImage;  // 아이템 아이콘 이미지
    public TextMeshProUGUI countText; // 아이템 갯수를 표시하는 텍스트

    public GameObject pickUpItemPrefab { get; set; } // 획득한 아이템의 원본 프리팹


    private FlashLight FlashLight { get; set; }

    public Item item { get; set; } // 아이템 정보
    public int count { get; set; } // 아이템 갯수

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
    public Transform objectCheckTransform; // 아이템 체크 위치를 나타내는 트랜스폼
    public float objectCheckRange = 5; // 아이템 체크 범위

    public InvenItem[] invenList = new InvenItem[10]; // 인벤토리 슬롯 배열
    private int currentSelectInven = 0; // 현재 선택된 인벤토리 슬롯의 인덱스

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


    // 처음 시작이 인벤토리 0번째 슬롯을 활성화.
    private void Start()
    {
        InvenSelect(0);
    }

    private void Update()
    {
        UpdateInventory(); // 인벤토리 업데이트
        ObjectPickUp();  // 아이템 획득 시도
        ObjectThrow();   // 아이템 버리기 시도
        InvenSelectInput();  // 인벤토리 슬롯 선택 입력
        InvenUseInput(); // 인벤토리 사용
    }

    // 계속해서 인벤토리 표시를 업데이트함.
    private void UpdateInventory()
    {
        for (int i = 0; i < invenList.Length; i++)
            UpdateShowItemDisplay(i, invenList[i].item?.sprite, invenList[i].count);
    }

    // Sprite가 있으면, 갯수가 0개 이상이면 표시.
    private void UpdateShowItemDisplay(int index, Sprite sprite, int count)
    {
        invenList[index].iconImage.sprite = sprite;
        invenList[index].countText.text = count.ToString();

        invenList[index].iconImage.gameObject.SetActive(sprite != null);
        invenList[index].countText.gameObject.SetActive(count > 0);
    }

    // 인벤토리에 빈공간과 중복되는 아이템이 존재하는지 확인하고 해당공간에 집어넣어줌.
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
                // 아이템을 버릴 수 없다면 return
                if (!invenList[currentSelectInven].item.isCanThrow) return;

                // Throw로 원본 Prefab을 복제해서 아이템을 추가.
                // 기존에 유지하고 있던 원본 Prefab은 씬에 계속 쌓이면 안되기에 ResetItem에서 삭제.
                if (invenList[currentSelectInven].pickUpItemPrefab != null)
                    Instantiate(invenList[currentSelectInven].pickUpItemPrefab, objectCheckTransform.position, objectCheckTransform.rotation).SetActive(true);

                RemoveItem(currentSelectInven);
            }
        }
    }

    private void AddItem(int inventorySlotIndex, Item item, GameObject itemPrefab)
    {
        // Pickup 했을 때 Throw를 위해서 원본 GameObject를 저장해놓음.
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

    // Item이 동일한 Item인지 판별하고 중복 아이템으로 아이템 카운트가 올라가도록 하기 위해서 name으로 비교함.
    // 기존에 있는 Item와 새로 들어온 Item 원본 자체는 같지만 각각 다른 Item이기 때문에 서로 같은지 비교가 되지 않음.
    // 그렇기 때문에 서로 데이터가 같은 name으로 판별함.
    public int GetOverlapSlot(Item item)
    {
        for (int i = 0; i < invenList.Length; i++)
        {
            if (invenList[i].item?.itemName == item.itemName)
                return i;
        }
        return -1;
    }

    // 제일 먼저 현재 선택된 inventory부터 검사하고 이미 아이템이 있다면 slot을 하나씩 돌아가면서 비어있는 공간을 찾음.
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


    // 숫자 키 입력으로 인벤토리 슬롯 선택 메서드 
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

    // 이전 선택한 인벤은 하이라이트 끄고 지금 선택한 인벤은 하이라이트를 킴.
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
            
            // 현재 선택된 인벤에 템이 있다면 실행.
            if (item != null)
            {
                // 선택된 아이템의 태그가 hammer인 경우
                if (invenList[currentSelectInven].item.tag == "hammer")
                {
                    // hammer 클래스 가져오기
                    hammer gha = invenList[currentSelectInven].item.GetComponent<hammer>();

                    // 아이템의 클론 생성
                    GameObject itemClone = Instantiate(invenList[currentSelectInven].pickUpItemPrefab, gha.handPos.transform.position, gha.handPos.transform.rotation);

                    Debug.Log("안 붙음");

                    // 아이템을 handPos의 자식으로 설정
                    itemClone.transform.parent = gha.handPos.transform;

                    // 아이템 활성화
                    itemClone.SetActive(true);

                    // hammer 스크립트의 SetEgint 메서드 호출
                    gha.SetEgint(itemClone, true);

                    itemClone.GetComponent<Rigidbody>().isKinematic = true;

                   itemClone.GetComponent<Rigidbody>().useGravity = true;    

                    // 아이템 사용에 성공
                    itemUsed = true;

                    pinput pinput = GetComponent<pinput>();

                    pinput.GetComponent<pinput>().onhand = true;
                }

                else
                {
                    // 아이템 사용에 성공했는지 여부를 저장하는 변수
                    item.GetComponent<Item>().UseItem();

                    itemUsed = true;

                }

                // 아이템 사용에 성공하고 아이템이 사용했을 때 없어지는 아이템이면 해당 인벤 아이템 제거
                if (itemUsed && item.isUseRemoveItem)
                    RemoveItem(currentSelectInven);
            }
        }
    }


    // Gizmos를 이용해 오브젝트 체크 범위를 시각적으로 표현
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(objectCheckTransform.position, objectCheckRange);
    }

  
}

