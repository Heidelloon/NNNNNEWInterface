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

    [Header("���� ����")]
    public ItemData inItem;
    public int count;

    //�ʿ��� UI
    [HideInInspector] //�ν����Ϳ� ǥ���� �ʿ䰡 ���� ���, ���� �� �ִ�.
    public Image itemImage;
    [HideInInspector]
    public Image selectImage;
    [HideInInspector]
    public TextMeshProUGUI countText;

    //
    public static Slot selectSlot;
    public static Slot dragSlot; //�巡������ ����
    public static Slot dropSlot; //�巡���� �������� ���� ����

    public GameObject copyDrag; //�巡�׸� �����ϱ� ���� ������ ������

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
        if (inItem == null) //������ ����
        {
            itemImage.sprite = null;
            itemImage.enabled = false; //�߰�
            countText.text = "";
        }
        else //������ ����
        {
            itemImage.sprite = inItem.image;
            itemImage.enabled = true; //�߰�
            countText.text = "" + count;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //�̹� ���õ� ������ ������
        if (selectSlot != null)
        {
            selectSlot.selectImage.enabled = false;
            selectSlot = null;
        }
        //���� ���� ����
        //�ش� ĭ�� �������� ������
        if (this.inItem != null)
        {
            selectSlot = this;
            selectSlot.selectImage.enabled = true;

            myInventory.OpenItemMenu(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�׸� �����ϴ� �κ�
        //(�̵��� �������� ���Կ��� �۵�)

        //�巡�� ���� ���
        dragSlot = this;

        //�̵� ���� - �̵��� �̹��� ����
        copyDrag = Instantiate(this.gameObject, canvas);

        //����ǰ ����
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
        //���콺 �Ǵ� ��ġ ��ġ�� ����    
        copyDrag.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //���� ������Ʈ ����
        if (copyDrag != null)
            Destroy(copyDrag);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //��� ��Ű�� �κ�
        //(����� �������� ���Կ��� �۵�)
        if (dragSlot != null && dragSlot != this)
        {
            if (dragMode == "moveAll")
            {

                ItemData dragItem = dragSlot.inItem;
                int dragCount = dragSlot.count;

                //������ ���� �ڸ��� �������
                if (this.inItem == null)
                {
                    //���ڸ��� ������ �ֱ�
                    this.inItem = dragItem;
                    this.count = dragCount;
                    this.InitSlot();

                    //���� �ڸ��� ������ ����
                    dragSlot.inItem = null;
                    dragSlot.count = 0;
                    dragSlot.InitSlot();
                }
                else
                {
                    //���� �������� ���
                    if (dragSlot.inItem == this.inItem)
                    {
                        //���� �������� ���� ����
                        this.count += dragCount;
                        this.InitSlot();

                        //���� �ڸ��� ������ ����
                        dragSlot.inItem = null;
                        dragSlot.count = 0;
                        dragSlot.InitSlot();
                    }
                    else //�ٸ� �������� ���
                    {
                        //���� �ڸ��� �� ������ �ֱ� (��������)
                        dragSlot.inItem = this.inItem;
                        dragSlot.count = this.count;
                        dragSlot.InitSlot();

                        //�� �ڸ��� ������ �ֱ� (��������)
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
