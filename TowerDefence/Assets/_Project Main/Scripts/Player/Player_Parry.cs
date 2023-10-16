using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Parry : MonoBehaviour
{
    float speed;
    bool isParriable=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
        if (speed >= 1 )
        {
            Debug.Log("1보다 빠르다");
            isParriable = true;
        }
        else
        {
            isParriable = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            Rigidbody rb= other.GetComponent<Rigidbody>();
            if(rb != null )
            {
                Vector3 dir=(rb.position-transform.position).normalized;
                rb.velocity = Vector3.zero;
                rb.AddForce(dir*speed,ForceMode.Impulse);
            }
        }
    }
}
