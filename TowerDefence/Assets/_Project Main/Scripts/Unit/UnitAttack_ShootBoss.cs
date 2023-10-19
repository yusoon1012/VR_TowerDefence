using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack_ShootBoss : MonoBehaviour
{
    public GameObject spherePrefab = default; // 투창 프리팹 
    //GameObject[] spheres = default; // 투창
    GameObject sphere = default; // 테스트
    private Transform firePosition; // 발사 위치
    private Vector3 poolPos = new Vector3(0f, -10f, 0f); // 풀 포지션
    private Vector3 bossPosition = default; // 타겟 포지션 (보스)

    private int unitHP = 100;

    private void Awake()
    {
        UnitBuildSystem.units.Add(transform.gameObject);
        GameObject boss = GameObject.FindWithTag("Boss");
        bossPosition = boss.transform.position;
        bossPosition.y = boss.GetComponent<Collider>().bounds.size.y * 0.55f; // 보스 키의 55% 지점 타격

        firePosition = transform.GetChild(0);
    }

    private void Update()
    {
        // TODO: 유닛 구매 여부 추가 
        if (Distance < 150f) // 반경 150 내 보스 존재
        {
            FindBoss();
        }

        // TODO: 유닛 구매 & 배치 상태에서 작동하도록 변경 
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            StartCoroutine(ReadyFire());
        }
    }

    //private float HitDistance
    //{
      
    //}

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

    }

    /// <summary>
    /// 연사 시간 조정
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReadyFire()
    {
        bool fire = true;

        while (fire)
        {
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

        sphere = Instantiate(spherePrefab, firePosition.position, Quaternion.identity);
        sphereRigid = sphere.GetComponent<Rigidbody>();

        sphereRigid.velocity = velocity;

        Path(velocity); // CaculateVelocity()의 계산값을 궤적을 그리는데 이용
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
