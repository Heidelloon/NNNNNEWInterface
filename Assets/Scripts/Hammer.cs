using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject hammerObject;
    public Transform handPos;
    public float throwPower = 5f;

    public void ThrowHammerObject()
    {
        GameObject hammer = Instantiate(hammerObject);
        hammer.transform.position = handPos.position;

        hammer.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.up) * throwPower);
    }
}
