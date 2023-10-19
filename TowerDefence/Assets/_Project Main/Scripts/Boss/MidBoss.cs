using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MidBoss : MonoBehaviour
{
    // { 변수 설정
    // 중간 보스 등장 시 공중에서 내려오는 속도
    public float lerpSpeed = 2f;
    // 중간 보스의 스펠 발사 쿨타임
    public float throwSphereTime = default;
    // 중간 보스의 페이즈
    public int midBossPhase = default;
    // 중간 보스 HP 량
    public int midBossHp = default;

    // 중간 보스 등장 시 최종적으로 이동할 위치
    private Vector3 groundMidBossPosition = Vector3.zero;
    // 중간 보스의 초기 위치 값
    private Vector3 midBossOriginPosition = Vector3.zero;
    // 중간 보스의 스펠 발사 전 스펠의 초기 위치 값

    // 최종 보스 오브젝트
    private GameObject finalBossObj = default;
    // 중간 보스 오브젝트
    private GameObject midBossObj = default;
    // 중간 보스의 발사되는 스펠 오브젝트
    private GameObject fireBallShootObject = default;
    // 중간 보스의 애니메이터
    private Animator midBossAnimator = default;

    // 중간 보스가 내려오는 상황인지 체크
    private bool lerping = false;
    // 중간 보스가 활성화 된 상태인지 체크
    private bool activeBoss = false;
    // Test : 필드에 몬스터가 있는지 없는지 체크
    private bool isMonsterClear = false;
    // 구체를 날리는 중인지 확인 (Animator)
    private bool isAttack = false;
    // 졸개 강화를 위해 점프하는 중인지 확인 (Animator)
    private bool isJump = false;
    // 중간 보스의 스펠 발사 쿨타임 까지의 중첩 값
    private float throwSphereTimepass = default;
    // 보스가 2 페이즈 상태일 때 구체 던지기와 졸개 강화 중 랜덤 선택 값
    private int rand = default;
    // } 변수 설정

    void Awake()
    {
        // 중간 보스 등장 시 최종적으로 이동할 위치 값 지정
        groundMidBossPosition = new Vector3(0f, 0f, 165f);
        // 중간 보스의 초기 위치 값
        midBossOriginPosition = new Vector3(0f, 500f, 165f);
        throwSphereTimepass = 0f;
        rand = 0;
        // } 초기 변수 값 설정
    }     // Awake()

    void Start()
    {
        // { CSV 파일 정보 읽기
        // CSV 에서 중간 보스 HP 값을 불러옴
        int.TryParse(GameManager.instance.bossData["Hp"][0], out midBossHp);
        // CSV 에서 중간 보스 구체 발사 쿨타임 값을 불러옴
        float.TryParse(GameManager.instance.bossData["Attack_Cooltime"][0], out throwSphereTime);
        // } CSV 파일 정보 읽기

        // { 초기 변수 값 설정
        // 중간 보스 오브젝트 참조
        midBossObj = GetComponent<GameObject>();
        midBossAnimator = GetComponent<Animator>();
        // 최종 보스 오브젝트를 찾아서 참조
        finalBossObj = GameObject.Find("FinalBoss");
        fireBallShootObject = GameObject.Find("ObjectPooling").gameObject;

        // 초기 게임 세팅 후 중간 보스를 비활성화 하는 코루틴 실행
        StartCoroutine(SettingTime());
    }     // Start()

    void Update()
    {
        // 보스가 현재 비활성화 상태이면 아래 기능들을 스킵 한다
        if (activeBoss == false) { return; }

        // lerping 값이 true 일때만 실행
        if (lerping == true)
        {
            // 중간 보스의 y 위치값이 1 보다 작거나 같으면
            if (transform.position.y <= 1)
            {
                // lerping 값을 false 로 바꿔 더이상 내려오지 않게 설정
                lerping = false;
                // 중간 보스의 위치를 최종적인 위치값으로 고정 시켜준다
                transform.position = groundMidBossPosition;
            }
            // 중간 보스의 y 위치값이 0 보다 크면
            else
            {
                // 중간 보스의 y 위치값을 0 으로 lerp 값을 넣어 부드럽게 변경시킴
                transform.position = Vector3.Lerp(transform.position, groundMidBossPosition, lerpSpeed * Time.deltaTime);
            }
        }

        // 중간 보스 페이즈가 1 이고, 내려오는 상황이 아니면 실행
        if (midBossPhase == 1 && lerping == false)
        {
            // 실시간 값을 더해줌
            throwSphereTimepass += Time.deltaTime;
            // 쿨타임 시간과 같아지면
            if (throwSphereTimepass >= throwSphereTime)
            {
                // 실시간 값 초기화
                throwSphereTimepass = 0f;
                // 구체를 날리는 함수 실행
                ReadyThrowSphere();
            }
        }
        // 중간 보스 페이즈가 3 이고, 내려오는 상황이 아니면 실행
        else if (midBossPhase == 3 && lerping == false)
        {
            // 실시간 값을 더해줌
            throwSphereTimepass += Time.deltaTime;
            // 쿨타임 시간과 같아지면
            if (throwSphereTimepass >= throwSphereTime)
            {
                // 실시간 값 초기화
                throwSphereTimepass = 0f;
                // 구체를 날리거나, 졸개 능력치 증가 중 랜덤으로 실행되는 함수 실행
                ThrowSphereOrPowerUp();
            }
        }
    }     // Update()

    // 중간 보스 오브젝트가 활성화 되었을 때 실행되는 함수
    void OnEnable()
    {
        // 최종 보스의 페이즈 값을 참조하여 중간 보스의 페이즈 값을 지정함
        midBossPhase = finalBossObj.GetComponent<FinalBoss>().finalBossPhase;
        Debug.LogFormat("중간 보스의 시작 페이즈 : {0}", midBossPhase);
        // 중간 보스가 등장해 내려올 수 있도록 해준다
        lerping = true;
        // 중간 보스를 활성화 상태로 바꿔준다
        activeBoss = true;
        // 중간 보스의 Hp 를 초기화 시켜준다
        midBossHp = 50;
    }     // OnEnable()

    public void TestHitDamage()
    {
        Debug.LogFormat("중간 보스에게 1 공격. HP : {0}", midBossHp - 1);
        HitDamage(1);
    }

    // 보스가 데미지를 받는 함수
    public void HitDamage(int damage)
    {
        // 중간 보스가 비활성화 상태면 스킵
        if (activeBoss == false) { return; }

        // 중간 보스의 HP 를 받은 데미지만큼 감소
        midBossHp -= damage;

        // 중간 보스의 HP 가 0 이 되면
        if (midBossHp <= 0)
        {
            // 중간 보스의 사망 함수를 실행한다
            ReadyMidBossDeath();
        }
    }     // HitDamage()

    // 구체 발사를 준비하는 함수
    private void ReadyThrowSphere()
    {
        // 공격 애니메이션을 켜줌
        isAttack = true;
        // 보스 애니메이션 값 변경
        midBossAnimator.SetBool("Attack", isAttack);
    }     // ReadyThrowSphere()

    // 졸개 능력치 증가를 준비하는 함수
    private void ReadySoldierPowerUp()
    {
        // 점프 애니메이션을 켜줌
        isJump = true;
        // 보스 애니메이션 값 변경
        midBossAnimator.SetBool("Jump", isJump);
    }     // ReadySoldierPowerUp()

    // 중간 보스의 스펠 발사 기능 함수
    private void ThrowSphere()
    {
        // 공격 애니메이션을 끔
        isAttack = false;
        // 보스 애니메이션 값 변경
        midBossAnimator.SetBool("Attack", isAttack);
        // 중간 보스를 참조하여 구체 날리기 함수 실행
        fireBallShootObject.GetComponent<ObjectPooling>().ReadyLaunch(1);
    }     // ThrowSphere()

    // 졸개 능력치 증가 버프 실행 함수
    private void SoldierPowerUp()
    {
        // 점프 애니메이션을 끔
        isJump = false;
        // 보스 애니메이션 값 변경
        midBossAnimator.SetBool("Jump", isJump);
        /* Init : 소환 된 상태의 졸개들의 능력치 증가 버프 */
    }     // SoldierPowerUp()

    // 구체 날리기 Or 졸개 능력치 증가 버프 중 실행 구분 함수
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

    // 중간 보스 사망을 준비하는 함수
    private void ReadyMidBossDeath()
    {
        // 보스를 비활성화 상태로 변경
        activeBoss = false;
        // 보스 사망 애니메이션 실행
        midBossAnimator.SetTrigger("Death");
    }     // ReadyMidBossDeath()

    // 중간 보스가 죽으면 실행되는 함수
    private void MidBossDeath()
    {
        // 최종 보스에게 현재 페이즈 값을 넘겨준다
        finalBossObj.GetComponent<FinalBoss>().DeathMidBoss(midBossPhase);
        // 중간 보스의 위치를 초기 위치 값으로 이동시킨다
        transform.position = midBossOriginPosition;
        // 스펠 발사 쿨타임을 초기화 시킨다
        throwSphereTimepass = 0f;
        // 중간 보스 오브젝트를 비활성화 시켜준다
        this.gameObject.SetActive(false);
    }     // DeathBoss()

    // 중간 보스의 세팅 시간이 끝나고 다시 오브젝트를 비활성화 시키는 함수
    private IEnumerator SettingTime()
    {
        // 0.5 초 후에
        yield return new WaitForSeconds(0.5f);

        // 중간 보스 오브젝트를 비활성화 시켜준다
        this.gameObject.SetActive(false);
    }     // SettingTime()
}
