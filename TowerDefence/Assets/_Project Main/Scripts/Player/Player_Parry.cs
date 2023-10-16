using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Parry : MonoBehaviour
{
    public ParticleSystem particle;
    Boss_Hp_Test boss;
    float speed;
    bool isParriable=false;
    // Start is called before the first frame update
    void Start()
    {
        boss=FindAnyObjectByType<Boss_Hp_Test>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude;
        if (speed >= 1f )
        {
            Debug.Log("1보다 빠르다");
            isParriable = true;
        }
        else
        {
            isParriable = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Meteor"))
        {
            if(isParriable) 
            {
            Rigidbody rb= other.GetComponent<Rigidbody>();
            if(rb != null )
            {
                Vector3 dir=(boss.transform.position-transform.position).normalized;
                rb.velocity = Vector3.zero;
                rb.AddForce(dir*(speed*5),ForceMode.Impulse);
                particle.Play();
            }
            }
        }
    }
}
