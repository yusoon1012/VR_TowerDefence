using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Normal : MonoBehaviour
{
    public float speed=30f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.velocity = Vector3.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
