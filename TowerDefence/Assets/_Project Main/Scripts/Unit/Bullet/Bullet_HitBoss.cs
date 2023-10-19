using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_HitBoss : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            // TODO: Boss HP Damage
            GameObject finalBoss = collision.gameObject;
            finalBoss.GetComponent<FinalBoss>().finalBossHp -= 75; // TODO: CSV를 통해 변경 가능하도록 수정. 
        }
    }
}
