using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;



    public class PlayerMove : MonoBehaviour
    {
        Rigidbody rigidbody; //�������� �������� �����ϱ� ���ؼ� ����
        Transform camTransform; //ī�޶��� ȸ�� ���¸� Ȱ���ϱ� ���� ����

        public float moveSeeed = 5f; //�̵��ӵ�
        public float runSpeed = 10f; //�޸��� �ӵ�
        [SerializeField]
        private float nowSeeed; //���� �޸��� �ӵ�

        public float rotateSpeed = 60f; //���� ��ȯ��, ���� ȸ�� �ϴ� �ӵ�

        public InputActionReference moveInput;
        public InputActionReference runinput;
        public InputActionReference jumpInput;
        public bool onTarget = false;



        public Animator animator;

        [SerializeField]
        private bool onMovable = true;

        public Seat nearSeat; //�����̿� �ִ� ���ڸ� ����
        [SerializeField]
        private bool onSit = false;

        [SerializeField]
        private bool onGround = false;
        //ī�޶� �������� 8���� ������ ����
        //�̵����⿡ ���缭 ���� ȸ��

        public float jumpHeight = 2f;
        private float gravity;

    private float Falltimeout =2f;

    float maxSlope = 45; //�÷��̾ ���� �� �ִ�
    [SerializeField]
    float velocityY = 0;

    bool onAct = false;

    public float FallTimeout = 0.15f;
    //���ϻ����϶� �ִϸ��̼ǿ� ���ϻ��¸� ������ �����ϴ� �ð�
    private float _fallTimeoutDelta;
    //���ϻ��°� ������ �ð�(�󸶳� �������̾�����)
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
            //�������� ������(�����ϴ� �߿��� ������� �ʵ���)
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
        //������ �߷��� �������� ���ؼ�
        //��縦 �����鼭 ����� ���� ������ �´�
        //�׷��� �������� �������� ����Ǵ� ���� ���Ƽ� ���.

            //�ӵ��� �����ϴ� �κ�
            float tSpeed = moveSeeed;
            if (runinput.action.ReadValue<float>() > 0)
                tSpeed = runSpeed;

            nowSeeed = Mathf.Lerp(nowSeeed, tSpeed, Time.deltaTime * 3f);

        //������(�ӷ�) ���� (�̵����� *�ӵ�)
        rigidbody.velocity = moveVec * nowSeeed + Vector3.up * velocityY;
            //+ new Vector3(0, yVel, 0);

            //�̵��������� ȸ�� ��Ű��
            Vector3 lookPos = transform.position + moveVec;

            lookPos.y = transform.position.y;

            //transform.LookAt(lookPos);


            if (inputVec != Vector2.zero)
            {
                //�̵��������� ȸ�� ��Ű��
                Quaternion rotNext = Quaternion.LookRotation(moveVec);

                if (onTarget == true)
                {
                    //ī�޶� �������� �Ĵٺ���
                    Quaternion camFront = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, rotNext, Time.deltaTime * rotateSpeed);
            }
            if (inputVec == Vector2.zero)
            {
                nowSeeed = moveSeeed;
            }

            //�ִϸ��̼� ó��
            if (onTarget == true)
            {
                //�ִϸ��̼� ó��
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
                //item.normal //�ε��� ���� ���� ����
                float angle = Vector3.Angle(Vector3.up, item.normal);
                float maxSlope = 45f; //�÷��̾ ���� �� �ִ� �ִ� ����

                if (angle <= maxSlope)
                {
                    //�ٴ��̴�(���� �� �ִ� ������)
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
    
