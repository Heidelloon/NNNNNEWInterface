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

    Rigidbody prigidbody; // ������ �������� �̿��ϱ� ���� ����

    Animator animator;

    public float moveSpeed = 5f; //�̵� �ӵ� 

    public float runSpeed = 10f; //�޸��� �ӵ�

    public float rotateSpeed = 15f; // ���� ��ȯ �� �� ȸ���ϴ� �ӵ�

    private float nowSpeed; //���� �޸��� �ӵ�

    public bool onAct = false; //� ������ ���������� Ȯ��


    public bool ontarget = false;

    public bool onparkour = false;

    public bool onhand = false;


    //public Seat nearSeat;

    public float jumpPower = 5.0f; //���� ��(��������)

    public int jumpLevel = 5; //���� Ƚ��(���� ����)

    public int jumpCount = 0; // ���� ��� ���� ���ΰ�(���� �Ұ���)

    public bool onGround = false;

    public float MaxSlope = 45f;// �÷��̾ ���� �� �ִ� �ִ� ����

    public GameObject handedItem; //�տ��� ������

    //private bool onAim = false; //���� ��������

   
    
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
            Debug.Log("���� ����");
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

        // �θ� ����
        item.transform.parent = null;   
        SetEgint(item, false);
    }

    void throwitem()
    {
        // �����۵� �� �ϳ��� ������
        GameObject item = handPos.transform.GetComponentInChildren<Rigidbody>().gameObject;

        handedItem = item;

        Debug.Log(item.name);

        item.transform.parent = null;

        // ������ �������� ���� �ùķ��̼� Ȱ��ȭ
        SetEgint(item, false);

        Collider[] itemcolliders = item.GetComponents<Collider>();

        foreach (Collider itemcollider in itemcolliders)
        {
            itemcollider.enabled = true;
        }

        item.GetComponent<Rigidbody>().isKinematic = false;

        // ���콺 ��ġ�� ���� ���� ���� ���
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

        // ���� ���� ũ�� ����
        float throwPower; // ������ ���� ũ��� �����ؾ� ��

        throwPower = throwpower;

        // ���� ���� ���ؼ� ������ ������
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
