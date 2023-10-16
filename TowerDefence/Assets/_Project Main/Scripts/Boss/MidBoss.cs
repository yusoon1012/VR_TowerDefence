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
    public float throwSphereTime = 5f;
    // 중간 보스의 페이즈
    public int midBossPhase = default;
    // 중간 보스 HP 량
    public int midBossHp = 50;

    //// 졸개를 소환할 HP 단계
    //public int spawnSoldierStep = 10;

    // 중간 보스 등장 시 최종적으로 이동할 위치
    private Vector3 groundMidBossPosition = Vector3.zero;
    // 중간 보스의 초기 위치 값
    private Vector3 midBossOriginPosition = Vector3.zero;
    // 중간 보스의 스펠 발사 전 스펠의 초기 위치 값
    private Vector3 bossFireBallShootPosition = Vector3.zero;

    // 최종 보스 오브젝트
    private GameObject finalBossObj = default;
    // 중간 보스 오브젝트
    private GameObject midBossObj = default;
    // 중간 보스의 발사되는 스펠 오브젝트
    private GameObject fireBall = default;

    // 중간 보스가 내려오는 상황인지 체크
    [SerializeField] private bool lerping = false;
    // 중간 보스가 활성화 된 상태인지 체크
    [SerializeField] private bool activeBoss = false;
    // 중간 보스의 스펠 발사 쿨타임 까지의 중첩 값
    [SerializeField] private bool testBool = false;
    private float throwSphereTimepass = default;
    // } 변수 설정

    void Awake()
    {
        // { 초기 변수 값 설정
        // 중간 보스 오브젝트 참조
        midBossObj = GetComponent<GameObject>();
        // 최종 보스 오브젝트를 찾아서 참조
        finalBossObj = GameObject.Find("FinalBoss");
        fireBall = GameObject.Find("BossFireBall");
        // 중간 보스 등장 시 최종적으로 이동할 위치 값 지정
        groundMidBossPosition = new Vector3(0f, 0f, 81.2f);
        // 중간 보스의 초기 위치 값
        midBossOriginPosition = new Vector3(0f, 500f, 81.2f);
        bossFireBallShootPosition = new Vector3(0f, 50f, 40f);
        throwSphereTimepass = 0f;
        // } 초기 변수 값 설정
    }     // Awake()

    void Start()
    {
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
            // 중간 보스의 y 위치값이 0 보다 작거나 같으면
            if (transform.position.y <= 1)
            {
                // lerping 값을 false 로 바꿔 더이상 내려오지 않게 설정
                lerping = false;
                testBool = true;
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

        //if (midBossPhase == 1 && lerping == false)
        //{
        //    throwSphereTimepass += Time.deltaTime;
        //    if (throwSphereTimepass >= throwSphereTime)
        //    {
        //        throwSphereTimepass = 0f;
        //        ThrowSphere();
        //    }
        //}

        if (testBool == true)
        {
            throwSphereTimepass += Time.deltaTime;
            Debug.LogFormat("스펠 쿨타임 진행 : {0}", throwSphereTimepass);
            if (throwSphereTimepass >= throwSphereTime)
            {
                throwSphereTimepass = 0f;
                ThrowSphere();
            }
        }
        else if (midBossPhase == 3 && lerping == false)
        {

        }
    }     // Update()

    // 중간 보스 오브젝트가 활성화 되었을 때 실행되는 함수
    void OnEnable()
    {
        // 최종 보스의 페이즈 값을 참조하여 중간 보스의 페이즈 값을 지정함
        midBossPhase = finalBossObj.GetComponent<FinalBoss>().finalBossPhase;
        Debug.Log(midBossPhase);
        // 중간 보스가 등장해 내려올 수 있도록 해준다
        lerping = true;
        // 중간 보스를 활성화 상태로 바꿔준다
        activeBoss = true;
        // 중간 보스의 Hp 를 초기화 시켜준다
        midBossHp = 50;
    }     // OnEnable()

    // 보스가 데미지를 받는 함수
    public void HitDamage(int damage)
    {
        // 중간 보스의 HP 를 받은 데미지만큼 감소
        midBossHp -= damage;

        // 중간 보스의 HP 가 0 이 되면
        if (midBossHp <= 0)
        {
            // 중간 보스의 사망 함수를 실행한다
            DeathBoss();
        }

        // 졸개를 소환할 HP 단계를 데미지만큼 감소
        //spawnSoldierStep -= damage;

    }     // HitDamage()

    // 중간 보스의 스펠 발사 기능 함수
    private void ThrowSphere()
    {
        Debug.Log("스펠 함수에 진입함");
        // 발사 스펠을 활성화 시킴
        fireBall.gameObject.SetActive(true);
        // 발사 스펠의 위치를 발사 전 위치로 초기화 시킴
        fireBall.transform.position = bossFireBallShootPosition;
    }     // ThrowSphere()

    // 중간 보스가 죽으면 실행되는 함수
    private void DeathBoss()
    {
        // 중간 보스를 비활성화 시킨다
        activeBoss = false;
        // 스펠 발사 쿨타임을 초기화 시킨다
        throwSphereTimepass = 0f;
        // 중간 보스 오브젝트를 비활성화 시켜준다
        this.gameObject.SetActive(false);
        // 중간 보스의 위치를 초기 위치 값으로 이동시킨다
        transform.position = midBossOriginPosition;
        // 최종 보스에게 현재 페이즈 값을 넘겨준다
        finalBossObj.GetComponent<FinalBoss>().DeathMidBoss();
    }     // DeathBoss()

    // 중간 보스의 세팅 시간이 끝나고 다시 오브젝트를 비활성화 시키는 함수
    private IEnumerator SettingTime()
    {
        // 0.5 초 후에
        yield return new WaitForSeconds(0.5f);

        fireBall.gameObject.SetActive(false);
        // 중간 보스 오브젝트를 비활성화 시켜준다
        this.gameObject.SetActive(false);
    }     // SettingTime()
}
