using Meta.WitAi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitAttack_ShootBoss : MonoBehaviour
{
    public GameObject stonePrefab = default; // 투창 프리팹 
    //GameObject[] spheres = default; // 투창
    GameObject stone = default; // 테스트
    private Transform firePosition; // 발사 위치
    private Vector3 poolPos = new Vector3(0f, -10f, 0f); // 풀 포지션
    private Vector3 bossPosition = default; // 타겟 포지션 (보스)

    // 유닛 HP
    public int shootBossHP = default; //(CSV) 보스 타격 유닛HP
    private bool hit = false; // 피격 상태 체크
    float time = 0f; // 피격 타임

    Transform pivot = default; // 투석 시 회전
    float initialSpeed = 10f; // 초기 회전 속도
    float decelaration = 0.5f; // 감속도
    bool fire = false; // 투척 중
    bool pivotRotate = false; // 회전

    private event EventHandler throwSphere;

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
        GameObject boss = GameObject.FindWithTag("Boss");
        Debug.Assert(boss != null);
        bossPosition = boss.transform.position;
        bossPosition.y = boss.GetComponent<Collider>().bounds.size.y * 0.55f; // 보스 키의 55% 지점 타격

        firePosition = transform.GetChild(1);
        pivot = transform.GetChild(0).transform.GetChild(1);

        throwSphere += PivotRotate;
    }

    private void Start()
    {
        shootBossHP = transform.GetComponent<AttackUnitProperty>().HP;
        //Invoke("TestStart", 3f); // 테스트용
    }

    //private void TestStart() { StartCoroutine(ReadyFire()); } // 테스트용

    private void Update()
    {
        // TODO: 유닛 구매 여부 추가 
        if (Distance < 1000f) // 반경 150 내 보스 존재 (임시값: 1000)
        {
            FindBoss();
        }

        // TODO: 유닛 구매 & 배치 상태에서 작동하도록 변경 
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            StartCoroutine(ReadyFire());
        }

        #region 유닛이 피격 당함
        FindEnemy(); // 피격 체크

        if (hit) // 피격 상태라면
        {
            int delayHit = 5; // 5초에 한 번씩 피격

            if (time < delayHit)
            {
                time += Time.deltaTime;
            }
            else if (time >= delayHit)
            {
                time = 0f; // 타임 초기화
                shootBossHP -= 5; // 피격 처리 (TODO: 졸개 공격력에서 가져오는 것으로 처리?)
                Debug.LogFormat("HP: {0}", shootBossHP);
            }
        }
        #endregion
    }

    /// <summary>
    /// 근처에 졸개가 있는지 검색하는 메서드 
    /// </summary>
    private void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 졸개 태그를 달고 있는 오브젝트 배열 검출

        for (int i = 0; i < enemies.Length; i++) // 모든 적 검사
        {
            // TODO: 유닛과의 거리 계산, 만약 일정 거리 내 있다면 유닛은 피격됨. 
            if (Vector3.Distance(transform.position, enemies[i].transform.position) <= 10f) // 거리 1 안쪽으로 적이 존재
            {
                hit = true;
            }
        }
    }

    #region 투척
    /// <summary>
    /// 보스 - 유닛 거리
    /// </summary>
    private float Distance
    {
        get
        {
            Vector3 unitPos = transform.position;
            unitPos.y = 0.5f;
            Vector3 bossPos = bossPosition;
            bossPos.y = 0.5f;

            float distance = Vector3.Distance(bossPos, unitPos);
            return distance;
        }
    }

    /// <summary>
    /// 보스를 향해 회전한다
    /// </summary>
    private void FindBoss()
    {
        float rotationSpeed = 10f; // 회전속도 (TODO: 이후 수정 필요)

        Vector3 unitPos = transform.position;
        unitPos.y = 0.5f;
        Vector3 bossPos = bossPosition;
        bossPos.y = 0.5f;

        Vector3 direction = (bossPos - unitPos).normalized; // 유닛 방향값 계산
        Quaternion unitRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, unitRotation, rotationSpeed * Time.deltaTime);

        Debug.Log("회전 완료");

    }

    /// 연사 시간 조정
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadyFire()
    {
        fire = true;

        while (fire)
        {
            throwSphere?.Invoke(this, EventArgs.Empty);
            Fire();

            yield return new WaitForSeconds(2.5f); // 연사 대기 시간
        }
    }

    /// <summary>
    /// 발사
    /// </summary>
    private void Fire()
    {
        Vector3 velocity = CaculateVelocity(bossPosition, transform.position, 2f); // Velocity 계산은 항시

        Rigidbody sphereRigid = default; // 테스트

        stone = Instantiate(stonePrefab, firePosition.position, Quaternion.identity);
        sphereRigid = stone.GetComponent<Rigidbody>();

        sphereRigid.velocity = velocity;

        Path(velocity); // CaculateVelocity()의 계산값을 궤적을 그리는데 이용
    }

    void PivotRotate(object sender, EventArgs e) { pivotRotate = true; } // 투척 시 이벤트 발생

    private void FixedUpdate()
    {
        if (fire && pivotRotate) // 앞으로 투척 
        {
            float degree = 200f;
            float currentYRotation = pivot.transform.rotation.eulerAngles.y;

            if (currentYRotation < 75)
            {
                Debug.Log("이동 시도");
                pivot.transform.Rotate(-Vector3.up * Time.fixedDeltaTime * degree);
            }
            else if (currentYRotation >= 75) // 75가 회전각도
            {
                Debug.Log("정지 시도");
                pivot.transform.rotation = pivot.transform.rotation; // 회전 정지
            }
        }
        else if (fire && !pivotRotate) // 재투척 준비
        {
            Debug.Log("재투척 준비");
        }
    }

    /// <summary>
    /// 계산
    /// time = 비행 시간
    /// </summary>
    private Vector3 CaculateVelocity(Vector3 bossPos, Vector3 unitPos, float time)
    {
        // 거리 차 구하기
        Vector3 distance = bossPos - unitPos; // 실거리
        Vector3 distanceXZ = distance; // 평면 거리
        distanceXZ.y = 0f;

        float distanceY = distance.y; // 높이 차
        float magXZ = distanceXZ.magnitude;

        // 속도 계산
        float velXZ = magXZ / time;
        float velY = distanceY / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        // 최종 계산
        Vector3 result = distanceXZ.normalized;
        result *= velXZ;
        result.y = velY;

        return result;

    }

    /// <summary>
    /// 궤적
    /// </summary>
    private void Path(Vector3 velocity)
    {
        Vector3 previousPoint = firePosition.position; // TODO: 에셋 추가 후 총구 위치로 변경 
        int resolution = 30; // 궤적 내 경유 포인트

        for (int i = 0; i < resolution; i++)
        {
            // float simulationTime = i / (float)resolution * launchData.timeToTarget;
            float simulationTime = i / (float)resolution * 1f;

            Vector3 displacement = velocity * simulationTime + Vector3.up * Physics.gravity.y * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = transform.position + displacement;
            Debug.DrawLine(previousPoint, drawPoint, Color.green);
            //lineRenderer.SetPosition(i - 1, drawPoint);
            previousPoint = drawPoint;
        }
    }
    #endregion
}
