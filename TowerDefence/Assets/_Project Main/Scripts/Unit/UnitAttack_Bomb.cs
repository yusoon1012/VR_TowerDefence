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

    /// <summary>
    /// 졸개를 감지하는 메서드
    /// </summary>
    private Collider[] EnemyGotcha
   {
        get
        {
            float radius = 5f; // 졸개 탐지 반경
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // 졸개 레이어마스크
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyLayer); // 졸개 검출

            return colliders;
        }
   }

    private void Update()
    {
        if (EnemyGotcha.Length > 0) // 반경 내 졸개 하나 이상 검출
        {
            // TODO: 졸개 & 이펙트 추가 후 폭발 처리 짜기
            Debug.Log("폭발!");
        }
    }
}
