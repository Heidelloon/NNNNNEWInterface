using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammer : MonoBehaviour
{
    public PMove PMove;

    public Transform handPos;

  public Rigidbody body;

    Collider phcollider;

    public bool onplayer = false;

    public float throwPower = 5f;

    public bool onitem = false;

    // Start is called before the first frame update
    void Start()
    {
        phcollider = GetComponent<Collider>();

        body = GetComponent<Rigidbody>();

        onplayer = true;
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetKey(KeyCode.T))
      //  {
      //      onplayer = false;

       //     drop1();
      //  }

      //  else
    //    {
      //      onplayer = true;
    //}

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (onplayer == true)
        {
            if (other.gameObject.CompareTag("player"))
            {
                Debug.Log("플레이어와 만남");

                if(Input.GetKey(KeyCode.L))
                {
                    onitem = true;

                    Debug.Log("무기 장착!");

                    transform.position = handPos.transform.position;

                    transform.rotation = handPos.transform.rotation;

                    Transform playerTransform = other.transform;
                    transform.parent = handPos.transform;

                    SetEgint(gameObject, true);
                }
            }
        }
    }
    
    void drop1()
    {

        onplayer = false;

        // 부모 해제
        transform.parent = null;


        GameObject item = PMove.transform.GetComponentInChildren<Rigidbody>().gameObject;

        SetEgint(item, false);
    }

  public void SetEgint(GameObject gameObject, bool isEgint)
  {
        Collider[] itemcolliders = GetComponents<Collider>();

        foreach (Collider itemcollider in itemcolliders)
        {
            itemcollider.enabled = !isEgint;
        }
        body.isKinematic = isEgint;
  }
}
