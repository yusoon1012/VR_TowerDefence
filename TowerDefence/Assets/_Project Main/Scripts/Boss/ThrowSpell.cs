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
        playerTransform = GameObject.Find("Player").transform;
    }     // Awake()

    void Update()
    {
        transform.LookAt(playerTransform);
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, speed * 0.05f * Time.deltaTime);
    }     // Update()

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            /* Init : 플레이어에게 스펠이 닿으면 실행 */
        }
    }     // OnTriggerEnter()
}
