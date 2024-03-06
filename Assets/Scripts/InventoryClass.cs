using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class InventoryClass : MonoBehaviour
{
    public GameObject slotSample; //���� ����
    public Vector2Int invenSize; //������ �κ��丮 ũ��

    public float viewHeight = 400f; //�������� ����
    public float bar_width = 20f; //��ũ�ѹ� �ʺ�
    public RectTransform viewRect; //�������� ���� RectTransform
    public RectTransform invenRect; //������ ���� �� RectTransform
    private GridLayoutGroup gridLayout;

    [SerializeField]
    private Slot[] slotList; //������� ���� ���ӿ�����Ʈ ���

    public ItemData[] itemList;//��ü ������ ���

    //������ ��Ŭ�� �޴��� ����ϱ� ���� ������
    [SerializeField]
    private GameObject UpUI; //��ü �޴�
    [SerializeField]
    private GameObject selectButton; //�޴� ��ư
    private static GameObject[] selectButtonList; //��ư ����Ʈ

    // Start is called before the first frame update
    void Start()
    {
        //invenRect = GetComponent<RectTransform>();
        gridLayout = invenRect.GetComponent<GridLayoutGroup>();

        slotSample.SetActive(false);

        //������ ��� ��������
        itemList = Resources.LoadAll<ItemData>("ItemDatas");

        //������ �޴� 
        //�κ��丮�� �������� ���, UpUI�� ��Ȱ��ȭ �ϸ� ã�� ���ϹǷ� ���� 
        UpUI = GameObject.Find("Canvas").transform.Find("UpUI").gameObject;
        selectButton = UpUI.transform.Find("Select").gameObject;
        UpUI.SetActive(false);

        InitInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            InitInventory();

        if (Input.GetKeyDown(KeyCode.R))
        {
            int rand = Random.Range(0, itemList.Length);
            int randCount = Random.Range(1, 4);

            AddItem(itemList[rand], randCount);
        }
    }

    void InitInventory()
    {
        //�κ��丮 ũ�� ����
        float ySize
            = gridLayout.padding.top + gridLayout.cellSize.y * invenSize.y
            + gridLayout.spacing.y * (invenSize.y - 1)
            + gridLayout.padding.bottom;

        float xSize
              = gridLayout.padding.left + gridLayout.cellSize.x * invenSize.x
            + gridLayout.spacing.x * (invenSize.x - 1)
            + gridLayout.padding.right;

        Vector2 viewSize = new Vector2(xSize + bar_width, viewHeight);

        viewRect.sizeDelta = viewSize;

        Vector2 calSize = new Vector2(0, ySize);

        invenRect.sizeDelta = calSize;



        //���� ���� ����
        if (slotList != null) //���������� ������
        {
            for (int i = 0; i < slotList.Length; i++)
            {
                Destroy(slotList[i].gameObject); //���ӿ�����Ʈ ����
            }
        }

        //���� ����
        int slotCount = invenSize.x * invenSize.y;
        slotList = new Slot[slotCount]; //�迭 ���� ��� ����

        slotSample.SetActive(true);
        for (int i = 0; i < slotCount; i++)
        {
            slotList[i]
                = Instantiate(slotSample, slotSample.transform.parent)
                .GetComponent<Slot>();
        }
        slotSample.SetActive(false);

    }
    public bool AddItem(ItemData addItem, int count = 1)
    {
        bool addSuccess = false;

        foreach (var slot in slotList)
        {
            //��ĭ ã�ų� ���� ������ ã��
            if (slot.inItem == null
                || slot.inItem == addItem)
            {
                slot.inItem = addItem;
                slot.count += count;
                slot.InitSlot();

                addSuccess = true;
                break; //���� ĭ�� Ȯ�� ���� �ʵ���, �ݺ��� ���� ����
            }
        }

        return addSuccess;
    }
    public void MoveItem(Slot origin, Slot next, int count = 1)
    {
        //���� �ٸ� �����ΰ�
        if (origin != next && count > 0)
        {
            Debug.Log("�̵� " + count + "��");

            if (next.inItem == null) //���� ������ �������
            {
                //���� ���� ó�� (��������)
                next.inItem = origin.inItem;
                next.count = count;
                next.InitSlot();

                //���� ���� ó�� (��������)
                origin.count -= count;
                if (origin.count <= 0)
                    origin.inItem = null;
                origin.InitSlot();

            }
            else //���� ������ ������� ����
            {
                if (origin.inItem == next.inItem)
                {
                    //���� ���� ó��(��������)
                    //next.inItem = origin.inItem;
                    next.count += count;
                    next.InitSlot();

                    //���� ���� ó��(��������)
                    origin.count -= count;
                    if (origin.count <= 0)
                        origin.inItem = null;
                    origin.InitSlot();
                }
            }
        }
    }

    public void RemoveItem(Slot slot, int count = 1)
    {
        //������ ������ ���� ���Կ� �����ϴ� �������� �����ϴ� �������� �ۼ�
        if (slot.inItem != null)
        {
            slot.count -= count;

            if (slot.count <= 0)
            {
                slot.inItem = null;
            }

            slot.InitSlot();
        }
    }
    public void RemoveAllItem(Slot slot)
    {
        //������ ������ ���� ���Կ� �����ϴ� �������� �����ϴ� �������� �ۼ�
        if (slot.inItem != null)
        {
            slot.count = 0;
            slot.inItem = null;

            slot.InitSlot();
        }
    }

    public void OpenItemMenu(Slot slot)
    {
        Vector2 UpPos = slot.transform.position
            + (Vector3)(gridLayout.cellSize * 0.5f);

        UpUI.transform.position = UpPos;

        selectButton.SetActive(true);

        //������ ��ư ��� ����
        if (selectButtonList != null)
        {
            foreach (var item in selectButtonList)
            {
                Destroy(item);
            }
        }

        //2�� ��ư�� ���� �ڸ� ����
        selectButtonList = new GameObject[2];

        GameObject newButton;

        //������ ��ư
        newButton
            = Instantiate(selectButton, selectButton.transform.parent);

        newButton.GetComponent<TextMeshProUGUI>().text = "Remove";
        //newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";
        newButton.GetComponent<Button>().onClick
            .AddListener(() => RemoveItem(slot, 1)); //���ٽ�
        newButton.GetComponent<Button>().onClick
            .AddListener(() => UpUI.SetActive(false)); //�߰�
        selectButtonList[0] = newButton;


        //��� ������ ��ư
        newButton
            = Instantiate(selectButton, selectButton.transform.parent);

        newButton.GetComponent<TextMeshProUGUI>().text = "Remove All";
        newButton.GetComponent<Button>().onClick
            .AddListener(() => RemoveAllItem(slot)); //���ٽ�
        newButton.GetComponent<Button>().onClick
            .AddListener(() => UpUI.SetActive(false)); //���ٽ�
        selectButtonList[1] = newButton;

        //���� ��ư ����
        selectButton.SetActive(false);

        UpUI.SetActive(true);
    }
}
