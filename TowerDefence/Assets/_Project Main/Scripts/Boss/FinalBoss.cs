using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FinalBoss : MonoBehaviour
{

    public Slider bossHp;
    // { 변수 설정
    // 보스 HP 량
    public float finalBossHp;
    private float finalBossMaxhp = 100;
    // 보스 페이즈
    public int finalBossPhase = default;
    // 졸개 소환 쿨타임
    public float spawnSoldierTime = default;
    // 스펠 발사 쿨타임
    public float throwSphereTime = default;
    // 보스 무적 상태 체크
    public bool bossImmotalForm = false;

    // 보스 게임 오브젝트
    private GameObject finalBossObj = default;
    // 중간 보스 게임 오브젝트
    private GameObject midBossObj = default;
    // 발사 스펠의 오브젝트
    private GameObject fireBallShootObject = default;
    // 최종 보스의 애니메이터
    private Animator finalBossAnimator = default;

    // 보스가 패턴을 실행하기까지의 시간값
    private float actionTime = default;
    // 보스가 패턴을 실행하기까지 걸리는 시간 최대값
    private float actionMaxTime = default;
    // 보스 HP 가 80% 남을 때 수치 체크
    private float boss80Hp = default;
    // 보스 HP 가 40% 남을 때 수치 체크
    private float boss40Hp = default;
    // 졸개 소환 쿨타임 까지의 중첩 값
    private float spawnSoldierTimepass = default;
    // 스펠 발사 쿨타임 까지의 중첩 값
    private float throwSphereTimepass = default;
    // 최종 보스가 구체 발사를 위해 날고 있는지 동작 확인 (Animator)
    private bool isFly = false;
    // 최종 보스가 졸개 능력치 업 스펠을 시전중인지 동작 확인 (Animator)
    private bool isRoar = false;
    // 최종 보스가 졸개 소환을 시전중인지 동작 확인 (Animator)
    private int isSpawn = default;
    // Test : 필드에 졸개들이 존재하는지 없는지 체크
    private bool isMonsterClear = false;
    // 마지막 페이즈에서 구체 발사 or 졸개 능력치 강화 랜덤 시전 값
    private int rand = default;
    // 졸개 소환 애니메이션중 랜덤 재생 값
    private int spawnRand = default;
    // } 변수 설정

    void Awake()
    {
        // { 초기 변수값 설정
        // 보스 오브젝트 참조
        finalBossHp = finalBossMaxhp;
        finalBossObj = GetComponent<GameObject>();
        finalBossAnimator = GetComponent<Animator>();
        // 중간 보스의 오브젝트를 찾아서 참조
        midBossObj = GameObject.Find("MidBoss");
       

        actionTime = 0f;
        actionMaxTime = 5f;
        boss80Hp = 0;
        boss40Hp = 0;
        finalBossPhase = 0;
        spawnSoldierTimepass = 0f;
        throwSphereTimepass = 0f;
        rand = 0;
        spawnRand = 0;
        isSpawn = 0;
        bossHp.value = finalBossHp / 100;
        // } 초기 변수값 설정
    }     // Awake()

    void Start()
    {
        // { CSV 파일 정보 읽기
        // CSV 에서 최종 보스 HP 값을 불러옴
        float.TryParse(GameManager.instance.bossData["Hp"][1], out finalBossHp);
        // CSV 에서 최종 보스 구체 발사 쿨타임 값을 불러옴
        float.TryParse(GameManager.instance.bossData["Attack_Cooltime"][1], out throwSphereTime);
        // CSV 에서 최종 보스의 졸개 소환 쿨타임 값을 불러옴
        float.TryParse(GameManager.instance.bossData["Spawn_Cooltime"][1], out spawnSoldierTime);
        // } CSV 파일 정보 읽기

        // { 초기 변수값 설정
        // 보스 오브젝트 참조
        finalBossObj = GetComponent<GameObject>();
        finalBossAnimator = GetComponent<Animator>();
        // 중간 보스의 오브젝트를 찾아서 참조
        midBossObj = GameObject.Find("MidBoss");
        fireBallShootObject = GameObject.Find("ObjectPooling").gameObject;

        // 보스 HP 에서 80% 수치 값 저장
        boss80Hp = finalBossHp * 0.8f;
        // 보스 HP 에서 40% 수치 값 저장
        boss40Hp = finalBossHp * 0.4f;
    }     // Start()

    void Update()
    {
        // 실시간으로 졸개 소환 쿨타임을 진행함
        spawnSoldierTimepass += Time.deltaTime;
        // 졸개 소환 쿨타임이 완료되고 최종 보스가 구체를 날리고 있지 않으면
        if (spawnSoldierTimepass >= spawnSoldierTime && isFly == false && isRoar == false)
        {
            Debug.Log("졸개 소환 완료");
            // 졸개 소환 쿨타임을 초기화함
            spawnSoldierTimepass = 0f;
            // 졸개를 소환하는 기능을 준비하는 함수를 실행
            ReadySpawnSoldier();
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

        // 중간 보스 처치 후 보스 페이즈가 2 라면 중간 보스의 구체 날리기를 사용함
        if (finalBossPhase == 2 && bossImmotalForm == false && isSpawn == 0)
        {
            throwSphereTimepass += Time.deltaTime;
            if (throwSphereTimepass >= throwSphereTime)
            {
                throwSphereTimepass = 0f;
                ReadyThrowSphere();
            }
        }
        // 중간 보스 처치 후 보스 페이즈가 4 라면 중간 보스의 구체 날리기 or 졸개 능력치 증가 둘 중에 하나를 사용함
        else if (finalBossPhase == 4 && bossImmotalForm == false && isSpawn == 0)
        {
            throwSphereTimepass += Time.deltaTime;
            if (throwSphereTimepass >= throwSphereTime)
            {
                throwSphereTimepass = 0f;
                ThrowSphereOrPowerUp();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (bossImmotalForm == false)
            {
                Debug.LogFormat("최종 보스에게 1 공격. HP : {0}", finalBossHp - 1);
                HitDamage(1);
            }
            else
            {
                midBossObj.GetComponent<MidBoss>().TestHitDamage();
            }
        }
    }     // Update()

    // 보스가 데미지를 받는 함수
    public void HitDamage(int damage)
    {
        // 보스가 무적 상태이면 데미지를 받지 않음
        if (bossImmotalForm == true) { return; }

        // 최종 보스의 HP 를 받은 데미지만큼 감소
        finalBossHp -= damage;
        bossHp.value = finalBossHp / 100;

        if (finalBossHp <= 0)
        {
            // 보스 사망 함수 실행
            ReadyFinalBossDeath();
        }
    }     // HitDamage()

    // 중간 보스가 사망 시 실행하는 함수
    public void DeathMidBoss(int bossPhase)
    {
        // 최종 보스의 페이즈를 1 증가시킨다
        finalBossPhase = bossPhase + 1;
        Debug.LogFormat("최종 보스의 현재 페이즈 : {0}", finalBossPhase);
        // 최종 보스의 무적 상태를 해제한다
        bossImmotalForm = false;
    }     // DeathMidBoss()

    // 구체를 날리거나 졸개들의 능력치를 올려주는 능력 중 선택하는 함수
    private void ThrowSphereOrPowerUp()
    {
        // 현재 필드에 졸개들이 없다면
        if (isMonsterClear == true)
        {
            // 구체 날리기 함수 실행
            ReadyThrowSphere();
        }
        // 현재 필드에 졸개들이 남아 있다면
        else
        {
            // 0 ~ 99 랜덤 값 생성
            rand = Random.Range(0, 100);
            // 랜덤 값이 60 보다 작으면
            if (rand < 60)
            {
                // 구체 날리기 함수 실행
                ReadyThrowSphere();
            }
            // 랜덤 값이 60 보다 크거나 같으면
            else if (rand >= 60)
            {
                // 졸개 능력치 증가 버프 함수 실행
                ReadySoldierPowerUp();
            }

            Debug.LogFormat("랜덤값 생성됨 : {0}", rand);
        }
    }     // ThrowSphereOrPowerUp()

    // 졸개 능력치 상승 버프를 준비하는 함수
    private void ReadySoldierPowerUp()
    {
        // 함성을 지르는 애니메이션을 켠다
        isRoar = true;
        // 보스 애니메이션 값 변경
        finalBossAnimator.SetBool("PowerUp", isRoar);
    }     // ReadySoldierPowerUp()

    // 졸개 능력치 상승 버프를 실행하는 함수
    private void SoldierPowerUp()
    {
        // 함성을 지르는 애니메이션을 끈다
        isRoar = false;
        // 보스 애니메이션 값 변경
        finalBossAnimator.SetBool("PowerUp", isRoar);

        /* Init : 소환 된 상태의 졸개들의 능력치 증가 버프 */

    }     // SoldierPowerUp()

    // 구체를 날리기 전 준비하는 함수
    private void ReadyThrowSphere()
    {
        // 최종 보스가 날고 있는 상태로 변경한다
        isFly = true;
        // 최종 보스의 날아 오르는 애니메이션을 실행시킨다
        finalBossAnimator.SetBool("Fly", isFly);
    }     // ReadyThrowSphere()

    // 애니메이션 진행 이후에 구체를 날리는 함수
    private void ThrowSphere()
    {
        // 최종 보스가 날고 있지 않는 상태로 변경한다
        isFly = false;
        // 최종 보스의 날아 오르는 애니메이션을 중지시킨다
        finalBossAnimator.SetBool("Fly", isFly);
        // 최종 보스로 참조하여 구체 날리는 함수를 실행
        fireBallShootObject.GetComponent<ObjectPooling>().ReadyLaunch(2);
    }     // ThrowSphere()

    // 보스 HP 가 80% 미만일 때 실행되는 함수
    private void Boss80HpStart()
    {
        // 최종 보스의 페이즈 값을 변경
        finalBossPhase = 1;
        // 보스가 무적 상태가 되도록 한다
        bossImmotalForm = true;
        // 중간 보스 오브젝트를 활성화 시켜준다
        midBossObj.SetActive(true);
    }     // Boss80HpStart()

    // 보스 HP 가 40% 미만일 때 실행되는 함수
    private void Boss40HpStart()
    {
        // 최종 보스의 페이즈 값을 변경
        finalBossPhase = 3;
        // 보스가 무적 상태가 되도록 한다
        bossImmotalForm = true;
        // 중간 보스 오브젝트를 활성화 시켜준다
        midBossObj.SetActive(true);
    }     // Boss40HpStart()

    // 보스 사망을 준비하는 함수
    private void ReadyFinalBossDeath()
    {
        // 중복 실행을 방지하기 위해서 보스를 무적상태로 만듬
        bossImmotalForm = true;
        // 보스 사망 애니메이션 실행
        finalBossAnimator.SetTrigger("Death");
    }     // ReadyFinalBossDeath()

    // 보스가 사망하는 함수
    private void FinalBossDeath()
    {
        // 보스의 오브젝트를 비활성화 한다
        this.gameObject.SetActive(false);
        Debug.Log("최종 보스 사망");

        /* Add : 보스 사망 후 게임 종료 */

    }     // FinalBossDeath()

    // 졸개 소환을 준비하는 함수 (애니메이션 실행)
    private void ReadySpawnSoldier()
    {
        // 1 ~ 2 랜덤 값 생성
        spawnRand = Random.Range(1, 3);
        // 랜덤 값을 애니메이터 값으로 지정
        isSpawn = spawnRand;
        // 애니메이터 값을 최신화
        finalBossAnimator.SetInteger("Spawn", isSpawn);
    }     // SpawnSoldier()

    // 졸개를 소환하는 기능의 함수
    private void SpawnSoldier()
    {
        // 졸개 소환 애니메이션을 종료함
        isSpawn = 0;
        // 보스 졸개 소환 애니메이션 값 변경
        finalBossAnimator.SetInteger("Spawn", isSpawn);
        // 졸개가 소환되는 함수 실행
        MonsterSpawn.instance.SetWave();
    }     // SpawnSoldier()
}
