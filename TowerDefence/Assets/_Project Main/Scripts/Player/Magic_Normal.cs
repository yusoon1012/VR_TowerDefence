using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Normal : MonoBehaviour
{
    public float speed=30f;
    public int damage;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {
            Boss_Hp_Test bossHp=collision.gameObject.GetComponent<Boss_Hp_Test>();
            if(bossHp!=null)
            {
                bossHp.LoseHp(damage);
                Destroy(gameObject);
            }
        }
    }
}
