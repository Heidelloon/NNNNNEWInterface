using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public bool isAvliableUse = false;

    private bool isLightOn = false; //true일 경우 손전등on
    private Light flashLight; //light 컴포넌트를 담는 변수

    void Start()
    {
        flashLight = this.GetComponent<Light>(); //오브젝트가 가진 light 컴포넌트를 가져옴.
    }

    void Update()
    {
        if (!isAvliableUse) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            isLightOn = !isLightOn; //f키를 눌러 손전등의 불빛을 on/off
        }

        flashLight.enabled = isLightOn;
    }
}