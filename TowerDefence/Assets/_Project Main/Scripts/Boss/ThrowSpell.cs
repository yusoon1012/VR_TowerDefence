using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThrowSpell : MonoBehaviour
{
    // 보스의 날아가는 스펠의 속도
    public float speed = 10f;
    // 보스 구체의 데미지 값
    public int power = default;

    public Transform initPosition;
    // 날아가는 스펠의 리짓바디
    private Rigidbody spellRigidbody = default;
    // 플레이어 트랜스폼
    [SerializeField]
    private Transform playerTransform = default;

    void Start()
    {
        // { CSV 파일 정보 읽기
        // CSV 에서 보스 투사체 데미지 값을 불러옴
        int.TryParse(GameManager.instance.bossData["Power"][0], out power);
        // } CSV 파일 정보 읽기

        // 날아가는 스펠의 리짓바디 값 참조
        spellRigidbody = GetComponent<Rigidbody>();
        // 플레이어의 트랜스폼 참조
        playerTransform = GameObject.Find("Player").transform;
        Vector3 dir = (playerTransform.position - initPosition.position).normalized;
        spellRigidbody.velocity = dir * speed;
    }     // Start()



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
            /* Init : 플레이어에게 스펠이 닿으면 실행 */
            spellRigidbody.velocity = Vector3.zero;
            Player_Status player = collision.gameObject.GetComponent<Player_Status>();
            if (player != null)
            {
                gameObject.SetActive(false);
                player.PlayerDamaged(5);
            }
        }
    }     // OnTriggerEnter()
}

