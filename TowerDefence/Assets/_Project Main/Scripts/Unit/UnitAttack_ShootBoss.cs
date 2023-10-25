using Meta.WitAi;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitAttack_ShootBoss : MonoBehaviour
{
    public GameObject stonePrefab = default; // 투창 프리팹 
    public GameObject[] stones = default; // 투석용 
    private int stoneNumber = 0; // 사용한 돌의 개수
    private Transform firePosition; // 발사 위치
    private Vector3 poolPos = new Vector3(0f, -10f, 0f); // 풀 포지션
    private Vector3 bossPosition = default; // 타겟 포지션 (보스)
    // 유닛 데미지
    public float shootBossDamage = default;
    // 유닛 HP
    public int shootBossHP = default; //(CSV) 보스 타격 유닛HP
    private bool hit = false; // 피격 상태 체크
    float time = 0f; // 피격 타임

    FinalBoss finalBoss = default; // 파이널 보스 스크립트
    MidBoss middleBoss = default; // 미드보스 스크립트

    Transform pivot = default; // 투석 시 회전
    float initialSpeed = 10f; // 초기 회전 속도
    float decelaration = 0.5f; // 감속도
    bool fire = false; // 투척 중
    bool pivotRotate = false; // 회전
    bool startFire = false; // 투척 대기 중

    GameObject boss = default;
    GameObject midBoss = default;

    private event EventHandler throwSphere;

    Shop_Unit shop_Unit = new Shop_Unit();

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);

        firePosition = transform.GetChild(1);
        pivot = transform.GetChild(0).transform.GetChild(1);

        stones = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            stones[i] = Instantiate(stonePrefab, poolPos, Quaternion.identity);
        }

        throwSphere += PivotRotate;

        shop_Unit = GameObject.Find("Shop").GetComponent<Shop_Unit>();
    }

    private void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        Debug.Assert(boss != null);
        midBoss = GameObject.FindWithTag("MidBoss");
        Debug.Assert(midBoss != null);
        finalBoss = boss.GetComponent<FinalBoss>();
        middleBoss = midBoss.GetComponent<MidBoss>();

        bossPosition = boss.transform.localPosition;
        bossPosition.y = boss.GetComponent<Collider>().bounds.size.y * 0.55f; // 보스 키의 55% 지점 타격

        shootBossDamage = transform.GetComponent<AttackUnitProperty>().damage;
        shootBossHP = transform.GetComponent<AttackUnitProperty>().HP;

        for (int i = 0; i < 10; i++)
        {
            stones[i].GetComponent<Bullet_HitBoss>().damage = shootBossDamage;
        }
    }

    private void Update()
    {
        FindBoss();

        // 반경 150 내 보스 존재 && 유닛 설치 활성화 상태에서 유닛 설치를 클릭했을때
        if (Distance < 1000f && shop_Unit.buildShootBoss && 
            ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            startFire = true;
        }

        #region 유닛이 피격 당함
        FindEnemy(); // 피격 체크

        if (hit) // 피격 상태라면
        {
            int delayHit = 5; // 데미지 타이머 

            if (time < delayHit)
            {
                time += Time.deltaTime;
            }
            else if (time >= delayHit)
            {
                time = 0f; // 타임 초기화
                shootBossHP -= 5; // 피격 처리 (TODO: 졸개 공격력에서 가져오는 것으로 처리?)
            }
        }

        if (shootBossHP <= 0)
        {
            // TODO: 유닛 파괴
            fire = false;
            shootBossHP = shootBossHP = transform.GetComponent<AttackUnitProperty>().HP;
        }
        #endregion

        // 보스 무적 시간, 중간 보스 등장
        if (finalBoss.bossImmotalForm) 
        {
            bossPosition = midBoss.transform.position;
            bossPosition.y = midBoss.GetComponent<Collider>().bounds.size.y * 0.55f;
            
        }
        else if (!finalBoss.bossImmotalForm) // 보스 무적 시간이 아니라면 
        {
            bossPosition = boss.transform.localPosition;
            bossPosition.y = boss.GetComponent<Collider>().bounds.size.y * 0.55f; // 보스 키의 55% 지점 타격
        }
    }

    public void StartFire() { StartCoroutine(ReadyFire()); } // 발사 시작


    /// <summary>
    /// 근처에 졸개가 있는지 검색하는 메서드 
    /// </summary>
    private void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 졸개 태그를 달고 있는 오브젝트 배열 검출

        for (int i = 0; i < enemies.Length; i++) // 모든 적 검사
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) <= 10f) // 거리 1 안쪽으로 적이 존재
            {
                hit = true;
            }
            else
            {
                hit = false;
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
        float rotationSpeed = 10f;

        Vector3 unitPos = transform.position;
        unitPos.y = 0.5f;
        Vector3 bossPos = bossPosition;
        bossPos.y = 0.5f;

        Vector3 direction = (bossPos - unitPos).normalized; // 유닛 방향값 계산
        Quaternion unitRotation = Quaternion.LookRotation(direction);
        Debug.Log(direction);
        Debug.Log(unitRotation);
        transform.rotation =
            Quaternion.Slerp(transform.rotation, unitRotation, rotationSpeed * Time.deltaTime);
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

        if (stoneNumber == 10) // 9번 돌까지 소모
        {
            stoneNumber = 0; // 초기화
        }

        stones[stoneNumber].transform.position = firePosition.position; // 0~9
        sphereRigid = stones[stoneNumber].GetComponent<Rigidbody>();
        stoneNumber++;

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
                pivot.transform.Rotate(-Vector3.up * Time.fixedDeltaTime * degree);
            }
            else if (currentYRotation >= 75) // 75가 회전각도
            {
                pivot.transform.rotation = pivot.transform.rotation; // 회전 정지
            }
        }
        else if (fire && !pivotRotate) // 재투척 준비
        {
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
        Vector3 previousPoint = firePosition.position;
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
