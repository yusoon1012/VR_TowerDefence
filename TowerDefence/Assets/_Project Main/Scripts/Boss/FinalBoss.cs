using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    // { 변수 설정
    // 보스 HP 량
    public int finalBossHp = 100;
    // 보스 페이즈
    public int finalBossPhase = default;
    // 졸개 소환 쿨타임
    public float spawnSoldierTime = 20f;
    // 스펠 발사 쿨타임
    public float throwSphereTime = 15f;
    // 보스 무적 상태 체크
    public bool bossImmotalForm = false;

    //// 졸개를 소환할 HP 단계
    //public int spawnSoldierStep = 10;

    // 스펠 발사 전 스펠의 초기 위치
    private Vector3 bossFireBallShootPosition = Vector3.zero;

    // 보스 게임 오브젝트
    private GameObject finalBossObj = default;
    // 중간 보스 게임 오브젝트
    private GameObject midBossObj = default;
    // 발사 스펠의 오브젝트
    private GameObject fireBall = default;

    // 보스가 패턴을 실행하기까지의 시간값
    private float actionTime = default;
    // 보스가 패턴을 실행하기까지 걸리는 시간 최대값
    private float actionMaxTime = default;
    // 보스 HP 가 80% 남을 때 수치 체크
    private float boss80Hp = default;
    // 보스 HP 가 40% 남을 때 수치 체크
    private float boss40Hp = default;
    // 스펠 발사 쿨타임 까지의 중첩 값
    private float spawnSoldierTimepass = default;
    // } 변수 설정

    void Awake()
    {
        // { 초기 변수값 설정
        // 보스 오브젝트 참조
        finalBossObj = GetComponent<GameObject>();
        // 중간 보스의 오브젝트를 찾아서 참조
        midBossObj = GameObject.Find("MidBoss");
        fireBall = GameObject.Find("BossFireBall");

        bossFireBallShootPosition = new Vector3(0f, 100f, 145f);

        actionTime = 0f;
        actionMaxTime = 5f;
        boss80Hp = 0;
        boss40Hp = 0;
        finalBossPhase = 0;
        spawnSoldierTimepass = 0f;
        // } 초기 변수값 설정
    }     // Awake()

    void Start()
    {
        // 보스 HP 에서 80% 수치 값 저장
        boss80Hp = finalBossHp * 0.8f;
        // 보스 HP 에서 40% 수치 값 저장
        boss40Hp = finalBossHp * 0.4f;

        StartCoroutine(MidBossTest());
    }     // Start()

    void Update()
    {
        // 실시간으로 졸개 소환 쿨타임을 진행함
        spawnSoldierTimepass += Time.deltaTime;
        // 졸개 소환 쿨타임이 완료되면
        if (spawnSoldierTimepass >= spawnSoldierTime)
        {
            // 졸개 소환 쿨타임을 초기화함
            spawnSoldierTimepass = 0f;
            // 졸개를 소환하는 기능의 함수를 실행
            SpawnSoldier();
        }

        // 보스의 남은 HP 가 80% 미만일 때
        if (finalBossHp < boss80Hp && finalBossPhase == 0)
        {
            // 중복 실행이 되지 않게 하기위해 bossPhase 값을 1 로 변경한다
            finalBossPhase = 1;
            // Boss80HpStart 함수 실행
            Boss80HpStart();
        }

        // 보스의 남은 HP 가 40% 미만일 때
        if (finalBossHp < boss40Hp && finalBossPhase == 2)
        {
            // 중복 실행이 되지 않게 하기위해 bossPhase 값을 3 으로 변경한다
            finalBossPhase = 3;
            // Boss40HpStart 함수 실행
            Boss40HpStart();
        }

        //// 졸개를 소환할 HP 단계가 충족되면
        //if (spawnSoldierStep <= 0)
        //{
        //    // 졸개를 소환할 HP 단계를 다시 초기화시킨다
        //    spawnSoldierStep = 10;
        //    // 졸개를 소환하는 기능의 함수를 실행한다
        //    SpawnSoldier();
        //}
    }     // Update()

    // 보스가 데미지를 받는 함수
    public void HitDamage(int damage)
    {
        // 최종 보스의 HP 를 받은 데미지만큼 감소
        finalBossHp -= damage;

        //// 졸개를 소환할 HP 단계를 데미지만큼 감소
        //spawnSoldierStep -= damage;
    }     // HitDamage()

    // 중간 보스가 사망 시 실행하는 함수
    public void DeathMidBoss()
    {
        // 최종 보스의 페이즈를 1 증가시킨다
        finalBossPhase += 1;
        // 최종 보스의 무적 상태를 해제한다
        bossImmotalForm = false;
    }     // DeathMidBoss()

    private IEnumerator MidBossTest()
    {
        yield return new WaitForSeconds(5f);

        midBossObj.gameObject.SetActive(true);
    }

    // 보스 HP 가 80% 미만일 때 실행되는 함수
    private void Boss80HpStart()
    {
        // 보스가 무적 상태가 되도록 한다
        bossImmotalForm = true;
        // 중간 보스 오브젝트를 활성화 시켜준다
        midBossObj.SetActive(true);
    }     // Boss80HpStart()

    // 보스 HP 가 40% 미만일 때 실행되는 함수
    private void Boss40HpStart()
    {
        // 보스가 무적 상태가 되도록 한다
        bossImmotalForm = true;
        // 중간 보스 오브젝트를 활성화 시켜준다
        midBossObj.SetActive(true);
    }     // Boss40HpStart()

    // 졸개를 소환하는 기능의 함수
    private void SpawnSoldier()
    {
        // Init : 졸개 소환 시 보스 애니메이션 실행

        //MonsterSpawn.instance.SetWave();
    }     // SpawnSoldier()
}
