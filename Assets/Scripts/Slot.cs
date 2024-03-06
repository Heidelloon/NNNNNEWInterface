using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IPointerClickHandler
    , IBeginDragHandler, IDragHandler, IEndDragHandler
    , IDropHandler, IPointerMoveHandler
{
    public InventoryClass myInventory;

    [Header("슬롯 정보")]
    public ItemData inItem;
    public int count;

    //필요한 UI
    [HideInInspector] //인스펙터에 표시할 필요가 없는 경우, 숨길 수 있다.
    public Image itemImage;
    [HideInInspector]
    public Image selectImage;
    [HideInInspector]
    public TextMeshProUGUI countText;

    //
    public static Slot selectSlot;
    public static Slot dragSlot; //드래그중인 슬롯
    public static Slot dropSlot; //드래그한 아이템을 놓을 슬롯

    public GameObject copyDrag; //드래그를 연출하기 위해 복제한 아이템

    //[SerializeField]
    private Transform canvas;

    public static string dragMode = "none";

    // Start is called before the first frame update
    void Start()
    {
        itemImage
             = transform.Find("Icon").GetComponent<Image>();
        selectImage
            = transform.Find("Outline").GetComponent<Image>();
        countText
            = transform.Find("Count").GetComponent<TextMeshProUGUI>();

        selectImage.enabled = false;

        canvas = GameObject.Find("Canvas").transform;

        InitSlot();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitSlot()
    {
        if (inItem == null) //아이템 없음
        {
            itemImage.sprite = null;
            itemImage.enabled = false; //추가
            countText.text = "";
        }
        else //아이템 있음
        {
            itemImage.sprite = inItem.image;
            itemImage.enabled = true; //추가
            countText.text = "" + count;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //이미 선택된 슬롯이 있을때
        if (selectSlot != null)
        {
            selectSlot.selectImage.enabled = false;
            selectSlot = null;
        }
        //선택 슬롯 설정
        //해당 칸에 아이템이 있으면
        if (this.inItem != null)
        {
            selectSlot = this;
            selectSlot.selectImage.enabled = true;

            myInventory.OpenItemMenu(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그를 시작하는 부분
        //(이동할 아이템의 슬롯에서 작동)

        //드래그 슬롯 기억
        dragSlot = this;

        //이동 연출 - 이동할 이미지 복제
        copyDrag = Instantiate(this.gameObject, canvas);

        //복제품 수정
        copyDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(120f, 120f);
        Slot tmpSlot = copyDrag.GetComponent<Slot>();
        tmpSlot.GetComponent<Image>().enabled = false;
        tmpSlot.countText.enabled = false;

        if (Input.GetKey(KeyCode.LeftAlt))
            dragMode = "moveOne";
        else if (Input.GetKey(KeyCode.LeftShift))
            dragMode = "moveHalf";
        else
            dragMode = "moveAll";
    }

    public void OnDrag(PointerEventData eventData)
    {
        //마우스 또는 터치 위치를 적용    
        copyDrag.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //복제 오브젝트 삭제
        if (copyDrag != null)
            Destroy(copyDrag);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //드롭 시키는 부분
        //(드롭한 아이템의 슬롯에서 작동)
        if (dragSlot != null && dragSlot != this)
        {
            if (dragMode == "moveAll")
            {

                ItemData dragItem = dragSlot.inItem;
                int dragCount = dragSlot.count;

                //아이템 놓을 자리가 비었는지
                if (this.inItem == null)
                {
                    //빈자리에 아이템 넣기
                    this.inItem = dragItem;
                    this.count = dragCount;
                    this.InitSlot();

                    //기존 자리의 아이템 제거
                    dragSlot.inItem = null;
                    dragSlot.count = 0;
                    dragSlot.InitSlot();
                }
                else
                {
                    //같은 아이템인 경우
                    if (dragSlot.inItem == this.inItem)
                    {
                        //같은 아이템의 수량 증가
                        this.count += dragCount;
                        this.InitSlot();

                        //기존 자리의 아이템 제거
                        dragSlot.inItem = null;
                        dragSlot.count = 0;
                        dragSlot.InitSlot();
                    }
                    else //다른 아이템인 경우
                    {
                        //기존 자리의 이 아이템 넣기 (순서주의)
                        dragSlot.inItem = this.inItem;
                        dragSlot.count = this.count;
                        dragSlot.InitSlot();

                        //이 자리에 아이템 넣기 (순서주의)
                        this.inItem = dragItem;
                        this.count = dragCount;
                        this.InitSlot();
                    }
                }

                dropSlot = null;
                dragSlot = null;
                if (selectSlot != null)
                {
                    selectSlot.selectImage.enabled = false;
                    selectSlot = null;
                }
            }
            else if (dragMode == "moveOne")
            {
                myInventory.MoveItem(dragSlot, this, 1);
            }
            else if (dragMode == "moveHalf")
            {
                int moveCount = Mathf.FloorToInt((float)dragSlot.count * 0.5f);
                myInventory.MoveItem(dragSlot, this, moveCount);
            }

        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (dragSlot != null)
        {
            if (dropSlot != null)
            {
                dropSlot.selectImage.enabled = false;
            }
            dropSlot = this;
            this.selectImage.enabled = true;
        }
    }
}
