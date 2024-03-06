using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PMove : MonoBehaviour
{

    Rigidbody prigidbody; // ������ �������� �̿��ϱ� ���� ����

    Transform camTransform; // ī�޶� ȸ�� ���� �̿� �� ����

    Animator animator;

    public float moveSpeed = 5f; //�̵� �ӵ� 

    public float runSpeed = 10f; //�޸��� �ӵ�

    public float rotateSpeed = 15f; // ���� ��ȯ �� �� ȸ���ϴ� �ӵ�

    private float nowSpeed; //���� �޸��� �ӵ�

    public bool onAct = false; //� ������ ���������� Ȯ��

    [SerializeField]

    float velocityY = 0;
    public InputActionReference moveInput;

    public InputActionReference runInput;

    public InputActionReference jumpInput;

    public bool ontarget = false;

    public bool onparkour = false;

    [SerializeField]
    private bool onMoveable = true;

    //public Seat nearSeat;

    public float jumpPower = 5.0f; //���� ��(��������)

    public int jumpLevel = 5; //���� Ƚ��(���� ����)

    public int jumpCount = 0; // ���� ��� ���� ���ΰ�(���� �Ұ���)

    bool OnAir = false; // ĳ���Ͱ� ���߿� ���ִ°�?

    public bool onGround = false;

    public float MaxSlope = 45f;// �÷��̾ ���� �� �ִ� �ִ� ����

    //ī�޶� ���� 8���� ������ ����

    // �̵� ���� �������� ȸ��

    // Start is called before the first frame update
    void Start()
    {
        prigidbody = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onMoveable)
            Move();

        else

            prigidbody.velocity = Vector3.zero + Vector3.up * velocityY;

        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }

        bool jump = jumpInput.action.triggered;

        if (!onAct && jump)
        {
            onAct = true;

            StartCoroutine(Air());
        }

    
        prigidbody.constraints = RigidbodyConstraints.FreezePositionY;

        prigidbody.freezeRotation = true;


    }

    IEnumerator Air()
    {
        onAct = true;

        if (!OnAir || (OnAir && jumpCount < jumpLevel))
        {

            //������ٵ� �̿��ؼ� ���� ���� ���Ѵ�.
            prigidbody.velocity = new Vector2(prigidbody.velocity.x, 1);
            prigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);

            //����Ƚ�� 1 �����ش�.
            jumpCount += 1;

            //float jumpPower = Mathf.Sqrt(2.0f * gravity * jumpHeight)

            //Vector3 lastVel = rigidbody.velocity;

            //lestVel.y = jumpPower;

            //rigidbody.velocity = lastVel;

            //�ٴ� üũ 1 - ����ĳ��Ʈ

            /*Vector3 rayOri = transform.position;

            Vector3 ratDir = Vector3.down;

            float rayDist = 1.0f;

            Debug.DrawLine(rayOri, rayOri + ratDir * rayDist);


            if (Physics.Raycast(rayOri, ratDir, out RaycastHit hit, rayDist))

                onGround = true;

            else
            {
                onGround = false;
            }*/

            yield return new WaitForSeconds(0.55f);

            onAct = false;

        }
    }
    /*IEnumerator Setsit()
    {
        onMoveable = false;

        transform.position = nearSeat.sitPos.position;

        transform.rotation = nearSeat.sitPos.rotation;

        animator.SetBool("onSit", true);
        yield return new WaitForSeconds(0.5f);

        onSit = true;
    }

    IEnumerator OffSit()
    {
        onMoveable = true;

        animator.SetBool("onSit", false);

        yield return new WaitForSeconds(0.5f);

        onSit = false;  

    }
    */

    void Move()
    {
        Vector2 inputVec = moveInput.action.ReadValue<Vector2>();

        Vector3 moveVec = new Vector3(inputVec.x, 0, inputVec.y);

        Quaternion camRot = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);

        moveVec = camRot * moveVec;

        float yVel = prigidbody.velocity.y;

        // �ӵ��� �����ϴ� �κ�

        float tSpeed = moveSpeed;

        float isRun = runInput.action.ReadValue<float>();

        if (isRun > 0)
            tSpeed = runSpeed;

        nowSpeed = Mathf.Lerp(nowSpeed, tSpeed, Time.deltaTime * 3f);

        //������(�ӷ�) ���� (�̵����� * �ӵ�)

        prigidbody.velocity = moveVec * nowSpeed
            + Vector3.up * yVel;

        //+ new Vector3(0, yVel, 0);

        //�̵� �������� ȸ����Ű��

        if (inputVec != Vector2.zero)
        {
            //�̵� �������� ȸ����Ű��
            //lerp�� ��ȭ ���
            Quaternion rotNext = Quaternion.LookRotation(moveVec);

            if (ontarget == true)
            {
                rotNext = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);

            }

            transform.rotation
                = Quaternion.Lerp(transform.rotation, rotNext, Time.deltaTime * rotateSpeed);

        }

        if (ontarget)
        {
            animator.SetFloat("inputX", inputVec.x);
            animator.SetFloat("inputY", inputVec.y);

        }

        else
        {
            float runAnimSpeed = inputVec.magnitude;

            /*if(nowSpeed > moveSpeed)
            {
                runAnimSpeed = (1 + nowSpeed / runSpeed) * runAnimSpeed;
            }
            */
            animator.SetFloat("inputX", 0);
            animator.SetFloat("inputY", runAnimSpeed);
        }

        if (isRun > 0)
        {
            animator.SetFloat("inputY", 2);
        }

    }

    /* private void OnTriggerEnter(Collider other)
     {
         if(other.TryGetComponent<Seat>(out Seat findseat))
         {
             nearSeat = findseat;    
         }
     }

     private void OnTriggerExit(Collider other)
     {
         if (other.TryGetComponent<Seat>(out Seat findseat))
         {
            if(nearSeat = findseat)
             {
                 nearSeat = null;
             }
         }
     }
    */
    private void OnCollisionStay(Collision collision)
    {
        foreach (var item in collision.contacts)
        {
            //item.normal// �΋H�� ���� ���� ����

            float angle = Vector3.Angle(Vector3.up, item.normal);

            if (angle <= MaxSlope)
            {
                onGround = true;

                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
}
