using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_HitBoss : MonoBehaviour
{
    FinalBoss finalBoss = new FinalBoss();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            Debug.Log("보스 타격!");

            GameObject finalBoss = collision.gameObject;
            finalBoss.GetComponent<FinalBoss>().HitDamage(10); // TODO: 데미지 임시값 교체 
        }

        // TODO: (임시) Destroy, (후에 수정) 오브젝트 풀링
    }
}
