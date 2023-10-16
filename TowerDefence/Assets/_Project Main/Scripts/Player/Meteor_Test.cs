using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Test : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player_Shoot>().gameObject;
        rb=GetComponent<Rigidbody>();
        Vector3 dir=(player.transform.position- transform.position).normalized;
        rb.velocity = dir * 10;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
