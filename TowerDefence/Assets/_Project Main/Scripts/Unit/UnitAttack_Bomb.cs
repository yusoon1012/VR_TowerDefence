using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
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
        if (collision.gameObject.CompareTag("Enemy")) // 충돌한 것이 졸개 태그라면
        {
            Explosion();
        }
    }

    private GameObject[] Enemies
    {
        get
        {
            float radius = 16 / 2f; // 폭발 반경

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius); // 검출 배열
            GameObject[] enemies = new GameObject[colliders.Length]; // 검출 적 배열

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.CompareTag("Enemy")) // 졸개 태그면
                {
                    enemies[i] = colliders[i].gameObject; // 추가
                }
            }
            return enemies;
        }
    }

    private void Explosion()
    {
        // TODO: 이펙트 추가

        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].transform.GetComponent<MonsterInfo>().MonsterDamaged(15); // 15만큼 공격
        }
    }
}
