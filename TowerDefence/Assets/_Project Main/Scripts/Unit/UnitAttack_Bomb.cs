using System;
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
    UnitBuildSystem buildSystem;

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
        buildSystem = GameObject.Find("Unit Manager").GetComponent<UnitBuildSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // 충돌한 것이 졸개 태그라면
        {
            Debug.Log("폭발!");
            Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("폭발!");
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
        Transform effectList = transform.GetChild(0);
        ParticleSystem[] effects = effectList.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem effect in effects) 
        {
            Debug.Assert(effect);
            effect.Play(); // 파티클 시스템 플레이

            buildSystem.ReturnPool(gameObject); // 본 유닛을 파괴하라는 메서드
        }
 
        for (int i = 0; i < Enemies.Length; i++)
        {
            //Enemies[i].transform.GetComponent<MonsterInfo>().MonsterDamaged(15); // 15만큼 공격
        }
    }
}
