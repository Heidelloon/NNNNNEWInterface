using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCam : MonoBehaviour
{
    //Follow ī�޶� ����ٴϸ鼭 ��ġ�� ����
    [Header("Follow")]
    public Transform followTarget; //���� ���
    public Vector3 followOffset = Vector3.zero; //���� ��ġ�� �����ϴ� ��
    public float followSpeed = 3f; //���� �ӵ�(�ε巴�� ���󰡴� ����)
    public float rotateSpeedX = 10f; //����ȸ�� �ӵ�
    [SerializeField]
    private float angleY = 0; // ���� ȸ�� ���¿� ���� ������ ��

    //Look ī�޶� �Ĵٺ��鼭 ȸ���� ����
    [Header("Look")]
    public Transform lookTarget; //�ٶ� ���
    public Vector3 lookOffset = Vector3.zero; //�ٶ� ��ġ�� �����ϴ� ��
    public float zoomSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position
            = followTarget.position + followTarget.rotation * followOffset;

        //angleY = 0; 
        //���� (������ �Ʒ���) ���ΰ��� �� �߱Ⱚ���� ����
        //���� �ڵ�� ���ΰ��� ȸ����ä�� ����Ǹ� ��߳�
        angleY = followTarget.eulerAngles.y;
    }

    //���� ������Ʈ
    //(��� ������Ʈ�� ������ ����)
    private void LateUpdate()
    {
        //���⿡ ���� �ڵ�� �Ź� ��� ������Ʈ�Ŀ� ����

    }
    //������ �ֱ⸶�� ������Ʈ
    //(�����ֱ�,�ξ� �ʰ� �ݺ�, �⺻���� 0.02���� �ݺ�)
    private void FixedUpdate()
    {
        //���⿡ ���� ������Ʈ ���� ���� �ֱ�� �ݺ�
        //�����ֱ⿡ ���缭 �ݺ�
        if (followTarget)
            CamFollow();
        if (lookTarget)
            CamLook();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
    }

    void CamFollow()
    {
        float mInputX = Input.GetAxis("Mouse X");
        angleY += mInputX * rotateSpeedX;


        Quaternion rotX = Quaternion.Euler(0, angleY, 0);

        Vector3 followPos
            = followTarget.position + rotX * followOffset;
        //transform.position = followPos;
        transform.position
            = Vector3.Lerp(transform.position, followPos, Time.deltaTime * followSpeed);

        Debug.DrawLine(followTarget.position, followPos, Color.cyan);
    }
    void CamLook()
    {
        Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        Vector3 lookPos = lookTarget.position + camRot * lookOffset;

        Debug.DrawLine(lookTarget.position, lookPos, Color.red);
        Debug.DrawLine(transform.position, lookPos, Color.blue);

        transform.LookAt(lookPos);
    }

    void Zoom()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        float nextFOV = Camera.main.fieldOfView + wheelInput * zoomSpeed;

        nextFOV = Mathf.Clamp(nextFOV, 30, 120);

        Camera.main.fieldOfView = nextFOV;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //ī�޶��� �ø�����ũ�� Player�����ϰ� ������ ����
            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //ī�޶��� �ø�����ũ�� everything���� ����
            Camera.main.cullingMask = -1;
        }
    }
}
