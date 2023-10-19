using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// 설치/공격형 유닛: 졸개 접근 시 폭발
/// </summary>
public class UnitAttack_Bomb : MonoBehaviour
{
    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int enemyMask = 1 << LayerMask.NameToLayer("Enemy"); // 졸개 레이어마스크

        if (collision.gameObject.layer == enemyMask) // 충돌한게 졸개 레이어마스크를 가지고 있다면 
        {
            Explosion();
        }
    }

    private Collider[] Enemies()
    {
        float radius = 15f; // 폭발 반경
        int enemyMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] enemyList = Physics.OverlapSphere(transform.position, radius, enemyMask); // 검출된 졸개 배열
        return enemyList;
    }

    private void Explosion()
    {
        // TODO: foreach문을 돌려 Enemies 게임오브젝트의 속성 스크립트를 가져와 HP를 깎는다. 
    }
}
