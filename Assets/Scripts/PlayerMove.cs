using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;



    public class PlayerMove : MonoBehaviour
    {
        Rigidbody rigidbody; //물리적인 움직임을 제어하기 위해서 접근
        Transform camTransform; //카메라의 회전 상태를 활용하기 위해 접근

        public float moveSeeed = 5f; //이동속도
        public float runSpeed = 10f; //달리는 속도
        [SerializeField]
        private float nowSeeed; //지금 달리는 속도

        public float rotateSpeed = 60f; //방향 전환시, 몸을 회전 하는 속도

        public InputActionReference moveInput;
        public InputActionReference runinput;
        public InputActionReference jumpInput;
        public bool onTarget = false;



        public Animator animator;

        [SerializeField]
        private bool onMovable = true;

        public Seat nearSeat; //가까이에 있는 의자를 접근
        [SerializeField]
        private bool onSit = false;

        [SerializeField]
        private bool onGround = false;
        //카메라 기준으로 8방향 움직임 구현
        //이동방향에 맞춰서 몸을 회전

        public float jumpHeight = 2f;
        private float gravity;

    private float Falltimeout =2f;

    float maxSlope = 45; //플레이어가 걸을 수 있는
    [SerializeField]
    float velocityY = 0;

    bool onAct = false;

    public float FallTimeout = 0.15f;
    //낙하상태일때 애니메이션에 낙하상태를 전달을 지연하는 시간
    private float _fallTimeoutDelta;
    //낙하상태가 유지된 시간(얼마나 낙하중이었는지)
    void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            camTransform = Camera.main.transform;

        gravity = - Physics.gravity.y;

       
    }

  

        IEnumerator Jump()
        {
        onAct = false;

        float jumpPower = Mathf.Sqrt(2.0f * gravity * jumpHeight);
            //Mathf.Sqrt(2.0f * gravity * jumpHeight);
           // Vector3 lastVel = rigidbody.velocity;
            //lastVel.y = jumpPower;
        
            animator.SetTrigger("Jump");
            yield return new WaitForSeconds(0.55f);
            onMovable = true;
            velocityY = jumpPower;
        //rigidbody.velocity = lastVel;
       
    }

        IEnumerator SetSit()
        {
            onMovable = false;

            transform.position = nearSeat.sitPos.position;
            transform.rotation = nearSeat.sitPos.rotation;
            animator.SetBool("onSit", true);
            yield return new WaitForSeconds(0.5f);
            onSit = true;
        }
        IEnumerator OffSit()
        {
            onMovable = true;
            transform.position = nearSeat.sitPos.position;
            transform.rotation = nearSeat.sitPos.rotation;
            animator.SetBool("onSit", false);
            yield return new WaitForSeconds(0.5f);
            onSit = true;
        }


        void Update()
        {
        if (onMovable)
            Move();
        else
            rigidbody.velocity = Vector3.zero + Vector3.up * velocityY;

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Wave");
            }
            if (nearSeat != null && Input.GetKeyDown(KeyCode.F))
            {
                if (onSit)
                    StartCoroutine(OffSit());
                else
                    StartCoroutine(SetSit());



            }



            if (!onAct && onGround && jumpInput.action.triggered) //  if(onGeound && jumpInput.action.triggered)
            {
            onAct = true;
            StartCoroutine(Jump());
            }
               // animator.SetBool("onGround", onGround);



        /*
        Vector3 rayOri =transform.position + Vector3.up * 0.05f;
        Vector3 ratDir = Vector3.down;
        float rayDist =0.3f;


        if (Physics.Raycast(rayOri, ratDir ,out RaycastHit hit, rayDist))
        {
            onGeound = true;
        }
        else
        {
            onGeound = false;
        }
        */

        velocityY -= gravity * Time.deltaTime;

        if (onGround)
        {
            //떨어지고 있을때(점프하던 중에는 적용되지 않도록)
            if (rigidbody.velocity.y < 0)
                velocityY = -2f;

            _fallTimeoutDelta = Falltimeout;
            animator.SetBool("onGround", true);
        }
        else
        {
            if (_fallTimeoutDelta >= 0f)
                _fallTimeoutDelta -= Time.deltaTime;
            else
                animator.SetBool("onGround", false);
        }
    }

        void Move()
        {
            // float InpulX = moveInput.action.ReadValue<Vector2>().x;
            // float Inpuly = moveInput.action.ReadValue<Vector2>().y;

            Vector2 inputVec = moveInput.action.ReadValue<Vector2>();
            Vector3 moveVec = new Vector3(inputVec.x, 0, inputVec.y);

            Quaternion camRot = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);

            moveVec = camRot * moveVec;

            //float yVel = rigidbody.velocity.y;
        //누적된 중력을 가져오기 위해서
        //경사를 오르면서 생기는 힘도 가지고 온다
        //그래서 멈췄을때 수직으로 적용되던 힘이 남아서 뜬다.

            //속도를 결정하는 부분
            float tSpeed = moveSeeed;
            if (runinput.action.ReadValue<float>() > 0)
                tSpeed = runSpeed;

            nowSeeed = Mathf.Lerp(nowSeeed, tSpeed, Time.deltaTime * 3f);

        //움직임(속력) 적용 (이동방향 *속도)
        rigidbody.velocity = moveVec * nowSeeed + Vector3.up * velocityY;
            //+ new Vector3(0, yVel, 0);

            //이동방향으로 회전 시키기
            Vector3 lookPos = transform.position + moveVec;

            lookPos.y = transform.position.y;

            //transform.LookAt(lookPos);


            if (inputVec != Vector2.zero)
            {
                //이동방향으로 회전 시키기
                Quaternion rotNext = Quaternion.LookRotation(moveVec);

                if (onTarget == true)
                {
                    //카메라 방향으로 쳐다보기
                    Quaternion camFront = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, rotNext, Time.deltaTime * rotateSpeed);
            }
            if (inputVec == Vector2.zero)
            {
                nowSeeed = moveSeeed;
            }

            //애니메이션 처리
            if (onTarget == true)
            {
                //애니메이션 처리
                animator.SetFloat("inputX", inputVec.x);
                animator.SetFloat("inputY", inputVec.y);
            }
            else
            {
                float runAnimSpeed = inputVec.magnitude; //0-1

                if (nowSeeed > moveSeeed)
                {
                    runAnimSpeed = (1 + nowSeeed / runSpeed) * inputVec.magnitude;
                }
                animator.SetFloat("inputX", 0);
                animator.SetFloat("inputY", runAnimSpeed);
            }

        }
        private void OnTriggerEnter(Collider other)
        {
            //TryGetComponent
            if (other.TryGetComponent(out Seat findSeat))
            {
                nearSeat = findSeat;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Seat findSeat))
            {
                if (nearSeat == findSeat)
                    nearSeat = null;
            }
        }
        private void OnCollisionStay(Collision collision)
        {
          
            foreach(var item in collision.contacts)
            {
                //item.normal //부딪힌 면의 위쪽 방향
                float angle = Vector3.Angle(Vector3.up, item.normal);
                float maxSlope = 45f; //플레이어가 걸을 수 있는 최대 기울기

                if (angle <= maxSlope)
                {
                    //바닥이다(걸을 수 있는 각도다)
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
    
