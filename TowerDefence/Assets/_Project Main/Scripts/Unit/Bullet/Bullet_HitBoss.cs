using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_HitBoss : MonoBehaviour
{
    FinalBoss finalBoss = new FinalBoss();
    public float damage = default; // UnitAttack_ShootBoss에서 전달받는 데미지 계수
    private Vector3 poolPos = new Vector3(0, -10, 0); // 풀 포지션

    private void Start()
    {
        GameObject shootBossUnit = GameObject.FindAnyObjectByType<UnitAttack_ShootBoss>().gameObject;
        damage = shootBossUnit.GetComponent<AttackUnitProperty>().damage;
    }

    #region Legacy
    private void OnCollisionEnter(Collision collision) // TODO: 데미지를 한 번만 입히도록
    {
        if (collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("MidBoss"))
        {
            Debug.Log("보스 유닛: 적을 공격");
            GameObject finalBoss = collision.gameObject;

            int realDamage = (int)damage;
            finalBoss.GetComponent<FinalBoss>().HitDamage(realDamage); // 보스에 데미지를 입힘. 
            
            ReturnPool();
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss") || other.gameObject.CompareTag("MidBoss"))
        {
            Debug.Log("보스 유닛: 적을 공격");
            GameObject finalBoss = other.gameObject;

            int realDamage = (int)damage;
            finalBoss.GetComponent<FinalBoss>().HitDamage(realDamage); // 보스에 데미지를 입힘. 

            ReturnPool();
        }
    }

    private void ReturnPool() { transform.position = poolPos; } // 풀로 복귀
}
