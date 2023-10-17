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
    public float throwSphereTime = 20f;
    // 중간 보스의 페이즈
    public int midBossPhase = default;
    // 중간 보스 HP 량
    public int midBossHp = 50;

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
    public GameObject fireBall = default;

    // 중간 보스가 내려오는 상황인지 체크
    private bool lerping = false;
    // 중간 보스가 활성화 된 상태인지 체크
    private bool activeBoss = false;
    // Test : 필드에 몬스터가 있는지 없는지 체크
    private bool isMonsterClear = false;
    // 중간 보스의 스펠 발사 쿨타임 까지의 중첩 값
    private float throwSphereTimepass = default;
    // 보스가 2 페이즈 상태일 때 구체 던지기와 졸개 강화 중 랜덤 선택 값
    private int rand = default;
    private Rigidbody rb;
    // } 변수 설정

    void Awake()
    {
        // { 초기 변수 값 설정
        // 중간 보스 오브젝트 참조
        midBossObj = GetComponent<GameObject>();
        // 최종 보스 오브젝트를 찾아서 참조
        finalBossObj = GameObject.Find("FinalBoss");
       
        // 중간 보스 등장 시 최종적으로 이동할 위치 값 지정
        groundMidBossPosition = new Vector3(720f,5f , 265f);
        // 중간 보스의 초기 위치 값
        midBossOriginPosition = new Vector3(720f, 500f, 265f);
        bossFireBallShootPosition = new Vector3(720f, 50f, 265f);
        throwSphereTimepass = 0f;
        rand = 0;
        rb=GetComponent<Rigidbody>();
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
            // 중간 보스의 y 위치값이 1 보다 작거나 같으면
            if (transform.position.y <= 5)
            {
                // lerping 값을 false 로 바꿔 더이상 내려오지 않게 설정
                lerping = false;
                rb.isKinematic = true;
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

        if (midBossPhase == 1 && lerping == false)
        {
            throwSphereTimepass += Time.deltaTime;
            if (throwSphereTimepass >= throwSphereTime)
            {
                fireBall.SetActive(false);
                throwSphereTimepass = 0f;
                ThrowSphere();
            }
        }
        else if (midBossPhase == 3 && lerping == false)
        {
            throwSphereTimepass += Time.deltaTime;
            if (throwSphereTimepass >= throwSphereTime)
            {
                throwSphereTimepass = 0f;
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

    // 보스가 데미지를 받는 함수
    public void HitDamage(int damage)
    {
        // 중간 보스의 HP 를 받은 데미지만큼 감소
        midBossHp -= damage;

        // 중간 보스의 HP 가 0 이 되면
        if (midBossHp <= 0)
        {
            // 중간 보스의 사망 함수를 실행한다
            MidBossDeath();
        }

        // 졸개를 소환할 HP 단계를 데미지만큼 감소
        //spawnSoldierStep -= damage;

    }     // HitDamage()

    // 중간 보스의 스펠 발사 기능 함수
    private void ThrowSphere()
    {
        // 발사 스펠을 활성화 시킴
        fireBall.transform.position = bossFireBallShootPosition;
        Rigidbody fireBallRigid=fireBall.gameObject.GetComponent<Rigidbody>();
        if (fireBallRigid != null)
        {
            fireBallRigid.velocity = Vector3.zero;
        }
        fireBall.gameObject.SetActive(true);
        // 발사 스펠의 위치를 발사 전 위치로 초기화 시킴
    }     // ThrowSphere()

    // 졸개 능력치 증가 버프 실행 함수
    private void SoldierPowerUp()
    {
        /* Init : 소환 된 상태의 졸개들의 능력치 증가 버프 */
    }     // SoldierPowerUp()

    // 구체 날리기 Or 졸개 능력치 증가 버프 중 실행 구분 함수
    private void ThrowSphereOrPowerUp()
    {
        // 현재 필드에 졸개들이 없다면
        if (isMonsterClear == true)
        {
            // 구체 날리기 함수 실행
            ThrowSphere();
        }
        // 현재 필드에 졸개들이 남아 있다면
        else
        {
            // 0, 1 랜덤 값 생성
            rand = Random.Range(0, 100);
            // 랜덤 값이 0 이면
            if (rand < 60)
            {
                // 구체 날리기 함수 실행
                ThrowSphere();
            }
            // 랜덤 값이 1 이면
            else if (rand >= 60)
            {
                // 졸개 능력치 증가 버프 함수 실행
                SoldierPowerUp();
            }

            Debug.LogFormat("랜덤값 생성됨 : {0}", rand);
        }
    }     // ThrowSphereOrPowerUp()

    // 중간 보스가 죽으면 실행되는 함수
    private void MidBossDeath()
    {
        // 최종 보스에게 현재 페이즈 값을 넘겨준다
        finalBossObj.GetComponent<FinalBoss>().DeathMidBoss(midBossPhase);
        // 중간 보스의 위치를 초기 위치 값으로 이동시킨다
        transform.position = midBossOriginPosition;
        // 중간 보스를 비활성화 시킨다
        activeBoss = false;
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

        // 보스가 날리는 구체 오브젝트를 비활성화 시켜준다
        fireBall.gameObject.SetActive(false);
        // 중간 보스 오브젝트를 비활성화 시켜준다
        this.gameObject.SetActive(false);
    }     // SettingTime()
}
