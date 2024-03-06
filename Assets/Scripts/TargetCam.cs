using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCam : MonoBehaviour
{
    //Follow 카메라 따라다니면서 위치를 조정
    [Header("Follow")]
    public Transform followTarget; //따라갈 대상
    public Vector3 followOffset = Vector3.zero; //따라갈 위치를 보정하는 값
    public float followSpeed = 3f; //따라갈 속도(부드럽게 따라가는 정도)
    public float rotateSpeedX = 10f; //수평회전 속도
    [SerializeField]
    private float angleY = 0; // 수평 회전 상태에 대한 임의의 값

    //Look 카메라 쳐다보면서 회전를 조정
    [Header("Look")]
    public Transform lookTarget; //바라볼 대상
    public Vector3 lookOffset = Vector3.zero; //바라볼 위치를 보정하는 값
    public float zoomSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position
            = followTarget.position + followTarget.rotation * followOffset;

        //angleY = 0; 
        //수정 (위에서 아래로) 주인공의 값 추기값으로 세팅
        //기존 코드는 주인공이 회전된채로 실행되면 어긋남
        angleY = followTarget.eulerAngles.y;
    }

    //늦은 업데이트
    //(모든 업데이트가 끝나고 실행)
    private void LateUpdate()
    {
        //여기에 넣은 코드는 매번 모든 업데이트후에 실행

    }
    //고정되 주기마다 업데이트
    //(물리주기,훨씬 늦게 반복, 기본으로 0.02마다 반복)
    private void FixedUpdate()
    {
        //여기에 넣은 업데이트 보다 늦은 주기로 반복
        //물리주기에 맞춰서 반복
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
            //카메라의 컬링마스크를 Player제외하고 보도록 변경
            Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("Player"));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //카메라의 컬링마스크를 everything으로 변경
            Camera.main.cullingMask = -1;
        }
    }
}
