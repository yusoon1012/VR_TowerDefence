using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설치/공격형 유닛: 졸개의 접근이 확인되면 칼날 회전
/// </summary>
public class UnitAttack_Blade : MonoBehaviour
{
    // (CSV) 근거리 타격 유닛 HP
    [SerializeField] private int bladeHP = default;
    // (CSV) 근거리 타격 유닛 공격속도
    [SerializeField] private float bladeSpeed = default;
    // (CSV) 근거리 타격 유닛 공격력
    [SerializeField] private float bladeDamage = default;
    // (CSV) 근거리 타격 유닛 회전속도
    [SerializeField] private int bladeRotateSpeed = default;

    // 칼날
    private Transform blade = default;
    // 칼날 회전 여부
    private bool rotateBlade = false;

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
        blade = transform.GetChild(0);
    }

    private void Start()
    {
        bladeHP = transform.GetComponent<AttackUnitProperty>().HP;
        bladeSpeed = transform.GetComponent<AttackUnitProperty>().speed;
        bladeDamage = transform.GetComponent<AttackUnitProperty>().damage;
        bladeRotateSpeed = transform.GetComponent<AttackUnitProperty>().rotateSpeed;
    }

    #region 졸개 감지
    /// <summary>
    /// 감지한 졸개 배열
    /// </summary>
    private GameObject[] EnemyGotcha
    {
        get
        {
            float range = 1 / 2; // 감지 반경 1m X 1m (절반 나누기)

            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(range, 0.5f, range),
                Quaternion.identity);
            GameObject[] targetEnemies = new GameObject[colliders.Length]; // 감지한 적 오브젝트 배열

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.CompareTag("Enemy")) // 적 태그만 감지
                {
                    targetEnemies[i] = colliders[i].gameObject;
                }
            }

            return targetEnemies;
        }
    }
    #endregion 

    /// <summary>
    /// 첫 진입 시 공격
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // 졸개 태그면
        {
            AttackDamage(other.gameObject); // 데미지 호출
        }    
    }

    /// <summary>
    /// 이후 지속 공격
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")) // 졸개 태그면
        {
            float time = 0f;

            if (time < bladeSpeed)
            {
                time += Time.deltaTime;
            }
            else if (time >= bladeSpeed)
            {
                AttackDamage(other.gameObject); // 데미지 호출
                time = 0f;
            }
        }
    }

    private void Update()
    {
        if (EnemyGotcha.Length > 0) // 반경 내 졸개 하나 이상 검출
        {
            rotateBlade = true; // 칼날 회전
            //StartCoroutine(AttackEnemy()); // Null 발생
        }
        else rotateBlade = false; // 아니면 정지
    }

    private void FixedUpdate()
    {
        int rotationsPerSecond = 3; // 초당 회전 횟수
        float rotateValue = 360f / rotationsPerSecond; // 초당 3회 회전

        if (rotateBlade)
        {
            blade.Rotate(0, rotateValue * Time.deltaTime, 0); // 중심으로 돌기?
        }
        else
        {
            blade.rotation = blade.rotation; // 칼날 회전 정지
        }
    }

    // MonsterInfo의 MonsterDamaged(임시값)
    private void AttackDamage(GameObject enemy)
    {
        // TODO: 졸개에 대한 데미지 처리 
        Debug.Log("데미지 처리");
    }
}
