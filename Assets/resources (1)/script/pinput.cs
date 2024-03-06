using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class pinput : MonoBehaviour
{

    public bool onitem = false;

    public float throwpower = 1f;

    public Transform handPos;

    public Transform handpos2;

    hammer ham;

    phone pho;

    public InputActionReference attackInput;

    Rigidbody prigidbody; // 물리적 움직임을 이용하기 위해 접근

    Animator animator;

    public float moveSpeed = 5f; //이동 속도 

    public float runSpeed = 10f; //달리는 속도

    public float rotateSpeed = 15f; // 방향 전환 시 몸 회전하는 속도

    private float nowSpeed; //지금 달리는 속도

    public bool onAct = false; //어떤 동작이 실행중인지 확인


    public bool ontarget = false;

    public bool onparkour = false;

    public bool onhand = false;


    //public Seat nearSeat;

    public float jumpPower = 5.0f; //점프 힘(수정가능)

    public int jumpLevel = 5; //점프 횟수(수정 가능)

    public int jumpCount = 0; // 지금 몇번 점프 중인가(수정 불가능)

    public bool onGround = false;

    public float MaxSlope = 45f;// 플레이어가 걸을 수 있는 최대 기울기

    public GameObject handedItem; //손에든 아이템

    //private bool onAim = false; //조준 상태인지

   
    
    // Start is called before the first frame update
    void Start()
    {
        prigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        ham = GetComponent<hammer>();

        pho = GetComponent<phone>();    

        onGround = true;

       
    }

    // Update is called once per frame
    void Update()
    {
        bool attack = attackInput.action.triggered;

        if (!onAct && onGround && attack)
        {
            StartCoroutine(Attack());
        }

        if(Input.GetKey(KeyCode.G))
        {
            //animator.SetTrigger("picking");
        }


        if(Input.GetKeyDown(KeyCode.P))
        {
            swaing();
        }

        if (Input.GetKey(KeyCode.V))
        {
            //animator.SetTrigger("onItem");
        }

        Aim();
    }


    IEnumerator Attack()
    {
        onAct = true;
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(0.55f);
        
        if(onhand == true)
        { 
            throwitem(); 
        }
 
        else if (onhand == false)
        {
                
        }


        yield return new WaitForSeconds(0.55f);

        onAct = false;
    }




    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Handrail"))
        {
            prigidbody.constraints = RigidbodyConstraints.FreezePositionY;

            prigidbody.freezeRotation = true;

            animator.SetBool("onParkour", true);
        }

        if (collision.gameObject.CompareTag("hammer"))
        {
            Debug.Log("접촉 성공");
        }
   

        if (collision.gameObject.CompareTag("smartphone"))
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                //animator.SetTrigger("onpho");
            } 
       
        }
        else
        {
            onitem = false;
        }

   
    }

    private void OnTriggerExit(Collider collision)
    {
        prigidbody.constraints = RigidbodyConstraints.None;

        prigidbody.freezeRotation = true;

        animator.SetBool("onParkour", false);
    }

    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit rayHit;

        float rayLength = 500f;

        int floorMask = LayerMask.GetMask("floor");

        if (Physics.Raycast(ray, out rayHit, rayLength, floorMask))
        {

            Vector3 playertomouse = rayHit.point - transform.position;

            playertomouse.y = 0f;

            Quaternion newrotation = Quaternion.LookRotation(playertomouse);

            prigidbody.MoveRotation(newrotation);
        }
    }


    void drop1()
    {

        GameObject item = transform.GetComponentInChildren<Rigidbody>().gameObject;

        // 부모 해제
        item.transform.parent = null;   
        SetEgint(item, false);
    }

    void throwitem()
    {
        // 아이템들 중 하나를 가져옴
        GameObject item = handPos.transform.GetComponentInChildren<Rigidbody>().gameObject;

        handedItem = item;

        Debug.Log(item.name);

        item.transform.parent = null;

        // 가져온 아이템의 물리 시뮬레이션 활성화
        SetEgint(item, false);

        Collider[] itemcolliders = item.GetComponents<Collider>();

        foreach (Collider itemcollider in itemcolliders)
        {
            itemcollider.enabled = true;
        }

        item.GetComponent<Rigidbody>().isKinematic = false;

        // 마우스 위치를 향한 방향 벡터 계산
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        float rayLength = 500f;
        int floorMask = LayerMask.GetMask("floor");

        Vector3 throwDirection;

        if (Physics.Raycast(ray, out rayHit, rayLength, floorMask))
        {
            throwDirection = rayHit.point - handPos.transform.position;
        }
        else
        {
            throwDirection = transform.forward * 50f;
        }

        // 던질 힘의 크기 설정
        float throwPower; // 적절한 힘의 크기로 조절해야 함

        throwPower = throwpower;

        // 던질 힘을 가해서 아이템 던지기
        item.GetComponent<Rigidbody>().AddForce(throwDirection.normalized * throwPower, ForceMode.Impulse);

        onhand = false;
    }

    void swaing()
    {
      
        animator.SetBool("slash", true);
      
    }

    void SetEgint(GameObject item, bool isEgint)
    {
        Collider[] itemcolliders = item.GetComponents<Collider>();

        foreach (Collider itemcollider in itemcolliders)
        {
            itemcollider.enabled = !isEgint;
        }
        prigidbody.isKinematic = isEgint;
    }
}
