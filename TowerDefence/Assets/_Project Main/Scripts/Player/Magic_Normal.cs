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
        Destroy(gameObject, 30f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {
            FinalBoss bossHp=collision.gameObject.GetComponent<FinalBoss>();
            if(bossHp!=null)
            {
                bossHp.HitDamage(damage);
                Destroy(gameObject);
            }
        }
        if(collision.gameObject.CompareTag("MidBoss"))
        {
            MidBoss midboss=collision.gameObject.GetComponent<MidBoss>();
            if(midboss!=null)
            {
                midboss.HitDamage(damage);
                Destroy(gameObject);
            }
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            MonsterInfo monster=collision.gameObject.GetComponent<MonsterInfo>();
            if(monster != null)
            {
                monster.MonsterDamaged(damage);
            }

        }
    }
}
