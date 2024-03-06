using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove3D : MonoBehaviour
{
    //역할 직책 이름;
    //리더 반장;
    public Rigidbody myRigidbody;
    //public Rigidbody[] myRigidbody; 여러개 찾을 때

    public float speed = 5;
    public float rotSpeed = 10f;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        // myRigidbody = GetComponents<Rigidbody>(); 여러개 찾을 때

        //마우스를 보이지 않게 함
        Cursor.visible = false;
        //마우스를 고정하게 함
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        Move();
        Rotate();
    }
    void Move()
    {
       // float inputX = 0; Input.GetAxis("Horizontal");
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        //입력에 대한 벡터를 만들기(방향)

        

        Vector3 inputVec = new Vector3(inputX, 0, inputY);

        //문제점 - 입력 방향이 실제 움직임으로 적응하기엔 카메라와 맞지 않다.
        //원인 - 카메라가 바라보는 방향과 맞지 않다.
        //원인 - 캐릭터가 바라보는 방향과 맞지 않다.
        //문제점2 - 중력이 갑자기 느리게 적용된다.
        //원인 -중력이 누적되지 못하게 0으로 계속 갱신 되있었다.
        //해결 - 속력을 변경하기 전에, 기존 속력의 y값을 함께 적용

        //입력방향을 실제이동방향으로 가공
        Vector3 moveVec = inputVec;
        //해결1. 캐릭터 기준으로 이동 하도록 하기
        //moveVec = transform.rotation * moveVec;
        //해결2. 카메라 기준으로 이동 하도록 하기
        //Quaternion camRot = Camera.main.transform.rotation;
        Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        moveVec = camRot * moveVec;

        //중력이 누적되도록 가공
        Vector3 fVel = moveVec * speed;
        //fVel += Vector3.up * myRigidbody.velocity.y; 밑에꺼랑 같음
        fVel += new Vector3(0, myRigidbody.velocity.y, 0);

        myRigidbody.velocity = fVel;

        //이동 방향으로 회전시키기
       /*
        if (inputX != 0 || inputY != 0)
        {
            Quaternion rotNext = Quaternion.LookRotation(moveVec);
            float rotLookSpeed = 10f;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotNext, rotLookSpeed * Time.deltaTime);
        }
       */
    }
    void Rotate()
    {
        //Input.GetAxis("Mouse X"); 
        //마우스의 수평 이동량을 나타낸다.
        //마우스 움직임이 없으면 0, 있으면 프레임?당 이동량
        float inPutMouseX = Input.GetAxis("Mouse X");
        float inPutScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        /*
        Quaternion rotY = Quaternion.Euler(0, rotSpeed * Time.deltaTime, 0);
        transform.rotation = rotY * transform.rotation;
        */

        float inpuRot = 0;

        if (Input.GetKey(KeyCode.Q))
            inpuRot = -1f;
        if (Input.GetKey(KeyCode.E))
            inpuRot = 1f;
        //축으로 물체를 회전 시키는 방법
        transform.Rotate(Vector3.up ,inPutMouseX * rotSpeed * Time.deltaTime); //마우스로 이동
        transform.Rotate(Vector3.up, inpuRot * rotSpeed * Time.deltaTime); //키보드로 이동

        //다른 위치를 중심으로 축으로 회전 시키는 방법
        //transform.RotateAround(Vector3.zero, Vector3.up, inPutMouseX * rotSpeed * Time.deltaTime);

        //Camera.main.transform.Rotate(transform.position, inPutMouseX * rotSpeed * Time.deltaTime);
    }
}
