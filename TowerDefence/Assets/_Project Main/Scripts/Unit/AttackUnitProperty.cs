using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설치/공격형 유닛의 속성 정보
/// </summary>
public class AttackUnitProperty : MonoBehaviour
{
    public int attackRange; // 공격범위
    public int damage; // 데미지(공격력)
    public int attackCount; // 공격대상
    public float speed; // 공격속도
    public int rotateSpeed; // 회전속도
    public int HP; // 체력
}
