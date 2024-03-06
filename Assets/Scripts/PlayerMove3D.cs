using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove3D : MonoBehaviour
{
    //���� ��å �̸�;
    //���� ����;
    public Rigidbody myRigidbody;
    //public Rigidbody[] myRigidbody; ������ ã�� ��

    public float speed = 5;
    public float rotSpeed = 10f;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        // myRigidbody = GetComponents<Rigidbody>(); ������ ã�� ��

        //���콺�� ������ �ʰ� ��
        Cursor.visible = false;
        //���콺�� �����ϰ� ��
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
        //�Է¿� ���� ���͸� �����(����)

        

        Vector3 inputVec = new Vector3(inputX, 0, inputY);

        //������ - �Է� ������ ���� ���������� �����ϱ⿣ ī�޶�� ���� �ʴ�.
        //���� - ī�޶� �ٶ󺸴� ����� ���� �ʴ�.
        //���� - ĳ���Ͱ� �ٶ󺸴� ����� ���� �ʴ�.
        //������2 - �߷��� ���ڱ� ������ ����ȴ�.
        //���� -�߷��� �������� ���ϰ� 0���� ��� ���� ���־���.
        //�ذ� - �ӷ��� �����ϱ� ����, ���� �ӷ��� y���� �Բ� ����

        //�Է¹����� �����̵��������� ����
        Vector3 moveVec = inputVec;
        //�ذ�1. ĳ���� �������� �̵� �ϵ��� �ϱ�
        //moveVec = transform.rotation * moveVec;
        //�ذ�2. ī�޶� �������� �̵� �ϵ��� �ϱ�
        //Quaternion camRot = Camera.main.transform.rotation;
        Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        moveVec = camRot * moveVec;

        //�߷��� �����ǵ��� ����
        Vector3 fVel = moveVec * speed;
        //fVel += Vector3.up * myRigidbody.velocity.y; �ؿ����� ����
        fVel += new Vector3(0, myRigidbody.velocity.y, 0);

        myRigidbody.velocity = fVel;

        //�̵� �������� ȸ����Ű��
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
        //���콺�� ���� �̵����� ��Ÿ����.
        //���콺 �������� ������ 0, ������ ������?�� �̵���
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
        //������ ��ü�� ȸ�� ��Ű�� ���
        transform.Rotate(Vector3.up ,inPutMouseX * rotSpeed * Time.deltaTime); //���콺�� �̵�
        transform.Rotate(Vector3.up, inpuRot * rotSpeed * Time.deltaTime); //Ű����� �̵�

        //�ٸ� ��ġ�� �߽����� ������ ȸ�� ��Ű�� ���
        //transform.RotateAround(Vector3.zero, Vector3.up, inPutMouseX * rotSpeed * Time.deltaTime);

        //Camera.main.transform.Rotate(transform.position, inPutMouseX * rotSpeed * Time.deltaTime);
    }
}
