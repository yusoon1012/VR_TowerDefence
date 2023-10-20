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
    [SerializeField] private int bladeDamage = default;
    // (CSV) 근거리 타격 유닛 회전속도
    [SerializeField] private int bladeRotateSpeed = default;
    // 칼날
    private Transform[] blades = default;
    // 칼날 회전 여부
    private bool rotateBlade = false;

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
        blades = transform.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        bladeHP = transform.GetComponent<AttackUnitProperty>().HP;
        bladeSpeed = transform.GetComponent<AttackUnitProperty>().speed;
        bladeDamage = transform.GetComponent<AttackUnitProperty>().damage;
        bladeRotateSpeed = transform.GetComponent<AttackUnitProperty>().rotateSpeed;
    }

    /// <summary>
    /// 감지한 졸개 배열
    /// </summary>
    private GameObject[] EnemyGotcha
    {
        get
        {
            int range = 1 / 2; // 감지 반경 1m X 1m (절반 나누기)

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
        float rotationsPerSecond = 3f; // 초당 회전 횟수
        float rotateValue = 360f / rotationsPerSecond; // 초당 3회 회전

        if (rotateBlade)
        {
            for (int i = 0; i < blades.Length; i++)
            {
                blades[i].RotateAround(transform.position, Vector3.up, rotateValue * Time.deltaTime);
            }
        }
        else
        {
            // 칼날 회전 정지
            Quaternion stopRotate = Quaternion.identity;

            for (int i = 0; i < blades.Length; i++)
            {
                blades[i].rotation = stopRotate;
            }
        }
    }

    // TODO: 졸개에 대한 데미지 처리 
    // MonsterInfo의 MonsterDamaged(임시값)

    private IEnumerator AttackEnemy()
    {
        while (rotateBlade) // 칼날이 돌아가고 있는 동안
        {
            foreach (GameObject enemy in EnemyGotcha)
            {
                enemy.GetComponent<MonsterInfo>().MonsterDamaged(bladeDamage); 
            }

            yield return new WaitForSeconds(bladeSpeed);
        }

        yield break;
    }
}
