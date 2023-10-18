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

        if (collision.gameObject.layer == enemyMask) // 충돌한 게 졸개 레이어마스크를 가지고 있다면 
        {
            Debug.Log("폭발!");
        }
    }
}
