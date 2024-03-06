using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


public class phone : MonoBehaviour
{
    public PMove PMove;

    public Transform handPos;

    Rigidbody body;

    Collider phcollider;

    public bool onplayer2 = false;

    public bool onitem = false;  

    // Start is called before the first frame update
    void Start()
    {
        phcollider = GetComponent<Collider>();

        body = GetComponent<Rigidbody>();

        onplayer2 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
           SE();
        }
        else 
        {
            onplayer2 = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onplayer2 == true)
        {
            if (other.gameObject.CompareTag("player"))
            {
                Debug.Log("씨발?");

                if (Input.GetKey(KeyCode.L))
                {
                    onitem = true;

                    Debug.Log(" ehoa?");

                    transform.position = handPos.transform.position;

                    transform.rotation = handPos.transform.rotation;

                    Transform playerTransform = other.transform;
                    transform.parent = handPos.transform;

                    SetEgint(gameObject, true);
                }

            }
        }
    }

    void SE()
    {

        onplayer2 = false;

        // 부모 해제
        transform.parent = null;


        GameObject item = PMove.transform.GetComponentInChildren<Rigidbody>().gameObject;

        SetEgint(item, false);
    }



    void SetEgint(GameObject gameObject, bool isEgint)
    {
        Collider[] itemcolliders = GetComponents<Collider>();

        foreach (Collider itemcollider in itemcolliders)
        {
            itemcollider.enabled = !isEgint;
        }
        body.isKinematic = isEgint;
    }
}
