using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.GraphicsBuffer;

public class MidBoss : MonoBehaviour
{
    #region 변수 설정

    // { 변수 설정
    // 중간 보스 등장 시 공중에서 내려오는 속도
    public float lerpSpeed = 2f;
    // 중간 보스의 스펠 발사 쿨타임
    public float throwSphereTime = default;
    // 중간 보스의 페이즈
    public int midBossPhase = default;
    // 중간 보스 HP 량
    public int midBossHp = default;

    public Transform midGroundposition = default;

    public Transform fireBallSpawnPosition;
    // 중간 보스 등장 시 최종적으로 이동할 위치
    private Vector3 groundMidBossPosition = Vector3.zero;
    // 중간 보스의 초기 위치 값
    private Vector3 midBossOriginPosition = Vector3.zero;
    // 중간 보스의 스펠 발사 전 스펠의 초기 위치 값

    // 최종 보스 오브젝트
    public GameObject finalBossObj = default;
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
    // 구체를 날리는 중인지 확인 (Animator)
    private bool isAttack = false;
    // 중간 보스의 스펠 발사 쿨타임 까지의 중첩 값
    private float throwSphereTimepass = default;

    private Rigidbody rb;
    // } 변수 설정

    #endregion 변수 설정

    void Awake()
    {
        // 중간 보스 등장 시 최종적으로 이동할 위치 값 지정
        groundMidBossPosition = new Vector3(720f,5f , 265f);
        // 중간 보스의 초기 위치 값
        midBossOriginPosition = new Vector3(0f, 500f, 165f);
        throwSphereTimepass = 0f;
        //rand = 0;
        rb=GetComponent<Rigidbody>();
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
        // 게임 오버 상태면 중간 보스 모든 기능을 정지시킨다
        if (GameManager.instance.isGameOver == true) { return; }
        // 보스가 현재 비활성화 상태이면 아래 기능들을 스킵 한다
        if (activeBoss == false) { return; }

        // lerping 값이 true 일때만 실행
        if (lerping == true)
        {
            // 중간 보스의 y 위치값이 1 보다 작거나 같으면
            if (transform.position.y <= 5)
            {
                // lerping 값을 false 로 바꿔 더이상 내려오지 않게 설정
                lerping = false;
                rb.isKinematic = true;
                // 중간 보스의 위치를 최종적인 위치값으로 고정 시켜준다
                transform.position = midGroundposition.position;
            }
            // 중간 보스의 y 위치값이 0 보다 크면
            else
            {
                // 중간 보스의 y 위치값을 0 으로 lerp 값을 넣어 부드럽게 변경시킴
                transform.position = Vector3.Lerp(transform.position, midGroundposition.position, lerpSpeed * Time.deltaTime);
            }
        }

        // 중간 보스 페이즈가 1 이고, 내려오는 상황이 아니면 실행
        if (midBossPhase >= 1 && lerping == false)
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
        int.TryParse(GameManager.instance.bossData["Hp"][0], out midBossHp);
    }     // OnEnable()

    public void TestHitDamage()
    {
        Debug.LogFormat("중간 보스에게 1 공격. HP : {0}", midBossHp - 1);
        HitDamage(1);
    }

    // 보스가 데미지를 받는 함수
    public void HitDamage(int damage)
    {
        // 게임 오버 상태면 데미지를 받지 않음
        if (GameManager.instance.isGameOver == true) { return; }
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

    // 중간 보스의 스펠 발사 기능 함수
    private void ThrowSphere()
    {
        // 공격 애니메이션을 끔
        isAttack = false;
        // 보스 애니메이션 값 변경
        midBossAnimator.SetBool("Attack", isAttack);
        // 중간 보스를 참조하여 구체 날리기 함수 실행
        fireBallShootObject.GetComponent<ObjectPooling>().ReadyLaunch(1);
    }

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
