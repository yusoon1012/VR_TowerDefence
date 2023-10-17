using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ThrowSpell : MonoBehaviour
{
    // 보스의 날아가는 스펠의 속도
    public float speed = 3f;
    
    // 날아가는 스펠의 리짓바디
    private Rigidbody spellRigidbody = default;
    // 플레이어 트랜스폼
    private Transform playerTransform = default;

    void Awake()
    {
        // 날아가는 스펠의 리짓바디 값 참조
        spellRigidbody = GetComponent<Rigidbody>();
        // 플레이어의 트랜스폼 참조
        playerTransform = GameObject.Find("Player").transform;
    }     // Awake()

    void Update()
    {
        // 보스의 구체가 플레이어를 바라보게 한다
        transform.LookAt(playerTransform);
        // 보스의 구체가 플레이어의 위치로 서서히 다가가게 한다
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, speed * 0.05f * Time.deltaTime);
    }     // Update()

    // 보스의 구체가 플레이어 콜라이더에 닿으면
    private void OnTriggerEnter(Collider collision)
    {
        // 보스의 구체가 플레이어 콜라이더에 닿으면
        if (collision.tag == "Player")
        {
            /* Init : 플레이어에게 스펠이 닿으면 실행 */
        }
    }     // OnTriggerEnter()
}
