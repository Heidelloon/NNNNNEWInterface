using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class InventoryClass : MonoBehaviour
{
    public GameObject slotSample; //슬롯 샘플
    public Vector2Int invenSize; //설정할 인벤토리 크기

    public float viewHeight = 400f; //보여지는 높이
    public float bar_width = 20f; //스크롤바 너비
    public RectTransform viewRect; //보여지는 영역 RectTransform
    public RectTransform invenRect; //슬롯이 담기는 곳 RectTransform
    private GridLayoutGroup gridLayout;

    [SerializeField]
    private Slot[] slotList; //만들어진 슬롯 게임오브젝트 목록

    public ItemData[] itemList;//전체 아이템 목록

    //아이템 우클릭 메뉴를 사용하기 위한 변수들
    [SerializeField]
    private GameObject UpUI; //전체 메뉴
    [SerializeField]
    private GameObject selectButton; //메뉴 버튼
    private static GameObject[] selectButtonList; //버튼 리스트

    // Start is called before the first frame update
    void Start()
    {
        //invenRect = GetComponent<RectTransform>();
        gridLayout = invenRect.GetComponent<GridLayoutGroup>();

        slotSample.SetActive(false);

        //아이템 목록 가져오기
        itemList = Resources.LoadAll<ItemData>("ItemDatas");

        //아이템 메뉴 
        //인벤토리가 여러개인 경우, UpUI를 비활성화 하면 찾지 못하므로 수정 
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
        //인벤토리 크기 결정
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



        //기존 슬롯 제거
        if (slotList != null) //기존슬롯이 있으면
        {
            for (int i = 0; i < slotList.Length; i++)
            {
                Destroy(slotList[i].gameObject); //게임오브젝트 삭제
            }
        }

        //슬롯 생성
        int slotCount = invenSize.x * invenSize.y;
        slotList = new Slot[slotCount]; //배열 관리 목록 생성

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
            //빈칸 찾거나 같은 아이템 찾기
            if (slot.inItem == null
                || slot.inItem == addItem)
            {
                slot.inItem = addItem;
                slot.count += count;
                slot.InitSlot();

                addSuccess = true;
                break; //남은 칸을 확인 하지 않도록, 반복문 강제 중지
            }
        }

        return addSuccess;
    }
    public void MoveItem(Slot origin, Slot next, int count = 1)
    {
        //서로 다른 슬롯인가
        if (origin != next && count > 0)
        {
            Debug.Log("이동 " + count + "개");

            if (next.inItem == null) //다음 슬롯이 비어있음
            {
                //다음 슬롯 처리 (순서주의)
                next.inItem = origin.inItem;
                next.count = count;
                next.InitSlot();

                //기존 슬롯 처리 (순서주의)
                origin.count -= count;
                if (origin.count <= 0)
                    origin.inItem = null;
                origin.InitSlot();

            }
            else //다음 슬롯이 비어있지 않음
            {
                if (origin.inItem == next.inItem)
                {
                    //다음 슬롯 처리(순서주의)
                    //next.inItem = origin.inItem;
                    next.count += count;
                    next.InitSlot();

                    //기존 슬롯 처리(순서주의)
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
        //아이템 삭제는 지정 슬롯에 존재하는 아이템을 삭제하는 내용으로 작성
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
        //아이템 삭제는 지정 슬롯에 존재하는 아이템을 삭제하는 내용으로 작성
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

        //기존의 버튼 목록 제거
        if (selectButtonList != null)
        {
            foreach (var item in selectButtonList)
            {
                Destroy(item);
            }
        }

        //2개 버튼을 넣을 자리 생성
        selectButtonList = new GameObject[2];

        GameObject newButton;

        //버리기 버튼
        newButton
            = Instantiate(selectButton, selectButton.transform.parent);

        newButton.GetComponent<TextMeshProUGUI>().text = "Remove";
        //newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";
        newButton.GetComponent<Button>().onClick
            .AddListener(() => RemoveItem(slot, 1)); //람다식
        newButton.GetComponent<Button>().onClick
            .AddListener(() => UpUI.SetActive(false)); //추가
        selectButtonList[0] = newButton;


        //모두 버리기 버튼
        newButton
            = Instantiate(selectButton, selectButton.transform.parent);

        newButton.GetComponent<TextMeshProUGUI>().text = "Remove All";
        newButton.GetComponent<Button>().onClick
            .AddListener(() => RemoveAllItem(slot)); //람다식
        newButton.GetComponent<Button>().onClick
            .AddListener(() => UpUI.SetActive(false)); //람다식
        selectButtonList[1] = newButton;

        //샘플 버튼 끄기
        selectButton.SetActive(false);

        UpUI.SetActive(true);
    }
}
