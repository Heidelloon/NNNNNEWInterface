using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerCam : MonoBehaviour
{
    public Transform targetPlayer;
    public InputActionReference CamRotate;
    public InputActionReference CamZoom;

    [Header("Follow")]
    public Vector3 followOffset;
    public float followSpeed = 7;
    [SerializeField]
    private float angle1;
    public float rotateSpeed = 5f;
    
    [Header("LootAt")]
    public Vector3 lookOffset;
    public float lookSpeed = 10f;
    void Start()
    {
        /* Vector3 starPos = targetPlayer.transform.rotation * followOffset;
         transform.position = starPos;
        */

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        angle1 = targetPlayer.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        CamMove();
    }

    private void Update()
    {
        Vector2 camRotinput = CamRotate.action.ReadValue<Vector2>();
        float camZoomInput = CamZoom.action.ReadValue<float>();

        //Debug.Log (camRotinput);

        //입력처리 하는 부분
        angle1 += camRotinput.x * rotateSpeed * Time.deltaTime;

        //줌인 줌아웃
        float nextFOV = Camera.main.fieldOfView + camZoomInput * Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Clamp(nextFOV, 30, 120);
    }

    void CamMove()
    {
        //따라가는 부분
        Vector3 followPos = targetPlayer.position + Quaternion.Euler(0, angle1, 0) * followOffset;
        transform.position = Vector3.Lerp(transform.position, followPos, Time.deltaTime * followSpeed);
     

        //바라보는 부분
        Vector3 lookPos = targetPlayer.position + lookOffset;
        Vector3 lookDir = lookPos - transform.position;
        lookDir = lookDir.normalized;

        Quaternion lookQuat = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookQuat, Time.deltaTime * lookSpeed);
    }
}
