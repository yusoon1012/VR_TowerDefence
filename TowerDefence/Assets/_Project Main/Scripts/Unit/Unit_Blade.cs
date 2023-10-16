using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Blade : MonoBehaviour
{
    // 칼날
    private GameObject blade = default;
    // 칼날 회전 여부
    private bool rotateBlade = false;

    private void Awake()
    {
        blade = transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// 졸개를 감지하는 메서드
    /// </summary>
    private Collider[] EnemyGotcha
    {
        get
        {
            float radius = 5f; // 졸개 탐지 반경
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy"); // 졸개 레이어마스크
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyLayer); // 졸개 검출

            return colliders;
        }
    }

    private void Update()
    {
        if (EnemyGotcha.Length > 0) // 반경 내 졸개 하나 이상 검출
        {
            Debug.Log("적 검출!");
            rotateBlade = true;
        }
        else rotateBlade = false; // 아니면 정지
    }

    private void FixedUpdate()
    {
        if (rotateBlade)
        {
            float rotateValue = 500f; // 칼날 회전값
            blade.transform.RotateAround(transform.position, Vector3.up, rotateValue * Time.deltaTime); // 회전
        }
        else blade.transform.rotation = blade.transform.rotation; // 정지
    }
}
