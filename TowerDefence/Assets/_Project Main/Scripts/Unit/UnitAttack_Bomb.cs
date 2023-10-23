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
    [SerializeField] private float damage = default; // (CSV) 폭발 유닛의 공격력
    [SerializeField] private int explosionRange = default; // (CSV) 폭발 범위
    [SerializeField] private int bombHP = default; // (CSV) 폭발 유닛 HP

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
        buildSystem = GameObject.Find("Unit Manager").GetComponent<UnitBuildSystem>();
    }

    private void Start()
    {
        damage = transform.GetComponent<AttackUnitProperty>().damage;
        explosionRange = transform.GetComponent<AttackUnitProperty>().attackRange;
        bombHP = transform.GetComponent<AttackUnitProperty>().HP;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // 충돌한 것이 졸개 태그라면
        {
            Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // 충돌한 것이 졸개 태그라면
        {
            Explosion();
        }
    }

    private GameObject[] Enemies
    {
        get
        {
            float radius = explosionRange / 2f; // 폭발 반경

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
        Transform effectList = transform.GetChild(1);
        ParticleSystem[] effects = effectList.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem effect in effects) 
        {
            Debug.Assert(effect);
            effect.Play(); // 파티클 시스템 플레이

            Invoke("BackPool", 0.3f); // 폭발 유닛 풀로 복귀 
        }
 
        for (int i = 0; i < Enemies.Length; i++)
        {
            Debug.LogFormat("{0}에게 데미지 처리", Enemies.Length);
            //Enemies[i].transform.GetComponent<MonsterInfo>().MonsterDamaged(damage); // TODO: 졸개 나오면 주석 해제
        }
    }

    private void BackPool() { buildSystem.ReturnPool(gameObject); }
}
