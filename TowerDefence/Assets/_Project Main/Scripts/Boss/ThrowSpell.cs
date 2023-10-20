using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThrowSpell : MonoBehaviour
{
    // 보스의 날아가는 스펠의 속도
    public float speed = 10f;
    // 보스 구체의 데미지 값
    public int power = default;

    public Vector3 initPosition;
    // 날아가는 스펠의 리짓바디
    private Rigidbody spellRigidbody = default;
    // 플레이어 트랜스폼
    [SerializeField]
    private Transform playerTransform = default;
    // 발사체가 날아가는 방향이 어디쪽인지 확인
    public bool shootFlip;

    void Start()
    {
        // { CSV 파일 정보 읽기
        // CSV 에서 보스 투사체 데미지 값을 불러옴
        int.TryParse(GameManager.instance.bossData["Power"][0], out power);
        // } CSV 파일 정보 읽기

        // 날아가는 스펠의 리짓바디 값 참조
        spellRigidbody = GetComponent<Rigidbody>();
        // 플레이어의 트랜스폼 참조
        
        //initPosition = transform.position;
        //Vector3 dir = (playerTransform.position - initPosition).normalized;
        //spellRigidbody.velocity = dir * speed;
    }     // Start()
    private void OnEnable()
    {
        spellRigidbody = GetComponent<Rigidbody>();

        initPosition = transform.position;
        Vector3 dir = (playerTransform.position - initPosition).normalized;
        spellRigidbody.velocity = dir * speed;
    }


    void Update()
    {
        //// 보스의 구체가 플레이어를 바라보게 한다
        //transform.LookAt(playerTransform);
        //// 보스의 구체가 플레이어의 위치로 서서히 다가가게 한다
        //transform.position = Vector3.Lerp(transform.position, playerTransform.position, speed * 0.05f * Time.deltaTime);
    }     // Update()

    // 보스의 구체가 플레이어 콜라이더에 닿으면
    private void OnTriggerEnter(Collider collision)
    {
        // 보스의 구체가 플레이어 콜라이더에 닿으면
        if (collision.tag == "Player")
        {
            spellRigidbody.velocity = Vector3.zero;
            Player_Status player = collision.gameObject.GetComponent<Player_Status>();
            if (player != null)
            {
                gameObject.SetActive(false);
                player.PlayerDamaged(5);
            }
        }
        // 콜라이더에 부딛힌 오브젝트 태그가 MidBoss 이고, 반대방향으로 날아가고 있으면 실행
        else if (collision.tag == "MidBoss" && shootFlip == true)
        {
            // 발사체의 운동량을 초기화
            spellRigidbody.velocity = Vector3.zero;
            // 콜라이더에 부딛힌 오브젝트의 MidBoss 스크립트 참조
            MidBoss midBoss = collision.gameObject.GetComponent<MidBoss>();
            // midBoss 오브젝트가 null 값이 아니면 실행
            if (midBoss != null)
            {
                // 발사체 오브젝트를 비활성화 시킨다
                gameObject.SetActive(false);
                // 중간 보스에게 데미지를 입힌다
                midBoss.HitDamage(10);
            }
        }
        // 콜라이더에 부딛힌 오브젝트 태그가 Boss 이고, 반대방향으로 날아가고 있으면 실행
        else if (collision.tag == "Boss" && shootFlip == true)
        {
            // 발사체의 운동량을 초기화
            spellRigidbody.velocity = Vector3.zero;
            // 콜라이더에 부딛힌 오브젝트의 FinalBoss 스크립트 참조
            FinalBoss finalBoss = collision.gameObject.GetComponent<FinalBoss>();
            // finalBoss 오브젝트가 null 값이 아니면 실행
            if (finalBoss != null)
            {
                // 발사체 오브젝트를 비활성화 시킨다
                gameObject.SetActive(false);
                // 최종 보스에게 데미지를 입힌다
                finalBoss.HitDamage(10);
            }
        }
    }     // OnTriggerEnter()

    // 발사체가 날아가는 방향을 반대로 바꿔주는 함수
    public void Reverse()
    {
        // 반대 방향으로 체크
        shootFlip = true;
    }     // Reverse()
}

