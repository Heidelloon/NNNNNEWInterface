using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PMove : MonoBehaviour
{

    Rigidbody prigidbody; // 물리적 움직임을 이용하기 위해 접근

    Transform camTransform; // 카메라 회전 상태 이용 시 접근

    Animator animator;

    public float moveSpeed = 5f; //이동 속도 

    public float runSpeed = 10f; //달리는 속도

    public float rotateSpeed = 15f; // 방향 전환 시 몸 회전하는 속도

    private float nowSpeed; //지금 달리는 속도

    public bool onAct = false; //어떤 동작이 실행중인지 확인

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

    public float jumpPower = 5.0f; //점프 힘(수정가능)

    public int jumpLevel = 5; //점프 횟수(수정 가능)

    public int jumpCount = 0; // 지금 몇번 점프 중인가(수정 불가능)

    bool OnAir = false; // 캐릭터가 공중에 떠있는가?

    public bool onGround = false;

    public float MaxSlope = 45f;// 플레이어가 걸을 수 있는 최대 기울기

    //카메라 기준 8방향 움직임 구현

    // 이동 방향 기준으로 회전

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

            //리지드바디를 이용해서 위로 힘을 가한다.
            prigidbody.velocity = new Vector2(prigidbody.velocity.x, 1);
            prigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);

            //점프횟수 1 더해준다.
            jumpCount += 1;

            //float jumpPower = Mathf.Sqrt(2.0f * gravity * jumpHeight)

            //Vector3 lastVel = rigidbody.velocity;

            //lestVel.y = jumpPower;

            //rigidbody.velocity = lastVel;

            //바닥 체크 1 - 레이캐스트

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

        // 속도를 결정하는 부분

        float tSpeed = moveSpeed;

        float isRun = runInput.action.ReadValue<float>();

        if (isRun > 0)
            tSpeed = runSpeed;

        nowSpeed = Mathf.Lerp(nowSpeed, tSpeed, Time.deltaTime * 3f);

        //움직임(속력) 적용 (이동방향 * 속도)

        prigidbody.velocity = moveVec * nowSpeed
            + Vector3.up * yVel;

        //+ new Vector3(0, yVel, 0);

        //이동 방향으로 회전시키기

        if (inputVec != Vector2.zero)
        {
            //이동 방향으로 회전시키기
            //lerp는 변화 담당
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
            //item.normal// 부딫힌 면의 위쪽 방향

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
