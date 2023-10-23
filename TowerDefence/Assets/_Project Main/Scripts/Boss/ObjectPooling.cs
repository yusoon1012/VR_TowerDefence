using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooling : MonoBehaviour
{
    #region 변수 설정

    // 구체 오브젝트 풀링 관리 오브젝트
    private GameObject poolingObject = default;
    // 보스가 발사하는 구체 오브젝트 배열
    /*
     * 0 : 파이어 구체
     * 1 : 라이트닝 구체
     * 2 : 포이즌 구체
     * 3 : 쉐도우 구체
     */
    public GameObject[] bossThrowSpell = new GameObject[4];
    // 최종 보스 오브젝트
    public GameObject finalBossObj;
    // 중간 보스 오브젝트
    public GameObject midBossObj;
    // 중간 보스의 구체 발사 초기 위치값
    public Transform midBossShootPosition;
    // 최종 보스의 구체 발사 초기 위치값
    public Transform finalBossShootPosition;
    // 구체 발사, 패링 안내문
    public Text parringInfo;

    // 구체 중복 발사를 방지하기 위해 이전에 발사한 구체 타입을 저장함
    private int beforeBallCheck = default;
    // 구체 발사 타입 랜덤 값
    private int rand = default;
    // 구체를 발사할 보스 종류 확인 (1 : 중간 보스, 2 : 최종 보스)
    private int bossType = default;

    private bool firstShootCheck = false;

    #endregion 변수 설정

    void Awake()
    {
        // { 초기 변수값 설정
        beforeBallCheck = 4;
        rand = 4;
        bossType = 0;
        // } 초기 변수값 설정
    }     // Awake()

    void Start()
    {
        // 구체 풀링 오브젝트 참조
        poolingObject = GetComponent<GameObject>().gameObject;

        // 초기 세팅을 위해 활성화 시켜놓은 오브젝트를 비활성화 시키는 함수를 실행한다
        StartCoroutine(SettingTime());
    }     // Start()

    // 초기 세팅을 위해 활성화 시켜놓은 오브젝트를 비활성화 시키는 함수
    private IEnumerator SettingTime()
    {
        // 0.5 초 후에
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 4; i++)
        {
            // 보스 구체 4 가지 타입들을 모두 비활성화 시킨다
            bossThrowSpell[i].gameObject.SetActive(false);
        }
    }     // SettingTime()

    // 보스 구체 발사 전 연산하는 함수
    public void ReadyLaunch(int bossType_)
    {
        // 이전에 발사한 구체가 존재할 경우
        if (beforeBallCheck != 4)
        {
            // 이전에 발사한 구체를 비활성화 시킨다
            bossThrowSpell[rand].gameObject.SetActive(false);
            bossThrowSpell[rand].transform.position = midBossShootPosition.position;
        }

        // 구체를 발사 할 보스 타입을 구분하여 저장한다
        bossType = bossType_;
        
        // 랜덤 값 중복 체크를 위해 while 문 실행
        while (true)
        {
            // 랜덤 값을 받는다
            SetRandom();

            // 받은 랜덤 값이 이전에 발사한 구체와 같으면
            if (rand == beforeBallCheck)
            {
                // 다시 처음으로 돌아가 랜덤 값을 다시 받는다
                continue;
            }
            // 받은 랜덤 값이 이전에 발사한 구체와 다른 타입이면
            else
            {
                // while 문을 나간다
                break;
            }
        }     // return : rand != beforeBallCheck

        // 발사 할 구체를 중복 체크 변수로 지정해준다
        beforeBallCheck = rand;
        // 구체를 발사하는 함수를 실행
        Launch();
    }     // ReadyLaunch()

    // 랜덤 값을 지정받는 함수
    private void SetRandom()
    {
        // 0 ~ 3 까지의 랜덤 값을 지정받는다
        rand = Random.Range(0, 4);
    }     // SetRandom()

    // 구체를 발사하는 함수
    private void Launch()
    {
        // 보스가 처음 구체를 발사하면
        if (firstShootCheck == false)
        {
            // 처음 출력 이후에 더이상 출력이 안되게 한다
            firstShootCheck = true;
            // 구체 발사와 패링 안내문을 출력한다
            parringInfo.gameObject.SetActive(true);
            // 구체 발사와 패링 안내문 종료 전 딜레이
            StartCoroutine(FirstShootCheckDelay());
        }

        // 보스 타입이 1 이면 중간 보스에서 구체를 발사
        if (bossType == 1)
        {
            // 랜덤 값으로 지정받은 구체 타입을 활성화
            bossThrowSpell[rand].transform.position = midBossShootPosition.position;
            Rigidbody rb = bossThrowSpell[rand].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            bossThrowSpell[rand].gameObject.SetActive(true);
            // 활성화 된 구체를 보스에서 발사 할 지역으로 이동시킨다
        }
        // 보스 타입이 2 면 최종 보스에서 구체를 발사
        else if (bossType == 2)
        {
            // 랜덤 값으로 지정받은 구체 타입을 활성화
            bossThrowSpell[rand].transform.position = finalBossShootPosition.position;
            Rigidbody rb = bossThrowSpell[rand].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            bossThrowSpell[rand].gameObject.SetActive(true);
            // 활성화 된 구체를 보스에서 발사 할 지역으로 이동시킨다.
        }

        // 보스 발사체가 날아갈 때 발사체 타입 정보를 보내 enum 타입을 결정하는 함수로 보낸다
        bossThrowSpell[rand].GetComponent<ThrowSpell>().MissileTypeCheck(rand);
    }     // Launch()

    // 딜레이 이후 구체 발사와 패링 안내문을 끈다
    private IEnumerator FirstShootCheckDelay()
    {
        yield return new WaitForSeconds(5f);

        parringInfo.gameObject.SetActive(false);
    }
}